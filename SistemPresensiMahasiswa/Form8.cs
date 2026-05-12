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

    
    public partial class InputPresensi : Form
    {

        private readonly SqlConnection conn;
        private readonly string connectionString =
        "Data Source=VICTUS-PUNYA-LU\\LUTFI;Initial Catalog=SistemPresensiDB;Integrated Security=True";
        public InputPresensi()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            this.Close();
            DashboardDosen dashboardDosen = new DashboardDosen();
            dashboardDosen.Show();
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


        private void LoadMahasiswa()
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                // Ambil ID untuk nilai sistem, dan Nama untuk tampilan user
                string query = "SELECT id_mahasiswa, nama FROM Mahasiswa";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Menghubungkan ComboBox dengan DataTable
                cbMahasiswa.DataSource = dt;

                // Apa yang dilihat oleh user di layar
                cbMahasiswa.DisplayMember = "nama";

                // Apa yang dikirim ke database saat klik 'Generate' (ID-nya)
                cbMahasiswa.ValueMember = "id_mahasiswa";

                // Agar ComboBox tidak langsung memilih item pertama saat form terbuka (opsional)
                cbMahasiswa.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat mahasiswa: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnInput_Click(object sender, EventArgs e)
        {
            // 1. Validasi: Pastikan tidak ada yang kosong
            if (cbMatakuliah.SelectedValue == null || cbDosen.SelectedValue == null ||
                cbMahasiswa.SelectedValue == null || cbStatus.SelectedItem == null)
            {
                MessageBox.Show("Semua kolom harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                // 2. Query Insert sesuai tabel Screenshot 2026-05-12 210145.png
                string query = @"INSERT INTO Presensi (tanggal, status, id_mahasiswa, id_matakuliah, id_dosen) 
                         VALUES (@tanggal, @status, @id_mhs, @id_mk, @id_dosen)";

                SqlCommand cmd = new SqlCommand(query, conn);

                // Mengambil nilai dari kontrol
                cmd.Parameters.AddWithValue("@tanggal", dtpTanggal.Value.Date);
                cmd.Parameters.AddWithValue("@status", cbStatus.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@id_mhs", cbMahasiswa.SelectedValue);
                cmd.Parameters.AddWithValue("@id_mk", cbMatakuliah.SelectedValue);
                cmd.Parameters.AddWithValue("@id_dosen", cbDosen.SelectedValue);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Presensi berhasil disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshTable(); // Fungsi untuk memperbarui tampilan DataGridView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menyimpan data: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void RefreshTable()
        {
            try
            {
                // Query untuk menampilkan data terbaru (menggabungkan Nama Mahasiswa agar mudah dibaca)
                string query = @"SELECT p.tanggal, m.nim, m.nama, p.status 
                         FROM Presensi p 
                         JOIN Mahasiswa m ON p.id_mahasiswa = m.id_mahasiswa 
                         ORDER BY p.id_presensi DESC"; // Data terbaru di atas

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Refresh: " + ex.Message);
            }
        }
        private void InputPresensi_Load(object sender, EventArgs e)
        {
            LoadMatakuliah();
            LoadDosen();
            LoadMahasiswa(); // Buat fungsi serupa untuk mengambil data mahasiswa

            // Mengisi pilihan Status secara manual sesuai aturan CHECK di database
            cbStatus.Items.Clear();
            cbStatus.Items.Add("Hadir");
            cbStatus.Items.Add("Izin");
            cbStatus.Items.Add("Sakit");
            cbStatus.Items.Add("Alpa");
        }

        private void btnClearForm_Click(object sender, EventArgs e)
        {
            // Mengembalikan ComboBox ke posisi tidak memilih apapun
            cbMatakuliah.SelectedIndex = -1;
            cbDosen.SelectedIndex = -1;
            cbMahasiswa.SelectedIndex = -1;
            cbStatus.SelectedIndex = -1;

            // Mengembalikan tanggal ke hari ini
            dtpTanggal.Value = DateTime.Now;

            // Memberikan fokus kembali ke input pertama agar user bisa langsung mulai lagi
            cbMatakuliah.Focus();
        }
    }
}
