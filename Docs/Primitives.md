# Primitives

From Sunflow Wiki

## Contents

1 Transforms

2 Object Motion Blur

3 Gumbo

4 Teapot

5 Background

6 Plane

6.1 Infinite Plane I

6.2 Infinite Plane II

7 Sphere

8 Hair

9 Julia

10 Particle

10.1 In 0.07.2

10.2 In 0.07.3

10.3 Other Method

11 Banchoff Surface

12 Torus

13 Cube Grid

14 Box

15 Janino Primitive

16 Cornell Box (0.07.2)

17 Cylinder (0.07.3)

18 Sphere Flake (0.07.3)

## Transforms

As with other objects you can use transforms just as described in the file-mesh transforms section. For

transforming, you can use or not use a lot of the transform options: translate (as in translate x y z), rotatex, rotatey,

rotatex, scalex, scaley, scalez, scaleu. You could also use a transform matrix by row (row) or column (col).

## Object Motion Blur

As with generic meshes, in the 0.07.3 SVN version of Sunflow primitives can be object blurred.

# Gumbo

```text
object {
shader simple_green
transform {
rotatex -90
scaleu 0.1
rotatey -25
translate 1.5 0 +1.5
}
type gumbo
name gumbo_3
subdivs 6
smooth false
}
```

# Teapot

```text
object {
shader simple_yellow
transform {
rotatex -90
scaleu 0.008
rotatey 245
translate -1.5 0 -3
}
type teapot
name teapot_1
subdivs 4
smooth false
}
```

# Background

```text
background {
color { "sRGB nonlinear" 0.057 0.221 0.400 }
}
```

# Plane

## You can define an infinite plane in two ways.

## Infinite Plane I

```text
object {
shader floor
type plane
p 0 0 0
p 4 0 3
p -3 0 4
}
```

The first p is the center of the plane, and the following two points on the plane. If you use 3 points to define a plane

instead of a single point and a normal (as described below) you will get texture coordinates on the infinite plane.

### Infinite Plane II

```text
object {
shader floor
type plane
p 0 0 0
n 0 1 0
}
```

The p is the center of the plane, and the n is the direction of the normal.

## Sphere

```text
object {
shader Mirror
type sphere
name mirror
c -30 30 20
r 20
}
```

## Hair

```text
object {
shader blue
type hair
segments 3
width .1
points 12 0.0 0.0 0.0 0.0 0.5 0.0 0.0 1.0 0.0 0.0 1.5 0.0
}
```

You put hair strands one after the other. For example, if you have 3 segments, this means each strand will need 4

vertices. So if you specify 20 points, you should end up with 5 distinct hair strands.

## Julia

```text
object {
shader simple1
transform { scalex 2 rotatey 45 rotatex -55 }
type julia
name left
q -0.125 -0.256 0.847 0.0895
iterations 8
epsilon 0.001
}
```

The line q is the main julia set parameter and defines its shape. The iterations and epsilon affect the speed and

accuracy of the calculation but are not required. If you comment these two lines out you will use the high quality

defaults.

## Particle

### In 0.07.2

```text
object {
shader grey
type particles
```

little_endian

filename "c:/your_folder/particles.dat"

```text
radius 0.03
}
```

particles.dat should be a binary file, ie: not human readable. The file should be exactly 4*3*n bytes long, where n is

the number of particles. The little_endian line is optional and will make the byte order little endian rather than the

default big endian.

### In 0.07.3

```text
object {
shader grey
type particles
points 5
```

0.5 0.2 0.3

0.6 0.2 0.3

0.7 0.2 0.3

0.8 0.2 0.3

0.9 0.2 0.3

```text
radius 0.03
}
```

The filename method still works obviously, it will be more efficient for large amounts of particles (the files can now

be specified relative to the include path).

### Other Method

Here is a workaround if you&#39;re using 0.07.2 and don&#39;t have a dat file handy. Create a .java file alongside your .sc

file with the following contents:

import org.sunflow.core.primitive.ParticleSurface;

