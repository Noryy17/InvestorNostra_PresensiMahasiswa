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

namespace SistemPresensiMahasiswa
{
    public partial class GenerateLaporan : Form
    {
        private readonly SqlConnection conn;
        private readonly string connectionString =
        "Data Source=LAPTOP-DSPPD9L7\\FAIDARYA;Initial Catalog=SistemPresensiDB;Integrated Security=True";
        public GenerateLaporan()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            this.Close();
            DashboardAdmin dashboardAdminForm = new DashboardAdmin();
            dashboardAdminForm.Show();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                // Query menggunakan JOIN untuk mengambil Nama dari tabel Mahasiswa
                // Filter menggunakan id_matakuliah dan id_dosen (asumsi ComboBox menyimpan ID di SelectedValue)
                string query = @"SELECT p.tanggal, m.nim, m.nama, p.status 
                         FROM Presensi p
                         INNER JOIN Mahasiswa m ON p.id_mahasiswa = m.id_mahasiswa
                         WHERE p.id_matakuliah = @idMK 
                         AND p.id_dosen = @idDosen 
                         AND p.tanggal BETWEEN @tglAwal AND @tglAkhir";

                SqlCommand cmd = new SqlCommand(query, conn);

                // Pastikan ComboBox Anda sudah di-set ValueMember-nya ke 'id_matakuliah' dan 'id_dosen'
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

                    // Opsional: Merapikan Header Kolom di DataGridView
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

        private void LoadMatakuliah()
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                // Ambil ID untuk nilai sistem, dan Nama untuk tampilan user
                string query = "SELECT id_matakuliah, nama_mk FROM Matakuliah";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Menghubungkan ComboBox dengan DataTable
                cbMatakuliah.DataSource = dt;

                // Apa yang dilihat oleh user di layar
                cbMatakuliah.DisplayMember = "nama_mk";

                // Apa yang dikirim ke database saat klik 'Generate' (ID-nya)
                cbMatakuliah.ValueMember = "id_matakuliah";

                // Agar ComboBox tidak langsung memilih item pertama saat form terbuka (opsional)
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
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                // Ambil id_dosen untuk sistem dan nama untuk tampilan
                string query = "SELECT id_dosen, nama FROM Dosen";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cbDosen.DataSource = dt;
                cbDosen.DisplayMember = "nama";   // Menampilkan "Dr. Budi Santoso", dsb.
                cbDosen.ValueMember = "id_dosen"; // Menyimpan angka ID (1, 2, dst.)

                cbDosen.SelectedIndex = -1; // Agar awalnya kosong
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

        private void dtpAkhir_ValueChanged(object sender, EventArgs e)
        {
            // Cek jika tanggal akhir lebih kecil dari tanggal awal
            if (dtpAkhir.Value < dtpAwal.Value)
            {
                MessageBox.Show("Tanggal akhir tidak boleh lebih kecil dari tanggal awal!",
                                "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Kembalikan tanggal akhir sama dengan tanggal awal
                dtpAkhir.Value = dtpAwal.Value;
            }
        }

        private void GenerateLaporan_Load(object sender, EventArgs e)
        {
            LoadMatakuliah();
            LoadDosen();

            dtpAwal.Value = DateTime.Now;
            dtpAkhir.Value = DateTime.Now;
        }
    }    
}

