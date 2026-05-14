# Primitives sca

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

8.1 Fixed Widths

8.2 Varying Widths

9 Julia

10 Particle

10.1 Other Method

11 Banchoff Surface

12 Torus

13 Cube Grid

14 Box

15 Cylinder

16 Sphere Flake

17 Implicit Surface

18 Meta Balls

19 Janino Primitive

## Transforms

As with other objects you can use transforms just as described in the file-mesh transforms section. For

transforming, you can use or not use a lot of the transform options: translate (as in translate x y z), rotatex, rotatey,

rotatex, scalex, scaley, scalez, scaleu. You could also use a transform matrix by row (row) or column (col).

## Object Motion Blur

As with generic meshes, in the 0.07.3 SVN version of Sunflow primitives can be object blurred.

# Gumbo

```text
geometry myGumbo {
type gumbo
param subdivs int 6
param smooth bool false
}
instance myGumbo.instance {
```

param geometry myGumbo

param shaders string[] 1

myShader

```text
param transform matrix row 0.091 0.042 0.0 1.5 0.0 0.0 0.1 0.0 0.042 -0.091 0.0 1.5 0.0 0.0 0.0 1.0
}
```

# Teapot

```text
geometry myTeapot {
type teapot
param subdivs int 6
param smooth bool false
}
instance myTeapot.instance {
```

param geometry myTeapot

param shaders string[] 1

myShader

```text
param transform matrix row 0.091 0.042 0.0 1.5 0.0 0.0 0.1 0.0 0.042 -0.091 0.0 1.5 0.0 0.0 0.0 1.0
}
```

# Background

```text
shader background.shader {
type constant
```

param color color "sRGB linear" 0.005 0.040 0.133

```text
}
geometry background {
type background
}
instance backgrond.instance {
param geometry string background
param shaders string background.shader
}
```

# Plane

## You can define an infinite plane in two ways.

![Image from page 125](images/page125_img1.png)

![Image from page 125](images/page125_img2.png)

![Image from page 125](images/page125_img3.png)

![Image from page 125](images/page125_img4.png)

![Image from page 125](images/page125_img5.png)

![Image from page 125](images/page125_img6.png)

![Image from page 125](images/page125_img7.png)

![Image from page 125](images/page125_img8.png)

![Image from page 125](images/page125_img9.png)

![Image from page 125](images/page125_img10.png)

![Image from page 125](images/page125_img11.png)

![Image from page 125](images/page125_img12.png)

## Infinite Plane I

```text
geometry myPlane {
type plane
param center point 0.0 0.0 0.0
param point1 point 4.0 0.0 3.0
param point2 point -3.0 0.0 4.0
}
instance myPlane.instance {
```

param geometry string myPlane

param shaders string[] 1

myShader

```text
}
```

## The first p is the center of the plane, and the following two points on the plane. If you use 3 points to define a plane

## instead of a single point and a normal (as described below) you will get texture coordinates on the infinite plane.

## Infinite Plane II

```text
geometry myPlane {
type plane
param center point 0.0 0.0 0.0
param normal vector 0.0 1.0 0.0
}
instance myPlane.instance {
```

param geometry string myPlane

param shaders string[] 1

myShader

```text
}
```

## The p is the center of the plane, and the n is the direction of the normal.

# Sphere

```text
geometry mySphere {
type sphere
}
instance mySphere.instance {
```

param geometry string mySphere

```text
param transform matrix row 20.0 0.0 0.0 -30.0 0.0 20.0 0.0 30.0 0.0 0.0 20.0 20.0 0.0 0.0 0.0 1.0
```

param shaders string[] 1

myShader

```text
}
```

# Hair

## Fixed Widths

![Image from page 126](images/page126_img1.png)

![Image from page 126](images/page126_img2.png)

![Image from page 126](images/page126_img3.png)

![Image from page 126](images/page126_img4.png)

![Image from page 126](images/page126_img5.png)

![Image from page 126](images/page126_img6.png)

```text
geometry myHair {
type hair
param segments int 3
param widths float 0.1
```

param points point[] vertex 4

0.0 0.0 0.0

0.0 0.5 0.0

0.0 1.0 0.0

0.0 1.5 0.0

```text
}
instance myHair.instance {
```

param geometry string myHair

param shaders string[] 1

myShader

```text
}
```

## For example, if you have 3 segments, this means each strand will need 4 vertices. So if you specify 20 points, you

## should end up with 5 distinct hair strands.

## Varying Widths

```text
geometry myHair {
type hair
param segments int 3
```

param widths float[] vertex 4 0.2 0.1 0.05 0.03

param points point[] vertex 4

0.0 0.0 0.0

0.0 0.5 0.0

0.0 1.0 0.0

0.0 1.5 0.0

```text
}
instance myHair.instance {
```

param geometry string myHair

param shaders string[] 1

myShader

```text
}
```

## You specify one width per vertex.

# Julia

```text
geometry myJulia {
type julia
param cw float -0.125
param cx float -0.256
param cy float 0.847
param cz float 0.0895
param iterations int 8
param epsilon float 0.0010
}
instance myJulia.instance {
```

param geometry string myJulia

param shaders string[] 1

myShader

```text
param transform matrix row 0.707 0.0 0.707 0.0 -0.579 0.574 0.579 0.0 -0.406 -0.819 0.406 0.0 0.0 0.0 0.0 1.0
}
```

## The line c values are the main julia set parameters and defines its shape. The iterations and epsilon affect the speed

