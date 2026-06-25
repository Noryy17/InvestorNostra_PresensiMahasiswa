using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ExcelDataReader;

namespace SistemPresensiMahasiswa
{
    public partial class KelolaMahasiswa : Form
    {
        private BindingSource bindingSource = new BindingSource();
        private DataTable dtMahasiswa = new DataTable();

        // Memanggil Class DAL tersentralisasi untuk menghapus dependensi connectionString
        private Connection_DAL_ db = new Connection_DAL_();
        private string excelFilePath = "";

        private void SimpanLog(string pesanError)
        {
            try
            {
                // PERBAIKAN: Disarankan menggunakan Stored Procedure untuk log demi keamanan arsitektur DAL
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@pPesan", pesanError)
                };

                db.ExecuteNonQueryStoredProcedure("sp_InsertLogError", parameters);
            }
            catch
            {
                // Di-ignore aman jika koneksi bermasalah
            }
        }

        public KelolaMahasiswa()
        {
            InitializeComponent();
        }

        private void KelolaMahasiswa_Load(object sender, EventArgs e)
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

            // Otomatis update foto setiap kali baris/posisi data bergeser
            bindingSource.PositionChanged += BindingSource_PositionChanged;

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                dtMahasiswa = db.ExecuteStoredProcedure("sp_GetMahasiswa");

                bindingSource.DataSource = dtMahasiswa;
                dataGridView1.DataSource = bindingSource;

