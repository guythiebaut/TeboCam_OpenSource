using System;
using System.Drawing;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class LevelControl : UserControl
    {

        private static System.Drawing.Bitmap levelBitmap;
        public IException tebowebException;

        public LevelControl()
        {

            InitializeComponent();
            levelBitmap = new Bitmap(levelbox.Size.Width, levelbox.Size.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

        }


        public void levelDraw(int val)
        {

            int sensePerc = 0;
            int lineStartX = 0;
            int lineStartY = 0;
            int lineLen = 0;
            int lineWid = 0;
            double onePct = 0;
            int greenLen = 0;
            int orangeLen = 0;
            int greenStart = 0;
            int orangeStart = 0;


            if (CameraRig.camerasAreConnected())
            {
                sensePerc = (int)Math.Floor(CameraRig.ConnectedCameras[CameraRig.CurrentlyDisplayingCamera].camera.movementVal * (double)100);
            }
            else
            {
                sensePerc = 100;
            }

            lineLen = levelbox.Size.Height;
            lineWid = levelbox.Size.Width;
            onePct = (double)lineLen / (double)100;
            greenLen = (int)Math.Floor((double)val * onePct);
            orangeLen = (int)Math.Floor(((double)100 - (double)val) * onePct);
            greenStart = (int)Math.Floor(((double)100 - (double)val) * onePct);
            orangeStart = (100 - val);

            System.Drawing.SolidBrush controlBrush = new System.Drawing.SolidBrush(System.Drawing.SystemColors.Control);
            System.Drawing.SolidBrush greenBrush = new System.Drawing.SolidBrush(System.Drawing.Color.GreenYellow);
            System.Drawing.SolidBrush orangeBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);

            if (levelBitmap != null)
            {

                lock (levelBitmap)
                {

                    levelBitmap = new Bitmap(lineWid, lineLen, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    Graphics levelObj = Graphics.FromImage(levelBitmap);



                    levelObj.FillRectangle(controlBrush, new Rectangle(lineStartX, lineStartY, lineWid, lineLen));

                    if (val > sensePerc)
                    {
                        greenStart = (int)Math.Floor(((double)100 - (double)sensePerc) * onePct);
                        greenLen = (int)Math.Floor((double)sensePerc * onePct);
                        orangeStart = ((int)Math.Floor(((double)100 - (double)val) * onePct));
                        orangeLen = (int)Math.Floor(((double)val - (double)sensePerc) * onePct);
                        levelObj.FillRectangle(greenBrush, new Rectangle(lineStartX, greenStart, lineWid, greenLen));
                        levelObj.FillRectangle(orangeBrush, new Rectangle(lineStartX, orangeStart, lineWid, orangeLen));
                    }
                    else
                    {
                        greenStart = (int)Math.Floor(((double)100 - (double)val) * onePct);
                        greenLen = (int)Math.Floor((double)val * onePct);
                        levelObj.FillRectangle(greenBrush, new Rectangle(lineStartX, greenStart, lineWid, greenLen));
                    }

                    controlBrush.Dispose();
                    greenBrush.Dispose();
                    orangeBrush.Dispose();
                    levelObj.Dispose();
                    levelbox.Invalidate();

                }
            }

        }



        public void levelDraw(int val, double sensitivity)
        {

            int sensePerc = 0;
            int lineStartX = 0;
            int lineStartY = 0;
            int lineLen = 0;
            int lineWid = 0;
            double onePct = 0;
            int greenLen = 0;
            int orangeLen = 0;
            int greenStart = 0;
            int orangeStart = 0;


            try
            {

                sensePerc = (int)Math.Floor(sensitivity);
                lineLen = levelbox.Size.Height;
                lineWid = levelbox.Size.Width;
                onePct = (double)lineLen / (double)100;
                greenLen = (int)Math.Floor((double)val * onePct);
                orangeLen = (int)Math.Floor(((double)100 - (double)val) * onePct);
                greenStart = (int)Math.Floor(((double)100 - (double)val) * onePct);
                orangeStart = (100 - val);

                System.Drawing.SolidBrush controlBrush = new System.Drawing.SolidBrush(System.Drawing.SystemColors.Control);
                System.Drawing.SolidBrush greenBrush = new System.Drawing.SolidBrush(System.Drawing.Color.GreenYellow);
                System.Drawing.SolidBrush orangeBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);

                lock (levelBitmap)
                {

                    levelBitmap = new Bitmap(lineWid, lineLen, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    Graphics levelObj = Graphics.FromImage(levelBitmap);

                    levelObj.FillRectangle(controlBrush, new Rectangle(lineStartX, lineStartY, lineWid, lineLen));

                    if (val > sensePerc)
                    {
                        greenStart = (int)Math.Floor(((double)100 - (double)sensePerc) * onePct);
                        greenLen = (int)Math.Floor((double)sensePerc * onePct);
                        orangeStart = ((int)Math.Floor(((double)100 - (double)val) * onePct));
                        orangeLen = (int)Math.Floor(((double)val - (double)sensePerc) * onePct);
                        levelObj.FillRectangle(greenBrush, new Rectangle(lineStartX, greenStart, lineWid, greenLen));
                        levelObj.FillRectangle(orangeBrush, new Rectangle(lineStartX, orangeStart, lineWid, orangeLen));
                    }
                    else
                    {
                        greenStart = (int)Math.Floor(((double)100 - (double)val) * onePct);
                        greenLen = (int)Math.Floor((double)val * onePct);
                        levelObj.FillRectangle(greenBrush, new Rectangle(lineStartX, greenStart, lineWid, greenLen));
                    }

                    controlBrush.Dispose();
                    greenBrush.Dispose();
                    orangeBrush.Dispose();
                    levelObj.Dispose();
                    levelbox.Invalidate();

                }

            }

            catch (Exception e)
            {
                TebocamState.tebowebException.LogException(e);
                System.Diagnostics.Debug.WriteLine(e.Message);
            }


        }




        private void levelbox_Paint(object sender, PaintEventArgs e)
        {

            lock (levelBitmap)
            {

                Graphics levelObj = e.Graphics;

                if (levelBitmap != null)
                {
                    levelObj.DrawImage(levelBitmap, 0, 0, levelBitmap.Width, levelBitmap.Height);
                }

            }

        }


    }
}
