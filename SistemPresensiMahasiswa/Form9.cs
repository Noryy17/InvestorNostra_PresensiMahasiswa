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
using System.Windows.Forms.DataVisualization.Charting; // WAJIB ADA: Library pendukung komponen Chart

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

        private void FormRekapPresensi_Load(object sender, EventArgs e)
        {
            LoadMatakuliah(); // Mengisi cbMatakuliah dengan daftar pelajaran
            dtpAwal.Value = DateTime.Now.AddMonths(-1); // Default 1 bulan terakhir
            dtpAkhir.Value = DateTime.Now;

            // Inisialisasi awal tampilan chart agar bersih
            if (chartPresensi.Series.Count > 0) chartPresensi.Series.Clear();
        }

        private void LoadMatakuliah()
        {
            try
            {
                DataTable dt = db.ExecuteStoredProcedure("sp_GetMatakuliah");

                cbMatakuliah.DisplayMember = "nama_mk";
                cbMatakuliah.ValueMember = "id_matakuliah";
                cbMatakuliah.DataSource = dt;
                cbMatakuliah.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat mata kuliah: " + ex.Message);
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
                // MENGGUNAKAN DAL: Menggunakan Parameterized Query lewat array parameter untuk mencegah SQL Injection
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@idMK", cbMatakuliah.SelectedValue),
                    new SqlParameter("@tglAwal", dtpAwal.Value.Date),
                    new SqlParameter("@tglAkhir", dtpAkhir.Value.Date)
                };

                // Memanggil data rekap melalui Stored Procedure (Asumsi nama SP: sp_GetRekapPresensi)
                // Jika di database-mu nama SP rekapnya berbeda, sesuaikan string di bawah ini:
                DataTable dt = db.ExecuteStoredProcedure("sp_GetRekapPresensi", parameters);

                dataGridView1.DataSource = dt;

                if (dt.Rows.Count > 0)
                {
                    // KODE SAKTI: Gambar data statistik ke komponen Chart secara otomatis
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
        // KODE SAKTI MODUL 14: MEMBUAT GRAFIK DARI DATA REKAP DATATABLE
        // ====================================================================
        private void TampilkanGrafikPresensi(DataTable dt)
        {
            // 1. Hitung akumulasi total Hadir, Izin, Sakit, Alpa dari seluruh baris data mahasiswa
            int totalHadir = 0;
            int totalIzin = 0;
            int totalSakit = 0;
            int totalAlpa = 0;

            foreach (DataRow row in dt.Rows)
            {
                totalHadir += Convert.ToInt32(row["Hadir"]);
                totalIzin += Convert.ToInt32(row["Izin"]);
                totalSakit += Convert.ToInt32(row["Sakit"]);
                totalAlpa += Convert.ToInt32(row["Alpa"]);
            }

            // 2. Bersihkan grafik lama
            chartPresensi.Series.Clear();
            chartPresensi.Titles.Clear();

            // 3. Tambahkan Judul Grafik Resmi
            chartPresensi.Titles.Add("Statistik Presensi Mahasiswa Kelas");

            // 4. Bikin Series Data Baru bergaya diagram batang (Column)
            Series ser = new Series("Status Presensi");
            ser.ChartType = SeriesChartType.Column; // Bisa diubah ke 'Pie' jika ingin diagram lingkaran

            // 5. Masukkan data angka kalkulasi ke dalam grafik koordinat kartesius
            ser.Points.AddXY("Hadir", totalHadir);
            ser.Points.AddXY("Izin", totalIzin);
            ser.Points.AddXY("Sakit", totalSakit);
            ser.Points.AddXY("Alpa", totalAlpa);

            // Beri warna dekorasi yang kontras dan rapi agar asdos terkesan
            ser.Points[0].Color = Color.MediumSeaGreen;
            ser.Points[1].Color = Color.Orange;
            ser.Points[2].Color = Color.DodgerBlue;
            ser.Points[3].Color = Color.Crimson;

            // Tampilkan angka nilai di atas batang grafik
            ser.IsValueShownAsLabel = true;

            // 6. Masukkan susunan data ke dalam kontrol UI Chart proyek
            chartPresensi.Series.Add(ser);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cbMatakuliah.SelectedIndex = -1;
            dtpAwal.Value = DateTime.Now.AddMonths(-1);
            dtpAkhir.Value = DateTime.Now;
            dataGridView1.DataSource = null;
            if (chartPresensi.Series.Count > 0) chartPresensi.Series.Clear(); // Bersihkan juga grafik saat di-clear
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