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
        const bool SHOW_API_SEARCH_RESULT = false;
        const bool ALWAYS_OPEN_HELP_PANEL = false;
        const Keys SHORTCUT = Keys.Control | Keys.F1;
        const Keys SHORTCUT_HELP_PANEL = Keys.Shift | Keys.F1;

        private string[] docPaths = DOC_PATHS;
        private string[] toc = TOCS;
        private string homePage;
        private bool showAPISearchResult = SHOW_API_SEARCH_RESULT;
        private bool alwaysOpenHelpPanel = ALWAYS_OPEN_HELP_PANEL;
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

        #endregion

        #region Behavior

        [DisplayName("Show API Search Results")]
        [Category("Behavior"), Description("Always show results after API Search."), DefaultValue(SHOW_API_SEARCH_RESULT)]
        public Boolean ShowAPISearchResult
        {
            get { return showAPISearchResult; }
            set { showAPISearchResult = value; }
        }

        [DisplayName("Always Open HelpPanel")]
        [Category("Behavior"), Description("Always open HelpPanel even no API found in API Search."), DefaultValue(ALWAYS_OPEN_HELP_PANEL)]
        public Boolean AlwaysOpenHelpPanel
        {
            get { return alwaysOpenHelpPanel; }
            set { alwaysOpenHelpPanel = value; }
        }

        #endregion

        #region Shortcuts

        [DisplayName("Shortcut")]
        [Category("Shortcuts"), Description("API search: Resolve the element at cursor position and OpenTheDoc in HelpPanel. Hide HelpPanel if it's floating and already focused (F1 has the same function)."), DefaultValue(SHORTCUT)]
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
        [Category("Shortcuts"), Description("Open HelpPanel with HomePage if closed, toggle visibility if it's floating, simply show if it's docking."), DefaultValue(SHORTCUT_HELP_PANEL)]
        public Keys ShortcutHelpPanel
        {
            get { return shortcutHelpPanel == Keys.None ? SHORTCUT_HELP_PANEL : shortcutHelpPanel; }
            set { shortcutHelpPanel = value; }
        }

        #endregion

        #region State Saving

        [Browsable(false)]
        public int MainSplitContainerSplitterDistance { get; set; }

        [Browsable(false)]
        public int ViewSplitContainerSplitterDistance { get; set; }

        #endregion
    }
}
