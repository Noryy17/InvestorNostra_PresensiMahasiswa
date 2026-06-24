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

        private void btnInputPresensi_Click(object sender, EventArgs e)
        {
            InputPresensi inputPresensi = new InputPresensi();
            inputPresensi.Show();
            this.Hide();
        }

        private void btnRekapPresensi_Click(object sender, EventArgs e)
        {
            RekapPresensi rekapPresensi = new RekapPresensi();
            rekapPresensi.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
             Login login = new Login();
                login.Show();
        }

        private void DashboardDosen_Load(object sender, EventArgs e)
        {

        }
    }
}
