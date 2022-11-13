using AngleSharp.Css;
using AngleSharp.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turbine_Forum_Importer.DataTypes;

namespace Turbine_Forum_Importer.Import
{
    public partial class FileImporter
    {

        private int threadId = 0;

        void ParseShowThread()
        {
            ShowThread_GetThreadInfo();
        }


        void ShowThread_GetThreadInfo()
        {
            ForumThread t = new ForumThread();

            // <link rel="canonical" href="showthread.php?57-Prismatic-Peas" />
            string match = "<link rel=\"canonical\" href=\"";
            var start = RawText.IndexOf(match);

            // Get the URL and ThreadID from the Canonical
            if(start != -1)
            {
                start = start + match.Length;
                var end = RawText.IndexOf('"', start); // get the closing of this tag

                string Canonical = RawText.Substring(start, end - start);

                // There are a few formats this can be in depending on what software created the archive

                // For the crawled files in the ZIP archive
                if (Canonical.IndexOf(".php-") != -1)
                    Canonical = Canonical.Substring(Canonical.IndexOf(".php-") + 5);

                // Regular format, Archive.org
                if (Canonical.IndexOf("?") != -1)
                    Canonical = Canonical.Substring(Canonical.IndexOf("?") + 1);

                // Trim any `/page2` or whatever else may be out there...
                if (Canonical.IndexOf("/") != -1)
                    Canonical = Canonical.Substring(0, Canonical.LastIndexOf("/"));

                int threadId = Int32.Parse(Canonical.Substring(0, Canonical.IndexOf("-")));
                t.Id = threadId;
                t.URL = Canonical;
            }

            // Extract the Thread Title
            start = RawText.IndexOf("<div id=\"pagetitle\"");
            if(start != -1)
            {
                var end = RawText.IndexOf("</div>", start);
                string title = StripTags(RawText.Substring(start, end - start));
                title = title.Replace("Thread:", "");
                t.Title = title.Trim();
            }

            var f = ShowThread_GetForumInfo();
            if(f.Id != 0)
            {
                t.Forum = f.Id;
                AddForum(f);
            }

            AddThread(t);
        }

        Forum ShowThread_GetForumInfo()
        {
            Forum f = new Forum();
        
            // TODO

            return f;
        }
    }
}
