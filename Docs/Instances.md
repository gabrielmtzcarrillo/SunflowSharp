# Instances

From Sunflow Wiki

## Contents

1 Instances

2 Transforms

3 Object Motion Blur

## Instances

Instancing is when you use the geometry of another object in a scene without having to duplicate the object&#39;s mesh

data. Instancing an object is useful in reducing the memory overhead, scene file size, and giving control over the

objects since you change the main object and those mesh changes will be propagated to all the instances. Instances

in Sunflow are great because you can change the location, rotation, scale, shader, and modifier of each instance. In

Sunflow 0.07.2, you would use the following to instance the geometry:

```text
instance {
name nameOfInstance
```

geometry theOriginalObjectName

```text
transform {
rotatex -90
scaleu 1.0
translate -1.0 3.0 -1.0
}
shader shaderForInstance
modifier modForInstance
}
```

Of course, the modifier line is optional.

## Transforms

As with other objects you can use transforms just as described in the file-mesh transforms section. For

transforming, you can use or not use a lot of the transform options: translate (as in translate x y z), rotatex, rotatey,

rotatex, scalex, scaley, scalez, scaleu. You could also use a transform matrix by row (row) or column (col).

If you are using Blender, the exporter can take dupligrouped objects in the scene and export them out as instances.

I go over this in the Blender exporter usage page here.

## Object Motion Blur

As with generic meshes, in the 0.07.3 SVN version of Sunflow instances can be object blurred.