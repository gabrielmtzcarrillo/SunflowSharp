# SunflowSharp (.NET 10 Modernized)

**SunflowSharp** is a modern C# port and evolution of the original [Sunflow](https://sunflow.sourceforge.net/) renderer, fully upgraded for **.NET 10**.

This project aims to provide a high-performance, experimentation-friendly ray tracing engine in C#, leveraging the latest language features and GPU acceleration capabilities.

![Render Sample](render.png)

## 🚀 Modernization Features (.NET 10)

The engine has been significantly overhauled to support modern development workflows:

- **Target Framework**: Migrated to `net10.0` for all core libraries and tools.
- **SDK-Style Projects**: Clean, modern project files with easy dependency management.
- **GPU Ready (ILGPU)**: Integrated **ILGPU** and **ILGPU.Algorithms**, establishing the foundation for offloading ray-primitive intersections and kernels to the GPU (CUDA/OpenCL/CPU fallback).
- **Thread Safety**: Implemented `ConcurrentBitmap`, a high-precision `float` buffer with atomic accumulation using `Interlocked` primitives, ensuring safe multi-threaded and GPU-driven rendering.
- **Robustness**: 
  - Added a **Magenta/Black Checkerboard Placeholder** for missing or corrupt texture assets.
  - Replaced obsolete/unsupported APIs (e.g., `Thread.Abort`) with modern alternatives.
- **Clean Code**: Enabled **Nullable Reference Types** (`<Nullable>enable</Nullable>`) across the solution to improve runtime stability.

## 🛠 Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Installation
```bash
git clone https://github.com/gabrielmtzcarrillo/SunflowSharp.git
cd SunflowSharp
```

### Building the Solution
```bash
dotnet build SunflowSharp.sln
```

## 📖 Usage Examples

### CLI Rendering
You can render scenes directly from the command line using the Test project. The renderer supports `.sc` and compressed `.sc.gz` scene formats.

```bash
# Render a demo scene (CLI)
dotnet run --project SunflowSharp.Test/SunflowSharp.Test.csproj -- examples/julia.sc.gz

# Render a demo scene with specific texture paths
dotnet run --project SunflowSharp.Test/SunflowSharp.Test.csproj -- examples/bump_demo.sc.gz
```

### Windows GUI (WinForms)
The GUI project has been modernized to run as a .NET 10 Windows application.

```bash
dotnet run --project SunflowSharp.Gui/SunflowSharp.Gui.csproj
```

## 🏗 Project Structure

- **SunflowSharp**: Core rendering engine logic (Acceleration structures, Shaders, Geometry).
- **SunflowSharp.FreeImage**: High-performance image saving/loading backend.
- **SunflowSharp.Gui**: Windows-based interactive preview tool.
- **SunflowSharp.Test**: Command-line interface for batch rendering and testing.

## ⚖ License

This project follows the same licensing model as the original Sunflow (GPL or similar). See `LICENSE` for details.

---
*Maintained by Gabriel Mtz Carrillo and modernized for the .NET 10 era.*
