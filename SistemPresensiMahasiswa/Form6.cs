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
        // Menggunakan Class DAL tersentralisasi untuk menghapus dependensi connectionString manual
        private Connection_DAL_ db = new Connection_DAL_();

        private string kodeMkAsli = "";

        public KelolaMatKul()
        {
            InitializeComponent();
        }

        // Otomatis load data saat form pertama kali dibuka
        private void KelolaMatKul_Load(object sender, EventArgs e)
        {
            // Konfigurasi awal DataGridView agar lebih rapi dan aman
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            LoadDataMataKuliah();
        }

        // PERBAIKAN: Menggunakan fungsi ExecuteStoredProcedure/ExecuteNonQuery dari DAL
        private void LoadDataMataKuliah()
        {
            try
            {
                dataGridView1.Rows.Clear();

                // Cek apakah kolom sudah dibuat di designer, jika belum baru kita buat lewat kode
                if (dataGridView1.Columns.Count == 0)
                {
                    dataGridView1.Columns.Add("kode_mk", "Kode MK");
                    dataGridView1.Columns.Add("nama_mk", "Nama Mata Kuliah");
                    dataGridView1.Columns.Add("sks", "SKS");
                }

                // REVISI: Jika Anda menggunakan query text, pastikan DAL mendukungnya.
                // Jika DAL Anda mewajibkan Stored Procedure, gantilah string di bawah dengan nama SP Anda (misal: "sp_GetMatakuliah")
                string queryTextOrSP = "SELECT kode_mk, nama_mk, sks FROM Matakuliah";

                DataTable dtMatkul = db.ExecuteStoredProcedure(queryTextOrSP);

                foreach (DataRow row in dtMatkul.Rows)
                {
                    dataGridView1.Rows.Add(
                        row["kode_mk"].ToString(),
                        row["nama_mk"].ToString(),
                        row["sks"].ToString()
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadDataMataKuliah();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKodeMK.Text) ||
                string.IsNullOrWhiteSpace(txtNamaMK.Text) ||
                string.IsNullOrWhiteSpace(txtSKS.Text))
            {
                MessageBox.Show("Semua data (Kode, Nama, SKS) wajib diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // REVISI: Menggunakan query/SP berparameter aman lewat DAL tanpa membuat SqlConnection manual
                // Dianjurkan mengganti query ini dengan nama Stored Procedure (misal: "sp_InsertMatakuliah") jika DAL hanya menerima SP
                string queryTextOrSP = "INSERT INTO Matakuliah (kode_mk, nama_mk, sks) VALUES (@Kode_MK, @Nama_MK, @SKS)";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Kode_MK", txtKodeMK.Text.Trim()),
                    new SqlParameter("@Nama_MK", txtNamaMK.Text.Trim()),
                    new SqlParameter("@SKS", Convert.ToInt32(txtSKS.Text.Trim()))
                };

                db.ExecuteNonQueryStoredProcedure(queryTextOrSP, parameters);

                MessageBox.Show("Data Mata Kuliah berhasil ditambahkan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadDataMataKuliah();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUbah_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKodeMK.Text))
            {
                MessageBox.Show("Pilih data dari tabel yang akan diubah!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(kodeMkAsli))
            {
                MessageBox.Show("Silakan klik/pilih data dari tabel terlebih dahulu sebelum mengubah!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // REVISI: Migrasi ke DAL tersentralisasi berparameter
                // Dianjurkan mengganti query ini dengan nama Stored Procedure (misal: "sp_UpdateMatakuliah") jika diperlukan
                string queryTextOrSP = "UPDATE Matakuliah SET kode_mk = @KodeBaru, nama_mk = @Nama_MK, sks = @SKS WHERE kode_mk = @KodeAsli";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@KodeBaru", txtKodeMK.Text.Trim()),
                    new SqlParameter("@Nama_MK", txtNamaMK.Text.Trim()),
                    new SqlParameter("@SKS", Convert.ToInt32(txtSKS.Text.Trim())),
                    new SqlParameter("@KodeAsli", kodeMkAsli)
                };

                db.ExecuteNonQueryStoredProcedure(queryTextOrSP, parameters);

                MessageBox.Show("Data berhasil diupdate", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                kodeMkAsli = ""; // Reset variabel penanda
                ClearForm();
                LoadDataMataKuliah();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKodeMK.Text))
            {
                MessageBox.Show("Pilih data yang ingin dihapus terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult resultConfirm = MessageBox.Show("Yakin ingin menghapus data mata kuliah ini?", "Konfirmasi Hapus",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultConfirm == DialogResult.Yes)
            {
                try
                {
                    // REVISI: Menghapus data menggunakan eksekusi terpusat DAL berparameter
                    string queryTextOrSP = "DELETE FROM Matakuliah WHERE kode_mk = @Kode_MK";

                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@Kode_MK", txtKodeMK.Text.Trim())
                    };

                    db.ExecuteNonQueryStoredProcedure(queryTextOrSP, parameters);

                    MessageBox.Show("Data berhasil dihapus", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                    LoadDataMataKuliah();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearForm()
        {
            txtKodeMK.Clear();
            txtNamaMK.Clear();
            txtSKS.Clear();
            txtKodeMK.Focus();
        }

        // PERBAIKAN: Mengubah nama event atau memastikan penanganan event klik sel diatur dengan aman
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtKodeMK.Text = row.Cells[0].Value?.ToString() ?? "";
                txtNamaMK.Text = row.Cells[1].Value?.ToString() ?? "";
                txtSKS.Text = row.Cells[2].Value?.ToString() ?? "";

                // Simpan kode asli di sini sebagai acuan klausa WHERE saat update data
                kodeMkAsli = txtKodeMK.Text;
            }
        }

        // Catatan: Jika di designer gridview Anda menggunakan CellContentClick, ganti atau arahkan event-nya ke method di bawah ini
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1_CellClick(sender, e);
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            this.Close();
            DashboardAdmin dashboardAdminForm = new DashboardAdmin();
            dashboardAdminForm.Show();
        }

        // Validasi KeyPress Input SKS
        private void txtSKS_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("SKS hanya boleh diisi dengan angka!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtSKS_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSKS.Text)) return;

            if (int.TryParse(txtSKS.Text, out int nilaiSks))
            {
                if (nilaiSks < 1 || nilaiSks > 6)
                {
                    MessageBox.Show("SKS harus di antara 1 sampai 6!", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSKS.Clear();
                    e.Cancel = true;
                }
            }
        }

        private void txtKodeMK_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Kode Mata Kuliah hanya boleh berisi huruf dan angka tanpa simbol/spasi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtNamaMK_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Nama Mata Kuliah hanya boleh diisi dengan huruf dan spasi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
    }
}