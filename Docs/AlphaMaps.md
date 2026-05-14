# Alpha Maps

From Sunflow Wiki

Sunflow 0.07.2 doesn&#39;t produce RGBA images, but the 0.07.3 SVN version and up does, provided that there is

no background primitive. Future releases will also be able to utilize the alpha in RGBA textures, but that isn&#39;t in the

SVN yet.

If you&#39;re still using 0.07.2 and you&#39;re looking to produce an alpha map for your rendered image you can do this to

get it:

Have your object(s) alone in a scene and have an ambocc shader (in override mode) in the scene have a distance

of 0 and have the dark and light colors reversed (black is light, white is dark). Here is what the ambocc shader

looks like in the scene file - note that you don&#39;t need any object to have this shader and the number of samples is

irrelevant:

```text
shader {
name amboccshader
type amb-occ2
bright { "sRGB nonlinear" 0.0 0.0 0.0 }
dark { "sRGB nonlinear" 1.0 1.0 1.0 }
samples 1
dist 0.0
}
```

If you add under this the following line:

```text
override amboccshader true
```

It will cause the whole sceen to use the ambocc shader with a distance of 0, allowing you to create an alpha map -

which is really an inverted flat ao pass.

Commenting this last line out will cause the scene to render as usual:

```text
%override amboccshader true
```