using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class ControlRoomCntl : UserControl
    {

        private List<Monitor> monitors = new List<Monitor>();
        private int windowWidth = 150;
        private int windowHeight = 150;
        Ping ping;

        public ControlRoomCntl(List<Camera> cameras, Ping pinger)
        {
            ping = pinger;
            InitializeComponent();
            LoadCameras(cameras);
        }

        private void LoadCameras(List<Camera> cameras)
        {
            for (int i = 0; i < 9; i++)
            {
                Monitor monitor = new Monitor();
                CameraWindow window = new CameraWindow();
                window.ping = ping;
                monitor.window = window;
                window.AutoSizeCameraWindow = false;
                window.Size = new Size(windowWidth, windowHeight);
                if (cameras.Count - 1 >= i)
                {
                    window.Camera = cameras[i];
                    monitor.FriendlyName = window.Camera.camNo.ToString();
                    EventHandler handlerWindow = (sender, args) => { windowSelected(monitor.FriendlyName); };
                    window.DoubleClick += handlerWindow;
                }
                monitors.Add(monitor);
            }
            int windowNo = 1;
            int xPos = -windowWidth;
            int yPos = -windowHeight;
            foreach (Monitor monitor in monitors)
            {
                if (new List<int>() { 1, 4, 7 }.Contains(windowNo))
                {
                    xPos = 2;
                    yPos += windowHeight + 2;
                }
                else
                {
                    xPos += windowWidth + 2;
                }
                monitor.window.Location = new Point(xPos, yPos);
                this.Controls.Add(monitor.window);
                windowNo++;
            }
            this.Width = monitors.Last().window.Right + 5;
            this.Height = monitors.Last().window.Bottom + 5;

        }

        private void windowSelected(string windowName)
        {
            CameraWindow window = monitors.Where(x => x.FriendlyName == windowName).First().window;

            if (window.Height != windowHeight)
            {
                monitors.ForEach(x => x.window.Size = new Size(windowWidth, windowHeight));

                int windowNo = 1;
                int xPos = -windowWidth;
                int yPos = -windowHeight;
                foreach (Monitor monitor in monitors)
                {
                    if (new List<int>() { 1, 4, 7 }.Contains(windowNo))
                    {
                        xPos = 2;
                        yPos += windowHeight + 2;
                    }
                    else
                    {
                        xPos += windowWidth + 2;
                    }
                    monitor.window.Location = new Point(xPos, yPos);
                    this.Controls.Add(monitor.window);
                    windowNo++;
                }
            }
            else
            {
                monitors.ForEach(x => x.window.Size = new Size(0, 0));
                monitors.ForEach(x => x.window.Location = new Point(0, 0));
                window.Height = this.Height;
                window.Width = this.Width;
            }
        }

        class Monitor
        {
            public string FriendlyName;
            public CameraWindow window = new CameraWindow();
            public Point position = new Point();
            public Size size = new Size();
        }

    }
}
