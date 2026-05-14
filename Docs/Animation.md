# Animation

From Sunflow Wiki

## Contents

1 Rendering An Animation

1.1 Using 0.07.2

1.2 Using 0.07.3

1.3 Command Line

2 Image Sequences

## Rendering An Animation

In order to render an animation in Sunflow you&#39;ll have to use a command line - not the GUI. The short of it is

```text
using an exporter of your choice to output individual scene files for each frame of your animation
```

(yourBaseFileName.001, yourBaseFileName.002, etc.) and a .java file which parses the information.

### Using 0.07.2

For the 0.07.2 version of Sunflow, here&#39;s an example parsing file that has a non-animated global settings file and the

animated frames:

```text
public void build() {
```

parse("yourBaseFileName" + ".settings.sc");

parse("yourBaseFileName" + "." + getCurrentFrame() + ".sc");

```text
}
```

Where yourBaseFileName is the name of your scene file without extensions added on.

### Using 0.07.3

For the SVN 0.07.3 version of Sunflow, the above file won&#39;t work. You&#39;ll need to replace the word "parse" with

the word "nclude",i and replace "getCurrentFrame()" with "currentFrame()" so it will look like this:

```text
public void build() {
```

include("yourBaseFileName" + ".settings.sc");

include("yourBaseFileName" + "." + currentFrame() + ".sc");

```text
}
```

### Command Line

So what&#39;s the command line to send an animation to Sunflow?

-anim 1 10 -o output.#.png scenename.java

You can also use paths:

-anim 1 10 -o myFolder\output.#.png "C:\My Animations\scenename.java"

What this is saying is send frame 1 through 10 and output each frame as a png (or hdr, tga, exr, igi), using the files

identified in scenename.java.

For us windows users who are using the sunflow.bat, this is how I run it from a windows command line:

c:\sunflow\sunflow.bat -anim 1 10 -o output.#.png scene.java

OR

add "-anim 1 10 -o outputfiles\output.#.png scenefiles\scene.java" after the sunflow.jar in the .bat file.

## Image Sequences

This, of course, will render out an image sequence. You&#39;ll need to then take this sequence and render it out into a

movie (if that&#39;s what you want to do) using software that can process said images into movie files. You can use

Blender&#39;s video sequence editor to do this by adding an image and selecting all the images in the file browser. You

can then render out the image sequence.

I did a little video (.zip) (http://www.geneome.net/other/videotutorials/SunflowAnimationUsingBlender-XviD.zip) on

the subject for Blender users. Other users still might find it helpful when I use the command line to render.

Also, if you&#39;re using the Blender exporter, you can run an animation command from the script by exporting an

animation, and with the animation button pressed, select the image type, then hit the Render button.