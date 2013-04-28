// Motion Detector
//
// Copyright © Andrew Kirillov, 2005
// andrew.kirillov@gmail.com
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Data;
using System.Windows.Forms;
using System.Threading;

namespace TeboCam
{
    /// <summary>
    /// Summary description for CameraWindow.
    /// </summary>




    public class CameraWindow : System.Windows.Forms.Control
    {

        //public int activeCamera;

        private Camera camera = null;
        private bool autosize = true;
        private bool needSizeUpdate = false;
        private bool firstFrame = true;

        public bool imageToFrame = false;
        public bool showCam = true;

        //private System.Timers.Timer timer;
        private Color rectColor = Color.Black;

        //****************************************************************************
        //****************************************************************************
        //****************************************************************************
        //****************************************************************************

        public bool haveTheFlag = false;

        public enum CursPos : int
        {
            WithinSelectionArea = 0,
            OutsideSelectionArea,
            TopLine,
            BottomLine,
            LeftLine,
            RightLine,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }

        public enum ClickAction : int
        {
            NoClick = 0,
            Dragging,
            Outside,
            TopSizing,
            BottomSizing,
            LeftSizing,
            TopLeftSizing,
            BottomLeftSizing,
            RightSizing,
            TopRightSizing,
            BottomRightSizing
        }


        public bool selectionOn = false;
        public bool selectingArea = false;

        public ClickAction CurrentAction;
        public static bool LeftButtonDown = false;
        public bool RectangleDrawn = false;
        public static bool ReadyToDrag = false;

        public static Point ClickPoint = new Point();
        public static Point CurrentTopLeft = new Point();
        public static Point CurrentBottomRight = new Point();
        public static Point DragClickRelative = new Point();


        public static int RectangleHeight = new int();
        public static int RectangleWidth = new int();

        Pen EraserPen = new Pen(Color.FromArgb(255, 255, 192), 1);
        Pen MyPen = new Pen(Color.Red, 2);

        public bool rectDrawn = false;

        //****************************************************************************
        //****************************************************************************
        //****************************************************************************
        //****************************************************************************



        // AutoSize property
        [DefaultValue(false)]
        public bool AutoSize
        {
            get { return autosize; }
            set
            {
                autosize = value;
                UpdatePosition();
            }
        }



        // Camera property
        [Browsable(false)]
        public Camera Camera
        {
            get { return camera; }
            set
            {
                // lock
                Monitor.Enter(this);

                // detach event
                if (camera != null)
                {
                    camera.NewFrame -= new EventHandler(camera_NewFrame);

                    //camera.Alarm -= new EventHandler(camera_Alarm);
                    //camera.motionAlarm -= new alarmEventHandler(camera_Alarm);

                    //timer.Stop();
                }

                camera = value;
                needSizeUpdate = true;
                firstFrame = true;
                //flash = 0;


                // attach event
                if (camera != null)
                {
                    camera.NewFrame += new EventHandler(camera_NewFrame);


                    //camera.Alarm += new EventHandler(camera_Alarm);                    
                    //camera.motionAlarm += new alarmEventHandler(camera_Alarm);

                    bubble.takePicture += new EventHandler(take_picture);
                    //bubble.pubPicture += new ImagePubEventHandler(take_picture_publish);
                    //preferences.drawInitialRectangle += new EventHandler(drawInitialRectangle);

                    webcamConfig.drawInitialRectangle -= new drawEventHandler(drawInitialRectangle);
                    webcamConfig.drawInitialRectangle += new drawEventHandler(drawInitialRectangle);

                    //timer.Start();
                }

                // unlock
                Monitor.Exit(this);
            }
        }


        // Constructor
        public CameraWindow()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer |
                ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);

