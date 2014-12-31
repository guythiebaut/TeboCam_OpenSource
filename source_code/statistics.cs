using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Text;

namespace TeboCam
{
    public class statistics
    {

        public static int statisticsCountLimit = 10000;
        private const string tab = "\t";
        private const string timestamp = "yyyyMMddHHmmfff";
        //this is the filename that will be used for writing statistics
        public static string fileName = string.Empty;

        private class movement
        {

            public int cameraId;
            public string cameraName;
            public int motionLevel;
            public int alarmLevel;
            public Int64 milliSecondsSinceStart;
            public string profile;
            public DateTime dateTime;
            public bool statReturnedPing;
            public bool statReturnedPublish;
            public bool statReturnedOnline;
            public bool statReturnedAlert;

        }

        private class LastMovement
        {

            public int cameraId;
            public int motionLevel;
            public Int64 milliSecondsSinceStart;
            public string profile;

        }

        public class movementResults
        {

            public int avgMvStart;
            public int avgMvLast;
            public int mvNow;

        }


        private static List<movement> statList = new List<movement>();
        private static List<LastMovement> lastMovementList = new List<LastMovement>();

        public static void AddStatistic(int p_cameraId, string p_cameraName, int p_motionLevel, int p_alarmLevel, Int64 p_milliSsecondsSinceStart, string p_profile, bool p_outputToFile, string p_fileName, double p_maxLength)
        {

            insertIfValueChanged(p_cameraId, p_cameraName, p_motionLevel, p_alarmLevel, p_milliSsecondsSinceStart, p_profile, p_outputToFile, p_fileName, p_maxLength);

        }

        public static List<object> lightSpikeDetected(int p_cam,
                                                      int p_mov,
                                                      int p_millisecs,
                                                      int p_tolerance,
                                                      string p_profile,
                                                      long p_atTime)
        {

            List<object> results = new List<object>();

            int perc = new int();
            bool spike = false;


            int lowVal = statistics.lowestValTime(p_cam, p_millisecs, p_profile, p_atTime);
            int difference = p_mov - lowVal;

            if (difference > 0)
            {

                //is average change within tolerance percentage?
                perc = (int)Math.Floor(((double)difference / (double)p_mov) * (double)100);
                spike = perc > p_tolerance;

            }


            results.Add(spike);
            results.Add(perc);
            return results;

        }

        private static void WriteToFile(string p_fileName, double p_MaxLength, movement p_mv)
        {


            if (fileName == string.Empty)
            {

                if (p_MaxLength > Convert.ToDouble(0))
                {

                    fileName = Path.GetDirectoryName(p_fileName) + "\\" + Path.GetFileNameWithoutExtension(p_fileName) + DateTime.Now.ToString(timestamp) + Path.GetExtension(p_fileName);

                }
                else
                {

                    fileName = p_fileName;

                }
            }

            if (!File.Exists(fileName))
            {

                using (StreamWriter w = File.AppendText(fileName))
                {

                    w.Write("cameraId" + tab + "cameraName" + tab + "motionLevel" + tab + "alarmLevel" + tab + "profile" + tab + "dateTime" + Environment.NewLine);

                }

            }


            FileInfo fi = new FileInfo(fileName);

            //create new file if file has reached largest size allowed



            if (p_MaxLength > Convert.ToDouble(0) && (Convert.ToDouble(fi.Length)>= (p_MaxLength * Convert.ToDouble(1024) * Convert.ToDouble(1024)) || fileName == string.Empty))
            {

                fileName = Path.GetDirectoryName(p_fileName) + "\\" + Path.GetFileNameWithoutExtension(p_fileName) + DateTime.Now.ToString(timestamp) + Path.GetExtension(p_fileName);

                using (StreamWriter w = File.AppendText(fileName))
                {

                    w.Write("cameraId" + tab + "cameraName" + tab + "motionLevel" + tab + "alarmLevel" + tab + "profile" + tab + "dateTime" + Environment.NewLine);

                }

            }





            using (StreamWriter w = File.AppendText(fileName))
            {


                string outString = p_mv.cameraId.ToString() + tab + p_mv.cameraName + tab + p_mv.motionLevel.ToString() + tab + p_mv.alarmLevel.ToString() + tab +
                    p_mv.profile.Replace(tab, " ") + tab + p_mv.dateTime.ToString("dd/MM/yyyy HH:mm:ss.fff") +
                    Environment.NewLine;

                w.Write(outString);

            }

        }

