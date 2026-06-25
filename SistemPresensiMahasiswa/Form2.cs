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

        private void DashboardAdmin_Load(object sender, EventArgs e)
        {
            // Tempat inisialisasi data dashboard jika ada di masa depan
        }

        private void btnKelolaMahasiswa_Click(object sender, EventArgs e)
        {
            KelolaMahasiswa kelolaMahaForm = new KelolaMahasiswa();
            kelolaMahaForm.Show();
            this.Hide(); // Sembunyikan dashboard admin
        }

        private void btnKelolaMatakuliah_Click(object sender, EventArgs e)
        {
            KelolaMatKul kelolaMatkulForm = new KelolaMatKul();
            kelolaMatkulForm.Show();
            this.Hide();
        }

        private void btnKelolaDosen_Click(object sender, EventArgs e)
        {
            KelolaDosen kelolaDosenForm = new KelolaDosen();
            kelolaDosenForm.Show();
            this.Hide();
        }

        private void btnGenerateLaporan_Click(object sender, EventArgs e)
        {
            GenerateLaporan generateLaporanForm = new GenerateLaporan();
            generateLaporanForm.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Berikan konfirmasi sebelum logout (UX Improvement)
            DialogResult result = MessageBox.Show("Apakah Anda yakin ingin logout?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Buka kembali form login
                Login loginForm = new Login();
                loginForm.Show();

                // Bersihkan dan tutup form dashboard sepenuhnya dari memori background
                this.Dispose();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Kosongkan saja atau hapus lewat jendela properti designer jika tidak dipakai
        }
    }
}