import org.sunflow.system.Parser;

```text
public void build() {
```

parse("your_scene.sc"); // the name of your scene file goes here

```text
try {
```

Parser p = new Parser(resolveIncludeFilename("your_ascii_filename.dat")); // the name of your particle file goes here

int n = p.getNextInt();

float[] data = new float[3 * n];

for (int i = 0; i < data.length; i++)

data[i] = p.getNextFloat();

p.close();

parameter("particles", "point", "vertex", data);

parameter("num", data.length / 3);

parameter("radius", 0.1f); // the radius of the particles goes here

geometry("particle_object_name", new ParticleSurface());

parameter("shaders", "shader_name"); // replace with the shader name you want to use

instance("particle_object_name.instance", "particle_object_name");

```text
} catch (Exception e) {
```

e.printStackTrace();

```text
}
}
```

Just render this .java file from the command line instead of the main .sc file. All the paths can be relative, you only

need to specify the filename as long as they are all in the same folder... I also assumed that your ascii file contains

the number of points in the first line.

## Banchoff Surface

```text
object {
shader default-shader
transform {
rotatex -90
scaleu 5
rotatey 25
translate 0 0 0
}
type banchoff
name myShape
}
```

A banchoff surface in the .sc format is a bit different because it is pre-defined so it has no settings. This is overcome

by using transforms as in the above example. I&#39;ve also had shading issues with it but haven&#39;t investigated/asked

about it.

## Torus

```text
object {
shader myShader
type torus
r 2 4
}
```

Where the first r value is the inner radius and the second is the outer radius.

## Cube Grid

The cube grid primitive is a unit block in the creation of unique shapes. You need to use some clever java to really

use it, so take a look at the menger sponge example and the isosurface example.

![Image from page 76](images/page76_img1.png)

![Image from page 76](images/page76_img2.png)

![Image from page 76](images/page76_img3.png)

![Image from page 76](images/page76_img4.png)

![Image from page 76](images/page76_img5.png)

![Image from page 76](images/page76_img6.png)

## Box

The box primitive isn&#39;t accessbile in the .sc format but can be created in java like any other primitive. I&#39;ve added it

myself to the sc file format in 0.07.3 by using this diff (.diff)

(http://www.geneome.net/drawer/sunflow/boxSCparser.diff) . This method creates a box which you can manipulate

with transforms.

## Janino Primitive

Like shaders, you can define a primitive via janino.

```text
object {
name myJaninoObject
type janino-tesselatable
```

<code>

Your code goes here

</code>

```text
}
```

## Cornell Box (0.07.2)

In 0.07.3 and up the cornell box is considered a light. In 0.07.2, it is considered an object and so you would use

the syntax below.

```text
object {
shader none
type cornellbox
```

corner0 -60 -60 0

corner1 60 60 100

```text
left 0.80 0.25 0.25
right 0.25 0.25 0.80
top 0.70 0.70 0.70
bottom 0.70 0.70 0.70
back 0.70 0.70 0.70
emit 15 15 15
samples 32
}
```

Let&#39;s do a quick run through. This "object" doesn&#39;t really have/need a shader so it&#39;s set to none. Setting a shader

won&#39;t do anything. The size of the box is defined by corner0 and corner1. Corner0&#39;s x y z values corresponds to

position of the lower left corner of the box closest to us. Corner1 is the back top right corner. In the example

above, the center of the world (0,0) is in the center of the floor. The color of the sides of the box are then defined.

The emit and samples values are just like the meshlight.

## Cylinder (0.07.3)

```text
object {
shader default-shader
transform {
rotatex -90
scaleu 5
rotatey 25
translate 0 0 0
}
type cylinder
name myShape
}
```

A cylinder in the .sc format is a bit different because it is pre-defined so it has no settings. This is overcome by using

transforms as in the above example.

## Sphere Flake (0.07.3)

```text
object {
shader metal
type sphereflake
name left
level 7
axis 2 3 4
radius 0.5
}
```

Level, axis, and radius are optional. When defined, the axis line is a vector.