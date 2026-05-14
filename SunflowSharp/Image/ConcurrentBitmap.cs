using System;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;

namespace SunflowSharp.Image
{
    /// <summary>
    /// A thread-safe image buffer designed for concurrent writes from multiple threads.
    /// Uses high-precision float components for R, G, and B.
    /// </summary>
    public class ConcurrentBitmap
    {
        private readonly float[] data;
        private readonly int width;
        private readonly int height;

        public ConcurrentBitmap(int width, int height)
        {
            this.width = width;
            this.height = height;
            // Interleaved R, G, B
            data = new float[width * height * 3];
        }

        public int Width => width;
        public int Height => height;

        /// <summary>
        /// Atomically adds a color to the pixel at (x, y).
        /// Useful for Monte Carlo integration where multiple samples are added to the same pixel.
        /// </summary>
        public void Accumulate(int x, int y, Color c)
        {
            if (x < 0 || x >= width || y < 0 || y >= height) return;
            int offset = (y * width + x) * 3;

            AtomicAdd(ref data[offset + 0], c.r);
            AtomicAdd(ref data[offset + 1], c.g);
            AtomicAdd(ref data[offset + 2], c.b);
        }

        /// <summary>
        /// Sets the pixel at (x, y) to a specific color. Not atomic relative to other writes to the same pixel.
        /// </summary>
        public void SetPixel(int x, int y, Color c)
        {
            if (x < 0 || x >= width || y < 0 || y >= height) return;
            int offset = (y * width + x) * 3;
            data[offset + 0] = c.r;
            data[offset + 1] = c.g;
            data[offset + 2] = c.b;
        }

        private static void AtomicAdd(ref float location, float value)
        {
            float newV, oldV, initialV;
            do
            {
                initialV = location;
                newV = initialV + value;
                oldV = Interlocked.CompareExchange(ref location, newV, initialV);
            } while (initialV != oldV);
        }

        /// <summary>
        /// Scales all pixels in the buffer by a constant factor.
        /// </summary>
        public void Scale(float factor)
        {
            for (int i = 0; i < data.Length; i++)
                data[i] *= factor;
        }

        /// <summary>
        /// Resolves the concurrent buffer into a standard SunflowSharp Bitmap.
        /// </summary>
        public Bitmap ToSunflowBitmap(bool isHDR)
        {
            Bitmap result = new Bitmap(width, height, isHDR);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int offset = (y * width + x) * 3;
                    result.setPixel(x, y, new Color(data[offset + 0], data[offset + 1], data[offset + 2]));
                }
            }
            return result;
        }

        /// <summary>
        /// Resolves the concurrent buffer into a System.Drawing.Bitmap.
        /// Applies simple gamma correction (sRGB).
        /// </summary>
        public System.Drawing.Bitmap ToSystemBitmap()
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(width, height, PixelFormat.Format32bppArgb);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0;
                for (int y = 0; y < height; y++)
                {
                    // Systems.Drawing uses (0,0) as top-left. Sunflow typically uses bottom-left for y=0.
                    // We'll follow Sunflow's convention and flip here for system display.
                    int targetY = height - 1 - y;
                    byte* row = ptr + (targetY * bmpData.Stride);
                    
                    for (int x = 0; x < width; x++)
                    {
                        int srcOffset = (y * width + x) * 3;
                        
                        // Convert to sRGB and clamp
                        byte r = (byte)Math.Clamp(RGBSpace.SRGB.gammaCorrect(data[srcOffset + 0]) * 255f, 0, 255);
                        byte g = (byte)Math.Clamp(RGBSpace.SRGB.gammaCorrect(data[srcOffset + 1]) * 255f, 0, 255);
                        byte b = (byte)Math.Clamp(RGBSpace.SRGB.gammaCorrect(data[srcOffset + 2]) * 255f, 0, 255);

                        row[x * 4 + 0] = b;
                        row[x * 4 + 1] = g;
                        row[x * 4 + 2] = r;
                        row[x * 4 + 3] = 255; // Alpha
                    }
                }
            }

            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}
