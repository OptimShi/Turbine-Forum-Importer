using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
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

        public Dictionary<int, Post> Posts = new Dictionary<int, Post>();
        public Dictionary<int, ForumThread> Threads = new Dictionary<int, ForumThread>();
        public Dictionary<int, User> Users = new Dictionary<int, User>();
        public Dictionary<int, Forum> Forums = new Dictionary<int, Forum>();
        
        HtmlParser parser = new HtmlParser();
        IHtmlDocument document;

        public FileImporter(string filename, bool onlyReadTemplate = false)
        {
            Filename = filename;

            // Some files are too large, because they're not the files we want (images, odd zips, etc)
            FileInfo fileInfo = new FileInfo(filename);
            long maxSizeToRead = 1024 * 1024 * 5; // 5MB
            if (fileInfo.Length > maxSizeToRead)
                return;

            RawText = File.ReadAllText(Filename);
            Template = getTemplate();
            if (!onlyReadTemplate) 
            { 
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
        }


        private void LoadDOM()
        {
            document = parser.ParseDocument(RawText);
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

        /// <summary>
        /// Parses the URL to retrieve the relevant data bits.
        /// Typically, this falls in the query string, but certain crawlers change this link.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetQueryStringFromURL(string url)
        {
            string QueryString = url;
            if (QueryString.IndexOf(".php-") != -1)
                QueryString = QueryString.Substring(QueryString.IndexOf(".php-") + 5);

            // Regular format, Archive.org
            if (QueryString.IndexOf("?") != -1)
                QueryString = QueryString.Substring(QueryString.IndexOf("?") + 1);

            // Trim any `/page2` or whatever else may be out there...
            if (QueryString.IndexOf("/") != -1)
                QueryString = QueryString.Substring(0, QueryString.LastIndexOf("/"));

            // Remove a trailing ".html"
            if (QueryString.IndexOf(".html") != -1 && QueryString.IndexOf(".html") == (QueryString.Length - 5))
                QueryString = QueryString.Substring(0, QueryString.LastIndexOf(".html"));

            // Remove a trailing "-pageX"
            if (QueryString.IndexOf("-page") != -1 && QueryString.IndexOf("-page") == (QueryString.Length - 6))
                QueryString = QueryString.Substring(0, QueryString.LastIndexOf("-page"));
            // -pageXX
            if (QueryString.IndexOf("-page") != -1 && QueryString.IndexOf("-page") == (QueryString.Length - 7))
                QueryString = QueryString.Substring(0, QueryString.LastIndexOf("-page"));



            return QueryString;
        }

        /// <summary>
        /// Gets the ID from the QueryString
        /// </summary>
        /// <param name="url">Format of 1-Asheron-s-Call-Discussions</param>
        /// <returns></returns>
        private int GetIdFromUrl(string url)
        {
            int id = 0;
            // Checks if the URL is already just a number, e.g. "12345". This can happen if title is non-ASCII, and the hyphen is dropped
            bool alreadyNumeric = int.TryParse(url, out id);
            if (!alreadyNumeric)
            {
                int.TryParse(url.Substring(0, url.IndexOf("-")), out id);
            }
            return id;
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
        private void AddUser(User u)
        {
            // ID of 0 means couldn't find what we were looking for...
            if (u.Id == 0) return;

            if (Users.ContainsKey(u.Id))
            {
                Users[u.Id].UpdateUser(u);
            }
            else
            {
                Users.Add(u.Id, u);
            }
        }
        private void AddPost(Post p)
        {
            // ID of 0 means couldn't find what we were looking for...
            if (p.Id == 0) return;

            if (Posts.ContainsKey(p.Id))
            {
                Posts[p.Id].UpdatePost(p);
            }
            else
            {
                Posts.Add(p.Id, p);
            }
        }

    }
}
