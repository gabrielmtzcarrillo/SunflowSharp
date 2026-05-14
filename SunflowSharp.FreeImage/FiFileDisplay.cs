using System;
using SunflowSharp.Core;
using SunflowSharp.Core.Display;
using FreeImageAPI;
using System.IO;

namespace SunflowSharp.FreeImage
{
    public class FiFileDisplay : FileDisplay
    {
        public FiFileDisplay(string filename)
            : base(filename)
        {
        }

        public override void imageEnd()
        {
            if (filename == null || concurrentBitmap == null) return;

            int width = concurrentBitmap.Width;
            int height = concurrentBitmap.Height;
            SunflowSharp.Image.Bitmap bmp = concurrentBitmap.ToSunflowBitmap(false);

            FIBITMAP fi = FreeImageAPI.FreeImage.Allocate(width, height, 32, 0, 0, 0);
            if (fi.IsNull) return;

            for (int y = 0; y < height; y++)
            {
                // FreeImage uses bottom-to-top by default for some formats, 
                // but let's check the SetPixel usage.
                for (int x = 0; x < width; x++)
                {
                    SunflowSharp.Image.Color c = bmp.getPixel(x, y);
                    // FreeImage SetPixelColor uses RGBQUAD
                    RGBQUAD color = new RGBQUAD();
                    color.rgbRed = (byte)Math.Clamp(c.r * 255f, 0, 255);
                    color.rgbGreen = (byte)Math.Clamp(c.g * 255f, 0, 255);
                    color.rgbBlue = (byte)Math.Clamp(c.b * 255f, 0, 255);
                    color.rgbReserved = 255;
                    FreeImageAPI.FreeImage.SetPixelColor(fi, (uint)x, (uint)y, ref color);
                }
            }

            FREE_IMAGE_FORMAT format = FREE_IMAGE_FORMAT.FIF_PNG;
            if (filename.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || filename.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                format = FREE_IMAGE_FORMAT.FIF_JPEG;
            else if (filename.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
                format = FREE_IMAGE_FORMAT.FIF_BMP;
            else if (filename.EndsWith(".tga", StringComparison.OrdinalIgnoreCase))
                format = FREE_IMAGE_FORMAT.FIF_TARGA;

            FreeImageAPI.FreeImage.Save(format, fi, filename, FREE_IMAGE_SAVE_FLAGS.DEFAULT);
            FreeImageAPI.FreeImage.Unload(fi);
        }
    }
}

