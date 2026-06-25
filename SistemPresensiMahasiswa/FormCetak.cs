using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace SistemPresensiMahasiswa
{
    public partial class FormCetak : Form
    {
        private readonly string connectionString = "Data Source=LAPTOP-DSPPD9L7\\FAIDARYA;Initial Catalog=SistemPresensiDB;Integrated Security=True";

        // Variabel untuk menyimpan parameter filter dari Form4
        private int _idMatakuliah;
        private int _idDosen;
        private DateTime _tglAwal;
        private DateTime _tglAkhir;

        // Constructor ini menerima parameter dari Form4
        public FormCetak(int idMatakuliah, int idDosen, DateTime tglAwal, DateTime tglAkhir)
        {
            InitializeComponent();
            _idMatakuliah = idMatakuliah;
            _idDosen = idDosen;
            _tglAwal = tglAwal;
            _tglAkhir = tglAkhir;
        }

        private void FormCetak_Load(object sender, EventArgs e)
        {
            TampilkanLaporan();
        }

        private void TampilkanLaporan()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Query untuk mengambil data berdasarkan filter
                    string query = @"SELECT p.tanggal AS Tanggal, 
                                            m.nim AS NIM, 
                                            m.nama AS NamaMahasiswa, 
                                            mk.nama_mk AS NamaMatakuliah, 
                                            d.nama AS NamaDosen, 
                                            p.status AS Status 
                                     FROM Presensi p
                                     INNER JOIN Mahasiswa m ON p.id_mahasiswa = m.id_mahasiswa
                                     INNER JOIN Matakuliah mk ON p.id_matakuliah = mk.id_matakuliah
                                     INNER JOIN Dosen d ON p.id_dosen = d.id_dosen
                                     WHERE p.id_matakuliah = @idMK 
                                     AND p.id_dosen = @idDosen 
                                     AND p.tanggal BETWEEN @tglAwal AND @tglAkhir";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idMK", _idMatakuliah);
                        cmd.Parameters.AddWithValue("@idDosen", _idDosen);
                        cmd.Parameters.AddWithValue("@tglAwal", _tglAwal);
                        cmd.Parameters.AddWithValue("@tglAkhir", _tglAkhir);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            // Konversi DataTable ke List object
                            List<LaporanPresensiData> listData = new List<LaporanPresensiData>();

                            foreach (DataRow row in dt.Rows)
                            {
                                listData.Add(new LaporanPresensiData
                                {
                                    Tanggal = Convert.ToDateTime(row["Tanggal"]),
                                    NIM = row["NIM"].ToString(),
                                    NamaMahasiswa = row["NamaMahasiswa"].ToString(),
                                    NamaMatakuliah = row["NamaMatakuliah"].ToString(),
                                    NamaDosen = row["NamaDosen"].ToString(),
                                    Status = row["Status"].ToString()
                                });
                            }

                            // Load Crystal Report
                            LaporanPresensi rpt = new LaporanPresensi();
                            rpt.SetDataSource(listData);

                            // Tampilkan di Viewer
                            crystalReportViewer1.ReportSource = rpt;
                            crystalReportViewer1.Refresh();
                        }
                        else
                        {
                            MessageBox.Show("Tidak ada data presensi yang ditemukan untuk kriteria ini.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close(); // Tutup form cetak jika tidak ada data
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat memuat laporan: " + ex.Message, "Error Sistem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}