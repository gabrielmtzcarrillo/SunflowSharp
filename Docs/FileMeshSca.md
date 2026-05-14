# FileMesh sca

From Sunflow Wiki

## Contents

1 File-Meshes

2 Transforms

3 Object Motion Blur

## File-Meshes

You can import an obj, stl, or ra3 file automatically into Sunflow without having to convert it to Sunflow&#39;s file forma:

```text
geometry objectName {
type file_mesh
param filename string test.obj
```

param smooth_normals bool true

```text
}
instance objectName.instance {
```

param geometry string objectName

param shaders string[] 1

myShader

```text
}
```

The filename can also be an absolute path.

The smooth_normals line is optional, it defaults to false (which is best if you have millions of triangles. Note that the

stl format does not store mesh connectivity so it isn&#39;t possible to smooth their normals.

Sunflow doesn&#39;t accept an obj file if there are texture coordinates and normals stored in it. For example, a line like f

1/1/1 2/2/2/ 3/3/3 in the obj file doesn&#39;t work. Certain exporters will allow you to not have this information

exported to the file. This limitation can be overcome using NeuroWorld&#39;s obj to .sc converter

(http://sunflow.sourceforge.net/phpbb2/viewtopic.php?t=356) .

## Transforms

It&#39;s important to note that objects (including file meshes) can also be transformed. The transform line goes under the

```text
shader declaration (or geometry declaration if it is an instance) in the instance syntax:
geometry objectName {
type file_mesh
param filename string test.obj
```

param smooth_normals bool true

```text
}
instance objectName.instance {
```

param geometry string objectName

param shaders string[] 1

myShader

```text
param transform matrix row 0.707 0.0 0.707 0.0 -0.579 0.574 0.579 0.0 -0.406 -0.819 0.406 0.0 0.0 0.0 0.0 1.0
}
```

# Object Motion Blur

## As with generic meshes, file meshes can be object blurred.

![Image from page 123](images/page123_img1.png)

![Image from page 123](images/page123_img2.png)

![Image from page 123](images/page123_img3.png)

![Image from page 123](images/page123_img4.png)

![Image from page 123](images/page123_img5.png)

![Image from page 123](images/page123_img6.png)