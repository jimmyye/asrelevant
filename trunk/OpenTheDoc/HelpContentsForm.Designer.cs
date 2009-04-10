namespace OpenTheDoc
{
    partial class HelpContentsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.contentTree = new System.Windows.Forms.FixedTreeView();
            this.contentToolStrip = new System.Windows.Forms.ToolStrip();
            this.contentsLabel = new System.Windows.Forms.ToolStripLabel();
            this.settingStripButton = new System.Windows.Forms.ToolStripButton();
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
            this.titleSearchToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.homePageToolStripButton = new System.Windows.Forms.ToolStripButton();
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
            this.statusStrip.Location = new System.Drawing.Point(0, 726);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1111, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(39, 17);
            this.statusLabel.Text = "Done";
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.contentTree);
            this.mainSplitContainer.Panel1.Controls.Add(this.contentToolStrip);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.viewSplitContainer);
            this.mainSplitContainer.Panel2.Controls.Add(this.searchToolStrip);
            this.mainSplitContainer.Size = new System.Drawing.Size(1111, 726);
            this.mainSplitContainer.SplitterDistance = 278;
            this.mainSplitContainer.TabIndex = 2;
            // 
            // contentTree
            // 
            this.contentTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentTree.FullRowSelect = true;
            this.contentTree.HideSelection = false;
            this.contentTree.Location = new System.Drawing.Point(0, 25);
            this.contentTree.Name = "contentTree";
            this.contentTree.ShowLines = false;
            this.contentTree.ShowNodeToolTips = true;
            this.contentTree.Size = new System.Drawing.Size(278, 701);
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
            this.contentToolStrip.Size = new System.Drawing.Size(278, 25);
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
            this.viewSplitContainer.Location = new System.Drawing.Point(0, 25);
            this.viewSplitContainer.Name = "viewSplitContainer";
            this.viewSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // viewSplitContainer.Panel1
            // 
            this.viewSplitContainer.Panel1.Controls.Add(this.searchResultListView);
            this.viewSplitContainer.Size = new System.Drawing.Size(829, 701);
            this.viewSplitContainer.SplitterDistance = 276;
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
            this.searchResultListView.Size = new System.Drawing.Size(829, 276);
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
            this.titleSearchToolStripLabel});
            this.searchToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.searchToolStrip.Location = new System.Drawing.Point(0, 0);
            this.searchToolStrip.Name = "searchToolStrip";
            this.searchToolStrip.Size = new System.Drawing.Size(829, 25);
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
            // titleSearchToolStripLabel
            // 
            this.titleSearchToolStripLabel.Name = "titleSearchToolStripLabel";
            this.titleSearchToolStripLabel.Size = new System.Drawing.Size(75, 22);
            this.titleSearchToolStripLabel.Text = "Title Search";
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
            // HelpContentsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1111, 748);
            this.Controls.Add(this.mainSplitContainer);
            this.Controls.Add(this.statusStrip);
            this.Name = "HelpContentsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Help - FlashDevelop";
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

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.FixedTreeView contentTree;
        private System.Windows.Forms.ToolStrip contentToolStrip;
        private System.Windows.Forms.ToolStripLabel contentsLabel;
        private System.Windows.Forms.ToolStripButton settingStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton refreshContentsStripButton;
        private System.Windows.Forms.ToolStripButton collapseOthersStripButton;
        private System.Windows.Forms.SplitContainer viewSplitContainer;
        private System.Windows.Forms.ListView searchResultListView;
        private System.Windows.Forms.ColumnHeader titleColumnHeader;
        private System.Windows.Forms.ColumnHeader parentColumnHeader;
        private System.Windows.Forms.ColumnHeader bookColumnHeader;
        private System.Windows.Forms.ToolStrip searchToolStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toggleSearchResultButton;
        private System.Windows.Forms.ToolStripComboBox searchFieldComboBox;
        private System.Windows.Forms.ColumnHeader typeColumnHeader;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel titleSearchToolStripLabel;
        private System.Windows.Forms.ToolStripButton homePageToolStripButton;
    }
}