                BindControls();
                TampilkanFotoMahasiswa();
                HitungTotal();
            }
            catch (Exception ex)
            {
                SimpanLog("Gagal Load Data: " + ex.Message);
                MessageBox.Show("Gagal load data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void TampilkanFotoMahasiswa()
        {
            try
            {
                if (bindingSource.Current != null)
                {
                    DataRowView currentView = (DataRowView)bindingSource.Current;
                    DataRow row = currentView.Row;

                    if (dtMahasiswa.Columns.Contains("foto") && row["foto"] != DBNull.Value)
                    {
                        byte[] imgBytes = (byte[])row["foto"];
                        using (MemoryStream ms = new MemoryStream(imgBytes))
                        {
                            pictureBoxFoto.SizeMode = PictureBoxSizeMode.Zoom;

                            // Bungkus dengan new Bitmap agar tidak mengunci stream asli
                            using (Image tempImg = Image.FromStream(ms))
                            {
                                pictureBoxFoto.Image = new Bitmap(tempImg);
                            }
                        }
                    }
                    else
                    {
                        pictureBoxFoto.Image = null;
                    }
                }
                else
                {
                    pictureBoxFoto.Image = null;
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
            if (string.IsNullOrEmpty(txtNIM.Text)) { MessageBox.Show("NIM harus diisi", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNIM.Focus(); return; }
            if (string.IsNullOrEmpty(txtNama.Text)) { MessageBox.Show("Nama harus diisi", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNama.Focus(); return; }
            if (string.IsNullOrEmpty(txtJurusan.Text)) { MessageBox.Show("Jurusan harus diisi", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtJurusan.Focus(); return; }

            try
            {
                object fotoParamValue = DBNull.Value;

                if (pictureBoxFoto.Image != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        pictureBoxFoto.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        fotoParamValue = ms.ToArray();
                    }
                }

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@pNIM", txtNIM.Text.Trim()),
                    new SqlParameter("@pNama", txtNama.Text.Trim()),
                    new SqlParameter("@pJurusan", txtJurusan.Text.Trim()),
                    new SqlParameter("@pFoto", fotoParamValue)
                };

                db.ExecuteNonQueryStoredProcedure("sp_InsertMahasiswaBaru", parameters);

                MessageBox.Show("Data dan Foto berhasil ditambahkan!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                SimpanLog("Error Insert: " + ex.Message);
                MessageBox.Show("Gagal menyimpan data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUbah_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNIM.Text) || string.IsNullOrEmpty(txtNama.Text) || string.IsNullOrEmpty(txtJurusan.Text))
            {
                MessageBox.Show("NIM, Nama, dan Jurusan tidak boleh kosong!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                byte[] fotoBytes = null;
                if (pictureBoxFoto.Image != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Bitmap bmp = new Bitmap(pictureBoxFoto.Image);
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        fotoBytes = ms.ToArray();
                    }
                }

                SqlParameter[] parameters = new SqlParameter[]
                {
                     new SqlParameter("@pNIM", txtNIM.Text.Trim()),
                     new SqlParameter("@pNama", txtNama.Text.Trim()),
                     new SqlParameter("@pJurusan", txtJurusan.Text.Trim()),
                     new SqlParameter("@pFoto", (object)fotoBytes ?? DBNull.Value)
                };

                db.ExecuteNonQueryStoredProcedure("sp_UpdateMahasiswa", parameters);
                MessageBox.Show("Data mahasiswa berhasil diperbarui!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memperbarui data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNIM.Text)) return;

            try
            {
                DialogResult resultConfirm = MessageBox.Show(
                    "Yakin ingin menghapus data mahasiswa ini? Seluruh riwayat presensi terkait juga akan terhapus.",
                    "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resultConfirm == DialogResult.Yes)
                {
                    SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@pNIM", txtNIM.Text) };
                    db.ExecuteNonQueryStoredProcedure("sp_DeleteMahasiswa", parameters);

                    MessageBox.Show("Data berhasil dihapus.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                SimpanLog("Error Delete: " + ex.Message);
                MessageBox.Show("Gagal menghapus data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult confirm = MessageBox.Show("Yakin ingin mereset data ke cadangan semula?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    db.ExecuteNonQueryStoredProcedure("sp_ResetData", new SqlParameter[] { });
                    MessageBox.Show("Data berhasil direset ke cadangan semula.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
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
                // PERBAIKAN: Hindari raw text SQL jika DAL mewajibkan Stored Procedure
                SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@pNIM", txtNIM.Text) };
                db.ExecuteNonQueryStoredProcedure("sp_SimulasiHackedNama", parameters);

                MessageBox.Show("Eksperimen selesai.", "Eksperimen SQLi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Aktivitas Ditolak Keamanan Database! \n\nDetail: " + ex.Message, "Proteksi Aktif", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void HitungTotal()
        {
            try
            {
                SqlParameter outputParam = new SqlParameter("@pCount", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                SqlParameter[] parameters = new SqlParameter[] { outputParam };
                db.ExecuteNonQueryStoredProcedure("sp_CountMahasiswa", parameters);

                if (outputParam.Value != DBNull.Value && outputParam.Value != null)
                {
                    lblTotal.Text = "Total Mahasiswa: " + outputParam.Value.ToString();
                }
                else
                {
                    lblTotal.Text = "Total Mahasiswa: 0";
                }
            }
            catch (Exception ex)
            {
                SimpanLog("Error HitungTotal: " + ex.Message);
                lblTotal.Text = "Total Mahasiswa: -";
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Sinkronisasi murni diatur aman oleh BindingSource_PositionChanged
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

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Excel Files|*.xlsx;*.xls";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    excelFilePath = ofd.FileName;
                    txtFilePath.Text = excelFilePath;
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(excelFilePath))
            {
                MessageBox.Show("Silakan pilih file Excel melalui tombol Browse!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                        });

                        DataTable dtExcel = result.Tables[0];
                        int barisBerhasil = 0;
                        int barisGagal = 0;

                        foreach (DataRow row in dtExcel.Rows)
                        {
                            if (row[0] == DBNull.Value || string.IsNullOrEmpty(row[0].ToString())) continue;

                            try
                            {
                                // PERBAIKAN: Parameter disamakan memakai '@p' sesuai sp_InsertMahasiswaBaru
                                SqlParameter[] parameters = new SqlParameter[]
                                {
                                    new SqlParameter("@pNIM", row[0].ToString().Trim()),
                                    new SqlParameter("@pNama", row[1].ToString().Trim()),
                                    new SqlParameter("@pJurusan", row[2].ToString().Trim()),
                                    new SqlParameter("@pFoto", DBNull.Value)
                                };

                                db.ExecuteNonQueryStoredProcedure("sp_InsertMahasiswaBaru", parameters);
                                barisBerhasil++;
                            }
                            catch
                            {
                                // Jika ada row yang duplikat NIM-nya, skip dan lanjut ke baris berikutnya
                                barisGagal++;
                            }
                        }

                        MessageBox.Show($"{barisBerhasil} data berhasil diimport. Gagal/Duplikat: {barisGagal}", "Hasil Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                SimpanLog("Gagal Import Excel: " + ex.Message);
                MessageBox.Show("Gagal melakukan import data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtNIM_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtNama_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtJurusan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
    }
}