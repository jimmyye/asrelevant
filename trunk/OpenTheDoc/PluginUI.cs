using System;
using System.Collections;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI;
using PluginCore;
using System.Xml;
using PluginCore.Helpers;
using PluginCore.Managers;
using System.IO;
using System.Collections.Generic;
using OpenTheDoc.Resources;

namespace OpenTheDoc
{
    public class PluginUI : UserControl
    {
        private const int ICON_BOOK_CLOSED = 0;
        private const int ICON_BOOK_OPEN = 1;
        private const int ICON_PAGE = 2;

        private ImageList imageList;
        private ToolStrip toolStrip;
        private ToolStripButton settingStripButton;
        private ListView relatedTopicsListView;
        private PluginMain pluginMain;

        private ListViewGroup defaultGroup;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton helpContentsStripButton;
        private ListViewItem infoNoTopicsFound;

        public PluginUI(PluginMain pluginMain)
        {
            this.InitializeComponent();
            this.InitializeGraphics();
            this.pluginMain = pluginMain;
            this.InitializeTheOthers();
        }

        #region Windows Forms Designer Generated Code

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.settingStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.helpContentsStripButton = new System.Windows.Forms.ToolStripButton();
            this.relatedTopicsListView = new System.Windows.Forms.ListView();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingStripButton,
            this.toolStripSeparator1,
            this.helpContentsStripButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(200, 25);
            this.toolStrip.TabIndex = 0;
            // 
            // settingStripButton
            // 
            this.settingStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.settingStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.settingStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.settingStripButton.Name = "settingStripButton";
            this.settingStripButton.Size = new System.Drawing.Size(23, 22);
            this.settingStripButton.Text = "Show Settings...";
            this.settingStripButton.Click += new System.EventHandler(this.settingStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // helpContentsStripButton
            // 
            this.helpContentsStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.helpContentsStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.helpContentsStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.helpContentsStripButton.Name = "helpContentsStripButton";
            this.helpContentsStripButton.Size = new System.Drawing.Size(23, 22);
            this.helpContentsStripButton.Text = "Help Contents";
            this.helpContentsStripButton.Click += new System.EventHandler(this.helpContentsStripButton_Click);
            // 
            // relatedTopicsListView
            // 
            this.relatedTopicsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.relatedTopicsListView.HideSelection = false;
            this.relatedTopicsListView.Location = new System.Drawing.Point(0, 25);
            this.relatedTopicsListView.MultiSelect = false;
            this.relatedTopicsListView.Name = "relatedTopicsListView";
            this.relatedTopicsListView.ShowItemToolTips = true;
            this.relatedTopicsListView.Size = new System.Drawing.Size(200, 275);
            this.relatedTopicsListView.TabIndex = 1;
            this.relatedTopicsListView.TileSize = new System.Drawing.Size(280, 20);
            this.relatedTopicsListView.UseCompatibleStateImageBehavior = false;
            this.relatedTopicsListView.View = System.Windows.Forms.View.Tile;
            this.relatedTopicsListView.Resize += new System.EventHandler(this.relatedTopicsListView_Resize);
            this.relatedTopicsListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.relatedTopicsListView_MouseClick);
            this.relatedTopicsListView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.relatedTopicsListView_KeyUp);
            // 
            // PluginUI
            // 
            this.Controls.Add(this.relatedTopicsListView);
            this.Controls.Add(this.toolStrip);
            this.Name = "PluginUI";
            this.Size = new System.Drawing.Size(200, 300);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Methods and Event Handlers

