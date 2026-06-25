using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemPresensiMahasiswa
{
    public class Connection_DAL_
    {
        // Ubah Data Source sesuai dengan nama Server SQL Server (SSMS) milikmu
        private string connectionString = "Data Source=VICTUS-PUNYA-LU\\LUTFI;Initial Catalog=SistemPresensiDB;Integrated Security=True;";

        public SqlConnection GetConn()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            return conn;
        }

        // ==========================================
        // BONUS: FUNGSI AMAN DARI SQL INJECTION (DAL)
        // ==========================================

        // Fungsi untuk Eksekusi Ambil Data (SELECT) lewat Stored Procedure
        public DataTable ExecuteStoredProcedure(string spName, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = GetConn())
            {
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        // Fungsi untuk Eksekusi Simpan/Ubah/Hapus (CUD) lewat Stored Procedure
        public bool ExecuteNonQueryStoredProcedure(string spName, SqlParameter[] parameters)
        {
            int result = 0;
            using (SqlConnection conn = GetConn())
            {
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }
            return result > 0;
        }
    }
}

