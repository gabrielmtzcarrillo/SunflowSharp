# Cameras sca

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

Cameras use matrix transforms such as

param transform matrix col myMatrixReadByColumn

param transform matrix row myMatrixReadByRow

Cameras can also have motion blur added.

### Pinhole

Probably what people would consider the "standard" perspective camera.

```text
camera myCamera {
type pinhole
param transform matrix row 0.0 0.416 -0.909 -18.19 0.0 0.909 0.416 8.97 1.0 0.0 0.0 -0.93 0.0 0.0 0.0 1.0
param fov float 30.0
param aspect float 1.77
param shift.x float 0.0
param shift.y float 0.0
}
options ::options {
```

param camera string myCamera

```text
}
```

You can also camera shift (which are optional lines), which basically takes the perspective shot you have and shifts

the view in the x or y without ruining the perspective. I would start with small values for shift.

### Thinlens

Depth of Field

The thinlens camera is our depth of field (dof) camera, which is also capable of doing bokeh effects. For a thinlens

without bokeh you use:

```text
camera myCamera {
type thinlens
param transform matrix row 0.707 -0.408 0.577 7.0 0.707 0.408 -0.577 -6.0 0.0 0.816 0.577 5.0 0.0 0.0 0.0 1.0
param fov float 49.134
param aspect float 1.333
param focus.distance float 30.0
param lens.radius float 1.0
}
options ::options {
```

param camera string myCamera

```text
}
```

Depth of field is also one of the three things (the others being motion blur and object motion blur) that are directly

affected by samples in the image block.

Bokeh

If you activate dof it&#39;s better to "ock"l the AA (e.g. an AA of 2/2) avoiding adaptive sampling. If you want to use

bokeh, you would add two attributes to the end (the number of sides and the rotation of the effect):

```text
camera myCamera {
type thinlens
param transform matrix row 0.707 -0.408 0.577 7.0 0.707 0.408 -0.577 -6.0 0.0 0.816 0.577 5.0 0.0 0.0 0.0 1.0
param fov float 49.134
param aspect float 1.333
param focus.distance float 30.0
param lens.radius float 1.0
param lens.sides int 6
param lens.rotation float 36.0
}
options ::options {
```

param camera string myCamera

```text
}
```

As with the pinhole camera, shifting has been added as an option. It looks like this:

![Image from page 105](images/page105_img1.png)

![Image from page 105](images/page105_img2.png)

![Image from page 105](images/page105_img3.png)

![Image from page 105](images/page105_img4.png)

![Image from page 105](images/page105_img5.png)

![Image from page 105](images/page105_img6.png)

![Image from page 105](images/page105_img7.png)

![Image from page 105](images/page105_img8.png)

![Image from page 105](images/page105_img9.png)

![Image from page 105](images/page105_img10.png)

![Image from page 105](images/page105_img11.png)

![Image from page 105](images/page105_img12.png)

![Image from page 105](images/page105_img13.png)

![Image from page 105](images/page105_img14.png)

![Image from page 105](images/page105_img15.png)

![Image from page 105](images/page105_img16.png)

![Image from page 105](images/page105_img17.png)

![Image from page 105](images/page105_img18.png)

```text
camera myCamera {
type thinlens
param transform matrix row 0.707 -0.408 0.577 7.0 0.707 0.408 -0.577 -6.0 0.0 0.816 0.577 5.0 0.0 0.0 0.0 1.0
param fov float 49.134
param aspect float 1.333
param shift.x float 0.0
param shift.y float 0.0
param focus.distance float 30.0
param lens.radius float 1.0
param lens.sides int 6
param lens.rotation float 36.0
}
options ::options {
```

param camera string myCamera

```text
}
```

## Spherical

## The spherical camera produces a longitude/lattitude environment map.

```text
camera myCamera {
type spherical
param transform matrix row 0.707 -0.408 0.577 7.0 0.707 0.408 -0.577 -6.0 0.0 0.816 0.577 5.0 0.0 0.0 0.0 1.0
}
options ::options {
```

param camera string myCamera

