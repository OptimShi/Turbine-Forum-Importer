namespace Turbine_Forum_Importer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.progress = new System.Windows.Forms.ToolStripProgressBar();
            this.statusFileCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textStatus = new System.Windows.Forms.TextBox();
            this.btnDoImport = new System.Windows.Forms.Button();
            this.btnOrganize = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progress,
            this.statusFileCount});
            this.statusStrip1.Location = new System.Drawing.Point(0, 690);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 17, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1385, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // progress
            // 
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(600, 29);
            this.progress.Visible = false;
            // 
            // statusFileCount
            // 
            this.statusFileCount.Name = "statusFileCount";
            this.statusFileCount.Size = new System.Drawing.Size(206, 30);
            this.statusFileCount.Text = "toolStripStatusLabel1";
            this.statusFileCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.statusFileCount.Visible = false;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.btnOrganize);
            this.panel1.Controls.Add(this.textStatus);
            this.panel1.Controls.Add(this.btnDoImport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1385, 690);
            this.panel1.TabIndex = 4;
            // 
            // textStatus
            // 
            this.textStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textStatus.Location = new System.Drawing.Point(0, 154);
            this.textStatus.Margin = new System.Windows.Forms.Padding(4);
            this.textStatus.Multiline = true;
            this.textStatus.Name = "textStatus";
            this.textStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textStatus.Size = new System.Drawing.Size(1384, 532);
            this.textStatus.TabIndex = 3;
            // 
            // btnDoImport
            // 
            this.btnDoImport.Location = new System.Drawing.Point(26, 43);
            this.btnDoImport.Margin = new System.Windows.Forms.Padding(4);
            this.btnDoImport.Name = "btnDoImport";
            this.btnDoImport.Size = new System.Drawing.Size(134, 41);
            this.btnDoImport.TabIndex = 2;
            this.btnDoImport.Text = "Import";
            this.btnDoImport.UseVisualStyleBackColor = true;
            this.btnDoImport.Click += new System.EventHandler(this.btnDoImport_Click);
            // 
            // btnOrganize
            // 
            this.btnOrganize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOrganize.Location = new System.Drawing.Point(1228, 63);
            this.btnOrganize.Name = "btnOrganize";
            this.btnOrganize.Size = new System.Drawing.Size(131, 40);
            this.btnOrganize.TabIndex = 4;
            this.btnOrganize.Text = "Organize";
            this.btnOrganize.UseVisualStyleBackColor = true;
            this.btnOrganize.Click += new System.EventHandler(this.btnOrganize_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1385, 712);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private StatusStrip statusStrip1;
        private ToolStripProgressBar progress;
        private Panel panel1;
        private TextBox textStatus;
        private Button btnDoImport;
        private ToolStripStatusLabel statusFileCount;
        private Button btnOrganize;
    }
}