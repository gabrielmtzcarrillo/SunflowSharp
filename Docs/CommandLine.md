# Commandline

From Sunflow Wiki

## Contents

1 Command Line Flags

1.1 List Of Flags

1.2 Flag Descriptions

1.3 Command Line Shaders

1.4 For SVN 0.07.3 only

## Command Line Flags

Sunflow&#39;s GUI is great for getting your scene rendered to a png file, but to really unlock Sunflow, there are several

command line options available to control the render result. There are several ways to access Sunflow through the

command line like editing the sunflow.bat/sunflow.sh file or navigating to the sunflow.jar location to type in the

commands. Here is the general look of the comamnd line with a few flags added:

```text
%Sunflow Path%\java -Xmx1G -jar sunflow.jar -quick_uvs -nogui -o "C:\outputlocation\output.png" "C:\scenelocation\test.sc"
```

### List Of Flags

For completeness sake I will list all the available command line flags but you can use the -help flag to get a list as

well:

-resolution W H

-aa MIN MAX

-filter X

-nogui

-o "filename"

-bucket X Y

-threads X

-smallmesh

-nogi

-nocaustics

-ipr

-anim

-bake <object_name>.instance, -bakedir ortho/-bakedir view

-sampler type

-lopri

-hipri

-dumpkd

![Image from page 138](images/page138_img1.png)

![Image from page 138](images/page138_img2.png)

![Image from page 138](images/page138_img3.png)

![Image from page 138](images/page138_img4.png)

![Image from page 138](images/page138_img5.png)

![Image from page 138](images/page138_img6.png)

-buildonly

-showaa

-pathgi X

-bench

-rtbench

-frame X

-X verbosity

-h or -help

-quick_uvs

-quick_normals

-quick_id

-quick_prims

-quick_gray

-quick_wire -aa MIN MAX -filter X

-quick_ambocc X

### Flag Descriptions

Here are some quick descriptions of the flags:

-resolution W H : Sets the image resolution, overriding the one set in the scene file.

-aa MIN MAX : You can set the anti-aliasing of the scene in the command line, overriding the one set in the

scene file.

-filter X : You can set the filter used for the scene, overriding the one set in the scene file. The available filters

are box, gaussian, mitchell, triangle, catmull-rom, blackman-harris, sinc, lanczos, and bspline (in the 0.07.3

SVN). See the filter section of the image settings for more info.

-nogui used to render images without opening the GUI. Required for all images except png files.

-o "filename" : used to render images out to a file type.

-bucket X Y : A larger bucket size means more RAM usage and less time rendering. Usually, a bucket size

64 is a good default - especially if you are using a wide pixel filter like gaussian or mitchell. There are six

```text
bucket order types available: hilbert (default), spiral, column, row, diagonal, and random. You can also use
```

these ordered in reverse by adding "reverse" in front of the name. To use reverse, you&#39;ll need to use quotes

around the reverse order (e.g. bucket 48 "reverse spiral") so that the bucket order is still parsed as one

token. The number in the line is the pixel size of the each bucket. There is some clamping done internally to

make sure the bucket size isn&#39;t too small or too big.

-threads X : For forcing a certain number of threads. Sunflow automagically detects if you have a multi-

threaded system so if you do, you should see the number of squares in the Sunflow window that you have

threads during rendering. However, there was a report that if you force more threads you *might* see a

performance gain - though in theory, you shouldn&#39;t.

-smallmesh : Load triangle meshes using triangles optimized for memory use.

-nogi : Turns off all global illumination in the scene.

-nocaustics : Turns off any caustics in the scene.

-ipr : Used to render a scene to the GUI as IPR rather than a final render.

-anim for animating with Sunflow.

-bake <object_name>.instance, -bakedir ortho (for diffuse maps), and -bakedir view (for

reflections/specular maps). See lightmap baking for more info.

-sampler type : Render using the specified algorithm.

-lopri : Set thread priority to low (default).

-hipri : Set thread priority to high.

-dumpkd : Dump KDTree to an obj file for visualization.

-buildonly : Do not call render method after loading the scene.

-showaa : Display sampling levels per pixel for bucket renderer.

-pathgi X : Use path tracing with n samples to render global illumination.

-bench : Run several built-in scenes for benchmark purposes.

-rtbench : Run realtime ray-tracing benchmark (cannot be used with -nogui)

-frame X : Set frame number to the specified value.

-X verbosity : Set the verbosity level: 0=none,1=errors,2=warnings,3=info,4=detailed.

-h or -help : Prints all commands.

### Command Line Shaders

-quick_uvs : Renders the UVs of objects.

-quick_normals : Renders the normals of objects.

-quick_id : Renders using a unique color for each instance.

-quick_prims : Renders using a unique color for each primitive.

-quick_gray : Renders the all the objects in the scene gray diffuse, overriding all shaders - great for lighting

tests.

-quick_wire -aa MIN MAX -filter X : Renders objects as wireframe. You set the aa and filter to be used

here because without it, the wire doesn&#39;t look great.

-quick_ambocc X : Renders a scene in ambient occlusion mode with a certain distance (X).

Only one command line shader at a time can be used in the command line.

### For SVN 0.07.3 only

-translate

Translate the old file format of the scene file to the new file format in 0.07.3 in either ascii or binary format:

...sunflow.bat -translate new_scene.sca my_old_scene.sc

Where .sca is the file extension for ascii scenes, and .scb is for binary scenes (not human readable unless you have

a hexadecimal editor handy.