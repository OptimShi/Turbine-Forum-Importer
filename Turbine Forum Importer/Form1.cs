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

        private void btnDoImport_Click(object sender, EventArgs e)
        {
            textStatus.Text = "";
            string path = @"D:\Web Development\Turbine\archives\zip\www.asheronscall.com\";
            //path = @"D:\Web Development\Turbine\archives\rar\";
            //path = @"D:\Web Development\Turbine\unique_samples\";
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            statusFileCount.Text = files.Length + " Files Found.";
            progress.Maximum = files.Length;
            progress.Value = 0;
            progress.Step = 1;

            progress.Visible = true;
            statusFileCount.Visible = true;

            Dictionary<int, Post> Posts = new Dictionary<int, Post>();
            Dictionary<int, ForumThread> Threads = new Dictionary<int, ForumThread>();
            Dictionary<int, User> Users = new Dictionary<int, User>();
            Dictionary<int, Forum> Forums = new Dictionary<int, Forum>();

            Db db = new Db();

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
                        Posts.Add(p.Key, p.Value);
                }

                foreach (var t in import.Threads)
                {
                    if (Threads.ContainsKey(t.Key))
                        Threads[t.Key].UpdateThread(t.Value);
                    else
                        Threads.Add(t.Key, t.Value);
                }

                foreach (var u in import.Users)
                {
                    if (Users.ContainsKey(u.Key))
                        Users[u.Key].UpdateUser(u.Value);
                    else
                        Users.Add(u.Key, u.Value);
                }

                foreach (var f in import.Forums)
                {
                    if (Forums.ContainsKey(f.Key))
                        Forums[f.Key].UpdateForum(f.Value);
                    else
                        Forums.Add(f.Key, f.Value);
                }


            }

            progress.Visible = false;
            statusFileCount.Visible = false;

        }
    }
}