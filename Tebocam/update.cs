using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using Ionic.Zip;
using teboweb;
using TeboCam;
using System.Drawing;
using System.Windows.Forms;

namespace teboweb
{
    class update
    {
        public static IException tebowebException;

        /// <summary>Get update and version information from specified online file - returns a List</summary>
        /// <param name="downloadsURL">URL to download file from</param>
        /// <param name="versionFile">Name of the pipe| delimited version file to download</param>
        /// <param name="resourceDownloadFolder">Folder on the local machine to download the version file to</param>
        /// <param name="startLine">Line number, of the version file, to read the version information from</param>
        /// <returns>List containing the information from the pipe delimited version file</returns>
        public static List<string> getUpdateInfo(string downloadsURL, string versionFile, string resourceDownloadFolder, int startLine, bool webDownload)
        {

            bool updateChecked = false;

            //create download folder if it does not exist
            if (!Directory.Exists(resourceDownloadFolder))
            {

                Directory.CreateDirectory(resourceDownloadFolder);

            }

            //let's try and download update information from the web
            if (webDownload)
            {
                updateChecked = webdata.downloadFromWeb(downloadsURL, versionFile, resourceDownloadFolder);
            }
            //let's try and download update information from the network
            else
            {
                updateChecked = downloadFromNetwork(downloadsURL, versionFile, resourceDownloadFolder);
            }


            //if the download of the file was successful
            if (updateChecked)
            {

                //get information out of download info file
                return populateInfoFromWeb(versionFile, resourceDownloadFolder, startLine);

            }
            //there is a chance that the download of the file was not successful
            else
            {

                return null;

            }

        }


        private static bool downloadFromNetwork(string path, string file, string targetFolder)
        {

            try
            {

                if (!File.Exists(path + file))
                {
                    return false;
                }

                File.Copy(path + file, targetFolder + file, true);
                return true;

            }
            catch (Exception e)
            {
                tebowebException.LogException(e);
                return false;
            }

        }


        /// <summary>Download file from the web immediately</summary>
        /// <param name="downloadsURL">URL to download file from</param>
        /// <param name="filename">Name of the file to download</param>
        /// <param name="downloadTo">Folder on the local machine to download the file to</param>
        /// <param name="unzip">Unzip the contents of the file</param>
        /// <returns>Void</returns>
        public static void installUpdateNow(string downloadsURL, string filename, string downloadTo, bool unzip)
        {

            bool downloadSuccess = webdata.downloadFromWeb(downloadsURL, filename, downloadTo);

            if (unzip)
            {

                unZip(downloadTo + filename, downloadTo);

            }

        }


        /// <summary>Starts the update application passing across relevant information</summary>
        /// <param name="downloadsURL">URL to download file from</param>
        /// <param name="filename">Name of the file to download</param>
        /// <param name="destinationFolder">Folder on the local machine to download the file to</param>
        /// <param name="processToEnd">Name of the process to end before applying the updates</param>
        /// <param name="postProcess">Name of the process to restart</param>
        /// <param name="startupCommand">Command line to be passed to the process to restart</param>
        /// <param name="updater"></param>
        /// <returns>Void</returns>
        public static void installUpdateRestart(string downloadsURL, string filename, string destinationFolder, string processToEnd, string postProcess, string startupCommand, string updater, int webUpdate,string debugFile)
        {

            string cmdLn = "";

            cmdLn += "|webUpdate|" + webUpdate.ToString();
            cmdLn += "|downloadFile|" + filename;
            cmdLn += "|URL|" + downloadsURL;
            cmdLn += "|destinationFolder|" + destinationFolder;
            cmdLn += "|processToEnd|" + processToEnd;
            cmdLn += "|postProcess|" + postProcess;
            cmdLn += "|command|" + @"/ " + startupCommand;
            cmdLn += "|debugpath|" + debugFile;

            cmdLn = CommandLine.finaliseCommandLine(cmdLn);

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = updater;
            startInfo.Arguments = cmdLn;
            Process.Start(startInfo);
        }



        private static List<string> populateInfoFromWeb(string versionFile, string resourceDownloadFolder, int line)
        {

            List<string> tempList = new List<string>();
            int ln;

            ln = 0;

            foreach (string strline in File.ReadAllLines(resourceDownloadFolder + versionFile))
            {

                if (ln == line)
                {

                    string[] parts = strline.Split('|');
                    foreach (string part in parts)
                    {

                        tempList.Add(part);

                    }

                    return tempList;

                }

                ln++;
            }


            return null;

        }



