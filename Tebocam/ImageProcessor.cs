using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TeboCam
{
    public class ImageProcessor
    {

        //public ImageProcessor()
        //{

        //}

        public static IException tebowebException;

        public Bitmap Decorate()
        {
            return null;
        }

        public static Bitmap ResizeImage(Bitmap imgToResize, int width, int height, bool keepRatio)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)width / (float)sourceWidth);
            nPercentH = ((float)height / (float)sourceHeight);
            nPercent = nPercentH < nPercentW ? nPercentH : nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Bitmap)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Bitmap)b;
        }


        public static Bitmap TimeStampImage(Movement.imageText imageTxt)
        {

            Bitmap imageIn = imageTxt.bitmap;
            string type = imageTxt.type;
            bool backingRectangle = imageTxt.backingRectangle;

            string position = imageTxt.position;
            string format = imageTxt.format;
            string colour = imageTxt.colour;
            string formatStr = "";
            Brush textBrush = Brushes.Black;
            Brush opaqueBrush = Brushes.Black;
            int time = 70;
            int date = 80;
            int full = 150;
            int textWidth = 0;


            try
            {
                switch (format)
                {
                    case "hhmm":
                        formatStr = DateTime.Now.ToString("HH:mm:ss",
                            System.Globalization.CultureInfo.InvariantCulture);
                        textWidth = time;
                        break;
                    case "ddmmyy":
                        formatStr = DateTime.Now.ToString("dd-MMM-yy",
                            System.Globalization.CultureInfo.InvariantCulture);
                        textWidth = date;
                        break;
                    case "ddmmyyhhmm":
                        formatStr = DateTime.Now.ToString("dd-MMM-yy HH:mm:ss",
                            System.Globalization.CultureInfo.InvariantCulture);
                        textWidth = full;
                        break;
                    case "analogue":
                        formatStr = "";
                        textWidth = 0;
                        break;
                    case "analoguedate":
                        formatStr = DateTime.Now.ToString("dd-MMM-yy",
                            System.Globalization.CultureInfo.InvariantCulture);
                        ;
                        textWidth = date;
                        break;
                    default:
                        formatStr = DateTime.Now.ToString("dd-MMM-yy HH:mm:ss",
                            System.Globalization.CultureInfo.InvariantCulture);
                        textWidth = full;
                        break;
                }

                switch (colour)
                {
                    case "red":
                        textBrush = Brushes.Red;
                        opaqueBrush = Brushes.White;
                        break;
                    case "black":
                        textBrush = Brushes.Black;
                        opaqueBrush = Brushes.White;
                        break;
                    case "white":
                        textBrush = Brushes.White;
                        opaqueBrush = Brushes.Black;
                        break;
                    default:
                        textBrush = Brushes.Black;
                        opaqueBrush = Brushes.White;
                        break;
                }


                int width = imageIn.Width;
                int height = imageIn.Height;
                int x = 0;
                int y = 0;

                switch (position)
                {
                    case "tl":
                        x = 5;
                        y = 5;
                        break;
                    case "tr":
                        x = width - textWidth;
                        y = 5;
                        break;
                    case "bl":
                        x = 5;
                        y = height - 20;
                        break;
                    case "br":
                        x = width - textWidth;
                        y = height - 20;
                        break;
                    default:
                        x = 5;
                        y = 5;
                        break;
                }


                if (format == "analogue" || format == "analoguedate")
                {


                    int Xpos = 0;
                    int Ypos = 0;
                    int dtYpos = 0;
                    int dtXpos = 0;
                    int stYpos = 0;
                    int stXpos = 0;
                    int radius = 0;
                    int borderCorrection = 0;
                    int dateCorrection = 0;
                    int dateOfffset = 20;
                    int statsCorrection = 0;
                    int statsOffset = 20;

                    if (format == "analoguedate")
                    {

                        dateCorrection = -dateOfffset;
                    }

                    string stformatStr = "";

                    if (imageTxt.stats.Count > 0)
                    {

                        stformatStr = "";
                        foreach (string str in imageTxt.stats)
                        {

                            stformatStr += str + ", ";

                        }

                        //remove that last comma and space
                        stformatStr = stformatStr.Remove(stformatStr.Length - 2);

                        statsCorrection = -statsOffset;

                    }

                    Size stsize = TextRenderer.MeasureText(stformatStr, new Font("Arial", 12, FontStyle.Regular));
                    Size dtsize = TextRenderer.MeasureText(formatStr, new Font("Arial", 12, FontStyle.Regular));
                    radius = (int)(Math.Min(imageIn.Height, imageIn.Width) / 12);
                    borderCorrection = radius;


                    switch (position)
                    {

                        case "tl":
                            Xpos = borderCorrection;
                            Ypos = imageIn.Height - borderCorrection;
                            dtXpos = 2;
                            dtYpos = radius * 2 - statsCorrection;
                            stXpos = 2;
                            stYpos = radius * 2;
                            break;
                        case "tr":
                            Xpos = imageIn.Width - borderCorrection;
                            Ypos = imageIn.Height - borderCorrection;
                            dtXpos = imageIn.Width - dtsize.Width - 2;
                            dtYpos = radius * 2 - statsCorrection;
                            stXpos = imageIn.Width - stsize.Width - 2;
                            stYpos = radius * 2;
                            break;
                        case "bl":
                            Xpos = borderCorrection;
                            Ypos = borderCorrection - dateCorrection - statsCorrection;
                            dtXpos = 2;
                            dtYpos = imageIn.Height - dateOfffset - 5;
                            stXpos = 2;
                            stYpos = imageIn.Height - dateOfffset - statsOffset - 5;
                            break;
                        case "br":
                            Xpos = imageIn.Width - borderCorrection;
                            Ypos = borderCorrection - dateCorrection - statsCorrection;
                            dtXpos = imageIn.Width - dtsize.Width - 2;
                            dtYpos = imageIn.Height - dateOfffset - 5;
                            stXpos = imageIn.Width - stsize.Width - 2;
                            stYpos = imageIn.Height - dateOfffset - statsOffset - 5;
                            break;
                        default: //tr
                            Xpos = imageIn.Width - borderCorrection;
                            Ypos = imageIn.Height - borderCorrection;
                            dtXpos = imageIn.Width - dtsize.Width - 2;
                            dtYpos = radius * 2 - statsCorrection;
                            stXpos = imageIn.Width - stsize.Width - 2;
                            stYpos = radius * 2;
                            break;
                    }

                    if (format == "analoguedate")
                    {

                        Graphics graphicsObj;
                        graphicsObj = Graphics.FromImage(imageIn);

                        if (backingRectangle)
                        {

                            graphicsObj.FillRectangle(opaqueBrush, dtXpos, dtYpos, dtsize.Width, dtsize.Height);

                        }

                        graphicsObj.DrawString(formatStr, new Font("Arial", 12, FontStyle.Regular), textBrush,
                            new PointF(dtXpos, dtYpos));

                        graphicsObj.Dispose();

                    }

                    if (imageTxt.stats.Count > 0)
                    {

                        Graphics graphicsObjStats;
                        graphicsObjStats = Graphics.FromImage(imageIn);
                        graphicsObjStats.FillRectangle(opaqueBrush, stXpos, stYpos, stsize.Width, stsize.Height);
                        graphicsObjStats.DrawString(stformatStr, new Font("Arial", 12, FontStyle.Regular), textBrush,
                            stXpos, stYpos);
                        graphicsObjStats.Dispose();

                    }


                    imageIn = drawClock(imageIn,
                        Color.FromName(colour),
                        Color.FromName(colour),
                        Color.FromName(colour),
                        Color.FromName(colour),
                        Color.Black, true, false,
                        Xpos,
                        Ypos,
                        radius,
                        imageIn.Width,
                        imageIn.Height,
                        backingRectangle,
                        false,
                        opaqueBrush);

                }
                else
                {
                    Graphics graphicsObj;
                    graphicsObj = Graphics.FromImage(imageIn);

                    if (backingRectangle)
                    {

                        graphicsObj.FillRectangle(opaqueBrush, x, y, textWidth, 20);

                    }

                    graphicsObj.DrawString(formatStr, new Font("Arial", 12, FontStyle.Regular), textBrush,
                        new PointF(x, y));
                    graphicsObj.Dispose();

                    if ((type == "Publish" || type == "Ping") && imageTxt.stats.Count > 0)
                    {
                        formatStr = "";
                        foreach (string str in imageTxt.stats)
                        {
                            formatStr += str + ", ";
                        }

                        //remove that last comma and space
                        formatStr = formatStr.Remove(formatStr.Length - 2);

                        Graphics graphicsObjStats;
                        graphicsObjStats = Graphics.FromImage(imageIn);
                        graphicsObjStats.FillRectangle(opaqueBrush, x, y + 21,
                            graphicsObjStats.MeasureString(formatStr, new Font("Arial", 12, FontStyle.Regular)).Width,
                            graphicsObjStats.MeasureString(formatStr, new Font("Arial", 12, FontStyle.Regular)).Height);
                        graphicsObjStats.DrawString(formatStr, new Font("Arial", 12, FontStyle.Regular), textBrush,
                            new PointF(x, y + 21));
                        graphicsObjStats.Dispose();

                    }
                }

                return imageIn;
            }
            catch (Exception e)
            {
                TebocamState.tebowebException.LogException(e);
                return imageIn;
            }

        }


        private static Bitmap drawClock(Bitmap p_clockBitmap,
                                        Color p_hourColour,
                                        Color p_minuteColour,
                                        Color p_secondColour,
                                        Color p_tickColour,
                                        Color p_innerDotColour,
                                        bool p_Draw5MinuteTicks,
                                        bool p_Draw1MinuteTicks,
                                        int p_xStart,
                                        int p_yStart,
                                        int p_radius,
                                        int p_width,
                                        int p_height,
                                        bool p_opaque,
                                        bool p_secondHand,
                                        Brush p_opaqueBrush)
        {

            try
            {

                //float pos_correction;

                float fCenterX;
                float fCenterY;
                float fHourThickness;
                float fMinThickness;
                float fSecThickness;

                float fHourLength;
                float fMinLength;
                float fSecLength;

                float fCenterCircleRadius;
                float fTicksThickness = 1;

                DateTime dateTime;

                Graphics clockObj = Graphics.FromImage(p_clockBitmap);

                p_yStart = p_clockBitmap.Height - p_yStart;


                dateTime = DateTime.Now;


                fHourLength = ((float)p_radius * 2F) / 3F / 1.65F;
                fMinLength = ((float)p_radius * 2F) / 3F / 1.20F;
                fSecLength = ((float)p_radius * 2F) / 3F / 1.15F;
                fHourThickness = (float)p_radius * 2 / 100;
                fMinThickness = (float)p_radius * 2 / 100;
                fSecThickness = (float)p_radius * 2 / 100;
                fCenterX = (float)p_xStart;
                fCenterY = (float)p_yStart;
                fCenterCircleRadius = (fCenterY) / 150;

                clockObj.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                clockObj.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                clockObj.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                if (p_opaque)
                {

                    clockObj.FillEllipse(p_opaqueBrush, fCenterX - (p_radius / 1.40F), fCenterY - (p_radius / 1.40F), (p_radius / 1.40F) * 2F, (p_radius / 1.40F) * 2F);

                }

                clockObj.TranslateTransform(fCenterX, fCenterY);
                Matrix m = clockObj.Transform;

                clockObj.RotateTransform((dateTime.Hour % 12 + dateTime.Minute / 60F) * 30);
                DrawPolygon(fHourThickness, fHourLength, p_hourColour, clockObj);

                clockObj.Transform = m;
                clockObj.RotateTransform(dateTime.Minute * 6 + dateTime.Second / 10F);
                DrawPolygon(fMinThickness, fMinLength, p_minuteColour, clockObj);

                if (p_secondHand)
                {

                    clockObj.Transform = m;
                    clockObj.RotateTransform(dateTime.Second * 6);
                    clockObj.DrawLine(new Pen(p_secondColour, fSecThickness), 0, fSecLength / 9, 0, -fSecLength);

                }


                for (int i = 0; i < 60; i++)
                {
                    clockObj.Transform = m;
                    clockObj.RotateTransform(i * 6);
                    if (p_Draw5MinuteTicks == true && i % 5 == 0) // Draw 5 minute ticks
                    {
                        clockObj.DrawLine(new Pen(p_tickColour, fTicksThickness),
                            0, -p_radius / 1.50F,
                            0, -p_radius / 1.65F);
                    }
                    else if (p_Draw1MinuteTicks == true) // draw 1 minute ticks
                    {
                        clockObj.DrawLine(new Pen(p_tickColour, fTicksThickness),
                              0, -p_radius / 1.50F,
                              0, -p_radius / 1.55F);
                    }
                }

                clockObj.FillEllipse(new SolidBrush(p_innerDotColour), -fCenterCircleRadius / 2, -fCenterCircleRadius / 2, fCenterCircleRadius, fCenterCircleRadius);

                clockObj.Dispose();

                return p_clockBitmap;

            }
            catch (Exception e)
            {
                TebocamState.tebowebException.LogException(e);
                return p_clockBitmap;
            }

        }


        private static void DrawPolygon(float fThickness, float fLength, Color color, Graphics g)
        {

            PointF A = new PointF(fThickness * 2F, 0);
            PointF B = new PointF(-fThickness * 2F, 0);
            PointF C = new PointF(0, -fLength);
            PointF D = new PointF(0, fThickness * 4F);
            PointF[] points = { A, D, B, C };
            g.FillPolygon(new SolidBrush(color), points);

        }

        public static bool ThumbnailCallback()
        {
            return false;
        }
        public static Bitmap GetThumb(Bitmap myBitmap)
        {
            Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
            return (Bitmap)myBitmap.GetThumbnailImage(80, 80, myCallback, IntPtr.Zero);
        }

        public static void SaveBitmap(Bitmap bitmap, string filename, IException exception)
        {
            try
            {
                //ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                //System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                //EncoderParameters myEncoderParameters = new EncoderParameters(1);
                //EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50);
                //myEncoderParameters.Param[0] = myEncoderParameter;

                if (File.Exists(filename)) File.Delete(filename);
                bitmap.Save(filename);
            }
            catch (Exception e)
            {
                exception.LogException(e);
            }
        }

        public static class mosaic
        {

            private static List<Bitmap> bitmaps = new List<Bitmap>();

            public static void clearList()
            {
                bitmaps.Clear();
            }

            public static void addToList(Bitmap bitmap)
            {
                bitmaps.Add(bitmap);
            }

            public static void addToList(string path)
            {
                addToList(new Bitmap(path));
            }

            public static void saveMosaicAsJpg(int imagesPerRow, string path, int compression)
            {

                Bitmap resultBit = getMosaicBitmap(imagesPerRow);

                ImageCodecInfo jgpEncoder = ImageProcessor.GetEncoder(ImageFormat.Jpeg);
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, compression);
                myEncoderParameters.Param[0] = myEncoderParameter;

                if (File.Exists(path)) File.Delete(path);
                resultBit.Save(path, jgpEncoder, myEncoderParameters);

            }

            public static void saveMosaicAsBmp(int imagesPerRow, string path)
            {

                Bitmap resultBit = getMosaicBitmap(imagesPerRow);
                if (File.Exists(path)) File.Delete(path);
                resultBit.Save(path);

            }

            /// <summary>
            /// using a List of Bitmaps as input a Bitmap patchwork is returned 
            /// </summary>
            /// <returns>Bitmap</returns>
            public static Bitmap getMosaicBitmap(int imagesPerRow)
            {

                try
                {

                    List<Bitmap> imageItems = bitmaps;
                    int imgCount = imageItems.Count;
                    int imagesX;
                    int xCount = 1;
                    int xPos = 0;
                    int yPos = 0;

                    //let's save some image real estate if we can
                    //if there are less images than wil fit into one row - trim the row size
                    if (imgCount < imagesPerRow)
                    {
                        imagesX = imgCount;
                    }
                    else
                    {
                        imagesX = imagesPerRow;
                    }

                    //get the width and height of the images(images must have same width and height)
                    int width = imageItems[0].Width;
                    int height = imageItems[0].Height;

                    //row count is rounded down count of images divided by columns
                    int rows = (int)Math.Floor((decimal)imgCount / (decimal)imagesX);

                    //if there is a remainder in dividing the count of images by columns
                    //add an extra row to the row count
                    bool remainder = decimal.Remainder((decimal)imgCount, (decimal)imagesX) > 0m;
                    if (remainder) rows++;

                    //we now know the dimensions of the Bitmap so let's create it
                    Bitmap mosaicImage = new System.Drawing.Bitmap(imagesX * width, rows * height);

                    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mosaicImage))
                    {

                        //fill the mosaic in black first
                        g.Clear(System.Drawing.Color.Black);

                        for (int i = 0; i < imgCount; i++)
                        {

                            //iterate through images adding to mosaic
                            //images are added from let to right then down one and row left to right etc.
                            g.DrawImage(imageItems[i], new System.Drawing.Rectangle(xPos, yPos, imageItems[i].Width, imageItems[i].Height));

                            xCount++;

                            if (xCount > imagesX)
                            {
                                xPos = 0;
                                xCount = 1;
                                yPos = yPos + height;

                            }
                            else
                            {
                                xPos = xPos + width;
                            }

                        }

                        imageItems.Clear();
                        return mosaicImage;


                    }



                }

                catch (Exception e)
                {
                    TebocamState.tebowebException.LogException(e);
                    return null;
                }

            }
        }

        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            if (codecs.Any(x => x.FormatID == format.Guid))
            {
                return codecs.First(x => x.FormatID == format.Guid);
            }

            return null;
        }


    }
}
