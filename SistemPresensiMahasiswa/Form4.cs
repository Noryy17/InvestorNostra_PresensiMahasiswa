using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SistemPresensiMahasiswa
{
    public partial class GenerateLaporan : Form
    {
        private readonly SqlConnection conn;
        private readonly string connectionString = "Data Source=LAPTOP-DSPPD9L7\\FAIDARYA;Initial Catalog=SistemPresensiDB;Integrated Security=True";

        public GenerateLaporan()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void GenerateLaporan_Load(object sender, EventArgs e)
        {
            LoadMatakuliah();
            LoadDosen();

            dtpAwal.Value = DateTime.Now;
            dtpAkhir.Value = DateTime.Now;
        }

        private void LoadMatakuliah()
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                string query = "SELECT id_matakuliah, nama_mk FROM Matakuliah";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cbMatakuliah.DataSource = dt;
                cbMatakuliah.DisplayMember = "nama_mk";
                cbMatakuliah.ValueMember = "id_matakuliah";
                cbMatakuliah.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat mata kuliah: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void LoadDosen()
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                string query = "SELECT id_dosen, nama FROM Dosen";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cbDosen.DataSource = dt;
                cbDosen.DisplayMember = "nama";
                cbDosen.ValueMember = "id_dosen";
                cbDosen.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat daftar dosen: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        // =========================================================
        // TOMBOL 1: Menampilkan Data di Tabel (DataGridView)
        // =========================================================
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                string query = @"SELECT p.tanggal, m.nim, m.nama, p.status 
                                 FROM Presensi p
                                 INNER JOIN Mahasiswa m ON p.id_mahasiswa = m.id_mahasiswa
                                 WHERE p.id_matakuliah = @idMK 
                                 AND p.id_dosen = @idDosen 
                                 AND p.tanggal BETWEEN @tglAwal AND @tglAkhir";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idMK", cbMatakuliah.SelectedValue);
                cmd.Parameters.AddWithValue("@idDosen", cbDosen.SelectedValue);
                cmd.Parameters.AddWithValue("@tglAwal", dtpAwal.Value.Date);
                cmd.Parameters.AddWithValue("@tglAkhir", dtpAkhir.Value.Date);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
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
                    MessageBox.Show("Tidak ada data presensi yang ditemukan.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        // =========================================================
        // TOMBOL 2: Membuka Kertas Crystal Report (FormCetak)
        // =========================================================
        private void btnCetakLaporan_Click(object sender, EventArgs e)
        {
            // 1. Pastikan user sudah milih combobox sebelum cetak
            if (cbMatakuliah.SelectedValue == null || cbDosen.SelectedValue == null)
            {
                MessageBox.Show("Pilih Mata Kuliah dan Dosen terlebih dahulu sebelum mencetak!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 2. Ambil filter yang dipilih user
                int idMk = Convert.ToInt32(cbMatakuliah.SelectedValue);
                int idDsn = Convert.ToInt32(cbDosen.SelectedValue);
                DateTime tglMulai = dtpAwal.Value.Date;
                DateTime tglSelesai = dtpAkhir.Value.Date;

                // 3. Buka FormCetak (Crystal Report) dan kirim filter datanya ke sana
                FormCetak frmCetak = new FormCetak(idMk, idDsn, tglMulai, tglSelesai);
                frmCetak.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal membuka halaman cetak: " + ex.Message);
            }
        }

        private void dtpAkhir_ValueChanged(object sender, EventArgs e)
        {
            if (dtpAkhir.Value < dtpAwal.Value)
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
            // Biarkan kosong, ini dibutuhkan oleh desain form
        }
    }
}