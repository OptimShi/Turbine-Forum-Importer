using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Turbine_Forum_Importer.DataTypes;

namespace Turbine_Forum_Importer.Import
{
    public class Db
    {
        MySqlConnection Connection;

        public Db()
        {
            string dbhost = "localhost";
            string dbuser = "root";
            string dbpassword = "";
            string dbname = "turbine_forums";
            Connection = new MySqlConnection($"SERVER={dbhost}; user id={dbuser}; password={dbpassword}; database={dbname}");
            Connection.Open();

            //RunSQL("do a broken thing");
            //RunSQL("select * from threads where id < 2");
            //RunSQL("select version()");
        }

        public bool RunSQL(string command)
        {
            var cmd = new MySqlCommand(command, Connection);
            try { 
                var result = cmd.ExecuteScalar(); 
            }catch(Exception ex)
            {
                // Something bad happened... Probably a crappy date!
            }
            
            return true;
        }

        public Dictionary<int, Forum> GetForums()
        {
            Dictionary<int, Forum> forums = new Dictionary<int, Forum>();

            string sql = "SELECT * FROM forums";
            MySqlCommand cmd = new MySqlCommand(sql, Connection);

            //Connection.Open();

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var f = new Forum();
                f.Id = reader.GetInt32("id");
                f.Title = reader.GetString("name");
                f.Description = reader.GetString("Description");
                forums[f.Id] = f;
            }

            reader.Close();
            return forums;
        }

        public Dictionary<int, User> GetUsers()
        {
            Dictionary<int, User> users = new Dictionary<int, User>();

            string sql = " SELECT * FROM users";
            MySqlCommand cmd = new MySqlCommand(sql, Connection);

            //Connection.Open();

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var u = new User();

                u.Id = reader.GetInt32("id");
                u.Name = reader.GetString("username");
                u.Url = reader.GetString("url");
                u.Avatar = reader.GetString("avatar");
                u.JoinDate = reader.GetString("join_date");
                u.Location = reader.GetString("location");
                u.PostCount = reader.GetInt32("posts");
                users[u.Id] = u;
            }

            reader.Close();
            return users;
        }

        public Dictionary<int, Post> GetPosts()
        {
            Dictionary<int, Post> posts = new Dictionary<int, Post>();

            string sql = " SELECT * FROM posts";
            MySqlCommand cmd = new MySqlCommand(sql, Connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var p = new Post();
                p.Id = reader.GetInt32("id");
                p.Thread = reader.GetInt32("thread");
                p.User = reader.GetInt32("user");
                p.Guest = reader.GetBoolean("guest_user");
                p.Username = reader.GetString("username");
                p.HTML = reader.GetString("html");
                //p.PostDate = reader.GetDateTime("date");
                posts[p.Id] = p;
            }

            reader.Close();
            return posts;
        }
        
        public Dictionary<int, ForumThread> GetThreads()
        {
            Dictionary<int, ForumThread> threads = new Dictionary<int, ForumThread>();

            string sql = "SELECT * FROM threads";
            MySqlCommand cmd = new MySqlCommand(sql, Connection);

            //Connection.Open();

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var t = new ForumThread();

                t.Id = reader.GetInt32("id");
                t.Title = reader.GetString("title");
                t.Forum = reader.GetInt32("forum");
                t.Url = reader.GetString("url");
                threads[t.Id] = t;
            }

            reader.Close();
            return threads;
        }


    }
}
