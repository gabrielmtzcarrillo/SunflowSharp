using System;
using SunflowSharp.Core;
using SunflowSharp.Image;
using SunflowSharp.Systems;

namespace SunflowSharp.Core.Display
{

    public class FrameDisplay : IDisplay
    {
        private string filename;
        private RenderFrame frame;

        public FrameDisplay()
        {
            this(null);
        }

        public FrameDisplay(string filename)
        {
            this.filename = filename;
            frame = null;
        }

        public void imageBegin(int w, int h, int bucketSize)
        {
            if (frame == null)
            {
                frame = new RenderFrame();
                frame.imagePanel.imageBegin(w, h, bucketSize);
                Dimension screenRes = Toolkit.getDefaultToolkit().getScreenSize();
                bool needFit = false;
                if (w >= (screenRes.getWidth() - 200) || h >= (screenRes.getHeight() - 200))
                {
                    frame.imagePanel.setPreferredSize(new Dimension((int)screenRes.getWidth() - 200, (int)screenRes.getHeight() - 200));
                    needFit = true;
                }
                else
                    frame.imagePanel.setPreferredSize(new Dimension(w, h));
                frame.pack();
                frame.setLocationRelativeTo(null);
                frame.setVisible(true);
                if (needFit)
                    frame.imagePanel.fit();
            }
            else
                frame.imagePanel.imageBegin(w, h, bucketSize);
        }

        public void imagePrepare(int x, int y, int w, int h, int id)
        {
            frame.imagePanel.imagePrepare(x, y, w, h, id);
        }

        public void imageUpdate(int x, int y, int w, int h, Color[] data)
        {
            frame.imagePanel.imageUpdate(x, y, w, h, data);
        }

        public void imageFill(int x, int y, int w, int h, Color c)
        {
            frame.imagePanel.imageFill(x, y, w, h, c);
        }

        public void imageEnd()
        {
            frame.imagePanel.imageEnd();
            if (filename != null)
                frame.imagePanel.save(filename);
        }

        private static class RenderFrame : JFrame
        {
            ImagePanel imagePanel;

            RenderFrame()
            {
                super("Sunflow v" + SunflowAPI.VERSION);
                setDefaultCloseOperation(EXIT_ON_CLOSE);
                //addKeyListener(new KeyAdapter() {//fixme: change to WinForms
                //    @Override
                //    public void keyPressed(KeyEvent e) {
                //        if (e.getKeyCode() == KeyEvent.VK_ESCAPE)
                //            System.exit(0);
                //    }
                //});
                imagePanel = new ImagePanel();
                setContentPane(imagePanel);
                pack();
            }
        }
    }
}