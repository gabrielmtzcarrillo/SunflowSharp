# Exporters

From Sunflow Wiki

## Exporters

If any of the external links go down please contact me through my site

(http://www.geneome.net/index.php/contact/) and I can link to my copies of them. I would rather not do that now

since the sites usually have the howto&#39;s and other interesting info. Be aware that some of these script may be old

and not work with current versions of the designated application. If you have updates or you know for certain the

script doesn&#39;t work with certain versions of the application, please tell me and I&#39;ll note it here.

Blender Exporter

3DS Max Exporter

Maya Exporter

SketchUp Exporter

Lightwave (http://www.kevinmacphail.com/6.html)

Cinema4D (.zip) (http://www.geneome.net/other/otherfiles/sfexporters/C4DSunflowExporterSuite.zip)

XSI (http://migaweb.de/downloads.php?id=2)

Houdini 8.1 (http://forums.odforce.net/index.php?showtopic=5527)

Houdini 9.1 (bottom of page) - Not working, and looking for help to get it working

(http://forums.odforce.net/index.php?showtopic=5527&st=12)

# Exporters/Blender

From Sunflow Wiki

< Exporters

This page deals with the script in the SVN. You can find the latest SVN script here

(http://sunflow.svn.sourceforge.net/viewvc/sunflow/trunk/exporters/blender/sunflow_export.py?view=log) .

To install the script in Blender:

1) Make sure you have Python 2.5 installed (2.3 for Mac) for Blender 2.44.

2) Download the script (the download button above the most recent SVN addition). I suggest right clicking

on the button and saving the file that way since some browsers open the script up in their own window.

3) Save the script to %your Blender folder%/.blend/scripts/.

3a) If you have Blender open when you do this, you can go to the user preferences panel > file paths button

and click the gear-looking button next to the Python entry (there doesn&#39;t have to be anything in the path) to

re-evaluate your script folder.

4) In Blender, go to the Scripts panel > Export, and look for the Sunflow Exporter. Click it to intialize it.

[/list] To start, I will say this: Sunflow is incredibly powerful in the right hands. The Blender exporter script

tries its best to help unlock Sunflow, but there are still several aspects of Sunflow that either Blender can&#39;t do

or aren&#39;t in the script yet (lightmap baking, camera motion blur, etc.). So my suggestion, to really know

Sunflow, is study the scene files and get to know what Sunflow can really do.

Okay, on to the show.

The Blender export script is constantly evolving, but I thought I would introduce the nuances of the script since

there are no official instructions. The script has two types of values that are exported from Blender, those that are in

Blender&#39;s interface and those that are exported from values you define in the script itself. Since the latter are

obvious (they are in the script), I thought I would mention what values can be Blender derived.

## Contents

1 HUGE Thing To Know

2 HUGE Thing To Know 2

3 Blender&#39;s Command Window

4 Layers

5 Scene Properties

6 Shader Properties (See the Shader Section of the Script for More Information)

7 World Properties

8 Light Properties

9 Textures

10 Camera Properties

11 Objects

12 Instancing/Dupligroups

13 Hair Objects

14 Particle Objects - FOR SVN 0.07.3 ONLY

15 Sunflow Command Line in the Blender Export Script

16 Regarding the "Configure SF" panel

17 Saving script settings to the .blend file

## HUGE Thing To Know

Every object in your scene needs to have a material set in Blender (any material, not necessarily a Sunflow specific

one) or you&#39;ll get a Nonetype object error. This is because the script looks for the material&#39;s name to define a

Sunflow shader, but if the object doesn&#39;t have a material set, there isn&#39;t a name for the script to look at, so the

export fails.

## HUGE Thing To Know 2

While working in Blender and at the same time have the Sunflow Blender Exporter Python Script running, it

"crashes" the script if you make a "undo (ctrl+z)" in Blender.

## Blender&#39;s Command Window

It&#39;s important to remember that the script will print to the Blender command window. It will tell you if an export has

been made successful (or if it&#39;s still in the process of exporting), how it&#39;s loading the Sunflow render window, etc.

Since Blender&#39;s command window is usually behind Blender we miss these messages, so if the script seems to have

frozen Blender, be sure to check the command window to see what&#39;s going on.

## Layers

Firstly, The exporter will only export objects on active layers.

## Scene Properties

1) Scene > Animation panel: The start and end frame are used to tell the script how many files the “Export As

Animation” exports out.

2) Scene > Format panel: The image size (e.g. 800 x 600) is used to define your image size in Sunflow.

3) Scene > Render panel: The image size percentage buttons can be used to reduce the “real” image size, just like

in Blender.

## Shader Properties (See the Shader Section of the Script for More

## Information)

4) Shading > Material Buttons > Material panel: The “Col” and “Spe” RGB values are used for a variety of

