# Mesh sca

From Sunflow Wiki

## Contents

1 Triangle Meshes

1.1 Syntax

1.2 Transforms

1.3 Face Shaders/Modifiers

1.4 Object Motion Blur

2 Bezier Patches

2.1 Bezier Format Example

3 Quad Meshes

4 Subdivision Surfaces

## Triangle Meshes

### Syntax

I’ll list the general format below, then give examples of each variation. It’s important to understand that in these

cases, a point is equal to a vertex.

```text
geometry objectName {
type triangle_mesh
```

param points point[] vertex X

```text
x y z
```

…

param triangles int[] X

A B C

…

param normals vector[] none/vertex/facevarying

param uvs texcoord[] none/vertex/facevarying

```text
}
instance objectName.instance {
```

param geometry string objectName

param shaders string[] 1

myShader

```text
}
```

For triangles, it is important to note that the triangle indices are listed using the point numbers starting with 0 as the

first point. So if you have three points/vertices defined, the triangle values would look like 0 1 2. Given this way of

indexing the triangles based on the number of points, there cannot be a triangle index number above ((the number of

points) – 1). So in the below examples, you can&#39;t have a triangle with an index of 3.

For normals and uvs, you have the option of using none, vertex, or facevarying coordinates. For none, no

normals/uvs will be used. The normals and uvs don’t have to use the same type to work, so you can have normals

vertex and uvs facevarying or normals facevarying and uvs none. A key point is that if you are using the vertex type

you need the same number of values that there are points. For facevarying, you need the same number of values

equal to the number of triangles (see below for examples).

For the vertex type, each point in the mesh will need its own normal or uv coordinate:

...

param points point[] vertex 3

x1 y1 z1

x2 y2 z2

x3 y3 z3

param triangles int[] 3

0 1 2

param normals vector[] vertex 3

d1 e1 f1

d2 e2 f2

d3 e3 f3

param uvs texcoord[] vertex 3

u1 v1

u2 v2

u3 v3

```text
}
```

For facevarying you would use this:

...

param points point[] vertex 3

x1 y1 z1

x2 y2 z2

x3 y3 z3

param triangles int[] 3

0 1 2

param normals vector[] facevarying 3

d1 e1 f1

d2 e2 f2

d3 e3 f3

param uvs texcoord[] facevarying 3

u1 v1

u2 v2

u3 v3

```text
}
```

Note that I don’t define the number of vertex normals or uvs like I do for the points and triangles since point and

triangle numbers will determine how many normal and uv coordinates I will need.

### Transforms

What about if you want to transform a vertex based object without having to re-export the object. No problem,

just transfrom the instance of the geometry:

```text
instance objectName.instance {
```

param geometry string objectName

param shaders string[] 1

myShader

```text
param transform matrix row 0.091 0.042 0.0 1.5 0.0 0.0 0.1 0.0 0.042 -0.091 0.0 1.5 0.0 0.0 0.0 1.0
}
```

## Keep in mind that the transforms are relative to the original position of the geometry.

## Face Shaders/Modifiers

## If you want to assign multiple shaders or modifiers to different faces on the same mesh, you can do that like so:

```text
<pre>geometry objectName {
type triangle_mesh
```

param points point[] vertex X

```text
x y z
```

…

param triangles int[] X

A1 B1 C1

A2 B2 C2

…

param normals vector[] none/vertex/facevarying

param uvs texcoord[] none/vertex/facevarying

param faceshaders int[] 2

0

1

```text
}
instance objectName.instance {
```

param geometry string objectName

param shaders string[] 2

myShader0

myShader1

param modifiers string[] 2

myModifier0

none

```text
}
```

## In the face shader section you are assigning shader 0 (the first shader in the shader list - myShader0) to the first

## triangle in the triangle list, shader 1 to the second triangle list, etc. It&#39;s the same with modifiers, but remember that

## for modifiers with textures (bump/normal map) you need uvs assigned. Also, if you don&#39;t have a modifier for a

## face, just use "None" in the list.

## Object Motion Blur

## Sunflow allows you to change the shutter time of the camera which by default clamped to [0,1]. For object motion

## blur to work the camera (pinhole or thinlens) in the scene needs the shutter lines added:

```text
camera myCamera {
type pinhole
param shutter.open float 0.0
param shutter.close float 1.0
param transform matrix row 0.0 0.416 -0.909 -18.19 0.0 0.909 0.416 8.97 1.0 0.0 0.0 -0.93 0.0 0.0 0.0 1.0
param fov float 30.0
param aspect float 1.77
}
```

![Image from page 118](images/page118_img1.png)

![Image from page 118](images/page118_img2.png)

![Image from page 118](images/page118_img3.png)

![Image from page 118](images/page118_img4.png)

![Image from page 118](images/page118_img5.png)

![Image from page 118](images/page118_img6.png)

```text
options ::options {
```

