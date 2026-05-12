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

        // Connection string sudah disesuaikan dengan milik Dain
        private readonly string connectionString =
            "Data Source=VICTUS-PUNYA-LU\\LUTFI;Initial Catalog=SistemPresensiDB;Integrated Security=True";

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
            ColNim.DataPropertyName = "nim";
            ColNamaMahasiswa.DataPropertyName = "nama";
            ColJurusan.DataPropertyName = "jurusan";

            // BindingNavigator
            bindingNavigator1.BindingSource = bindingSource;

            LoadData();
        }

        // MODUL 9 - LANGKAH 4: Menambahkan load data (DIUPDATE PAKAI SP)
        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Menggunakan Stored Procedure sp_GetMahasiswa alih-alih query biasa
                    using (SqlCommand cmd = new SqlCommand("sp_GetMahasiswa", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure; // Wajib agar sistem tahu ini SP

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            dtMahasiswa = new DataTable();
                            da.Fill(dtMahasiswa);

                            bindingSource.DataSource = dtMahasiswa;
                            dataGridView1.DataSource = bindingSource;

                            BindControls();
                        }
                    }
                }

                // Panggil fitur hitung total setelah data berhasil dimuat
                HitungTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // MODUL 9 - LANGKAH 8: INSERT Aman (DIUPDATE PAKAI SP)
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
                    using (SqlCommand cmd = new SqlCommand("sp_InsertMahasiswaBaru", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure; // Panggil SP Insert

                        cmd.Parameters.AddWithValue("@NIM", txtNIM.Text);
                        cmd.Parameters.AddWithValue("@Nama", txtNama.Text);
                        cmd.Parameters.AddWithValue("@Jurusan", txtJurusan.Text);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Data Mahasiswa berhasil ditambahkan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadData(); // LoadData sudah memuat HitungTotal di dalamnya
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error Tambah", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // UPDATE (DIUPDATE PAKAI SP)
        private void btnUbah_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_UpdateMahasiswa", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure; // Panggil SP Update

                        cmd.Parameters.AddWithValue("@NIM", txtNIM.Text);
                        cmd.Parameters.AddWithValue("@Nama", txtNama.Text);
                        cmd.Parameters.AddWithValue("@Jurusan", txtJurusan.Text);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Data berhasil diupdate", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error Ubah", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // DELETE (DIUPDATE PAKAI SP)
        private void btnHapus_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult resultConfirm = MessageBox.Show("Yakin ingin menghapus data dengan NIM " + txtNIM.Text + "?",
                                                             "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (resultConfirm == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("sp_DeleteMahasiswa", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure; // Panggil SP Delete
                            cmd.Parameters.AddWithValue("@NIM", txtNIM.Text);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Data berhasil dihapus", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error Hapus", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // MODUL 9 - LANGKAH 9: Script Reset Data Otomatis
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
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

        // Event kosong
        private void lblTotal_Click(object sender, EventArgs e)
        {

        }

        // FUNGSI LAMA: Hitung Total Mahasiswa dari Stored Procedure
        private void HitungTotal()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_CountMahasiswa", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter outputParam = new SqlParameter("@Total", SqlDbType.Int);
                        outputParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(outputParam);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        // Pastikan di form design Dain ada Label bernama 'lblTotal'
                        lblTotal.Text = "Total Mahasiswa: " + outputParam.Value.ToString();
                    }
                }
            }
            catch (Exception)
            {
                lblTotal.Text = "Total Mahasiswa: -";
            }
        }

        private void txtNIM_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Cek apakah karakter yang ditekan bukan angka DAN bukan tombol Backspace
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Batalkan input karakter tersebut
                MessageBox.Show("NIM hanya boleh diisi dengan angka!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtNama_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Cek apakah karakter bukan huruf, bukan spasi, DAN bukan Backspace
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true; // Batalkan input
                MessageBox.Show("Nama hanya boleh diisi dengan huruf!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtJurusan_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Cek apakah karakter bukan huruf, bukan spasi, DAN bukan Backspace
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true; // Batalkan input
                MessageBox.Show("Jurusan hanya boleh diisi dengan huruf!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}