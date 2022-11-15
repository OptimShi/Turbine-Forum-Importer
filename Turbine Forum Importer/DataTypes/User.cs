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

        public string GetSQLStatement()
        {
            string sql = "";
            return sql;
        }

        public void UpdateUser(User newUser)
        {
            // Update our PostCount if what we have now is higher...
            if (newUser.PostCount > PostCount)
                PostCount = newUser.PostCount;
        }

    }
}
