# Image sca

From Sunflow Wiki

## Contents

1 Image Block

1.1 Anti-Aliasing

1.2 Samples

1.3 Contrast Threshold

1.4 Filters

1.5 Jitter

2 Bucket Size/Order

3 Banding

4 Tone Mapping

## Image Block

There image block dictates the resolution of the image as well as a few other image options. The samples and jitter

lines are optional and if they are left out samples will equal 1, contrast will be 0.1, and jitter will be false will be used

as the default.

```text
options ::options {
```

param resolutionX int 800

param resolutionY int 600

```text
param aa.min int 0
param aa.max int 2
param aa.samples int 4
param aa.contrast float 0.1
param filter string gaussian
param aa.jitter bool false
}
```

### Anti-Aliasing

The aa line is the settings for the adaptive anti-aliasing. These control the under/over-sampling of the image. The

image will be first sampled at the rate prescribed by minimum aa value (the first value). Then, based on color and

surface normal differences, the image will be refined up to the rate prescribed by the maximum aa value (the second

value). Taken from the Sunflow ReadMe file:

A value of 0 corresponds to 1 sample per pixel. A value of -1 corresponds to 1 sample every 2 pixels (1 per 2x2

block) A value of -2 corresponds to 1 sample every 4 pixels (1 per 4x4 block) A value of -3 corresponds to 1

sample every 8 pixels (1 per 8x8 block) ... A value of 1 corresponds to 4 samples per pixel (2x2 subpixel grid) A

value of 2 corresponds to 16 samples per pixel (4x4 subpixel grid) A value of 3 corresponds to 64 samples per

pixel (8x8 subpixel grid) ...

Examples:

- quick undersampled preview: -2 0

- preview with some edge refinement: 0 1

- final rendering: 1 2

You can turn adaptive anti-aliasing off by setting the min and max values to the same number. For example, an aa of

0 0 will be 1 sample per pixel with no subpixels.

You can You can see a video on explaining what adaptive anti-aliasing is here (XviD .avi)

(http://www.geneome.net/other/videotutorials/AdaptiveSamplingInSunflow-XviD.avi) .

### Samples

Samples are the number of samples. Surprised? When used they indirectly affect many aspects of the scene but

directly affects DoF and camera/object motion blur.

### Contrast Threshold

There is a line in the image block in which you can change the default contrast threshold. This affects the point at

which the renderer decides to adaptively refine between four pixels when doing adaptive anti-aliasing. This line isn&#39;t

required and the default is usually the right setting. I personally don&#39;t see a change in the render with different values.

### Filters

If you are oversampling the image (i.e., having the min and max aa positive numbers) you will likely want to use

more advanced filters to control the look of the image. The available filters are:

box (filter size = 1)

triangle (filter size = 2)

gaussian (filter size = 3)

mitchell (filter size = 4)

catmull-rom (filter size = 4)

blackman-harris (filter size = 4)

sinc (filter size = 4)

lanczos (filter size = 4)

bspline (filter size = 4)

Check this page (http://gardengnomesoftware.com/dll_patch.php) out for a good overview of filters. Triangle and

box are better for previews since they are faster. The other filters are recommended for final image rendering (my

personal favorite is mitchell).

### Jitter

The jitter line (either true or false) turns jitter on or off. Jitter moves the rays randomly a small amount to reduce

aliasing that might still be present even when anti-aliasing is turned on. So jittering these pixels makes the aliasing

issues become less perceptible. Jitter should be turned off (false) when doing animations because the randomness of

```text
jitter makes it very obvious it&#39;s on when moving from frame to frame.
```

## Bucket Size/Order

You change the bucket order in the scene file just like you can in the command line. In the scene file, if you want to

change the order from the default hilbert (for which you don&#39;t need to add any line) you would type the following as

it&#39;s own line in the .sc file:

```text
options ::options {
param bucket.size int 64
param bucket.order string column
}
```

If you want a reverse order it would look like this:

```text
options ::options {
param bucket.size int 64
```

param bucket.order string "reverse spiral"

```text
}
```

A larger bucket size means more RAM usage and less time rendering. Usually, a bucket size 64 is a good default -

especially if you are using a wide pixel filter like gaussian or mitchell. There are six bucket order types available:

hilbert (default), spiral, column, row, diagonal, and random. You can also use these ordered in reverse by adding

"reverse" in front of the name. To use reverse, you&#39;ll need to use quotes around the reverse order so that the bucket

order is still parsed as one token. The number in the line is the pixel size of the each bucket. There is some clamping

done internally to make sure the bucket size isn&#39;t too small or too big.

## Banding

Banding (or "posterization") is the effect where, instead of having a smooth color gradient, a portion of the image

shows distinct steps or bands of colors. This is particularly evident in backgrounds with large areas of a single,

slowly varying color.

This effect occurs because the 8 bits per channel in the rendered image are insufficient to accurately represent the

continuous gradation of color, and are instead rounded (or "clipped") to a nearby color value. The result is that

values jump from one integer to the next after staying constant for several pixels. Unfortunately, the human eye is

particularly sensitive to edge transitions such as these.

In this case the only solution found so far is to apply dithering (which only seems to be found in Photoshop) when

converting to 24 bit RGB. This essentially applies random noise to the image, which makes it harder to see the

transitions.

## Tone Mapping

If you find yourself looking to tone map or you intend to tone map your image before it&#39;s even rendered, you may

want to consider this tip. Render a bigger image (at least 2x in each direction), then you can reduce the number of

```text
samples since the resize (to your originally intended size) will blur the noise. This will avoid having the problem of
```

the high intensity pixels filtering too far out when Sunflow does its filtering. Ideally you would render with aa samples

set to 0 0 (fixed at 1 sample per pixel) and a really big image. Then do your tonemapping on that big image and

then you can resize to final resolution. The OpenEXR output driver is probably the better choice (vs .hdr) if you

want to do this since it can handle arbitrary image sizes (it writes a bucket at a time).