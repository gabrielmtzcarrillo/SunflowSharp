using ILGPU;
using ILGPU.Runtime;
using ILGPU.Algorithms;
using System;
using SunflowSharp.Maths;

namespace SunflowSharp.Core.Gpu
{
    public struct GpuRay
    {
        public float ox, oy, oz;
        public float dx, dy, dz;
        public float tMin, tMax;

        public GpuRay(float ox, float oy, float oz, float dx, float dy, float dz, float tMin, float tMax)
        {
            this.ox = ox;
            this.oy = oy;
            this.oz = oz;
            this.dx = dx;
            this.dy = dy;
            this.dz = dz;
            this.tMin = tMin;
            this.tMax = tMax;
        }
    }

    public struct GpuIntersection
    {
        public int primID;
        public float u, v, t;

        public bool Hit => primID >= 0;

        public void Reset()
        {
            primID = -1;
            t = float.MaxValue;
        }
    }

    public struct GpuTriangle
    {
        public float v0x, v0y, v0z;
        public float e1x, e1y, e1z;
        public float e2x, e2y, e2z;
        public float nx, ny, nz;

        public GpuTriangle(Point3 v0, Point3 v1, Point3 v2)
        {
            v0x = v0.x; v0y = v0.y; v0z = v0.z;
            e1x = v1.x - v0.x; e1y = v1.y - v0.y; e1z = v1.z - v0.z;
            e2x = v2.x - v0.x; e2y = v2.y - v0.y; e2z = v2.z - v0.z;
            
            // Geometric normal
            float nx_raw = e1y * e2z - e1z * e2y;
            float ny_raw = e1z * e2x - e1x * e2z;
            float nz_raw = e1x * e2y - e1y * e2x;
            float lenSq = nx_raw * nx_raw + ny_raw * ny_raw + nz_raw * nz_raw;
            if (lenSq > 0)
            {
                float invLen = 1.0f / XMath.Sqrt(lenSq);
                nx = nx_raw * invLen;
                ny = ny_raw * invLen;
                nz = nz_raw * invLen;
            }
            else
            {
                nx = ny = nz = 0;
            }
        }
    }

    public static class Kernels
    {
        // Simple Kensler triangle intersection
        public static void IntersectTriangle(
            GpuRay ray,
            GpuTriangle tri,
            ref GpuIntersection isect,
            int primID)
        {
            float edge0x = tri.e1x;
            float edge0y = tri.e1y;
            float edge0z = tri.e1z;
            float edge1x = -tri.e2x;
            float edge1y = -tri.e2y;
            float edge1z = -tri.e2z;

            float nx = edge0y * edge1z - edge0z * edge1y;
            float ny = edge0z * edge1x - edge0x * edge1z;
            float nz = edge0x * edge1y - edge0y * edge1x;

            float v = ray.dx * nx + ray.dy * ny + ray.dz * nz;
            if (XMath.Abs(v) < 1e-10f) return;
            float iv = 1.0f / v;

            float edge2x = tri.v0x - ray.ox;
            float edge2y = tri.v0y - ray.oy;
            float edge2z = tri.v0z - ray.oz;

            float va = nx * edge2x + ny * edge2y + nz * edge2z;
            float t = iv * va;

            if (t < ray.tMin || t > ray.tMax || t > isect.t)
                return;

            float ix = edge2y * ray.dz - edge2z * ray.dy;
            float iy = edge2z * ray.dx - edge2x * ray.dz;
            float iz = edge2x * ray.dy - edge2y * ray.dx;

            float v1 = ix * edge1x + iy * edge1y + iz * edge1z;
            float beta = iv * v1;
            if (beta < 0) return;

            float v2 = ix * edge0x + iy * edge0y + iz * edge0z;
            if ((v1 + v2) * (v > 0 ? v : -v) > v * v) return;

            float gamma = iv * v2;
            if (gamma < 0) return;

            isect.t = t;
            isect.u = beta;
            isect.v = gamma;
            isect.primID = primID;
        }

        public static void RayGenKernel(
            Index2D index,
            ArrayView2D<GpuIntersection, Stride2D.DenseX> output,
            int width,
            int height,
            GpuRay baseRay,
            ArrayView<GpuTriangle> triangles,
            ArrayView<int> tree,
            ArrayView<int> objects)
        {
            GpuIntersection isect;
            isect.primID = -1;
            isect.t = float.MaxValue;
            isect.u = 0;
            isect.v = 0;

            // Simple ray setup (orthographic for test)
            GpuRay ray = baseRay;
            ray.ox += (float)index.X / width;
            ray.oy += (float)index.Y / height;

            for (int i = 0; i < triangles.IntLength; i++)
            {
                IntersectTriangle(ray, triangles[i], ref isect, i);
            }

            output[index] = isect;
        }
    }
}
