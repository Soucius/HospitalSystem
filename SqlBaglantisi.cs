using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Proje_Hastane
{
    class SqlBaglantisi
    {
        string dbAddress = "Data Source=DESKTOP-UAP88AC\\SQLEXPRESS;Initial Catalog=HastaneProje;Integrated Security=True";

        public SqlConnection baglanti()
        {
            SqlConnection baglan = new SqlConnection(dbAddress);
            baglan.Open();
            return baglan;
        }
    }
}
