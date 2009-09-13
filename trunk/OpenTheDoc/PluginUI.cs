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
using System.Drawing;
using WeifenLuo.WinFormsUI.Docking;

namespace OpenTheDoc
{
    public class PluginUI : UserControl
    {
        private const int ICON_BOOK_CLOSED = 0;
        private const int ICON_BOOK_OPEN = 1;
        private const int ICON_PAGE = 2;

        private ImageList imageList;
        private PluginMain pluginMain;
        private DockPanel dockPanel;

        private CheckBox matchCaseCheckBox;
        private RadioButton containsRadioButton;
        private RadioButton startsWithRadioButton;

        private ToolStripControlHost matchCaseHost;
        private ToolStripControlHost containsHost;
        private ToolStripControlHost startsWithHost;

        private bool isNodeClicked = false;

        private ColumnHeader typeColumnHeader;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        private ColumnHeader titleColumnHeader;
        private ColumnHeader parentColumnHeader;
        private ListView searchResultListView;
        private ColumnHeader bookColumnHeader;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripComboBox searchFieldComboBox;
        private ToolStripButton toggleSearchResultButton;
        private SplitContainer viewSplitContainer;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton collapseOthersStripButton;
        private FixedTreeView contentTree;
        private ToolStrip contentToolStrip;
        private ToolStripLabel contentsLabel;
        private ToolStripButton settingStripButton;
        private ToolStripButton homePageToolStripButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton refreshContentsStripButton;
        private SplitContainer mainSplitContainer;
        private ToolStrip searchToolStrip;
        private ComboBox categoryComboBox;
        private ToolStripLabel searchResultsToolStripLabel;

        public PluginUI(PluginMain pluginMain)
        {
            this.pluginMain = pluginMain;

            this.InitializeComponent();
            this.InitComponent();
            this.InitializeGraphics();
        }

        #region Properties

        private bool IsMatchCase
        {
            get { return this.matchCaseCheckBox.Checked; }
            set { this.matchCaseCheckBox.Checked = value; }
        }

        private bool IsContains
        {
            get { return this.containsRadioButton.Checked; }
            set { this.containsRadioButton.Checked = value; }
        }

        private Settings Settings
        {
            get { return this.pluginMain.Settings as Settings; }
        }

        private string SearchResultCount
        {
            set { this.searchResultsToolStripLabel.Text = "Search Results: " + value; }
        }

        private Category SelectedCategory
        {
            get
            {
                return (this.categoryComboBox.SelectedItem as Category) ?? Settings.CATEGORIES[0]/*All Books*/;
            }
            set
            {
                this.categoryComboBox.SelectedItem = value;
            }
        }

        #endregion

