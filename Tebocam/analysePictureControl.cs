using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class analysePictureControl : UserControl
    {


        public string cam;
        public long time;
        public int movLevel;
        public Color borderColour;
        ToolTip calebrateImageTip = new ToolTip();

        public analysePictureControl()
        {
            InitializeComponent();
        }

        public analysePictureControl(Bitmap p_picture, string p_name, long p_time, Color p_borderColour, int p_level)
        {
            InitializeComponent();

            imageBox.Image = resizeImage(p_picture, imageBox.Width, imageBox.Height);
            cam = p_name;
            time = p_time;
            borderColour = p_borderColour;
            movLevel = p_level;
            calebrateImageTip.Active = true;
            calebrateImageTip.IsBalloon = true;
            calebrateImageTip.InitialDelay = 500;
            calebrateImageTip.AutoPopDelay = 5000;
            calebrateImageTip.SetToolTip(this.imageBox, "Motion level: " + p_level.ToString());
        }

        private static Bitmap resizeImage(Bitmap imgage, int width, int height)
        {
            var imgToResize = new Bitmap(imgage);
            var sourceWidth = imgToResize.Width;
            var sourceHeight = imgToResize.Height;
            var nPercentW = width / (float)sourceWidth;
            var nPercentH = height / (float)sourceHeight;
            var nPercent = Math.Max(nPercentH, nPercentW);
            var destWidth = (int)(sourceWidth * nPercent);
            var destHeight = (int)(sourceHeight * nPercent);
            var bitmap = new Bitmap(destWidth, destHeight);
            var g = Graphics.FromImage(bitmap);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return bitmap;
        }

        private void imageBorder_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(borderColour), new Rectangle(0, 0, imageBorder.Width, imageBorder.Height));
        }
    }
}
