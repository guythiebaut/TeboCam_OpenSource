using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using Ionic.Zip;
using teboweb;


namespace teboweb
{


    class update
    {

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
            catch (Exception)
            {

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
        public static void installUpdateRestart(string downloadsURL, string filename, string destinationFolder, string processToEnd, string postProcess, string startupCommand, string updater, bool webUpdate,string debugFile)
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
            catch (System.Exception)
            {
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



    }
}

