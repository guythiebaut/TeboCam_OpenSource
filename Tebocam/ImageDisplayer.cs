using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class ImageDisplayer : UserControl
    {
        public ImageDisplayer(int p_control_width, int p_control_height, int p_image_width, int p_image_height)
        {
            InitializeComponent();

            this.Width = p_control_width;
            this.Height = p_control_width;


            int imageSeparation = 10;

            ImagePageInfo.ImagesPerRow = p_control_width / (p_image_width + imageSeparation);
            ImagePageInfo.Rows = p_control_height / (p_image_height + imageSeparation);


        }



        private void ImageDisplayer_Load(object sender, EventArgs e)
        {

        }


        public void AddImage(string p_filename, int p_level)
        {

            ImageContainer img = new ImageContainer(p_filename, p_level);
            Images.ImageContainers.Add(img);
            this.Controls.Add(Images.ImageContainers[Images.ImageContainers.Count - 1].picbox);

            Images.ImageContainers[Images.ImageContainers.Count - 1].picbox.Height = 100;
            Images.ImageContainers[Images.ImageContainers.Count - 1].picbox.Width = 100;

            if (Images.ImageContainers.Count == 1)
            {

                Images.ImageContainers[Images.ImageContainers.Count - 1].picbox.Left = 10;
                Images.ImageContainers[Images.ImageContainers.Count - 1].picbox.Top = 10;

            }
            else
            {

                Images.ImageContainers[Images.ImageContainers.Count - 1].picbox.Left = Images.ImageContainers[Images.ImageContainers.Count - 2].picbox.Left + Images.ImageContainers[Images.ImageContainers.Count - 2].picbox.Width + 10;
                Images.ImageContainers[Images.ImageContainers.Count - 1].picbox.Top = Images.ImageContainers[Images.ImageContainers.Count - 2].picbox.Top;

            }



        }








        private static bool ThumbnailCallback()
        {
            return false;
        }
        private Bitmap GetThumb(string filename)
        {

            return (Bitmap)(new Bitmap(filename)).GetThumbnailImage(80, 80, new Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);

        }





        private List<int> PagePosition()
        {

            List<int> result = new List<int>();

            foreach (ImageContainer item in Images.ImageContainers)
            {




            }



            return result;

        }

        //public class ImageInformation
        //{

        //    public string filename;
        //    public int level;

        //}


    }




    class ImagePageInfo
    {

        public static int ImagesPerRow;
        public static int Rows;

    }

    class Images
    {

        public static List<ImageContainer> ImageContainers = new List<ImageContainer>();

    }


    class ImageContainer
    {

        public PictureBox picbox = new PictureBox();
        public string filename;
        public int level;
        public DateTime datetime;
        public int page = 0;
        public int position = 0;

        private static bool ThumbnailCallback()
        {
            return false;
        }


        public ImageContainer(string p_filename, int p_level)
        {


            filename = Path.GetFileName(p_filename);
            level = p_level;
            datetime = File.GetCreationTime(p_filename);
            //picbox.ImageLocation = resizeImage(Path.GetDirectoryName(p_filename) + "\\", Path.GetFileName(p_filename), 100, 100);
            picbox.Image = resizeImage(Path.GetDirectoryName(p_filename) + "\\", Path.GetFileName(p_filename), 100, 100);



        }




        private Bitmap resizeImage(string path, string originalFilename, int canvasWidth, int canvasHeight)
        {


            Image image = Image.FromFile(path + originalFilename);
            int originalWidth = image.Width;
            int originalHeight = image.Height;


            Image thumbnail = new Bitmap(canvasWidth, canvasHeight); // changed parm names
            Graphics graphic = Graphics.FromImage(thumbnail);

            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphic.CompositingQuality = CompositingQuality.HighQuality;

            /* ------------------ new code --------------- */

            // Figure out the ratio
            double ratioX = (double)canvasWidth / (double)originalWidth;
            double ratioY = (double)canvasHeight / (double)originalHeight;
            // use whichever multiplier is smaller
            double ratio = ratioX < ratioY ? ratioX : ratioY;

            // now we can get the new height and width
            int newHeight = Convert.ToInt32(originalHeight * ratio);
            int newWidth = Convert.ToInt32(originalWidth * ratio);

            // Now calculate the X,Y position of the upper-left corner 
            // (one of these will always be zero)
            int posX = Convert.ToInt32((canvasWidth - (originalWidth * ratio)) / 2);
            int posY = Convert.ToInt32((canvasHeight - (originalHeight * ratio)) / 2);

            graphic.Clear(Color.White); // white padding
            graphic.DrawImage(image, posX, posY, newWidth, newHeight);

            /* ------------- end new code ---------------- */
            return (Bitmap)thumbnail;



            //System.Drawing.Imaging.ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
            //EncoderParameters encoderParameters = new EncoderParameters(1);
            //encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);


            //thumbnail.Save(path + newWidth + "." + originalFilename, info[1], encoderParameters); ;
            //return path + newWidth + "." + originalFilename;


        }









    }






}

