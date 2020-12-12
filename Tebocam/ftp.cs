using System;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;

namespace TeboCam
{
    class ftp
    {
        public static IException tebowebException;
        public static ILog log;
        public static bool testFtp = false;
        public static bool testFtpError = false;

        #region ::::::::::::::::::::::::New Upload::::::::::::::::::::::::

        public void UploadFiles(string[] args)
        {
        }

        #endregion



            #region ::::::::::::::::::::::::Upload::::::::::::::::::::::::
        public static bool Upload(string filename, string ftpServerIP, string ftpUserID, string ftpPassword, int checkTimes)
        {

            try
            {
                FileInfo fileInf = new FileInfo(filename);
                FtpWebRequest reqFTP;

                // Create FtpWebRequest object from the Url provided
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new System.Uri("ftp://" + ftpServerIP + "/" + fileInf.Name));

                // Provide the WebPermission Credintials
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                // By default KeepAlive is true, where the control connection is not closed
                // after a command is executed.
                reqFTP.KeepAlive = false;

                // Specify the command to be executed.
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

                // Specify the data transfer type.
                reqFTP.UseBinary = true;

                // Notify the server about the size of the uploaded file
                reqFTP.ContentLength = fileInf.Length;

                // The buffer size is set to 2kb
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;

                // Opens a file stream (System.IO.FileStream) to read the file to be uploaded
                FileStream fs = fileInf.OpenRead();


                // Stream to which the file to be upload is written
                Stream strm = reqFTP.GetRequestStream();

                // Read from the file stream 2kb at a time
                contentLen = fs.Read(buff, 0, buffLength);

                // Till Stream content ends
                while (contentLen != 0)
                {
                    // Write Content from the file stream to the FTP Upload Stream
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }

                // Close the file stream and the Request Stream
                strm.Close();
                fs.Close();
                return true;
            }
            catch (Exception e)
            {
                tebowebException.LogException(e);
                log.AddLine("FTP error: Upload");
                if (testFtp) { testFtpError = true; }
                return false;
            }


        }
        #endregion

        #region ::::::::::::::::::::::::GetFileList::::::::::::::::::::::::
        public static ArrayList GetFileList()
        {
            string ftpServerIP = ConfigurationHelper.GetCurrentProfile().ftpRoot;
            string ftpUserID = ConfigurationHelper.GetCurrentProfile().ftpUser;
            string ftpPassword = ConfigurationHelper.GetCurrentProfile().ftpPass;
            ArrayList tempList = new ArrayList();

            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/"));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());

                string line = reader.ReadLine();
                while (line != null)
                {
                    tempList.Add(line);
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
                return tempList;
            }
            catch (Exception e)
            {
                tebowebException.LogException(e);
                log.AddLine("FTP error: GetFileList");
                return tempList;
            }


        }
        #endregion

        #region ::::::::::::::::::::::::DeleteFTP::::::::::::::::::::::::
        public static bool DeleteFTP(string fileName, string ftpServerIP, string ftpUserID, string ftpPassword, bool getResponse)
        {
            try
            {
                string uri = "ftp://" + ftpServerIP + "/" + fileName;
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + fileName));
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

                if (getResponse)
                {
                    long size = response.ContentLength;
                    Stream datastream = response.GetResponseStream();
                    StreamReader sr = new StreamReader(datastream);
                    string result = sr.ReadToEnd();
                    sr.Close();
                    datastream.Close();
                    response.Close();
                }
            }
            catch (Exception e)
            {
                tebowebException.LogException(e);
                log.AddLine("FTP file: " + fileName + " not deleted");
                if (testFtp) { testFtpError = true; };
                return false;
            }

            return true;

        }
        #endregion
    }
}