param camera string myCamera

```text
}
```

## Once that’s enabled, you would blur the object with the following syntax (similar to camera motion blur), but with

## the added “times” line, which is the time over which the motion is defined (uniform spacing is assumed). In the

## below example, I am blurring the object to simulate it traveling some distance (translation).

```text
geometry myTeapot {
type teapot
}
instance myTeapot.instance {
```

param geometry string myTeapot

param shaders string[] 1

someShader

param modifiers string[] 1

bumpMap

```text
param transform.steps int 3
```

param transform.times float[] none 2 0.0 1.0

param transform[0] matrix row -0.006 0.016 0.0 1.5 0.0 0.0 0.018 0.0 0.016 0.008 0.0 -1.0 0.0 0.0 0.0 1.0

param transform[1] matrix row -0.008 0.016 0.0 1.5 0.0 0.0 0.018 0.0 0.016 0.008 0.0 -1.5 0.0 0.0 0.0 1.0

param transform[2] matrix row -0.008 0.016 0.0 1.5 0.0 0.0 0.018 0.0 0.016 0.008 0.0 -2.0 0.0 0.0 0.0 1.0

```text
}
```

## Motion blur is also one of the three things (the others being dof and camera motion blur) that are directly affected

## by samples in the image block, so if it’s not there, you’ll want to add it or the default of 1 is used.

# Bezier Patches

```text
geometry myObject {
type bezier_mesh
```

param nu int X

param nv int X

```text
param uwrap bool false
param vwrap bool false
```

param points point[] vertex X

```text
x y z
```

...

```text
}
instance myObject.instance {
```

param geometry string myObject

param shaders string[] 1

myShader

```text
param transform matrix row 0.091 0.042 0.0 1.5 0.0 0.0 0.1 0.0 0.042 -0.091 0.0 1.5 0.0 0.0 0.0 1.0
}
```

## Where X is the number of cv&#39;s in u and v, wrap is equivalent to renderman&#39;s uwrap/vwrap option (optional and

## defaults to false), and points are the cv data and should be in the same order as in the renderman spec and there

## should be exactly 3*(u*v) values.

![Image from page 119](images/page119_img1.png)

![Image from page 119](images/page119_img2.png)

![Image from page 119](images/page119_img3.png)

![Image from page 119](images/page119_img4.png)

![Image from page 119](images/page119_img5.png)

![Image from page 119](images/page119_img6.png)

![Image from page 119](images/page119_img7.png)

![Image from page 119](images/page119_img8.png)

![Image from page 119](images/page119_img9.png)

![Image from page 119](images/page119_img10.png)

![Image from page 119](images/page119_img11.png)

![Image from page 119](images/page119_img12.png)

![Image from page 119](images/page119_img13.png)

![Image from page 119](images/page119_img14.png)

![Image from page 119](images/page119_img15.png)

![Image from page 119](images/page119_img16.png)

![Image from page 119](images/page119_img17.png)

![Image from page 119](images/page119_img18.png)

## Bezier Format Example

```text
geometry myObject {
type bezier_mesh
param nu int 4
param nv int 7
param uwrap bool false
param vwrap bool false
```

param points point[] vertex 28

3.2 0.0 2.25

3.2 -0.15 2.25

2.8 -0.15 2.25

2.8 0.0 2.25

3.45 0.0 2.3625

3.45 -0.15 2.3625

2.9 -0.25 2.325

2.9 0.0 2.325

3.525 0.0 2.34375

3.525 -0.25 2.34375

2.8 -0.25 2.325

2.8 0.0 2.325

3.3 0.0 2.25

3.3 -0.25 2.25

2.7 -0.25 2.25

2.7 0.0 2.25

2.4 0.0 1.875

2.4 -0.25 1.875

2.3 -0.25 1.95

2.3 0.0 1.95

3.1 0.0 0.675

3.1 -0.66 0.675

2.6 -0.66 1.275

2.6 0.0 1.275

1.7 0.0 0.45

1.7 -0.66 0.45

1.7 -0.66 1.275

1.7 0.0 1.275

```text
}
instance myObject.instance {
```

param geometry string myObject

param shaders string[] 1

myShader

```text
param transform matrix row 0.091 0.042 0.0 1.5 0.0 0.0 0.1 0.0 0.042 -0.091 0.0 1.5 0.0 0.0 0.0 1.0
}
```

# Quad Meshes

## @@@

# Subdivision Surfaces

## Implements Catmull-Clark type subdivisions from raw mesh data. Currently there is a constant level via the number

## of iterations.

## Syntax needed.

![Image from page 120](images/page120_img1.png)

![Image from page 120](images/page120_img2.png)

![Image from page 120](images/page120_img3.png)

![Image from page 120](images/page120_img4.png)

![Image from page 120](images/page120_img5.png)

![Image from page 120](images/page120_img6.png)