## and accuracy of the calculation but are not required. If you comment these two lines out you will use the high quality

## defaults.

# Particle

```text
geometry myParticles {
type particles
```

param particles point[] vertex 5

0.5 0.2 0.3

0.6 0.2 0.3

0.7 0.2 0.3

0.8 0.2 0.3

0.9 0.2 0.3

```text
param num int 5
param radius float 0.03
}
instance myParticles.instance {
```

param geometry string myParticles

param shaders string[] 1

myShader

```text
}
```

## Other Method

## Here is an workaround when 0.07.2 was being used. Create a .java file alongside your .sca file with the following

## contents (might note work due to source code changes since 0.07.3:

import org.sunflow.core.primitive.ParticleSurface;

import org.sunflow.system.Parser;

```text
public void build() {
```

include("your_scene.sc"); // the name of your scene file goes here

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

![Image from page 128](images/page128_img1.png)

![Image from page 128](images/page128_img2.png)

![Image from page 128](images/page128_img3.png)

![Image from page 128](images/page128_img4.png)

![Image from page 128](images/page128_img5.png)

![Image from page 128](images/page128_img6.png)

## Just render this .java file from the command line instead of the main .sc file. All the paths can be relative, you only

## need to specify the filename as long as they are all in the same folder... I also assumed that your ascii file contains

## the number of points in the first line.

# Banchoff Surface

```text
geometry myBanchoff {
type banchoff
}
instance myBanchoff.instance {
```

param geometry string myBanchoff

param shaders string[] 1

myShader

```text
param transform matrix row 0.707 0.0 0.707 0.0 -0.579 0.574 0.579 0.0 -0.406 -0.819 0.406 0.0 0.0 0.0 0.0 1.0
}
```

# Torus

```text
geometry myTorus {
type torus
```

param radiusInner float 2.0

param radiusOuter float 4.0

```text
}
instance myTorus.instance {
```

param geometry string myTorus

param shaders string[] 1

myShader

```text
param transform matrix row 0.707 0.0 0.707 0.0 -0.579 0.574 0.579 0.0 -0.406 -0.819 0.406 0.0 0.0 0.0 0.0 1.0
}
```

# Cube Grid

## The cube grid primitive is a unit block in the creation of unique shapes. You need to use some clever java to really

## use it, so take a look at the menger sponge example and the isosurface example.

# Box

```text
geometry myBox {
type box
}
instance myBox.instance {
```

param geometry string myBox

param shaders string[] 1

myShader

![Image from page 129](images/page129_img1.png)

![Image from page 129](images/page129_img2.png)

![Image from page 129](images/page129_img3.png)

![Image from page 129](images/page129_img4.png)

![Image from page 129](images/page129_img5.png)

![Image from page 129](images/page129_img6.png)

![Image from page 129](images/page129_img7.png)

![Image from page 129](images/page129_img8.png)

![Image from page 129](images/page129_img9.png)

![Image from page 129](images/page129_img10.png)

![Image from page 129](images/page129_img11.png)

![Image from page 129](images/page129_img12.png)

![Image from page 129](images/page129_img13.png)

![Image from page 129](images/page129_img14.png)

![Image from page 129](images/page129_img15.png)

![Image from page 129](images/page129_img16.png)

![Image from page 129](images/page129_img17.png)

![Image from page 129](images/page129_img18.png)

```text
param transform matrix row 0.707 0.0 0.707 0.0 -0.579 0.574 0.579 0.0 -0.406 -0.819 0.406 0.0 0.0 0.0 0.0 1.0
}
```

# Cylinder

```text
<pre>geometry myCyl {
type cylinder
}
instance myCyl.instance {
```

param geometry string myCyl

param shaders string[] 1

myShader

```text
param transform matrix row 0.707 0.0 0.707 0.0 -0.579 0.574 0.579 0.0 -0.406 -0.819 0.406 0.0 0.0 0.0 0.0 1.0
}
```

# Sphere Flake

```text
geometry mySF {
type sphereflake
param level int 7
param axis vector 2.0 3.0 4.0
param radius float 0.5
}
instance mySF.instance {
```

param geometry string mySF

param shaders string[] 1

myShader

```text
param transform matrix row 0.707 0.0 0.707 0.0 -0.579 0.574 0.579 0.0 -0.406 -0.819 0.406 0.0 0.0 0.0 0.0 1.0
}
```

## The level, axis, and radius lines are optional.

# Implicit Surface

## @@@

# Meta Balls

## @@@

# Janino Primitive

## Like shaders, you can define a primitive via janino.

![Image from page 130](images/page130_img1.png)

![Image from page 130](images/page130_img2.png)

![Image from page 130](images/page130_img3.png)

![Image from page 130](images/page130_img4.png)

![Image from page 130](images/page130_img5.png)

![Image from page 130](images/page130_img6.png)

![Image from page 130](images/page130_img7.png)

![Image from page 130](images/page130_img8.png)

![Image from page 130](images/page130_img9.png)

![Image from page 130](images/page130_img10.png)

![Image from page 130](images/page130_img11.png)

![Image from page 130](images/page130_img12.png)

![Image from page 130](images/page130_img13.png)

![Image from page 130](images/page130_img14.png)

![Image from page 130](images/page130_img15.png)

![Image from page 130](images/page130_img16.png)

![Image from page 130](images/page130_img17.png)

![Image from page 130](images/page130_img18.png)

@@@