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
    }
}
