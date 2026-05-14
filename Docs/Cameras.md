# Cameras

From Sunflow Wiki

## Contents

1 Cameras

1.1 Pinhole

1.2 Thinlens

1.2.1 Depth of Field

1.2.2 Bokeh

1.3 Spherical

1.4 Fisheye

2 Camera Motion Blur

3 Camera Shutter

## Cameras

Sunflow has four camera types: Pinhole, thinlens, spherical, and fisheye. I&#39;ll go into the syntax used for each and a

few points that need to be made.

A main aspects of all the cameras is the eye, target, and up values. Though obvious, I&#39;ll go over what they are. Eye

is where in the world space the eye of the camera is, the target is where is the worldspace the camera is looking at,

and the up value dictates in what direction (in terms of the worldspace) the camera sees as up (basically how the

```text
camera is rotated). Cameras can also use matrix transforms in place of the eye, target, and up values such as
transform col myMatrixReadByColumn
transform row myMatrixReadByRow
```

Cameras can also have motion blur added.

### Pinhole

Probably what people would consider the "standard" perspective camera.

```text
camera {
type pinhole
eye 7.0 -6.0 5.0
target 6.0 -5.0 4.0
up -0.30 0.30 0.90
fov 49.134
aspect 1.333
}
```

The SVN (0.07.3) has a neat feature added to the pinhole, which is camera shifting. It basically takes the

perspective shot you have and shifts the view in the x or y without ruining the perspective. I would start with small

values for shift. It looks like this:

```text
camera {
type pinhole
eye 7.0 -6.0 5.0
target 6.0 -5.0 4.0
up -0.30 0.30 0.90
fov 49.134
aspect 1.333
shift 0.1 0.2
}
```

### Thinlens

Depth of Field

The thinlens camera is our depth of field (dof) camera, which is also capable of doing bokeh effects. If you activate

dof it&#39;s better to "ock"l the AA (e.g. an AA of 2/2) avoiding adaptive sampling. For a thinlens without bokeh you

use:

```text
camera {
type thinlens
eye 7.0 -6.0 5.0
target 6.0 -5.0 4.0
up -0.30 0.30 0.90
fov 49.134
aspect 1.333
fdist 30.0
lensr 1.0
}
```

Depth of field is also one of the three things (the others being motion blur and object motion blur) that are directly

affected by samples in the image block:

```text
image {
resolution 400 225
aa 0 0
samples 3
filter gaussian
}
```

Bokeh

If you want to use bokeh, you would add two attributes to the end (the number of sides and the rotation of the

effect):

```text
camera {
type thinlens
eye 7.0 -6.0 5.0
target 6.0 -5.0 4.0
up -0.30 0.30 0.90
fov 49.134
aspect 1.333
fdist 30.0
lensr 1.0
sides 6
rotation 36.0
}
```

## As with the pinhole camera, shifting has been added as an option. It looks like this:

```text
camera {
type thinlens
eye 7.0 -6.0 5.0
target 6.0 -5.0 4.0
up -0.30 0.30 0.90
fov 49.134
aspect 1.333
shift 0.2 0.2
fdist 30.0
lensr 1.0
sides 6
rotation 36.0
}
```

## Spherical

## The spherical camera produces a longitude/lattitude environment map.

```text
camera {
type spherical
eye 7.0 -6.0 5.0
target 6.0 -5.0 4.0
up -0.30 0.30 0.90
}
```

## Fisheye

## A classic lens.

```text
camera {
type fisheye
eye 7.0 -6.0 5.0
target 6.0 -5.0 4.0
up -0.30 0.30 0.90
}
```

# Camera Motion Blur

Camera motion blur is available in the current 0.07.2 release and you can see some example images below in the

second post. Object motion blur has been added to the SVN (0.07.3) and if you are looking to use it, you&#39;ll need

to compile it from the source.

Camera motion blur is also one of the three things (the others being dof and object motion blur) that are directly

affected by samples in the image block, so if it’s not there, you’ll want to add it or the default of 1 is used. For

example:

```text
image {
resolution 400 225
aa 0 2
samples 3
filter gaussian
}
```

So if your&#39;re not getting a smooth results you might want to try increasing the samples or reducing the step increases

(as described below).

Camera motion blur consists of different steps of the transform and those steps differing is aspects of the transform.

For example, here is a camera with a 3 step motion blur with the x up vector changing:

```text
camera {
type pinhole
steps 3
{
eye 1.3 -0.9 1.1
target 0.6 -0.4 0.7
up 0 0 1
}
{
eye 1.3 -0.9 1.1
target 0.6 -0.4 0.7
up 0.1 0 1
}
{
eye 1.3 -0.9 1.1
target 0.6 -0.4 0.7
up 0.2 0 1
}
fov 49.1343426412
aspect 1.33333333333
}
```

You may use this with the other camera lens types as well. Eye, target and up vector are free to vary at each step.

You can also specify transformation matrices directly using:

steps <n>

```text
transform row myMatrixReadByRow
transform row myOtherMatrixReadByRow
```

...

or

steps <n>

```text
transform col myMatrixReadByColumn
transform col myOtherMatrixReadByColumn
```

...

You can also specify transforms for the non-motion blurred case (when "steps n" is omitted). You can give the raw

data is column/row major form, or via a series of translate/scale/rotate commands. Let me know if you need more

details/examples on that part."

## Camera Shutter

Sunflow 0.07.3 has added functionality to the camera, allowing you to change the shutter time (the times over which

the moving camera is defined (uniform spacing is assumed)) which in 0.07.2 is clamped to [0,1]. For object motion

blur to work the camera in the scene needs the shutter line added:

```text
camera {
type pinhole
shutter 0 1
eye -18.19 8.97 -0.93
target -0.690 0.97 -0.93
up 0 1 0
fov 30
aspect 1.77
}
```