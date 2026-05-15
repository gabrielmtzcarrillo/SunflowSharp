using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Versioning;
using System.Threading;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using SunflowSharp.Core;
using WinRT.Interop;
using Windows.Graphics;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace SunflowSharp.Gui
{
    [SupportedOSPlatform("windows10.0.17763.0")]
    public sealed partial class MainWindow : Window, IDisplay
    {
        private readonly object bitmapLock = new object();
        private Bitmap? bitmap;
        private WriteableBitmap? previewBitmap;
        private byte[] pixelBytes = Array.Empty<byte>();
        private int imageWidth;
        private int imageHeight;
        private int previewRefreshQueued;
        private bool renderStarted;

        public MainWindow()
        {
            InitializeComponent();

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
            AppWindow.SetIcon("Assets/AppIcon.ico");
            Activated += MainWindow_Activated;
        }

        private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            if (renderStarted)
            {
                return;
            }

            renderStarted = true;
            Thread renderThread = new Thread(Render)
            {
                IsBackground = true,
                Name = "SunflowSharp WinUI renderer"
            };
            renderThread.Start();
        }

        private void Render()
        {
            GuiApi api = new GuiApi("gumbo_and_teapot.sc.gz");
            api.build();
            api.render("::options", this);
        }

        public void imageBegin(int w, int h, int bucketSize)
        {
            lock (bitmapLock)
            {
                bitmap?.Dispose();
                bitmap = new Bitmap(w, h, PixelFormat.Format32bppArgb);
                pixelBytes = new byte[w * h * 4];
                imageWidth = w;
                imageHeight = h;
                previewRefreshQueued = 0;
            }

            QueueUiAndWait(() =>
            {
                lock (bitmapLock)
                {
                    previewBitmap = new WriteableBitmap(w, h);
                    RenderImage.Source = previewBitmap;
                }

                RenderImage.Width = w;
                RenderImage.Height = h;
                SaveButton.IsEnabled = true;
                StatusText.Text = $"Rendering {w} x {h}";
                Title = "SunflowSharp GUI - Rendering...";
                AppWindow.Resize(new SizeInt32(Math.Max(w + 48, 480), Math.Max(h + 144, 360)));
            });
        }

        public void imagePrepare(int x, int y, int w, int h, int id)
        {
        }

        public void imageUpdate(int x, int y, int w, int h, SunflowSharp.Image.Color[] data)
        {
            lock (bitmapLock)
            {
                if (bitmap is null || previewBitmap is null)
                {
                    return;
                }

                for (int localY = 0, index = 0; localY < h; localY++)
                {
                    for (int localX = 0; localX < w; localX++, index++)
                    {
                        int destinationX = x + localX;
                        int destinationY = y + localY;
                        int rgb = data[index].copy().toNonLinear().toRGB();
                        Color pixel = Color.FromArgb(rgb);

                        bitmap.SetPixel(destinationX, destinationY, pixel);
                        SetPreviewPixel(destinationX, destinationY, pixel);
                    }
                }
            }

            RequestPreviewRefresh();
        }

        public void imageFill(int x, int y, int w, int h, SunflowSharp.Image.Color c)
        {
            Color pixel = Color.FromArgb(c.copy().toNonLinear().toRGB());

            lock (bitmapLock)
            {
                if (bitmap is null)
                {
                    return;
                }

                int maxX = Math.Min(x + w, imageWidth);
                int maxY = Math.Min(y + h, imageHeight);

                for (int currentY = y; currentY < maxY; currentY++)
                {
                    for (int currentX = x; currentX < maxX; currentX++)
                    {
                        bitmap.SetPixel(currentX, currentY, pixel);
                        SetPreviewPixel(currentX, currentY, pixel);
                    }
                }
            }

            RequestPreviewRefresh();
        }

        public void imageEnd()
        {
            QueueUi(() =>
            {
                StatusText.Text = "Done";
                Title = "SunflowSharp GUI - Done";
            });
        }

        private void SetPreviewPixel(int x, int y, Color pixel)
        {
            int offset = ((y * imageWidth) + x) * 4;
            pixelBytes[offset] = pixel.B;
            pixelBytes[offset + 1] = pixel.G;
            pixelBytes[offset + 2] = pixel.R;
            pixelBytes[offset + 3] = pixel.A;
        }

        private void RequestPreviewRefresh()
        {
            if (previewBitmap is null || Interlocked.Exchange(ref previewRefreshQueued, 1) == 1)
            {
                return;
            }

            QueueUi(() =>
            {
                lock (bitmapLock)
                {
                    if (previewBitmap is null)
                    {
                        Interlocked.Exchange(ref previewRefreshQueued, 0);
                        return;
                    }

                    using Stream stream = previewBitmap.PixelBuffer.AsStream();
                    stream.Position = 0;
                    stream.Write(pixelBytes, 0, pixelBytes.Length);
                    previewBitmap.Invalidate();
                }

                Interlocked.Exchange(ref previewRefreshQueued, 0);
            });
        }

        private void QueueUi(Action action)
        {
            if (DispatcherQueue.HasThreadAccess)
            {
                action();
                return;
            }

            DispatcherQueue.TryEnqueue(() => action());
        }

        private void QueueUiAndWait(Action action)
        {
            if (DispatcherQueue.HasThreadAccess)
            {
                action();
                return;
            }

            using ManualResetEventSlim completed = new ManualResetEventSlim(false);
            Exception? failure = null;

            DispatcherQueue.TryEnqueue(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    failure = ex;
                }
                finally
                {
                    completed.Set();
                }
            });

            completed.Wait();

            if (failure is not null)
            {
                throw failure;
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Bitmap? snapshot;

            lock (bitmapLock)
            {
                snapshot = bitmap is null ? null : (Bitmap)bitmap.Clone();
            }

            if (snapshot is null)
            {
                return;
            }

            FileSavePicker picker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                SuggestedFileName = "sunflow-render"
            };
            picker.FileTypeChoices.Add("Bitmap image", new List<string> { ".bmp" });
            InitializeWithWindow.Initialize(picker, WindowNative.GetWindowHandle(this));

            StorageFile? file = await picker.PickSaveFileAsync();
            if (file is null)
            {
                snapshot.Dispose();
                return;
            }

            await using Stream output = await file.OpenStreamForWriteAsync();
            output.SetLength(0);
            snapshot.Save(output, ImageFormat.Bmp);
            snapshot.Dispose();

            StatusText.Text = $"Saved {file.Name}";
        }
    }
}
