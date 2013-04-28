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


        #region ::::::::::::::::::::::::Upload::::::::::::::::::::::::
        public static bool Upload(string filename, string ftpServerIP, string ftpUserID, string ftpPassword)
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
                //UploadSuccess(null, new EventArgs());
            }
            catch (Exception)
            {
                //UploadError(null, new EventArgs()); 
                bubble.logAddLine("FTP error: Upload");
                if (bubble.testFtp) { bubble.testFtpError = true; }
                return false;
                //MessageBox.Show(ex.Message, "Upload Error");
            }


        }
        #endregion

        #region ::::::::::::::::::::::::GetFileList::::::::::::::::::::::::
        public static ArrayList GetFileList()
        {

            bubble.fileBusy = true;

            string ftpServerIP = config.getProfile(bubble.profileInUse).ftpRoot;
            string ftpUserID = config.getProfile(bubble.profileInUse).ftpUser;
            string ftpPassword = config.getProfile(bubble.profileInUse).ftpPass;
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
                bubble.fileBusy = false;
                //GetFileListSuccess(null, new EventArgs()); 
                return tempList;
            }
            catch 
            {
                //GetFileListError(null, new EventArgs()); 
                bubble.logAddLine("FTP error: GetFileList");
                bubble.fileBusy = false;
                return tempList;
            }


        }
        #endregion

        #region ::::::::::::::::::::::::DeleteFTP::::::::::::::::::::::::
        public static bool DeleteFTP(string fileName, string ftpServerIP, string ftpUserID, string ftpPassword)
        {
            bubble.fileBusy = true;

            try
            {
                string uri = "ftp://" + ftpServerIP + "/" + fileName;
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + fileName));

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;

                string result = String.Empty;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                long size = response.ContentLength;
                Stream datastream = response.GetResponseStream();
                StreamReader sr = new StreamReader(datastream);
                result = sr.ReadToEnd();
                sr.Close();
                datastream.Close();
                response.Close();
                //DeleteSuccess(null, new EventArgs()); 
            }
            catch (Exception)
            {
                //DeleteError(null, new EventArgs()); 
                bubble.fileBusy = false;
                bubble.logAddLine("FTP file: " + fileName + " not deleted");
                if (bubble.testFtp) { bubble.testFtpError = true; };
                //MessageBox.Show(ex.Message, "FTP 2.0 Delete");
                return false;
            }

            bubble.fileBusy = false;
            return true;

        }
        #endregion




    }
}

