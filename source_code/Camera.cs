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
    using System.Collections.Generic;
    using System.Linq;

    using AForge.Video;
    using AForge.Video.DirectShow;
    using AForge.Vision.Motion;

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


        private double _alarmLvl;

        public double alarmLvl
        {
            get
            {
                return _alarmLvl;
            }
            set
            {
                _alarmLvl = value;
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

        private string _name;

        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
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

        private string cameraName;
        private bool ipCamera;

        private IVideoSource videoSource = null;
        private MotionDetector motionDetector = null;
        //private Bitmap lastFrame = null;
        public Bitmap pubFrame = null;

        private bool _detectionOn = false;



        public bool detectionOn
        {

            get { return _detectionOn; }

            set
            {

                if (MotionDetector != null)
                {

                    MotionDetector.Reset();

                }


                if (!value)
                {

                    MotionLevelArgs a = new MotionLevelArgs();
                    CamIdArgs b = new CamIdArgs();
                    a.lvl = 0;
                    a.alarmLvl = movementVal;
                    b.cam = camNo;
                    b.name = name;
                    motionLevelEvent(null, a, b);

                }

                _detectionOn = value;

            }


        }



        public bool alert = false;
        public bool alarmActive = false;
        public bool publishActive = false;

        // image width and height
        private int width = -1, height = -1;

        // alarm level
        public int camNo = 0;
        public double movementVal = 1;
        //public int timeSpike = 0;
        //public int toleranceSpike = 0;
        public bool triggeredBySpike = false;
        public bool certifiedTriggeredByNonSpike = true;



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

        public string name
        {
            get { return cameraName; }
        }

        public bool isIPCamera
        {
            get { return ipCamera; }
        }

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
            get { return (videoSource == null) ? 0 : (int)videoSource.BytesReceived; }
        }
        // Running property
        public bool Running
        {
            get { return (videoSource == null) ? false : videoSource.IsRunning; }
        }
        // MotionDetector property
        public MotionDetector MotionDetector
        {
            get { return motionDetector; }
            set { motionDetector = value; }
        }

        public List<DateTime> framesDt = new List<DateTime>();




        //Carried over from motiondetector3optimized class

        private bool Calibrating = false;
        private bool AreaOffAtMotionTriggered = false;
        private bool AreaOffAtMotionReset = false;
        private bool AreaDetection = false;
        private bool AreaDetectionWithin = false;
        private bool ExposeArea = false;
        private int RectWidth = 80;
        private int RectHeight = 80;
        private int RectX = 20;
        private int RectY = 20;

        public Frames frames = new Frames();
        public int FrameRateCalculated = 0;
        public DateTime ConnectedAt;

        public bool calibrating
        {
            get { return Calibrating; }
            set { Calibrating = value; }
        }
        public bool areaOffAtMotionTriggered
        {
            get { return AreaOffAtMotionTriggered; }
            set { AreaOffAtMotionTriggered = value; }
        }
        public bool areaOffAtMotionReset
        {
            get { return AreaOffAtMotionReset; }
            set { AreaOffAtMotionReset = value; }
        }
        public bool areaDetection
        {
            get { return AreaDetection; }
            set { AreaDetection = value; }
        }
        public bool areaDetectionWithin
        {
            get { return AreaDetectionWithin; }
            set { AreaDetectionWithin = value; }
        }
        public bool exposeArea
        {
            get { return ExposeArea; }
            set { ExposeArea = value; }
        }
        public int rectWidth
        {
            get { return RectWidth; }
            set { RectWidth = value; }
        }
        public int rectHeight
        {
            get { return RectHeight; }
            set { RectHeight = value; }
        }
        public int rectX
        {
            get { return RectX; }
            set { RectX = value; }
        }
        public int rectY
        {
            get { return RectY; }
            set { RectY = value; }
        }

        //Carried over from motiondetector3optimized class

        public bool frameRateTrack = false;
        public class FrameRateInfo
        {
            public DateTime dateTime = DateTime.Now;
            public long frames = 1;
        }

        public class Frames
        {
            int count = 0;
            public List<FrameRateInfo> framesInfo = new List<FrameRateInfo>();
            //maintain a count of 1000 frames
            public void addFrame()
            {
                framesInfo.Add(new FrameRateInfo());
            }

            public int LastFrameSecondsAgo()
            {
                if (framesInfo.Count == 0) return int.MaxValue;
                return (int)(DateTime.Now - framesInfo.Last().dateTime).TotalSeconds;
            }

            public void purge(int numberToKeep)
            {
                DateTime now = DateTime.Now;
                int deleteUpTo = 0;

                for (int i = framesInfo.Count - 1; i >= 0; i--)
                {
                    if ((now - framesInfo[i].dateTime).TotalSeconds > numberToKeep)
                    {
                        deleteUpTo = i + 1;
                        break;
                    }
                }

                if (deleteUpTo > 0)
                {
                    framesInfo.RemoveRange(0, deleteUpTo);
                    count = framesInfo.Count;
                }


                //if (numberToKeep > framesInfo.Count)
                //{
                //    return;
                //}

                //if (numberToKeep == 0)
                //{
                //    framesInfo.Clear();
                //    count = 0;
                //}
                //else
                //{
                //    //framesInfo.RemoveRange(numberToKeep - 1, framesInfo.Count - numberToKeep);
                //    framesInfo.RemoveRange(0, framesInfo.Count - numberToKeep);
                //    count = numberToKeep;
                //}
            }
        }




        // Constructor
        public Camera(VideoCaptureDevice source)
                : this(source, null, null)
        { }


        public Camera(VideoCaptureDevice source, MotionDetector detector, string _name)
        {

            ipCamera = false;
            cameraName = _name;
            pubFrame = null;
            this.videoSource = source;
            this.motionDetector = detector;
            videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);

        }


        public Camera(MJPEGStream source, MotionDetector detector, string _name)
        {

            ipCamera = true;
            cameraName = _name;
            pubFrame = null;
            this.videoSource = source;
            this.motionDetector = detector;
            videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);

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

                try
                {
                    pubFrame = (Bitmap)e.Frame.Clone();
                    //#ToDo
                    if (frameRateTrack)
                    {
                        frames.addFrame();
                    }
                    //purge outside of this on a semi-regular basis
                    //framesDtCount.purge(100);
                    //framesDt.Add(DateTime.Now);
                }
                catch (Exception)
                {
                    pubFrame = (Bitmap)Properties.Resources.imagenotavailable.Clone();
                }



                if (_detectionOn)
                {


                    if ((calibrating && camNo == CameraRig.trainCam) || (alarmActive && alert))
                    {

                        // apply motion detector
                        if (motionDetector != null)
                        {


                            //*************************************
                            //pre-process bitmap for area detection
                            //*************************************

                            Bitmap areaDetectionPreparedImage;

                            if (areaOffAtMotionTriggered && !areaOffAtMotionReset)
                            {
                                motionDetector.Reset();
                                areaOffAtMotionReset = true;
                            }

                            if (areaDetection && !areaOffAtMotionTriggered)
                            {
                                areaDetectionPreparedImage = selectArea(pubFrame, AreaDetectionWithin, RectWidth, RectHeight, RectX, RectY);

                                if (exposeArea)
                                {
                                    pubFrame = areaDetectionPreparedImage;
                                }
                            }
                            else
                            {
                                areaDetectionPreparedImage = pubFrame;
                            }


                            //*************************************
                            //pre-process bitmap for area detection
                            //*************************************



                            motionDetector.ProcessFrame(areaDetectionPreparedImage);


                            MotionLevelArgs a = new MotionLevelArgs();
                            CamIdArgs b = new CamIdArgs();
                            a.lvl = motionDetector.MotionDetectionAlgorithm.MotionLevel;
                            a.alarmLvl = movementVal;
                            b.cam = camNo;
                            b.name = name;
                            motionLevelEvent(null, a, b);

                            // check motion level
                            if (calibrating && camNo == CameraRig.trainCam)
                            {
                                bubble.train(motionDetector.MotionDetectionAlgorithm.MotionLevel);
                            }
                            else
                            {
                                if (alarmActive &&
                                    alert &&
                                    motionDetector.MotionDetectionAlgorithm.MotionLevel >= movementVal
                                    && motionAlarm != null)
                                {
                                    CamIdArgs c = new CamIdArgs();
                                    c.cam = camNo;
                                    c.name = name;
                                    LevelArgs l = new LevelArgs();
                                    l.lvl = Convert.ToInt32(100 * motionDetector.MotionDetectionAlgorithm.MotionLevel);

                                    motionAlarm(null, c, l);

                                }
                                else
                                {
                                    certifiedTriggeredByNonSpike = false;
                                    triggeredBySpike = false;
                                }
                            }

                        }



                    }

                }//if (_detectionOn)



                // image dimension
                width = pubFrame.Width;
                height = pubFrame.Height;

                //#ref5617


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
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












        // On new frame
        private void video_NewFrameOLD(object sender, NewFrameEventArgs e)
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

                try
                {
                    pubFrame = (Bitmap)e.Frame.Clone();
                }
                catch (Exception)
                {
                    pubFrame = (Bitmap)Properties.Resources.imagenotavailable.Clone();
                }

                if (_detectionOn)
                {

                    // apply motion detector
                    if (motionDetector != null)
                    {

                        //*************************************
                        //pre-process bitmap for area detection
                        //*************************************

                        Bitmap areaDetectionPreparedImage;

                        if (areaOffAtMotionTriggered && !areaOffAtMotionReset)
                        {
                            motionDetector.Reset();
                            areaOffAtMotionReset = true;
                        }

                        if (areaDetection && !areaOffAtMotionTriggered)
                        {
                            areaDetectionPreparedImage = selectArea(pubFrame, AreaDetectionWithin, RectWidth, RectHeight, RectX, RectY);

                            if (exposeArea)
                            {
                                pubFrame = areaDetectionPreparedImage;
                            }
                        }
                        else
                        {
                            areaDetectionPreparedImage = pubFrame;
                        }


                        //*************************************
                        //pre-process bitmap for area detection
                        //*************************************


                        motionDetector.ProcessFrame(areaDetectionPreparedImage);


                        MotionLevelArgs a = new MotionLevelArgs();
                        CamIdArgs b = new CamIdArgs();
                        a.lvl = motionDetector.MotionDetectionAlgorithm.MotionLevel;
                        a.alarmLvl = movementVal;
                        b.cam = camNo;
                        b.name = name;
                        motionLevelEvent(null, a, b);


                        // check motion level
                        if (calibrating && camNo == CameraRig.trainCam)
                        {
                            bubble.train(motionDetector.MotionDetectionAlgorithm.MotionLevel);
                        }
                        else
                        {
                            if (alarmActive && alert && motionDetector.MotionDetectionAlgorithm.MotionLevel >= movementVal && motionAlarm != null)
                            {

                                CamIdArgs c = new CamIdArgs();
                                c.cam = camNo;
                                c.name = name;
                                LevelArgs l = new LevelArgs();
                                l.lvl = Convert.ToInt32(100 * motionDetector.MotionDetectionAlgorithm.MotionLevel);

                                motionAlarm(null, c, l);

                            }
                        }
                    }

                }//if (_detectionOn)



                // image dimension
                width = pubFrame.Width;
                height = pubFrame.Height;

                //#ref5617


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
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




        public Bitmap selectArea(Bitmap image, bool toHide, int width, int height, int topLeftX, int topLeftY)
        {

            Bitmap processedBitmap = null;
            int imageHeight = image.Height;
            int imageWidth = image.Width;

            Graphics graphicsObj;

            if (topLeftY + height > image.Height) { height = image.Height - topLeftY; }
            if (height < 1) { height = 1; }
            if (topLeftY < 0) { topLeftY = 0; }
            if (topLeftY >= image.Height)
            {
                topLeftY = image.Height - 1;
                height = 1;
            }

            if (topLeftX + width > image.Width) { width = image.Width - topLeftX; }
            if (width < 1) { width = 1; }
            if (topLeftX < 0) { topLeftX = 0; }
            if (topLeftX >= image.Width)
            {
                topLeftX = image.Width - 1;
                width = 1;
            }

            if (!toHide)
            {
                try
                {
                    SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
                    processedBitmap = (Bitmap)image.Clone();
                    graphicsObj = Graphics.FromImage((Bitmap)processedBitmap);
                    Rectangle rectangleObj = new Rectangle(topLeftX, topLeftY, width, height);
                    graphicsObj.FillRectangle(myBrush, rectangleObj);
                }
                catch { }
            }
            else
            {
                try
                {
                    Rectangle rectangleObj = new Rectangle(topLeftX, topLeftY, width, height);
                    processedBitmap = image.Clone(rectangleObj, image.PixelFormat);
                }
                catch { }
            }

            return processedBitmap;

        }





    }
}
