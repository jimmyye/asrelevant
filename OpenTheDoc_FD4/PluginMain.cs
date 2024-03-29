using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using OpenTheDoc.Resources;
using PluginCore;
using PluginCore.Helpers;
using PluginCore.Localization;
using PluginCore.Managers;
using PluginCore.Utilities;
using WeifenLuo.WinFormsUI.Docking;


namespace OpenTheDoc
{
    public class PluginMain : IPlugin
    {
        private String pluginName = "OpenTheDoc";
        private String pluginGuid = "9E4B09DE-2F48-441e-AC80-89AD74BBB250";
        private String pluginHelp = "http://www.flashdevelop.org/community/viewtopic.php?f=4&t=2318";
        private String pluginDesc = "Open Documentations for FlashDevelop 3.";
        private String pluginAuth = "Jimmy Ye";
        private String settingFilename;
        private Settings settingObject;
        private DockContent pluginPanel;
        private PluginUI pluginUI;
        private Image pluginImage;

        //private string tabID;   // For SingleInstanceMode

        // commands
        private const string OPEN_THE_DOC = "OpenTheDoc";
        private const string OPEN_THE_DOC_NEW_TAB = "OpenTheDoc.NewTab";
        private const string OPEN_HELP_PANEL = "OpenTheDoc.OpenHelpPanel";
        public static string OPEN_THE_URL_NEW_TAB = "OpenTheDoc.UrlNewTab";

        private Dictionary<string, Book> bookCache;             // <tocPath, book>

        private bool IsNotFirst
        {
            get { return this.settingObject.SingleInstanceMode && !FlashDevelop.MainForm.IsFirst; }
        }

        #region Required Properties

        /// <summary>
        /// Name of the plugin
        /// </summary> 
        public String Name
        {
            get { return this.pluginName; }
        }

        /// <summary>
        /// GUID of the plugin
        /// </summary>
        public String Guid
        {
            get { return this.pluginGuid; }
        }

        /// <summary>
        /// Author of the plugin
        /// </summary> 
        public String Author
        {
            get { return this.pluginAuth; }
        }

        /// <summary>
        /// Description of the plugin
        /// </summary> 
        public String Description
        {
            get { return this.pluginDesc; }
        }

        /// <summary>
        /// Web address for help
        /// </summary> 
        public String Help
        {
            get { return this.pluginHelp; }
        }

        /// <summary>
        /// Object that contains the settings
        /// </summary>
        [Browsable(false)]
        public Object Settings
        {
            get { return this.settingObject; }
        }

        public int Api
        {
            get { return 1; }
        }

        #endregion

        #region Required Methods

        /// <summary>
        /// Initializes the plugin
        /// </summary>
        public void Initialize()
        {
            this.InitBasics();
            this.LoadSettings();
            this.AddEventHandlers();
            this.InitLocalization();
            this.CreatePluginPanel();
            this.CreateMenuItem();
        }

        /// <summary>
        /// Disposes the plugin
        /// </summary>
        public void Dispose()
        {
            // Close HelpPanel if not visible
            if (this.pluginPanel.IsFloat && this.WindowVisible == false)
                this.pluginPanel.Hide();

            this.SaveSettings();
        }

