using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon
{
    internal class MyConnection
    {
        public static string type;
        
    }
    class ProjectConnection
    {
        public static SqlConnection sqlConn = null;
        public void Connection_Today()
        {
            sqlConn = new SqlConnection("Data Source=WUQLY\\SQLEXPRESS;Initial Catalog=BeautySalonDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True");
        }
    }
}
