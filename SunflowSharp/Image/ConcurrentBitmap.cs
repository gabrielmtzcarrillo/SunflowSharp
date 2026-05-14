using System;
using System.Threading;

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
    }
}
