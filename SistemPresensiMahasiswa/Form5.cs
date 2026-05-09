using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SistemPresensiMahasiswa
{
    public partial class KelolaMahasiswa : Form
    {
        // MODUL 9 - LANGKAH 2: Menambahkan BindingSource & DataTable
        private BindingSource bindingSource = new BindingSource();
        private DataTable dtMahasiswa = new DataTable();

        // Sesuaikan connection string dengan server Anda
        private readonly string connectionString =
            "Data Source=LAPTOP-2TIS9UVD\\RIZQIHUDAYA;Initial Catalog=SistemPresensiDB;Integrated Security=True";

        public KelolaMahasiswa()
        {
            InitializeComponent();
        }

        // MODUL 9 - LANGKAH 3: Menambahkan form Load
        private void KelolaMahasiswa_Load_1(object sender, EventArgs e)
        {
            // Setting Grid
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // FIX 1: Mapping Kolom UI ke Kolom Database
            // Pastikan string ini sesuai huruf besar/kecil dengan di database Anda!
            ColNim.DataPropertyName = "nim";
            ColNamaMahasiswa.DataPropertyName = "nama";
            ColJurusan.DataPropertyName = "jurusan";

            // BindingNavigator
            bindingNavigator1.BindingSource = bindingSource;

            LoadData();
        }

        // MODUL 9 - LANGKAH 4: Menambahkan load data
        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM vwMahasiswaPublic";

                    using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                    {
                        dtMahasiswa = new DataTable();
                        da.Fill(dtMahasiswa);

                        bindingSource.DataSource = dtMahasiswa;
                        dataGridView1.DataSource = bindingSource;

                        BindControls();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load data: " + ex.Message);
            }
        }

        // MODUL 9 - LANGKAH 5: Menambahkan bind control
        private void BindControls()
        {
            txtNIM.DataBindings.Clear();
            txtNama.DataBindings.Clear();
            txtJurusan.DataBindings.Clear();

            // FIX 2: Mapping Textbox ke Kolom Database
            txtNIM.DataBindings.Add("Text", bindingSource, "nim");
            txtNama.DataBindings.Add("Text", bindingSource, "nama");
            txtJurusan.DataBindings.Add("Text", bindingSource, "jurusan");
        }

        // MODUL 9 - LANGKAH 7: Menggunakan DataAdapter untuk Load Data
        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        // MODUL 9 - LANGKAH 8: INSERT Aman (Parameterized Query)
        private void btnTambah_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNIM.Text == "") { MessageBox.Show("NIM harus diisi"); txtNIM.Focus(); return; }
                if (txtNama.Text == "") { MessageBox.Show("Nama harus diisi"); txtNama.Focus(); return; }
                if (txtJurusan.Text == "") { MessageBox.Show("Jurusan harus diisi"); txtJurusan.Focus(); return; }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Mahasiswa (nim, nama, jurusan) VALUES (@NIM, @Nama, @Jurusan)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@NIM", txtNIM.Text);
                        cmd.Parameters.AddWithValue("@Nama", txtNama.Text);
                        cmd.Parameters.AddWithValue("@Jurusan", txtJurusan.Text);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Data Mahasiswa berhasil ditambahkan");
                ClearForm();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        // UPDATE (Lanjutan Penerapan Aman)
        private void btnUbah_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"UPDATE Mahasiswa SET Nama = @Nama, Jurusan = @Jurusan WHERE NIM = @NIM";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@NIM", txtNIM.Text);
                        cmd.Parameters.AddWithValue("@Nama", txtNama.Text);
                        cmd.Parameters.AddWithValue("@Jurusan", txtJurusan.Text);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Data berhasil diupdate");
                            ClearForm();
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Data tidak ditemukan");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        // DELETE (Lanjutan Penerapan Aman)
        private void btnHapus_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult resultConfirm = MessageBox.Show("Yakin ingin menghapus data?", "Konfirmasi",
                                                             MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resultConfirm == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM Mahasiswa WHERE NIM = @NIM";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@NIM", txtNIM.Text);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Data berhasil dihapus");
                    ClearForm();
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        // MODUL 9 - LANGKAH 9: Script Reset Data Otomatis (DENGAN FIX IDENTITY_INSERT)
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Menggunakan teknik UPDATE JOIN alih-alih DELETE & INSERT
                    // Ini akan menyalin kembali nama & jurusan dari tabel Backup ke tabel utama
                    // tanpa merusak relasi dengan tabel Presensi atau memicu error Identity.
                    string query = @"
                IF OBJECT_ID('dbo.Mahasiswa_Backup') IS NOT NULL
                BEGIN
                    UPDATE m
                    SET m.nama = b.nama,
                        m.jurusan = b.jurusan
                    FROM dbo.Mahasiswa m
                    INNER JOIN dbo.Mahasiswa_Backup b ON m.nim = b.nim;
                END";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Data berhasil direset ke kondisi awal");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Reset gagal: " + ex.Message);
            }
        }

        // MODUL 9 - LANGKAH 10: Simulasi SQL Injection (Konseptual)
        private void btnInject_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // QUERY TIDAK AMAN - String Concatenation sesuai modul
                    // Coba masukkan di text NIM: ' OR 1=1 --
                    string query = "UPDATE Mahasiswa SET Nama='HACKED' WHERE NIM='" + txtNIM.Text + "'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        int result = cmd.ExecuteNonQuery();
                        MessageBox.Show(result + " baris terupdate");
                    }
                }

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Mengatur perpindahan data saat baris DataGridView diklik
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                bindingSource.Position = e.RowIndex;
            }
        }

        // Navigasi Kembali
        private void btnKembali_Click(object sender, EventArgs e)
        {
            this.Close();
            DashboardAdmin dashboardAdmin = new DashboardAdmin();
            dashboardAdmin.Show();
        }

        // Method Pembantu: Membersihkan form input
        private void ClearForm()
        {
            txtNIM.Clear();
            txtNama.Clear();
            txtJurusan.Clear();
            txtNIM.Focus();
        }
    }
}