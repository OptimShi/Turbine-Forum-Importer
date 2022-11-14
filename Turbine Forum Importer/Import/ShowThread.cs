using AngleSharp;
using AngleSharp.Css;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Text;
using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Turbine_Forum_Importer.DataTypes;
using static Google.Protobuf.WellKnownTypes.Field.Types;

namespace Turbine_Forum_Importer.Import
{
    public partial class FileImporter
    {

        private int threadId = 0;

        void ParseShowThread()
        {
            LoadDOM();

            ForumThread t = new ForumThread();
            ShowThread_GetThreadInfo(t);
            t.Title = ShowThread_GetThreadTitle();

            t.Forum = ShowThread_GetForumInfo();
            AddThread(t);

            var users = ShowThread_GetUsers();
            var posts = ShowThread_GetPosts();
        }

        ForumThread ShowThread_GetThreadInfo(ForumThread t)
        {
            // <link rel="canonical" href="showthread.php?57-Prismatic-Peas" />
            string match = "<link rel=\"canonical\" href=\"";
            var start = RawText.IndexOf(match);

            // Get the URL and ThreadID from the Canonical
            if (start != -1)
            {
                start = start + match.Length;
                var end = RawText.IndexOf('"', start); // get the closing of this tag

                string Canonical = GetQueryStringFromURL(RawText.Substring(start, end - start));

                t.Id = GetIdFromUrl(Canonical); ;
                threadId = t.Id;
                t.URL = Canonical;
            }

            return t;
        }

        string ShowThread_GetThreadTitle()
        {
            var titleEle = document.QuerySelector("#pagetitle");
            if (titleEle != null)
            {
                string title = titleEle.TextContent;
                title = title.Replace("Thread:", "");
                return title.Trim();
            }
            return "";
        }

        int ShowThread_GetForumInfo()
        {
            Forum f = new Forum();
            /*
            #breadcrumb
            <li class="navbit"><a href="index.php">Forum</a></li>
            <li class="navbit"><a href="forumdisplay.php?1-Asheron-s-Call-Discussions">Asheron's Call Discussions</a></li>
            <li class="navbit"><a href="forumdisplay.php?2-AC1-General-Discussions">AC1 General Discussions</a></li>
            <li class="navbit lastnavbit"><span> Current Thread Title </span></li>
            */

            var Navbit = document.QuerySelectorAll("#breadcrumb li.navbit");
            if(Navbit.Length > 1)
            {
                // Start at 1 since 0 should be just the Index
                // Go to Length-1 since the last one is just the title of the thread, not an actual NAV
                for(var i = 1; i < Navbit.Length-1; i++)
                {
                    Forum navForum = new Forum();
                    var navItem = Navbit[i];
                    navForum.Title = navItem.TextContent;

                    if(navItem.Children.Length > 0 && navItem.Children[0].NodeName == "A") {
                        var href = navItem.Children[0];
                        var link = GetQueryStringFromURL(href.Attributes["href"].Value);
                        navForum.Id = GetIdFromUrl(link);
                        navForum.Url = link;
                    }

                    AddForum(navForum);
                    f = navForum;
                }
            }

            return f.Id;
        }

        Dictionary<string, User> ShowThread_GetUsers()
        {
            Dictionary<string, User> users = new Dictionary<string, User>();

            var userInfo = document.QuerySelectorAll(".userinfo");
    
            foreach(var u in userInfo)
            {
                User user = new User();
                var username = u.QuerySelector("a.username");
                if(username != null)
                {
                    user.Name = username.TextContent;
                    var link = GetQueryStringFromURL(username.Attributes["href"].Value);
                    user.Url = link;

                    user.Id = GetIdFromUrl(link);
                    
                }

                var userExtra = u.QuerySelector(".userinfo_extra");
                bool PostCountFound = false;
                if(userExtra != null && userExtra.Children.Length > 0)
                {
                    foreach(var extraChild in userExtra.Children)
                    {
                        if (PostCountFound)
                        {
                            user.PostCount = Int32.Parse(extraChild.TextContent.Replace(",", ""));
                            break;
                        }
                        else if(extraChild.TextContent == "Posts")
                        {
                            // The dd is "Posts", the dt is the actual post count.
                            // Setting this flag tells us the next item is the count.
                            PostCountFound = true;
                        }
                    }
                }

                // You could post as a guest??
                var guests = document.QuerySelectorAll("span.username.guest");
                foreach (var guest in guests)
                {
                    user.Guest = true;
                    user.Id = 0;
                    user.Name = guest.TextContent;
                }

    
                AddUser(user);
                if((user.Id > 0 || user.Guest) && users.ContainsKey(user.Name) == false)
                {
                    users.Add(user.Name, user);
                }
            }

            return users;
        }

        Dictionary<int, Post> ShowThread_GetPosts()
        {
            Dictionary<int, Post> MyPosts = new Dictionary<int, Post>();

            var postItems = document.QuerySelectorAll("#posts > li");
            foreach(var postItem in postItems)
            {
                var post = new Post();
                post.HTML = postItem.InnerHtml;

                var postDate = getDateFromPostdate(postItem);
                if (postDate != null)
                    post.PostDate = (DateTime)postDate;

                var id = postItem.Id.Replace("post_", "");
                if (id.Length > 0)
                    post.Id = Int32.Parse(id);


                AddPost(post);
                if(post.Id > 0)
                    MyPosts.Add(post.Id, post);
            }

            return MyPosts;
        }

        DateTime? getDateFromPostdate(IElement? postItem)
        {
            var dateItem = postItem.QuerySelector("span.date");
            if(dateItem != null)
            {
                string[] formats = { "yyyyMMdd", "HH:mm a" };
                // define('TURBINE_DATE_FORMAT', 'm-d-Y g:i a');

                //string[] formats = { "mm-dd-yyyy", "hh:mmss" };
                if (DateTime.TryParseExact(dateItem.TextContent, "M-d-yyyy, hh:mm t ", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                {
                    return dt;
                }
                    
            }
            return null;
        }
    }
}
