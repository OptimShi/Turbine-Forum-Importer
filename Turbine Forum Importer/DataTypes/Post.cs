using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace Turbine_Forum_Importer.DataTypes
{
    public class Post
    {
        public int Id;
        public int Thread;

        public int User;
        public string Username = "";
        public bool Guest = false;

        public string HTML = "";
        public DateTime PostDate;

        public bool Modified = false;

        public string GetSQLStatement()
        {
            string sql = "REPLACE INTO `posts` (`id`, `thread`, `user`, `guest_user`, `username`, `content`, `html`, `date`) VALUES ";
            string content = StripTag(HTML).Trim();
            string dateForMySql = PostDate.ToString("yyyy-MM-dd HH:mm:ss");
            sql += $"({Id}, " +
                $"{Thread}, " +
                $"{User}, " +
                $"{Guest}, " +
                $"'{MySqlHelper.EscapeString(Username)}', " +
                $"'{MySqlHelper.EscapeString(content)}', " +
                $"'{MySqlHelper.EscapeString(HTML)}', " +
                $"'{MySqlHelper.EscapeString(dateForMySql)}')";
            return sql;
        }

        /// <summary>
        ///  Tries to update the item, if anything has been updated since the version we have stored.
        ///  If anything has changed, then Modified is set to true
        /// </summary>
        /// <param name="newUser"></param>
        public void UpdatePost(Post newPost)
        {
            // Make sure all the values in p are in this POST
            // Some may be null for whatever reason

            if (Thread == 0 && newPost.Thread != 0)
            {
                Thread = newPost.Thread;
                Modified = true;

            }

            if (User == 0 && newPost.User != 0)
            {
                User = newPost.User;
                Modified = true;

            }
        }


        /// <summary>
        ///     HTML and other mark up tags stripped from a given the string ListOfAllowedTags.
        ///     This Method is the ASP.NET Version of the PHP Strip_Tags Method. It will strip out all html and xml tags
        ///     except for the ones explicitly allowed in the second parameter.  If allowed, this method DOES NOT strip out
        ///     attributes.
        ///     REF: https://stackoverflow.com/questions/10971317/c-sharp-strip-html-tags-decode-entities
        /// </summary>
        /// <param name="htmlString">
        ///     The HTML string.
        /// </param>
        /// <param name="listOfAllowedTags">
        ///     The list of allowed tags.  if null, then nothing allowed.  otherwise, ex: "b,<br/>,<hr>,p,i,<u>"
        /// </param>
        /// <returns>
        ///     Cleaned String
        /// </returns>
        /// <author>James R.</author>
        /// <createdate>10-27-2011</createdate>
        public string StripTag(string htmlString, string? listOfAllowedTags = null)
        {
            if (string.IsNullOrEmpty(htmlString))
            {
                return htmlString;
            }

            // this is the reg pattern that will retrieve all tags
            string patternThatGetsAllTags = "</?[^><]+>";

            // Create the Regex for all of the Allowed Tags
            string patternForTagsThatAreAllowed = string.Empty;
            if (!string.IsNullOrEmpty(listOfAllowedTags))
            {
                // get the HTML starting tag, such as p,i,b from an example string of <p>,<i>,<b>
                Regex remove = new Regex("[<>\\/ ]+");

                // now strip out /\<> and spaces
                listOfAllowedTags = remove.Replace(listOfAllowedTags, string.Empty);

                // split at the commas
                string[] listOfAllowedTagsArray = listOfAllowedTags.Split(',');

                foreach (string allowedTag in listOfAllowedTagsArray)
                {
                    if (string.IsNullOrEmpty(allowedTag))
                    {
                        // jump to next element of array.
                        continue;
                    }

                    string patternVersion1 = "<" + allowedTag + ">"; // <p>
                    string patternVersion2 = "<" + allowedTag + " [^><]*>$";

                    // <img src=stuff  or <hr style="width:50%;" />
                    string patternVersion3 = "</" + allowedTag + ">"; // closing tag

                    // if it is not the first time, then add the pipe | to the end of the string
                    if (!string.IsNullOrEmpty(patternForTagsThatAreAllowed))
                    {
                        patternForTagsThatAreAllowed += "|";
                    }

                    patternForTagsThatAreAllowed += patternVersion1 + "|" + patternVersion2 + "|" + patternVersion3;
                }
            }

            // Get all html tags included in the string
            Regex regexHtmlTag = new Regex(patternThatGetsAllTags);

            if (!string.IsNullOrEmpty(patternForTagsThatAreAllowed))
            {
                MatchCollection allTagsThatMatched = regexHtmlTag.Matches(htmlString);

                foreach (Match theTag in allTagsThatMatched)
                {
                    Regex regOfAllowedTag = new Regex(patternForTagsThatAreAllowed);
                    Match matchOfTag = regOfAllowedTag.Match(theTag.Value);

                    if (!matchOfTag.Success)
                    {
                        // if not allowed replace it with nothing
                        htmlString = htmlString.Replace(theTag.Value, string.Empty);
                    }
                }
            }
            else
            {
                // else strip out all tags
                htmlString = regexHtmlTag.Replace(htmlString, string.Empty);
            }

            return htmlString;
        }
    }
}
