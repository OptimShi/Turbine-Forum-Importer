using Turbine_Form_Importer.Import;

namespace Turbine_Form_Importer
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