        /// <summary>
        /// Initializes the used graphics
        /// </summary>
        private void InitializeGraphics()
        {
            this.imageList = new ImageList();
            this.imageList.ColorDepth = ColorDepth.Depth32Bit;
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.Add(PluginBase.MainForm.FindImage("92"));     // ICON_BOOK_CLOSED
            this.imageList.Images.Add(PluginBase.MainForm.FindImage("95"));     // ICON_BOOK_OPEN
            this.imageList.Images.Add(PluginBase.MainForm.FindImage("275"));    // ICON_PAGE
            this.imageList.Images.Add(PluginBase.MainForm.FindImage("229"));    // infomation
            this.imageList.Images.Add(PluginBase.MainForm.FindImage("54"));     // setting

            this.relatedTopicsListView.SmallImageList = this.imageList;
            this.relatedTopicsListView.LargeImageList = this.imageList;

            this.helpContentsStripButton.Image = this.imageList.Images[0];
            this.settingStripButton.Image = this.imageList.Images[4];
        }

        private void InitializeTheOthers()
        {
            defaultGroup = new ListViewGroup("Related Topics");
            this.infoNoTopicsFound = new ListViewItem(LocaleHelper.GetString("Info.NoTopicsFound"), 3, defaultGroup);
            this.relatedTopicsListView.Groups.Add(defaultGroup);
            this.relatedTopicsListView.Items.Add(this.infoNoTopicsFound);
        }

        internal void UpdateRelatedTopicsList(List<System.Collections.Specialized.NameValueCollection> relatedTopicsList)
        {
            this.relatedTopicsListView.BeginUpdate();
            this.relatedTopicsListView.Items.Clear();

            if (relatedTopicsList == null || relatedTopicsList.Count == 0)
            {
                this.infoNoTopicsFound.Group = defaultGroup;
                this.relatedTopicsListView.Items.Add(this.infoNoTopicsFound);
            }
            else
            {
                foreach (var relatedTopics in relatedTopicsList)
                {
                    var book = new ListViewGroup(relatedTopics.Get(0));
                    relatedTopicsListView.Groups.Add(book);
                    for (int i = 1; i < relatedTopics.Count; i++)
                    {
                        var topic = new ListViewItem(new string[] { relatedTopics.Get(i), relatedTopics.GetKey(i) }, ICON_PAGE, book);
                        topic.Tag = relatedTopics.GetKey(i);
                        relatedTopicsListView.Items.Add(topic);
                    }
                }
            }

            this.relatedTopicsListView.EndUpdate();
        }

        private string GetSelectedTopicUrl()
        {
            if (relatedTopicsListView.SelectedItems.Count == 0) return null;
            ListViewItem selectedTopic = this.relatedTopicsListView.SelectedItems[0];
            return selectedTopic.Tag as string;
        }

        // Shows the plugin settings
        private void settingStripButton_Click(object sender, EventArgs e)
        {
            PluginBase.MainForm.ShowSettingsDialog("OpenTheDoc");
        }

        private void Open()
        {
            string selectedTopicUrl = this.GetSelectedTopicUrl();
            if (selectedTopicUrl == null) return;
            this.pluginMain.OpenUrl(selectedTopicUrl);
        }
        private void relatedTopicsListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) this.Open();
        }
        private void relatedTopicsListView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.Open();
        }
        private void openStripButton_Click(object sender, EventArgs e)
        {
            this.Open();
        }

        private void OpenInHelpContents()
        {
            string selectedTopicUrl = this.GetSelectedTopicUrl();
            if (selectedTopicUrl == null) return;
            this.pluginMain.OpenHelpContents(selectedTopicUrl);
        }
        private void openInHelpContentsStripButton1_Click(object sender, EventArgs e)
        {
            this.OpenInHelpContents();
        }

        private void helpContentsStripButton_Click(object sender, EventArgs e)
        {
            this.pluginMain.OpenHelpContents(this.pluginMain.HomePage);
        }

        private void relatedTopicsListView_Resize(object sender, EventArgs e)
        {
            int width = this.relatedTopicsListView.Size.Width - 32;
            if (width < 280) width = 280;
            this.relatedTopicsListView.TileSize = new System.Drawing.Size(width, 20);
        }

        #endregion
    }
}
