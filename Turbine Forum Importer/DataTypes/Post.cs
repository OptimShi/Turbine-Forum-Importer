using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbine_Forum_Importer.DataTypes
{
    public class Post
    {
        public int Id;
        public int Thread;

        public int User;
        public string Username;
        public bool Guest = false;

        public string HTML;
        public DateTime PostDate;

        public string GetSQLStatement()
        {
            string sql = "";
            return sql;
        }

        public void UpdatePost(Post p)
        {
            // Make sure all the values in p are in this POST
            // Some may be null for whatever reason
        }

    }
}
