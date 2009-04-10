using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Xml.XPath;

namespace TocGen
{
    public class TocGen
    {
        static public string OTD_TOC = "otd_toc.xml";
        private Dictionary<string, XmlNode> nodeCache;
        private XmlDocument xd;

        public string docPath { get; set; }
        public string title { get; set; }

        public TocGen(string title, string docPath)
        {
            this.docPath = docPath;
            this.title = title;
        }

        static public string getTitle(string docPath)
        {
            string title = "";
            string indexHtmlPath = Path.Combine(docPath, "index.html");
            if (File.Exists(indexHtmlPath))
            {
                XmlNode indexXml = Html2Xml(indexHtmlPath);
                if (indexXml != null)
                {
                    XmlNode titleNode = indexXml.SelectSingleNode("//title");
                    if (titleNode != null)
                        title = titleNode.InnerText;
                }
            }
            return title;
        }

        public void Generate()
        {
            if (!Directory.Exists(docPath)) return;

            nodeCache = new Dictionary<string, XmlNode>();
            xd = new XmlDocument();
            xd.AppendChild(xd.CreateXmlDeclaration("1.0", "UTF-8", null));
            
            XmlElement toc = xd.CreateElement("toc");
            toc.SetAttribute("label", title);
            xd.AppendChild(toc);

            // All Classes
            string allClassesLabel = "All Classes";
            string allClassesHref = "class-summary.html";
            if (File.Exists(Path.Combine(docPath, allClassesHref)))
            {
                XmlNode allClasses = createTopicNode(allClassesLabel, allClassesHref);
                ParseSummary(allClasses);
                toc.AppendChild(allClasses);

                foreach (XmlNode classNode in allClasses)
                    ParseClass(classNode);

                MainForm.Output(allClassesLabel + " done: " + allClasses.ChildNodes.Count + " classes.");
            }

            // All Packages
            string allPackagesLabel = "All Packages";
            string allPackagesHref = "package-summary.html";
            if (File.Exists(Path.Combine(docPath, allPackagesHref)))
            {
                XmlNode allPackages = createTopicNode(allPackagesLabel, allPackagesHref);
                ParseSummary(allPackages);
                toc.AppendChild(allPackages);

                foreach (XmlNode packageNode in allPackages)
                {
                    ParseSummary(packageNode);
                    foreach (XmlNode classNode in packageNode)
                        ParseClass(classNode);
                }
                MainForm.Output(allPackagesLabel + " done: " + allPackages.ChildNodes.Count + " packages.");
            }

            // Language Elements
            string languageElementsLabel = "Language Elements";
            string languageElementsHref = "language-elements.html";
            if (File.Exists(Path.Combine(docPath, languageElementsHref)))
            {
                XmlNode languageElements = createTopicNode(languageElementsLabel, languageElementsHref);
                ParseSummary(languageElements);
                toc.AppendChild(languageElements);

                foreach (XmlNode languageElement in languageElements)
                    ParseLanguageElement(languageElement);

                MainForm.Output(languageElementsLabel + " done.");
            }

            // Index
            string indexLabel = "Index";
            string indexHref = "all-index-A.html";
            if (File.Exists(Path.Combine(docPath, indexHref)))
            {
                XmlNode index = createTopicNode(indexLabel, indexHref);
                toc.AppendChild(index);

                MainForm.Output(indexLabel + " done.");
            }

            // Appendixes
            string appendixesLabel = "Appendixes";
            string appendixesHref = "appendixes.html";
            if (File.Exists(Path.Combine(docPath, appendixesHref)))
            {
                XmlNode appendixes = createTopicNode(appendixesLabel, appendixesHref);
                ParseSummary(appendixes);
                toc.AppendChild(appendixes);

                MainForm.Output(appendixesLabel + " done.");
            }

            // Conventions
            string conventionsLabel = "Conventions";
            string conventionsHref = "conventions.html";
            if (File.Exists(Path.Combine(docPath, conventionsHref)))
            {
                XmlNode conventions = createTopicNode(conventionsLabel, conventionsHref);
                toc.AppendChild(conventions);

                MainForm.Output(conventionsLabel + " done.");
            }
        }

        public void Write()
        {
            xd.Save(Path.Combine(docPath, OTD_TOC));
        }

