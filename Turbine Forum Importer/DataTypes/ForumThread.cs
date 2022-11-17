using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbine_Forum_Importer.DataTypes
{
    public class ForumThread
    {
        public int Id;
        public string Title;
        //public string Description;

        // Original, base URL of this Thread
        public string Url;

        // ID of the forum this thread is in
        public int Forum;

        public bool Modified = false;

        public string GetSQLStatement()
        {
            string sql = "REPLACE INTO `threads` (`id`, `title`, `forum`, `url`) VALUES ";
            sql += $"({Id}, " +
                $"'{MySqlHelper.EscapeString(Title)}', " +
                $"{Forum}, " +
                $"'{MySqlHelper.EscapeString(Url)}')";
            return sql;
        }

        /// <summary>
        ///  Tries to update the item, if anything has been updated since the version we have stored.
        ///  If anything has changed, then Modified is set to true
        /// </summary>
        /// <param name="newUser"></param>
        public void UpdateThread(ForumThread t)
        {
            // Make sure all the values in p are in this POST
            // Some may be null for whatever reason
        }

    }
}
