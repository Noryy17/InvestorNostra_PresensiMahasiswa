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
        // REVISI 1: Konsisten menggunakan DAL sebagai pusat kendali query database
        private Connection_DAL_ db = new Connection_DAL_();

        // Variabel penampung parameter filter
        private int _idMatakuliah;
        private int _idDosen;
        private DateTime _tglAwal;
        private DateTime _tglAkhir;

        // Constructor menerima parameter filter dari form pemicu (Form4 / Admin)
        public FormCetak(int idMatakuliah, int idDosen, DateTime tglAwal, DateTime tglAkhir)
        {
            InitializeComponent();
            _idMatakuliah = idMatakuliah;
            _idDosen = idDosen;
            _tglAwal = tglAwal.Date; // Ambil tanggalnya saja (00:00:00)
            _tglAkhir = tglAkhir.Date.AddDays(1).AddTicks(-1); // Set ke akhir hari (23:59:59) agar data di hari terakhir ikut tercetak
        }

        private void FormCetak_Load(object sender, EventArgs e)
        {
            TampilkanLaporan();
        }

        private void TampilkanLaporan()
        {
            try
            {
                // REVISI 2: Menyusun parameter query SQL Injection Protection via DAL
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@idMK", _idMatakuliah),
                    new SqlParameter("@idDosen", _idDosen),
                    new SqlParameter("@tglAwal", _tglAwal),
                    new SqlParameter("@idAkhir", _tglAkhir) // Mengikuti nama parameter tglAkhir
                };
                // Perbaikan typo nama parameter agar sinkron dengan query string
                parameters[3].ParameterName = "@tglAkhir";

                // Eksekusi query via fungsi yang ada di class DAL Anda
                // Catatan: Jika di DAL menggunakan query text biasa, pastikan memanggil fungsi eksekusi text (misal: ExecuteQueryText), 
                // jika menggunakan stored procedure, ganti query string di atas dengan nama SP-nya.
                DataTable dt = db.ExecuteStoredProcedure("sp_GetLaporanPresensi", parameters);

                if (dt != null && dt.Rows.Count > 0)
                {
                    // Konversi DataTable ke List Object Data Source Crystal Report
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

                    // REVISI 3: Proteksi inisialisasi file rpt
                    ReportDocument rpt = new ReportDocument();
                    // Load report langsung dari file rpt proyek agar aman dari masalah caching assembly
                    rpt.Load(Application.StartupPath + @"\LaporanPresensi.rpt");
                    rpt.SetDataSource(listData);

                    // Tampilkan ke dalam komponen Crystal Report Viewer di UI
                    crystalReportViewer1.ReportSource = rpt;
                    crystalReportViewer1.Refresh();
                }
                else
                {
                    MessageBox.Show("Tidak ada data presensi yang ditemukan untuk kriteria ini.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat memuat laporan: " + ex.Message, "Error Sistem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}