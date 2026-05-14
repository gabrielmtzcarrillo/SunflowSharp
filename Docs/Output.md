# Output

From Sunflow Wiki

It is important to note that when using the gui you can only save out png files, so you&#39;ll need to use the command

line to get the other file types (along with png).

From the command line, use the following commands:

-nogui -o output.hdr scenefile.sc

-nogui -o output.exr scenefile.sc

-nogui -o output.tga scenefile.sc

-nogui -o output.png scenefile.sc

When rendering really large images the exr output driver only needs to keep the active buckets in memory

instead of keeping the entire image in memory (which is good).

In the SVN (Sunflow 0.07.3), Sunflow also has support for the IGI file type (Indigo Image format (.igi) HDR file

type), so it would look like:

-nogui -o output.igi scenefile.sc

For windows users add "-nogui -o output.hdr scenefile.sc" after the sunflow.jar in the .bat file OR

c:\sunflow\sunflow.bat -nogui -o output.hdr folderinsunflow\scenefile.sc