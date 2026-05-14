# GI sca

From Sunflow Wiki

## Contents

1 Global Illumination

1.1 Path Tracing

1.2 Ambient Occlusion

1.3 Fake Ambient Term

2 Overriding

## Global Illumination

Sunflow supports 3 main global illumination (gi) types: Path Tracing, Ambient Occlusion, and Fake Ambient Term.

Remember that only one gi type at a time can be used in the scene.

### Path Tracing

Probably the most recognized and classic way of doing true global illumination. Sunflow&#39;s implementation only

handles diffuse inter-reflection. There’s not much to say about it except that is probably the most accurate, but

slowest, gi method. It usually gives out a noisy image unless your samples are really high. Its implementation is very

straight forward:

```text
options ::options {
param gi.engine string path
param gi.path.samples int 32
}
```

You can also manipulate the number of bounces using the diffuse trace depths by adding:

```text
options ::options {
param depths.diffuse int 1
param depths.reflection int 4
param depths.refraction int 4
param depths.transparency int 10
}
```

So a param depths.diffuse int of 1 means 1 bounce, a param depths.diffuse int of 2 means two bounces, and so on.

The more bounces you have the longer your render will take. The other trace-depths are used for glass or thin glass

shaders.

### Ambient Occlusion

What more can I say than it&#39;s ambient occlusion. The settings are pretty straight forward:

```text
options ::options {
param gi.engine string ambocc
```

param gi.ambocc.bright color "sRGB linear" 1.0 1.0 1.0

param gi.ambocc.dark color "sRGB linear" 0.0 0.0 0.0

```text
param gi.ambocc.samples int 32
param gi.ambocc.maxdist float 3.0
}
```

### Fake Ambient Term

Get some quick ambient light in your scene using this one. You can find the reference for this here

(http://www.cs.utah.edu/~shirley/papers/rtrt/node7.html) .

```text
options ::options {
param gi.engine string fake
param gi.fake.up vector 0.0 1.0 0.0
```

param gi.fake.sky color "sRGB linear" 0.0 0.0 0.0

param gi.fake.ground color "sRGB linear" 1.0 1.0 1.0

```text
}
```

## Overriding

Overriding, or seeing the contribution of the global illumination is done via the command line. Specifically the -

view_gi flag.