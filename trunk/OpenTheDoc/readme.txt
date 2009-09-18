OpenTheDoc: Open ASDocs for FlashDevelop 3

Installation
* Main Menu - Tools - Application Files - Copy OpenTheDoc.dll to the Plugins folder

API Search
* F1 to search when cursor on a word and OpenTheDoc in HelpPenal.
* Ctrl+F1 to search and open in new tab in HelpPenal.

Help Penal
* Supports ASDocs with TOC(Table Of Contents) file.
* Title search.
* Shift+F1 to show, F1, Ctrl+F1, Shift+F1 to hide.
* Tabs
* SingleInstanceMode: Only one HelpPanel for multiple FlashDevelop instances

Documentations
* Unorganized documentations with TOC, e.g. AS2 reference in Flash IDE.
* Well-organized documentations without TOC. You can use TocGen to generate TOCs from standard ASDocs.
* Well-organized documentations with TOC, you can find and download them in http://livedocs.adobe.com.
* Documentation categories. Categorize by attribute "categories" of the root node of TOC file
* 4 kinds of DocPath:
  X:\path\to\alldocs\as3\
  X:\path\to\alldocs\*, means all folders in alldocs
  $(ProjectPath)\docs
  $(GlobalClasspaths)\..\docs

Some ASDocs release with Flash IDE, Flex Builder.
Flex3 reference in Flex Builder 3: 
Find doc.zip and extract it to SomeFolder\doc\, put toc.xml and tocAPI.xml to the same folder.
toc.xml is for Flex Help, and tocAPI.xml is for Language Reference.

Download
http://code.google.com/p/asrelevant/downloads/list

Source (svn)
http://asrelevant.googlecode.com/svn/trunk/OpenTheDoc

Changes: 

2.2.0  2009-09-19
* Enable WebBrowser shortcuts (Ctrl+C, Ctrl+F, etc.)
* Language detecting. You have to add attribute "categories" to the root node of TOC file, with value: as2, as3, etc.
* HelpPanel = HelpContents + OpenTheDocPanel, like other FD panels
* Save and restore states, size...
* Documentation categories. Categorize by attribute "categories" of the root node of TOC file
* Press Refresh button to update Contents
* 3 more kinds of DocPath:
  X:\path\to\alldocs\*, means all folders in alldocs
  $(ProjectPath)\docs
  $(GlobalClasspaths)\..\docs
* Tabs
* SingleInstanceMode: Only one HelpPanel for multiple FlashDevelop instances
* New search option: "Equals"
* Others refer to Settings

2.1.0  2009-04-11
- Title Search
- HomePage for HelpContents
- Shortcut for HelpContents
- Collapse Others for contents tree
- Remove context menu from OpenTheDoc Panel
fix:
- Jump far away from the selected node.
- Any click on a branch will close it when it has only one child node.

2.0.0  2008-10-07
0. Change a lot
1. Add a panel and Help Contents
2. Show related topics in the panel
3. TOC has higher priority than Well-organized when parsing a document


1.0.2  2007-12-14
Works with AS2 docs now, thanks to the help_toc.xml file. 

1.0.1  2007-12-12 
DocPahts: You can now set paths as many as you want, instead of 4. 
Aative DocPaths: Indices of DocPaths you want OpenTheDoc to search (orderly). The first available doc will be opened. Assume that URLs are always available. 
Shortcut: Custom shortcut works now! 
Handle F1: If you like to OpenTheDoc when press F1, set this true. If OpenTheDoc doesnot find any available docs, FD handles this. 
Change "someClass.html#function" to "someClass.html#function()". 

1.0.0  2007-12-11 
Now that the HelpPanel cannot work in FD3, I wrote a really simple plugin to OpenTheDoc. I am waiting for the HelpPanel for FD3  

Usage: There are 4 path(or URL) of the docs can be set: local Flashdoc, local Flexdoc, live Flashdoc, live Flexdoc. And you can choose to open the docs in either the FD's browser or the system's, however, they both have some problems. 