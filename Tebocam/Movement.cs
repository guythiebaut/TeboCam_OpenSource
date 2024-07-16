using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TeboCam
{
    public static class Movement
    {
        public static event EventHandler pulseEvent;
        public static int motionLevelprevious = 0;
        public static int lastUpdateSeq = 0;
        public static int lastStartSeq = 0;
        public static int updateSeq = 0;
        public static ArrayList moveStats = new ArrayList();
        public static ArrayList imagesSaved = new ArrayList();
        public static int motionLevel = 0;

        public class imageText
        {
            public Bitmap bitmap;
            public string type;
            public bool backingRectangle;
            public List<string> stats;
            public string position;
            public string format;
            public string colour;
        }

        public static class imagesFromMovement
        {
            public enum TypeEnum
            {
                All,
                Ftp,
                Email,
                FtpAndEMail
            }

            public class item
            {
                public string fileName;
                public bool ftp;
                public bool email;
            }

            public static List<item> imageList = new List<item>();

            public static void addImageRange(ArrayList images)
            {
                for (int i = 0; i < images.Count; i++)
                {
                    item itm = new item();
                    itm.fileName = images[i].ToString(); ;
                    itm.ftp = false;
                    itm.email = false;
                    imageList.Add(itm);
                }
            }


            public static void listsClear(TypeEnum type)
            {

                if (type == TypeEnum.All)
                {

                    imageList.Clear();

                }
                else
                {
                    for (int i = imageList.Count - 1; i >= 0; i--)
                    {
                        switch (type)
                        {
                            case TypeEnum.Ftp:
                                if (imageList[i].ftp) imageList.RemoveAt(i);
                                break;
                            case TypeEnum.Email:
                                if (imageList[i].email) imageList.RemoveAt(i);
                                break;
                            case TypeEnum.FtpAndEMail:
                                if (imageList[i].ftp && imageList[i].email) imageList.RemoveAt(i);
                                break;
                        }
                    }
                }
            }

            public static int ftpToProcess()
            {
                return imageList.Count(x => !x.ftp);
            }

            public static int emailToProcess()
            {
                return imageList.Count(x => !x.email);
            }
        }

        public class publishCams
        {
            private List<bool> cams = new List<bool>();

            public publishCams(int cameras)
            {
                for (int i = 0; i <= cameras; i++)
                {
                    cams.Add(false);
                }
            }

            public void publishToCamAdd(int cam)
            {
                cams[cam] = true;
            }

            public void publishToCamRemove(int cam)
            {
                cams[cam] = false;
            }
        }


        public class AlertClass
        {
            bool alert;

            public bool on
            {
                get { return alert; }
                set
                {
                    alert = value;
                    CameraRig.alert(value);
                }
            }
        }

        public static void areaOffAtMotionInit()
        {
            CameraRig.AreaOffAtMotionTriggered = false;
            //areaOffAtMotionReset = false;
            CameraRig.AreaOffAtMotionReset = false;
        }

        public static void RecordMotionStats(MotionLevelArgs a, CamIdArgs b)
        {


            double maxFileLength = 0;

            if (ConfigurationHelper.GetCurrentProfile().StatsToFileTimeStamp)
            {

                maxFileLength = ConfigurationHelper.GetCurrentProfile().StatsToFileMb;

            }

            statistics.AddStatistic(b.camNo,
                ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), b.name).friendlyName.Trim(),
                Convert.ToInt32((int)Math.Floor(a.lvl * 100)),
                Convert.ToInt32((int)Math.Floor(a.alarmLvl * 100)),
                time.millisecondsSinceStart(),
                ConfigurationHelper.GetCurrentProfileName(),
                ConfigurationHelper.GetCurrentProfile().StatsToFileOn,
                ConfigurationHelper.GetCurrentProfile().StatsToFileLocation,
                maxFileLength);

        }

        //add most recent batch of movement images to arraylist
        public static void movementAddImages()
        {
            try
            {
                int currentUpdateSeq = updateSeq;

                if (lastUpdateSeq != currentUpdateSeq)
                {
                    teboDebug.writeline(teboDebug.movementAddImagesVal + 1);

                    //only pulse every 5 images
                    if (currentUpdateSeq % 5 == 0)
                    {
                        pulseEvent(null, new EventArgs());
                    }

                    int tmpInt = imagesSaved.Count;
                    teboDebug.writeline(teboDebug.movementAddImagesVal + 2);
                    ArrayList tmpArrLst = new ArrayList(imagesSaved.GetRange(lastStartSeq, (tmpInt - lastStartSeq)));
                    Movement.imagesFromMovement.addImageRange(tmpArrLst);
                    lastStartSeq = tmpInt;
                    lastUpdateSeq = currentUpdateSeq;
                    Sound.RingMyBell(false);
                }
            }
            catch (Exception e)
            {
                TebocamState.tebowebException.LogException(e);
            }
        }

        public static void moveStatsInitialise()
        {
            for (int i = 0; i < 12; i++)
            {
                moveStats.Add(0);
            }
        }


        public static void motionEvent(object sender, MotionLevelArgs a, CamIdArgs b)
        {
            levelLine(a, b);
            Movement.RecordMotionStats(a, b);
        }

        public static event EventHandler motionLevelChanged;

        public static void levelLine(MotionLevelArgs a, CamIdArgs b)
        {
            if (b.camNo == CameraRig.CurrentlyDisplayingCamera)
            {
                motionLevel = Convert.ToInt32((int)Math.Floor(a.lvl * 100));

                if (motionLevelChanged != null && motionLevel != motionLevelprevious)
                {
                    motionLevelprevious = motionLevel;
                    motionLevelChanged(null, new EventArgs());
                }
            }
        }

        public static event EventHandler motionDetectionActivate;
        public static event EventHandler motionDetectionInactivate;

        public static void MotionDetectionActivate()
        {
            motionDetectionActivate(null, new EventArgs());
        }

        public static void MotionDetectionInactivate()
        {
            motionDetectionInactivate(null, new EventArgs());
        }
    }
}
