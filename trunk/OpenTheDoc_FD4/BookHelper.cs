using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using PluginCore.Helpers;

namespace OpenTheDoc
{
    class BookHelper
    {
        public static string FLASH_TOC = "help_toc.xml";
        
        public static Book MakeBook(string docPath, string toc)
        {
            string tocPath = Path.Combine(docPath, toc);
            XmlDocument xd = new XmlDocument();
            xd.PreserveWhitespace = false;
            xd.Load(tocPath);
            XmlNode tocRoot = xd.DocumentElement;

            // TOC structure of Flash doc is different
            if (toc.Equals(FLASH_TOC))
                tocRoot = FlashToc2FlexToc(tocRoot);

            Book book = new Book(docPath, tocRoot);
            return book;
        }

        // Flash TOC nodes have levels, "level2" deeper than "level1", 
        // "level" > "book", "level2" > "level1", etc.
        private static XmlNode FlashToc2FlexToc(XmlNode tocRoot)
        {
            // Create a new root node without the childNodes of tocRoot
            XmlElement newToc = tocRoot.CloneNode(false) as XmlElement;
            newToc.SetAttribute("label", newToc.GetAttribute("title"));
            newToc.RemoveAttribute("title");

            Stack parentNodes = new Stack();
            parentNodes.Push(newToc);

            foreach (XmlNode child in tocRoot.ChildNodes)
            {
                XmlNode currentParentNode = parentNodes.Peek() as XmlNode;
                XmlNode node = child.Clone();
                ToFlexTocNode(node);

                int result = node.Name.CompareTo(currentParentNode.Name);
                switch (result)
                {
                    case 1: // node has a deeper level, make currentParentNode its parent
                        currentParentNode.AppendChild(node);
                        parentNodes.Push(node);
                        break;
                    case 0: // They have the same level, push node into parentNodes
                        parentNodes.Pop();
                        (parentNodes.Peek() as XmlNode).AppendChild(node);
                        parentNodes.Push(node);
                        break;
                    case -1: // node has a higher level
                        while (node.Name.CompareTo((parentNodes.Pop() as XmlNode).Name) < 0)
                            ;//do nothing
                        (parentNodes.Peek() as XmlNode).AppendChild(node);
                        parentNodes.Push(node);
                        break;
                }
            }
            return newToc;
        }

        private static void ToFlexTocNode(XmlNode flashTocNode)
        {
            XmlElement xe = flashTocNode as XmlElement;
            xe.SetAttribute("label", xe.GetAttribute("name"));
            xe.RemoveAttribute("name");
        }
    }
}
