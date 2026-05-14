# Modifiers sca

From Sunflow Wiki

## Contents

1 Modifiers

1.1 Bump Map

1.2 Normal Map

1.3 Perlin Noise

## Modifiers

There are 3 object modifiers in Sunflow. A bump, normal, and perlin. At this time, only one modifier type can be

applied to an object at a time.

The modifiers are applied to objects by adding a line to the object (or instance) below the shader declaration. Here

is an example of a sphere with a modifier:

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

param transform[2] matrix row -0.008 0.016 0.00 1.5 0.0 0.00 0.018 0.0 0.016 0.008 0.00 -2.0 0.0 0.0 0.0 1.0

```text
}
```

The image types that are recognized are tga, png, jpg, bmp, hdr, and igi. For textures, the objects must have

UVs mapped. Also note that textures can also be in relative paths. Textures are initialized as a texture object:

```text
texture myTex {
type color_texture_lookup
```

param filename string C:\myImage.jpg

```text
}
```

This object van then be called in later shaders.

### Bump Map

![Image from page 133](images/page133_img1.png)

![Image from page 133](images/page133_img2.png)

![Image from page 133](images/page133_img3.png)

![Image from page 133](images/page133_img4.png)

![Image from page 133](images/page133_img5.png)

![Image from page 133](images/page133_img6.png)

```text
modifier bumpName {
type bump_map
```

param color string myTex

```text
param scale float 1.3
}
```

### Normal Map

```text
modifier normalName {
type normal_map
```

param color string myTex

```text
}
```

### Perlin Noise

```text
modifier perlinName {
type perlin
param function int 0
param size float 1.0
param scale float 0.5
}
```

The "size" parameter affects as you might guess how spread-out or tight the bumps are. However larger values

make a tighter texture (smaller, coarser bumps), and smaller values spread it out more (bigger shallower bumps).

The scale parameter affects the height of the bumps.

A function parameter of 0 results in a generally uniform noise function. A value of 1 gives a striped function along

the x axis. Any other value for the function parameter performs a turbulence function.