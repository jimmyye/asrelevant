using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using PluginCore;
using PluginCore.Helpers;
using PluginCore.Managers;

namespace OpenTheDoc
{
    public partial class HelpContentsForm : Form
    {
        private const int ICON_BOOK_CLOSED = 0;
        private const int ICON_BOOK_OPEN = 1;
        private const int ICON_PAGE = 2;

        private ImageList imageList;
        private PluginMain pluginMain;
        private Browser browser;          // html document browser

        private CheckBox matchCaseCheckBox;
        private RadioButton containsRadioButton;
        private RadioButton startsWithRadioButton;

        private ToolStripControlHost matchCaseHost;
        private ToolStripControlHost containsHost;
        private ToolStripControlHost startsWithHost;

        private bool isNodeClicked = false;

        public HelpContentsForm(PluginMain pluginMain)
        {
            this.InitializeComponent();
            this.InitOtherComponent();
            this.InitializeGraphics();
            this.pluginMain = pluginMain;
            this.viewSplitContainer.Panel1Collapsed = true;

            this.UpdateContentTree();
        }

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

        #region Init

        private void InitOtherComponent()
        {
            // 
            // browser
            // 
            this.browser = new Browser();
            this.browser.Dock = DockStyle.Fill;
            this.browser.WebBrowser.ScriptErrorsSuppressed = true;
            this.browser.WebBrowser.StatusTextChanged += new EventHandler(WebBrowser_StatusTextChanged);
            this.browser.WebBrowser.Navigating += new WebBrowserNavigatingEventHandler(WebBrowser_Navigating);
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

            this.viewSplitContainer.Panel2.Controls.Add(this.browser);

            this.searchToolStrip.Items.Add(this.startsWithHost);
            this.searchToolStrip.Items.Add(this.containsHost);
            this.searchToolStrip.Items.Add(this.matchCaseHost);
            this.IsContains = true;
        }

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

            // Uses FlashDevelop's icon
            this.Icon = (PluginBase.MainForm as Form).Icon;
        }

        #endregion

        #region ContentTree

        private void UpdateContentTree()
        {
            List<Book> books = this.pluginMain.GetBooks();

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

            TreeNode root = this.getRoot(node);
            if (root == node || root == null) return;

            string href = xmlnode.GetAttribute("href");
            if (href == string.Empty) return;

            string docPath = root.Tag as string;
            string url = Path.Combine(docPath, href);

            isNodeClicked = true;
            this.OpenUrl(url);
            this.pluginMain.DebugPrint("Doc Url:", url);
        }

        public void OpenUrl(string url)
        {
            this.browser.WebBrowser.Navigate(url);
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
            this.UpdateContentTree();
        }

        private void settingStripButton_Click(object sender, EventArgs e)
        {
            PluginBase.MainForm.ShowSettingsDialog("OpenTheDoc");
        }

        private void homePageToolStripButton_Click(object sender, EventArgs e)
        {
            this.OpenUrl(this.pluginMain.HomePage);
        }

        private void collapseOthersStripButton_Click(object sender, EventArgs e)
        {
            TreeNode currentNode = this.contentTree.SelectedNode;
            this.contentTree.CollapseAll();
            this.contentTree.SelectedNode = currentNode;
        }

        #endregion

        #region Browser

        private void WebBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (isNodeClicked)
            {
                isNodeClicked = false;
                return;
            }
            try
            {
                string url = e.Url.ToString();
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

                    // Find the nearest one to the selectedNode
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

        private void WebBrowser_StatusTextChanged(object sender, EventArgs e)
        {
            this.statusLabel.Text = this.browser.WebBrowser.StatusText;
        }

        #endregion

        #region Search
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

        private void searchFieldComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (Char)Keys.Return || this.searchFieldComboBox.Text.Trim() == "") return;
            this.viewSplitContainer.Panel1Collapsed = false;
            this.searchResultListView.Focus();

            string sText = this.searchFieldComboBox.Text;
            string xpath;

            System.Text.StringBuilder xpathStringFormat = new System.Text.StringBuilder("//*[");

            if (this.IsContains) xpathStringFormat.Append("contains");
            else xpathStringFormat.Append("starts-with");

            if (this.IsMatchCase)
            {
                xpathStringFormat.Append("(@label,'{0}')]");
                xpath = String.Format(xpathStringFormat.ToString(), sText);
            }
            else
            {
                xpathStringFormat.Append("(translate(@label,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ'), '{0}')]");
                xpath = String.Format(xpathStringFormat.ToString(), sText.ToUpper());
            }

            if (!this.searchFieldComboBox.Items.Contains(sText))
                this.searchFieldComboBox.Items.Insert(0, sText);
            if (this.searchFieldComboBox.Items.Count > 5)
                this.searchFieldComboBox.Items.RemoveAt(5);

            List<Book> books = this.pluginMain.GetBooks();
            
            this.searchResultListView.Items.Clear();
            foreach (Book book in books)
            {
                string bookPath = book.Path;
                XmlNode toc = book.Toc;

                XmlNodeList results = toc.SelectNodes(xpath);
                if (results.Count == 0) continue;

                //System.Collections.Specialized.NameValueCollection relatedTopics = new System.Collections.Specialized.NameValueCollection();
                List<string> temp = new List<string>();
                string bookTitle = book.Title;

                foreach (XmlElement node in results)
                {
                    string href = node.GetAttribute("href");
                    string file = Path.Combine(bookPath, href);
                    if (temp.Contains(file)) continue;

                    string label = node.GetAttribute("label");
                    XmlElement parent = node.ParentNode as XmlElement;
                    string parentLabel = parent.GetAttribute("label");
                    string type = "";

                    if (parentLabel.EndsWith("Properties") ||
                        parentLabel.EndsWith("Constants") ||
                        parentLabel.EndsWith("Methods") ||
                        parentLabel.Equals("Styles") ||
                        parentLabel.Equals("Events") ||
                        parentLabel.Equals("Constructor"))
                    {
                        type = parentLabel;
                        parentLabel = (parent.ParentNode as XmlElement).GetAttribute("label");
                    }

                    ListViewItem item = new ListViewItem(new string[] { label, type, parentLabel, bookTitle });
                    item.Tag = file;
                    this.searchResultListView.Items.Add(item);

                    temp.Add(file);
                }
            }
            e.Handled = true;
        }
        #endregion

    }
}
