using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace OpenTheDoc
{
    [Serializable]
    public class Settings
    {
        static public string[] DOC_PATHS = new string[]{
            @"C:\Program Files\Adobe\Flex Builder 3\doc",
            @"C:\Documents and Settings\All Users\Application Data\Adobe\Flash CS3\en\Configuration\HelpPanel\Help\ActionScriptLangRefV3",
        };

        static public string[] TOCS = { "otd_toc.xml", "help_toc.xml", "tocAPI.xml", "toc.xml" };

        const bool HANDLE_F1 = false;
        const bool OPEN_FIRST_TOPIC = false;
        const Keys SHORTCUT = Keys.Control | Keys.F1;
        const Keys SHORTCUT_HELP_PANEL = Keys.Shift | Keys.F1;

        private string[] docPaths = DOC_PATHS;
        private string[] toc = TOCS;
        private string homePage;
        private bool openFirstTopic = OPEN_FIRST_TOPIC;
        private bool handleF1 = HANDLE_F1;
        private Keys shortcut = SHORTCUT;
        private Keys shortcutHelpPanel = SHORTCUT_HELP_PANEL;

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
            get { return toc == null ? TOCS : toc; }
            set { toc = value; }
        }

        [DisplayName("HomePage")]
        [Category("Documentation"), Description("HomePage")]
        public string HomePage
        {
            get { return homePage; }
            set { homePage = value; }
        }

        [DisplayName("Open the first topic")]
        [Category("Documentation"), Description("Automatically open the first available Related Topic."), DefaultValue(OPEN_FIRST_TOPIC)]
        public Boolean OpenFirstTopic
        {
            get { return openFirstTopic; }
            set { openFirstTopic = value; }
        }

        #endregion

        #region Shortcuts

        [DisplayName("Shortcut")]
        [Category("Shortcuts"), Description("Resolve the element at cursor position and OpenTheDoc in HelpPanel. (API search)"), DefaultValue(SHORTCUT)]
        public Keys Shortcut
        {
            get { return shortcut == Keys.None ? SHORTCUT : shortcut; }
            set { shortcut = value; }
        }

        [DisplayName("Handle F1")]
        [Category("Shortcuts"), Description("If true, uses F1 for API search and the default handler of FD will be suppressed."), DefaultValue(HANDLE_F1)]
        public Boolean HandleF1
        {
            get { return handleF1; }
            set { handleF1 = value; }
        }

        [DisplayName("Shortcut for HelpPanel")]
        [Category("Shortcuts"), Description("Open HelpPanel with HomePage."), DefaultValue(SHORTCUT_HELP_PANEL)]
        public Keys ShortcutHelpPanel
        {
            get { return shortcutHelpPanel == Keys.None ? SHORTCUT_HELP_PANEL : shortcutHelpPanel; }
            set { shortcutHelpPanel = value; }
        }

        #endregion
    }
}
