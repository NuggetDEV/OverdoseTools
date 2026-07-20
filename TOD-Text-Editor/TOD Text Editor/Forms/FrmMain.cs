using Helper;

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TOD_Localization_Tool
{
    public partial class FrmMain : Form
    {


        MainAsset main;
        public FrmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Filter = "Main file(*.main)|*.main";

            if (OFD.ShowDialog() == DialogResult.OK)
            {
                TxtMainPath.Text = OFD.FileName;
            }
        }

        private void LoadMainFile()
        {
            if (main != null)
            {
                main.Dispose();
            }
            if (!File.Exists(TxtMainPath.Text))
            {
                throw new Exception("Can't find main file!\n\n make sure you select right file.");
            }

            if (checkBox1.Checked)
            {
                if (!File.Exists(TxtMainPath.Text + ".bak"))
                {
                    File.Copy(TxtMainPath.Text, TxtMainPath.Text + ".bak");
                }
            }

            //if (File.Exists(TxtMainPath.Text + ".bak"))
            //{
            //    File.Copy(TxtMainPath.Text + ".bak",TxtMainPath.Text,true);
            //}

            main = new MainAsset(new MStream(TxtMainPath.Text));

            ClearLog();
        }

        private string GetFolderPath()
        {

            FolderDialog folderDialog = new FolderDialog();

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                return folderDialog.FileName;
            }
            else
            {
                return null;
            }

        }

        private void ExportTexts_Click(object sender, EventArgs e)
        {
            try
            {
                LoadMainFile();
                string FolderPath = GetFolderPath();
                if (FolderPath == null) return;

                Directory.SetCurrentDirectory(FolderPath);

                foreach (var entry in main.FilesEntres.Where(x => x.Key.EndsWith(".txt", StringComparison.InvariantCultureIgnoreCase)))
                {
                    var fontasset = main.GetFile(entry.Value) as TextAsset;
                    Directory.CreateDirectory(Path.GetDirectoryName(entry.Key));
                    File.WriteAllLines(entry.Key, fontasset.GetStringFormFile());
                }

                LogLine("Successfully extracted to " + FolderPath + "!");
            }
            catch (Exception ex)
            {
                LogLine("Extraction FAILED: " + ex.Message);
            }
        }

        private void ImportTexts_Click(object sender, EventArgs e)
        {
            try
            {
                LoadMainFile();
                string FolderPath = GetFolderPath();
                if (FolderPath == null) return;

                Directory.SetCurrentDirectory(FolderPath);
                foreach (var entry in main.FilesEntres.Where(x => x.Key.EndsWith(".txt")))
                {
                    var fontasset = main.GetFile(entry.Value) as TextAsset;

                    if (!File.Exists(entry.Key))
                        continue;

                    fontasset.EditFile(File.ReadAllText(entry.Key));
                }
                main.SaveFile(TxtMainPath.Text);

                if (TOD_Localization_Tool.TextAsset.EditWarnings.Any())
                {
                    LogLine("Successfully rebuilt at " + TxtMainPath.Text + "!");
                    LogLine("WARNINGS:");
                    foreach (var w in TOD_Localization_Tool.TextAsset.EditWarnings)
                        LogLine("  " + w);
                }
                else
                {
                    LogLine("Successfully rebuilt at " + TxtMainPath.Text + "!");
                }
            }
            catch (Exception ex)
            {
                LogLine("Rebuild FAILED: " + ex.Message);
            }
        }



        private void Log(string text)
        {
            textBox1.AppendText(text);
        }
        private void LogLine(string text)
        {
            textBox1.AppendText(text + "\r\n");
        }

        private void ClearLog()
        {
            textBox1.Clear();
        }
    }
}
