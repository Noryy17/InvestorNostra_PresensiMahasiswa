using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SistemPresensiMahasiswa
{
    public partial class Login : Form
    {
        private readonly SqlConnection conn;
        private readonly string connectionString =
        "Data Source=LAPTOP-2TIS9UVD\\RIZQIHUDAYA;Initial Catalog=SistemPresensiDB;Integrated Security=True";
        public Login()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            try
            {
                conn.Open();

                // Cek apakah user adalah Admin
                string queryAdmin = "SELECT * FROM Admin WHERE username=@username AND password=@password";
                using (SqlCommand cmdAdmin = new SqlCommand(queryAdmin, conn))
                {
                    cmdAdmin.Parameters.AddWithValue("@username", username);
                    cmdAdmin.Parameters.AddWithValue("@password", password);

                    SqlDataReader readerAdmin = cmdAdmin.ExecuteReader();
                    if (readerAdmin.HasRows)
                    {
                        // Login sebagai Admin
                        readerAdmin.Close();
                        conn.Close();

                        DashboardAdmin dashAdmin = new DashboardAdmin();
                        dashAdmin.Show();
                        this.Hide();
                        return;
                    }
                    readerAdmin.Close();
                }

                // Cek apakah user adalah Dosen
                string queryDosen = "SELECT * FROM Dosen WHERE username=@username AND password=@password";
                using (SqlCommand cmdDosen = new SqlCommand(queryDosen, conn))
                {
                    cmdDosen.Parameters.AddWithValue("@username", username);
                    cmdDosen.Parameters.AddWithValue("@password", password);

                    SqlDataReader readerDosen = cmdDosen.ExecuteReader();
                    if (readerDosen.HasRows)
                    {
                        // Login sebagai Dosen
                        readerDosen.Close();
                        conn.Close();

                        DashboardDosen dashDosen = new DashboardDosen();
                        dashDosen.Show();
                        this.Hide();
                        return;
                    }
                    readerDosen.Close();
                }

                conn.Close();
                MessageBox.Show("Username atau password salah!", "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }
    }
}
