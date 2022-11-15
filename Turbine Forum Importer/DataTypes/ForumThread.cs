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

        // Original, base URL of this Thread
        public string URL;

        // ID of the forum this thread is in
        public int Forum; 

        public string GetSQLStatement()
        {
            //$sql = "REPLACE INTO `threads` (`id`, `title`, `forum`, `url`) VALUES
            //              ({$thread['id']}, '{$threadTitle}', {$thread['forum']}, '{$thread['url']}')";
            string sql = "";
            return sql;
        }

        public void UpdateThread(ForumThread t)
        {
            // Make sure all the values in p are in this POST
            // Some may be null for whatever reason
        }

    }
}
