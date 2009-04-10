using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TocGen
{
    public partial class MainForm : Form
    {
        static private TextBox output;
        
        public MainForm()
        {
            InitializeComponent();
            output = this.outputTextBox;
        }

        private void addPathButton_Click(object sender, EventArgs e)
        {
            foreach (string path in inputTextBox.Lines)
            {
                if (!Directory.Exists(path)) continue;

                string title = TocGen.getTitle(path);

                ListViewItem item = new ListViewItem(new string[] { title, path });
                docPathListView.Items.Add(item);
            }
            inputTextBox.Clear();
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            this.generateButton.Enabled = false;
            tabControl.SelectedTab = outputTabPage;
            this.outputTextBox.Clear();

            foreach (ListViewItem item in docPathListView.Items)
            {
                DateTime begin = DateTime.Now;
                string title = item.Text;
                string path = item.SubItems[1].Text;

                MainForm.Output(title);
                if (File.Exists(Path.Combine(path, TocGen.OTD_TOC)))
                {
                    MainForm.Output(TocGen.OTD_TOC + " exists!");
                    if (this.overWriteCheckBox.Checked)
                        MainForm.Output("Over Write!");
                    else
                    {
                        MainForm.Output("Skip!");
                        MainForm.Output("");
                        continue;
                    }
                }

                TocGen gen = new TocGen(title, path);
                gen.Generate();
                gen.Write();

                MainForm.Output("Elapsed " + (DateTime.Now - begin).TotalSeconds.ToString(".##") + " seconds.");
                MainForm.Output("");
            }

            MainForm.Output("All done!");
            this.generateButton.Enabled = true;
        }

        static public void Output(string text)
        {
            output.AppendText(text + Environment.NewLine);
        }

        private void clearPathsButton_Click(object sender, EventArgs e)
        {
            docPathListView.Items.Clear();
        }
    }
}
