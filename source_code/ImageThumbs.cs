using System;
using System.Collections.Generic;
using System.Text;

namespace TeboCam
{
    public class ImageThumbs
    {

        //private static string[] pics = new string[20];
        public static int picsInWindowCount = 0;
        public static List<string> thumbNames = new List<string>();
        public static List<string> thumbPics = new List<string>();
        private static List<string[]> thumbFiles = new List<string[]>();
        private static bool noDataYet = true;

        private static int firstFreeBlock = 0;
        public static int windowsFull = 0;

        public static List<string[]> thumbs = new List<string[]>();


        public static void initPics(int thumbs)
        {
            picsInWindowCount = thumbs;
            string[] pics = new string[picsInWindowCount];
            thumbFiles.Add(pics);
        }

        public static string[,] populateWindow(int windowNum)
        {

            string[,] picsForWindow = new string[picsInWindowCount, 2];

            if (noDataYet)
            { return picsForWindow; }

            try
            {

                if (windowNum > windowsFull) { windowNum = windowsFull; }

                string[] firstBlock = new string[picsInWindowCount];
                firstBlock = thumbFiles[windowNum];

                int tmpInt = 0;

                foreach (string name in thumbNames)
                {
                    string tmpStr = firstBlock[tmpInt];
                    if (tmpStr != null)
                    {
                        picsForWindow[tmpInt, 0] = name;
                        picsForWindow[tmpInt, 1] = tmpStr;
                    }
                    tmpInt++;
                }
                return picsForWindow;
            }
            catch (Exception)
            {
                string[,] errorBlank = new string[picsInWindowCount, 2];
                return errorBlank;
            }
        }

        public static void reset()
        {
            firstFreeBlock = 0;
            foreach (string[] block in thumbFiles)
            {
                for (int i = 0; i < block.Length; i++)
                {
                    block[i] = null;
                }

            }
        }

        public static void addThumbToPictureBox(string pic)
        {

            int firstFreeCell = 0;
            bool freeCell = false;

            string[] freeBlock = new string[picsInWindowCount];
            freeBlock = thumbFiles[firstFreeBlock];

            foreach (string cell in freeBlock)
            {
                if (cell != null)
                {
                    firstFreeCell++;
                }
                else
                {
                    freeCell = true;
                    break;
                }
            }

            if (freeCell)
            {
                freeBlock[firstFreeCell] = pic;
                thumbFiles[firstFreeBlock] = freeBlock;
            }
            else
            {
                string[] newBlock = new string[picsInWindowCount];
                newBlock[0] = pic;
                thumbFiles.Add(newBlock);
                firstFreeBlock++;
                windowsFull = firstFreeBlock;
            }
            noDataYet = false;
        }




    }
}