shaders.

5) Shading > Material Buttons > Mirror Transp panel: The RayMir value (when the Ray Mirror is pressed) is used

in Sunflow&#39;s mirror shader and the IOR value (when the Ray Transp button is pressed) is used in Sunflow&#39;s glass

shader. The RayMir value is also used for the shiny shader, but you don&#39;t need the button pressed. Side note: The

SF glass shader has two absorption values that are hard coded in the script, I would like to use the limits value in

Blender for one of them, however it isn&#39;t a part of the Blender Python API.

6) The script understands material indices. So you can assign different materials to different faces of an object and it

will be understood by Sunflow.

## World Properties

7) Shading > World Buttons > World panel: Unless you specify your own background in the script, the Horizon

RGB values will be used as the background color in Sunflow.

## Light Properties

8 ) Shading > lamp buttons > Preview panel/Lamp panel: Sunflow supports 4 light types that Blender has... sort of:

The Point lamp, Sun, Area (aka meshlight), and Spot. Spot is the sort of. If you have spots in your Blender scene

they are replaced by directional (cylindrical) lights in Sunflow. You&#39;ll need to adjust the distance (in the spot&#39;s lamp

panel) as close as possible to the ground receiving the cone of light if you want radius as close as possible. Sun is

another unique one since Sunflow&#39;s sun has turbidity. So in the background panel you&#39;ll find a button that says

"mporti sun." With this pressed and a sun in the scene, you&#39;ll be able to export out a sun where you can select the

```text
samples and turbidity settings. Also you can export a Sunflow spherical lamp from Blender by using a hemi-lamp
```

position as the lamps center, and the distance as the radius.

9) Shading > Lamp buttons > Lamp panel: You can specify the size of area lamps (square or rectangular) , the

```text
color of any lamp&#39;s RGB values, and any lamp&#39;s energy. Though for energy (which is by default 1.0), you can use
```

the script to multiply the value since in Sunflow, the Blender values always show up dark.

10a) Meshlights (thanks to Heavily Tessellated for pointing this out in another thread): If you are looking to make a

mesh a light source, begin the name of the object with "meshlight," and it will be interpreted as a meshlight. The

meshes material settings will be ignored except for the diffuse color (which will be the light&#39;s color). You can control

the meshlight power on the Light tab of the exporter. The important thing to understand is that the light samples are

per face. So if, in HT&#39;s example, you were thinking of making a florescent light tube and you used a 16-sided

cylinder for it in Blender, that works out to: 16 quads become 32 side face triangles, +16 end tris, +16 other end

tris = 64 individual light sources, times whatever sample value you chose... say 16 (the exporter default) would

make it 1024 light samples. That&#39;s pretty intense, but in the scheme of things it&#39;s not so bad.

10b) If an image texture name is "bllight"i (must be lowercase), then that image will be used as an image based light.

It doesn&#39;t need to be applied to an object or a textured shader to work, so I would suggest adding the texture to

the World textures to indicate that it&#39;s on its own. The image based light is infinitely far away, so if you have a

```text
camera in a closed box, then the ibl won&#39;t show through and the image will render without a light. The script exports
```

it with general center and up vactors, so you&#39;ll need to play with the settings to get the placement exactly the way

you want it.

## Textures

11) Shading > Texture buttons > Texture panel: This information is in the shader info panel of the script but I&#39;ll

mention it here. If an image is in the first texture slot, it will be used in the diffuse channel of the shader (if the shader

supports textures). If an image is in the second texture slot, it will be used as either a normal or bump map for the

shader. You specify which by beginning the name texture with either “normal” or bump.” The scale of the effect of

the bump or normal map is dictated by having the Nor button of the Map To panel pressed for that texture and

using the Nor slider to control the value. If there is a non-image texture in slot 4 and you begin the name with

"perlin," the perlin modifier (in the SVN for 0.07.3) will be used. The Nor slider controls the scale, but the function

and size are currently hard coded. If an image is in the third texture slot, it will be used in the specular channel of the

