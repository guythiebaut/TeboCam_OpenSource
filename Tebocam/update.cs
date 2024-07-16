using Ionic.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TeboCam;

namespace teboweb
{
    public class UpdateInfo
    {
        public string app;
        public string version;
        public string downloadFileUrl;
        public string downloadFile;
        public string newsSeq;
        public string newsFileUrl;
        public string newsFile;
    }

    class update
    {
        public static IException tebowebException;

        /// <summary>Get update and version information from specified online file - returns a List</summary>
        /// <param name="downloadsURL">URL to download file from</param>
        /// <param name="versionFile">Name of the pipe| delimited version file to download</param>
        /// <param name="resourceDownloadFolder">Folder on the local machine to download the version file to</param>
        /// <param name="startLine">Line number, of the version file, to read the version information from</param>
        /// <returns>List containing the information from the pipe delimited version file</returns>
        public static UpdateInfo getUpdateInfo(string product, string downloadsURL, string versionFile, string resourceDownloadFolder, int startLine, bool webDownload)
        {

            var tebocamInfo = new UpdateInfo();

            //create download folder if it does not exist
            if (!Directory.Exists(resourceDownloadFolder))
            {
                Directory.CreateDirectory(resourceDownloadFolder);
            }

            try
            {
                //let's try and download update information from the web
                if (webDownload)
                {
                    webdata.downloadFromWeb(downloadsURL, sensitiveInfo.versionFile, resourceDownloadFolder);
                }
                //let's try and download update information from the network
                else
                {
                    downloadFromNetwork(downloadsURL, versionFile, resourceDownloadFolder);
                }

                string versionJson = string.Empty;
                List<UpdateInfo> info = new List<UpdateInfo>();
                try
                {
                    versionFile = File.ReadAllText(resourceDownloadFolder + "\\" + versionFile);
                    info = JsonConvert.DeserializeObject<List<UpdateInfo>>(versionJson);
                    tebocamInfo = info.Where(x => x.app.ToLower() == product.ToLower()).FirstOrDefault();
                }
                catch (Exception e)
                {
                    TebocamState.tebowebException.LogException(e);
                    tebocamInfo = new UpdateInfo()
                    {
                        app = product,
                        version = "0",
                        downloadFile = string.Empty,
                        downloadFileUrl = string.Empty,
                        newsFile = string.Empty,
                        newsFileUrl = string.Empty,
                        newsSeq = "0"
                    };
                }
            }
            catch (Exception ex)
            {
                TebocamState.tebowebException.LogException(ex);
            }

            return tebocamInfo;
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
        public static void InstallUpdateRestart(string downloadsURL, string filename, string destinationFolder, string processToEnd, string postProcess, string startupCommand, string updater, int webUpdate, string debugFile)
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

        public static UpdateInfo check_for_updates(bool devMachine,
                                                     ref double newsSeq,
                                                     ref Configuration configuration,
                                                     ref Button newsInfo)
        {
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
            var updateInfo = update.getUpdateInfo(sensitiveInfo.product, sensitiveInfo.downloadsURL, versionFile, TebocamState.resourceDownloadFolder, 1, true);

            if (updateInfo == null || updateInfo.version == null)
            {
                //error in update
                updateInfo.version = "0";
                return updateInfo;
            }
            else
            {
                //download the news information file if a new one is available
                if (double.Parse(updateInfo.newsSeq) > newsSeq)
                {
                    try
                    {
                        installUpdateNow(updateInfo.newsFileUrl, updateInfo.newsFile, TebocamState.resourceDownloadFolder, true);

                        //move all the unzipped files out of the download folder into the parent resource folder
                        //leave the zip file where it is to be deleted with the resource download folder
                        DirectoryInfo di = new DirectoryInfo(TebocamState.resourceDownloadFolder);
                        FileInfo[] files = di.GetFiles();

                        foreach (FileInfo fi in files)
                        {
                            if (fi.Name != updateInfo.newsFile)
                                File.Copy(TebocamState.resourceDownloadFolder + fi.Name, TebocamState.resourceFolder + fi.Name, true);
                        }

                        newsSeq = double.Parse(updateInfo.newsSeq);
                        configuration.newsSeq = newsSeq;
                        newsInfo.BackColor = Color.Gold;
                    }
                    catch (Exception e)
                    {
                        TebocamState.tebowebException.LogException(e);
                        return updateInfo;
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
                            return updateInfo;
                        }
                    }
                }
            }

            return updateInfo;
        }

    }
}

