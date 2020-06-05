using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Collections;
using System.Threading;
using System.Diagnostics;







namespace TeboCam
{
    class ftp
    {

        public static event EventHandler UploadError;
        public static event EventHandler UploadSuccess;
        public static event EventHandler GetFileListError;
        public static event EventHandler GetFileListSuccess;
        public static event EventHandler DeleteError;
        public static event EventHandler DeleteSuccess;
        public static IException tebowebException;
        public static bool testFtp = false;
        public static bool testFtpError = false;
        private WebClient _webClient;

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


                //deprecated 20180622
                //if (checkTimes > 0)
                //{
                //    var files = GetFileList();
                //    for (int i = 0; i < checkTimes; i++)
                //    {
                //        if (files.Contains(filename))
                //        {
                //            return true;
                //        }
                //        Thread.Sleep(1000);
                //    }
                //}

                return true;
                //UploadSuccess(null, new EventArgs());
            }
            catch (Exception e)
            {
                //UploadError(null, new EventArgs()); 
                TebocamState.tebowebException.LogException(e);
                TebocamState.log.AddLine("FTP error: Upload");
                if (testFtp) { testFtpError = true; }
                return false;
                //MessageBox.Show(ex.Message, "Upload Error");
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
                //GetFileListSuccess(null, new EventArgs()); 
                return tempList;
            }
            catch (Exception e)
            {
                TebocamState.tebowebException.LogException(e);
                //GetFileListError(null, new EventArgs()); 
                TebocamState.log.AddLine("FTP error: GetFileList");
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
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + fileName));
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
                //DeleteSuccess(null, new EventArgs()); 
            }
            catch (Exception e)
            {
                TebocamState.tebowebException.LogException(e);
                //DeleteError(null, new EventArgs()); 
                TebocamState.log.AddLine("FTP file: " + fileName + " not deleted");
                if (testFtp) { testFtpError = true; };
                //MessageBox.Show(ex.Message, "FTP 2.0 Delete");
                return false;
            }

            return true;

        }
        #endregion




    }
}

