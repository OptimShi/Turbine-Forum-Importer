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
        public int PostCount;
        private bool Turbine; // Is this user a Turbine employee, used for DevTracker
        public bool Guest = false;
    }
}
