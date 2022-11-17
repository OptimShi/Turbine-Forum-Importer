using Turbine_Forum_Importer.DataTypes;
using Turbine_Forum_Importer.Import;

namespace Turbine_Forum_Importer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Dictionary<int, Post> Posts = new Dictionary<int, Post>();
        Dictionary<int, ForumThread> Threads = new Dictionary<int, ForumThread>();
        Dictionary<int, User> Users = new Dictionary<int, User>();
        Dictionary<int, Forum> Forums = new Dictionary<int, Forum>();

        Db db = new Db();

        private void InitDbTables()
        {
            Forums = db.GetForums();
            Users = db.GetUsers();
            Posts = db.GetPosts();
            Threads = db.GetThreads();
        }

        private void btnDoImport_Click(object sender, EventArgs e)
        {
            if (Posts.Count == 0) InitDbTables();

            textStatus.Text = "";
            string path = @"D:\Web Development\Turbine\archives\zip\";
            //path = @"D:\Web Development\Turbine\archives\rar\";
            //path = @"D:\Web Development\Turbine\unique_samples\";
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            statusFileCount.Text = files.Length + " Files Found.";
            progress.Maximum = files.Length;
            progress.Value = 0;
            progress.Step = 1;

            progress.Visible = true;
            statusFileCount.Visible = true;

            foreach (string file in files)
            {
                progress.PerformStep();

                var import = new FileImporter(file);
                string basename = Path.GetFileName(file);
                textStatus.AppendText(basename + " -- " + import.Template + Environment.NewLine);

                foreach (var p in import.Posts)
                {
                    if (Posts.ContainsKey(p.Key))
                        Posts[p.Key].UpdatePost(p.Value);
                    else
                    {
                        p.Value.Modified = true;
                        Posts.Add(p.Key, p.Value);
                    }
                }

                foreach (var t in import.Threads)
                {
                    if (Threads.ContainsKey(t.Key))
                        Threads[t.Key].UpdateThread(t.Value);
                    else
                    {
                        t.Value.Modified = true;
                        Threads.Add(t.Key, t.Value);
                    }

                }

                foreach (var u in import.Users)
                {
                    if (Users.ContainsKey(u.Key))
                        Users[u.Key].UpdateUser(u.Value);
                    else
                    {
                        u.Value.Modified = true;
                        Users.Add(u.Key, u.Value);
                    }
                }

                foreach (var f in import.Forums)
                {
                    if (Forums.ContainsKey(f.Key))
                        Forums[f.Key].UpdateForum(f.Value);
                    else
                    {
                        f.Value.Modified = true;
                        Forums.Add(f.Key, f.Value);
                    }
                }
            }

            progress.Visible = false;
            statusFileCount.Visible = false;

            CopyToDb();
        }

        private void CopyToDb()
        {
            var totalCount = Users.Values.Sum(x => x.Modified == true ? 1 : 0);
            totalCount += Forums.Values.Sum(x => x.Modified == true ? 1 : 0);
            totalCount += Posts.Values.Sum(x => x.Modified == true ? 1 : 0);
            totalCount += Threads.Values.Sum(x => x.Modified == true ? 1 : 0);
            statusFileCount.Text = totalCount.ToString() + " Entries to Post to Database.";
            progress.Maximum = totalCount;
            progress.Value = 0;

            progress.Visible = true;
            statusFileCount.Visible = true;

            var usersToImport = Users.Values.Where(x => x.Modified == true).ToList();
            foreach (var u in usersToImport)
            {
                var sql = u.GetSQLStatement();
                if (sql != "") db.RunSQL(sql);
                progress.PerformStep();
            }

            var forumsToImport = Forums.Values.Where(x => x.Modified == true).ToList();
            foreach (var f in forumsToImport)
            {
                var sql = f.GetSQLStatement();
                if (sql != "") db.RunSQL(sql);
                progress.PerformStep();
            }

            var postsToImport = Posts.Values.Where(x => x.Modified == true).ToList();
            foreach (var p in postsToImport)
            {
                var sql = p.GetSQLStatement();
                if (sql != "") db.RunSQL(sql);
                progress.PerformStep();
            }

            var threadsToImport = Threads.Values.Where(x => x.Modified == true).ToList();
            foreach (var t in threadsToImport)
            {
                var sql = t.GetSQLStatement();
                if (sql != "") db.RunSQL(sql);
                progress.PerformStep();
            }

            progress.Visible = false;
            statusFileCount.Visible = false;

        }
    }
}