        private static bool unZip(string file, string unZipTo)//, bool deleteZipOnCompletion)
        {
            try
            {

                // Specifying Console.Out here causes diagnostic msgs to be sent to the Console
                // In a WinForms or WPF or Web app, you could specify nothing, or an alternate
                // TextWriter to capture diagnostic messages. 

                using (ZipFile zip = ZipFile.Read(file))
                {
                    // This call to ExtractAll() assumes:
                    //   - none of the entries are password-protected.
                    //   - want to extract all entries to current working directory
                    //   - none of the files in the zip already exist in the directory;
                    //     if they do, the method will throw.
                    zip.ExtractAll(unZipTo);
                }

                //if (deleteZipOnCompletion) File.Delete(unZipTo + file);

            }
            catch (Exception e)
            {
                TebocamState.tebowebException.LogException(e);
                return false;
            }

            return true;

        }

        /// <summary>Updates the update application by renaming prefixed files</summary>
        /// <param name="updaterPrefix">Prefix of files to be renamed</param>
        /// <param name="containingFolder">Folder on the local machine where the prefixed files exist</param>
        /// <returns>Void</returns>
        public static void updateMe(string updaterPrefix, string containingFolder)
        {

            DirectoryInfo dInfo = new DirectoryInfo(containingFolder);
            FileInfo[] updaterFiles = dInfo.GetFiles(updaterPrefix + "*");
            int fileCount = updaterFiles.Length;

            foreach (FileInfo file in updaterFiles)
            {

                string newFile = containingFolder + file.Name;
                string origFile = containingFolder + @"\" + file.Name.Substring(updaterPrefix.Length, file.Name.Length - updaterPrefix.Length);

                if (File.Exists(origFile)) { File.Delete(origFile); }

                File.Move(newFile, origFile);

            }

        }

        public static List<string> check_for_updates(bool devMachine, 
                                                     ref double newsSeq,
                                                     ref Configuration configuration, 
                                                     ref Button newsInfo)
        {
            List<string> updateDat = new List<string>();
            string versionFile = "";

            //set version file depending on machine installation
            if (!devMachine)
            {
                versionFile = sensitiveInfo.versionFile;
            }
            else
            {
                versionFile = sensitiveInfo.versionFileDev;
            }

            //get the update information into a List
            updateDat = update.getUpdateInfo(sensitiveInfo.downloadsURL, versionFile, TebocamState.resourceDownloadFolder, 1, true);

            if (updateDat == null)
            {
                //error in update
                List<string> err = new List<string>();
                err.Add("");
                err.Add("0");
                return err;
            }
            else
            {
                //download the news information file if a new one is available
                if (double.Parse(updateDat[4]) > newsSeq)
                {
                    update.installUpdateNow(updateDat[5], updateDat[6], TebocamState.resourceDownloadFolder, true);

                    try
                    {
                        //move all the unzipped files out of the download folder into the parent resource folder
                        //leave the zip file where it is to be deleted with the resource download folder
                        DirectoryInfo di = new DirectoryInfo(TebocamState.resourceDownloadFolder);
                        FileInfo[] files = di.GetFiles();

                        foreach (FileInfo fi in files)
                        {
                            if (fi.Name != updateDat[6])
                                File.Copy(TebocamState.resourceDownloadFolder + fi.Name, TebocamState.resourceFolder + fi.Name,
                                    true);
                        }

                        newsSeq = double.Parse(updateDat[4]);
                        configuration.newsSeq = double.Parse(updateDat[4]);
                        newsInfo.BackColor = Color.Gold;
                    }
                    catch (Exception e)
                    {
                        TebocamState.tebowebException.LogException(e);
                        return updateDat;
                    }

                    if (Directory.Exists(TebocamState.resourceDownloadFolder))
                    {
                        try
                        {
                            Directory.Delete(TebocamState.resourceDownloadFolder, true);
                        }
                        catch (Exception e)
                        {
                            TebocamState.tebowebException.LogException(e);
                            return updateDat;
                        }
                    }
                }
            }

            return updateDat;
        }

    }
}

