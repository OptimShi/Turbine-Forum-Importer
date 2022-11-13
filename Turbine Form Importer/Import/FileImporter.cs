using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Turbine_Forum_Importer.DataTypes;

namespace Turbine_Forum_Importer.Import
{
    public partial class FileImporter
    {
        protected string RawText; // Raw HTML (or whatever) in the file to import

        public string Template;
        private string Filename;

        Dictionary<int, Post> Posts = new Dictionary<int, Post>();
        Dictionary<int, ForumThread> Threads = new Dictionary<int, ForumThread>();
        Dictionary<int, User> Users = new Dictionary<int, User>();
        Dictionary<int, Forum> Forums = new Dictionary<int, Forum>();

        public FileImporter(string filename){
            Filename = filename;
            RawText = File.ReadAllText(Filename);
            Template = getTemplate();

            if (Template != "")
            {
                switch (Template)
                {
                    case "SHOWTHREAD":
                        ParseShowThread();
                        break;
                    default:
                        // Do nothing for now.
                        break;

                }
            }
        }

        /// <summary>
        /// Helper function, equivalent to PHP's StripTags();
        /// https://thedeveloperblog.com/c-sharp/remove-html-tags
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static string StripTags(string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }


        // Each forum page has a template in a comment at the top of the file, after the DOCTYPE but before the <html> declaration.
        private string getTemplate()
        {
            string matchStr = "<!-- BEGIN TEMPLATE: ";
            var start = RawText.IndexOf(matchStr);
            var htmlStart = RawText.IndexOf("<html");
        
            if(start != -1 && htmlStart > start)
            {
                start = start + matchStr.Length;
                var end = RawText.IndexOf("-->");
                string template = RawText.Substring(start, end - start);
                return template.Trim();
            }

            return "";
        }

        private void AddThread(ForumThread t)
        {
            // ID of 0 means couldn't find what we were looking for...
            if (t.Id == 0) return;

            if (Threads.ContainsKey(t.Id))
            {

            }
            else
            {
                Threads.Add(t.Id, t);
            }
        }

        private void AddForum(Forum f)
        {
            // ID of 0 means couldn't find what we were looking for...
            if (f.Id == 0) return;

            if (Forums.ContainsKey(f.Id))
            {

            }
            else
            {
                Forums.Add(f.Id, f);
            }
        }

    }
}