        // all interfaces and classes are in both "All Classes" and "All Packages"
        private XmlNode getCacheNode(string href)
        {
            return nodeCache.ContainsKey(href) ? nodeCache[href].Clone() : null;
        }

        private string GetAttribute(XmlNode node, string attr)
        {
            return (node as XmlElement).GetAttribute(attr);
        }

        // class-summary, package-summary, etc.
        private void ParseSummary(XmlNode summaryNode)
        {
            string summaryHref = GetAttribute(summaryNode, "href");
            XmlNode summaryPageXml = Html2Xml(Path.Combine(docPath, summaryHref));
            if (summaryPageXml == null) return;

            string xpathSummaryTable = "//table[@class='summaryTable']";
            XmlNodeList summaryTableList = summaryPageXml.SelectNodes(xpathSummaryTable);

            foreach (XmlNode summaryTable in summaryTableList)
            {
                XmlNode parentNode = summaryNode;
                string subSummaryLabel = summaryTable.PreviousSibling.InnerText;
                if (subSummaryLabel.Contains("Functions") || subSummaryLabel.Contains("Constants"))
                {
                    string subSummaryHref = summaryHref + '#' + GetAttribute(summaryTable.PreviousSibling.PreviousSibling, "name");
                    XmlNode subSummaryNode = createTopicNode(subSummaryLabel, subSummaryHref);
                    summaryNode.AppendChild(subSummaryNode);
                    parentNode = subSummaryNode;
                    // Cache it
                    //nodeCache.Add(subSummaryHref, subSummaryNode);
                }
                // TODO: sort the list. interfaces are now in the beginning of the list.
                string xpathItemAnchor = "//td[@class='summaryTableSecondCol']/a | //td[@class='summaryTableSecondCol']/i/a";
                XmlNodeList itemAnchorList = summaryTable.Clone().SelectNodes(xpathItemAnchor);

                string baseURL = summaryHref.Substring(0, summaryHref.LastIndexOf('/') + 1);
                foreach (XmlNode itemAnchor in itemAnchorList)
                {
                    string itemLabel = itemAnchor.InnerText.Trim();
                    string itemHref = baseURL + GetAttribute(itemAnchor, "href");

                    XmlNode itemNode = getCacheNode(itemHref);
                    if (itemNode == null)
                        itemNode = createTopicNode(itemLabel, itemHref);
                    parentNode.AppendChild(itemNode);
                }
            }
        }

        // class
        private void ParseClass(XmlNode classNode)
        {
            string classHref = GetAttribute(classNode, "href");
            if (getCacheNode(classHref) != null) return;

            XmlNode classPageXml = Html2Xml(Path.Combine(docPath, classHref));
            if (classPageXml == null) return;

            XmlNodeList sectionDivList = classPageXml.SelectNodes("//div[@class='summarySection']");
            foreach (XmlNode sectionDiv in sectionDivList)
            {
                // <div class="summaryTableTitle">Public Properties</div>
                string sectionLabel = sectionDiv.FirstChild.InnerText.Trim();
                // <a name="propertySummary"/>
                string sectionHref = classHref + '#' + GetAttribute(sectionDiv.PreviousSibling, "name");
                XmlNode sectionNode = createTopicNode(sectionLabel, sectionHref);
                classNode.AppendChild(sectionNode);

                string xpathMemberTag = "//tr[@class='']/td[@class='summaryTableSignatureCol']//*[(name()='a' or name()='span') and @class='signatureLink']";
                XmlNodeList memberTagList = sectionDiv.Clone().SelectNodes(xpathMemberTag);

                foreach (XmlNode memberTag in memberTagList)
                {
                    string memberLabel = memberTag.InnerText;

                    string memberHref = GetAttribute(memberTag, "href");
                    if (memberHref == null) // style
                        memberHref = '#' + GetAttribute(memberTag.ParentNode.ParentNode.ParentNode.FirstChild.FirstChild, "name");
                    memberHref = classHref + memberHref;

                    XmlNode memberNode = createTopicNode(memberLabel, memberHref);
                    sectionNode.AppendChild(memberNode);
                }
            }
            // Cache it
            nodeCache.Add(classHref, classNode);
        }

