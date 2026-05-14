# Modifiers

From Sunflow Wiki

## Contents

1 Modifiers

1.1 Bump Map

1.2 Normal Map

1.3 Perlin Noise (SVN 0.07.3 only)

## Modifiers

There are 3 object modifiers in Sunflow. A bump, normal, and perlin. At this time, only one modifier type can be

applied to an object at a time.

The modifiers are applied to objects by adding a line to the object (or instance) below the shader declaration. Here

is an example of a sphere with a modifier:

```text
object {
shader default
modifier modName
type sphere
c -30 30 20
r 20
}
```

Also note that textures can also be in relative paths:

texture texturepath/mybump.jpg

### Bump Map

```text
modifier {
name bumpName
type bump
```

texture "C:\texturepath\mybump.jpg"

```text
scale -0.02
}
```

In 0.07.2 the bump scale is set for very small values and actaully is in the negative scale. In the SVN 0.07.3 version

of Sunflow, the scale has been fixed making it positive, but also magnifying the effect of the values by a thousand.

So a 0.07.2 value of -0.02 would be equivalent to 20 in 0.07.3. This magnification was due to finding

(http://sunflow.sourceforge.net/phpbb2/viewtopic.php?t=233) that for a bump scale in most images, a value of

around 0.001 was usually needed. So in 0.07.3, this value is now the more reasonable 1 value.

### Normal Map

```text
modifier {
name normalName
type normalmap
```

texture "C:\texturepath\mynormal.jpg"

```text
}
```

### Perlin Noise (SVN 0.07.3 only)

```text
modifier {
name perlinName
type perlin
function 0
size 1
scale 0.5
}
```

The "size" parameter affects as you might guess how spread-out or tight the bumps are. However larger values

make a tighter texture (smaller, coarser bumps), and smaller values spread it out more (bigger shallower bumps).

The scale parameter affects the height of the bumps.

A function parameter of 0 results in a generally uniform noise function. A value of 1 gives a striped function along

the x axis. Any other value for the function parameter performs a turbulence function.