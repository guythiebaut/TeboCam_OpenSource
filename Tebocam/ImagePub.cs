using System;
using System.Collections.Generic;

namespace TeboCam
{
    public static class ImagePub
    {

        public delegate void ImagePubEventHandler(object source, ImagePubArgs e);
        public static event ImagePubEventHandler publishPicture;

        public class ImagePubArgs : EventArgs
        {
            public string _option;
            public List<string> _lst;

            public string option
            {
                get { return _option; }
                set { _option = value; }
            }

            public int _camNo;
            public int CamNo
            {
                get { return _camNo; }
                set { _camNo = value; }
            }

            public List<string> lst
            {
                get { return _lst; }
                set { _lst = value; }
            }
        }

        public static void PubPicture(ImagePub.ImagePubArgs a)
        {
            publishPicture(null, a);
        }
    }
}
