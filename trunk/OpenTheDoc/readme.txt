OpenTheDoc: Open ASDocs for FlashDevelop 3

1. Installation
Main Menu - Tools - Application Files - Copy OpenTheDoc.dll to the Plugins folder
If you have an older version, first backup your DocPaths and delete the setting file in Data/OpenTheDoc.

2. Related Topics
Put your cursor on a word and press Ctrl+F1 to search.
Click an item to Open the topic in browser.
You can choose a DocViewer in settings, such as Help Contents.

3. Help Contents
Supports ASDocs with TOC(Table Of Contents) file.
Title search.

4. Documentations
- Unorganized documentations with TOC, e.g. AS2 reference in Flash IDE.
- Well-organized documentations without TOC. You can use TocGen to generate TOCs from standard ASDocs.
- Well-organized documentations with TOC, you can find and download them in http://livedocs.adobe.com.

Some ASDocs release with Flash IDE, Flex Builder.
Flex3 reference in Flex Builder 3: 
Find doc.zip and extract it to SomeFolder\doc\, put toc.xml and tocAPI.xml to the same folder.
toc.xml is for Flex Help, and tocAPI.xml is for Language Reference.


Changelog: 

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