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
    public partial class KelolaDosen : Form
    {
        private readonly SqlConnection conn;
        private readonly string connectionString =
        "Data Source=VICTUS-PUNYA-LU\\LUTFI;Initial Catalog=SistemPresensiDB;Integrated Security=True";
        public KelolaDosen()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }

                if (txtNIP.Text == "")
                {
                    MessageBox.Show("NIM harus diisi");
                    txtNIP.Focus();
                    return;
                }

                if (txtNama.Text == "")
                {
                    MessageBox.Show("Nama harus diisi");
                    txtNama.Focus();
                    return;
                }

                if (txtUsername.Text == "")
                {
                    MessageBox.Show("Kode Prodi harus diisi");
                    txtUsername.Focus();
                    return;
                }

                if (txtPassword.Text == "")
                {
                    MessageBox.Show("NIM harus diisi");
                    txtPassword.Focus();
                    return;
                }

                string query = "INSERT INTO Dosen " +
                               "(nip, nama, username, password) " +
                               "VALUES (@NIP, @Nama, @Username, @Password)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NIP", txtNIP.Text);
                cmd.Parameters.AddWithValue("@Nama", txtNama.Text);
                cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Data Dosen berhasil ditambahkan");
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

                string query = @"UPDATE Dosen
                         SET Nama = @Nama,
                             Username = @Username,
                             Password = @Password
                         WHERE NIP = @NIP";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NIP", txtNIP.Text);
                cmd.Parameters.AddWithValue("@Nama", txtNama.Text);
                cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text);

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
                    string query = "DELETE FROM Dosen WHERE NIP = @NIP";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@NIP", txtNIP.Text);

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

                dataGridView1.Columns.Add("id_dosen", "id_dosen");
                dataGridView1.Columns.Add("NIP", "NIP");
                dataGridView1.Columns.Add("nama", "nama");
                dataGridView1.Columns.Add("username", "username");
                dataGridView1.Columns.Add("password", "password");

                string query = "SELECT * FROM Dosen";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dataGridView1.Rows.Add(
                        reader["id_dosen"].ToString(),
                        reader["NIP"].ToString(),
                        reader["Nama"].ToString(),
                        reader["Username"].ToString(),
                        reader["Password"].ToString()
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
            txtNIP.Clear();
            txtNama.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            txtNIP.Focus();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // make sure it's not the header row
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                
                txtNIP.Text = row.Cells["NIP"].Value.ToString();
                txtNama.Text = row.Cells["Nama"].Value.ToString();
                txtUsername.Text = row.Cells["Username"].Value.ToString();
                txtPassword.Text = row.Cells["Password"].Value.ToString();
            }
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            this.Close();
            DashboardAdmin dashboardAdminForm = new DashboardAdmin();
            dashboardAdminForm.Show();
        }

        private void txtNIP_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Cek apakah karakter yang ditekan bukan angka DAN bukan tombol Backspace
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Batalkan input karakter tersebut
                MessageBox.Show("NIP hanya boleh diisi dengan angka!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
    }
}
