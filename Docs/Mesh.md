# Mesh

From Sunflow Wiki

## Contents

1 Triangle Meshes

1.1 Syntax

1.2 Transforms

1.3 Face Shaders/Modifiers

1.4 Object Motion Blur

2 Bezier Patches

2.1 Bezier Format Example

2.2 Transforms

3 Quad Meshes

## Triangle Meshes

### Syntax

I’ll list the general format below, then give examples of each variation. It’s important to understand that in these

cases, a point is equal to a vertex.

```text
object {
shader default
type generic-mesh
name meshName
```

points X

```text
x y z
```

…

triangles X

A B C

…

normals none/vertex/facevarying

uvs none/vertex/facevarying

```text
}
```

For triangles, it is important to note that the triangle indices are listed using the point numbers starting with 0 as the

first point. So if you have three points/vertices defined, the triangle values would look like 0 1 2. Given this way of

indexing the triangles based on the number of points, there cannot be a triangle index number above ((the number of

points) – 1). So in the below examples, you can&#39;t have a triangle with an index of 3.

For normals and uvs, you have the option of using none, vertex, or facevarying coordinates. For none, no

normals/uvs will be used. The normals and uvs don’t have to use the same type to work, so you can have “normals

vertex” and “uvs facevarying” or “normals facevarying” and “uvs none”. A key point is that if you are using the

vertex type you need the same number of values that there are points. For facevarying, you need the same number

of values equal to the number of triangles (see below for examples).

For the vertex type, each point in the mesh will need its own normal or uv coordinate:

...

```text
points 3
```

x1 y1 z1

x2 y2 z2

x3 y3 z3

```text
triangles 1
```

0 1 2

```text
normals vertex
```

d1 e1 f1

d2 e2 f2

d3 e3 f3

```text
uvs vertex
```

u1 v1

u2 v2

u3 v3

```text
}
```

For facevarying you would use this format:

...

```text
points 3
```

x1 y1 z1

x2 y2 z2

x3 y3 z3

```text
triangles 1
```

0 1 2

```text
normals facevarying
```

d1 e1 f1 d2 e2 f2 d3 e3 f3

```text
uvs facevarying
```

u1 v1 u2 v2 u3 v3

```text
}
```

Note that I don’t define the number of vertex normals or uvs like I do for the points and triangles since point and

triangle numbers will determine how many normal and uv coordinates I will need.

### Transforms

Want to rotate your object around a particular normal? Then in your mesh that has vertex normals, after the

shader/modifier call use the syntax:

```text
transform {
rotate x y z d
}
```

Where x, y, and z are the normal coordinates and d is the degrees of rotation about that normal.

What about if you want to transform a vertex based object without having to re-export the object. No problem:

```text
object {
shader default
transform {
rotatex 60.0
translate 3.2 1.2 0.8
```

...

```text
}
type generic-mesh
```

For transform, you can use or not use the transform options: translate, rotatex, rotatey, rotatex, scalex, scaley,

scalez, and scaleu. You could also use a transform matrix by row (transform row) or column (transform col).

```text
object {
shader default
transform col my4x4MatrixReadByColumn
type generic-mesh
```

Keep in mind that the values are relative to the orginal position of the object.

### Face Shaders/Modifiers

If you want to assign multiple shaders or modifiers to different faces on the same mesh, you can do that like so:

```text
object {
shaders 2
```

shaderName0

shaderName1

```text
modifiers 2
```

bumpName0

"None"

```text
type generic-mesh
name meshName
points 6
```

x1 y1 z1

x2 y2 z2

x3 y3 z3

x4 y4 z4

x5 y5 z5

x6 y6 z6

```text
triangles 2
```

0 1 2

1 2 3

```text
normals none
uvs facevarying
```

u1 v1 u2 v2 u3 v3

u4 v4 u5 v5 u6 v6

face_shaders

0

1

```text
}
```

In the face shader section you are assigning shader 0 (the first shader in the shader list - shaderName0) to the first

triangle in the triangle list, shader 1 to the second triangle list, etc. It&#39;s the same with modifiers, but remember that

for modifiers with textures (bump/normal map) you need uvs assigned. Also, if you don&#39;t have a modifier for a

face, just use "None" in the list.

## Object Motion Blur

## Sunflow 0.07.3 has added functionality to the camera, allowing you to change the shutter time which in 0.07.2 is

## clamped to [0,1]. For object motion blur to work the camera in the scene needs the shutter line added:

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

## Once that’s enabled, you would blur the object with the following syntax (similar to camera motion blur), but with

## the added “times” line, which is the time over which the motion is defined. In the below example, I am blurring the

## object to simulate it traveling some distance (translation).

```text
object {
shader someShader
modifier bumpMap
```

transform

```text
steps 3
times 0 1
{
rotatex -90
scaleu 0.018
rotatey 245
translate 1.5 0 -1
}
{
rotatex -90
scaleu 0.018
rotatey 245
translate 1.5 0 -1.5
}
{
rotatex -90
scaleu 0.018
rotatey 245
translate 1.5 0 -2
}
type teapot
name myTeapot
```

## Note that you can use row (transform row) or column (transform col) transform matrices as well.

```text
object {
shader someShader
modifier bumpMap
```

transform

```text
steps 3
times 0 1
transform col my4x4MatrixReadByColumn1
transform col my4x4MatrixReadByColumn2
transform col my4x4MatrixReadByColumn3
type teapot
name myTeapot
```

Motion blur is also one of the three things (the others being dof and camera motion blur) that are directly affected

by samples in the image block, so if it’s not there, you’ll want to add it or the default of 1 is used. For example:

```text
image {
resolution 400 225
aa 0 2
samples 3
filter gaussian
}
```

## Bezier Patches

```text
object {
shader shaderName
type bezier-mesh
```

n X Y

```text
wrap false false
```

points x y z x y z...

```text
}
```

Where n is the number of cv&#39;s in u and v, wrap is equivalent to renderman&#39;s uwrap/vwrap option (optional and

defaults to false), and points are the cv data and should be in the same order as in the renderman spec and there

should be exactly 3*(u*v) values.

### Bezier Format Example

```text
object {
shader myShader
type bezier-mesh
n 4 7
wrap false false
points 3.2 0 2.25 3.2 -0.15 2.25 2.8 -0.15 2.25 2.8 0 2.25 3.45 0 2.3625 3.45 -0.15
```

2.3625 2.9 -0.25 2.325 2.9 0 2.325 3.525 0 2.34375 3.525 -0.25 2.34375 2.8 -0.25 2.325

2.8 0 2.325 3.3 0 2.25 3.3 -0.25 2.25 2.7 -0.25 2.25 2.7 0 2.25 2.4 0 1.875 2.4 -0.25

1.875 2.3 -0.25 1.95 2.3 0 1.95 3.1 0 0.675 3.1 -0.66 0.675 2.6 -0.66 1.275 2.6 0

1.275 1.7 0 0.45 1.7 -0.66 0.45 1.7 -0.66 1.275 1.7 0 1.275

```text
}
```

### Transforms

As with other objects you can use transforms on a bezier object just as described in the file-mesh transforms

section. For transforming, you can use or not use the transform options: translate (as in translate x y z), rotatex,

rotatey, rotatex, scalex, scaley, scalez, scaleu. You could also use a transform matrix by row (transform row) or

column (transform col).

## Quad Meshes

Not in the .sc file format, but is in the source and is available in the .sca format.