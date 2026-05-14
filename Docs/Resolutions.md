# Getting Resolutions Greater Than 16K

From Sunflow Wiki

The code (up to 0.7.3) limits resolution to 16k. If you&#39;re looking to create an image with resolution dimensions

greater than 16384 - no problem! You can recompile the source with a subtle change to a single file. In

\trunk\src\org\sunflow\core\Scene.java (starting at line 309 in 0.7.3) you will see this:

```text
// limit resolution to 16k
```

imageWidth = MathUtils.clamp(imageWidth, 1, 1 << 14);

imageHeight = MathUtils.clamp(imageHeight, 1, 1 << 14);

A quick bit shift change to the following and a recompile will open up resolutions beyond 16K:

```text
// do not limit resolution to 16k
```

imageWidth = MathUtils.clamp(imageWidth, 1, 1 << 15);

imageHeight = MathUtils.clamp(imageHeight, 1, 1 << 15);