            this.MouseDown -= new MouseEventHandler(mouse_Click);
            this.MouseDown += new MouseEventHandler(mouse_Click);
            this.MouseMove -= new MouseEventHandler(mouse_Move);
            this.MouseMove += new MouseEventHandler(mouse_Move);
            this.MouseUp -= new MouseEventHandler(mouse_Up);
            this.MouseUp += new MouseEventHandler(mouse_Up);


        }









        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            //this.timer = new System.Timers.Timer();
            //((System.ComponentModel.ISupportInitialize)(this.timer)).BeginInit();
            // 
            // timer
            // 
            //this.timer.Interval = 250;
            //this.timer.SynchronizingObject = this;
            //this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
            //((System.ComponentModel.ISupportInitialize)(this.timer)).EndInit();

        }
        #endregion

        // Paint control
        protected override void OnPaint(PaintEventArgs pe)
        {
            if ((needSizeUpdate) || (firstFrame))
            {
                UpdatePosition();
                needSizeUpdate = false;
            }

            // lock
            Monitor.Enter(this);

            Graphics g = pe.Graphics;
            Rectangle rc = this.ClientRectangle;
            Pen pen = new Pen(rectColor, 1);

            // draw rectangle
            //newcode

            if (imageToFrame)
            {
                rc.Width = 320;
                rc.Height = 240;
            }
            //newcode


            g.DrawRectangle(pen, rc.X, rc.Y, rc.Width - 1, rc.Height - 1);

            //if (CameraRig.rig[CameraRig.activeCam].cam != null)
            if (camera != null)
            {
                try
                {
                    //20110403 was causing freezing
                    //CameraRig.rig[CameraRig.activeCam].cam.Lock();
                    camera.Lock();

                    // draw frame
                    //if (CameraRig.rig[CameraRig.activeCam].cam.LastFrame != null)
                    if (camera.pubFrame != null)
                    {
                        if (showCam)
                        {
                            //newcode
                            if (imageToFrame)
                            {
                                g.DrawRectangle(pen, camera.pubFrame.Width, camera.pubFrame.Height, rc.Width, rc.Height);
                                g.DrawImage(resizeImage(camera.pubFrame, 320, 240), rc.X + 1, rc.Y + 1, rc.Width - 2, rc.Height - 2);
                            }
                            else
                            {
                                g.DrawImage(camera.pubFrame, rc.X + 1, rc.Y + 1, rc.Width - 2, rc.Height - 2);
                            }
                            //newcode
                            firstFrame = false;

                        }
                    }
                    else
                    {
                        if (showCam)
                        {

                            // Create font and brush
                            Font drawFont = new Font("Arial", 12);
                            SolidBrush drawBrush = new SolidBrush(Color.White);

                            g.DrawString("Connecting ...", drawFont, drawBrush, new PointF(5, 5));

                            drawBrush.Dispose();
                            drawFont.Dispose();
                        }
                    }

                }
                catch (Exception)
                {
                    //CameraRig.rig[CameraRig.activeCam].cam.Unlock();
                    camera.Unlock();
                }
                finally
                {
                    //CameraRig.rig[CameraRig.activeCam].cam.Unlock();
                    camera.Unlock();
                }
            }

            pen.Dispose();

            // unlock
            Monitor.Exit(this);
            base.OnPaint(pe);
        }


        private static Size resizeImagesize(Size fromSize, Size toSize)
        {
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)toSize.Width / (float)fromSize.Width);
            nPercentH = ((float)toSize.Height / (float)fromSize.Height);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(fromSize.Width * nPercent);
            int destHeight = (int)(fromSize.Height * nPercent);

            Size tmpSize = new Size();
            tmpSize.Width = destWidth;
            tmpSize.Height = destHeight;
            return tmpSize;

        }

        private static Bitmap resizeImage(Bitmap imgToResize, int width, int height)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)width / (float)sourceWidth);
            nPercentH = ((float)height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Bitmap)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Bitmap)b;
        }






        // Update position and size of the control
        public void UpdatePosition()
        {
            try
            {
                // lock
                Monitor.Enter(this);

                if ((autosize) && (this.Parent != null))
                {
                    Rectangle rc = this.Parent.ClientRectangle;

                    //int width = CameraRig.rig[CameraRig.activeCam].cam.LastFrame.Width;
                    //int height = CameraRig.rig[CameraRig.activeCam].cam.LastFrame.Height;

                    int width = camera.pubFrame.Width;
                    int height = camera.pubFrame.Height;

                    //if (CameraRig.rig[CameraRig.activeCam].cam != null)
                    if (camera != null)
                    {

                        //CameraRig.rig[CameraRig.activeCam].cam.Lock();
                        camera.Lock();

                        // get frame dimension
                        //if (CameraRig.rig[CameraRig.activeCam].cam.LastFrame != null)
                        if (camera.pubFrame != null)
                        {
                            //width = CameraRig.rig[CameraRig.activeCam].cam.LastFrame.Width;
                            //height = CameraRig.rig[CameraRig.activeCam].cam.LastFrame.Height;
                            width = camera.pubFrame.Width;
                            height = camera.pubFrame.Height;
                        }
                        //CameraRig.rig[CameraRig.activeCam].cam.Unlock();
                        camera.Unlock();
                    }

                    //
                    this.SuspendLayout();
                    this.Size = new Size(width + 2, height + 2);
                    this.ResumeLayout();

                }
                // unlock
                Monitor.Exit(this);
            }
            catch
            { Monitor.Exit(this); }
        }

        // On new frame ready
        private void camera_NewFrame(object sender, System.EventArgs e)
        {
            Invalidate();
        }

        private void take_picture(object sender, System.EventArgs e)
        {

            haveTheFlag = true;


            string fName = "pingPicture.jpg";
            Bitmap saveBmp = null;
            try
            {

                List<string> lst = new List<string>();

                if (config.getProfile(bubble.profileInUse).pingStatsStamp)
                {

                    statistics.movementResults stats = new statistics.movementResults();
                    stats = statistics.statsForCam(CameraRig.activeCam, bubble.profileInUse, "Ping");

                    lst.Add(stats.avgMvStart.ToString());
                    lst.Add(stats.avgMvLast.ToString());
                    lst.Add(stats.mvNow.ToString());
                    lst.Add(Convert.ToBoolean(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[CameraRig.activeCam].cameraName, "alarmActive")) ? "On" : "Off");
                    lst.Add(config.getProfile(bubble.profileInUse).pingInterval.ToString() + " Mins");

                }

                imageText stampArgs = new imageText();
                stampArgs.bitmap = (Bitmap)camera.pubFrame.Clone();
                stampArgs.type = "Ping";
                stampArgs.backingRectangle = config.getProfile(bubble.profileInUse).pingTimeStampRect;
                stampArgs.stats = lst;

                //saveBmp = bubble.timeStampImage((Bitmap)CameraRig.rig[CameraRig.activeCam].cam.pubFrame.Clone(), "Ping");
                //saveBmp = bubble.timeStampImage((Bitmap)camera.pubFrame.Clone(), "Ping", config.getProfile(bubble.profileInUse).pingTimeStampRect);
                saveBmp = bubble.timeStampImage(stampArgs);

                //specify jpeg compression
                //ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                //System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                //EncoderParameters myEncoderParameters = new EncoderParameters(1);
                //EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                //myEncoderParameters.Param[0] = myEncoderParameter;
                //// Save the bitmap as a JPG file with 50 quality level compression.
                ////0 is greatest compression, 100 is least compression
                //saveBmp.Save(bubble.tmpFolder + fName, jgpEncoder, myEncoderParameters);

                ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, config.getProfile(bubble.profileInUse).pingCompression);
                myEncoderParameters.Param[0] = myEncoderParameter;
                saveBmp.Save(bubble.tmpFolder + fName, jgpEncoder, myEncoderParameters);


                //saveBmp.Save(bubble.tmpFolder + fName, ImageFormat.Jpeg);
                Bitmap thumb = bubble.GetThumb(saveBmp);
                thumb.Save(bubble.tmpFolder + bubble.tmbPrefix + fName, ImageFormat.Jpeg);

                saveBmp.Dispose();
                thumb.Dispose();
                bubble.logAddLine("Image saved: " + fName);
                bubble.pingError = false;
                haveTheFlag = false;
                //old code 20091226
                //camera.pubFrame.Save(bubble.imageFolder + fName, ImageFormat.Jpeg);
                //old code 20091226
            }
            catch (Exception)
            {
                haveTheFlag = false;
                bubble.pingError = true;
                bubble.logAddLine("Error in saving image: " + fName);
            }
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }





        //****************************************************************************
        //****************************************************************************
        //****************************************************************************
        //****************************************************************************

        private void mouse_Click(object sender, MouseEventArgs e)
        {

            selectingArea = true;

            if (e.Button == MouseButtons.Left)
            {
                Control source = (Control)sender;
                Point screen = source.PointToScreen(new Point(e.X, e.Y));
                ClickPoint = PointToClient(screen);

                SetClickAction(ClickPoint);
                LeftButtonDown = true;

                if (RectangleDrawn)
                {
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    DragClickRelative.X = ClickPoint.X - CurrentTopLeft.X;
                    DragClickRelative.Y = ClickPoint.Y - CurrentTopLeft.Y;
                }

            }

        }

        private void mouse_Move(object sender, MouseEventArgs e)
        {


            bool draw = false;
            bool drag = false;
            bool resize = false;

            Control source = (Control)sender;
            Point screen = source.PointToScreen(new Point(e.X, e.Y));
            Point ClickPoint = PointToClient(screen);



            if (selectionOn) { CursorPosition(ClickPoint); }
            else { this.Cursor = Cursors.Arrow; }


            if (selectingArea && selectionOn)
            {


                if (ClickPoint.X > source.Width)
                {
                    Cursor.Position = new Point(Cursor.Position.X - 1, Cursor.Position.Y);
                }
                if (ClickPoint.X < 0)
                {
                    Cursor.Position = new Point(Cursor.Position.X + 1, Cursor.Position.Y);
                }


                if (ClickPoint.Y > source.Height)
                {
                    Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y - 1);
                }
                if (ClickPoint.Y < 0)
                {
                    Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y + 1);
                }



                if (LeftButtonDown && !RectangleDrawn) { draw = true; }

                if (RectangleDrawn)
                {
                    if (CurrentAction == ClickAction.Dragging) { drag = true; draw = false; }
                    if (CurrentAction != ClickAction.Dragging && CurrentAction != ClickAction.Outside) { resize = true; drag = false; draw = false; }
                }

                if (draw || drag || resize)
                {
                    source.Refresh();
                    if (draw) { DrawSelection(ClickPoint); }
                    if (drag) { DragSelection(ClickPoint, source.Width, source.Height); }
                    if (resize) { ResizeSelection(ClickPoint, source.Width, source.Height); }
                }

            }
            else
            {
                if (rectDrawn)
                {
                    source.Refresh();




                    drawRect
                    (
                    CameraRig.rig[CameraRig.drawCam].cam.MotionDetector.rectX,
                    CameraRig.rig[CameraRig.drawCam].cam.MotionDetector.rectY,
                    CameraRig.rig[CameraRig.drawCam].cam.MotionDetector.rectWidth,
                    CameraRig.rig[CameraRig.drawCam].cam.MotionDetector.rectHeight
                    );



                    // drawRect((int)CameraRig.rigInfoGet(bubble.profileInUse, "rectX"), (int)CameraRig.rigInfoGet(bubble.profileInUse, "rectY"), (int)CameraRig.rigInfoGet(bubble.profileInUse, "rectWidth"), (int)CameraRig.rigInfoGet(bubble.profileInUse, "rectHeight")); 
                    //drawRect(config.getProfile(bubble.profileInUse).rectX, config.getProfile(bubble.profileInUse).rectY, config.getProfile(bubble.profileInUse).rectWidth, config.getProfile(bubble.profileInUse).rectHeight);
                    rectDrawn = false;
                }

            }

        }

        private void mouse_Up(object sender, MouseEventArgs e)
        {
            selectingArea = false;
            RectangleDrawn = true;
            LeftButtonDown = false;
            CurrentAction = ClickAction.NoClick;

        }

        public void drawRectOnOpen()
        {
            selectionOn = true;
            selectingArea = false;
            RectangleDrawn = true;
            LeftButtonDown = false;
            CurrentAction = ClickAction.NoClick;



            CurrentTopLeft.X = CameraRig.rig[CameraRig.drawCam].cam.MotionDetector.rectX;
            CurrentTopLeft.Y = CameraRig.rig[CameraRig.drawCam].cam.MotionDetector.rectY;
            CurrentBottomRight.X = CameraRig.rig[CameraRig.drawCam].cam.MotionDetector.rectX + CameraRig.rig[CameraRig.drawCam].cam.MotionDetector.rectWidth;
            CurrentBottomRight.Y = CameraRig.rig[CameraRig.drawCam].cam.MotionDetector.rectY + CameraRig.rig[CameraRig.drawCam].cam.MotionDetector.rectHeight;


            //CurrentTopLeft.X = (int)CameraRig.rigInfoGet(bubble.profileInUse, "rectX");
            //CurrentTopLeft.Y = (int)CameraRig.rigInfoGet(bubble.profileInUse, "rectY");
            //CurrentBottomRight.X = (int)CameraRig.rigInfoGet(bubble.profileInUse, "rectX") + (int)CameraRig.rigInfoGet(bubble.profileInUse, "rectWidth");
            //CurrentBottomRight.Y = (int)CameraRig.rigInfoGet(bubble.profileInUse, "rectY") + (int)CameraRig.rigInfoGet(bubble.profileInUse, "rectHeight");

            //CurrentTopLeft.X = config.getProfile(bubble.profileInUse).rectX;
            //CurrentTopLeft.Y = config.getProfile(bubble.profileInUse).rectY;
            //CurrentBottomRight.X = config.getProfile(bubble.profileInUse).rectX + config.getProfile(bubble.profileInUse).rectWidth;
            //CurrentBottomRight.Y = config.getProfile(bubble.profileInUse).rectY + config.getProfile(bubble.profileInUse).rectHeight;


            rectDrawn = true;
            this.Invalidate();


        }

        private void DragSelection(Point cursor, int width, int height)
        {

            //Ensure that the rectangle stays within the bounds of the screen

            Graphics g = this.CreateGraphics();

            if (cursor.X - DragClickRelative.X > 0 && cursor.X - DragClickRelative.X + RectangleWidth < width)
            {
                CurrentTopLeft.X = cursor.X - DragClickRelative.X;
                CurrentBottomRight.X = CurrentTopLeft.X + RectangleWidth;
            }
            else
                //Selection area has reached the right side of the screen
                if (cursor.X - DragClickRelative.X > 0)
                {
                    CurrentTopLeft.X = width - RectangleWidth;
                    CurrentBottomRight.X = CurrentTopLeft.X + RectangleWidth;
                }
                //Selection area has reached the left side of the screen
                else
                {
                    CurrentTopLeft.X = 0;
                    CurrentBottomRight.X = CurrentTopLeft.X + RectangleWidth;
                }

            if (cursor.Y - DragClickRelative.Y > 0 && cursor.Y - DragClickRelative.Y + RectangleHeight < height)
            {
                CurrentTopLeft.Y = cursor.Y - DragClickRelative.Y;
                CurrentBottomRight.Y = CurrentTopLeft.Y + RectangleHeight;
            }
            else
                //Selection area has reached the bottom of the screen
                if (cursor.Y - DragClickRelative.Y > 0)
                {
                    CurrentTopLeft.Y = height - RectangleHeight;
                    CurrentBottomRight.Y = CurrentTopLeft.Y + RectangleHeight;
                }
                //Selection area has reached the top of the screen
                else
                {
                    CurrentTopLeft.Y = 0;
                    CurrentBottomRight.Y = CurrentTopLeft.Y + RectangleHeight;
                }

            drawRect(CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);

        }

        private void DrawSelection(Point cursor)
        {

            this.Cursor = Cursors.Cross;

            //Calculate X Coordinates
            if (cursor.X < ClickPoint.X)
            {
                CurrentTopLeft.X = cursor.X;
                CurrentBottomRight.X = ClickPoint.X;
            }
            else
            {
                CurrentTopLeft.X = ClickPoint.X;
                CurrentBottomRight.X = cursor.X;
            }

            //Calculate Y Coordinates
            if (cursor.Y < ClickPoint.Y)
            {
                CurrentTopLeft.Y = cursor.Y;
                CurrentBottomRight.Y = ClickPoint.Y;
            }
            else
            {
                CurrentTopLeft.Y = ClickPoint.Y;
                CurrentBottomRight.Y = cursor.Y;
            }

            drawRect(CurrentTopLeft.X, CurrentTopLeft.Y, CurrentBottomRight.X - CurrentTopLeft.X, CurrentBottomRight.Y - CurrentTopLeft.Y);

        }

        private void drawInitialRectangle(object sender, drawArgs e)
        {

            drawRect(e.x, e.y, e.width, e.height);


            //drawRect((int)CameraRig.rigInfoGet(bubble.profileInUse, "rectX"), (int)CameraRig.rigInfoGet(bubble.profileInUse, "rectY"), (int)CameraRig.rigInfoGet(bubble.profileInUse, "rectWidth"), (int)CameraRig.rigInfoGet(bubble.profileInUse, "rectHeight"));
            //drawRect(config.getProfile(bubble.profileInUse).rectX, config.getProfile(bubble.profileInUse).rectY, config.getProfile(bubble.profileInUse).rectWidth, config.getProfile(bubble.profileInUse).rectHeight);

        }

        private void drawRect(int topLeftX, int topLeftY, int width, int height)
        {

            try
            {
                Graphics g = this.CreateGraphics();

                //if (topLeftY + height > CameraRig.rig[CameraRig.activeCam].cam.pubFrame.Height) { height = CameraRig.rig[CameraRig.activeCam].cam.pubFrame.Height - topLeftY; }
                if (topLeftY + height > camera.pubFrame.Height) { height = camera.pubFrame.Height - topLeftY; }
                if (height < 1) { height = 1; }
                if (topLeftY < 0) { topLeftY = 0; }
                //if (topLeftY >= CameraRig.rig[CameraRig.activeCam].cam.pubFrame.Height)
                if (topLeftY >= camera.pubFrame.Height)
                {
                    topLeftY = camera.pubFrame.Height - 1;
                    height = 1;
                }


                //if (topLeftX + width > CameraRig.rig[CameraRig.activeCam].cam.pubFrame.Width) { width = CameraRig.rig[CameraRig.activeCam].cam.pubFrame.Width - topLeftX; }
                if (topLeftX + width > camera.pubFrame.Width) { width = camera.pubFrame.Width - topLeftX; }
                if (width < 1) { width = 1; }
                if (topLeftX < 0) { topLeftX = 0; }
                //if (topLeftX >= CameraRig.rig[CameraRig.activeCam].cam.pubFrame.Width)
                if (topLeftX >= camera.pubFrame.Width)
                {
                    //topLeftX = CameraRig.rig[CameraRig.activeCam].cam.pubFrame.Width - 1;
                    topLeftX = camera.pubFrame.Width - 1;
                    width = 1;
                }

                g.DrawRectangle(MyPen, topLeftX, topLeftY, width, height);
                g.Dispose();


                config.getProfile(bubble.profileInUse).rectX = topLeftX;
                config.getProfile(bubble.profileInUse).rectY = topLeftY;
                config.getProfile(bubble.profileInUse).rectWidth = width;
                config.getProfile(bubble.profileInUse).rectHeight = height;

                CameraRig.rig[CameraRig.drawCam].cam.MotionDetector.rectX = topLeftX;
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[CameraRig.drawCam].cameraName, "rectX", topLeftX);
                CameraRig.rig[CameraRig.drawCam].cam.MotionDetector.rectY = topLeftY;
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[CameraRig.drawCam].cameraName, "rectY", topLeftY);
                CameraRig.rig[CameraRig.drawCam].cam.MotionDetector.rectWidth = width;
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[CameraRig.drawCam].cameraName, "rectWidth", width);
                CameraRig.rig[CameraRig.drawCam].cam.MotionDetector.rectHeight = height;
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[CameraRig.drawCam].cameraName, "rectHeight", height);

                CameraRig.rig[CameraRig.drawCam].cam.Lock();
                CameraRig.rig[CameraRig.drawCam].cam.MotionDetector.Reset();
                CameraRig.rig[CameraRig.drawCam].cam.Unlock();

                //System.Diagnostics.Debug.WriteLine(CameraRig.drawCam);


            }
            catch { }

        }




        private void SetClickAction(Point cursor)
        {
            if (CursorPosition(cursor) == CursPos.BottomLine)
            {
                CurrentAction = ClickAction.BottomSizing;
            }
            if (CursorPosition(cursor) == CursPos.TopLine)
            {
                CurrentAction = ClickAction.TopSizing;
            }
            if (CursorPosition(cursor) == CursPos.LeftLine)
            {
                CurrentAction = ClickAction.LeftSizing;
            }
            if (CursorPosition(cursor) == CursPos.TopLeft)
            {
                CurrentAction = ClickAction.TopLeftSizing;
            }
            if (CursorPosition(cursor) == CursPos.BottomLeft)
            {
                CurrentAction = ClickAction.BottomLeftSizing;
            }
            if (CursorPosition(cursor) == CursPos.RightLine)
            {
                CurrentAction = ClickAction.RightSizing;
            }
            if (CursorPosition(cursor) == CursPos.TopRight)
            {
                CurrentAction = ClickAction.TopRightSizing;
            }
            if (CursorPosition(cursor) == CursPos.BottomRight)
            {
                CurrentAction = ClickAction.BottomRightSizing;
            }
            if (CursorPosition(cursor) == CursPos.WithinSelectionArea)
            {
                CurrentAction = ClickAction.Dragging;
            }
            if (CursorPosition(cursor) == CursPos.OutsideSelectionArea)
            {
                CurrentAction = ClickAction.Outside;
            }
        }

        private void ResizeSelection(Point cursor, int width, int height)
        {
            //camera.Unlock();

            Graphics g = this.CreateGraphics();

            if (CurrentAction == ClickAction.LeftSizing)
            {
                if (cursor.X < CurrentBottomRight.X - 10)
                {
                    //Erase the previous rectangle
                    CurrentTopLeft.X = cursor.X;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    drawRect(CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                }
            }
            if (CurrentAction == ClickAction.TopLeftSizing)
            {
                if (cursor.X < CurrentBottomRight.X - 10 && cursor.Y < CurrentBottomRight.Y - 10)
                {
                    //Erase the previous rectangle
                    CurrentTopLeft.X = cursor.X;
                    CurrentTopLeft.Y = cursor.Y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    drawRect(CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                }
            }
            if (CurrentAction == ClickAction.BottomLeftSizing)
            {
                if (cursor.X < CurrentBottomRight.X - 10 && cursor.Y > CurrentTopLeft.Y + 10)
                {
                    //Erase the previous rectangle
                    CurrentTopLeft.X = cursor.X;
                    CurrentBottomRight.Y = cursor.Y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    //g.DrawRectangle(MyPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    drawRect(CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                }
            }
            if (CurrentAction == ClickAction.RightSizing)
            {
                if (cursor.X > CurrentTopLeft.X + 10)
                {
                    //Erase the previous rectangle
                    CurrentBottomRight.X = cursor.X;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    drawRect(CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                }
            }
            if (CurrentAction == ClickAction.TopRightSizing)
            {
                if (cursor.X > CurrentTopLeft.X + 10 && cursor.Y < CurrentBottomRight.Y - 10)
                {
                    //Erase the previous rectangle
                    CurrentBottomRight.X = cursor.X;
                    CurrentTopLeft.Y = cursor.Y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    drawRect(CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                }
            }
            if (CurrentAction == ClickAction.BottomRightSizing)
            {
                if (cursor.X > CurrentTopLeft.X + 10 && cursor.Y > CurrentTopLeft.Y + 10)
                {
                    //Erase the previous rectangle
                    CurrentBottomRight.X = cursor.X;
                    CurrentBottomRight.Y = cursor.Y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    //g.DrawRectangle(MyPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    drawRect(CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                }
            }
            if (CurrentAction == ClickAction.TopSizing)
            {
                if (cursor.Y < CurrentBottomRight.Y - 10)
                {
                    //Erase the previous rectangle
                    CurrentTopLeft.Y = cursor.Y;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    drawRect(CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                }
            }
            if (CurrentAction == ClickAction.BottomSizing)
            {
                if (cursor.Y > CurrentTopLeft.Y + 10)
                {
                    //Erase the previous rectangle
                    CurrentBottomRight.Y = cursor.Y;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    drawRect(CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                }
            }


        }

        private CursPos CursorPosition(Point cursor)
        {
            if (((cursor.X > CurrentTopLeft.X - 10 && cursor.X < CurrentTopLeft.X + 10)) && ((cursor.Y > CurrentTopLeft.Y + 10) && (cursor.Y < CurrentBottomRight.Y - 10)))
            {
                this.Cursor = Cursors.SizeWE;
                return CursPos.LeftLine;
            }
            if (((cursor.X >= CurrentTopLeft.X - 10 && cursor.X <= CurrentTopLeft.X + 10)) && ((cursor.Y >= CurrentTopLeft.Y - 10) && (cursor.Y <= CurrentTopLeft.Y + 10)))
            {
                this.Cursor = Cursors.SizeNWSE;
                return CursPos.TopLeft;
            }
            if (((cursor.X >= CurrentTopLeft.X - 10 && cursor.X <= CurrentTopLeft.X + 10)) && ((cursor.Y >= CurrentBottomRight.Y - 10) && (cursor.Y <= CurrentBottomRight.Y + 10)))
            {
                this.Cursor = Cursors.SizeNESW;
                return CursPos.BottomLeft;
            }
            if (((cursor.X > CurrentBottomRight.X - 10 && cursor.X < CurrentBottomRight.X + 10)) && ((cursor.Y > CurrentTopLeft.Y + 10) && (cursor.Y < CurrentBottomRight.Y - 10)))
            {
                this.Cursor = Cursors.SizeWE;
                return CursPos.RightLine;
            }
            if (((cursor.X >= CurrentBottomRight.X - 10 && cursor.X <= CurrentBottomRight.X + 10)) && ((cursor.Y >= CurrentTopLeft.Y - 10) && (cursor.Y <= CurrentTopLeft.Y + 10)))
            {
                this.Cursor = Cursors.SizeNESW;
                return CursPos.TopRight;
            }
            if (((cursor.X >= CurrentBottomRight.X - 10 && cursor.X <= CurrentBottomRight.X + 10)) && ((cursor.Y >= CurrentBottomRight.Y - 10) && (cursor.Y <= CurrentBottomRight.Y + 10)))
            {
                this.Cursor = Cursors.SizeNWSE;
                return CursPos.BottomRight;
            }
            if (((cursor.Y > CurrentTopLeft.Y - 10) && (cursor.Y < CurrentTopLeft.Y + 10)) && ((cursor.X > CurrentTopLeft.X + 10 && cursor.X < CurrentBottomRight.X - 10)))
            {
                this.Cursor = Cursors.SizeNS;
                return CursPos.TopLine;
            }
            if (((cursor.Y > CurrentBottomRight.Y - 10) && (cursor.Y < CurrentBottomRight.Y + 10)) && ((cursor.X > CurrentTopLeft.X + 10 && cursor.X < CurrentBottomRight.X - 10)))
            {
                this.Cursor = Cursors.SizeNS;
                return CursPos.BottomLine;
            }
            if (
                (cursor.X >= CurrentTopLeft.X + 10 && cursor.X <= CurrentBottomRight.X - 10) && (cursor.Y >= CurrentTopLeft.Y + 10 && cursor.Y <= CurrentBottomRight.Y - 10))
            {
                this.Cursor = Cursors.Hand;
                return CursPos.WithinSelectionArea;
            }
            this.Cursor = Cursors.Arrow;
            return CursPos.OutsideSelectionArea;


        }





    }


    public delegate void ImageSavedEventHandler(object source, ImageSavedArgs e);

    public class ImageSavedArgs : EventArgs
    {
        private string _image;

        public string image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
            }
        }
    }



}