```text
}
```

## Fisheye

## A classic lens.

```text
camera myCamera {
type fisheye
param transform matrix row 0.707 -0.408 0.577 7.0 0.707 0.408 -0.577 -6.0 0.0 0.816 0.577 5.0 0.0 0.0 0.0 1.0
}
options ::options {
```

param camera string myCamera

```text
}
```

# Camera Motion Blur

## Camera motion blur is available in the current release. Object motion blur has also been added.

![Image from page 106](images/page106_img1.png)

![Image from page 106](images/page106_img2.png)

![Image from page 106](images/page106_img3.png)

![Image from page 106](images/page106_img4.png)

![Image from page 106](images/page106_img5.png)

![Image from page 106](images/page106_img6.png)

![Image from page 106](images/page106_img7.png)

![Image from page 106](images/page106_img8.png)

![Image from page 106](images/page106_img9.png)

![Image from page 106](images/page106_img10.png)

![Image from page 106](images/page106_img11.png)

![Image from page 106](images/page106_img12.png)

![Image from page 106](images/page106_img13.png)

![Image from page 106](images/page106_img14.png)

![Image from page 106](images/page106_img15.png)

![Image from page 106](images/page106_img16.png)

![Image from page 106](images/page106_img17.png)

![Image from page 106](images/page106_img18.png)

Camera motion blur is also one of the three things (the others being dof and object motion blur) that are directly

affected by samples in the image block, so if it’s not there, you’ll want to add it or the default of 1 is used. So if

you&#39;re not getting a smooth results you might want to try increasing the samples or reducing the step increases (as

described below).

Camera motion blur consists of different steps of the transform (in either row or column major format) and those

steps differing is aspects of the transform. For example, here is a camera with a 3 step motion blur with the x up

vector changing:

```text
camera myCamera {
type pinhole
param shutter.open float 0.0
param shutter.close float 1.0
param transform.steps int 3
```

param transform.times float[] none 2 0.0 1.0

param transform[0] matrix row 0.581 -0.343 0.738 1.3 0.814 0.245 -0.527 -0.9 0.0 0.907 0.422 1.1 0.0 0.0 0.0 1.0

param transform[1] matrix row 0.603 -0.304 0.738 1.3 0.796 0.299 -0.527 -0.9 -0.060 0.905 0.422 1.1 0.0 0.0 0.0 1.0

param transform[2] matrix row 0.623 -0.260 0.738 1.3 0.772 0.355 -0.527 -0.9 -0.125 0.898 0.422 1.1 0.0 0.0 0.0 1.0

```text
param fov float 49.134
param aspect float 1.33
}
options ::options {
```

param camera string myCamera

```text
}
```

You may use this with the other camera lens types as well.

## Camera Shutter

Sunflow has added functionality for the pinhole and thinlens cameras, allowing you to change the shutter time (the

times over which the moving camera is defined (uniform spacing is assumed)) which used to be clamped to [0,1].

For object motion blur to work the camera in the scene needs the shutter line added:

```text
camera myCamera {
type pinhole
param shutter.open float 0.0
param shutter.close float 1.0
param transform matrix row 0.0 0.416 -0.909 -18.19 0.0 0.909 0.416 8.97 1.0 0.0 0.0 -0.93 0.0 0.0 0.0 1.0
param fov float 30.0
param aspect float 1.77
param shift.x float 0.0
param shift.y float 0.0
}
options ::options {
```

param camera string myCamera

```text
}
```

![Image from page 107](images/page107_img1.png)

![Image from page 107](images/page107_img2.png)

![Image from page 107](images/page107_img3.png)

![Image from page 107](images/page107_img4.png)

![Image from page 107](images/page107_img5.png)

![Image from page 107](images/page107_img6.png)

![Image from page 107](images/page107_img7.png)

![Image from page 107](images/page107_img8.png)

![Image from page 107](images/page107_img9.png)

![Image from page 107](images/page107_img10.png)

![Image from page 107](images/page107_img11.png)

![Image from page 107](images/page107_img12.png)