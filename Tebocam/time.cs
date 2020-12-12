using System;
using System.Collections.Generic;
using System.Text;

namespace TeboCam
{
    class time
    {

        private const int millisecsInSecond = 1000;
        private const int millisecsInMinute = 60000;
        private const int millisecsInHour = 3600000;
        private const int millisecsInDay = 86400000;

        private const int secsInMinute = 60;
        private const int secsInHour = 3600;
        private const int secsInDay = 86400;

        private static int startday = 0;
        private static int starttime = 0;

        private static void setStart()
        {
            startday = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));
            starttime = secondsSinceMidnight();
        }


        public static int secondsSinceMidnight()
        {
            string tmpStr = DateTime.Now.ToString("HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            int hour = Convert.ToInt32(LeftRightMid.Left(tmpStr, 2));
            int mins = Convert.ToInt32(LeftRightMid.Mid(tmpStr, 3, 2));
            int secs = Convert.ToInt32(LeftRightMid.Right(tmpStr, 2));
            int secsSinceMidnight = (hour * secsInHour) + (mins * secsInMinute) + secs;
            return secsSinceMidnight;
        }

        public static int secondsSinceMidnight(DateTime dt)
        {
            string tmpStr = dt.ToString("HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            int hour = Convert.ToInt32(LeftRightMid.Left(tmpStr, 2));
            int mins = Convert.ToInt32(LeftRightMid.Mid(tmpStr, 3, 2));
            int secs = Convert.ToInt32(LeftRightMid.Right(tmpStr, 2));
            int secsSinceMidnight = (hour * secsInHour) + (mins * secsInMinute) + secs;
            return secsSinceMidnight;
        }

        public static int millisecondsSinceMidnight()
        {
            string tmpStr = DateTime.Now.ToString("HH:mm:ss:fff", System.Globalization.CultureInfo.InvariantCulture);
            int hour = Convert.ToInt32(LeftRightMid.Left(tmpStr, 2));
            int mins = Convert.ToInt32(LeftRightMid.Mid(tmpStr, 3, 2));
            int secs = Convert.ToInt32(LeftRightMid.Mid(tmpStr, 6, 2));
            int millisecs = Convert.ToInt32(LeftRightMid.Right(tmpStr, 3));
            int millisecsSinceMidnight = (hour * millisecsInHour) + (mins * millisecsInMinute) + (secs * millisecsInSecond) + millisecs;
            return millisecsSinceMidnight;
        }

        public static int secondsSinceStart()
        {

            if (startday == 0) setStart();

            int thisday = Convert.ToInt32(currentDateYYYYMMDD());
            int daysSinceStart = Math.Abs(thisday - startday);


            int result = (daysSinceStart * secsInDay) - starttime + secondsSinceMidnight();
            return result;
        }

        public static int secondsSinceStart(string dateTime)
        {

            DateTime dt = Convert.ToDateTime(dateTime);
            int firsttime = secondsSinceMidnight(dt);

            int thisday = Convert.ToInt32(currentDateYYYYMMDD());
            int firstday = Convert.ToInt32(dt.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));
            int daysSinceStart = Math.Abs(thisday - firstday);


            int result = (daysSinceStart * secsInDay) - firsttime + secondsSinceMidnight();
            return result;
        }

        public static Int64 millisecondsSinceStart()
        {

            if (startday == 0) setStart();

            int thisday = Convert.ToInt32(currentDateYYYYMMDD());
            int daysSinceStart = Math.Abs(thisday - startday);


            Int64 result = (daysSinceStart * millisecsInDay) - (starttime * millisecsInSecond) + millisecondsSinceMidnight();
            return result;

        }

        public static int timeInSeconds(string time)
        {
            int hour = Convert.ToInt32(LeftRightMid.Left(time, 2));
            int mins = Convert.ToInt32(LeftRightMid.Right(time, 2));
            int secs = (hour * secsInHour) + (mins * secsInMinute);
            return secs;
        }


        public static string currentTime()
        {
            return DateTime.Now.ToString("HHmm", System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string currentDateYYYYMMDD()
        {
            return DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string currentDateTimeSql()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        }

    }
}
