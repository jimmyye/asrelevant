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

        private HelpContentsForm helpContents;
        private Image helpContentsImage;
        private FlashDevelop.Controls.Browser browser;          // html document browser
        private DockContent document;                           // container for the browser
        private List<NameValueCollection> relatedTopicsList;    // relatedTopicsList[i]: related topics in a book
        private Dictionary<string, Book> bookCache;             // <tocPath, book>

        public string HomePage
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
            // Handle custom "OpenTheDoc", and "ShowDocumentation" when HandleF1 is true.
            switch (e.Type)
            {
                case EventType.Command:
                    string command = (e as DataEvent).Action;
                    bool handlingF1 = this.settingObject.HandleF1 && command == "ShowDocumentation";

                    if (command == "OpenTheDoc" || handlingF1)
                    {
                        Hashtable itemDetailsHashtable = (e as DataEvent).Data as Hashtable;
                        Dictionary<string, string> itemDetails = new Dictionary<string, string>();
                        foreach (DictionaryEntry item in itemDetailsHashtable)
                            itemDetails.Add(item.Key as string, item.Value as string);

                        this.DebugPrint("Item Details:", itemDetails);

                        relatedTopicsList = new List<NameValueCollection>();
                        NameValueCollection relatedTopics;  // relatedTopics in a book. The first item will be a group of the ListView
                        NameValueCollection onlineDocs = null;  // The first item will be a group of the ListView

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

                            XmlNode toc = book.Toc;
                            string path = book.Path;

                            relatedTopics = new NameValueCollection();
                            relatedTopics.Add(path, XmlHelper.GetAttribute(toc, "label"));

                            // Something like "Sprite" or "Sprite *", "*" can be anything: "Sprite[ *]"
                            string formatString = "//*[@label='{0}' or starts-with(@label, '{0} ')]";
                            string xpathBase = string.Format(formatString, itemDetails["ItmName"]);

                            string xpath;
                            XmlElement result;
                            string file;

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
                                    file = Path.Combine(path, href);
                                    relatedTopics.Add(file, string.Format("{0} ({1}.{0})", itemDetails["ItmName"], itemDetails["ItmTypPkgName"]));

                                    // Traces back to look for the ItmTpyName, that is its class
                                    for (XmlElement n = result.ParentNode as XmlElement; n != null; n = n.ParentNode as XmlElement)
                                    {
                                        string label = n.GetAttribute("label");
                                        if (!label.Contains(itemDetails["ItmTypName"])) continue;

                                        href = n.GetAttribute("href");
                                        file = Path.Combine(path, href);
                                        formatString = "{0}" + (isTopLevelClass ? "" : " ({1})");
                                        label = string.Format(formatString, itemDetails["ItmTypName"], itemDetails["ItmTypPkgName"]);
                                        relatedTopics.Add(file, label);
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
                                    file = Path.Combine(path, href);
                                    relatedTopics.Add(file, string.Format("{0} ({1})", itemDetails["ItmTypName"], itemDetails["ItmTypPkgName"]));

                                    // Looks for constructors
                                    xpath = string.Format("//*[starts-with(@label, '{0}')]", itemDetails["ItmName"]);
                                    XmlNodeList results = result.Clone().SelectNodes(xpath);    // Xpath search from result.Clone(), it's root
                                    if (results.Count != 0)
                                    {
                                        foreach (XmlElement node in results)
                                        {
                                            href = node.GetAttribute("href");
                                            file = Path.Combine(path, href);
                                            relatedTopics.Add(file, string.Format("{0} constructor", itemDetails["ItmTypName"]));
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
                                if (results.Count != 0)
                                {
                                    foreach (XmlElement node in results)
                                    {
                                        string href = node.GetAttribute("href");
                                        file = Path.Combine(path, href);
                                        if (relatedTopics[file] == null)
                                        {
                                            string parentLabel = (node.ParentNode as XmlElement).GetAttribute("label");
                                            relatedTopics.Add(file, string.Format("{0} (@{1})", itemDetails["ItmTypName"], parentLabel));
                                        }
                                    }
                                }
                            }

                            if (relatedTopics.Count > 1)
                            {
                                this.relatedTopicsList.Add(relatedTopics);
                                this.DebugPrint(string.Format("Xpath Select Results of {0}:", XmlHelper.GetAttribute(toc, "label")), relatedTopics);
                            }
                        }
                        #endregion

                        #region Well-Organized Books without TOC and On-line Documents
                        foreach(string path in this.GetBooksWithoutToc())
                        {
                            string file;
                            relatedTopics = null;
                            if (path.ToLower().StartsWith("http"))
                            {
                                if (onlineDocs == null)
                                {
                                    onlineDocs = new NameValueCollection();
                                    onlineDocs.Add(path, "On-line Help (Unknown Availability)");
                                }
                                file = new Uri(new Uri(path), itemDetails["ItmTypPkgNameURL"] + ".html").ToString();
                                if (!isClass)
                                    file = file + "#" + itemDetails["ItmName"] + (isFunction ? "()" : "");
                                onlineDocs.Add(file, file);
                            }
                            else
                            {
                                file = Path.Combine(path, itemDetails["ItmTypPkgNamePath"] + ".html");
                                if (!File.Exists(file)) continue;

                                relatedTopics = new NameValueCollection();
                                relatedTopics.Add(path, path);
                                string classFile = file;
                                if (!isClass)
                                {
                                    file = file + "#" + itemDetails["ItmName"] + (isFunction ? "()" : "");
                                    relatedTopics.Add(new Uri(file).ToString(),
                                        string.Format("{0} ({1}.{0})", itemDetails["ItmName"], itemDetails["ItmTypPkgName"]));
                                }
                                
                                string formatString = "{0}" + (isTopLevelClass ? "" : " ({1})");
                                relatedTopics.Add(new Uri(classFile).ToString(),
                                    string.Format(formatString, itemDetails["ItmTypName"], itemDetails["ItmTypPkgName"]));
                            }

                            if (relatedTopics != null) this.relatedTopicsList.Add(relatedTopics);
                        }
                        if (onlineDocs != null) this.relatedTopicsList.Add(onlineDocs);
                        #endregion

                        this.ShowRelatedTopics();
                        e.Handled = true;
                    }
                    break;
            }
        }
        #endregion

        #region Custom Methods

        /// <summary>
        /// Initializes important variables
        /// </summary>
        public void InitBasics()
        {
            String dataPath = Path.Combine(PathHelper.DataDir, "OpenTheDoc");
            if (!Directory.Exists(dataPath)) Directory.CreateDirectory(dataPath);
            this.settingFilename = Path.Combine(dataPath, "Settings.fdb");
            this.pluginImage = PluginBase.MainForm.FindImage("222");
            this.helpContentsImage = PluginBase.MainForm.FindImage("92");
        }

        /// <summary>
        /// Adds the required event handlers
        /// </summary> 
        public void AddEventHandlers()
        {
            EventManager.AddEventHandler(this, EventType.Command);
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
            menu.DropDownItems.Add(new ToolStripMenuItem(LocaleHelper.GetString("Label.ViewMenuItem.PluginPanel"), this.pluginImage, new EventHandler(this.OpenDocPanel), this.settingObject.Shortcut));
            PluginBase.MainForm.IgnoredKeys.Add(this.settingObject.Shortcut);

            menu = (ToolStripMenuItem)PluginBase.MainForm.FindMenuItem("ViewMenu");
            menu.DropDownItems.Add(new ToolStripMenuItem(LocaleHelper.GetString("Label.ViewMenuItem.HelpPanel"), this.helpContentsImage, new EventHandler(this.OpenHelpContentsHandler), this.settingObject.ShortcutHelpContents));
            PluginBase.MainForm.IgnoredKeys.Add(this.settingObject.ShortcutHelpContents);
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
        /// Opens the plugin panel if closed and ends a "OpenTheDoc" action
        /// </summary>
        public void OpenDocPanel(Object sender, System.EventArgs e)
        {
            this.pluginPanel.Show();
            this.pluginPanel.BringToFront();
            
            ITabbedDocument doc = PluginBase.MainForm.CurrentDocument;
            // editor ready?
            if (doc == null) return;
            ScintillaNet.ScintillaControl sci = doc.IsEditable ? doc.SciControl : null;
            if(sci!=null)
            ASCompletion.Completion.ASComplete.ResolveElement(sci, "OpenTheDoc");
        }

        /// <summary>
        /// Opens the Help Panel if closed
        /// </summary>
        public void OpenHelpContentsHandler(Object sender, System.EventArgs e)
        {
            this.OpenHelpContents(this.HomePage);
        }

        /// <summary>
        /// Opens the Help Panel if closed
        /// </summary>
        public void OpenHelpContents(string url)
        {
            if (this.helpContents == null ||  this.helpContents.IsDisposed)
                this.helpContents = new HelpContentsForm(this);
            this.helpContents.Show();
            this.helpContents.BringToFront();

            if (url != null) this.helpContents.OpenUrl(url);
        }

        private void ShowRelatedTopics()
        {
            this.pluginUI.UpdateRelatedTopicsList(this.relatedTopicsList);
            if (this.settingObject.OpenFirstTopic)
            {
                try
                {
                    string url = relatedTopicsList[0].GetKey(1);
                    this.OpenUrl(url);
                }
                catch { return; }
            }
        }

        internal void OpenUrl(string url)
        {
            url = url.Replace('?', '#');
            if (this.settingObject.DocViewer == OpenTheDoc.DocumentViewer.InternalBrowser)
                this.GetBrowser().WebBrowser.Navigate(url);
            else if (this.settingObject.DocViewer == OpenTheDoc.DocumentViewer.SystemBrowser)
                Process.Start(url);
            else
                this.OpenHelpContents(url);

            this.DebugPrint("Doc Url:", url);
        }

        public List<Book> GetBooks()
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

        public List<string> GetBooksWithoutToc()
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

        private FlashDevelop.Controls.Browser GetBrowser()
        {
            if (this.browser == null || this.browser.IsDisposed)
            {
                this.browser = new FlashDevelop.Controls.Browser();
                this.browser.Dock = DockStyle.Fill;
            }
            if (this.document == null || this.document.IsDisposed)
                this.document = PluginBase.MainForm.CreateCustomDocument(browser);

            this.browser.WebBrowser.ScriptErrorsSuppressed = true;
            return this.browser;
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
}
