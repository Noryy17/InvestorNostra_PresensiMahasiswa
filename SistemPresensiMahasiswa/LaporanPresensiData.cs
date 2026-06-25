using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemPresensiMahasiswa
{
    public class LaporanPresensiData
    {
        public DateTime Tanggal { get; set; }
        public string NIM { get; set; }
        public string NamaMahasiswa { get; set; }
        public string NamaMatakuliah { get; set; }
        public string NamaDosen { get; set; }
        public string Status { get; set; }
    }
}
