using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemPresensiMahasiswa
{
    public partial class DashboardDosen : Form
    {
        public DashboardDosen()
        {
            InitializeComponent();
        }

        private void DashboardDosen_Load(object sender, EventArgs e)
        {
            // Tempat inisialisasi data atau nama dosen jika diperlukan di masa depan
        }

        private void btnInputPresensi_Click(object sender, EventArgs e)
        {
            InputPresensi inputPresensi = new InputPresensi();
            inputPresensi.Show();
            this.Hide(); // Sembunyikan dashboard dosen aman
        }

        private void btnRekapPresensi_Click(object sender, EventArgs e)
        {
            RekapPresensi rekapPresensi = new RekapPresensi();
            rekapPresensi.Show();
            this.Hide(); // Sembunyikan dashboard dosen aman
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Tambahkan konfirmasi logout demi kenyamanan dosen (UX Improvement)
            DialogResult result = MessageBox.Show("Apakah Anda yakin ingin keluar dari akun dosen?", "Konfirmasi Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Instansiasi form login kembali
                Login login = new Login();
                login.Show();

                // Hancurkan form dashboard ini sepenuhnya dari RAM komputer
                this.Dispose();
            }
        }
    }
}