// Motion Detector
//
// Copyright © Andrew Kirillov, 2005
// andrew.kirillov@gmail.com
//
namespace TeboCam
{
    using System;
    using System.Drawing;
    using System.Threading;
    //using VideoSource;
    using System.Diagnostics;

    using AForge.Video;
    using AForge.Video.DirectShow;

    public delegate void alarmEventHandler(object source, CamIdArgs e, LevelArgs l);
    public delegate void motionLevelEventHandler(object source, MotionLevelArgs a, CamIdArgs b);
    public delegate void drawEventHandler(object source, drawArgs e);

    public class MotionLevelArgs : EventArgs
    {
        private double _lvl;

        public double lvl
        {
            get
            {
                return _lvl;
            }
            set
            {
                _lvl = value;
            }
        }
    }

    public class AlarmArgs : EventArgs
    {
        private int _cam;

        public int cam
        {
            get
            {
                return _cam;
            }
            set
            {
                _cam = value;
            }
        }
    }

    public class CamIdArgs : EventArgs
    {
        private int _cam;

        public int cam
        {
            get
            {
                return _cam;
            }
            set
            {
                _cam = value;
            }
        }
    }

    public class LevelArgs : EventArgs
    {
        private int _lvl;

        public int lvl
        {
            get
            {
                return _lvl;
            }
            set
            {
                _lvl = value;
            }
        }
    }

    public class drawArgs : EventArgs
    {
        private int _x;
        private int _y;
        private int _width;
        private int _height;

        public int x
        {
            get { return _x; }
            set { _x = value; }
        }
        public int y
        {
            get { return _y; }
            set { _y = value; }
        }
        public int width
        {
            get { return _width; }
            set { _width = value; }
        }
        public int height
        {
            get { return _height; }
            set { _height = value; }
        }

    }






    /// <summary>
    /// Camera class
    /// </summary>
    public class Camera
    {

        private IVideoSource videoSource = null;
        private IMotionDetector motionDetecotor = null;
        //private Bitmap lastFrame = null;
        public Bitmap pubFrame = null;

        public bool alert = false;
        public bool alarmActive = false;
        public bool publishActive = false;

        // image width and height
        private int width = -1, height = -1;

        // alarm level
        public int cam = 0;
        public double movementVal = 1;

        //
        public event motionLevelEventHandler motionLevelEvent;
        public event alarmEventHandler motionAlarm;
        public event EventHandler NewFrame;


        // LastFrame property
        //public Bitmap LastFrame
        //{
        //    get { return lastFrame; }
        //}
        // Width property
        public int Width
        {
            get { return width; }
        }
        // Height property
        public int Height
        {
            get { return height; }
        }
        // FramesReceived property
        public int FramesReceived
        {
            get { return (videoSource == null) ? 0 : videoSource.FramesReceived; }
        }
        // BytesReceived property
        public int BytesReceived
        {
            get { return (videoSource == null) ? 0 : videoSource.BytesReceived; }
        }
        // Running property
        public bool Running
        {
            get { return (videoSource == null) ? false : videoSource.IsRunning; }
        }
        // MotionDetector property
        public IMotionDetector MotionDetector
        {
            get { return motionDetecotor; }
            set { motionDetecotor = value; }
        }

        // Constructor
        public Camera(VideoCaptureDevice source)
            : this(source, null)
        { }
        public Camera(VideoCaptureDevice source, IMotionDetector detector)
        {
            pubFrame = null;
            this.videoSource = source;
            this.motionDetecotor = detector;
            videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
        }


        public Camera(MJPEGStream source, IMotionDetector detector)
        {
            pubFrame = null;
            this.videoSource = source;
            this.motionDetecotor = detector;
            source.NewFrame += new NewFrameEventHandler(video_NewFrame);
        }





        // Start video source
        public void Start()
        {
            if (videoSource != null)
            {
                videoSource.Start();
            }
        }

        // Siganl video source to stop
        public void SignalToStop()
        {
            if (videoSource != null)
            {
                videoSource.SignalToStop();
            }
        }

        // Wait video source for stop
        public void WaitForStop()
        {
            // lock
            Monitor.Enter(this);

            if (videoSource != null)
            {
                videoSource.WaitForStop();
            }
            // unlock
            Monitor.Exit(this);
        }

        // Abort camera
        public void Stop()
        {
            // lock
            Monitor.Enter(this);

            if (videoSource != null)
            {
                videoSource.Stop();
            }
            // unlock
            Monitor.Exit(this);
        }

        // Lock it
        public void Lock()
        {
            try
            {
                Monitor.Enter(this);
            }
            catch { }
        }

        // Unlock it
        public void Unlock()
        {
            try
            {
                Monitor.Exit(this);
            }
            catch { }
        }

        // On new frame
        private void video_NewFrame(object sender, NewFrameEventArgs e)
        {
            try
            {
                // lock
                Monitor.Enter(this);

                // dispose old frame
                if (pubFrame != null)
                {
                    pubFrame.Dispose();
                }


                //lastFrame = (Bitmap)e.Frame.Clone();
                pubFrame = (Bitmap)e.Frame.Clone();


                // apply motion detector
                if (motionDetecotor != null)
                {

                    motionDetecotor.ProcessFrame(ref pubFrame);


                    MotionLevelArgs a = new MotionLevelArgs();
                    CamIdArgs b = new CamIdArgs();
                    a.lvl = motionDetecotor.MotionLevel;
                    b.cam = cam;
                    motionLevelEvent(null, a, b);


                    // check motion level
                    if (motionDetecotor.calibrating && cam == CameraRig.trainCam)
                    {
                        bubble.train(motionDetecotor.MotionLevel);
                    }
                    else
                    {
                        if (alarmActive && alert && motionDetecotor.MotionLevel >= movementVal && motionAlarm != null)
                        {

                            CamIdArgs c = new CamIdArgs();
                            c.cam = cam;
                            LevelArgs l = new LevelArgs();
                            l.lvl = Convert.ToInt32(100 * motionDetecotor.MotionLevel);

                            motionAlarm(null, c, l);

                        }
                    }
                }

                // image dimension
                width = pubFrame.Width;
                height = pubFrame.Height;

                //#ref5617


            }
            catch (Exception)
            {
            }
            finally
            {
                // unlock
                Monitor.Exit(this);
            }

            // notify client
            if (NewFrame != null)
            {
                NewFrame(this, new EventArgs());
            }
        }





    }
}