        /// <summary>
        /// Handles the incoming events
        /// </summary>
        public void HandleEvent(Object sender, NotifyEvent e, HandlingPriority prority)
        {
            switch (e.Type)
            {
                // Handle custom command OPEN_THE_DOC and OPEN_THE_DOC_NEW_TAB, API Search
                case EventType.Command:
                    DataEvent de = e as DataEvent;
                    string command = de.Action;
                    if (command == OPEN_THE_DOC || command == OPEN_THE_DOC_NEW_TAB)
                    {
                        // Copy Hashtable to Dictionary<string,string> to avoid casting
                        Hashtable itemDetailsHashtable = (e as DataEvent).Data as Hashtable;
                        Dictionary<string, string> itemDetails = new Dictionary<string, string>();

                        foreach (DictionaryEntry item in itemDetailsHashtable)
                            itemDetails.Add(item.Key as string, item.Value as string);
                        DebugPrint("Item Details:", itemDetails);

                        string lang = PluginBase.CurrentProject.Language;
                        DebugPrint("Project Language:", lang);

                        // API Search
                        List<SearchResult> resultList = APISearch(itemDetails, lang);
                        this.pluginUI.UpdateSearchResultList(resultList, this.settingObject.ShowAPISearchResult);

                        if (resultList.Count > 0)
                        {
                            // SingleInstanceMode
                            if (this.IsNotFirst)
                            {
                                DebugPrint("SingleInstanceMode");
                                //if (this.settingObject.OneTabPerFDInstance)
                                //{
                                //    if (string.IsNullOrEmpty(this.tabID))
                                //        this.tabID = DateTime.Now.Ticks.ToString();
                                //    command = this.tabID;
                                //}
                                // Let the first instance open the doc
                                SingleInstanceApp.NotifyExistingInstance(new string[] { resultList[0].filePath, command });
                            }
                            else
                                OpenHelpPanel(resultList[0].filePath, command == OPEN_THE_DOC_NEW_TAB);
                        }
                        
                        e.Handled = true;
                    }
                    else if (command == OPEN_THE_URL_NEW_TAB)
                    {
                        OpenHelpPanel(de.Data as string, true);
                    }
                    break;

                case EventType.Keys:
                    Keys key = (e as KeyEvent).Value;
                    if (key == this.settingObject.ShortcutCurrentTab || key == this.settingObject.ShortcutNewTab)
                    {
                        // To show tip, not to show documentation
                        if (key == Keys.F1)
                        {
                            if (PluginCore.Controls.UITools.CallTip.CallTipActive || PluginCore.Controls.CompletionList.Active)
                                break;
                        }
                        OpenTheDoc(key == this.settingObject.ShortcutCurrentTab ? OPEN_THE_DOC : OPEN_THE_DOC_NEW_TAB);

                        // SingleInstanceMode
                        if (this.IsNotFirst)
                            ;
                        else
                        {
                            if (this.settingObject.AlwaysOpenHelpPanel)
                                OpenHelpPanel();
                        }

                        e.Handled = true;
                    }
                    
                    break;
            }
        }

        #endregion

        #region Common Methods

        /// <summary>
        /// Initializes important variables
        /// </summary>
        public void InitBasics()
        {
            String dataPath = Path.Combine(PathHelper.DataDir, "OpenTheDoc");
            if (!Directory.Exists(dataPath)) Directory.CreateDirectory(dataPath);
            this.settingFilename = Path.Combine(dataPath, "Settings.fdb");
            this.pluginImage = PluginBase.MainForm.FindImage("222");
        }

        /// <summary>
        /// Adds the required event handlers
        /// </summary> 
        public void AddEventHandlers()
        {
            EventManager.AddEventHandler(this, EventType.Command | EventType.Keys, HandlingPriority.High);
            PluginBase.MainForm.IgnoredKeys.Add(this.settingObject.ShortcutCurrentTab);
            PluginBase.MainForm.IgnoredKeys.Add(this.settingObject.ShortcutNewTab);
        }

        /// <summary>
        /// Initializes the localization of the plugin
        /// </summary>
        public void InitLocalization()
        {
            LocaleVersion locale = PluginBase.MainForm.Settings.LocaleVersion;
            switch (locale)
            {

                /*case LocaleVersion.fi_FI : 
                    // We have Finnish available... or not. :)
                    LocaleHelper.Initialize(LocaleVersion.fi_FI);
                    break;*/

                default:
                    // Plugins should default to English...
                    LocaleHelper.Initialize(LocaleVersion.en_US);
                    break;

            }
            this.pluginDesc = LocaleHelper.GetString("Info.Description");
        }


