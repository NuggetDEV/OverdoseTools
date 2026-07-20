namespace TOD_Localization_Tool
{
    partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.TxtMainPath = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.ImportTexts = new System.Windows.Forms.Button();
            this.ExportTexts = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.subtitleLabel = new System.Windows.Forms.Label();
            this.headerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(22, 27, 34);
            this.headerPanel.Controls.Add(this.titleLabel);
            this.headerPanel.Controls.Add(this.subtitleLabel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(420, 64);
            this.headerPanel.TabIndex = 9;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.titleLabel.ForeColor = System.Drawing.Color.FromArgb(88, 166, 255);
            this.titleLabel.Location = new System.Drawing.Point(16, 10);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(294, 32);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "TOD Text Editor";
            // 
            // subtitleLabel
            // 
            this.subtitleLabel.AutoSize = true;
            this.subtitleLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.subtitleLabel.ForeColor = System.Drawing.Color.FromArgb(139, 148, 158);
            this.subtitleLabel.Location = new System.Drawing.Point(19, 42);
            this.subtitleLabel.Name = "subtitleLabel";
            this.subtitleLabel.Size = new System.Drawing.Size(130, 15);
            this.subtitleLabel.TabIndex = 1;
            this.subtitleLabel.Text = "Total Overdose - Text Tool";
            // 
            // ExportTexts
            // 
            this.ExportTexts.BackColor = System.Drawing.Color.FromArgb(35, 134, 54);
            this.ExportTexts.FlatAppearance.BorderSize = 0;
            this.ExportTexts.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(46, 160, 67);
            this.ExportTexts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExportTexts.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.ExportTexts.ForeColor = System.Drawing.Color.White;
            this.ExportTexts.Location = new System.Drawing.Point(14, 78);
            this.ExportTexts.Name = "ExportTexts";
            this.ExportTexts.Size = new System.Drawing.Size(190, 55);
            this.ExportTexts.TabIndex = 6;
            this.ExportTexts.Text = "📤  Extract .txt Files";
            this.ExportTexts.UseVisualStyleBackColor = false;
            this.ExportTexts.Click += new System.EventHandler(this.ExportTexts_Click);
            // 
            // ImportTexts
            // 
            this.ImportTexts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ImportTexts.BackColor = System.Drawing.Color.FromArgb(212, 76, 71);
            this.ImportTexts.FlatAppearance.BorderSize = 0;
            this.ImportTexts.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(237, 86, 80);
            this.ImportTexts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ImportTexts.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.ImportTexts.ForeColor = System.Drawing.Color.White;
            this.ImportTexts.Location = new System.Drawing.Point(216, 78);
            this.ImportTexts.Name = "ImportTexts";
            this.ImportTexts.Size = new System.Drawing.Size(192, 55);
            this.ImportTexts.TabIndex = 7;
            this.ImportTexts.Text = "🔨  Rebuild Main File";
            this.ImportTexts.UseVisualStyleBackColor = false;
            this.ImportTexts.Click += new System.EventHandler(this.ImportTexts_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(139, 148, 158);
            this.label1.Location = new System.Drawing.Point(12, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Main file path:";
            // 
            // TxtMainPath
            // 
            this.TxtMainPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtMainPath.BackColor = System.Drawing.Color.FromArgb(13, 17, 23);
            this.TxtMainPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtMainPath.ForeColor = System.Drawing.Color.FromArgb(201, 209, 217);
            this.TxtMainPath.Location = new System.Drawing.Point(14, 164);
            this.TxtMainPath.Name = "TxtMainPath";
            this.TxtMainPath.Size = new System.Drawing.Size(293, 22);
            this.TxtMainPath.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.FromArgb(33, 38, 45);
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(58, 65, 78);
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(48, 54, 61);
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.FromArgb(201, 209, 217);
            this.button1.Location = new System.Drawing.Point(311, 163);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 24);
            this.button1.TabIndex = 1;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBox1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(201, 209, 217);
            this.checkBox1.Location = new System.Drawing.Point(14, 192);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(194, 19);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Create a backup of current .main archive";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(13, 17, 23);
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Font = new System.Drawing.Font("Consolas", 9F);
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(139, 148, 158);
            this.textBox1.Location = new System.Drawing.Point(14, 218);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(394, 265);
            this.textBox1.TabIndex = 8;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(13, 17, 23);
            this.ClientSize = new System.Drawing.Size(420, 495);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.TxtMainPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ImportTexts);
            this.Controls.Add(this.ExportTexts);
            this.Controls.Add(this.headerPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
            this.Name = "FrmMain";
            this.Text = "TOD Text Editor";
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label subtitleLabel;
        private System.Windows.Forms.TextBox TxtMainPath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button ImportTexts;
        private System.Windows.Forms.Button ExportTexts;
        private System.Windows.Forms.TextBox textBox1;
    }
}
