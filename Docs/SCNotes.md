# SC Notes

From Sunflow Wiki

## Contents

1 Order Of Code Blocks

2 Include

3 Commenting Out Lines

3.1 Scene File

3.2 Java File Or Janino

## Order Of Code Blocks

It&#39;s important to keep track of the order of the code blocks in cases of shaders and instances because the scene file

parser reads these code blocks in order. If the order of the blocks isn&#39;t right, for example an object with a certain

```text
shader is read before the shader block, the object won&#39;t get the shader because when Sunflow read the object&#39;s
shader it didn&#39;t know that shader since it hadn&#39;t seen it yet. Similarly with instances, the object needs to be read first
```

before the instance of that object is defined. Here&#39;s the order I suggest:

Image Settings

Lights

Shaders

Modifiers

Objects

Instances

## Include

You can include other .sc files to be used along with your orignal scene file by adding the following line to the

original scene file:

```text
include myOtherScene.sc
```

This helps when you have some big files that are hard to manage (mesh files) or if you have scene settings that you

know eill always be constant and you don&#39;t want to accidentally change.

## Commenting Out Lines

If you want to not have something in your scene to work or show up you can always comment out sections of the

scene file rather than delete blocks of code.

## Scene File

## There are 2 ways to do this in the scene file:

```text
%Comment out a single line with a "%" sign.
/* Comment out a single line
or many lines
```

with a "/*" and "*/"

```text
at the beginning and end
```

of your code */

## Java File Or Janino

## In java, There are 2 ways to do this:

```text
//Comment out a single line with "//".
/* Comment out a single line
or many lines
```

with a "/*" and "*/"

```text
at the beginning and end
```

of your code */