        /// <summary>
        /// Creates a menu item for the plugin and adds a ignored key
        /// </summary>
        public void CreateMenuItem()
        {
            ToolStripMenuItem menu = (ToolStripMenuItem)PluginBase.MainForm.FindMenuItem("ViewMenu");
            menu.DropDownItems.Add(new ToolStripMenuItem(LocaleHelper.GetString("Label.ViewMenuItem.PluginPanel"), this.pluginImage, new EventHandler(this.OpenHelpPanel), this.settingObject.ShortcutHelpPanel));
            PluginBase.MainForm.IgnoredKeys.Add(this.settingObject.ShortcutHelpPanel);
        }

        /// <summary>
        /// Creates a plugin panel for the plugin
        /// </summary>
        public void CreatePluginPanel()
        {
            this.pluginUI = new PluginUI(this);
            this.pluginUI.Text = LocaleHelper.GetString("Title.PluginPanel");

            this.pluginPanel = PluginBase.MainForm.CreateDockablePanel(this.pluginUI, this.pluginGuid, this.pluginImage, DockState.DockRight);
            this.pluginPanel.KeyPreview = true;
            this.pluginPanel.KeyDown += new KeyEventHandler(pluginPanel_KeyDown);

            // TODO: better solution
            // prevent HelpPanel open when run FD
            if (this.IsNotFirst)
                this.pluginPanel.Layout += new LayoutEventHandler(pluginPanel_Layout);

            // SingleInstanceMode, the fisrt instance
            if (FlashDevelop.MainForm.IsFirst)
            {
                DebugPrint("IsFirst");
                SingleInstanceApp.Message += delegate(Object sender, Object message)
                {
                    string[] args = message as string[];
                    if (args[1] == OPEN_HELP_PANEL)
                        OpenHelpPanel();
                    else
                        OpenHelpPanel(args[0], args[1] == OPEN_THE_DOC_NEW_TAB);
                };
                SingleInstanceApp.Initialize();
            }
        }

        // prevent HelpPanel open when run FD
        private void pluginPanel_Layout(object sender, LayoutEventArgs e)
        {
            if (this.IsNotFirst)
                this.WindowVisible = false;
        }

