using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;

namespace SistemPresensiMahasiswa
{
    public class Connection_DAL_
    {
        private string connectionString;

        public Connection_DAL_()
        {
            // Mengikuti instruksi poin 19.a: Memanggil fungsi untuk mendapatkan IP
            string serverIP = GetIPAddressServer();

            // Menyusun connection string menggunakan IP dinamis hasil fungsi
            connectionString = $"Data Source={serverIP};Initial Catalog=SistemPresensiDB;User ID=sa;Password=LavaIce3115;";
        }

        // Mengikuti instruksi poin 19.b: Fungsi untuk mengambil IP Address
        private string GetIPAddressServer()
        {
            string ipAddress = "";
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddress = ip.ToString();
                    break;
                }
            }
            return ipAddress;
        }

        public SqlConnection GetConn()
        {
            return new SqlConnection(connectionString);
        }

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
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddRange(parameters);
                    }

                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    cmd.Parameters.Clear();
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

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(parameters);

                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    result = cmd.ExecuteNonQuery();

                    cmd.Parameters.Clear();
                }
            }
            return result > 0;
        }
    }
}