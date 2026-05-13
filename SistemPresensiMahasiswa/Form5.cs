using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SistemPresensiMahasiswa
{
    public partial class KelolaMahasiswa : Form
    {

        private BindingSource bindingSource = new BindingSource();
        private DataTable dtMahasiswa = new DataTable();

        private readonly string connectionString =
            "Data Source=LAPTOP-2TIS9UVD\\RIZQIHUDAYA;Initial Catalog=SistemPresensiDB;Integrated Security=True";

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
            try
            {
                if (txtNIM.Text == "") { MessageBox.Show("NIM harus diisi"); txtNIM.Focus(); return; }
                if (txtNama.Text == "") { MessageBox.Show("Nama harus diisi"); txtNama.Focus(); return; }
                if (txtJurusan.Text == "") { MessageBox.Show("Jurusan harus diisi"); txtJurusan.Focus(); return; }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_InsertMahasiswaBaru", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@NIM", txtNIM.Text);
                        cmd.Parameters.AddWithValue("@Nama", txtNama.Text);
                        cmd.Parameters.AddWithValue("@Jurusan", txtJurusan.Text);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Data berhasil ditambahkan");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
            catch (Exception ex)
            {
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
            catch (Exception ex)
            {
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
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
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
        private void lblTotal_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNIM_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void txtNIM_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {

        }

        private void txtNama_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {

        }

        private void txtJurusan_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {

        }
    }
}