        // Hide HelpPanel when one of the shortcuts is pressed
        private void pluginPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == this.settingObject.ShortcutHelpPanel ||
                e.KeyData == this.settingObject.ShortcutCurrentTab ||
                e.KeyData == this.settingObject.ShortcutNewTab)
                this.WindowVisible = false;
        }

        /// <summary>
        /// Loads the plugin settings
        /// </summary>
        public void LoadSettings()
        {
            this.settingObject = new Settings();
            if (!File.Exists(this.settingFilename)) this.SaveSettings();
            else
            {
                Object obj = ObjectSerializer.Deserialize(this.settingFilename, this.settingObject);
                this.settingObject = (Settings)obj;
            }
        }

        /// <summary>
        /// Saves the plugin settings
        /// </summary>
        public void SaveSettings()
        {
            if (this.pluginUI != null)
                this.pluginUI.SaveState();
            ObjectSerializer.Serialize(this.settingFilename, this.settingObject);
        }

        /// <summary>
        /// Opens the plugin panel if closed, else if IsFloat toggle visibility
        /// </summary>
        public void OpenHelpPanel(Object sender, System.EventArgs e)
        {
            // SingleInstanceMode
            if (this.IsNotFirst)
            {
                // Let the first instance open the panel
                SingleInstanceApp.NotifyExistingInstance(new string[] { "", OPEN_HELP_PANEL });
                return;
            }

            if (this.pluginPanel.IsHidden)
            {
                this.pluginUI.Reset();
                OpenHelpPanel(this.settingObject.HomePage, false);
            }
            else
            {
                if (this.pluginPanel.IsFloat)
                    this.WindowVisible = !this.WindowVisible;
                else
                    OpenHelpPanel();
            }
        }

        private void OpenHelpPanel(string url, bool newTab)
        {
            OpenHelpPanel();
            this.pluginUI.OpenUrl(url, newTab);
        }

        //private void OpenHelpPanel(string url, string data)
        //{
        //    OpenHelpPanel();

        //    if (data == OPEN_THE_DOC)
        //        this.pluginUI.OpenUrl(url, false);
        //    else if (data == OPEN_THE_DOC_NEW_TAB)
        //        this.pluginUI.OpenUrl(url, true);
        //    else
        //        this.pluginUI.OpenUrl(url, data);
        //}

        private void OpenHelpPanel()
        {
            this.WindowVisible = true;

            this.pluginPanel.Show();
            this.pluginPanel.BringToFront();
            this.pluginPanel.Focus();
        }

        // Resolve current element and call command "OpenTheDoc" or "OpenTheDocNewTab"
        private void OpenTheDoc(string command)
        {
            ITabbedDocument doc = PluginBase.MainForm.CurrentDocument;
            // editor ready?
            if (doc == null) return;

            ScintillaNet.ScintillaControl sci = doc.IsEditable ? doc.SciControl : null;
            if (sci != null)
                ASCompletion.Completion.ASComplete.ResolveElement(sci, command);
        }

        // Visibility of the window that holds HelpPanel
        internal bool WindowVisible
        {
            get
            {
                if (this.pluginPanel != null && this.pluginPanel.IsFloat)
                    return this.pluginPanel.FloatPane.FloatWindow.Visible;
                else
                    return false;
            }
            set
            {
                if (this.pluginPanel != null && this.pluginPanel.IsFloat)
                    this.pluginPanel.FloatPane.FloatWindow.Visible = value;
            }
        }
        
        #endregion

        #region Core Methods

        // Inspired by eylon
        private List<String> GetDocPaths() 
        {
            this.DebugPrint("Begin GetDocPaths()...", "");
            List<string> docPaths = new List<string>();
            foreach (string docPath in settingObject.DocPaths)
            {
                if (docPath.ToLower().StartsWith("http:"))
                    docPaths.Add(docPath);
                // $(ProjectPath)\docs
                else if (docPath.StartsWith("$(ProjectPath)") && PluginBase.CurrentProject != null)
                {
                    string path = PluginBase.CurrentProject.GetAbsolutePath(docPath.Replace("$(ProjectPath)\\", ""));
                    if (Directory.Exists(path))
                    {
                        docPaths.Add(path);
                        DebugPrint("Project docPath: ", path);
                    }
                }
                // $(GlobalClasspaths)\..\docs
                else if (docPath.StartsWith("$(GlobalClasspaths)") && ASCompletion.Context.ASContext.Context.Settings != null)
                {
                    string relativePath = docPath.Replace("$(GlobalClasspaths)\\", "");
                    foreach (string globalClasspath in ASCompletion.Context.ASContext.Context.Settings.UserClasspath)
                    {
                        string path = Path.Combine(globalClasspath, relativePath);
                        if (Directory.Exists(path))
                        {
                            docPaths.Add(path);
                            DebugPrint("GlobalClass docPath: ", path);
                        }
                    }
                }
                // X:\path\to\alldocs\*
                else if (docPath.EndsWith("*")) // All paths in basePath
                {
                    string basePath = docPath.Remove(docPath.Length - 1);
                    if (Directory.Exists(basePath))
                    {
                        DebugPrint("Paths in " + basePath);
                        foreach (string path in Directory.GetDirectories(basePath))
                        {
                            docPaths.Add(path);
                            DebugPrint(path);
                        }
                    }
                }
                // Put this one in the end because "$(GlobalClasspaths)\..\docs" equals "docs"
                // which will be took as relative path and it exists: FlashDevelop\docs
                else if (Directory.Exists(docPath))
                    docPaths.Add(docPath);
                
            }
            this.DebugPrint("End GetDocPaths().", "");
            return docPaths;
        }

        // Updates bookCache after changing settings
        internal void UpdateBookCache()
        {
            this.DebugPrint("Begin UpdateBookCache()...", "");
            Dictionary<string, Book> newBookCache = new Dictionary<string, Book>();
            foreach (string docPath in GetDocPaths())
            {
                if (!Directory.Exists(docPath) || docPath.ToLower().StartsWith("http"))
                    continue;

                foreach (string toc in this.settingObject.TOC)
                {
                    string tocPath = Path.Combine(docPath, toc);
                    if (!File.Exists(tocPath)) continue;

                    Book book;
                    if (this.bookCache != null && this.bookCache.ContainsKey(tocPath))
                    {
                        book = this.bookCache[tocPath];
                        this.DebugPrint("Load from cache. tocPath: ", tocPath);
                    }
                    else
                    {
                        book = BookHelper.MakeBook(docPath, toc);
                        this.DebugPrint("Load from disk. tocPath: ", tocPath);
                    }
                    newBookCache.Add(tocPath, book);
                }
            }
            this.bookCache = newBookCache;
            this.DebugPrint("End UpdateBookCache().", "");
        }

        // Get books from bookCache, filter by category,
        // including those have no categories if in !strict mode
        // and category==string.Empty means all categories
        internal List<Book> GetBooks(string category, bool strict)
        {
            List<Book> books = new List<Book>();
            if (this.bookCache == null) this.UpdateBookCache();

            this.DebugPrint("GetBooks by category: ", category);
            foreach (Book book in this.bookCache.Values)
            {
                if (category == string.Empty ||
                    book.Categories.Contains(category) ||
                    (!strict && book.Categories.Count == 0))
                {
                    books.Add(book);
                    this.DebugPrint("Book added. Title: ", book.Title);
                }
            }
            return books;
        }

        internal List<string> GetBooksWithoutToc()
        {
            List<string> books = new List<string>();
            foreach (string docPath in GetDocPaths())
            {
                if (docPath.ToLower().StartsWith("http"))
                {
                    books.Add(docPath);
                    this.DebugPrint("Online. docPath: ", docPath);
                }
                else if (!Directory.Exists(docPath))
                    continue;

                bool withoutToc = true;
                foreach (string toc in this.settingObject.TOC)
                {
                    string tocPath = Path.Combine(docPath, toc);
                    if (File.Exists(tocPath))
                    {
                        withoutToc = false;
                        break;
                    }
                }

                if (withoutToc)
                {
                    books.Add(docPath);
                    this.DebugPrint("Without TOC. docPath: ", docPath);
                }
            }
            return books;
        }

        internal List<SearchResult> APISearch(Dictionary<string, string> itemDetails, string category)
        {
            List<SearchResult> resultList = new List<SearchResult>();

            bool isClass = itemDetails["ItmKind"] == "class";
            bool isFunction = itemDetails["ItmKind"] == "function";
            bool isTopLevelClass = itemDetails["ItmTypName"] == itemDetails["ItmTypPkgName"];

            #region Books with TOC
            foreach (Book book in this.GetBooks(category, false))
            {
                // Something like "Sprite" or "Sprite *", "*" can be anything: "Sprite[ *]"
                string formatString = "//*[@label='{0}' or starts-with(@label, '{0} ')]";
                string xpathBase = string.Format(formatString, itemDetails["ItmName"]);
                string xpath;
                XmlElement result;

                string filePath;
                string label;

                XmlNode toc = book.Toc;
                string path = book.Path;

                if (!isClass)
                {
                    string xpathItemTypName = string.Format(formatString, itemDetails["ItmTypName"]);
                    // structures like: "Sprite[ *]//startDrag[ *]"
                    xpath = xpathItemTypName + xpathBase;
                    result = toc.SelectSingleNode(xpath) as XmlElement;

                    if (result == null && isFunction)
                    {
                        // structures like: "Sprite[ *]//startDrag()"  // AS3 Reference
                        xpath = string.Format("{0}//*[@label='{1}()']", xpathItemTypName, itemDetails["ItmName"]);
                        result = toc.SelectSingleNode(xpath) as XmlElement;
                    }

                    if (result == null)
                    {
                        // structures like: "Sprite[ *]//[*]Sprite.startDrag[*]"  // AS2 Reference of some language (Chinese)
                        xpath = string.Format("{0}//*[contains(@label, '{1}.{2} ')]", xpathItemTypName, itemDetails["ItmTypName"], itemDetails["ItmName"]);
                        result = toc.SelectSingleNode(xpath) as XmlElement;
                    }

                    if (result != null)
                    {
                        string href = result.GetAttribute("href");
                        filePath = Path.Combine(path, href);
                        label = string.Format("{0} ({1}.{0})", itemDetails["ItmName"], itemDetails["ItmTypPkgName"]);

                        resultList.Add(new SearchResult(label, itemDetails["ItmKind"], "", book.Title, filePath));

                        // Traces back to look for the ItmTpyName, that is its class
                        for (XmlElement n = result.ParentNode as XmlElement; n != null; n = n.ParentNode as XmlElement)
                        {
                            label = n.GetAttribute("label");
                            if (!label.Contains(itemDetails["ItmTypName"])) continue;

                            href = n.GetAttribute("href");
                            filePath = Path.Combine(path, href);
                            formatString = "{0}" + (isTopLevelClass ? "" : " ({1})");
                            label = string.Format(formatString, itemDetails["ItmTypName"], itemDetails["ItmTypPkgName"]);

                            resultList.Add(new SearchResult(label, "class", "", book.Title, filePath));
                            break;
                        }
                    }
                }
                // This item is a Class but not a Top Level Class
                else if (isClass && !isTopLevelClass)
                {
                    // structures like: "flash.display[ *]//Sprite[ *]"
                    xpath = string.Format(formatString, itemDetails["ItmTypPkg"]) + xpathBase;
                    result = toc.SelectSingleNode(xpath) as XmlElement;

                    if (result == null)
                    {
                        // labels like: "Sprite (flash.display.Sprite)"  // AS2 Reference
                        xpath = string.Format("//*[@label='{0} ({1})']", itemDetails["ItmName"], itemDetails["ItmTypPkgName"]);
                        result = toc.SelectSingleNode(xpath) as XmlElement;
                    }

                    if (result != null)
                    {
                        string href = result.GetAttribute("href");
                        filePath = Path.Combine(path, href);
                        label = string.Format("{0} ({1})", itemDetails["ItmTypName"], itemDetails["ItmTypPkgName"]);

                        resultList.Add(new SearchResult(label, itemDetails["ItmKind"], "", book.Title, filePath));

                        // Looks for constructors
                        xpath = string.Format("//*[starts-with(@label, '{0}')]", itemDetails["ItmName"]);
                        XmlNodeList results = result.Clone().SelectNodes(xpath);    // Xpath search from result.Clone(), it's root
                        if (results.Count != 0)
                        {
                            foreach (XmlElement node in results)
                            {
                                href = node.GetAttribute("href");
                                filePath = Path.Combine(path, href);
                                label = string.Format("{0} constructor", itemDetails["ItmTypName"]);

                                resultList.Add(new SearchResult(label, "", "", book.Title, filePath));
                            }
                        }
                    }
                }
                // This item is a Top Level Class
                else if (isClass && isTopLevelClass)
                {
                    // labels like: "Sprite[ *]" or "Sprite()" (AS3 Reference)
                    formatString = "//*[@label='{0}' or starts-with(@label, '{0} ') or @label='{0}()']";
                    xpath = string.Format(formatString, itemDetails["ItmName"]);
                    XmlNodeList results = toc.SelectNodes(xpath);

                    List<string> tempList = new List<string>();
                    if (results.Count != 0)
                    {
                        foreach (XmlElement node in results)
                        {
                            string href = node.GetAttribute("href");
                            filePath = Path.Combine(path, href);
                            if (!tempList.Contains(filePath))
                            {
                                string parentLabel = (node.ParentNode as XmlElement).GetAttribute("label");
                                resultList.Add(new SearchResult(itemDetails["ItmTypName"], "", parentLabel, book.Title, filePath));
                                tempList.Add(filePath);
                            }
                        }
                    }
                }
            }
            #endregion

            #region Well-Organized Books without TOC and On-line Documents
            foreach (string path in this.GetBooksWithoutToc())
            {
                string filePath;
                string label;
                if (path.ToLower().StartsWith("http"))
                {
                    filePath = new Uri(new Uri(path), itemDetails["ItmTypPkgNameURL"] + ".html").ToString();
                    if (!isClass)
                        filePath = filePath + "#" + itemDetails["ItmName"] + (isFunction ? "()" : "");

                    resultList.Add(new SearchResult(filePath, "", "", "On-line Help (Unknown Availability)", filePath));
                }
                else
                {
                    filePath = Path.Combine(path, itemDetails["ItmTypPkgNamePath"] + ".html");
                    if (!File.Exists(filePath)) continue;

                    string classFilePath = filePath;
                    if (!isClass)
                    {
                        filePath = filePath + "#" + itemDetails["ItmName"] + (isFunction ? "()" : "");
                        label = string.Format("{0} ({1}.{0})", itemDetails["ItmName"], itemDetails["ItmTypPkgName"]);

                        resultList.Add(new SearchResult(label, "", "", path, filePath));
                    }

                    string formatString = "{0}" + (isTopLevelClass ? "" : " ({1})");
                    label = string.Format(formatString, itemDetails["ItmTypName"], itemDetails["ItmTypPkgName"]);

                    resultList.Add(new SearchResult(label, "", "", path, classFilePath));
                }
            }
            #endregion

            return resultList;
        }

        internal enum SearchOption
        {
            Contains,
            StartsWith,
            Equals
        }

        internal List<SearchResult> TitleSearch(string text, SearchOption opt, bool matchCase, string category)
        {
            List<SearchResult> resultList = new List<SearchResult>();

            // Build XPath according to options
            string xpath;
            string format;
            string firstOperand;  // The first operand

            if (opt == SearchOption.Contains) format = "//*[contains({0},'{1}')]";
            else if (opt == SearchOption.StartsWith) format = "//*[starts-with({0},'{1}')]";
            else format = "//*[{0}='{1}']";

            if (matchCase)
            {
                firstOperand = "@label";
                xpath = String.Format(format, firstOperand, text);
            }
            else
            {
                firstOperand = "translate(@label,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')";
                xpath = String.Format(format, firstOperand, text.ToUpper());
            }

            // Search
            foreach (Book book in this.GetBooks(category, true))
            {
                XmlNodeList results = book.Toc.SelectNodes(xpath);
                if (results.Count == 0) continue;

                List<string> tempList = new List<string>();
                foreach (XmlElement node in results)
                {
                    string href = node.GetAttribute("href");
                    string filePath = Path.Combine(book.Path, href);
                    if (tempList.Contains(filePath)) continue;

                    string label = node.GetAttribute("label");

                    // Get parent node label and determine the type
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

                    tempList.Add(filePath);
                    resultList.Add(new SearchResult(label, type, parentLabel, book.Title, filePath));
                }
            }

            return resultList;
        }

        #endregion

        #region DEBUG

        [Conditional("DEBUG")]
        private void DebugPrint(string info, Dictionary<string, string> dict)
        {
            TraceManager.Add("\n" + info);
            foreach (var de in dict)
                TraceManager.Add(de.Key + ": " + de.Value);
        }

        [Conditional("DEBUG")]
        internal void DebugPrint(string info, string str)
        {
            TraceManager.Add("\n" + info);
            TraceManager.Add(str);
        }

        [Conditional("DEBUG")]
        static public void DebugPrint(string str)
        {
            TraceManager.Add(str);
        }

        #endregion

    }

    #region Custom Classes and Structs

    struct SearchResult
    {
        public string title;
        public string type;
        public string parentTitle;
        public string bookTitle;
        public string filePath;

        public SearchResult( string title, string type, string parentTitle, string bookTitle, string filePath)
        {
            this.title = title;
            this.type = type;
            this.parentTitle = parentTitle;
            this.bookTitle = bookTitle;
            this.filePath = filePath;
        }
    }

    [Serializable]
    public class Category
    {
        private string title = string.Empty;
        private string keyword = string.Empty;

        public Category() { }
        public Category(string title, string keyword)
        {
            this.title = title;
            this.keyword = keyword;
        }

        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        public string Keyword
        {
            get { return this.keyword; }
            set { this.keyword = value; }
        }
    }

    #endregion
}
