using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SistemPresensiMahasiswa
{
    public partial class GenerateLaporan : Form
    {
        // Memanggil Class DAL yang sudah mendukung IP dinamis multi-user
        private Connection_DAL_ db = new Connection_DAL_();

        public GenerateLaporan()
        {
            InitializeComponent();
        }

        private void GenerateLaporan_Load(object sender, EventArgs e)
        {
            LoadMatakuliah();
            LoadDosen();

            // Set default tanggal awal ke awal bulan Juni 2026 agar data dummy langsung otomatis terfilter
            dtpAwal.Value = new DateTime(2026, 6, 1);
            dtpAkhir.Value = DateTime.Now;
        }

        private void LoadMatakuliah()
        {
            try
            {
                // REVISI: Mengambil data lewat Stored Procedure dari objek DAL (db)
                DataTable dt = db.ExecuteStoredProcedure("sp_GetLookupMatakuliah");

                if (dt != null)
                {
                    cbMatakuliah.DataSource = dt;
                    cbMatakuliah.DisplayMember = "nama_mk";
                    cbMatakuliah.ValueMember = "id_matakuliah";
                    cbMatakuliah.SelectedIndex = -1; // Kosongkan pilihan awal
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat mata kuliah: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDosen()
        {
            try
            {
                // REVISI: Mengambil data lewat Stored Procedure dari objek DAL (db)
                DataTable dt = db.ExecuteStoredProcedure("sp_GetLookupDosen");

                if (dt != null)
                {
                    cbDosen.DataSource = dt;
                    cbDosen.DisplayMember = "nama";
                    cbDosen.ValueMember = "id_dosen";
                    cbDosen.SelectedIndex = -1; // Kosongkan pilihan awal
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat daftar dosen: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // TOMBOL 1: Menampilkan Data di Tabel (DataGridView)
        // =========================================================
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            // Validasi: Cegah error null jika combobox belum dipilih user
            if (cbMatakuliah.SelectedValue == null || cbDosen.SelectedValue == null)
            {
                MessageBox.Show("Silakan pilih Mata Kuliah dan Dosen terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // REVISI: Susun parameter array untuk dikirim secara aman ke DAL
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@idMK", cbMatakuliah.SelectedValue),
                    new SqlParameter("@idDosen", cbDosen.SelectedValue),
                    new SqlParameter("@tglAwal", dtpAwal.Value.Date),
                    new SqlParameter("@tglAkhir", dtpAkhir.Value.Date)
                };

                // REVISI: Jalankan SP pengambil laporan data presensi
                DataTable dt = db.ExecuteStoredProcedure("sp_GenerateLaporanPresensi", parameters);

                if (dt != null && dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                    dataGridView1.Columns["tanggal"].HeaderText = "Tanggal";
                    dataGridView1.Columns["nim"].HeaderText = "NIM";
                    dataGridView1.Columns["nama"].HeaderText = "Nama Mahasiswa";
                    dataGridView1.Columns["status"].HeaderText = "Status Presensi";
                }
                else
                {
                    dataGridView1.DataSource = null;
                    MessageBox.Show("Tidak ada data presensi yang ditemukan untuk filter tersebut.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat memuat laporan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // TOMBOL 2: Membuka Kertas Crystal Report (FormCetak)
        // =========================================================
        private void btnCetakLaporan_Click(object sender, EventArgs e)
        {
            if (cbMatakuliah.SelectedValue == null || cbDosen.SelectedValue == null)
            {
                MessageBox.Show("Pilih Mata Kuliah dan Dosen terlebih dahulu sebelum mencetak!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int idMk = Convert.ToInt32(cbMatakuliah.SelectedValue);
                int idDsn = Convert.ToInt32(cbDosen.SelectedValue);
                DateTime tglMulai = dtpAwal.Value.Date;
                DateTime tglSelesai = dtpAkhir.Value.Date;

                // Buka FormCetak Crystal Report
                FormCetak frmCetak = new FormCetak(idMk, idDsn, tglMulai, tglSelesai);
                frmCetak.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal membuka halaman cetak: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dtpAkhir_ValueChanged(object sender, EventArgs e)
        {
            if (dtpAkhir.Value.Date < dtpAwal.Value.Date)
            {
                MessageBox.Show("Tanggal akhir tidak boleh lebih kecil dari tanggal awal!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpAkhir.Value = dtpAwal.Value;
            }
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            this.Close();
            DashboardAdmin dashboardAdminForm = new DashboardAdmin();
            dashboardAdminForm.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Dibutuhkan oleh Windows Form Designer
        }
    }
}