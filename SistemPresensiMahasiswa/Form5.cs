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

        // Biarkan connection string ini sesuai dengan laptopmu
        private readonly string connectionString =
      "Data Source=LAPTOP-DSPPD9L7\\FAIDARYA;Initial Catalog=SistemPresensiDB;Integrated Security=True";

        // =================================================================
        // KODE FASE 2: METHOD LOGGING (Berdasarkan Modul Praktikum 11)
        // =================================================================
        private void SimpanLog(string pesanError)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string queryLog = "INSERT INTO LogError (waktu, pesan_error) VALUES (GETDATE(), @pesan)";
                using (SqlCommand cmdLog = new SqlCommand(queryLog, conn))
                {
                    cmdLog.Parameters.AddWithValue("@pesan", pesanError);
                    conn.Open();
                    cmdLog.ExecuteNonQuery();
                }
            }
        }
        // =================================================================

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

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_GetMahasiswa", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            dtMahasiswa = new DataTable();
                            da.Fill(dtMahasiswa);

                            bindingSource.DataSource = dtMahasiswa;
                            dataGridView1.DataSource = bindingSource;

                            BindControls();
                        }
                    }
                }

                HitungTotal();
            }
            catch (Exception ex)
            {
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

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (txtNIM.Text == "") { MessageBox.Show("NIM harus diisi"); txtNIM.Focus(); return; }
            if (txtNama.Text == "") { MessageBox.Show("Nama harus diisi"); txtNama.Focus(); return; }
            if (txtJurusan.Text == "") { MessageBox.Show("Jurusan harus diisi"); txtJurusan.Focus(); return; }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // =======================================================
                // IMPLEMENTASI TCL (TRANSACTION MANAGEMENT)
                // =======================================================
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_InsertMahasiswaBaru", conn, trans))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@NIM", txtNIM.Text);
                        cmd.Parameters.AddWithValue("@Nama", txtNama.Text);
                        cmd.Parameters.AddWithValue("@Jurusan", txtJurusan.Text);

                        // =======================================================
                        // KODE UPLOAD FOTO (BLOB) 
                        // =======================================================
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
                            // Jika user tidak memilih foto, biarkan kosong di database
                            cmd.Parameters.AddWithValue("@Foto", DBNull.Value);
                        }
                        // =======================================================

                        cmd.ExecuteNonQuery();
                    }

                    trans.Commit(); // Jika semua proses (termasuk foto) sukses, simpan!
                    MessageBox.Show("Data dan Foto berhasil ditambahkan (TCL Commit Sukses)");
                }
                catch (SqlException sqlEx)
                {
                    trans.Rollback(); // Jika error, batalkan semua!
                    SimpanLog("SQL Error Insert (Rollback): " + sqlEx.Message);
                    MessageBox.Show("Gagal menyimpan data (TCL Rollback Aktif): " + sqlEx.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    trans.Rollback(); // Jika aplikasi crash, batalkan semua!
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
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_UpdateMahasiswa", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@NIM", txtNIM.Text);
                        cmd.Parameters.AddWithValue("@Nama", txtNama.Text);
                        cmd.Parameters.AddWithValue("@Jurusan", txtJurusan.Text);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Data berhasil diupdate");
                LoadData();
            }
            catch (SqlException sqlEx)
            {
                SimpanLog("SQL Error Update: " + sqlEx.Message);
                MessageBox.Show("Gagal mengupdate data: " + sqlEx.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                SimpanLog("App Error Update: " + ex.Message);
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult resultConfirm = MessageBox.Show(
                  "Yakin ingin menghapus data?",
                  "Konfirmasi",
                  MessageBoxButtons.YesNo,
                  MessageBoxIcon.Question);

                if (resultConfirm == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand("sp_DeleteMahasiswa", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@NIM", txtNIM.Text);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Data berhasil dihapus");
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
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                IF OBJECT_ID('dbo.Mahasiswa_Backup') IS NOT NULL
                BEGIN
                    -- Hapus dulu data yang referensi Mahasiswa
                    DELETE FROM dbo.Presensi;
                    DELETE FROM dbo.KRS;
                    DELETE FROM dbo.Mahasiswa;

                    SET IDENTITY_INSERT dbo.Mahasiswa ON;
                    INSERT INTO dbo.Mahasiswa (id_mahasiswa, nim, nama, jurusan)
                    SELECT id_mahasiswa, nim, nama, jurusan FROM dbo.Mahasiswa_Backup;
                    SET IDENTITY_INSERT dbo.Mahasiswa OFF;
                END";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Data berhasil direset");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Reset gagal: " + ex.Message);
            }
        }

        private void btnInject_Click(object sender, EventArgs e)
        {
            // =================================================================
            // KODE FASE 2: UJI COBA SQL INJECTION DAN TRIGGER KEAMANAN
            // =================================================================
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Kueri rentan ini dibiarkan untuk mendemonstrasikan kelemahan
                    string query =
            "UPDATE Mahasiswa SET Nama='HACKED' WHERE NIM='" +
            txtNIM.Text + "'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        int result = cmd.ExecuteNonQuery();
                        MessageBox.Show(result + " baris terupdate");
                    }
                }

                LoadData();
            }
            catch (SqlException ex)
            {
                // Jika trigger SQL Server menggagalkan mass update, error-nya ditangkap di sini
                MessageBox.Show("Aktivitas Ditolak oleh Sistem Keamanan: \n\n" + ex.Message, "Keamanan Aktif", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            // =================================================================
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
            catch (Exception)
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
            txtNIM.Focus();
        }

        private void lblTotal_Click(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void txtNIM_TextChanged(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void txtNIM_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) { }
        private void txtNama_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) { }
        private void txtJurusan_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) { }

        private void UploadFoto_Click(object sender, EventArgs e)
        {
            // Membuka jendela dialog untuk memilih file
            OpenFileDialog opnfd = new OpenFileDialog();
            opnfd.Filter = "Image Files (*.jpg;*.jpeg;*.png;)|*.jpg;*.jpeg;*.png;";

            if (opnfd.ShowDialog() == DialogResult.OK)
            {
                // KODE SAKTI: Memaksa gambar menyesuaikan kotak (Zoom) dengan rapi
                pictureBoxFoto.SizeMode = PictureBoxSizeMode.Zoom;

                // Menampilkan gambar ke layar
                pictureBoxFoto.Image = new Bitmap(opnfd.FileName);
            }
        }
    }
}