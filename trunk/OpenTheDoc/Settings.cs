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
        static public string[] DOC_PATHS = new string[]
        {
            @"C:\Program Files\Adobe\Flex Builder 3\doc",
            @"C:\Documents and Settings\All Users\Application Data\Adobe\Flash CS3\en\Configuration\HelpPanel\Help\ActionScriptLangRefV3",
        };

        static public string[] TOCS = { "otd_toc.xml", "help_toc.xml", "tocAPI.xml", "toc.xml" };

        static public List<Category> CATEGORIES = new List<Category>() 
        {
            new Category("All Books", ""), 
            new Category("Features", "flashfeatures"), 
            new Category("ActionScript 2.0", "as2"), 
            new Category("ActionScript 3.0", "as3"), 
            new Category("ActionScript 2.0 Components", "components2"), 
            new Category("ActionScript 3.0 Components", "components3"), 
            new Category("Extending", "extending"), 
            new Category("Language References (ActionScript & Components)", "languagereferences"), 
            new Category("Flash Lite 1.x", "flashlite1"), 
            new Category("Flash Lite 2.x", "flashlite2"), 
            new Category("3rd Party", "3rdparty")
        };

        const bool HANDLE_F1 = false;
        const bool SHOW_API_SEARCH_RESULT = false;
        const bool ALWAYS_OPEN_HELP_PANEL = false;
        const Keys SHORTCUT = Keys.F1;
        const Keys SHORTCUT_NEW_TAB = Keys.Control | Keys.F1;
        const Keys SHORTCUT_HELP_PANEL = Keys.Shift | Keys.F1;

        private string[] docPaths = DOC_PATHS;
        private string[] toc = TOCS;
        private List<Category> categories = new List<Category>(CATEGORIES);
        private string homePage;
        private bool showAPISearchResult = SHOW_API_SEARCH_RESULT;
        private bool alwaysOpenHelpPanel = ALWAYS_OPEN_HELP_PANEL;
        private Keys shortcutCurrentTab = SHORTCUT;
        private Keys shortcutNewTab = SHORTCUT_NEW_TAB;
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
            get { return toc ?? TOCS; }
            set { toc = value; }
        }

        [DisplayName("HomePage")]
        [Category("Documentation"), Description("HomePage")]
        public string HomePage
        {
            get { return homePage; }
            set { homePage = value; }
        }

        [DisplayName("Categories")]
        [Category("Documentation"), Description("Categorize docs by attribute \"categories\" of root node of TOC file. Title Search only searches the selected category of books.")]
        public List<Category> Categories
        {
            get { return categories ?? (categories = new List<Category>(CATEGORIES)); }
            set { categories = value; }
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

        [DisplayName("API Search")]
        [Category("Shortcuts"), Description("Resolve the element at cursor position and OpenTheDoc in HelpPanel. Hide HelpPanel if it's floating and already focused."), DefaultValue(SHORTCUT)]
        public Keys ShortcutCurrentTab
        {
            get { return shortcutCurrentTab == Keys.None ? SHORTCUT : shortcutCurrentTab; }
            set { shortcutCurrentTab = value; }
        }

        [DisplayName("APISearchNewTab")]
        [Category("Shortcuts"), Description("API search and OpenTheDoc in a new tab. Hide HelpPanel if it's floating and already focused."), DefaultValue(SHORTCUT_NEW_TAB)]
        public Keys ShortcutNewTab
        {
            get { return shortcutNewTab == Keys.None ? SHORTCUT_NEW_TAB : shortcutNewTab; }
            set { shortcutNewTab = value; }
        }

        [DisplayName("HelpPanel")]
        [Category("Shortcuts"), Description("Open HelpPanel with HomePage if closed, or toggle visibility if it's floating, or simply show if it's docking."), DefaultValue(SHORTCUT_HELP_PANEL)]
        public Keys ShortcutHelpPanel
        {
            get { return shortcutHelpPanel == Keys.None ? SHORTCUT_HELP_PANEL : shortcutHelpPanel; }
            set { shortcutHelpPanel = value; }
        }

        #endregion

        #region State Saving

        private int mainSplitContainerSplitterDistance;
        private int viewSplitContainerSplitterDistance;
        private Category selectedCategory;

        [Browsable(false)]
        public int MainSplitContainerSplitterDistance
        {
            get { return mainSplitContainerSplitterDistance == 0 ? 250 : mainSplitContainerSplitterDistance; }
            set { mainSplitContainerSplitterDistance = value; }
        }

        [Browsable(false)]
        public int ViewSplitContainerSplitterDistance
        {
            get { return viewSplitContainerSplitterDistance == 0 ? 100 : viewSplitContainerSplitterDistance; }
            set { viewSplitContainerSplitterDistance = value; }
        }

        [Browsable(false)]
        public Category SelectedCategory
        {
            get { return selectedCategory; }
            set { selectedCategory = value; }
        }

        #endregion
    }
}