        // "language-elements.html"
        private void ParseLanguageElement(XmlNode summaryNode)
        {
            string summaryHref = GetAttribute(summaryNode, "href");

            // Global Constants & Global Functions are in one page, different section.
            if (summaryHref.Contains("#")) parseSection(summaryNode);

            XmlNode summaryPageXml = Html2Xml(Path.Combine(docPath, summaryHref));
            if (summaryPageXml == null) return;

            string xpathItemAnchor = "//td[@class='summaryTableSignatureCol' or @class='summaryTableStatementCol']/a[@class='signatureLink']";
            XmlNodeList itemAnchorList = summaryPageXml.SelectNodes(xpathItemAnchor);
            foreach (XmlNode itemAnchor in itemAnchorList)
            {
                string itemLabel = itemAnchor.InnerText;
                if (summaryHref.Equals("operators.html"))
                {
                    string operatorLabel = itemAnchor.ParentNode.PreviousSibling.InnerText;
                    if (!operatorLabel.Trim().Equals(""))
                        itemLabel = string.Format("{0} ({1})", operatorLabel, itemAnchor.InnerText);
                }
                string itemHref = summaryHref + GetAttribute(itemAnchor, "href");

                XmlNode itemNode = createTopicNode(itemLabel, itemHref);
                summaryNode.AppendChild(itemNode);
            }
        }

        // Global Constants & Global Functions in Language Elements
        private void parseSection(XmlNode summaryNode)
        {
            string summaryHref = GetAttribute(summaryNode, "href");

            string[] temp = summaryHref.Split(new char[] { '#' });
            summaryHref = temp[0];
            string sectionAnchor = temp[1];

            XmlNode summaryPageXml = Html2Xml(Path.Combine(docPath, summaryHref));
            if (summaryPageXml == null) return;

            string xpathSection = string.Format("//a[@name='{0}']", sectionAnchor);
            XmlNode sectionAnchorNode = summaryPageXml.SelectSingleNode(xpathSection);
            if (sectionAnchorNode == null) return;

            XmlNode sectionNode = sectionAnchorNode.NextSibling.NextSibling;

            string xpathItemAnchor = "//td[@class='summaryTableSignatureCol']//a[@class='signatureLink']";
            XmlNodeList itemAnchorList = sectionNode.Clone().SelectNodes(xpathItemAnchor);
            foreach (XmlNode itemAnchor in itemAnchorList)
            {
                string itemLabel = itemAnchor.InnerText;
                string itemHref = summaryHref + GetAttribute(itemAnchor, "href");

                XmlNode itemNode = createTopicNode(itemLabel, itemHref);
                summaryNode.AppendChild(itemNode);
            }
        }

        // create a "topic" XmlNode that has "label" and "href" attribute
        private XmlNode createTopicNode(string label, string href)
        {
            XmlElement topicNode = xd.CreateNode(XmlNodeType.Element, "topic", "") as XmlElement;
            topicNode.SetAttribute("label", label);
            topicNode.SetAttribute("href", href);

            return topicNode;
        }

        // convert html to xml and return the root node
        static private XmlNode Html2Xml(string htmlPath)
        {
            if (!File.Exists(htmlPath)) return null;
            try
            {
                // "An XmlReader implementation for loading SGML (including HTML)
                // converting it to well formed XML..."
                // http://wiki.developer.mindtouch.com/SgmlReader
                Sgml.SgmlReader r = new Sgml.SgmlReader();

                StringWriter sw = new StringWriter();
                XmlTextWriter w = new XmlTextWriter(sw);

                string htmlString = File.ReadAllText(htmlPath);
                if (htmlPath.EndsWith("operators.html"))
                    htmlString = htmlString.Replace("<<", "&lt;<");
                StringReader sr = new StringReader(htmlString);

                r.DocType = "HTML";
                r.InputStream = sr;

                while (!r.EOF)
                    w.WriteNode(r, true);
                w.Close();

                XmlDocument xd = new XmlDocument();
                xd.LoadXml(sw.ToString());
                return xd.DocumentElement;
            }
            catch (Exception ex)
            {
                MainForm.Output("Error occurs when parsing " + htmlPath);
                MainForm.Output(ex.Message);
                return null;
            }
        }
    }
}
