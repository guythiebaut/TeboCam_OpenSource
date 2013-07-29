using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace TeboCam
{
    public partial class analysePictureControl : UserControl
    {


        public string cam;
        public long time;
        public Color borderColour;

        public analysePictureControl()
        {

            InitializeComponent();

        }


        public analysePictureControl(Bitmap p_picture, string p_name, long p_time,Color p_borderColour)
        {

            InitializeComponent();

            imageBox.Image = resizeImage(p_picture, imageBox.Width, imageBox.Height);
            cam = p_name;
            time = p_time;
            borderColour = p_borderColour;

        }




        public void setborder(Color colour)
        {

            Graphics border = this.imageBorder.CreateGraphics();
            border.FillRectangle(new SolidBrush(colour), 0, 0, imageBorder.Width, imageBorder.Height);
            //imageBorder.SynchronisedInvoke(() => imageBorder.CreateGraphics().FillRectangle(new SolidBrush(Color.Red), 0, 0, imageBorder.Width, imageBorder.Height));
            //border.Dispose();

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

        private void imageBorder_Paint(object sender, PaintEventArgs e)
        {

            e.Graphics.FillRectangle(new SolidBrush(borderColour), new Rectangle(0, 0, imageBorder.Width, imageBorder.Height));
                

        }


    }
}
