using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OpenTheDoc
{
    public class Book
    {
        public string Path { get; set; }
        public string Title { get; set; }
        public XmlNode Toc { get; set; }
        public List<string> Categories { get; set; }

        public Book(){}

        public Book(string path, XmlNode toc)
        {
            Path = path;
            Title = (toc as XmlElement).GetAttribute("label");
            Toc = toc;

            Categories = new List<string>((toc as XmlElement).GetAttribute("categories").Split(new char[] { ',' }));
            // Remove the empty item when attribute "categories" is null or empty
            Categories.Remove("");
        }
    }
}
