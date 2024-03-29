// Modified from FlashDevelop.Controls.Browser

using System;
using System.Text;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using PluginCore.Localization;
using PluginCore;
using OpenTheDoc.Resources;

namespace OpenTheDoc
{
    public class Browser : UserControl
    {
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton goButton;
        private System.Windows.Forms.ToolStripButton backButton;
        private System.Windows.Forms.ToolStripButton forwardButton;
        private System.Windows.Forms.ToolStripButton refreshButton;
        private System.Windows.Forms.ToolStripComboBox addressComboBox;
        private WebBrowser webBrowser;

        public Browser()
        {
            this.Font = PluginBase.Settings.DefaultFont;
            this.InitializeComponent();
            this.InitializeLocalization();
            this.InitializeInterface();
        }

        #region Windows Forms Designer Generated Code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Browser));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.backButton = new System.Windows.Forms.ToolStripButton();
            this.forwardButton = new System.Windows.Forms.ToolStripButton();
            this.refreshButton = new System.Windows.Forms.ToolStripButton();
            this.addressComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.goButton = new System.Windows.Forms.ToolStripButton();
            this.webBrowser = new WebBrowser();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.CanOverflow = false;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backButton,
            this.forwardButton,
            this.refreshButton,
            this.addressComboBox,
            this.goButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Padding = new System.Windows.Forms.Padding(2, 1, 2, 2);
            this.toolStrip.Size = new System.Drawing.Size(620, 25);
            this.toolStrip.TabIndex = 3;
            // 
            // backButton
            // 
            this.backButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.backButton.Enabled = false;
            this.backButton.Image = ((System.Drawing.Image)(resources.GetObject("backButton.Image")));
            this.backButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(23, 22);
            this.backButton.Text = "Back";
            this.backButton.Click += new System.EventHandler(this.BackButtonClick);
            // 
            // forwardButton
            // 
            this.forwardButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.forwardButton.Enabled = false;
            this.forwardButton.Image = ((System.Drawing.Image)(resources.GetObject("forwardButton.Image")));
            this.forwardButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.forwardButton.Name = "forwardButton";
            this.forwardButton.Size = new System.Drawing.Size(23, 22);
            this.forwardButton.Text = "Forward";
            this.forwardButton.Click += new System.EventHandler(this.ForwardButtonClick);
            // 
            // refreshButton
            // 
            this.refreshButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.refreshButton.Image = ((System.Drawing.Image)(resources.GetObject("refreshButton.Image")));
            this.refreshButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(23, 22);
            this.refreshButton.Margin = new Padding(0, 0, 1, 0);
            this.refreshButton.Text = "Refresh";
            this.refreshButton.Click += new System.EventHandler(this.RefreshButtonClick);
            // 
            // addressComboBox
            // 
            this.addressComboBox.AutoSize = false;
            this.addressComboBox.Name = "addressComboBox";
            this.addressComboBox.Size = new System.Drawing.Size(450, 21);
            this.addressComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AddressComboBoxKeyPress);
            this.addressComboBox.SelectedIndexChanged += new System.EventHandler(this.AddressComboBoxSelectedIndexChanged);
            // 
            // goButton
            // 
            this.goButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.goButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.goButton.Image = ((System.Drawing.Image)(resources.GetObject("goButton.Image")));
            this.goButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(23, 22);
            this.goButton.Text = "Go";
            this.goButton.Click += new System.EventHandler(this.BrowseButtonClick);
            // 
            // webBrowser
            //
            this.webBrowser.AllowWebBrowserDrop = true;
            this.webBrowser.WebBrowserShortcutsEnabled = true;
            this.webBrowser.ScriptErrorsSuppressed = true;
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(0, 25);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(620, 375);
            this.webBrowser.TabIndex = 2;
            this.webBrowser.CanGoForwardChanged += new System.EventHandler(this.WebBrowserPropertyUpdated);
            this.webBrowser.CanGoBackChanged += new System.EventHandler(this.WebBrowserPropertyUpdated);
            this.webBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.WebBrowserNavigated);
            this.webBrowser.DocumentTitleChanged += new System.EventHandler(this.WebBrowserDocumentTitleChanged);
            this.webBrowser.NewWindow += new System.ComponentModel.CancelEventHandler(this.WebBrowserNewWindow);
            // 
            // Browser
            // 
            this.Controls.Add(this.webBrowser);
            this.Controls.Add(this.toolStrip);
            this.Name = "Browser";
            this.Size = new System.Drawing.Size(620, 400);
            this.Resize += new System.EventHandler(this.BrowserResize);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Methods And Event Handlers
        
        /// <summary>
        /// Accessor for the webBrowser
        /// </summary>
        public WebBrowser WebBrowser
        {
            get { return this.webBrowser; }
        }

        /// <summary>
        /// Accessor for the addressComboBox
        /// </summary>
        public ToolStripComboBox AddressBox
        {
            get { return this.addressComboBox; }
        }

        /// <summary>
        /// Initializes localized texts to the controls
        /// </summary>
        private void InitializeLocalization()
        {
            this.goButton.Text = LocaleHelper.GetString("Label.Go");
            this.backButton.Text = LocaleHelper.GetString("Label.Back");
            this.forwardButton.Text = LocaleHelper.GetString("Label.Forward");
            this.refreshButton.Text = LocaleHelper.GetString("Label.Refresh");
        }

        /// <summary>
        /// Initializes the ui based on settings
        /// </summary>
        private void InitializeInterface()
        {
            this.toolStrip.Renderer = new DockPanelStripRenderer(true);
            this.addressComboBox.FlatStyle = PluginBase.Settings.ComboBoxFlatStyle;
        }

        /// <summary>
        /// Resizes the address combo box when form is resized
        /// </summary>
        private void BrowserResize(Object sender, EventArgs e)
        {
            Size size = new Size();
            size.Height = this.addressComboBox.Height;
            size.Width = this.toolStrip.Size.Width - 100;
            this.addressComboBox.Size = size;
        }

        // ++++ ++++ Modified ++++ ++++ //
        /// <summary>
        /// If the page tries to open a new window use itseft
        /// </summary>
        private void WebBrowserNewWindow(Object sender, CancelEventArgs e)
        {
            // TODO: Open in a new tab
            this.webBrowser.Navigate(this.webBrowser.StatusText);
            e.Cancel = true;
        }

        /// <summary>
        /// Handles the web browser property changed event
        /// </summary>
        private void WebBrowserPropertyUpdated(Object sender, EventArgs e)
        {
            this.backButton.Enabled = this.webBrowser.CanGoBack;
            this.forwardButton.Enabled = this.webBrowser.CanGoForward;
        }

        // ++++ ++++ Modified ++++ ++++ //
        /// <summary>
        /// Handles the web browser navigated event
        /// </summary>
        private void WebBrowserNavigated(Object sender, WebBrowserNavigatedEventArgs e)
        {
            this.addressComboBox.Text = e.Url.ToString();
        }

        /// <summary>
        /// Handles the web browser title changed event
        /// </summary>
        private void WebBrowserDocumentTitleChanged(Object sender, EventArgs e)
        {
            if (this.webBrowser.DocumentTitle.Trim() == "")
                this.Parent.Text = this.webBrowser.Document.Domain;
            else
                this.Parent.Text = this.webBrowser.DocumentTitle.Split(new string[]{" - "}, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        /// <summary>
        /// Handles the combo box index changed event
        /// </summary>
        private void AddressComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            String url = this.addressComboBox.SelectedItem.ToString();
            this.webBrowser.Navigate(url);
        }

        /// <summary>
        /// Browses to the previous page in history
        /// </summary>
        private void BackButtonClick(Object sender, EventArgs e)
        {
            this.webBrowser.GoBack();
        }

        /// <summary>
        /// Browses to the next page in history
        /// </summary>
        private void ForwardButtonClick(Object sender, EventArgs e)
        {
            this.webBrowser.GoForward();
        }

        /// <summary>
        /// Reloads the current pages contents
        /// </summary>
        private void RefreshButtonClick(Object sender, EventArgs e)
        {
            this.webBrowser.Refresh();
        }

        /// <summary>
        /// Browses to the specified url on click
        /// </summary>
        private void BrowseButtonClick(Object sender, EventArgs e)
        {
            String url = this.addressComboBox.Text;
            if (!this.addressComboBox.Items.Contains(url))
                this.addressComboBox.Items.Insert(0, url);
 
            this.webBrowser.Navigate(url);
        }

        /// <summary>
        /// Handles the combo box key press event
        /// </summary>
        private void AddressComboBoxKeyPress(Object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                String url = this.addressComboBox.Text;
                if (!this.addressComboBox.Items.Contains(url))
                    this.addressComboBox.Items.Insert(0, url);

                this.webBrowser.Navigate(url);
            }
        }

        #endregion

    }

    // ++++ ++++ Modified: Deleted WebBrowserEx ++++ ++++ //
}
