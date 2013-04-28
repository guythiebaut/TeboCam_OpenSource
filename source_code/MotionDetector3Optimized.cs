// Motion Detector
//
// Copyright © Andrew Kirillov, 2005
// andrew.kirillov@gmail.com
//
namespace TeboCam
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    using AForge.Imaging;
    using AForge.Imaging.Filters;

    /// <summary>
    /// MotionDetector3Optimized
    /// </summary>
    public class MotionDetector3Optimized : IMotionDetector
    {


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
        private int Id = 0;

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
        public int id
        {
            get { return Id; }
            set { Id = value; }
        }




        private byte[] backgroundFrame = null;
        private byte[] currentFrame = null;
        private byte[] currentFrameDilatated = null;

        private int counter = 0;

        private bool calculateMotionLevel = false;
        private int width;	// image width
        private int height;	// image height
        private int pixelsChanged;

        // Motion level calculation - calculate or not motion level
        public bool MotionLevelCalculation
        {
            get { return calculateMotionLevel; }
            set { calculateMotionLevel = value; }
        }

        // Motion level - amount of changes in percents
        public double MotionLevel
        {
            get
            {
                if ((double)pixelsChanged / (width * height) > 0)
                {
                    return (double)pixelsChanged / (width * height);
                }
                else
                {
                    return (double)0;
                }
            }
        }

        // Constructor
        public MotionDetector3Optimized()
        {
        }

        // Reset detector to initial state
        public void Reset()
        {
            backgroundFrame = null;
            currentFrame = null;
            currentFrameDilatated = null;
            counter = 0;
        }

        

        public Bitmap selectArea(Bitmap image, bool toHide, int width, int height, int topLeftX, int topLeftY)
        {

            Bitmap returnBitmap = null;
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
                    returnBitmap = (Bitmap)image.Clone();
                    graphicsObj = Graphics.FromImage((Bitmap)returnBitmap);
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
                    returnBitmap = image.Clone(rectangleObj, image.PixelFormat);
                }
                catch { }
            }

            return returnBitmap;

        }
                

        // Process new frame
        public void ProcessFrame(ref Bitmap imageIn)
        {

            Bitmap image;

            if (areaOffAtMotionTriggered && !areaOffAtMotionReset)
            {
                Reset();
                areaOffAtMotionReset = true;
            }

            if (areaDetection && !areaOffAtMotionTriggered)
            {
                image = selectArea(imageIn, AreaDetectionWithin, RectWidth, RectHeight, RectX, RectY);

                if (exposeArea)
                {
                    imageIn = image;
                }
            }
            else
            {
                image = imageIn;
            }


            // get image dimension
            width = image.Width;
            height = image.Height;

            int fW = (((width - 1) / 8) + 1);
            int fH = (((height - 1) / 8) + 1);
            int len = fW * fH;


            if (backgroundFrame == null)
            {
                // alloc memory for a backgound image and for current image
                backgroundFrame = new byte[len];
                currentFrame = new byte[len];
                currentFrameDilatated = new byte[len];

                // lock image
                BitmapData imgData = image.LockBits(
                    new Rectangle(0, 0, width, height),
                    ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                // create initial backgroung image
                PreprocessInputImage(imgData, width, height, backgroundFrame);

                // unlock the image
                image.UnlockBits(imgData);

                // just return for the first time
                return;
            }



            // lock image
            BitmapData data = image.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);


            // preprocess input image
            PreprocessInputImage(data, width, height, currentFrame);

            if (++counter == 2)
            {
                counter = 0;

                // move background towards current frame
                for (int i = 0; i < len; i++)
                {
                    int t = currentFrame[i] - backgroundFrame[i];
                    if (t > 0)
                        backgroundFrame[i]++;
                    else if (t < 0)
                        backgroundFrame[i]--;
                }
            }

            // difference and thresholding
            pixelsChanged = 0;
            for (int i = 0; i < len; i++)
            {
                int t = currentFrame[i] - backgroundFrame[i];
                if (t < 0)
                    t = -t;

                if (t >= 15)
                {
                    pixelsChanged++;
                    currentFrame[i] = (byte)255;
                }
                else
                {
                    currentFrame[i] = (byte)0;
                }
            }
            if (calculateMotionLevel)
                pixelsChanged *= 64;
            else
                pixelsChanged = 0;

            // dilatation analogue for borders extending
            // it can be skipped
            for (int i = 0; i < fH; i++)
            {
                for (int j = 0; j < fW; j++)
                {
                    int k = i * fW + j;
                    int v = currentFrame[k];

                    // left pixels
                    if (j > 0)
                    {
                        v += currentFrame[k - 1];

                        if (i > 0)
                        {
                            v += currentFrame[k - fW - 1];
                        }
                        if (i < fH - 1)
                        {
                            v += currentFrame[k + fW - 1];
                        }
                    }
                    // right pixels
                    if (j < fW - 1)
                    {
                        v += currentFrame[k + 1];

                        if (i > 0)
                        {
                            v += currentFrame[k - fW + 1];
                        }
                        if (i < fH - 1)
                        {
                            v += currentFrame[k + fW + 1];
                        }
                    }
                    // top pixel
                    if (i > 0)
                    {
                        v += currentFrame[k - fW];
                    }
                    // right pixel
                    if (i < fH - 1)
                    {
                        v += currentFrame[k + fW];
                    }

                    currentFrameDilatated[k] = (v != 0) ? (byte)255 : (byte)0;
                }


            }

            // postprocess the input image
            if (Calibrating)
            {
                PostprocessInputImage(data, width, height, currentFrameDilatated);
            }

            // unlock the image
            image.UnlockBits(data);
        }



        // Preprocess input image
        private void PreprocessInputImage(BitmapData data, int width, int height, byte[] buf)
        {
            int stride = data.Stride;
            int offset = stride - width * 3;
            int len = (int)((width - 1) / 8) + 1;
            int rem = ((width - 1) % 8) + 1;
            int[] tmp = new int[len];
            int i, j, t1, t2, k = 0;

            unsafe
            {
                byte* src = (byte*)data.Scan0.ToPointer();

                for (int y = 0; y < height; )
                {
                    // collect pixels
                    Array.Clear(tmp, 0, len);

                    // calculate
                    for (i = 0; (i < 8) && (y < height); i++, y++)
                    {
                        // for each pixel
                        for (int x = 0; x < width; x++, src += 3)
                        {
                            // grayscale value using BT709
                            tmp[(int)(x / 8)] += (int)(0.2125f * src[RGB.R] + 0.7154f * src[RGB.G] + 0.0721f * src[RGB.B]);
                        }
                        src += offset;
                    }

                    // get average values
                    t1 = i * 8;
                    t2 = i * rem;

                    for (j = 0; j < len - 1; j++, k++)
                        buf[k] = (byte)(tmp[j] / t1);
                    buf[k++] = (byte)(tmp[j] / t2);
                }
            }
        }

        // Postprocess input image
        private void PostprocessInputImage(BitmapData data, int width, int height, byte[] buf)
        {
            int stride = data.Stride;
            int offset = stride - width * 3;
            int len = (int)((width - 1) / 8) + 1;
            int lenWM1 = len - 1;
            int lenHM1 = (int)((height - 1) / 8);
            int rem = ((width - 1) % 8) + 1;

            int i, j, k;

            unsafe
            {
                byte* src = (byte*)data.Scan0.ToPointer();

                // for each line
                for (int y = 0; y < height; y++)
                {
                    i = (y / 8);

                    // for each pixel
                    for (int x = 0; x < width; x++, src += 3)
                    {
                        j = x / 8;
                        k = i * len + j;

                        // check if we need to highlight moving object
                        if (buf[k] == 255)
                        {
                            // check for border
                            if (
                                ((x % 8 == 0) && ((j == 0) || (buf[k - 1] == 0))) ||
                                ((x % 8 == 7) && ((j == lenWM1) || (buf[k + 1] == 0))) ||
                                ((y % 8 == 0) && ((i == 0) || (buf[k - len] == 0))) ||
                                ((y % 8 == 7) && ((i == lenHM1) || (buf[k + len] == 0)))
                                )
                            {

                                src[RGB.R] = 255;
                            }
                        }
                    }
                    src += offset;
                }
            }
        }
    }
}
