# Lightmap

From Sunflow Wiki

## Lightmap Baking

1) Make sure the object you want to bake textures for has non-overlapping UV coordinates that fall in the

[0,1] square (i.e. all UVs need to be between 0 and 1).

2) Export your scene.

3) Render it to make sure you have the scene set up properly and the looks the way you want.

4) Launch a new render on the command line with the following extra arguments:

-bake <object_name>.instance

replacing <object_name> with the name of the mesh to bake. Then,

-bakedir ortho

for diffuse maps.

or

-bakedir view

for reflections/speculars as seen from the main camera which will only look right when viewed through the

same camera viewpoint.

So the command line might look something like this for a diffuse map:

java -Xmx1G -jar sunflow.jar -nogui -o "C:\outputlocation\output.xxx" "C:\scenelocation\test.sc" -bake yourobjectname.instance -bakedir ortho %*

Where 1G is 1 gig of memory to be used (you can change the amount) and where xxx is the file type you want the

map (e.g. hdr, png, exr, tga, or igi (if you use 0.07.3)).

The image that gets rendered will then be mapped back to your objects UVs.

![Image from page 143](images/page143_img1.png)

![Image from page 143](images/page143_img2.png)

![Image from page 143](images/page143_img3.png)

![Image from page 143](images/page143_img4.png)

![Image from page 143](images/page143_img5.png)

![Image from page 143](images/page143_img6.png)