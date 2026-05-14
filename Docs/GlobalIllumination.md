# GI

From Sunflow Wiki

## Contents

1 Global Illumination

1.1 Instant GI

1.2 Irradiance Caching (Final Gathering)

1.3 Path Tracing

1.4 Ambient Occlusion

1.5 Fake Ambient Term

2 Overriding

## Global Illumination

Sunflow supports 5 main global illumination (gi) types: Instant GI, Irradiance Caching (aka Final Gathering), Path

Tracing, Ambient Occlusion, and Fake Ambient Term. Remember that only one gi type at a time can be used in the

scene.

### Instant GI

This type of gi is based on this paper (pdf) (http://graphics.uni-ulm.de/Singularity.pdf) (page 6 really explains

everything in a nice way with pictures). Hopefully, my understanding of the method is okay enough that I can

adequately interpret the settings. What happens is that random points are added to the scene then a ray is traced

from a position in the scene to those points to determine the radiance of that position. If rays coming from that point

don’t meet a particular criteria (set by the bias), a ray is scattered until it does meet the criteria.

```text
gi {
type igi
samples 64
sets 1
b 0.01
```

bias-samples 0

```text
}
```

The samples values is the number of samples (virtual photons) used per set. Increasing the samples gives smoother

results, but increases render time.

The number of sets seems to be the number of sets of virtual photons emitted. The more sets, the more points, and

the more points added to the calculation, the more noisy less illuminated areas become.

The % bias, or b, is used for the estimate of direct illumination. If this value is too high, you’ll see spots of light

where the illumination is pooling, but if it’s too low you’ll lose indirect illumination contributions. This value should

be above zero, but usually low numbers do the trick. Try starting with 1 and work your way down in big steps.

If the % bias is too high, you’ll see spots of light in your scene, most often in the corners or other nooks and

crannies. Decrease the bias to get rid of them. This then can begin to reduce the amount of light in those corners

since you are effectively reducing the amount of rays finding their way to the corners. With bias-samples, you can

increase the amount of samples of those bias-diverted rays to get some of that light back into the corners. A value

of 0 will not increase the sampling so you’ll keep the slightly darker corners, but increasing the value to 1 or greater

will give unbiased sampling results. To me, this is one of the key features of the paper, so definitely experiment with

this value. Be forewarned, the larger you make this number the slower your scene will render.

You can also manipulate the number of bounces using the diffuse trace depths by adding:

```text
trace-depths {
diff 1
refl 4
refr 4
}
```

A diff of 1 means 1 bounce, a diff of 2 means two bounces, and so on. The more bounces, the longer your render

will take. The refl and refr trace-depths are used for glass shaders.

### Irradiance Caching (Final Gathering)

In a final gather, a hemisphere above a point is sampled by shooting rays at it. These rays bounce off the

hemisphere. Secondary bounces are then calculated with either path tracing rays or global photon maps. Here is

what the syntax for irradiance caching looks like when using path tracing for secondary bounces:

```text
gi {
type irr-cache
samples 512
tolerance 0.01
spacing 0.05 5.0
}
```

The samples values is the number of samples (virtual rays) used to calculate the irradiance.

The tolerance option indicates how much error you will allow in the calculation. If the tolerance is set high, the

calculation will be faster, but the result will not be as good. If the tolerance is set low, the calculation will be slower,

but the result will be better.

The spacing option is used to determine a minimum and maximum distance from the sampled point that will be used

to look for other initial ray bounces. The points found within this range will be used to determine what the irradiance

intensity should be at the initial point.

Using the above syntax, secondary bounces from the point will be calculated using path tracing, which can be pretty

slow. To speed it up, you can instead use a global photon map using the following syntax:

```text
gi {
type irr-cache
samples 512
tolerance 0.01
spacing 0.05 5.0
global 1000000 grid 100 0.75
}
```

The only difference is the addition of the global line. The increase in speed from using global photons is based on

pre-computing local irradiance values at the many photon positions. In Sunflow, you set the number of global

photons you want to use and how you want the photons mapped, either in a grid or a kd tree. The grid is a better

way to go, but at least you know the kd option is available (though using kd might throw an exception and cause

the scene to stop rendering in some cases). Then you need to define the global estimate and radius (in the example

above the estimate is 100 and the radius is 0.75). These values are used at a single secondary bounced photon (of

the many photons used). At that point, a sphere with a radius (global radius) expands outward to encompass a

certain number of other photons (global estimate) to be used to determine the irradiance at that point. For global

estimates, typically 30 to 200 photons are used.

### Path Tracing

Probably the most recognized and classic way of doing true global illumination. Sunflow&#39;s implementation only

handles diffuse inter-reflection and will not produce any caustics. There’s not much to say about it except that is

probably the most accurate, but slowest, gi method. It usually gives out a noisy image unless your samples are really

high. Its implementation is very straight forward:

```text
gi {
type path
samples 32
}
```

To calculate how many samples to use, the [Sunflow FAQ (http://home.comcast.net/~gamma-ray/sf/sunflow-

faq.htm) ] recommends setting the anti-aliasing to 0 0 in the image block, then adjusting the gi samples until the

noise disappears. Finally, increase the anti-aliasing, avoiding adaptive anti-aliasing, and divide the samples in the gi

block according to the amount of over-sampling you are doing (4, 16, etc.)

You can also manipulate the number of bounces using the diffuse trace depths by adding:

```text
trace-depths {
diff 1
refl 4
refr 4
}
```

So a diff of 1 means 1 bounce, a diff of 2 means two bounces, and so on. The more bounces you have the longer

your render will take. The refl and refr trace-depths are used for glass shaders.

### Ambient Occlusion

What more can I say than it&#39;s ambient occlusion. The settings are pretty straight forward:

```text
gi {
type ambocc
bright { "sRGB nonlinear" 1 1 1 }
dark { "sRGB nonlinear" 0 0 0 }
samples 32
maxdist 3.0
}
```

### Fake Ambient Term

Get some quick ambient light in your scene using this one. You can find the reference for this here

(http://www.cs.utah.edu/~shirley/papers/rtrt/node7.html) .

```text
gi {
type fake
up 0 1 0
sky { "sRGB nonlinear" 0 0 0 }
ground { "sRGB nonlinear" 1 1 1 }
}
```

## Overriding

Like shader overrides, you can also override the global photons and global illumination to render only these

feature’s contribution to the scene (so you can fine tune your settings). To view global photons you would use:

```text
shader {
name debug_globals
type view-global
}
```

override debug_globals false

To view the gi in the scene, you would use:

```text
shader {
name debug_gi
type view-irradiance
}
```

override debug_gi false

To render the scene normally all you would need to do is comment out the override line:

```text
% override debug_gi false
```

or

```text
% override debug_globals false
```