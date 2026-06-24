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
    public partial class DashboardAdmin : Form
    {
        public DashboardAdmin()
        {
            InitializeComponent();
        }

        private void btnKelolaMahasiswa_Click(object sender, EventArgs e)
        {
            KelolaMahasiswa kelolaMahaForm = new KelolaMahasiswa();
            kelolaMahaForm.Show();
            this.Hide(); // sembunyikan dashboard admin
        }

        private void btnKelolaMatakuliah_Click(object sender, EventArgs e)
        {
            KelolaMatKul kelolaMatkulForm = new KelolaMatKul();
            kelolaMatkulForm.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            Login loginForm = new Login();
            loginForm.Show();
        }

        private void btnKelolaDosen_Click(object sender, EventArgs e)
        {
            KelolaDosen kelolaDosenForm = new KelolaDosen();
            kelolaDosenForm.Show();
            this.Hide();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnGenerateLaporan_Click(object sender, EventArgs e)
        {
            GenerateLaporan generateLaporanForm = new GenerateLaporan();
            generateLaporanForm.Show();
            this.Hide();
        }

        private void DashboardAdmin_Load(object sender, EventArgs e)
        {

        }
    }
}
