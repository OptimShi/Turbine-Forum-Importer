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
            string path, completePath, errorPath;

            textStatus.Text = "";

            var archiveName = "zip";

            switch (archiveName)
            {
                case "zip":
                    path = @"D:\Web Development\turbine\archives\zip\SHOWTHREAD\";
                    completePath = @"D:\Web Development\turbine\archives\zip\complete\";
                    errorPath = @"D:\Web Development\turbine\archives\zip\error\";
                    break;
                case "rar":
                    path = @"D:\Web Development\turbine\archives\rar\files\SHOWTHREAD\";
                    completePath = @"D:\Web Development\turbine\archives\rar\complete\";
                    errorPath = @"D:\Web Development\turbine\archives\rar\error\";
                    break;
                case "wayback_small":
                    path = @"E:\Turbine\WayBack Ruby Crawl\en\";
                    completePath = @"E:\Turbine\WayBack Ruby Crawl\complete\";
                    errorPath = @"E:\Turbine\WayBack Ruby Crawl\errors\";
                    break;
                case "wayback_large":
                    path = @"E:\Turbine\websites\files\en\";
                    completePath = @"E:\Turbine\websites\complete\";
                    errorPath = @"E:\Turbine\websites\error\";
                    break;
                default:
                    path = "";
                    completePath = "";
                    errorPath = "";
                    break;
            }

            //path = @"D:\Web Development\Turbine\unique_samples\";
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            //string[] files = new string[1];
            //files[0] = @"D:\Web Development\turbine\archives\rar\SHOWTHREAD\showthreadff32.html";

            statusFileCount.Text = files.Length + " Files Found.";
            progress.Maximum = files.Length;
            progress.Value = 0;
            progress.Step = 1;

            int postEveryNumber = 1000;
            int counter = 0;

            progress.Visible = true;
            statusFileCount.Visible = true;

            foreach (string file in files)
            {
                progress.PerformStep();

                //if (file.IndexOf("showthread.php") == -1)
                //    continue;
                try
                {

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

                    MoveFile(file, path, completePath);
                }
                catch(Exception ex)
                {
                     MoveFile(file, path, errorPath);
                    // Do we do something here?
                }
                counter++;
                if(counter > postEveryNumber)
                {
                    CopyToDb();
                    counter = 0;
                }
            }

            progress.Visible = false;
            statusFileCount.Visible = false;

            CopyToDb();
        }

        private void MoveFile(string src, string srcPath, string destPath)
        {
            var destFile = src.Replace(srcPath, destPath);
            // Try to create the destination path
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(destFile)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destFile));
                }
                File.Move(src, destFile);
            }catch(Exception ex)
            {
                // Error moving file? ?
            }
        }

        private void CopyToDb()
        {
            //return;

            var totalCount = Users.Values.Sum(x => x.Modified == true ? 1 : 0);
            totalCount += Forums.Values.Sum(x => x.Modified == true ? 1 : 0);
            totalCount += Posts.Values.Sum(x => x.Modified == true ? 1 : 0);
            totalCount += Threads.Values.Sum(x => x.Modified == true ? 1 : 0);
            //statusFileCount.Text = totalCount.ToString() + " Entries to Post to Database.";
            //progress.Maximum = totalCount;
            //progress.Value = 0;

            //progress.Visible = true;
            //statusFileCount.Visible = true;

            var usersToImport = Users.Values.Where(x => x.Modified == true).ToList();
            foreach (var u in usersToImport)
            {
                var sql = u.GetSQLStatement();
                if (sql != "") db.RunSQL(sql);
                Users[u.Id].Modified = false;
                //progress.PerformStep();
            }

            var forumsToImport = Forums.Values.Where(x => x.Modified == true).ToList();
            foreach (var f in forumsToImport)
            {
                var sql = f.GetSQLStatement();
                if (sql != "") db.RunSQL(sql);
                Forums[f.Id].Modified = false;
                //progress.PerformStep();
            }

            var postsToImport = Posts.Values.Where(x => x.Modified == true).ToList();
            foreach (var p in postsToImport)
            {
                var sql = p.GetSQLStatement();
                if (sql != "") db.RunSQL(sql);
                Posts[p.Id].Modified = false;
                //progress.PerformStep();
            }

            var threadsToImport = Threads.Values.Where(x => x.Modified == true).ToList();
            foreach (var t in threadsToImport)
            {
                var sql = t.GetSQLStatement();
                if (sql != "") db.RunSQL(sql);
                Threads[t.Id].Modified = false;
                //progress.PerformStep();
            }

            //progress.Visible = false;
            //statusFileCount.Visible = false;

        }

        private void btnOrganize_Click(object sender, EventArgs e)
        {
            textStatus.Text = "";
            string path = "";

            var archiveName = "zip";

            switch (archiveName)
            {
                case "zip":
                    path = @"D:\Web Development\turbine\archives\zip\";
                    break;
                case "rar":
                    path = @"D:\Web Development\turbine\archives\rar\files\SHOWTHREAD\";
                    break;
                case "wayback_small":
                    path = @"E:\Turbine\WayBack Ruby Crawl\en\";
                    break;
                case "wayback_large":
                    path = @"E:\Turbine\websites\files\en\";
                    break;
                default:
                    path = "";
                    break;
            }

            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            statusFileCount.Text = files.Length + " Files Found.";
            progress.Maximum = files.Length;
            progress.Value = 0;
            progress.Step = 1;

            progress.Visible = true;
            statusFileCount.Visible = true;

            foreach (string file in files)
            {
                var import = new FileImporter(file, true);
                string template = import.Template;
                string basename = Path.GetFileName(file);
                textStatus.AppendText(basename + " -- " + import.Template + Environment.NewLine);

                if(template == "" || template == null)
                    template = "other";

                string destinationPath = Path.Combine(path, template);
                if (!Directory.Exists(destinationPath))
                {
                    Directory.CreateDirectory(destinationPath);
                }
                string destination = Path.Combine(destinationPath, basename);
                if(destination == file) { continue; } // Skip files already organized!
                int unique = 1;
                if (File.Exists(destination))
                {
                    while (File.Exists(destination))
                    {
                        string baseFileName = Path.GetFileNameWithoutExtension(basename);
                        string extension = Path.GetExtension(basename);
                        destination = Path.Combine(destinationPath, baseFileName + "_" + unique.ToString() + extension);
                        unique++;
                    }
                }
                File.Move(file, destination);
                progress.PerformStep();
            }

            progress.Visible = false;
            statusFileCount.Visible = false;
        }
    }
}