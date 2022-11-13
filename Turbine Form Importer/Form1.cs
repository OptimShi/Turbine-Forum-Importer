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
            path = @"D:\Web Development\Turbine\new_forums\unique_samples\";
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            statusFileCount.Text = files.Length + " Files Found.";
            progress.Maximum = files.Length;
            progress.Value = 0;
            progress.Step = 1;

            progress.Visible = true;
            statusFileCount.Visible = true;

            Dictionary<uint, Post> Posts = new Dictionary<uint, Post>();
            Dictionary<uint, ForumThread> Threads = new Dictionary<uint, ForumThread>();
            Dictionary<uint, User> Users = new Dictionary<uint, User>();
            Dictionary<uint, Forum> Fourms = new Dictionary<uint, Forum>();

            foreach (string file in files)
            {
                progress.PerformStep();

                var import = new FileImporter(file);
                string basename = Path.GetFileName(file);
                textStatus.AppendText(basename + " -- " + import.Template + Environment.NewLine);
            }

            progress.Visible = false;
            statusFileCount.Visible = false;

        }
    }
}