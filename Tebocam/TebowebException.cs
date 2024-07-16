using System;
using System.IO;
namespace TeboCam
{

    public interface IException
    {
        void LogException(Exception a);
    }

    public class TebowebException : IException
    {

        readonly bool _exceptionsLoggedToFile;
        readonly string _exceptionFileName;
        readonly string _exceptionFilePath;

        public TebowebException(string exceptionFilePath, string exceptionFileName)
        {
            if (exceptionFilePath != null)
            {
                _exceptionFilePath = exceptionFilePath;
                _exceptionFileName = exceptionFileName;
                _exceptionsLoggedToFile = true;
            }

        }

        public void LogException(Exception e)
        {
            if (_exceptionsLoggedToFile)
            {
                ExceptionToFile(e.Message + Environment.NewLine + Environment.NewLine + e.StackTrace);
            }
        }

        private void ExceptionToFile(string exception)
        {
            using (StreamWriter w = new StreamWriter(Path.Combine(_exceptionFilePath, _exceptionFileName), true))
            {
                string time = string.Format("[{0}]", DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture));
                w.WriteLine();
                w.WriteLine(time);
                w.WriteLine(exception);
            }
        }

    }
}