        #region Windows Forms Designer Generated Code

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.categoryComboBox = new System.Windows.Forms.ComboBox();
            this.contentTree = new System.Windows.Forms.FixedTreeView();
            this.contentToolStrip = new System.Windows.Forms.ToolStrip();
            this.contentsLabel = new System.Windows.Forms.ToolStripLabel();
            this.settingStripButton = new System.Windows.Forms.ToolStripButton();
            this.homePageToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshContentsStripButton = new System.Windows.Forms.ToolStripButton();
            this.collapseOthersStripButton = new System.Windows.Forms.ToolStripButton();
            this.viewSplitContainer = new System.Windows.Forms.SplitContainer();
            this.searchResultListView = new System.Windows.Forms.ListView();
            this.titleColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.typeColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.parentColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.bookColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.searchToolStrip = new System.Windows.Forms.ToolStrip();
            this.searchFieldComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toggleSearchResultButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.searchResultsToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.statusStrip.SuspendLayout();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            this.contentToolStrip.SuspendLayout();
            this.viewSplitContainer.Panel1.SuspendLayout();
            this.viewSplitContainer.SuspendLayout();
            this.searchToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 542);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(925, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(35, 17);
            this.statusLabel.Text = "Done";
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.categoryComboBox);
            this.mainSplitContainer.Panel1.Controls.Add(this.contentTree);
            this.mainSplitContainer.Panel1.Controls.Add(this.contentToolStrip);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.viewSplitContainer);
            this.mainSplitContainer.Panel2.Controls.Add(this.searchToolStrip);
            this.mainSplitContainer.Size = new System.Drawing.Size(925, 542);
            this.mainSplitContainer.SplitterDistance = 231;
            this.mainSplitContainer.TabIndex = 2;
            // 
            // categoryComboBox
            // 
            this.categoryComboBox.DisplayMember = "Title";
            this.categoryComboBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.categoryComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.categoryComboBox.Location = new System.Drawing.Point(0, 25);
            this.categoryComboBox.Name = "categoryComboBox";
            this.categoryComboBox.Size = new System.Drawing.Size(231, 21);
            this.categoryComboBox.TabIndex = 2;
            this.categoryComboBox.SelectedIndexChanged += new System.EventHandler(this.categoryComboBox_SelectedIndexChanged);
            // 
            // contentTree
            // 
            this.contentTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.contentTree.FullRowSelect = true;
            this.contentTree.HideSelection = false;
            this.contentTree.Location = new System.Drawing.Point(0, 45);
            this.contentTree.Name = "contentTree";
            this.contentTree.ShowLines = false;
            this.contentTree.ShowNodeToolTips = true;
            this.contentTree.Size = new System.Drawing.Size(231, 499);
            this.contentTree.TabIndex = 0;
            this.contentTree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.contentTree_BeforeExpand);
            this.contentTree.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.contentTree_BeforeCollapse);
            this.contentTree.NodeClicked += new System.Windows.Forms.FixedTreeView.NodeClickedHandler(this.nodeSelected);
            // 
            // contentToolStrip
            // 
            this.contentToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.contentToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsLabel,
            this.settingStripButton,
            this.homePageToolStripButton,
            this.toolStripSeparator1,
            this.refreshContentsStripButton,
            this.collapseOthersStripButton});
            this.contentToolStrip.Location = new System.Drawing.Point(0, 0);
            this.contentToolStrip.Name = "contentToolStrip";
            this.contentToolStrip.Size = new System.Drawing.Size(231, 25);
            this.contentToolStrip.TabIndex = 1;
            // 
            // contentsLabel
            // 
            this.contentsLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.contentsLabel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold);
            this.contentsLabel.Name = "contentsLabel";
            this.contentsLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.contentsLabel.Size = new System.Drawing.Size(69, 22);
            this.contentsLabel.Text = "Contents";
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
            // homePageToolStripButton
            // 
            this.homePageToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.homePageToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.homePageToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.homePageToolStripButton.Name = "homePageToolStripButton";
            this.homePageToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.homePageToolStripButton.Text = "HomePage";
            this.homePageToolStripButton.Click += new System.EventHandler(this.homePageToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // refreshContentsStripButton
            // 
            this.refreshContentsStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.refreshContentsStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.refreshContentsStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refreshContentsStripButton.Name = "refreshContentsStripButton";
            this.refreshContentsStripButton.Size = new System.Drawing.Size(23, 22);
            this.refreshContentsStripButton.Text = "Refresh Contents";
            this.refreshContentsStripButton.Click += new System.EventHandler(this.refreshContentsStripButton_Click);
            // 
            // collapseOthersStripButton
            // 
            this.collapseOthersStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.collapseOthersStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.collapseOthersStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.collapseOthersStripButton.Name = "collapseOthersStripButton";
            this.collapseOthersStripButton.Size = new System.Drawing.Size(23, 22);
            this.collapseOthersStripButton.Text = "Collapse Others";
            this.collapseOthersStripButton.Click += new System.EventHandler(this.collapseOthersStripButton_Click);
            // 
            // viewSplitContainer
            // 
            this.viewSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.viewSplitContainer.Location = new System.Drawing.Point(0, 25);
            this.viewSplitContainer.Name = "viewSplitContainer";
            this.viewSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // viewSplitContainer.Panel1
            // 
            this.viewSplitContainer.Panel1.Controls.Add(this.searchResultListView);
            this.viewSplitContainer.Size = new System.Drawing.Size(690, 517);
            this.viewSplitContainer.SplitterDistance = 107;
            this.viewSplitContainer.TabIndex = 4;
            // 
            // searchResultListView
            // 
            this.searchResultListView.AllowColumnReorder = true;
            this.searchResultListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.titleColumnHeader,
            this.typeColumnHeader,
            this.parentColumnHeader,
            this.bookColumnHeader});
            this.searchResultListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchResultListView.FullRowSelect = true;
            this.searchResultListView.GridLines = true;
            this.searchResultListView.HideSelection = false;
            this.searchResultListView.Location = new System.Drawing.Point(0, 0);
            this.searchResultListView.MultiSelect = false;
            this.searchResultListView.Name = "searchResultListView";
            this.searchResultListView.ShowItemToolTips = true;
            this.searchResultListView.Size = new System.Drawing.Size(690, 107);
            this.searchResultListView.TabIndex = 0;
            this.searchResultListView.UseCompatibleStateImageBehavior = false;
            this.searchResultListView.View = System.Windows.Forms.View.Details;
            this.searchResultListView.SelectedIndexChanged += new System.EventHandler(this.searchResultListView_SelectedIndexChanged);
            // 
            // titleColumnHeader
            // 
            this.titleColumnHeader.Text = "Title";
            this.titleColumnHeader.Width = 250;
            // 
            // typeColumnHeader
            // 
            this.typeColumnHeader.Text = "Type";
            this.typeColumnHeader.Width = 140;
            // 
            // parentColumnHeader
            // 
            this.parentColumnHeader.Text = "Parent";
            this.parentColumnHeader.Width = 200;
            // 
            // bookColumnHeader
            // 
            this.bookColumnHeader.Text = "Book";
            this.bookColumnHeader.Width = 250;
            // 
            // searchToolStrip
            // 
            this.searchToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.searchToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchFieldComboBox,
            this.toolStripSeparator3,
            this.toggleSearchResultButton,
            this.toolStripSeparator2,
            this.searchResultsToolStripLabel});
            this.searchToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.searchToolStrip.Location = new System.Drawing.Point(0, 0);
            this.searchToolStrip.Name = "searchToolStrip";
            this.searchToolStrip.Size = new System.Drawing.Size(690, 25);
            this.searchToolStrip.TabIndex = 3;
            // 
            // searchFieldComboBox
            // 
            this.searchFieldComboBox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.searchFieldComboBox.Name = "searchFieldComboBox";
            this.searchFieldComboBox.Size = new System.Drawing.Size(200, 25);
            this.searchFieldComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.searchFieldComboBox_KeyPress);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toggleSearchResultButton
            // 
            this.toggleSearchResultButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toggleSearchResultButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toggleSearchResultButton.Name = "toggleSearchResultButton";
            this.toggleSearchResultButton.Size = new System.Drawing.Size(23, 22);
            this.toggleSearchResultButton.Text = "Toggle Search Result";
            this.toggleSearchResultButton.Click += new System.EventHandler(this.toggleSearchResultButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // searchResultsToolStripLabel
            // 
            this.searchResultsToolStripLabel.Name = "searchResultsToolStripLabel";
            this.searchResultsToolStripLabel.Size = new System.Drawing.Size(88, 22);
            this.searchResultsToolStripLabel.Text = "Search Results: ";
            // 
            // PluginUI
            // 
            this.Controls.Add(this.mainSplitContainer);
            this.Controls.Add(this.statusStrip);
            this.Name = "PluginUI";
            this.Size = new System.Drawing.Size(925, 564);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel1.PerformLayout();
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            this.mainSplitContainer.Panel2.PerformLayout();
            this.mainSplitContainer.ResumeLayout(false);
            this.contentToolStrip.ResumeLayout(false);
            this.contentToolStrip.PerformLayout();
            this.viewSplitContainer.Panel1.ResumeLayout(false);
            this.viewSplitContainer.ResumeLayout(false);
            this.searchToolStrip.ResumeLayout(false);
            this.searchToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
            this.Load += new EventHandler(PluginUI_Load);
        }

        #endregion

        #region Init

        private void InitComponent()
        {
            //
            // dockPanel
            //
            this.dockPanel = new DockPanel();
            //this.dockPanel.TabIndex = 2;
            this.dockPanel.DocumentStyle = DocumentStyle.DockingWindow;
            this.dockPanel.Dock = DockStyle.Fill;
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.ActiveDocumentChanged += new EventHandler(dockPanel_ActiveDocumentChanged);
            // 
            // matchCaseCheckBox
            // 
            this.matchCaseCheckBox = new CheckBox();
            this.matchCaseCheckBox.Text = "Match Case";
            this.matchCaseCheckBox.BackColor = Color.Transparent;
            this.matchCaseHost = new ToolStripControlHost(this.matchCaseCheckBox);
            this.matchCaseHost.Alignment = ToolStripItemAlignment.Right;
            // 
            // containsRadioButton
            // 
            this.containsRadioButton = new RadioButton();
            this.containsRadioButton.Text = "Contains";
            this.containsRadioButton.BackColor = Color.Transparent;
            this.containsHost = new ToolStripControlHost(this.containsRadioButton);
            this.containsHost.Alignment = ToolStripItemAlignment.Right;
            // 
            // startsWithRadioButton
            // 
            this.startsWithRadioButton = new RadioButton();
            this.startsWithRadioButton.Text = "Starts With";
            this.startsWithRadioButton.BackColor = Color.Transparent;
            this.startsWithHost = new ToolStripControlHost(this.startsWithRadioButton);
            this.startsWithHost.Alignment = ToolStripItemAlignment.Right;

            this.viewSplitContainer.Panel2.Controls.Add(this.dockPanel);
            this.viewSplitContainer.Panel1Collapsed = true;

            this.searchToolStrip.Items.Add(this.startsWithHost);
            this.searchToolStrip.Items.Add(this.containsHost);
            this.searchToolStrip.Items.Add(this.matchCaseHost);
            this.IsContains = true;
        }

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
            this.imageList.Images.Add(PluginBase.MainForm.FindImage("66"));     // refresh
            this.imageList.Images.Add(PluginBase.MainForm.FindImage("275|9|3|2"));     // synchronize
            this.imageList.Images.Add(PluginBase.MainForm.FindImage("47"));     // toggle search result
            this.imageList.Images.Add(PluginBase.MainForm.FindImage("224"));     // home page

            this.contentTree.ImageList = this.imageList;
            this.settingStripButton.Image = this.imageList.Images[4];
            this.refreshContentsStripButton.Image = this.imageList.Images[5];
            this.collapseOthersStripButton.Image = this.imageList.Images[6];
            this.toggleSearchResultButton.Image = this.imageList.Images[7];
            this.homePageToolStripButton.Image = this.imageList.Images[8];
        }

        private void PluginUI_Load(object sender, EventArgs e)
        {
            this.UpateCategoryComboBox();
            RestoreState();
            //this.UpdateContentTree();
        }

        #endregion

        #region Content Tree

        private void UpdateContentTree()
        {
            List<Book> books = this.pluginMain.GetBooks(this.SelectedCategory.Keyword, true);

            this.contentTree.BeginUpdate();
            this.contentTree.Nodes.Clear();

            foreach (Book book in books)
            {
                TreeNode root = new TreeNode();
                root.Text = root.Name = book.Title;
                root.Tag = book.Path;

                this.LoadChildNodes(root, book.Toc);
                this.contentTree.Nodes.Add(root);
            }
            this.contentTree.EndUpdate();
        }

        private void LoadChildNodes(TreeNode treeNode, XmlNode tocNode)
        {
            if (tocNode == null) return;
            if (treeNode.Nodes.Count > 1) return;   // ChildNodes loaded
            if (treeNode.Nodes.Count == 1 && treeNode.Nodes[0].Text != "Shows PlusMinus") return;   // ChildNodes loaded
            treeNode.Nodes.Clear();

            // Loads direct childNodes only, others will be loaded asynchronously
            foreach (XmlNode node in tocNode)
            {
                TreeNode tn = new TreeNode();
                tn.Text = tn.Name = XmlHelper.GetAttribute(node, "label");
                tn.Tag = node;
                treeNode.Nodes.Add(tn);

                if (node.HasChildNodes) tn.Nodes.Add("Shows PlusMinus");   // Makes this treeNode show the PlusMinus ("+", "-")
                else
                {
                    tn.ImageIndex = ICON_PAGE;
                    tn.SelectedImageIndex = ICON_PAGE;
                }
            }
        }

        private TreeNode getRoot(TreeNode node)
        {
            if (node == null) return null;
            if (node.Level == 0) return node;

            return this.getRoot(node.Parent);
        }

        private void nodeSelected(object sender, TreeNode node)
        {
            XmlElement xmlnode = node.Tag as XmlElement;
            if (xmlnode == null) return;
            if (xmlnode.GetAttribute("label") == string.Empty) return;

            TreeNode root = getRoot(node);
            if (root == node || root == null) return;

            string href = xmlnode.GetAttribute("href");
            if (href == string.Empty) return;

            string docPath = root.Tag as string;
            string url = Path.Combine(docPath, href);

            isNodeClicked = true;
            OpenUrl(url);
            pluginMain.DebugPrint("Doc Url:", url);
        }

        // Select a tree node according to the url
        private void SelectTreeNode(string url)
        {
            try
            {
                foreach (TreeNode node in contentTree.Nodes)
                {
                    string bookPath = new Uri(node.Tag as string).ToString();
                    if (!url.ToLower().StartsWith(bookPath.ToLower())) continue;

                    int startIndex = bookPath.Length;
                    if (bookPath[bookPath.Length - 1] != '/') startIndex += 1;
                    string href = url.Substring(startIndex);

                    // Actually, any node is fine, because XPath always search from the root
                    XmlNode toc = (node.Nodes[0].Tag as XmlNode).ParentNode;

                    // Flex Toc uses '?' in href
                    XmlNodeList results = toc.SelectNodes(string.Format("//*[@href='{0}' or @href='{1}']", href, href.Replace('#', '?')));

                    // AS2 Referece of Flash will add "#"+Integer to the end of the href
                    if (results.Count == 0) results = toc.SelectNodes(string.Format("//*[@href='{0}']", href.Split('#')[0]));
                    if (results.Count == 0) continue;

                    this.pluginMain.DebugPrint("result.Count: ", results.Count.ToString());
                    List<string>[] fullPaths = new List<string>[results.Count];
                    List<string> fullPath = null;
                    for (int i = 0; i < results.Count; i++)
                    {
                        XmlNode xn = results[i];
                        fullPath = new List<string>();
                        for (XmlNode n = xn; n != toc.ParentNode; n = n.ParentNode)
                            fullPath.Add(XmlHelper.GetAttribute(n, "label"));

                        fullPath.Reverse();
                        fullPaths[i] = fullPath;

                        this.pluginMain.DebugPrint("fullPaths: ", string.Join("\\", fullPath.ToArray()));
                    }

                    // Find the nearest node to the selectedNode
                    int index = 0;
                    if (contentTree.SelectedNode != null)
                    {
                        string selectedNodeFullPath = contentTree.SelectedNode.FullPath;
                        string[] snp = selectedNodeFullPath.Split('\\');
                        this.pluginMain.DebugPrint("selectedNodeFullPath: ", selectedNodeFullPath);


                        int[] commonCount = new int[fullPaths.Length];
                        for (int i = 0; i < fullPaths.Length; i++)
                        {
                            var fp = fullPaths[i];
                            for (int j = 0; j < fp.Count && j < snp.Length; j++)
                            {
                                if (fp[j].Equals(snp[j]))
                                    commonCount[i]++;
                            }
                            if (commonCount[i] > commonCount[index])
                                index = i;
                        }
                    }

                    this.pluginMain.DebugPrint("result index: ", index.ToString());

                    TreeNodeCollection c = contentTree.Nodes;
                    TreeNode target = null;
                    foreach (string step in fullPaths[index])
                    {
                        target = c[step];
                        if (target == null) break;  // Not found

                        this.LoadChildNodes(target, target.Tag as XmlNode); // Asynchronously
                        c = target.Nodes;
                    }

                    if (target != null)
                    {
                        contentTree.SelectedNode = target;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorManager.ShowError(ex);
            }
        }

        private void contentTree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.ImageIndex = ICON_BOOK_OPEN;
            e.Node.SelectedImageIndex = ICON_BOOK_OPEN;

            this.LoadChildNodes(e.Node, e.Node.Tag as XmlNode); // Asynchronously
        }

        private void contentTree_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.ImageIndex = ICON_BOOK_CLOSED;
            e.Node.SelectedImageIndex = ICON_BOOK_CLOSED;
        }

        private void refreshContentsStripButton_Click(object sender, EventArgs e)
        {
            this.pluginMain.UpdateBookCache();
            this.UpateCategoryComboBox();
            //this.UpdateContentTree();
        }

        private void settingStripButton_Click(object sender, EventArgs e)
        {
            PluginBase.MainForm.ShowSettingsDialog("OpenTheDoc");
        }

        private void homePageToolStripButton_Click(object sender, EventArgs e)
        {
            this.OpenUrl(this.Settings.HomePage);
        }

        private void collapseOthersStripButton_Click(object sender, EventArgs e)
        {
            TreeNode currentNode = this.contentTree.SelectedNode;
            this.contentTree.CollapseAll();
            this.contentTree.SelectedNode = currentNode;
        }

        private void UpateCategoryComboBox()
        {
            this.categoryComboBox.Items.Clear();
            this.categoryComboBox.Items.AddRange(this.Settings.Categories.ToArray());
            if (this.categoryComboBox.Items.Count > 0)
                this.categoryComboBox.SelectedIndex = 0;    // Will send SelectedIndexChanged event
        }

        private void categoryComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateContentTree();
            // TODO: Restore selected node
        }

        #endregion

        #region Content Tree State

        Dictionary<DockContent, TreeState> treeStates = new Dictionary<DockContent, TreeState>();
        private void SaveContentTreeState(DockContent dc)
        {
            if (dc == null) return;

            this.contentTree.SaveExpandedState();
            this.contentTree.SaveScrollState();
            TreeState state = this.contentTree.State;

            TreeState stateClone = new TreeState
            {
                highlight = this.contentTree.SelectedNode.FullPath,
                TopPath = state.TopPath,
                BottomPath = state.BottomPath,
                ExpandedPaths = new ArrayList(state.ExpandedPaths)
            };

            PluginMain.DebugPrint("Save: " + stateClone.highlight);
            if (treeStates.ContainsKey(dc))
                treeStates[dc] = stateClone;
            else
                treeStates.Add(dc, stateClone);
        }

        private void RestoreContentTreeState(DockContent dc)
        {
            TreeState state = treeStates[dc];
            this.contentTree.BeginStatefulUpdate();
            this.contentTree.CollapseAll();
            this.contentTree.EndStatefulUpdate(state);

            if (state.highlight != null)
            {
                TreeNode toHighligh = this.contentTree.FindClosestPath(state.highlight);
                if (toHighligh != null) this.contentTree.SelectedNode = toHighligh;
            }
            // Have to RestoreScrollState again after set SelectNode
            this.contentTree.RestoreScrollState();
        }

        #endregion

        #region Tabs

        private DockContent CreateDockContent(string url)
        {
            DockContent dc = new DockContent();
            dc.DockAreas = DockAreas.Document;
            dc.Text = "New Tab";
            dc.Controls.Add(CreateBrowser(url));
            dc.Show(this.dockPanel);
            
            return dc;
        }

        private DockContent previousActiveDockContent = null;
        private DockContent CurrentActiveDockContent
        {
            get
            {
                return this.dockPanel.ActiveDocument as DockContent;
            }
        }

        private void dockPanel_ActiveDocumentChanged(object sender, EventArgs e)
        {
            PluginMain.DebugPrint("ActiveDocumentChanged");
            if (this.CurrentActiveDockContent == null) return;
            
            if (treeStates.ContainsKey(this.CurrentActiveDockContent))
            {
                // Save state
                if (this.previousActiveDockContent != null)
                {
                    PluginMain.DebugPrint("save prev");
                    SaveContentTreeState(this.previousActiveDockContent);
                }
                // Restore state
                RestoreContentTreeState(this.CurrentActiveDockContent);
            }
            else
                SaveContentTreeState(this.CurrentActiveDockContent);

            this.previousActiveDockContent = this.CurrentActiveDockContent;
            this.CurrentActiveDockContent.Activate();
        }

        #endregion

        #region Browser

        private Browser CreateBrowser(string url)
        {
            Browser b = new Browser();
            b.Dock = DockStyle.Fill;
            b.WebBrowser.PreviewKeyDown += new PreviewKeyDownEventHandler(WebBrowser_PreviewKeyDown);
            b.WebBrowser.StatusTextChanged += new EventHandler(WebBrowser_StatusTextChanged);
            b.WebBrowser.Navigating += new WebBrowserNavigatingEventHandler(WebBrowser_Navigating);
            b.WebBrowser.Navigate(url);

            return b;
        }

        private Browser CurrentActiveBrowser
        {
            get
            {
                try
                {
                    Browser b = this.CurrentActiveDockContent.Controls[0] as Browser;
                    return b;
                }
                catch
                {
                    return null;
                }
            }
        }

        public void OpenUrl(string url)
        {
            OpenUrl(url, false);
        }

        public void OpenUrl(string url, bool newTab)
        {
            if (newTab || CurrentActiveBrowser == null)
            {
                SaveContentTreeState(this.previousActiveDockContent);
                CreateDockContent(url);
            }
            else
                CurrentActiveBrowser.WebBrowser.Navigate(url);
        }

        private void WebBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (isNodeClicked)
                isNodeClicked = false;
            else
                SelectTreeNode(e.Url.ToString());
        }

        private void WebBrowser_StatusTextChanged(object sender, EventArgs e)
        {
            this.statusLabel.Text = (sender as WebBrowser).StatusText;
        }

        // Hide HelpPanel when one of the shortcuts is pressed when WebBrowser is focused
        private void WebBrowser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == this.Settings.ShortcutHelpPanel ||
                e.KeyData == this.Settings.ShortcutCurrentTab ||
                e.KeyData == this.Settings.ShortcutNewTab)
                this.pluginMain.WindowVisible = false;
        }

        #endregion

        #region Search

        // Open a topic selected in search results
        private void searchResultListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.searchResultListView.SelectedItems.Count < 1) return;

            string url = this.searchResultListView.SelectedItems[0].Tag as string;
            OpenUrl(url);
        }

        private void toggleSearchResultButton_Click(object sender, EventArgs e)
        {
            this.viewSplitContainer.Panel1Collapsed = !this.viewSplitContainer.Panel1Collapsed;
        }

        // Begins title search when "Enter" is pressed
        private void searchFieldComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (Char)Keys.Return || this.searchFieldComboBox.Text.Trim() == "") return;
            string sText = this.searchFieldComboBox.Text;

            // search history list
            if (!this.searchFieldComboBox.Items.Contains(sText))
                this.searchFieldComboBox.Items.Insert(0, sText);
            if (this.searchFieldComboBox.Items.Count > 5)
                this.searchFieldComboBox.Items.RemoveAt(5);

            List<SearchResult> resultList = this.pluginMain.TitleSearch(sText, this.IsContains, this.IsMatchCase, this.SelectedCategory.Keyword);
            this.UpdateSearchResultList(resultList, true);
            this.searchResultListView.Focus();
            e.Handled = true;
        }

        internal void UpdateSearchResultList(List<SearchResult> resultList, bool showSearchResults)
        {
            this.searchResultListView.Items.Clear();
            foreach (SearchResult r in resultList)
            {
                ListViewItem item = new ListViewItem(new string[] { r.title, r.type, r.parentTitle, r.bookTitle });
                item.Tag = r.filePath;
                this.searchResultListView.Items.Add(item);
            }
            this.SearchResultCount = resultList.Count.ToString();

            this.viewSplitContainer.Panel1Collapsed = !showSearchResults;
        }

        #endregion

        #region State

        internal void Reset()
        {
            this.UpdateContentTree();
            this.UpdateSearchResultList(new List<SearchResult>(), false);
        }

        // Call by PluginMain.SaveSettings()
        internal void SaveState()
        {
            this.Settings.MainSplitContainerSplitterDistance = this.mainSplitContainer.SplitterDistance;
            this.Settings.ViewSplitContainerSplitterDistance = this.viewSplitContainer.SplitterDistance;
            this.Settings.SelectedCategory = this.SelectedCategory;
        }

        private void RestoreState()
        {
            this.mainSplitContainer.SplitterDistance = this.Settings.MainSplitContainerSplitterDistance;
            this.viewSplitContainer.SplitterDistance = this.Settings.ViewSplitContainerSplitterDistance;
            this.SelectedCategory = this.Settings.SelectedCategory;
        }

        #endregion
    }
}
