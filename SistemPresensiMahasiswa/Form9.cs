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
    public partial class RekapPresensi : Form
    {

        private readonly SqlConnection conn;
        private readonly string connectionString =
        "Data Source=VICTUS-PUNYA-LU\\LUTFI;Initial Catalog=SistemPresensiDB;Integrated Security=True";
        public RekapPresensi()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            this.Close();
            DashboardDosen dashboardDosen = new DashboardDosen();
            dashboardDosen.Show();
        }

        private void btnRekap_Click(object sender, EventArgs e)
        {
            // Validasi agar user memilih mata kuliah dulu
            if (cbMatakuliah.SelectedValue == null)
            {
                MessageBox.Show("Silakan pilih Mata Kuliah terlebih dahulu!");
                return;
            }

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                // Query menghitung status per mahasiswa untuk MK tertentu
                string query = @"SELECT 
                            m.nim, 
                            m.nama,
                            COUNT(CASE WHEN p.status = 'Hadir' THEN 1 END) AS Hadir,
                            COUNT(CASE WHEN p.status = 'Izin' THEN 1 END) AS Izin,
                            COUNT(CASE WHEN p.status = 'Sakit' THEN 1 END) AS Sakit,
                            COUNT(CASE WHEN p.status = 'Alpa' THEN 1 END) AS Alpa
                         FROM Mahasiswa m
                         INNER JOIN Presensi p ON m.id_mahasiswa = p.id_mahasiswa
                         WHERE p.id_matakuliah = @idMK 
                         AND p.tanggal BETWEEN @tglAwal AND @tglAkhir
                         GROUP BY m.nim, m.nama
                         ORDER BY m.nim ASC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idMK", cbMatakuliah.SelectedValue);
                cmd.Parameters.AddWithValue("@tglAwal", dtpAwal.Value.Date);
                cmd.Parameters.AddWithValue("@tglAkhir", dtpAkhir.Value.Date);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Tidak ada data presensi untuk periode dan mata kuliah ini.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }


        private void LoadMatakuliah()
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                // Ambil ID untuk nilai sistem, dan Nama untuk tampilan user
                string query = "SELECT id_matakuliah, nama_mk FROM Matakuliah";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Contoh urutan yang lebih aman:
                cbMatakuliah.DisplayMember = "nama_mk";
                cbMatakuliah.ValueMember = "id_matakuliah";
                cbMatakuliah.DataSource = dt; // DataSource terakhir
                cbMatakuliah.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat mata kuliah: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void FormRekapPresensi_Load(object sender, EventArgs e)
        {
            LoadMatakuliah(); // Mengisi cbMatakuliah dengan daftar pelajaran
            dtpAwal.Value = DateTime.Now.AddMonths(-1); // Default 1 bulan terakhir
            dtpAkhir.Value = DateTime.Now;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cbMatakuliah.SelectedIndex = -1;
            dtpAwal.Value = DateTime.Now.AddMonths(-1);
            dtpAkhir.Value = DateTime.Now;
            dataGridView1.DataSource = null; // Menghapus hasil rekap di tabel
        }
    }
}
