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
        public Ping ping;
        public bool MotionDisplay = false;
        private bool autosizeCameraWindow = true;
        private bool needSizeUpdate = false;
        private bool firstFrame = true;

        public bool imageToFrame = false;
        public bool showCam = true;

        //private System.Timers.Timer timer;
        private Color rectColor = Color.Black;

        //public static IException TebocamState.tebowebException;

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
        public bool AutoSizeCameraWindow
        {
            get { return autosizeCameraWindow; }
            set
            {
                autosizeCameraWindow = value;
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

                }

                camera = value;
                needSizeUpdate = true;
                firstFrame = true;
                //flash = 0;


                // attach event
                if (camera != null)
                {

                    camera.NewFrame += new EventHandler(camera_NewFrame);
                    ping.takePingPicture -= new EventHandler(take_ping_picture);
                    ping.takePingPicture += new EventHandler(take_ping_picture);
                    webcamConfig.drawInitialRectangle -= new drawEventHandler(drawInitialRectangle);
                    webcamConfig.drawInitialRectangle += new drawEventHandler(drawInitialRectangle);

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
            ///Monitor.Enter(this);

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

                    camera.Lock();

                    //Bitmap toDisplay = this.MotionDisplay ? camera.motionFrame : camera.pubFrame;
                    Bitmap toDisplay = camera.pubFrame;

                    // draw frame
                    if (camera.pubFrame != null)
                    {
                        if (showCam)
                        {
                            //newcode
                            if (imageToFrame)
                            {
                                g.DrawRectangle(pen, toDisplay.Width, toDisplay.Height, rc.Width, rc.Height);
                                g.DrawImage(ImageProcessor.ResizeImage(toDisplay, 320, 240, true), rc.X + 1, rc.Y + 1, rc.Width - 2, rc.Height - 2);
                            }
                            else
                            {
                                g.DrawImage(toDisplay, rc.X + 1, rc.Y + 1, rc.Width - 2, rc.Height - 2);
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
                catch (Exception e)
                {
                    //TebocamState.tebowebException.LogException(e);
                    camera.Unlock();
                }
                finally
                {

                    camera.Unlock();

                }
            }

            pen.Dispose();

            // unlock
            //Monitor.Exit(this);
            base.OnPaint(pe);
        }


        //private static Size resizeImagesize(Size fromSize, Size toSize)
        //{
        //    float nPercent = 0;
        //    float nPercentW = 0;
        //    float nPercentH = 0;

        //    nPercentW = ((float)toSize.Width / (float)fromSize.Width);
        //    nPercentH = ((float)toSize.Height / (float)fromSize.Height);

        //    if (nPercentH < nPercentW)
        //        nPercent = nPercentH;
        //    else
        //        nPercent = nPercentW;

        //    int destWidth = (int)(fromSize.Width * nPercent);
        //    int destHeight = (int)(fromSize.Height * nPercent);

        //    Size tmpSize = new Size();
        //    tmpSize.Width = destWidth;
        //    tmpSize.Height = destHeight;
        //    return tmpSize;

        //}




        // Update position and size of the control
        public void UpdatePosition()
        {

            try
            {
                // lock
                Monitor.Enter(this);

                if (autosizeCameraWindow && this.Parent != null && camera != null && camera.pubFrame != null)
                {
                    Rectangle rc = this.Parent.ClientRectangle;

                    int width = camera.pubFrame.Width;
                    int height = camera.pubFrame.Height;

                    if (camera != null)
                    {

                        camera.Lock();

                        // get frame dimension
                        if (camera.pubFrame != null)
                        {

                            width = camera.pubFrame.Width;
                            height = camera.pubFrame.Height;

                        }

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
            catch (Exception e)
            {
                //TebocamState.tebowebException.LogException(e);
                Monitor.Exit(this);
            }
        }

        // On new frame ready
        private void camera_NewFrame(object sender, System.EventArgs e)
        {
            Invalidate();
        }

        private void take_ping_picture(object sender, System.EventArgs e)
        {

            haveTheFlag = true;


            string fName = "pingPicture.jpg";
            Bitmap saveBmp = null;
            try
            {

                List<string> lst = new List<string>();

                if (ConfigurationHelper.GetCurrentProfile().pingStatsStamp)
                {
                    statistics.movementResults stats = new statistics.movementResults();
                    stats = statistics.statsForCam(CameraRig.CurrentlyDisplayingCamera, ConfigurationHelper.GetCurrentProfileName(), "Ping", CameraRig.ConnectedCameras[CameraRig.CurrentlyDisplayingCamera].cameraName);
                    lst.Add(stats.avgMvStart.ToString());
                    lst.Add(stats.avgMvLast.ToString());
                    lst.Add(stats.mvNow.ToString());
                    lst.Add(ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), CameraRig.ConnectedCameras[CameraRig.CurrentlyDisplayingCamera].cameraName).alarmActive ? "On" : "Off");
                    lst.Add(ConfigurationHelper.GetCurrentProfile().pingInterval.ToString() + " Mins");
                }

                var stampArgs = new Movement.imageText();

                if (ConfigurationHelper.GetCurrentProfile().pingAll)
                {
                    //mosaic mos = new mosaic();
                    int imgHeight = 0;
                    int imgWidth = 0;

                    //#todo there may be issue here if an image is not present
                    //set the height and width to the largest image
                    foreach (ConnectedCamera item in CameraRig.ConnectedCameras)
                    {
                        if (item.camera.pubFrame.Height > imgHeight || item.camera.pubFrame.Width > imgWidth)
                        {
                            imgHeight = item.camera.pubFrame.Height;
                            imgWidth = item.camera.pubFrame.Width;
                        }
                    }

                    foreach (ConnectedCamera item in CameraRig.ConnectedCameras)
                    {
                        ImageProcessor.mosaic.addToList(ImageProcessor.ResizeImage((Bitmap)item.camera.pubFrame.Clone(), imgWidth, imgHeight, false));
                    }

                    stampArgs.bitmap = (Bitmap)ImageProcessor.mosaic.getMosaicBitmap(4).Clone();
                }
                else//if (config.GetCurrentProfile().pingStatsStamp)
                {
                    stampArgs.bitmap = (Bitmap)camera.pubFrame.Clone();
                }


                stampArgs.type = "Ping";
                stampArgs.position = ConfigurationHelper.GetCurrentProfile().pingTimeStampPosition;
                stampArgs.format = ConfigurationHelper.GetCurrentProfile().pingTimeStampFormat;
                stampArgs.colour = ConfigurationHelper.GetCurrentProfile().pingTimeStampColour;
                stampArgs.backingRectangle = ConfigurationHelper.GetCurrentProfile().pingTimeStampRect;
                stampArgs.stats = lst;

                saveBmp = ConfigurationHelper.GetCurrentProfile().pingTimeStamp ? ImageProcessor.TimeStampImage(stampArgs) : stampArgs.bitmap;

                ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, ConfigurationHelper.GetCurrentProfile().pingCompression);
                myEncoderParameters.Param[0] = myEncoderParameter;
                saveBmp.Save(TebocamState.tmpFolder + fName, jgpEncoder, myEncoderParameters);

                Bitmap thumb = ImageProcessor.GetThumb(saveBmp);
                thumb.Save(TebocamState.tmpFolder + TebocamState.tmbPrefix + fName, ImageFormat.Jpeg);

                saveBmp.Dispose();
                thumb.Dispose();
                TebocamState.log.AddLine("Image saved: " + fName);
                //bubble.pingError = false;
                haveTheFlag = false;

            }
            catch (Exception ex)
            {
                //TebocamState.tebowebException.LogException(ex);
                haveTheFlag = false;
                //bubble.pingError = true;
                TebocamState.log.AddLine("Error in saving image: " + fName);
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
                    CameraRig.ConnectedCameras[CameraRig.ConfigCam].camera.rectX,
                    CameraRig.ConnectedCameras[CameraRig.ConfigCam].camera.rectY,
                    CameraRig.ConnectedCameras[CameraRig.ConfigCam].camera.rectWidth,
                    CameraRig.ConnectedCameras[CameraRig.ConfigCam].camera.rectHeight
                    );




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



            CurrentTopLeft.X = CameraRig.ConnectedCameras[CameraRig.ConfigCam].camera.rectX;
            CurrentTopLeft.Y = CameraRig.ConnectedCameras[CameraRig.ConfigCam].camera.rectY;
            CurrentBottomRight.X = CameraRig.ConnectedCameras[CameraRig.ConfigCam].camera.rectX + CameraRig.ConnectedCameras[CameraRig.ConfigCam].camera.rectWidth;
            CurrentBottomRight.Y = CameraRig.ConnectedCameras[CameraRig.ConfigCam].camera.rectY + CameraRig.ConnectedCameras[CameraRig.ConfigCam].camera.rectHeight;


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

        }

        private void drawRect(int topLeftX, int topLeftY, int width, int height)
        {

            try
            {
                Graphics g = this.CreateGraphics();

                if (topLeftY + height > camera.pubFrame.Height)
                {
                    height = camera.pubFrame.Height - topLeftY;
                }
                if (height < 1)
                {
                    height = 1;
                }
                if (topLeftY < 0)
                {
                    topLeftY = 0;
                }

                if (topLeftY >= camera.pubFrame.Height)
                {
                    topLeftY = camera.pubFrame.Height - 1;
                    height = 1;
                }


                if (topLeftX + width > camera.pubFrame.Width)
                {
                    width = camera.pubFrame.Width - topLeftX;
                }
                if (width < 1)
                {
                    width = 1;
                }
                if (topLeftX < 0)
                {
                    topLeftX = 0;
                }

                if (topLeftX >= camera.pubFrame.Width)
                {

                    topLeftX = camera.pubFrame.Width - 1;
                    width = 1;
                }

                g.DrawRectangle(MyPen, topLeftX, topLeftY, width, height);
                g.Dispose();


                ConfigurationHelper.GetCurrentProfile().rectX = topLeftX;
                ConfigurationHelper.GetCurrentProfile().rectY = topLeftY;
                ConfigurationHelper.GetCurrentProfile().rectWidth = width;
                ConfigurationHelper.GetCurrentProfile().rectHeight = height;

                CameraRig.ConnectedCameras[CameraRig.ConfigCam].camera.rectX = topLeftX;
                ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(),
                    CameraRig.ConnectedCameras[CameraRig.ConfigCam].cameraName).rectX = topLeftX;
                CameraRig.ConnectedCameras[CameraRig.ConfigCam].camera.rectY = topLeftY;
                ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(),
                    CameraRig.ConnectedCameras[CameraRig.ConfigCam].cameraName).rectY = topLeftY;
                CameraRig.ConnectedCameras[CameraRig.ConfigCam].camera.rectWidth = width;
                ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(),
                    CameraRig.ConnectedCameras[CameraRig.ConfigCam].cameraName).rectWidth = width;
                CameraRig.ConnectedCameras[CameraRig.ConfigCam].camera.rectHeight = height;
                ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(),
                    CameraRig.ConnectedCameras[CameraRig.ConfigCam].cameraName).rectHeight = height;

                CameraRig.ConnectedCameras[CameraRig.ConfigCam].camera.Lock();
                CameraRig.ConnectedCameras[CameraRig.ConfigCam].camera.MotionDetector.Reset();
                CameraRig.ConnectedCameras[CameraRig.ConfigCam].camera.Unlock();


            }
            catch (Exception e)
            {
                //TebocamState.tebowebException.LogException(e);
            }
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