```text
shader (if the shader supports textures). Remember, image textures need to be UV mapped.
```

12) Any texture panel: If the image texture name is "bllight"i (must be lowercase), then that image will be used as an

image based light.

## Camera Properties

13) Editing > Camera panel: The lens value is used to determine the fov in Sunflow and the DOF distance value is

used in the exporter as, you guessed it, the DOF distance. COMING SOON: In Sunflow&#39;s SVN there is support

for camera shifting. If you&#39;re using an SVN Sunflow, the script has the shift values taken from Blender&#39;s script value

for the pinhole camera, but it&#39;s commented out. If you want to use it, just un-comment that line.

## Objects

14) The key objects Blender that can be exported are NURBS surfaces and mesh objects. NURBS surfaces

render out a bit blocky, so I suggest converting to a mesh (ALT+C in Blender), then subsurfing. The exporter will

take the Render Level of the subsurf modifier as well as the set smooth result (if used) for mesh objects. UV

information (if any) is also taken from objects in Blender and is the key method of mapping with an image.

## Instancing/Dupligroups

The script will understand if an object is dupligrouped to an empty. The script will then create a Sunflow instance of

that object in the empty&#39;s location using the empty&#39;s rotation and scale. To dupligroup an object: Select your main

object, the CTRL+G to add to a new group. You can change the group name in the object panel Once that&#39;s done,

use Spacebar>Add>Group>GroupName. This will add an instanced object on an empty. This empty&#39;s location,

rotatation, and scale will be used as the instance&#39;s location, rotation, and scale in the exported Sunflow scene file.

It&#39;s important to note that in Blender, the instances will all use the same material that the main object has. In Sunflow

you can change the shader of the instances, or do different groups in Blender. The script assumes that the object

and mesh names are the same, so if your instances aren&#39;t exporting, this might be the reason.

## Hair Objects

Sunflow has a hair object type so I got Blender hair particle data to be able to be exported out to Sunflow. Just

create a static particle system with "vect" enabled, set the "step" value to 1, export, and there you have it! The hair

width is currently hard coded to a set value since the Blender Python API doesn&#39;t have access to the strand button.

The Sunflow SVN can deal with changing widths over the legth of the hair, but I havent&#39;s added that ability to the

script for the same reason. I&#39;ve found that hair usually needs a high AA (like 0,3) with a gaussian filter to look nice.

## Particle Objects - FOR SVN 0.07.3 ONLY

If you happen to be using an SVN version of Sunlfow you can export particle data, or the vertices of objects

whose name begins with "particleob" as a Sunflow particle object. Like hair, the radius of the particles is hard

coded. You might be thinking that 0.07.2 has particle objects so why is 0.07.3 needed. Answer: Only 0.07.3

allows the point data to be stored in the .sc file, whereas 0.07.2 requires an external binary .dat file.

Here is some more info regarding the script:

## Sunflow Command Line in the Blender Export Script

olivS added the ability to use execute Sunflow command line functions through the exporter. All of these functions

can be found in the Render Settings button and are enacted by ==first exporting the .sc file==, adding the flags you

