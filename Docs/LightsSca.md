# Lights sca

From Sunflow Wiki

## Contents

1 Lights

1.1 Samples

2 Attenuated Lights

2.1 Point Light

2.2 Meshlight/Area Light

3 Non-Attenuated Lights

3.1 Spherical Lights

3.2 Directional Lights

4 Inifinitely Far Away Lights

4.1 Image Based Light

4.2 Sunsky

5 Showboating Lights

5.1 Cornell Box

## Lights

Keep in mind that for colors the syntax sRGB nonlinear is used in these examples. These values are the color space

most of us are used to. If you prefer, you can use other color spaces which I outline in the shader overview.

It&#39;s also important to note that at this time IBL and Sunsky do not emit photons, therefore, no caustics are

viewable when using these lights. All other lights can emit photons.

### Samples

Samples, samples, samples. Samples are key for the lights that use them. If they are too low, your image will look

bad. If they are too high, you&#39;re render will take forever. So it&#39;s important to experiment to get the right setting for

your scene. I suggest starting with low samples, and working your way up till you reach just the right amount.

## Attenuated Lights

The point and area lights are attenuated, meaning that the farther away you go away from the light, the less

power/influence on the scene it has. Also remember that you can use the power/radiance in negative numbers to

remove light from the scene.

### Point Light

```text
light myPointLight {
type point
param center point 1.0 3.0 6.0
```

param power color "sRGB linear" 100.0 100.0 100.0

```text
}
```

## For the point light, power is measured in watts.

## Meshlight/Area Light

```text
light meshLamp {
type triangle_mesh
```

param radiance color "sRGB linear" 100.0 100.0 100.0

```text
param samples int 16
```

param points point[] vertex 4

0.6 0.1 6.0

0.3 1.0 6.0

1.0 1.0 5.5

1.0 0.3 5.5

param triangles int[] 6

0 1 2 0 2 3

```text
}
```

## Any mesh can become a light. For mesh lights you specify the radiance (which is watts/sr/m^2), and the total power

## is radiance*area*pi (in watts if your area was m^2). The example above is a simple square. Area lights/mesh lgihts

## automatically create soft shadows and you control the quality of shadows by changing the samples value. An

## important thing to keep in mind is that the light samples are per face (per triangle), so the more complicated the

## mesh the more it will take to render. For this reason a simple two triangle quad is probably the way to go. See this

## thread (http://sunflow.sourceforge.net/phpbb2/viewtopic.php?t=128) for more information.

# Non-Attenuated Lights

## Spherical Lights

```text
light myspherelight {
type sphere
```

param radiance color "sRGB linear" 100.0 100.0 100.0

```text
param center point 5.0 -1.5 6.0
param radius float 30.0
param samples int 16
}
```

## Directional Lights

```text
light mydirectionallight {
type directional
param source point 4.0 1.0 6.0
param dir vector -0.5 -0.2 -1.0
param radius float 23.0
```

param radiance color "sRGB linear" 1.0 1.0 1.0

```text
}
```

Intensity is controlled by the radiance value. So for a bigger intensity you could use something like:

param radiance color "sRGB linear" 100.0 100.0 100.0

## Inifinitely Far Away Lights

For IBL/Sunsky power, the exact power is measured from the content of the map or the sun position. For IBL it

will also depend on a correctly calibrated image.

### Image Based Light

```text
light myibl {
type ibl
```

param texture string C:\mypath\myimage.hdr

```text
param center vector 0.0 -1.0 0.0
param up vector 0.0 0.0 1.0
param fixed bool true
param samples int 200
param lowsamples int 200
}
```

You can use both low and high dynamic range images whose samples are controlled separately. The "center" vector

is a world space direction that points towards the center pixel in the map, letting you rotate the map however you

want. The "up" orients the map relative to that axis. This is to be able to support multiple 3d applications: some like

to have Y pointing up, others Z.

An important feature of this light is that we can turn importance sampling on or off. Using fixed bool false

(importance sampling) forces use of unique samples per shading point instead of fixing them over the whole image.

Which basically means that when lock false is set, the points of the image that will affect the light result the most (i.e.

more “important”) will be sampled more.

Importance sampling turned off (lock true) for a

variety of shaders with various settings.

![Image from page 111](images/page111_img1.png)

Importance sampling turned on (lock false).

You can find the files that were used to create the above image here (.zip)

(http://www.geneome.net/other/otherfiles/IBLTest.zip) .

So a conclusion one could draw from these tests is that when using the phong and uber shaders with high power

and glossy values respectively, using importance sampling can reduce the light points from the ibl. Increasing

```text
samples does not rid the phong and uber shaders of, what Kirk referred to as, “constellations.”
```

You don&#39;t have explicit control over the handedness of the rotation. If it looks like your image is coming in flipped,

just do that in your favorite hdr image editor.

Sunflow only supports lon/lat image maps at the moment. So if you have your images in another format (spherical

probe or cube-map) you&#39;ll need to do some kind of conversion in another program (like HDRShop). You set the

```text
samples of different types of images, for high dynamic range images, you would use "param samples int" to set the
```

samples, whereas you would use "param lowsamples int" for low dynamic range images (e.g. png files).

### Sunsky

```text
light mysunsky {
type sunsky
param up vector 0.0 0.0 1.0
param east vector 0.0 1.0 0.0
param sundir vector 0.5 0.2 0.8
param turbidity float 6.0
param samples int 16
}
```

There isn&#39;t a setting in the syntax that controls sun intensity, but you can instead control the suns direction in terms of

angle to the object. So if the Sunsky direction is at a near 0 degree angle with the object (the sun on the horizon) it

will be dark and the sky will be more a sunset color. If the direction is more high in the sky at around 80 degrees it

![Image from page 112](images/page112_img1.png)

will be bright with the sky being white/blue. Changing the up and east values can also change the look, but these are

more used to change how the Sunsky is interpreted in different world spaces which might be required in different

applications. The up and east values in the above example usually work for everyone.

The Sunsky light has a set horizon where the sky stops and the blackness of the world shows up. Normally an

infinite plane is the work-around. Future versions of Sunflow might have a control to extend the sky, but you can

also modify the source and compile Sunflow yourself so the sky extends on its own. In

src.org.sunflow.core.light.SunSkyLight.java go to the line that says

groundExtendSky = false;

Change it to

groundExtendSky = true;

Compile Sunflow and the sky will no longer terminate at the horizon.

## Showboating Lights

The Cornell Box isn&#39;t really a light that you would use in a typical scene but it is useful to illuminate your models.

### Cornell Box

```text
light nameofcornellbox {
type cornell_box
param corner0 point -60.0 -60.0 0.0
param corner1 point 60.0 60.0 100.0
```

param leftColor color "sRGB linear" 0.8 0.25 0.25

param rightColor color "sRGB linear" 0.25 0.25 0.8

param topColor color "sRGB linear" 0.7 0.7 0.7

param bottomColor color "sRGB linear" 0.7 0.7 0.7

param backColor color "sRGB linear" 0.7 0.7 0.7

param radiance color "sRGB linear" 15.0 15.0 15.0

```text
param samples int 32
}
```

Let&#39;s do a quick run through. The size of the box is defined by corner0 and corner1. Corner0&#39;s x y z values

corresponds to position of the lower left corner of the box closest to us. Corner1 is the back top right corner. In the

example above, the center of the world (0,0) is in the center of the floor. The color of the sides of the box are then

defined. The emit and samples values are just like the meshlight (above).