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



            return "";
        }
    }
}