want (though you don&#39;t need to add any flags), then hit the “Render exported” button. It will render your image out

to the location of the .sc file (with any flags you might have selected). I recently added on to this the ability to render

out an animation sequence (after first being exported as .sc files). Just keep the “Export As Animation” button

turned on and then hit the “Render exported” button. It will then render your sequence out in the same location the

.sc files are in. You can also use the IPR button pressed to get a quick look at the progressive render. You can hit

"esc" to stop the render.

## Regarding the "Configure SF" panel

This panel has several settings that are necessary for the command line functions of the script to work (mentioned

above). The settings are saved in a config file in your Blender folders .blend/scripts/bpydata/ folder as path2sf.cfg

which you edit or delete. Since these settings are for use with the script&#39;s command line, you don&#39;t need to add/save

these settings if all you are going to do is export a .sc file. IMPORTANT: If you want to use the render button

(mentioned above in the command line paragraph) in the script to load Sunflow from the script, you MUST set your

Sunflow and Java paths. The paths would be the folders in which Sunflow and Java reside (e.g. C:\Sunflow\ and

C:\Java\jdk1.6.0_01\bin\). Also note that the two paths can&#39;t have white spaces (e.g. "Program files") - that&#39;s just

the way Blender is when using its command line to run things.

==Samples in the AA panel== (from Chris) The samples value affects DOF and motion blur together. They are in

the image block and not in the camera as they are properties of the image sampler, not the camera itself. What it

does is super-sample the lens/time areas. It can have an effect on the quality of other elements (like shadows or GI)

but only indirectly as a result of those (sub) pixels being sampled more often.

## Saving script settings to the .blend file

There are enough options in the exporter that if you wanted to save the same buttons pressed and values changed

for a specific scene, you would need to start writing it down somewhere. Enter ID properties. In the config panel of

the script you can send the settings you have set to the .blend file so when you save the .blend, these settings will be

saved with it. When you open the script again, those values will be auto imported back to the script.

# Exporters/3DSMax

From Sunflow Wiki

< Exporters

The most recent version of the Max to Sunflow exporter is v0.24. It can be downloaded here

(http://www.anidesign.de/index_sc.htm) .

In its current state, the script can export the following...

## Contents

1 General Settings

2 Cameras

3 Lights

4 Geometry

5 Textures

6 Shaders

6.1 Diffuse

6.2 Constant

6.3 Glass

6.4 Mirror

6.5 Phong

6.6 Ward

6.7 Shiny

6.8 Uber

6.9 Amb-Occ

6.10 IMPORTANT

7 Other Stuff

8 Installation

## General Settings

- Antialias settings

- Sample settings

- Filter settings

- Bucket settings

- GI settings

- Photon settings

- Background colours (if the background in Max is black, then it is ignored)

## Cameras

- Both free and target cameras can be exported

- If you enable DOF, you can use the cameras &#39;Far Range&#39; to focus

- Also with DOF, if the &#39;Lens Sides&#39; setting is less than 3, a circular lens will be used

## Lights

- Point (omni) and directional lights (free / target spotlights / directlights)

- IES_Sun, mr_Sun and Daylight systems

- IBL (image-based lighting) is automatic if an HDR or EXR image is used as an environment map (and is turned

on). To get the same view of your HDR in SunFlow as in Max, you will need to set the U Tiling value to -1

- Meshlights: the object MUST have a name beginning with "meshlight", and a standard material with an output map

in the diffuse channel. The diffuse color is multiplied by the output amount to get the radiance amount.

## Geometry

- Primitives: sphere, torus, teapot, plane (planes with a render-scale of more than 1 get converted to an infinite

ground-plane)

- Standard particle systems and PFlow systems

- Hair and Fur modifiers: the width setting is taken from the "Root Thick" setting in the modifier, the amount from

the "Hair Count" setting, and the shader just uses the the "Root Color" setting to create a diffuse shader

- Everything else gets turned into a generic mesh (including UVs and Material IDs)

- Object instances get exported as SunFlow instances

## Textures

- Bitmaps in the Diffuse map channel get exported as part of the shader

- Bitmaps in the Bump map channel get exported as bump modifiers. Uses the the &#39;Bump Amount&#39; value from the

maps&#39; Output settings.

## Shaders

### Diffuse

- All objects with no material assigned get a diffuse shader with the object colour

- All objects with a material type not covered by the other shaders get a diffuse shader with the object colour

- Standard and raytrace materials with shader type OrenNayerBlinn get exported as a diffuse shader using the

```text
diffuse colour
```

### Constant

- Any materials with a self-illumination value of more than 0 get exported as a constant shader

- Any materials with a self-illumination / luminosity colour more than 0,0,0 get exported as a constant shader

### Glass

- Any materials with an opacity of less than 100 are exported as a glass shader using the diffuse colour and IOR set

in Max

### Mirror

- Raytrace materials with a reflection colour more than 0,0,0 get exported as a mirror shader using the reflect

```text
colour as the mirror colour
```

- Raytrace materials with a reflection amount of more than 0 get exported as a mirror shader using the reflect

```text
amount to set a greyscale for the mirror colour
```

### Phong

- Standard and raytrace materials with shader type Blinn or Phong get exported as a phong shader using the diffuse

and specular colours in Max

- The blur strength is calculated using the Soften setting in Max x 1000

### Ward

- Standard and raytrace materials with shader type Anisotropic or Multi-Layer get exported as a ward shader using

the diffuse and specular colours in Max.

- X and Y roughness is calculated using the glossiness setting (0 = very blurry, 100 = sharp) and the anisotropy

setting (0 = X and Y equal, 50 = Y half the width of X, etc.)

### Shiny

- Standard and raytrace materials with shader type Metal or Strauss get exported as a shiny shader using the diffuse

colour

- Shininess is set using the glossiness amount

### Uber

- any Shellac material exports the Base Material as an Uber shader

- diffuse color and texture are set using the Diffuse color and map slot in Max

- specular color and texture are set using the Specular color and map slot in Max

- diffuse and specular blend amounts are set using the Map Amount settings for the Diffuse and Specular maps

```text
divided by 100
```

- samples are set using the Specular Level amount in Max

- glossiness is set using the Soften amount in Max x 100 (I couldn&#39;t use the Glossiness setting in Max because it

only handles integers)

### Amb-Occ

- any Standard material with a Falloff map in the Diffuse slot exports as an amb-occ shader

- the Falloff front colour is used for the SunFlow bright colour

- the Falloff back colour is used for the SunFlow dark colour

- at the moment, the "samples" and "dist" values are fixed at 32 and 10. If I can think of a good way of setting them

in Max, I&#39;ll change this

### IMPORTANT

Shaders always get assigned in the following order:

1. Any standard material with a falloff map in the diffuse slot gets an amb-occ shader

2. Any material with self-illumination gets a constant shader

3. Any material with transparency gets a glass shader

4. Any raytrace material with reflection gets a mirror shader

5. After that the different shader types get assigned according to the material types listed above

## Other Stuff

- Support for single frames and animations: uses the Render Scene Dialog to get the image size and still / animation /

```text
range settings
```

- Animations get written to multiple files (test0000.sc, test0001.sc, etc.)

- Particle systems get written to an extra .bin file which has the same filename as the .sc file.

- Particle systems get written to an extra *.part.sc file which has the same filename as the .sc file

- If the &#39;Separate geometry file&#39; checkbox is checked, all geometry is exported to an extra file with the ending

*.geo.sc

- The &#39;Skip&#39; checkbox (only active if the &#39;Separate geometry file&#39; is checked) is useful if you&#39;ve already exported

everything once, and just need to tweak materials and render settings. When this is on, the exporter will export

everything except the geometry.

- If the &#39;Start Sunflow after exporting&#39; checkbox is checked, the exported scene is automatically rendered (the first

time you do this you need to set the paths to Sunflow and Java by clicking on the &#39;Set Paths&#39; button)

## Installation

The main script (max2sunflow024.mse) goes in the main Scripts folder (usually C:\Program Files\Autodesk\3ds

Max 9\Scripts)

The macroscript (max2sunflow.mcr) goes in the macroscripts folder: C:\Program Files\Autodesk\3ds Max

9\ui\macroscripts

The 4 icons (*.bmp) go in the icons folder: C:\Program Files\Autodesk\3ds Max 9\ui\Icons

Now start Max, go to Customize -> Customize User Interface... and add the macroscript to a toolbar or menu of

your choice (I always use the &#39;Extras&#39; toolbar for my bits and pieces)

If you are going to be exporting a lot of particles, or very heavy scenes, it night be an idea to increase the memory

allocation for MAXScript. You can do this by going into the &#39;Customize -> Preferences... -> MAXScript&#39; settings,

and increasing the &#39;Initial heap allocation&#39; setting. I&#39;ve got mine set to 64MB.

# Exporters/Maya

From Sunflow Wiki

< Exporters

The exporter source files are in the SVN, however the compiled version has disappeared, so if you have the Maya

API and have a build environment, please help us out and build it for us!.

# Exporters/SketchUp

From Sunflow Wiki

< Exporters

You can find the SketchUp exporter in the SVN

(http://sunflow.svn.sourceforge.net/viewvc/sunflow/trunk/exporters/sketchup/) , however the user manual can be

found bundled with it on Didier Bur&#39;s site (http://www.crai.archi.fr/RubyLibraryDepot/Ruby/em_fil_page.htm) in the

su2sf_11.zip file. For those that have the plugin already installed I&#39;ll point to the manual here (.pdf)

(http://www.geneome.net/other/otherfiles/sfexporters/su2sf_user_guide.pdf) .

## Mac Installation

Place the su2sf.rb inside your home folder&#39;s Library/Application Support/Google SketchUp 6/PlugIns/. You may

need to create the PlugIns directory there if it doesn&#39;t exist already.

Launch SketchUp and you&#39;ll see there is a PlugIns menu item which contains the Sunflow Exporter.