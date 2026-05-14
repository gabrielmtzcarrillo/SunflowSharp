# Compiling

From Sunflow Wiki

## Contents

1 Compiling Intro

2 Compiling Sunflow for Java 1.6

3 Compiling Sunflow for Java 1.5

## Compiling Intro

Yes, Sunflow is open source! And you can check out the latest features being added to the SVN by getting the

source and compiling it yourself. The method described below is just one way to do it since there is also an ant

build file in the source. So if you are using an IDE (for example, Eclipse) you can use that instead. Keep in mind

that the Sunflow SVN needs Java 1.6.0 and up to work! For Mac users that have Java 1.5, see the end of this post

for how told build Sunflow for Java 1.5.

If you have Java 1.6 and up, you can download my build of the SVN (0.07.3) here (zip)

(http://www.geneome.net/other/otherfiles/SunflowSVN.zip) .

## Compiling Sunflow for Java 1.6

1) First, you&#39;ll need the source. The source uses SVN so you&#39;ll have to pick the client of choice in your OS to get

the sources. As a Windows user I use TortoiseSVN (http://tortoisesvn.net) . It&#39;s crazy easy since it&#39;s a shell

program and all you have to do is right click in a folder, select SVN checkout, and use the following location:

https://sunflow.svn.sourceforge.net/svnroot/sunflow

for command line folks, you would use:

svn co https://sunflow.svn.sourceforge.net/svnroot/sunflow sunflow

2) Once you have the sources you&#39;ll see three folders: branches, tags, and trunks. In tags, you can find older

versions or the current version, but it&#39;s trunk where you will find the latest additions not found in the final release. In

this folder you will need to create a folder named "classes." You don&#39;t need to put anything in it.

3) To compile you&#39;ll need the JDK installed and have it available in your path. For Windows users you do that by

adding the java path (C:\...\bin) to the environment variables under "path." Windows users could also use a .bat,

similar the sunflow.bat that indicates the location of java, and the following command to compile it, and unix users

can use the shell script.

If your compiling from the path, type javac to make sure it&#39;s there (or if the path is not set, just include the explicit

path to javac ("c:\program files\java\....\bin\javac") before the following:

Assuming you are in the trunk folder, the command line to compile Sunflow is:

javac -classpath janino.jar -sourcepath src -d classes -g:none -O src/*.java

If that works, you can run Sunflow from the compiled code using:

java -cp .;classes;janino.jar -server -Xmx1024M SunflowGUI

Or you can build your own sunflow.jar using the command below:

jar cvfm sunflow.jar yourmanifestname.mf -C classes\ .

The manifest file (the yourmanifestname.mf) is simply a file you create that states the main class and where it is

located. If you have the manifest in the trunk folder this is the text it should include:

Manifest-Version: 1.0

Main-Class: SunflowGUI

Class-Path: janino.jar

Be sure to end the last line with a return (blank line).

## Compiling Sunflow for Java 1.5

It&#39;s the same instructions as above, except that you&#39;ll need to apply a patch to the source code. You can see the

thread discussing this here (http://sunflow.sourceforge.net/phpbb2/viewtopic.php?t=357) , but for completeness

sake I will add phihag&#39;s patch (.diff) (http://www.geneome.net/drawer/sunflow/J15-ImagePanel.diff)

If you&#39;re looking for already made Java 1.5 SVN builds, check out phihag&#39;s site (http://phihag.de/sunflow/#sunflow-

inofficial) .