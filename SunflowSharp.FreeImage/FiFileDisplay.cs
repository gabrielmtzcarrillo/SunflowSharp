using System;
using System.Drawing;
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
            
            using (Bitmap b = concurrentBitmap.ToSystemBitmap())
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    b.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                    stream.Seek(0, SeekOrigin.Begin);
                    FIBITMAP fi = FreeImageAPI.FreeImage.LoadFromStream(stream);
                    //fixme: switch based on extension
                    FreeImageAPI.FreeImage.Save(FREE_IMAGE_FORMAT.FIF_PNG, fi, filename, FREE_IMAGE_SAVE_FLAGS.DEFAULT);
                    FreeImageAPI.FreeImage.Unload(fi);
                }
            }
        }
    }
}

