using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

//using System.Text;

namespace TeboCam
{
    public class statistics
    {
        public const int statisticsCountLimit = 1000000;
        private const string tab = "\t";

        private const string timestamp = "yyyyMMddHHmmfff";

        //this is the filename that will be used for writing Statistics
        public static string fileName = string.Empty;

        public static IException tebowebException;

        private class movement
        {
            public int cameraId;
            public string cameraName;
            public int motionLevel;
            public int alarmLevel;
            public long milliSecondsSinceStart;
            public string profile;
            public DateTime dateTime;

        }

        private class LastMovement
        {
            public int cameraId;
            public int motionLevel;

            public string profile;
        }

        public class movementResults
        {
            public string camera;
            public int avgMvStart;
            public int avgMvLast;
            public int mvNow;
        }


        private static readonly List<movement> statList = new List<movement>();
        private static List<LastMovement> lastMovementList = new List<LastMovement>();

        public static void AddStatistic(int p_cameraId, string p_cameraName, int p_motionLevel, int p_alarmLevel,
            long p_milliSsecondsSinceStart, string p_profile, bool p_outputToFile, string p_fileName,
            double p_maxLength)
        {
            insertIfValueChanged(p_cameraId, p_cameraName, p_motionLevel, p_alarmLevel, p_milliSsecondsSinceStart,
                p_profile, p_outputToFile, p_fileName, p_maxLength);
        }

        public static List<object> lightSpikeDetected(int p_cam,
            int p_mov,
            int p_millisecs,
            int p_tolerance,
            string p_profile,
            long p_atTime)
        {
            var results = new List<object>();

            var perc = new int();
            var spike = false;


            var lowVal = lowestValTime(p_cam, p_millisecs, p_profile, p_atTime);
            var difference = p_mov - lowVal;

            if (difference > 0)
            {
                //is average change within tolerance percentage?
                perc = (int)Math.Floor(difference / (double)p_mov * 100);
                spike = perc > p_tolerance;
            }


            results.Add(spike);
            results.Add(perc);
            return results;
        }

        private static void WriteToFile(string p_fileName, double p_MaxLength, movement p_mv)
        {
            if (fileName == string.Empty)
                if (p_MaxLength > Convert.ToDouble(0))
                    fileName = Path.GetDirectoryName(p_fileName) + "\\" + Path.GetFileNameWithoutExtension(p_fileName) +
                               DateTime.Now.ToString(timestamp) + Path.GetExtension(p_fileName);
                else
                    fileName = p_fileName;

            if (!Directory.Exists(new FileInfo(fileName).Directory.FullName))
                Directory.CreateDirectory(new FileInfo(fileName).Directory.FullName);

            if (!File.Exists(fileName))
                using (var w = File.CreateText(fileName))
                {
                    w.Write("cameraId" + tab + "cameraName" + tab + "motionLevel" + tab + "alarmLevel" + tab +
                            "profile" + tab + "dateTime" + Environment.NewLine);
                }
            else
                using (var w = File.AppendText(fileName))
                {
                    w.Write("cameraId" + tab + "cameraName" + tab + "motionLevel" + tab + "alarmLevel" + tab +
                            "profile" + tab + "dateTime" + Environment.NewLine);
                }


            var fi = new FileInfo(fileName);

            //create new file if file has reached largest size allowed


            if (p_MaxLength > Convert.ToDouble(0) &&
                (Convert.ToDouble(fi.Length) >= p_MaxLength * Convert.ToDouble(1024) * Convert.ToDouble(1024) ||
                 fileName == string.Empty))
            {
                fileName = Path.GetDirectoryName(p_fileName) + "\\" + Path.GetFileNameWithoutExtension(p_fileName) +
                           DateTime.Now.ToString(timestamp) + Path.GetExtension(p_fileName);

                using (var w = File.AppendText(fileName))
                {
                    w.Write("cameraId" + tab + "cameraName" + tab + "motionLevel" + tab + "alarmLevel" + tab +
                            "profile" + tab + "dateTime" + Environment.NewLine);
                }
            }


            using (var w = File.AppendText(fileName))
            {
                var outString = p_mv.cameraId + tab + p_mv.cameraName + tab + p_mv.motionLevel + tab + p_mv.alarmLevel +
                                tab +
                                p_mv.profile.Replace(tab, " ") + tab +
                                p_mv.dateTime.ToString("dd/MM/yyyy HH:mm:ss.fff") +
                                Environment.NewLine;

                w.Write(outString);
            }
        }

