# FileMesh

From Sunflow Wiki

## Contents

1 File-Meshes

2 Transforms

3 Object Motion Blur

## File-Meshes

You can import an obj, stl, or ra3 file automatically into Sunflow without having to convert it to Sunflow&#39;s file forma:

```text
object {
shader myShader
type file-mesh
name objectName
filename test.obj
```

smooth_normals true

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
shader declaration (or geometry declaration if it is an instance) in the object syntax. For transforms, you can use or
```

not use the following transform options:

```text
translate (translate x y z)
```

rotatex

rotatey

rotatex

scalex

scaley

scalez

scaleu

You could also use a transform matrix by row (row) or column (col). For example, here is one using some of the

above transforms:

```text
object {
shader myShader
transform {
translate 1.0 4.6 0.3
scaleu 0.5
}
type file-mesh
name objectName
```

filename myOBJ.obj

smooth_normals true

```text
}
```

And one using matrices:

```text
object {
name nameOfObject
shader shaderForObject
transform col my4x4MatrixReadByColumn
```

...

```text
}
```

## Object Motion Blur

As with generic meshes, in the 0.07.3 SVN version of Sunflow file meshes can be object blurred.