# Instances sca

From Sunflow Wiki

## Contents

1 Instances

2 Object Motion Blur

## Instances

Instancing is when you use the geometry of another object in a scene without having to duplicate the object&#39;s mesh

data. Instancing an object is useful in reducing the memory overhead, scene file size, and giving control over the

objects since you change the main object and those mesh changes will be propagated to all the instances. Instances

in Sunflow are great because you can change the location, rotation, scale, shader, and modifier of each instance.

What&#39;s great about the .sca file format is that you declare geometry and at least one instance of that geometry so it

shows up in the scene. So really, adding another instance is quite trivial. You would use the following to instance the

geometry:

```text
instance nameOfInstance {
```

param geometry string theOriginalObjectName

```text
param transform matrix row 1.0 0.0 0.0 -1.0 0.0 0.0 1.0 3.0 0.0 -1.0 0.0 -1.0 0.0 0.0 0.0 1.0
```

param shaders string[] 1

shaderForInstance

param modifiers string[] 1

modifierForInstance

```text
}
```

Of course, the modifier lines are optional.

## Object Motion Blur

As with generic meshes, instances can be object blurred.