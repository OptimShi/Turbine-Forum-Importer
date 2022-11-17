using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;
using System.Xml.Linq;

namespace Turbine_Forum_Importer.DataTypes
{
    public class Forum
    {
        public int Id = 0;
        public string Url = "";
        public string Title = "";
        public string Description = "";
        public int Parent;
        public int SortOrder;

        public bool Modified = false;

        public string GetSQLStatement() {
            string sql = "REPLACE INTO `forums` (`id`, `name`, `description`, `url`) VALUES ";
            sql += $"({Id}, " +
                $"'{MySqlHelper.EscapeString(Title)}', " +
                $"'{MySqlHelper.EscapeString(Description)}', " +
                $"'{MySqlHelper.EscapeString(Url)}')";
            return sql;
        }

        /// <summary>
        ///  Tries to update the item, if anything has been updated since the version we have stored.
        ///  If anything has changed, then Modified is set to true
        /// </summary>
        /// <param name="newUser"></param>
        public void UpdateForum(Forum f)
        {
            // Make sure all the values in p are in this POST
            // Some may be null for whatever reason
        }

    }
}
