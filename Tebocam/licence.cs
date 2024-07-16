using System.Collections.Generic;


namespace TeboCam
{
    class licence
    {
        private static List<int> camsSelected = new List<int>();

        public static bool licenced = false;

        public static int camsAllowed = 9;


        public static int camsSuported()
        {

            return camsAllowed;

        }


        public static bool selectCam(int cam)
        {

            if (camsSelected.Count + 1 <= camsSuported())
            {
                camsSelected.Add(cam);
                return true;
            }
            else
            {
                return false;
            }

        }

        public static void deselectCam(int cam)
        {

            for (int i = 0; i < camsSelected.Count; i++)
            {

                if (camsSelected[i] == cam) camsSelected.RemoveAt(i);

            }

        }

        /// <summary>
        /// returns a bool showing if a camera is selected as active
        /// </summary>
        /// <returns>bool</returns>
        public static bool aCameraIsSelected()
        {

            return camsSelected.Count > 0;

        }


        /// <param name="cam"> The camera to check.</param>
        /// <returns>Bool showing if the camera is active.</returns>
        public static bool CamIsActive(int cam)
        {

            foreach (int camera in camsSelected)
            {

                if (camera == cam) return true;

            }

            return false;


        }




    }
}
