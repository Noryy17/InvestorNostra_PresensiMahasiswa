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
    public partial class KelolaMatKul : Form
    {
        private readonly SqlConnection conn;
        private readonly string connectionString =
        "Data Source=VICTUS-PUNYA-LU\\LUTFI;Initial Catalog=SistemPresensiDB;Integrated Security=True";
        public KelolaMatKul()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }

                if (txtKodeMK.Text == "")
                {
                    MessageBox.Show("Kode Mata Kuliah harus diisi");
                    txtKodeMK.Focus();
                    return;
                }

                if (txtNamaMK.Text == "")
                {
                    MessageBox.Show("Nama Mata Kuliah harus diisi");
                    txtNamaMK.Focus();
                    return;
                }

                if (txtSKS.Text == "")
                {
                    MessageBox.Show("SKS harus diisi");
                    txtSKS.Focus();
                    return;
                }

                string query = "INSERT INTO Matakuliah " +
                               "(kode_mk, nama_mk, sks) " +
                               "VALUES (@Kode_MK, @Nama_MK, @SKS)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Kode_MK", txtKodeMK.Text);
                cmd.Parameters.AddWithValue("@Nama_MK", txtNamaMK.Text);
                cmd.Parameters.AddWithValue("@SKS", txtSKS.Text);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Data Mata Kuliah berhasil ditambahkan");
                    ClearForm();
                    btnLoad.PerformClick();
                }
                else
                {
                    MessageBox.Show("Data gagal ditambahkan");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void btnUbah_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }

                string query = @"UPDATE Matakuliah
                         SET nama_mk = @Nama_MK,
                             sks = @SKS
                         WHERE kode_mk = @Kode_MK";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Kode_MK", txtKodeMK.Text);
                cmd.Parameters.AddWithValue("@Nama_MK", txtNamaMK.Text);
                cmd.Parameters.AddWithValue("@SKS", txtSKS.Text);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Data berhasil diupdate");
                    ClearForm();
                    btnLoad.PerformClick();
                }
                else
                {
                    MessageBox.Show("Data tidak ditemukan");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }

                DialogResult resultConfirm = MessageBox.Show(
                    "Yakin ingin menghapus data?",
                    "Konfirmasi",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultConfirm == DialogResult.Yes)
                {
                    string query = "DELETE FROM Matakuliah WHERE kode_mk = @Kode_MK";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Kode_MK", txtKodeMK.Text);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Data berhasil dihapus");
                        ClearForm();
                        btnLoad.PerformClick();
                    }
                    else
                    {
                        MessageBox.Show("Data tidak ditemukan");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }

                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

                dataGridView1.Columns.Add("Kode_MK", "Kode_MK");
                dataGridView1.Columns.Add("Nama_MK", "Nama_MK");
                dataGridView1.Columns.Add("SKS", "SKS");

                string query = "SELECT * FROM Matakuliah";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dataGridView1.Rows.Add(
                        reader["Kode_MK"].ToString(),
                        reader["Nama_MK"].ToString(),
                        reader["SKS"].ToString()
                    );
                }

                reader.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan data: " + ex.Message);
            }
        }

        private void ClearForm()
        {
            txtKodeMK.Clear();
            txtNamaMK.Clear();
            txtSKS.Clear();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // make sure it's not the header row
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtKodeMK.Text = row.Cells["Kode_MK"].Value.ToString();
                txtNamaMK.Text = row.Cells["Nama_MK"].Value.ToString();
                txtSKS.Text = row.Cells["SKS"].Value.ToString();
            }
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            this.Close();
            DashboardAdmin dashboardAdminForm = new DashboardAdmin();
            dashboardAdminForm.Show();
        }

        private void txtSKS_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Cek apakah karakter yang ditekan bukan angka DAN bukan tombol Backspace
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Batalkan input karakter tersebut
                MessageBox.Show("SKS hanya boleh diisi dengan angka!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtSKS_Validating(object sender, CancelEventArgs e)
        {
            int nilaiSks;
            if (int.TryParse(txtSKS.Text, out nilaiSks))
            {
                if (nilaiSks < 1 || nilaiSks > 6)
                {
                    MessageBox.Show("SKS harus di antara 1 sampai 6!");
                    txtSKS.Clear();
                    e.Cancel = true; // Mencegah user pindah ke input lain sebelum diperbaiki
                }
            }
        }

        private void KelolaMatKul_Load(object sender, EventArgs e)
        {

        }

            private void txtKodeMK_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Mengizinkan: Huruf (IsLetter), Angka (IsDigit), dan Backspace (IsControl)
            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Batalkan input jika itu simbol atau spasi
                MessageBox.Show("Kode Mata Kuliah hanya boleh berisi huruf dan angka tanpa simbol!",
                                "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtNamaMK_KeyPress(object sender, KeyPressEventArgs e)
        {
           if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true; // Batalkan input karakter tersebut
                MessageBox.Show("Nama Mata Kuliah hanya boleh diisi dengan huruf!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
    }
    }
}
