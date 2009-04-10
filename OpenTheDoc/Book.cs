using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using PluginCore.Helpers;

namespace OpenTheDoc
{
    public class Book
    {
        public string Path { get; set; }
        public string Title { get; set; }
        public XmlNode Toc { get; set; }

        public Book(){}

        public Book(string path, XmlNode toc)
        {
            Path = path;
            Title = XmlHelper.GetAttribute(toc, "label");
            Toc = toc;
        }
    }
}