        public static int lowestValTime(int p_cameraId, int p_milliseconds, string p_profile, long p_atTime)
        {
            var lowestVal = 100;
            var startMilli = new long();
            var lastMilli = new long();

            //start from teh most recenbt statistic
            for (var i = statList.Count - 1; i >= 0; i--)
                //we have a match on camera and profile
                if (statList[i].cameraId == p_cameraId
                    && statList[i].profile == p_profile
                    && statList[i].milliSecondsSinceStart <= p_atTime)
                {
                    //first time through so we set the last time to the 
                    //most recent time recorded
                    if (startMilli == 0)
                        startMilli = statList[i].milliSecondsSinceStart;

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
                        // as we need to account for gaps where values remain the same
                        if (i > 0)
                            for (var a = i - 1; a >= 0; a--)
                                if (statList[a].cameraId == p_cameraId
                                    && statList[a].profile == p_profile
                                    && statList[a].motionLevel < lowestVal)
                                {
                                    lowestVal = statList[a].motionLevel;
                                    break;
                                }

                        break;
                    }
                }

            try
            {
                return lowestVal;
            }
            catch (Exception e)
            {
                TebocamState.tebowebException.LogException(e);
                return 0;
            }
        }


        public static void clear()
        {
            statList.Clear();
        }


        private static void insertIfValueChanged(int p_cameraId, string p_cameraName, int p_motionLevel,
            int p_alarmLevel, long p_milliSsecondsSinceStart, string p_profile, bool p_outputToFile, string p_fileName,
            double p_maxLength)
        {
            var statsLimit = statisticsCountLimit;

            //keep the list of Statistics to a reasonable size
            if (statList.Count > statsLimit)
                statList.RemoveRange(0, 1000);

            var lastRecordedStatSinceChange =
                statList.LastOrDefault(x => x.cameraId == p_cameraId && x.profile == p_profile);

            if (lastRecordedStatSinceChange == null)
            {
                var mv = new movement();
                mv.cameraId = p_cameraId;
                mv.cameraName = p_cameraName;
                mv.motionLevel = p_motionLevel;
                mv.alarmLevel = p_alarmLevel;
                mv.milliSecondsSinceStart = p_milliSsecondsSinceStart;
                mv.dateTime = DateTime.Now;
                mv.profile = p_profile;
                statList.Add(mv);
                return;
            }

            if (lastRecordedStatSinceChange.motionLevel != p_motionLevel)
            {
                var mv = new movement();
                mv.cameraId = p_cameraId;
                mv.cameraName = p_cameraName;
                mv.motionLevel = p_motionLevel;
                mv.alarmLevel = p_alarmLevel;
                mv.milliSecondsSinceStart = p_milliSsecondsSinceStart;
                mv.dateTime = DateTime.Now;
                mv.profile = p_profile;
                statList.Add(mv);

                if (p_outputToFile)
                    WriteToFile(p_fileName, p_maxLength, statList.Last());
            }
        }


        public static movementResults statsForCam(int icameraId, string iprofile, string imageType, string friendlyName)
        {
            bool statReturnedPing = false;
            bool statReturnedPublish = false;
            bool statReturnedOnline = false;
            bool statReturnedAlert = false;

            var firstCount = new int();
            var firstSum = new int();
            var lastCount = new int();
            var lastSum = new int();
            var currMv = new int();

            firstCount = 0;
            firstSum = 0;
            lastCount = 0;
            lastSum = 0;
            currMv = 0;


            var stats = statList.Where(x => x.cameraId == icameraId && x.profile == iprofile);

            foreach (var mv in stats)
            {
                var statsReturned = new bool();

                switch (imageType)
                {
                    case "Publish":
                        statsReturned = statReturnedPublish;
                        break;
                    case "Online":
                        statsReturned = statReturnedOnline;
                        break;
                    case "Ping":
                        statsReturned = statReturnedPing;
                        break;
                    case "Alert":
                        statsReturned = statReturnedAlert;
                        break;
                    default:
                        statsReturned = statReturnedPublish;
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
                        statReturnedPublish = true;
                        break;
                    case "Online":
                        statReturnedOnline = true;
                        break;
                    case "Ping":
                        statReturnedPing = true;
                        break;
                    case "Alert":
                        statReturnedAlert = true;
                        break;
                    default:
                        statReturnedPublish = true;
                        break;
                }
            }

            var mvR = new movementResults();
            mvR.avgMvLast = (int)Math.Floor(lastSum / (double)lastCount);
            mvR.avgMvStart = (int)Math.Floor(firstSum / (double)firstCount);

            mvR.avgMvLast = mvR.avgMvLast > 0 ? mvR.avgMvLast : 0;
            mvR.avgMvStart = mvR.avgMvStart > 0 ? mvR.avgMvStart : 0;

            mvR.mvNow = currMv;

            mvR.camera = friendlyName;

            return mvR;
        }
    }
}