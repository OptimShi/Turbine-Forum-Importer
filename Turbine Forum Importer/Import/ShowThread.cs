using AngleSharp;
using AngleSharp.Css;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Text;
using Google.Protobuf.Collections;
using Microsoft.VisualBasic.ApplicationServices;
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
using User = Turbine_Forum_Importer.DataTypes.User;

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

        User ShowThread_GetUser(IElement u)
        {
            User user = new User();
            var username = u.QuerySelector("a.username");
            if (username != null)
            {
                user.Name = username.TextContent;
                var link = GetQueryStringFromURL(username.Attributes["href"].Value);
                user.Url = link;

                user.Id = GetIdFromUrl(link);

            }

            var avatar = u.QuerySelector(".postuseravatar img");
            if(avatar != null)
            {
                user.Avatar = avatar.Attributes["src"].Value;
            }

            var userExtra = u.QuerySelector(".userinfo_extra");
            bool PostCountFound = false;
            if (userExtra != null && userExtra.Children.Length > 0)
            {
                for(var i = 0; i< userExtra.Children.Length; i++)
                {
                    switch (userExtra.Children[i].TextContent)
                    {
                        case "Posts":
                            i++;
                            user.PostCount = Int32.Parse(userExtra.Children[i].TextContent.Replace(",", ""));
                            break;
                        case "Join Date":
                            i++;
                            user.JoinDate = userExtra.Children[i].TextContent.Replace(",", "");
                            break;
                        case "Location":
                            i++;
                            user.Location = userExtra.Children[i].TextContent.Replace(",", "");
                            break;
                        default:
                            var test = userExtra.Children[i].TextContent;
                            break;
                    }
                }

                foreach (var extraChild in userExtra.Children)
                {
                    if (PostCountFound)
                    {
                        user.PostCount = Int32.Parse(extraChild.TextContent.Replace(",", ""));
                        break;
                    }
                    else if (extraChild.TextContent == "Posts")
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
            return user;
        }

        Dictionary<string, User> ShowThread_GetUsers()
        {
            Dictionary<string, User> users = new Dictionary<string, User>();

            var userInfo = document.QuerySelectorAll(".userinfo");
    
            foreach(var u in userInfo)
            {
                var user = ShowThread_GetUser(u);
                if ((user.Id > 0 || user.Guest) && users.ContainsKey(user.Name) == false)
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
                var content = postItem.QuerySelector(".postcontent");
                if (content != null)
                    post.HTML = content.InnerHtml.Trim() ;

                var postDate = getDateFromPostdate(postItem);
                if (postDate != null)
                    post.PostDate = (DateTime)postDate;

                var id = postItem.Id.Replace("post_", "");
                if (id.Length > 0)
                    post.Id = Int32.Parse(id);

                var userInfo = postItem.QuerySelector(".userinfo");
                var user = ShowThread_GetUser(userInfo);
                post.User = user.Id;
                post.Username = user.Name;
                post.Guest = user.Guest;

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
                CultureInfo enUS = new CultureInfo("en-US");
                //              "11-04-2004 04:43 PM"
                string format = "MM'-'dd'-'yyyy hh:mm tt";
                string testDate = DateTime.Now.ToString(format);
                string dateString = dateItem.TextContent.Replace(",", "").Trim();
                //dateString = dateString.Replace("-", "/");
                if (DateTime.TryParse(dateString,  out DateTime dt))
                    return dt;
                    
            }
            return null;
        }
    }
}
