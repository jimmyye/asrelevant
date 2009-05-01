using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        private String pluginDesc = "Open ASDocs for FlashDevelop 3.";
        private String pluginAuth = "Jimmy Ye";
        private String settingFilename;
        private Settings settingObject;
        private DockContent pluginPanel;
        private PluginUI pluginUI;
        private Image pluginImage;

        private const string OPEN_THE_DOC = "OpenTheDoc";

        private Dictionary<string, Book> bookCache;             // <tocPath, book>

        internal string HomePage
        {
            get { return this.settingObject.HomePage; }
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
            this.SaveSettings();
        }

        /// <summary>
        /// Handles the incoming events
        /// </summary>
        public void HandleEvent(Object sender, NotifyEvent e, HandlingPriority prority)
        {
            switch (e.Type)
            {
                // Handle custom "OpenTheDoc", and "ShowDocumentation" when HandleF1 is true.
                case EventType.Command:
                    string command = (e as DataEvent).Action;
                    bool handlingF1 = this.settingObject.HandleF1 && command == "ShowDocumentation";

                    if (command == "OpenTheDoc" || handlingF1)
                    {
                        // Copy Hashtable to Dictionary<string,string> to avoid casting
                        Hashtable itemDetailsHashtable = (e as DataEvent).Data as Hashtable;
                        Dictionary<string, string> itemDetails = new Dictionary<string, string>();

                        foreach (DictionaryEntry item in itemDetailsHashtable)
                            itemDetails.Add(item.Key as string, item.Value as string);
                        this.DebugPrint("Item Details:", itemDetails);

                        // API Search
                        List<SearchResult> resultList = this.APISearch(itemDetails);
                        this.pluginUI.UpdateSearchResultList(resultList, !this.settingObject.OpenFirstTopic);

                        string url = "";
                        if (this.settingObject.OpenFirstTopic && resultList.Count > 0)
                            url = resultList[0].filePath;
                        this.OpenHelpPanel(url);

                        e.Handled = true;
                    }
                    break;

                // Shortcut
                case EventType.Keys:
                    Keys key = (e as KeyEvent).Value;
                    if (key == this.settingObject.Shortcut)
                        this.OpenTheDoc();
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
            EventManager.AddEventHandler(this, EventType.Command | EventType.Keys);
            PluginBase.MainForm.IgnoredKeys.Add(this.settingObject.Shortcut);
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
            ObjectSerializer.Serialize(this.settingFilename, this.settingObject);
        }

        /// <summary>
        /// Opens the plugin panel if closed
        /// </summary>
        public void OpenHelpPanel(Object sender, System.EventArgs e)
        {
            this.pluginUI.Reset();
            this.OpenHelpPanel(this.HomePage);
        }

        private void OpenHelpPanel(string url)
        {
            this.pluginPanel.Show();
            this.pluginPanel.BringToFront();

            this.pluginUI.OpenUrl(url);
        }

        // Resolve current element and call command "OpenTheDoc"
        private void OpenTheDoc()
        {
            ITabbedDocument doc = PluginBase.MainForm.CurrentDocument;
            // editor ready?
            if (doc == null) return;

            ScintillaNet.ScintillaControl sci = doc.IsEditable ? doc.SciControl : null;
            if (sci != null)
                ASCompletion.Completion.ASComplete.ResolveElement(sci, OPEN_THE_DOC);
        }

        #endregion

        #region Core Methods

        // Get books those have TOC
        internal List<Book> GetBooks()
        {
            List<Book> books = new List<Book>();
            if (this.bookCache == null)
                this.bookCache = new Dictionary<string, Book>();

            foreach (string docPath in this.settingObject.DocPaths)
            {
                if (!Directory.Exists(docPath) || docPath.ToLower().StartsWith("http"))
                    continue;

                foreach (string toc in this.settingObject.TOC)
                {
                    string tocPath = Path.Combine(docPath, toc);
                    if (!File.Exists(tocPath)) continue;

                    if (this.bookCache.ContainsKey(tocPath))
                    {
                        books.Add(this.bookCache[tocPath]);
                        this.DebugPrint("From cache. tocPath: ", tocPath);
                        continue;
                    }

                    Book book = BookHelper.MakeBook(docPath, toc);
                    books.Add(book);
                    this.bookCache.Add(tocPath, book);

                    this.DebugPrint("MakeBook. tocPath: ", tocPath);
                }
            }
            return books;
        }

        internal List<string> GetBooksWithoutToc()
        {
            List<string> books = new List<string>();
            foreach (string docPath in this.settingObject.DocPaths)
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
                    if (File.Exists(tocPath)) withoutToc = false;
                }

                if (withoutToc)
                {
                    books.Add(docPath);
                    this.DebugPrint("Without TOC. docPath: ", docPath);
                }
            }
            return books;
        }

        internal List<SearchResult> APISearch(Dictionary<string, string> itemDetails)
        {
            List<SearchResult> resultList = new List<SearchResult>();

            bool isClass = itemDetails["ItmKind"] == "class";
            bool isFunction = itemDetails["ItmKind"] == "function";
            bool isTopLevelClass = itemDetails["ItmTypName"] == itemDetails["ItmTypPkgName"];

            string lang = PluginBase.CurrentProject.Language;
            this.DebugPrint("Project Language:", lang);

            #region Books with TOC
            foreach (Book book in this.GetBooks())
            {
                // Language detecting
                if (book.Categories.Count > 0 && !book.Categories.Contains(lang)) continue;

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

        internal List<SearchResult> TitleSearch(string text, bool contains, bool matchCase)
        {
            List<SearchResult> resultList = new List<SearchResult>();

            // Build XPath according to options
            string xpath;
            System.Text.StringBuilder xpathStringFormat = new System.Text.StringBuilder("//*[");

            if (contains) xpathStringFormat.Append("contains");
            else xpathStringFormat.Append("starts-with");

            if (matchCase)
            {
                xpathStringFormat.Append("(@label,'{0}')]");
                xpath = String.Format(xpathStringFormat.ToString(), text);
            }
            else
            {
                xpathStringFormat.Append("(translate(@label,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ'), '{0}')]");
                xpath = String.Format(xpathStringFormat.ToString(), text.ToUpper());
            }

            foreach (Book book in this.GetBooks())
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
        private void DebugPrint(string info, NameValueCollection nvc)
        {
            TraceManager.Add("\n" + info);
            foreach (string key in nvc.AllKeys)
                TraceManager.Add(key + ": " + nvc[key]);
        }

        #endregion
    }

    #region Custom Structure
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
    #endregion
}