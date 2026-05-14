using System;
using SunflowSharp.Core;
using SunflowSharp.Image;

namespace SunflowSharp.Core.Display
{

    public class FileDisplay : IDisplay
    {
        protected ConcurrentBitmap? concurrentBitmap;
        protected string? filename;

        public FileDisplay(bool saveImage)
        {
            // a constructor that allows the image to not be saved
            // usefull for benchmarking purposes
            concurrentBitmap = null;
            filename = saveImage ? "output.png" : null;
        }

        public FileDisplay(string? filename)
        {
            concurrentBitmap = null;
            this.filename = filename == null ? "output.png" : filename;
        }

        public virtual void imageBegin(int w, int h, int bucketSize)
        {
            if (concurrentBitmap == null || concurrentBitmap.Width != w || concurrentBitmap.Height != h)
                concurrentBitmap = new ConcurrentBitmap(w, h);
        }

        public virtual void imagePrepare(int x, int y, int w, int h, int id)
        {
        }

        public virtual void imageUpdate(int x, int y, int w, int h, Color[] data)
        {
            if (concurrentBitmap == null) return;
            for (int j = 0, index = 0; j < h; j++)
                for (int i = 0; i < w; i++, index++)
                    concurrentBitmap.SetPixel(x + i, concurrentBitmap.Height - 1 - (y + j), data[index]);
        }

        public virtual void imageFill(int x, int y, int w, int h, Color c)
        {
            if (concurrentBitmap == null) return;
            Color cg = c;
            for (int j = 0; j < h; j++)
                for (int i = 0; i < w; i++)
                    concurrentBitmap.SetPixel(x + i, concurrentBitmap.Height - 1 - (y + j), cg);
        }

        public virtual void imageEnd()
        {
            if (filename != null && concurrentBitmap != null)
            {
                Bitmap bitmap = concurrentBitmap.ToSunflowBitmap(filename.EndsWith(".hdr"));
                bitmap.save(filename);
            }
        }
    }
}