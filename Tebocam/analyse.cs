using System.Collections.Generic;
using System.Drawing;

namespace TeboCam
{
    public class analyse
    {

        public List<analysePictureControl> images = new List<analysePictureControl>();


        public void newPictureControl(Bitmap picture, string name,long time,Color colour,int level)
        {

            analysePictureControl pic = new analysePictureControl(picture, name, time, colour,level);
            images.Add(pic);

        }

        public void newPictureControl(string picture, string name, long time, Color colour, int level)
        {

            analysePictureControl pic = new analysePictureControl(picture, name, time, colour, level);
            images.Add(pic);

        }


    }
}
