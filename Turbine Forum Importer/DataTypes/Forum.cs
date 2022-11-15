using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbine_Forum_Importer.DataTypes
{
    public class Forum
    {
        public int Id = 0;
        public string Url;
        public string Title;
        public string Description;
        public int Parent;
        public int SortOrder;

        public string GetSQLStatement() {
            string sql = "";
            return sql;
        }

        public void UpdateForum(Forum f)
        {
            // Make sure all the values in p are in this POST
            // Some may be null for whatever reason
        }

    }
}
