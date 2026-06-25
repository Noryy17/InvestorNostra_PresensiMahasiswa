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
    public partial class InputPresensi : Form
    {
        // Menggunakan arsitektur DAL yang sudah terpusat
        private Connection_DAL_ db = new Connection_DAL_();

        public InputPresensi()
        {
            InitializeComponent();
        }

        private void InputPresensi_Load(object sender, EventArgs e)
        {
            // Mengisi pilihan Status secara manual sesuai aturan CHECK di database
            cbStatus.Items.Clear();
            cbStatus.Items.Add("Hadir");
            cbStatus.Items.Add("Izin");
            cbStatus.Items.Add("Sakit");
            cbStatus.Items.Add("Alpa");

            // Memuat semua komponen data master ke ComboBox via DAL
            LoadMatakuliah();
            LoadDosen();
            LoadMahasiswa();

            // Tampilkan data tabel presensi saat form pertama kali dibuka
            RefreshTable();
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            this.Close();
            DashboardDosen dashboardDosen = new DashboardDosen();
            dashboardDosen.Show();
        }

        private void LoadDosen()
        {
            try
            {
                // Menggunakan Stored Procedure sp_GetLookupDosen yang sudah Anda buat di database
                DataTable dt = db.ExecuteStoredProcedure("sp_GetLookupDosen", null);

                cbDosen.DataSource = dt;
                cbDosen.DisplayMember = "nama";
                cbDosen.ValueMember = "id_dosen";
                cbDosen.SelectedIndex = -1; // Default kosong
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat daftar dosen: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMatakuliah()
        {
            try
            {
                // Menggunakan Stored Procedure sp_GetLookupMatakuliah yang sudah Anda buat di database
                DataTable dt = db.ExecuteStoredProcedure("sp_GetLookupMatakuliah", null);

                cbMatakuliah.DataSource = dt;
                cbMatakuliah.DisplayMember = "nama_mk";
                cbMatakuliah.ValueMember = "id_matakuliah";
                cbMatakuliah.SelectedIndex = -1; // Default kosong
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat mata kuliah: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMahasiswa()
        {
            try
            {
                // Pastikan sp_GetMahasiswa mengembalikan kolom 'id_mahasiswa' dan 'nama'
                DataTable dt = db.ExecuteStoredProcedure("sp_GetMahasiswa", null);

                cbMahasiswa.DataSource = dt;
                cbMahasiswa.DisplayMember = "nama";

                // PERBAIKAN: Gunakan 'id_mahasiswa' sebagai ValueMember, bukan 'nim'
                // Pastikan sp_GetMahasiswa Anda mencakup kolom id_mahasiswa
                cbMahasiswa.ValueMember = "id_mahasiswa";
                cbMahasiswa.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat mahasiswa: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnInput_Click(object sender, EventArgs e)
        {
            if (cbMatakuliah.SelectedValue == null || cbDosen.SelectedValue == null ||
                cbMahasiswa.SelectedValue == null || cbStatus.SelectedItem == null)
            {
                MessageBox.Show("Semua kolom harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@tanggal", dtpTanggal.Value.Date),
                    new SqlParameter("@status", cbStatus.SelectedItem.ToString()),
                    // Sekarang ini akan mengambil id_mahasiswa yang benar (Integer)
                    new SqlParameter("@id_mahasiswa", Convert.ToInt32(cbMahasiswa.SelectedValue)),
                    new SqlParameter("@id_mk", Convert.ToInt32(cbMatakuliah.SelectedValue)),
                    new SqlParameter("@id_dosen", Convert.ToInt32(cbDosen.SelectedValue))
                };

                bool result = db.ExecuteNonQueryStoredProcedure("sp_InsertPresensi", parameters);

                if (result)
                {
                    MessageBox.Show("Presensi berhasil disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnClearForm_Click(sender, e);
                    RefreshTable();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menyimpan data ke database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // ... log error
            }
        }
        private void RefreshTable()
        {
            try
            {
                // Menampilkan riwayat data terbaru melalui DAL
                string query = @"SELECT p.tanggal AS [Tanggal], m.nim AS [NIM], m.nama AS [Nama Mahasiswa], p.status AS [Status] 
                                 FROM Presensi p 
                                 JOIN Mahasiswa m ON p.id_mahasiswa = m.id_mahasiswa 
                                 ORDER BY p.tanggal DESC, m.nim ASC";

                DataTable dt = db.ExecuteStoredProcedure(query, null);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Refresh Table: " + ex.Message);
            }
        }

        private void btnClearForm_Click(object sender, EventArgs e)
        {
            cbMatakuliah.SelectedIndex = -1;
            cbDosen.SelectedIndex = -1;
            cbMahasiswa.SelectedIndex = -1;
            cbStatus.SelectedIndex = -1;
            dtpTanggal.Value = DateTime.Today;
            cbMatakuliah.Focus();
        }
    }
}