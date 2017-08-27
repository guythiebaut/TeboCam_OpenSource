using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace TeboCam
{

    [Serializable]
    public class Log
    {

        public List<logLine> Lines = new List<logLine>();
        public string FileName;

        public Log()
        { }


        public Log(string FileName)
        {

            this.FileName = FileName;

        }

        public Log ReadXMLFile(string filename)
        {
            return (Log)Serialization.SerializeFromXMLFile(filename, this);
        }

        public void WriteXMLFile(string filename, Log log)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            Serialization.SerializeToXMLFile(filename, log);
        }


        public void AddLine(DateTime dateTime, string message)
        {

            Lines.Add(new logLine() { DT = dateTime, Message = message });

        }

        public long Count()
        {
            return Lines.Count();
        }

        public void Clear()
        {
            Lines.Clear();
        }

    }

    public class logLine
    {

        public DateTime DT;
        public string Message;

    }



}
