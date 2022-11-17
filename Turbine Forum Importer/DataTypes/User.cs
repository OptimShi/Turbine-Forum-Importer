using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbine_Forum_Importer.DataTypes
{
    public class User
    {
        public int Id;
        public string Name;
        public string Url;
        private bool Turbine; // Is this user a Turbine employee, used for DevTracker
        public bool Guest = false;

        public string Avatar = "";
        public int PostCount;
        public string JoinDate = "";
        public string Location = "";

        public bool Modified = false;

        public string GetSQLStatement()
        {
            string sql = "REPLACE INTO `users` (`id`, `username`, `url`, `posts`, `avatar`, `join_date`, `location`) VALUES ";
            sql += $"({Id}, " +
                $"'{MySqlHelper.EscapeString(Name)}', " +
                $"'{MySqlHelper.EscapeString(Url)}', " +
                $"{PostCount}, " +
                $"'{MySqlHelper.EscapeString(Avatar)}', " +
                $"'{MySqlHelper.EscapeString(JoinDate)}', " +
                $"'{MySqlHelper.EscapeString(Location)}')";
            return sql;
        }

        /// <summary>
        ///  Tries to update the item, if anything has been updated since the version we have stored.
        ///  If anything has changed, then Modified is set to true
        /// </summary>
        /// <param name="newUser"></param>
        public void UpdateUser(User newUser)
        {
            // Update our PostCount if what we have now is higher...
            if (newUser.PostCount > PostCount)
            {
                PostCount = newUser.PostCount;
                Modified = true;
            }

            if (Url == "" && newUser.Url != "")
            {
                Url = newUser.Url;
                Modified = true;
            }
            if (Avatar == "" && newUser.Avatar != "")
            {
                Avatar = newUser.Avatar;
                Modified = true;
            }
            if (JoinDate == "" && newUser.JoinDate != "")
            {
                JoinDate = newUser.JoinDate;
                Modified = true;
            }
            if (Location == "" && newUser.Location != "")
            {
                Location = newUser.Location;
                Modified = true;
            }
        }

    }
}
