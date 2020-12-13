using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace TeboCam
{
    public interface ILog
    {
        Log ReadXMLFile(string filename);
        void WriteXMLFile(string filename, ILog log);
        void AddLine(string message);
        void AddLine(DateTime dateTime, string message);
    }

    [Serializable]
    public class Log : ILog
    {

        public List<logLine> Lines = new List<logLine>();
        public string FileName;
        public event EventHandler LogAdded;

        public Log()
        { }

        public Log(string FileName)
        {

            this.FileName = FileName;

        }

        public Log ReadXMLFile(string filename)
        {
            return Serialization.SerializeFromXmlFile<Log>(filename);
        }

        public void WriteXMLFile(string filename, ILog log)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            Serialization.SerializeToXmlFile(filename, log);
        }

        public void AddLine(string message)
        {
            AddLine(DateTime.Now, message);
        }

        public void AddLine(DateTime dateTime, string message)
        {
            Lines.Add(new logLine() { DT = dateTime, Message = message });
            LogAdded?.Invoke(null, new EventArgs());
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
