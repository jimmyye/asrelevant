using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace OpenTheDoc
{
    public enum DocumentViewer
    {
        InternalBrowser,
        SystemBrowser,
        HelpContents
    }

    [Serializable]
    public class Settings
    {
        static public string[] DOC_PATHS = new string[]{
            @"C:\Program Files\Adobe\Flex Builder 3\doc",
            @"C:\Documents and Settings\All Users\Application Data\Adobe\Flash CS3\en\Configuration\HelpPanel\Help\ActionScriptLangRefV3",
        };

        const bool HANDLE_F1 = false;
        const bool OPEN_FIRST_TOPIC = false;

        private string[] docPaths = DOC_PATHS;
        private string[] toc = { "otd_toc.xml", "help_toc.xml", "tocAPI.xml", "toc.xml" };
        private string homePage;
        private DocumentViewer docViewer = DocumentViewer.HelpContents;
        private bool openFirstTopic = OPEN_FIRST_TOPIC;
        private bool handleF1 = HANDLE_F1;
        private Keys shortcut = Keys.Control | Keys.F1;
        private Keys shortcutHelpContents = Keys.Shift | Keys.F1;

        #region Documentation

        [DisplayName("DocPaths")]
        [Category("Documentation"), Description("Path to Documentation.")]
        public string[] DocPaths
        {
            get { return docPaths; }
            set { docPaths = value; }
        }

        [DisplayName("TOCs")]
        [Category("Documentation"), Description("File name of TOC. There can be more than one TOC file in a folder.")]
        public string[] TOC
        {
            get { return toc; }
            set { toc = value; }
        }

        [DisplayName("HomePage")]
        [Category("Documentation"), Description("HomePage")]
        public string HomePage
        {
            get { return homePage; }
            set { homePage = value; }
        }

        #endregion

        #region Behavior

        [DisplayName("Open the first topic")]
        [Category("Behavior"), Description("Automatically open the first available Related Topic."), DefaultValue(OPEN_FIRST_TOPIC)]
        public Boolean OpenFirstTopic
        {
            get { return openFirstTopic; }
            set { openFirstTopic = value; }
        }

        [DisplayName("Open docs in")]
        [Category("Behavior"), Description("The browser used to view documents."), DefaultValue(DocumentViewer.HelpContents)]
        public DocumentViewer DocViewer
        {
            get { return docViewer; }
            set { docViewer = value; }
        }

        #endregion

        #region Shortcuts

        [DisplayName("Shortcut")]
        [Category("Shortcuts"), Description("Search the item at cursor position."), DefaultValue(Keys.Control | Keys.F1)]
        public Keys Shortcut
        {
            get { return shortcut; }
            set { shortcut = value; }
        }

        [DisplayName("Handle F1")]
        [Category("Shortcuts"), Description("If true, uses F1 for shortcut and the default action of FD will not happen."), DefaultValue(HANDLE_F1)]
        public Boolean HandleF1
        {
            get { return handleF1; }
            set { handleF1 = value; }
        }

        [DisplayName("Shortcut for HelpContents")]
        [Category("Shortcuts"), Description("Open HelpContents."), DefaultValue(Keys.Shift | Keys.F1)]
        public Keys ShortcutHelpContents
        {
            get { return shortcutHelpContents; }
            set { shortcutHelpContents = value; }
        }
        #endregion
    }

}
