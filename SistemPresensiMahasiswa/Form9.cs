using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting; // Library pendukung komponen Chart

namespace SistemPresensiMahasiswa
{
    public partial class RekapPresensi : Form
    {
        // MENGGUNAKAN DAL: Memakai class Connection_DAL_ sebagai pusat kendali query database
        private Connection_DAL_ db = new Connection_DAL_();

        public RekapPresensi()
        {
            InitializeComponent();
        }

        // REVISI 1: Menyelaraskan nama event load dengan nama Form (RekapPresensi_Load)
        private void RekapPresensi_Load(object sender, EventArgs e)
        {
            LoadMatakuliah(); // Mengisi cbMatakuliah dengan daftar pelajaran
            dtpAwal.Value = DateTime.Today.AddMonths(-1); // Default 1 bulan terakhir aman (tanpa komponen jam)
            dtpAkhir.Value = DateTime.Today;

            // Inisialisasi awal tampilan chart agar bersih
            if (chartPresensi.Series.Count > 0) chartPresensi.Series.Clear();
        }

        private void LoadMatakuliah()
        {
            try
            {
                DataTable dt = db.ExecuteStoredProcedure("sp_GetMatakuliah", null);

                // Tambahkan ini untuk debugging:
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Data Mata Kuliah tidak ditemukan di database (DataTable kosong).", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                cbMatakuliah.DataSource = dt;
                cbMatakuliah.DisplayMember = "nama_mk";
                cbMatakuliah.ValueMember = "id_matakuliah";
                cbMatakuliah.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat mata kuliah: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRekap_Click(object sender, EventArgs e)
        {
            // Validasi agar user memilih mata kuliah dulu
            if (cbMatakuliah.SelectedValue == null)
            {
                MessageBox.Show("Silakan pilih Mata Kuliah terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Menggunakan Parameterized Query lewat array parameter untuk mencegah SQL Injection
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@idMK", cbMatakuliah.SelectedValue),
                    new SqlParameter("@tglAwal", dtpAwal.Value.Date),
                    new SqlParameter("@tglAkhir", dtpAkhir.Value.Date)
                };

                // Memanggil data rekap melalui Stored Procedure
                DataTable dt = db.ExecuteStoredProcedure("sp_GetRekapPresensi", parameters);

                dataGridView1.DataSource = dt;

                if (dt != null && dt.Rows.Count > 0)
                {
                    // Gambar data statistik ke komponen Chart secara otomatis
                    TampilkanGrafikPresensi(dt);
                }
                else
                {
                    if (chartPresensi.Series.Count > 0) chartPresensi.Series.Clear();
                    MessageBox.Show("Tidak ada data presensi untuk periode dan mata kuliah ini.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Rekap: " + ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====================================================================
        // MEMBUAT GRAFIK DARI DATA REKAP DATATABLE (ANTI-CRASH)
        // ====================================================================
        private void TampilkanGrafikPresensi(DataTable dt)
        {
            int totalHadir = 0;
            int totalIzin = 0;
            int totalSakit = 0;
            int totalAlpa = 0;

            foreach (DataRow row in dt.Rows)
            {
                // REVISI 2: Proteksi DBNull.Value agar program tidak crash jika ada kolom database yang kosong
                totalHadir += row["Hadir"] != DBNull.Value ? Convert.ToInt32(row["Hadir"]) : 0;
                totalIzin += row["Izin"] != DBNull.Value ? Convert.ToInt32(row["Izin"]) : 0;
                totalSakit += row["Sakit"] != DBNull.Value ? Convert.ToInt32(row["Sakit"]) : 0;
                totalAlpa += row["Alpa"] != DBNull.Value ? Convert.ToInt32(row["Alpa"]) : 0;
            }

            // Bersihkan grafik lama
            chartPresensi.Series.Clear();
            chartPresensi.Titles.Clear();

            // Tambahkan Judul Grafik Resmi
            chartPresensi.Titles.Add("Statistik Total Presensi Mahasiswa");

            // Bikin Series Data Baru bergaya diagram batang (Column)
            Series ser = new Series("Status Presensi");
            ser.ChartType = SeriesChartType.Column;

            // Masukkan data angka kalkulasi ke dalam grafik koordinat
            ser.Points.AddXY("Hadir", totalHadir);
            ser.Points.AddXY("Izin", totalIzin);
            ser.Points.AddXY("Sakit", totalSakit);
            ser.Points.AddXY("Alpa", totalAlpa);

            // Beri warna dekorasi yang kontras dan informatif
            ser.Points[0].Color = Color.MediumSeaGreen;
            ser.Points[1].Color = Color.Orange;
            ser.Points[2].Color = Color.DodgerBlue;
            ser.Points[3].Color = Color.Crimson;

            // Tampilkan angka nilai tepat di atas batang grafik
            ser.IsValueShownAsLabel = true;

            // Masukkan susunan data ke dalam kontrol UI Chart proyek
            chartPresensi.Series.Add(ser);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cbMatakuliah.SelectedIndex = -1;
            dtpAwal.Value = DateTime.Today.AddMonths(-1);
            dtpAkhir.Value = DateTime.Today;
            dataGridView1.DataSource = null;

            if (chartPresensi.Series.Count > 0) chartPresensi.Series.Clear();
            if (chartPresensi.Titles.Count > 0) chartPresensi.Titles.Clear();
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            this.Close();
            DashboardDosen dashboardDosen = new DashboardDosen();
            dashboardDosen.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}