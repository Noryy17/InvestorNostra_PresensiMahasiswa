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
        // Memakai objek DAL yang otomatis mendeteksi IP Server dinamis
        private Connection_DAL_ db = new Connection_DAL_();
        private string nipAsli = "";

        public KelolaDosen()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            btnLoad.PerformClick();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNIP.Text) || string.IsNullOrWhiteSpace(txtNama.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Semua input data dosen harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Menggunakan SqlParameter untuk dikirim ke fungsi ExecuteNonQueryStoredProcedure
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@NIP", txtNIP.Text.Trim()),
                    new SqlParameter("@Nama", txtNama.Text.Trim()),
                    new SqlParameter("@Username", txtUsername.Text.Trim()),
                    new SqlParameter("@Password", txtPassword.Text.Trim())
                };

                bool sukses = db.ExecuteNonQueryStoredProcedure("sp_InsertDosen", parameters);

                if (sukses)
                {
                    MessageBox.Show("Data Dosen berhasil ditambahkan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                    btnLoad.PerformClick();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUbah_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNIP.Text) || string.IsNullOrEmpty(nipAsli))
            {
                MessageBox.Show("Silakan klik/pilih data dari tabel terlebih dahulu sebelum mengubah!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@NipBaru", txtNIP.Text.Trim()),
                    new SqlParameter("@Nama", txtNama.Text.Trim()),
                    new SqlParameter("@Username", txtUsername.Text.Trim()),
                    new SqlParameter("@Password", txtPassword.Text.Trim()),
                    new SqlParameter("@NipAsli", nipAsli)
                };

                bool sukses = db.ExecuteNonQueryStoredProcedure("sp_UpdateDosen", parameters);
                if (sukses)
                {
                    MessageBox.Show("Data akun dosen berhasil diupdate", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    nipAsli = "";
                    ClearForm();
                    btnLoad.PerformClick();
                }
                else
                {
                    MessageBox.Show("Data dengan NIP tersebut tidak ditemukan atau gagal diperbarui", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNIP.Text))
            {
                MessageBox.Show("Silakan pilih data dosen yang ingin dihapus dari tabel!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult resultConfirm = MessageBox.Show("Yakin ingin menghapus data dosen ini?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultConfirm == DialogResult.Yes)
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@NIP", txtNIP.Text.Trim())
                    };

                    bool sukses = db.ExecuteNonQueryStoredProcedure("sp_DeleteDosen", parameters);

                    if (sukses)
                    {
                        MessageBox.Show("Data berhasil dihapus", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearForm();
                        btnLoad.PerformClick();
                    }
                    else
                    {
                        MessageBox.Show("Data tidak ditemukan", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridView1.Rows.Clear();
                DataGridView1.Columns.Clear();

                DataGridView1.Columns.Add("id_dosen", "ID Dosen");
                DataGridView1.Columns.Add("nip", "NIP");
                DataGridView1.Columns.Add("nama", "Nama");
                DataGridView1.Columns.Add("username", "Username");
                DataGridView1.Columns.Add("password", "Password");

                // Mengambil data tabel menggunakan ExecuteStoredProcedure dari DAL
                DataTable dt = db.ExecuteStoredProcedure("sp_GetAllDosen");

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        DataGridView1.Rows.Add(
                            row["id_dosen"].ToString(),
                            row["nip"].ToString(),
                            row["nama"].ToString(),
                            row["username"].ToString(),
                            row["password"].ToString()
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void dataGridViewDosen_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DataGridView1.Rows[e.RowIndex];
                txtNIP.Text = row.Cells[1].Value?.ToString() ?? "";
                txtNama.Text = row.Cells[2].Value?.ToString() ?? "";
                txtUsername.Text = row.Cells[3].Value?.ToString() ?? "";
                txtPassword.Text = row.Cells[4].Value?.ToString() ?? "";
                nipAsli = txtNIP.Text;
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
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("NIP hanya boleh diisi dengan angka!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtNama_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Nama hanya boleh diisi dengan huruf!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
    }
}