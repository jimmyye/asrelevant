namespace TocGen
{
    partial class MainForm
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
            this.generateButton = new System.Windows.Forms.Button();
            this.addPathButton = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.docPathTabPage = new System.Windows.Forms.TabPage();
            this.tipsLabel = new System.Windows.Forms.Label();
            this.inputLabel = new System.Windows.Forms.Label();
            this.docPathListView = new System.Windows.Forms.ListView();
            this.titleColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.pathColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.outputTabPage = new System.Windows.Forms.TabPage();
            this.outputTextBox = new System.Windows.Forms.TextBox();
            this.clearPathsButton = new System.Windows.Forms.Button();
            this.overWriteCheckBox = new System.Windows.Forms.CheckBox();
            this.tabControl.SuspendLayout();
            this.docPathTabPage.SuspendLayout();
            this.outputTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // generateButton
            // 
            this.generateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.generateButton.Location = new System.Drawing.Point(592, 439);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(75, 25);
            this.generateButton.TabIndex = 2;
            this.generateButton.Text = "Generate";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // addPathButton
            // 
            this.addPathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addPathButton.Location = new System.Drawing.Point(12, 439);
            this.addPathButton.Name = "addPathButton";
            this.addPathButton.Size = new System.Drawing.Size(87, 25);
            this.addPathButton.TabIndex = 5;
            this.addPathButton.Text = "Add to List";
            this.addPathButton.UseVisualStyleBackColor = true;
            this.addPathButton.Click += new System.EventHandler(this.addPathButton_Click);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.docPathTabPage);
            this.tabControl.Controls.Add(this.outputTabPage);
            this.tabControl.Location = new System.Drawing.Point(12, 13);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(655, 419);
            this.tabControl.TabIndex = 6;
            // 
            // docPathTabPage
            // 
            this.docPathTabPage.Controls.Add(this.tipsLabel);
            this.docPathTabPage.Controls.Add(this.inputLabel);
            this.docPathTabPage.Controls.Add(this.docPathListView);
            this.docPathTabPage.Controls.Add(this.inputTextBox);
            this.docPathTabPage.Location = new System.Drawing.Point(4, 22);
            this.docPathTabPage.Name = "docPathTabPage";
            this.docPathTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.docPathTabPage.Size = new System.Drawing.Size(647, 393);
            this.docPathTabPage.TabIndex = 0;
            this.docPathTabPage.Text = "Documentation Path";
            this.docPathTabPage.UseVisualStyleBackColor = true;
            // 
            // tipsLabel
            // 
            this.tipsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tipsLabel.AutoSize = true;
            this.tipsLabel.Location = new System.Drawing.Point(348, 235);
            this.tipsLabel.Name = "tipsLabel";
            this.tipsLabel.Size = new System.Drawing.Size(216, 13);
            this.tipsLabel.TabIndex = 5;
            this.tipsLabel.Text = "Select an item and then click to edit the title.";
            // 
            // inputLabel
            // 
            this.inputLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.inputLabel.AutoSize = true;
            this.inputLabel.Location = new System.Drawing.Point(-2, 235);
            this.inputLabel.Name = "inputLabel";
            this.inputLabel.Size = new System.Drawing.Size(90, 13);
            this.inputLabel.TabIndex = 4;
            this.inputLabel.Text = "Input Paths Here:";
            // 
            // docPathListView
            // 
            this.docPathListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.docPathListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.titleColumnHeader,
            this.pathColumnHeader});
            this.docPathListView.FullRowSelect = true;
            this.docPathListView.GridLines = true;
            this.docPathListView.LabelEdit = true;
            this.docPathListView.Location = new System.Drawing.Point(0, 0);
            this.docPathListView.MultiSelect = false;
            this.docPathListView.Name = "docPathListView";
            this.docPathListView.ShowItemToolTips = true;
            this.docPathListView.Size = new System.Drawing.Size(647, 232);
            this.docPathListView.TabIndex = 3;
            this.docPathListView.UseCompatibleStateImageBehavior = false;
            this.docPathListView.View = System.Windows.Forms.View.Details;
            // 
            // titleColumnHeader
            // 
            this.titleColumnHeader.Text = "Title";
            this.titleColumnHeader.Width = 320;
            // 
            // pathColumnHeader
            // 
            this.pathColumnHeader.Text = "Path";
            this.pathColumnHeader.Width = 320;
            // 
            // inputTextBox
            // 
            this.inputTextBox.AcceptsReturn = true;
            this.inputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.inputTextBox.Location = new System.Drawing.Point(0, 255);
            this.inputTextBox.Multiline = true;
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(647, 136);
            this.inputTextBox.TabIndex = 2;
            // 
            // outputTabPage
            // 
            this.outputTabPage.Controls.Add(this.outputTextBox);
            this.outputTabPage.Location = new System.Drawing.Point(4, 22);
            this.outputTabPage.Name = "outputTabPage";
            this.outputTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.outputTabPage.Size = new System.Drawing.Size(647, 393);
            this.outputTabPage.TabIndex = 1;
            this.outputTabPage.Text = "Output";
            this.outputTabPage.UseVisualStyleBackColor = true;
            // 
            // outputTextBox
            // 
            this.outputTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputTextBox.Location = new System.Drawing.Point(3, 3);
            this.outputTextBox.Multiline = true;
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.Size = new System.Drawing.Size(641, 387);
            this.outputTextBox.TabIndex = 2;
            // 
            // clearPathsButton
            // 
            this.clearPathsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.clearPathsButton.Location = new System.Drawing.Point(105, 439);
            this.clearPathsButton.Name = "clearPathsButton";
            this.clearPathsButton.Size = new System.Drawing.Size(81, 25);
            this.clearPathsButton.TabIndex = 5;
            this.clearPathsButton.Text = "Clear List";
            this.clearPathsButton.UseVisualStyleBackColor = true;
            this.clearPathsButton.Click += new System.EventHandler(this.clearPathsButton_Click);
            // 
            // overWriteCheckBox
            // 
            this.overWriteCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.overWriteCheckBox.AutoSize = true;
            this.overWriteCheckBox.Location = new System.Drawing.Point(192, 446);
            this.overWriteCheckBox.Name = "overWriteCheckBox";
            this.overWriteCheckBox.Size = new System.Drawing.Size(77, 17);
            this.overWriteCheckBox.TabIndex = 7;
            this.overWriteCheckBox.Text = "Over Write";
            this.overWriteCheckBox.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 477);
            this.Controls.Add(this.overWriteCheckBox);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.clearPathsButton);
            this.Controls.Add(this.addPathButton);
            this.Controls.Add(this.generateButton);
            this.Name = "MainForm";
            this.Text = "TOC Generator for ASDoc";
            this.tabControl.ResumeLayout(false);
            this.docPathTabPage.ResumeLayout(false);
            this.docPathTabPage.PerformLayout();
            this.outputTabPage.ResumeLayout(false);
            this.outputTabPage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.Button addPathButton;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage docPathTabPage;
        private System.Windows.Forms.TabPage outputTabPage;
        private System.Windows.Forms.TextBox outputTextBox;
        private System.Windows.Forms.ListView docPathListView;
        private System.Windows.Forms.TextBox inputTextBox;
        private System.Windows.Forms.Label inputLabel;
        private System.Windows.Forms.ColumnHeader titleColumnHeader;
        private System.Windows.Forms.ColumnHeader pathColumnHeader;
        private System.Windows.Forms.Button clearPathsButton;
        private System.Windows.Forms.Label tipsLabel;
        private System.Windows.Forms.CheckBox overWriteCheckBox;
    }
}

