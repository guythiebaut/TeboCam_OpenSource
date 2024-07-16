using System;
using System.Collections.Generic;
using System.IO;

namespace TeboCam
{
    [Serializable]
    public class StastisticalData
    {
        public List<Statistic> StatisticCollection = new List<Statistic>();

        public StastisticalData()
        {
        }

        public Configuration ReadXmlFile(string filename)
        {
            return Serialization.SerializeFromXmlFile<Configuration>(filename);
        }

        public void WriteXmlFile(string filename, Configuration config)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            Serialization.SerializeToXmlFile(filename, config);
        }

        public void NewStat(Statistic stat)
        {
            StatisticCollection.Add(stat);
        }


        [Serializable]
        public class Statistic
        {
            private int cameraId;
            private string cameraName;
            private int motionLevel;
            private int alarmLevel;
            private DateTime dateTime;
        }





    }
}
