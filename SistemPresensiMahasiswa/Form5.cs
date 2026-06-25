using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SistemPresensiMahasiswa
{
    public partial class KelolaMahasiswa : Form
    {
        private BindingSource bindingSource = new BindingSource();
        private DataTable dtMahasiswa = new DataTable();

        // Inisialisasi object Class Data Access Layer (DAL)
        private Connection_DAL_ db = new Connection_DAL_();

        // Biarkan connection string ini sesuai dengan laptopmu (Dipakai khusus untuk transaksi internal)
        private readonly string connectionString =
            "Data Source=VICTUS-PUNYA-LU\\LUTFI;Initial Catalog=SistemPresensiDB;Integrated Security=True";

        // =================================================================
        // METHOD LOGGING (Berdasarkan Modul Praktikum 11)
        // =================================================================
        private void SimpanLog(string pesanError)
        {
            try
            {
                string queryLog = "INSERT INTO LogError (waktu, pesan_error) VALUES (GETDATE(), @pesan)";
                // Menggunakan koneksi langsung via string khusus untuk background logging
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmdLog = new SqlCommand(queryLog, conn))
                    {
                        cmdLog.Parameters.AddWithValue("@pesan", pesanError);
                        conn.Open();
                        cmdLog.ExecuteNonQuery();
                    }
                }
            }
            catch
            { // Di-ignore agar tidak memicu loop exception jika log gagal 
            }
        }

        public KelolaMahasiswa()
        {
            InitializeComponent();
        }

        private void KelolaMahasiswa_Load_1(object sender, EventArgs e)
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            ColNim.DataPropertyName = "nim";
            ColNamaMahasiswa.DataPropertyName = "nama";
            ColJurusan.DataPropertyName = "jurusan";

            bindingNavigator1.BindingSource = bindingSource;

            // Hubungkan event ketika baris data berubah agar foto ikut ter-refresh
            bindingSource.PositionChanged += BindingSource_PositionChanged;

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // MENGGUNAKAN DAL: Memanggil sp_GetMahasiswa tanpa menulis ulang SqlConnection manual
                dtMahasiswa = db.ExecuteStoredProcedure("sp_GetMahasiswa");

                bindingSource.DataSource = dtMahasiswa;
                dataGridView1.DataSource = bindingSource;

                BindControls();
                TampilkanFotoMahasiswa(); // Update tampilan foto untuk baris pertama
                HitungTotal();
            }
            catch (Exception ex)
            {
                SimpanLog("Gagal Load Data: " + ex.Message);
                MessageBox.Show("Gagal load data: " + ex.Message);
            }
        }

        private void BindControls()
        {
            txtNIM.DataBindings.Clear();
            txtNama.DataBindings.Clear();
            txtJurusan.DataBindings.Clear();

            txtNIM.DataBindings.Add("Text", bindingSource, "nim");
            txtNama.DataBindings.Add("Text", bindingSource, "nama");
            txtJurusan.DataBindings.Add("Text", bindingSource, "jurusan");
        }

        // Method pendukung untuk memunculkan gambar dari data biner (BLOB) database
        private void TampilkanFotoMahasiswa()
        {
            try
            {
                if (bindingSource.Current != null)
                {
                    DataRowView currentView = (DataRowView)bindingSource.Current;
                    DataRow row = currentView.Row;

                    // Memeriksa apakah kolom foto tersedia di DataTable dan tidak bernilai NULL
                    if (dtMahasiswa.Columns.Contains("foto") && row["foto"] != DBNull.Value)
                    {
                        byte[] imgBytes = (byte[])row["foto"];
                        using (MemoryStream ms = new MemoryStream(imgBytes))
                        {
                            pictureBoxFoto.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBoxFoto.Image = Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        pictureBoxFoto.Image = null; // Kosongkan box jika mahasiswa tidak punya foto
                    }
                }
            }
            catch (Exception ex)
            {
                pictureBoxFoto.Image = null;
                SimpanLog("Error Render Image: " + ex.Message);
            }
        }

        private void BindingSource_PositionChanged(object sender, EventArgs e)
        {
            TampilkanFotoMahasiswa();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (txtNIM.Text == "") { MessageBox.Show("NIM harus diisi"); txtNIM.Focus(); return; }
            if (txtNama.Text == "") { MessageBox.Show("Nama harus diisi"); txtNama.Focus(); return; }
            if (txtJurusan.Text == "") { MessageBox.Show("Jurusan harus diisi"); txtJurusan.Focus(); return; }

            // Menggunakan transaksi manual di tombol ini (Sesuai syarat TCL di UCP)
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_InsertMahasiswaBaru", conn, trans))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@NIM", txtNIM.Text);
                        cmd.Parameters.AddWithValue("@Nama", txtNama.Text);
                        cmd.Parameters.AddWithValue("@Jurusan", txtJurusan.Text);

                        // KODE UPLOAD FOTO (BLOB)
                        if (pictureBoxFoto.Image != null)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                pictureBoxFoto.Image.Save(ms, pictureBoxFoto.Image.RawFormat);
                                cmd.Parameters.AddWithValue("@Foto", ms.ToArray());
                            }
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Foto", DBNull.Value);
                        }

                        cmd.ExecuteNonQuery();
                    }

                    trans.Commit();
                    MessageBox.Show("Data dan Foto berhasil ditambahkan! (TCL Commit Sukses)", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (SqlException sqlEx)
                {
                    trans.Rollback();
                    SimpanLog("SQL Error Insert (Rollback): " + sqlEx.Message);
                    MessageBox.Show("Gagal menyimpan data (TCL Rollback Aktif): \n" + sqlEx.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    SimpanLog("App Error Insert (Rollback): " + ex.Message);
                    MessageBox.Show("Terjadi kesalahan sistem (TCL Rollback Aktif): " + ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            LoadData();
        }

        private void btnUbah_Click(object sender, EventArgs e)
        {
            try
            {
                // Konversi foto baru jika ada perubahan
                byte[] fotoBlob = null;
                if (pictureBoxFoto.Image != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        pictureBoxFoto.Image.Save(ms, pictureBoxFoto.Image.RawFormat);
                        fotoBlob = ms.ToArray();
                    }
                }

                // MENGGUNAKAN DAL: Menyusun parameter secara terstruktur dan aman dari SQL Injection
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@NIM", txtNIM.Text),
                    new SqlParameter("@Nama", txtNama.Text),
                    new SqlParameter("@Jurusan", txtJurusan.Text),
                    new SqlParameter("@Foto", (object)fotoBlob ?? DBNull.Value)
                };

                bool isSuccess = db.ExecuteNonQueryStoredProcedure("sp_UpdateMahasiswa", parameters);

                if (isSuccess)
                {
                    MessageBox.Show("Data dan Foto Mahasiswa berhasil diperbarui!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Gagal memperbarui data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException sqlEx)
            {
                SimpanLog("SQL Error Update: " + sqlEx.Message);
                MessageBox.Show("Gagal mengupdate data: " + sqlEx.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                SimpanLog("App Error Update: " + ex.Message);
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult resultConfirm = MessageBox.Show(
                    "Yakin ingin menghapus data mahasiswa ini? Seluruh riwayat presensi terkait juga akan terhapus.",
                    "Konfirmasi Hapus",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultConfirm == DialogResult.Yes)
                {
                    // MENGGUNAKAN DAL: Pemanggilan SP Delete secara ringkas
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@NIM", txtNIM.Text)
                    };

                    db.ExecuteNonQueryStoredProcedure("sp_DeleteMahasiswa", parameters);

                    MessageBox.Show("Data berhasil dihapus dari sistem.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
            }
            catch (SqlException sqlEx)
            {
                SimpanLog("SQL Error Delete: " + sqlEx.Message);
                MessageBox.Show("Gagal menghapus data: " + sqlEx.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                SimpanLog("App Error Delete: " + ex.Message);
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                string query = @"
                IF OBJECT_ID('dbo.Mahasiswa_Backup') IS NOT NULL
                BEGIN
                    DELETE FROM dbo.Presensi;
                    DELETE FROM dbo.KRS;
                    DELETE FROM dbo.Mahasiswa;

                    SET IDENTITY_INSERT dbo.Mahasiswa ON;
                    INSERT INTO dbo.Mahasiswa (id_mahasiswa, nim, nama, jurusan)
                    SELECT id_mahasiswa, nim, nama, jurusan FROM dbo.Mahasiswa_Backup;
                    SET IDENTITY_INSERT dbo.Mahasiswa OFF;
                END";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Data berhasil direset ke kondisi cadangan semula.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Reset gagal: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnInject_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Kueri sengaja dibuat rentan untuk demonstrasi pengujian proteksi di hadapan Asdos
                    string query = "UPDATE Mahasiswa SET Nama='HACKED' WHERE NIM='" + txtNIM.Text + "'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        int result = cmd.ExecuteNonQuery();
                        MessageBox.Show(result + " baris terupdate. Sistem Anda ternyata rentan SQL Injection!", "Eksperimen SQLi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                LoadData();
            }
            catch (SqlException ex)
            {
                // Trigger trg_PreventMassUpdate di SSMS akan melempar error ke blok catch ini
                MessageBox.Show("Aktivitas Masal Ditolak Keamanan Database! \n\nDetail Pemicu: " + ex.Message, "Proteksi Trigger Aktif", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void HitungTotal()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_CountMahasiswa", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter outputParam = new SqlParameter("@Total", SqlDbType.Int);
                        outputParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(outputParam);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        lblTotal.Text = "Total Mahasiswa: " + outputParam.Value.ToString();
                    }
                }
            }
            catch
            {
                lblTotal.Text = "Total Mahasiswa: -";
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                bindingSource.Position = e.RowIndex;
            }
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            this.Close();
            DashboardAdmin dashboardAdmin = new DashboardAdmin();
            dashboardAdmin.Show();
        }

        private void ClearForm()
        {
            txtNIM.Clear();
            txtNama.Clear();
            txtJurusan.Clear();
            pictureBoxFoto.Image = null;
            txtNIM.Focus();
        }

        private void UploadFoto_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog opnfd = new OpenFileDialog())
            {
                opnfd.Filter = "Image Files (*.jpg;*.jpeg;*.png;)|*.jpg;*.jpeg;*.png;";

                if (opnfd.ShowDialog() == DialogResult.OK)
                {
                    pictureBoxFoto.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBoxFoto.Image = new Bitmap(opnfd.FileName);
                }
            }
        }

        // Placeholder events
        private void lblTotal_Click(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void txtNIM_TextChanged(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void txtNIM_KeyPress(object sender, KeyPressEventArgs e) { }
        private void txtNama_KeyPress(object sender, KeyPressEventArgs e) { }
        private void txtJurusan_KeyPress(object sender, KeyPressEventArgs e) { }
    }
}