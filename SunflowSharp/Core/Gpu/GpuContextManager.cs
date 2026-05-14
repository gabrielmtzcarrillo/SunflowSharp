using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.CPU;
using ILGPU.Runtime.Cuda;
using System;
using System.Collections.Generic;
using SunflowSharp.Core.Gpu;
using SunflowSharp.Maths;
using SunflowSharp.Systems;

namespace SunflowSharp.Core.Gpu
{
    public class GpuContextManager : IDisposable
    {
        private Context context;
        private Accelerator accelerator;
        private Action<Index2D, ArrayView2D<GpuIntersection, Stride2D.DenseX>, int, int, GpuRay, ArrayView<GpuTriangle>, ArrayView<int>, ArrayView<int>> rayGenKernel;

        public GpuContextManager()
        {
            // Fallback chain: CUDA -> CPU
            context = Context.Create(builder => builder.Cuda().CPU());
            
            Device? device = null;
            foreach (var d in context.Devices)
            {
                if (d.AcceleratorType == AcceleratorType.Cuda)
                {
                    device = d;
                    break;
                }
            }

            if (device == null)
            {
                foreach (var d in context.Devices)
                {
                    if (d.AcceleratorType == AcceleratorType.CPU)
                    {
                        device = d;
                        break;
                    }
                }
            }

            if (device == null)
                throw new InvalidOperationException("No suitable ILGPU device found.");

            accelerator = device.CreateAccelerator(context);

            UI.printInfo(UI.Module.API, "ILGPU Accelerator: {0}", accelerator.Name);

            // Pre-compile kernel
            rayGenKernel = accelerator.LoadAutoGroupedStreamKernel<Index2D, ArrayView2D<GpuIntersection, Stride2D.DenseX>, int, int, GpuRay, ArrayView<GpuTriangle>, ArrayView<int>, ArrayView<int>>(Kernels.RayGenKernel);
        }

        public void Render(int width, int height, GpuTriangle[] cpuTriangles, int[] cpuTree, int[] cpuObjects)
        {
            using var trianglesBuffer = accelerator.Allocate1D(cpuTriangles);
            using var treeBuffer = accelerator.Allocate1D(cpuTree);
            using var objectsBuffer = accelerator.Allocate1D(cpuObjects);
            using var outputBuffer = accelerator.Allocate2DDenseX<GpuIntersection>(new Index2D(width, height));

            GpuRay baseRay = new GpuRay(0, 0, -10, 0, 0, 1, 0, 1000);

            rayGenKernel(outputBuffer.Extent.ToIntIndex(), outputBuffer.View, width, height, baseRay, trianglesBuffer.View, treeBuffer.View, objectsBuffer.View);

            accelerator.Synchronize();

            // Download results
            // GpuIntersection[,] results = outputBuffer.GetAsArray2D();
        }

        public void Dispose()
        {
            accelerator.Dispose();
            context.Dispose();
        }
    }
}
