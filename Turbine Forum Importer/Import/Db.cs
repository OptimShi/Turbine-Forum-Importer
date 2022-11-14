using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;

namespace Turbine_Forum_Importer.Import
{
    public class Db
    {
        MySqlConnection Connection;

        public Db()
        {
            string dbhost = "localhost";
            string dbuser = "root";
            string dbpassword = "";
            string dbname = "turbine_forums";
            Connection = new MySqlConnection($"SERVER={dbhost}; user id={dbuser}; password={dbpassword}; database={dbname}");
            Connection.Open();

            //RunSQL("do a broken thing");
            RunSQL("select * from threads where id < 2");
            //RunSQL("select version()");
        }

        public bool RunSQL(string command)
        {
            var cmd = new MySqlCommand(command, Connection);

            var result = cmd.ExecuteScalar();
            return true;
        }


    }
}