        public static int lowestValTime(int p_cameraId, int p_milliseconds, string p_profile, long p_atTime)
        {

            int lowestVal = 100;
            long startMilli = new long();
            long lastMilli = new long();

            //start from teh most recenbt statistic
            for (int i = statList.Count - 1; i >= 0; i--)
            {

                //we have a match on camera and profile
                if (statList[i].cameraId == p_cameraId
                    && statList[i].profile == p_profile
                    && statList[i].milliSecondsSinceStart <= p_atTime)
                {


                    //first time through so we set the last time to the 
                    //most recent time recorded
                    if (startMilli == 0)
                    {

                        startMilli = statList[i].milliSecondsSinceStart;

                    }

                    //the time between the start time and current stat is less than 
                    //or equal to the time frame 
                    if (startMilli - statList[i].milliSecondsSinceStart <= p_milliseconds)
                    {


                        //the level is lower than the lowest level found so far
                        if (statList[i].motionLevel < lowestVal)
                        {

                            lowestVal = statList[i].motionLevel;
                            lastMilli = statList[i].milliSecondsSinceStart;

                            //System.Diagnostics.Debug.Print(lowestVal.ToString());

                            //if (lowestVal == 0)
                            //{

                            //    System.Diagnostics.Debug.Print("zero value");

                            //}

                        }

                    }
                    else
                    {

                        //find if there is a lower value just before the current value
                        // as we need to account for gaps were values remain the same
                        if (i > 0)
                        {

                            for (int a = i - 1; a >= 0; a--)
                            {

                                if (statList[a].cameraId == p_cameraId
                                    && statList[a].profile == p_profile
                                    && statList[a].motionLevel < lowestVal)
                                {

                                    lowestVal = statList[a].motionLevel;
                                    break;

                                }

                            }

                        }

                        break;

                    }


                }


            }

            try
            {

                return lowestVal;

            }
            catch (Exception)
            {


                return 0;

            }


        }



        public static void clear()
        {

            statList.Clear();
        }


        private static void insertIfValueChanged(int p_cameraId, string p_cameraName, int p_motionLevel, int p_alarmLevel, Int64 p_milliSsecondsSinceStart, string p_profile, bool p_outputToFile, string p_fileName, double p_maxLength)
        {

            int statsLimit = statisticsCountLimit;
            bool found = false;

            //keep the list of statistics to a reasonable size
            if (statList.Count > statsLimit)
            {

                statList.RemoveRange(0, 1);

            }

            foreach (LastMovement item in lastMovementList)
            {

                if (item.cameraId == p_cameraId && item.profile == p_profile)
                {

                    found = true;

                    if (item.motionLevel != p_motionLevel)
                    {

                        item.milliSecondsSinceStart = p_milliSsecondsSinceStart;
                        item.motionLevel = p_motionLevel;

                        movement mv = new movement();
                        mv.cameraId = p_cameraId;
                        mv.cameraName = p_cameraName;
                        mv.motionLevel = p_motionLevel;
                        mv.alarmLevel = p_alarmLevel;
                        mv.milliSecondsSinceStart = p_milliSsecondsSinceStart;
                        mv.dateTime = DateTime.Now;
                        mv.profile = p_profile;

                        statList.Add(mv);

                        if (p_outputToFile)
                        {

                            WriteToFile(p_fileName, p_maxLength, statList.Last());

                        }

                    }

                    break;

                }

            }


            if (!found)
            {


                LastMovement mov = new LastMovement();
                mov.cameraId = p_cameraId;
                mov.milliSecondsSinceStart = p_milliSsecondsSinceStart;
                mov.motionLevel = p_motionLevel;
                mov.profile = p_profile;
                lastMovementList.Add(mov);


            }


        }


        public static movementResults statsForCam(int icameraId, string iprofile, string imageType)
        {

            int firstCount = new int();
            int firstSum = new int();
            int lastCount = new int();
            int lastSum = new int();
            int currMv = new int();

            firstCount = 0;
            firstSum = 0;
            lastCount = 0;
            lastSum = 0;
            currMv = 0;


            foreach (movement mv in statList)
            {

                if (mv.cameraId == icameraId && mv.profile == iprofile)
                {

                    bool statsReturned = new bool();

                    switch (imageType)
                    {
                        case "Publish":
                            statsReturned = mv.statReturnedPublish;
                            break;
                        case "Online":
                            statsReturned = mv.statReturnedOnline;
                            break;
                        case "Ping":
                            statsReturned = mv.statReturnedPing;
                            break;
                        case "Alert":
                            statsReturned = mv.statReturnedAlert;
                            break;
                        default:
                            statsReturned = mv.statReturnedPublish;
                            break;
                    }

                    if (statsReturned)
                    {

                        firstCount++;
                        firstSum += mv.motionLevel;

                    }
                    else
                    {

                        firstCount++;
                        firstSum += mv.motionLevel;
                        lastCount++;
                        lastSum += mv.motionLevel;

                    }

                    currMv = mv.motionLevel;

                    switch (imageType)
                    {
                        case "Publish":
                            mv.statReturnedPublish = true;
                            break;
                        case "Online":
                            mv.statReturnedOnline = true;
                            break;
                        case "Ping":
                            mv.statReturnedPing = true;
                            break;
                        case "Alert":
                            mv.statReturnedAlert = true;
                            break;
                        default:
                            mv.statReturnedPublish = true;
                            break;
                    }




                }

            }

            movementResults mvR = new movementResults();
            mvR.avgMvLast = (int)Math.Floor((double)lastSum / (double)lastCount);
            mvR.avgMvStart = (int)Math.Floor((double)firstSum / (double)firstCount);

            mvR.avgMvLast = mvR.avgMvLast > 0 ? mvR.avgMvLast : 0;
            mvR.avgMvStart = mvR.avgMvStart > 0 ? mvR.avgMvStart : 0;

            mvR.mvNow = currMv;

            return mvR;

        }




    }
}
