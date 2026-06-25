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
        // REVISI 1: Gunakan class Connection_DAL_ sebagai pusat kendali query database (Otomatis memakai IP dinamis)
        private Connection_DAL_ db = new Connection_DAL_();

        public Login()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            // Kosmetik form saat pertama kali dimuat jika perlu
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Validasi Input Kosong (UX Improvement)
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Username dan Password tidak boleh kosong!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // REVISI 2: Siapkan susunan parameter SQL untuk dikirim ke DAL
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@username", username),
                    new SqlParameter("@password", password)
                };

                // --- CEK LOGIN ADMIN VIA STORED PROCEDURE ---
                DataTable dtAdmin = db.ExecuteStoredProcedure("sp_LoginAdmin", parameters);

                if (dtAdmin != null && dtAdmin.Rows.Count > 0)
                {
                    // Login sukses sebagai Admin
                    DashboardAdmin dashAdmin = new DashboardAdmin();
                    dashAdmin.Show();
                    this.Hide();
                    return;
                }

                // REVISI 3: Re-instansiasi ulang parameter agar bersih saat digunakan kembali untuk query kedua
                parameters = new SqlParameter[]
                {
                    new SqlParameter("@username", username),
                    new SqlParameter("@password", password)
                };

                // --- CEK LOGIN DOSEN VIA STORED PROCEDURE ---
                DataTable dtDosen = db.ExecuteStoredProcedure("sp_LoginDosen", parameters);

                if (dtDosen != null && dtDosen.Rows.Count > 0)
                {
                    // Login sukses sebagai Dosen
                    DashboardDosen dashDosen = new DashboardDosen();
                    dashDosen.Show();
                    this.Hide();
                    return;
                }

                // Jika kedua pengecekan di atas terlewati (tidak ada data matching)
                MessageBox.Show("Username atau password salah!", "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtUsername.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan sistem: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PanelCard_Paint(object sender, PaintEventArgs e)
        {
            // Tempat kustomisasi grafis panel jika ada
        }
    }
}