
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;
//using Microsoft.VisualBasic;
using System.ComponentModel;

namespace TeboCam
{

    class FileManager
    {
        static BackgroundWorker bw = new BackgroundWorker();
        public static string configFile = "config";
        public static string graphFile = "graph";
        public static string logFile = "log";
        public static string keyFile = "licence";
        public static string testFile = "test";




        #region ::::::::::::::::::::::::backupFile::::::::::::::::::::::::
        public static void backupFile(string file)
        {
            string path = "";

            if (file == "graph")
            {
                path = bubble.xmlFolder + graphFile;
            }

            if (file == "log")
            {
                path = bubble.xmlFolder + logFile;
            }

            if (file == "config")
            {
                path = bubble.xmlFolder + configFile;
            }

            File.Copy(path + ".xml", path + ".bak", true);

        }
        #endregion


        #region ::::::::::::::::::::::::readXmlFile::::::::::::::::::::::::
        public static bool readXmlFile(string file, bool fromBackup)
        {

            string suffix;
            int result = 0;

            if (fromBackup)
            {
                suffix = ".bak";
            }
            else
            {
                suffix = ".xml";
            }

            if (file == "config")
            {
                result = ReadFile(file, bubble.xmlFolder + configFile + suffix);
            }

            if (file == "licenceKey")
            {
                result = ReadFile(file, bubble.xmlFolder + keyFile + suffix);
            }


            return result == 1;

        }

        #endregion




        #region ::::::::::::::::::::::::clearLog::::::::::::::::::::::::
        public static void clearLog()
        {
            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);
            bubble.logAddLine("Making copy of log file with time stamp - " + timeStamp);
            WriteFile("log");
            File.Copy(bubble.xmlFolder + "log.xml", bubble.logFolder + @"\log_" + timeStamp + ".xml");
            bubble.log.Clear();
            bubble.logAddLine("Previous log file archived.");
            WriteFile("log");
        }
        #endregion
        #region ::::::::::::::::::::::::clearFiles::::::::::::::::::::::::


        public static void clearFiles(string folder)
        {

            bubble.logAddLine("Getting list of computer files from " + folder);
            string[] files = Directory.GetFiles(folder);
            bubble.logAddLine("Starting delete of computer files from " + folder);

            if (!bubble.fileBusy)
            {
                bubble.fileBusy = true;
                foreach (string file in files)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch
                    {
                        bubble.logAddLine("Error in deleting files from " + folder);
                    }
                }
                bubble.fileBusy = false;
            }
            bubble.logAddLine("Deletion of computer files completed from " + folder);
        }

        #endregion

        #region ::::::::::::::::::::::::clearFtp::::::::::::::::::::::::
        private static void clearFtpWork(object sender, DoWorkEventArgs e)
        {
            try
            {

                bubble.logAddLine("Getting list of web files.");
                ArrayList ftpFiles = ftp.GetFileList();

                bubble.logAddLine("Starting delete of web files via ftp.");
                int tmpInt = 0;
                foreach (string img in ftpFiles)
                {


                    if (img.Length > config.getProfile(bubble.profileInUse).filenamePrefix.Length + bubble.ImgSuffix.Length)
                    {
                        if (LeftRightMid.Left(img, config.getProfile(bubble.profileInUse).filenamePrefix.Length) == config.getProfile(bubble.profileInUse).filenamePrefix && LeftRightMid.Right(img, bubble.ImgSuffix.Length) == bubble.ImgSuffix)
                        {
                            tmpInt++;
                        }
                    }
                }
                bubble.logAddLine(tmpInt.ToString() + " web files to delete via ftp.");


                //List of all files on ftp site
                foreach (string img in ftpFiles)
                {

                    //if the prefix and suffix correspond to TeboCam image files then delete this file
                    if (img.Length > config.getProfile(bubble.profileInUse).filenamePrefix.Length + bubble.ImgSuffix.Length)
                    {
                        if (LeftRightMid.Left(img, config.getProfile(bubble.profileInUse).filenamePrefix.Length) == config.getProfile(bubble.profileInUse).filenamePrefix && LeftRightMid.Right(img, bubble.ImgSuffix.Length) == bubble.ImgSuffix)
                        {
                            ftp.DeleteFTP(img, config.getProfile(bubble.profileInUse).ftpRoot, config.getProfile(bubble.profileInUse).ftpUser, config.getProfile(bubble.profileInUse).ftpPass, false);
                            tmpInt--;
                            bubble.logAddLine(tmpInt.ToString() + " web files left to delete via ftp.");
                        }
                    }
                }
                bubble.logAddLine("Deletion of web files via ftp completed.");
            }
            catch
            {
                bubble.logAddLine("Error in deleting web files.");
            }
        }

        public static void clearFtp()
        {
            bw.DoWork -= new DoWorkEventHandler(clearFtpWork);
            bw.DoWork += new DoWorkEventHandler(clearFtpWork);
            bw.WorkerSupportsCancellation = true;
            bw.RunWorkerAsync();
        }
        #endregion


        #region ::::::::::::::::::::::::ReadFile::::::::::::::::::::::::



        private static int ReadFile(string file, string path)
        {
                 
            #region ::::::::::::::::::::::::Read Config::::::::::::::::::::::::


            if (file == "config")
            {
                string fileName = path;// bubble.xmlFolder + configFile + ".xml";

                XmlTextReader configDataCheck = new XmlTextReader(fileName);

                string profileVer = "";

                bool newConfig = false;

                try
                {

                    while (configDataCheck.Read())
                    {
                        if (configDataCheck.NodeType == XmlNodeType.Element)
                        {
                            //###
                            if (configDataCheck.LocalName.Equals("version"))
                            {
                                profileVer = configDataCheck.ReadString();
                                newConfig = true;
                                break;
                            }
                        }
                    }
                    configDataCheck.Close();

                }
                catch (Exception e)
                {
                    configDataCheck.Close();
                    MessageBox.Show(e.ToString());
                    return 0;
                }

                config.addProfile();
                bool firstProfile = true;
                XmlTextReader configData = new XmlTextReader(fileName);
                string profileName = "";


                try
                {

                    while (configData.Read())
                    {
                        if (configData.NodeType == XmlNodeType.Element)
                        {


                            //profile header
                            if (configData.LocalName.Equals("profileStart"))
                            {
                                //add a new profile
                                if (!firstProfile)
                                {
                                    config.addProfile();
                                }
                                profileName = configData.ReadString().ToLower();

                            }
                            //profile header

                            //profile footer
                            if (configData.LocalName.Equals("profileEnd"))
                            {

                                config.getProfile("##newProf##").profileVersion = profileVer;
                                config.getProfile("##newProf##").profileName = profileName.ToLower();
                                firstProfile = false;

                            }
                            //profile footer



                            if (configData.LocalName.Equals("profileInUse"))
                            {
                                bubble.profileInUse = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("newsSeq"))
                            {
                                bubble.newsSeq = Convert.ToInt32(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("mysqlDriver"))
                            {
                                bubble.mysqlDriver = configData.ReadString();
                            }


                            if (configData.LocalName.Equals("sentByName"))
                            {
                                config.getProfile("##newProf##").sentByName = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("pingSubject"))
                            {
                                config.getProfile("##newProf##").pingSubject = configData.ReadString();
                            }

                            //!!!!!!!!!!!!!!!!!!!!!!!!!!
                            //Webcam individual settings
                            //!!!!!!!!!!!!!!!!!!!!!!!!!!

                            if (configData.LocalName.Equals(CameraRig.infoEnum.webcam.ToString()))
                            {
                                config.getProfile("##newProf##").webcam = configData.ReadString();
                                CameraRig.addInfo(CameraRig.infoEnum.webcam, config.getProfile("##newProf##").webcam);
                                CameraRig.addInfo(CameraRig.infoEnum.profileName, profileName.ToLower());
                            }

                            if (configData.LocalName.Equals(CameraRig.infoEnum.alarmActive.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.alarmActive, Convert.ToBoolean(configData.ReadString()));
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.publishActive.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.publishActive, Convert.ToBoolean(configData.ReadString()));
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.friendlyName.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.friendlyName, configData.ReadString());
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.displayButton.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.displayButton, Convert.ToInt32(configData.ReadString()));
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.areaDetection.ToString()))
                            {
                                config.getProfile("##newProf##").areaDetection = Convert.ToBoolean(configData.ReadString());
                                CameraRig.addInfo(CameraRig.infoEnum.areaDetection, config.getProfile("##newProf##").areaDetection);
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.areaDetectionWithin.ToString()))
                            {
                                config.getProfile("##newProf##").areaDetectionWithin = Convert.ToBoolean(configData.ReadString());
                                CameraRig.addInfo(CameraRig.infoEnum.areaDetectionWithin, config.getProfile("##newProf##").areaDetectionWithin);
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.areaOffAtMotion.ToString()))
                            {
                                config.getProfile("##newProf##").areaOffAtMotion = Convert.ToBoolean(configData.ReadString());
                                CameraRig.addInfo(CameraRig.infoEnum.areaOffAtMotion, config.getProfile("##newProf##").areaOffAtMotion);
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.rectX.ToString()))
                            {
                                config.getProfile("##newProf##").rectX = Convert.ToInt32(configData.ReadString());
                                CameraRig.addInfo(CameraRig.infoEnum.rectX, config.getProfile("##newProf##").rectX);
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.rectY.ToString()))
                            {
                                config.getProfile("##newProf##").rectY = Convert.ToInt32(configData.ReadString());
                                CameraRig.addInfo(CameraRig.infoEnum.rectY, config.getProfile("##newProf##").rectY);
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.rectWidth.ToString()))
                            {
                                config.getProfile("##newProf##").rectWidth = Convert.ToInt32(configData.ReadString());
                                CameraRig.addInfo(CameraRig.infoEnum.rectWidth, config.getProfile("##newProf##").rectWidth);
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.rectHeight.ToString()))
                            {
                                config.getProfile("##newProf##").rectHeight = Convert.ToInt32(configData.ReadString());
                                CameraRig.addInfo(CameraRig.infoEnum.rectHeight, config.getProfile("##newProf##").rectHeight);
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.movementVal.ToString()))
                            {
                                config.getProfile("##newProf##").movementVal = Convert.ToDouble(configData.ReadString());
                                CameraRig.addInfo(CameraRig.infoEnum.movementVal, config.getProfile("##newProf##").movementVal);
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.timeSpike.ToString()))
                            {
                                config.getProfile("##newProf##").timeSpike = Convert.ToInt32(configData.ReadString());
                                CameraRig.addInfo(CameraRig.infoEnum.timeSpike, config.getProfile("##newProf##").timeSpike);
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.toleranceSpike.ToString()))
                            {
                                config.getProfile("##newProf##").toleranceSpike = Convert.ToInt32(configData.ReadString());
                                CameraRig.addInfo(CameraRig.infoEnum.toleranceSpike, config.getProfile("##newProf##").toleranceSpike);
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.lightSpike.ToString()))
                            {
                                config.getProfile("##newProf##").lightSpike = Convert.ToBoolean(configData.ReadString());
                                CameraRig.addInfo(CameraRig.infoEnum.lightSpike, config.getProfile("##newProf##").lightSpike);
                            }


                            if (configData.LocalName.Equals(CameraRig.infoEnum.pubTime.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.pubTime, Convert.ToInt32(configData.ReadString()));
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.pubHours.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.pubHours, Convert.ToBoolean(configData.ReadString()));
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.pubMins.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.pubMins, Convert.ToBoolean(configData.ReadString()));
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.pubSecs.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.pubSecs, Convert.ToBoolean(configData.ReadString()));
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.publishWeb.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.publishWeb, Convert.ToBoolean(configData.ReadString()));
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.publishLocal.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.publishLocal, Convert.ToBoolean(configData.ReadString()));
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.timerOn.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.timerOn, Convert.ToBoolean(configData.ReadString()));
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.fileURLPubWeb.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.fileURLPubWeb, configData.ReadString());
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.filenamePrefixPubWeb.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.filenamePrefixPubWeb, configData.ReadString());
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.cycleStampCheckedPubWeb.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.cycleStampCheckedPubWeb, Convert.ToInt32(configData.ReadString()));
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.startCyclePubWeb.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.startCyclePubWeb, Convert.ToInt32(configData.ReadString()));
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.endCyclePubWeb.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.endCyclePubWeb, Convert.ToInt32(configData.ReadString()));
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.currentCyclePubWeb.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.currentCyclePubWeb, Convert.ToInt32(configData.ReadString()));
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.stampAppendPubWeb.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.stampAppendPubWeb, Convert.ToBoolean(configData.ReadString()));
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.fileAlertPubLoc.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.fileAlertPubLoc, configData.ReadString());
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.fileAlertPubCust.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.fileAlertPubCust, Convert.ToBoolean(configData.ReadString()));
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.fileDirPubLoc.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.fileDirPubLoc, configData.ReadString());
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.fileDirPubCust.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.fileDirPubCust, Convert.ToBoolean(configData.ReadString()));
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.filenamePrefixPubLoc.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.filenamePrefixPubLoc, configData.ReadString());
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.cycleStampCheckedPubLoc.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.cycleStampCheckedPubLoc, Convert.ToInt32(configData.ReadString()));
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.startCyclePubLoc.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.startCyclePubLoc, Convert.ToInt32(configData.ReadString()));
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.endCyclePubLoc.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.endCyclePubLoc, Convert.ToInt32(configData.ReadString()));
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.currentCyclePubLoc.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.currentCyclePubLoc, Convert.ToInt32(configData.ReadString()));
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.stampAppendPubLoc.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.stampAppendPubLoc, Convert.ToBoolean(configData.ReadString()));
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.ipWebcamAddress.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.ipWebcamAddress, configData.ReadString());
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.ipWebcamUser.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.ipWebcamUser, decrypt(configData.ReadString()));
                            }
                            if (configData.LocalName.Equals(CameraRig.infoEnum.ipWebcamPassword.ToString()))
                            {
                                CameraRig.addInfo(CameraRig.infoEnum.ipWebcamPassword, decrypt(configData.ReadString()));
                            }





                            //!!!!!!!!!!!!!!!!!!!!!!!!!!
                            //Webcam individual settings
                            //!!!!!!!!!!!!!!!!!!!!!!!!!!

                            if (configData.LocalName.Equals("freezeGuard"))
                            {
                                config.getProfile("##newProf##").freezeGuard = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("freezeGuardWindowShow"))
                            {
                                config.getProfile("##newProf##").freezeGuardWindowShow = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("updatesNotify"))
                            {
                                config.getProfile("##newProf##").updatesNotify = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("countdownNow"))
                            {
                                config.getProfile("##newProf##").countdownNow = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("cycleStamp"))
                            {
                                config.getProfile("##newProf##").cycleStamp = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("activatecountdown"))
                            {
                                config.getProfile("##newProf##").activatecountdown = Convert.ToInt32(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("activatecountdownTime"))
                            {
                                config.getProfile("##newProf##").activatecountdownTime = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("countdownTime"))
                            {
                                config.getProfile("##newProf##").countdownTime = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("alert"))
                            {
                                config.getProfile("##newProf##").AlertOnStartup = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("maxImagesToEmail"))
                            {
                                config.getProfile("##newProf##").maxImagesToEmail = Convert.ToInt64(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("ping"))
                            {
                                config.getProfile("##newProf##").ping = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("pingAll"))
                            {
                                config.getProfile("##newProf##").pingAll = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("pingInterval"))
                            {
                                config.getProfile("##newProf##").pingInterval = Convert.ToInt32(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("captureMovementImages"))
                            {
                                config.getProfile("##newProf##").captureMovementImages = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("sendNotifyEmail"))
                            {
                                config.getProfile("##newProf##").sendNotifyEmail = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("sendFullSizeImages"))
                            {
                                config.getProfile("##newProf##").sendFullSizeImages = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("sendThumbnailImages"))
                            {
                                config.getProfile("##newProf##").sendThumbnailImages = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("sendMosaicImages"))
                            {
                                config.getProfile("##newProf##").sendMosaicImages = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("mosaicImagesPerRow"))
                            {
                                config.getProfile("##newProf##").mosaicImagesPerRow = Convert.ToInt32(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("loadImagesToFtp"))
                            {
                                config.getProfile("##newProf##").loadImagesToFtp = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("baselineVal"))
                            {
                                config.getProfile("##newProf##").baselineVal = Convert.ToDouble(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("imageSaveInterval"))
                            {
                                config.getProfile("##newProf##").imageSaveInterval = Convert.ToDouble(configData.ReadString());
                            }


                            if (configData.LocalName.Equals("filenamePrefix"))
                            {
                                config.getProfile("##newProf##").filenamePrefix = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("cycleStampChecked"))
                            {
                                config.getProfile("##newProf##").cycleStampChecked = Convert.ToInt32(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("startCycle"))
                            {
                                config.getProfile("##newProf##").startCycle = Convert.ToInt64(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("endCycle"))
                            {
                                config.getProfile("##newProf##").endCycle = Convert.ToInt64(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("currentCycle"))
                            {
                                config.getProfile("##newProf##").currentCycle = Convert.ToInt64(configData.ReadString());
                            }


                            if (configData.LocalName.Equals("emailNotifyInterval"))
                            {
                                config.getProfile("##newProf##").emailNotifyInterval = Convert.ToInt64(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("emailUser"))
                            {
                                config.getProfile("##newProf##").emailUser = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("emailPassword"))
                            {
                                config.getProfile("##newProf##").emailPass = decrypt(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("lockdownPassword"))
                            {
                                config.getProfile("##newProf##").lockdownPassword = decrypt(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("lockdownOn"))
                            {
                                config.getProfile("##newProf##").lockdownOn = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("smtpHost"))
                            {
                                config.getProfile("##newProf##").smtpHost = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("stmpPort"))
                            {
                                config.getProfile("##newProf##").smtpPort = Convert.ToInt32(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("ssl"))
                            {
                                config.getProfile("##newProf##").EnableSsl = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("sendTo"))
                            {
                                config.getProfile("##newProf##").sendTo = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("replyTo"))
                            {
                                config.getProfile("##newProf##").replyTo = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("sentBy"))
                            {
                                config.getProfile("##newProf##").sentBy = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("mailSubject"))
                            {
                                config.getProfile("##newProf##").mailSubject = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("mailBody"))
                            {
                                config.getProfile("##newProf##").mailBody = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("ftpUser"))
                            {
                                config.getProfile("##newProf##").ftpUser = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("ftpPass"))
                            {
                                config.getProfile("##newProf##").ftpPass = decrypt(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("ftpRoot"))
                            {
                                config.getProfile("##newProf##").ftpRoot = configData.ReadString();
                            }

                            if (configData.LocalName.Equals("pubImage"))
                            {
                                config.getProfile("##newProf##").pubImage = Convert.ToBoolean(configData.ReadString());
                            }


                            if (configData.LocalName.Equals("pubFtpUser"))
                            {
                                config.getProfile("##newProf##").pubFtpUser = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("pubFtpPass"))
                            {
                                config.getProfile("##newProf##").pubFtpPass = decrypt(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("pubFtpRoot"))
                            {
                                config.getProfile("##newProf##").pubFtpRoot = configData.ReadString();
                            }

                            //20101026 can be dropped after 201101 as variables should no longer be present
                            if (configData.LocalName.Equals("pubStamp"))
                            {
                                config.getProfile("##newProf##").pubStamp = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("pubStampDate"))
                            {
                                config.getProfile("##newProf##").pubStampDate = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("pubStampTime"))
                            {
                                config.getProfile("##newProf##").pubStampTime = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("pubStampDateTime"))
                            {
                                config.getProfile("##newProf##").pubStampDateTime = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("timerStartPub"))
                            {
                                config.getProfile("##newProf##").timerStartPub = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("timerEndPub"))
                            {
                                config.getProfile("##newProf##").timerEndPub = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("timerOnMov"))
                            {
                                config.getProfile("##newProf##").timerOnMov = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("scheduleOnAtStart"))
                            {
                                config.getProfile("##newProf##").scheduleOnAtStart = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("activateAtEveryStartup"))
                            {
                                config.getProfile("##newProf##").activateAtEveryStartup = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("timerStartMov"))
                            {
                                config.getProfile("##newProf##").timerStartMov = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("timerEndMov"))
                            {
                                config.getProfile("##newProf##").timerEndMov = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("webUpd"))
                            {
                                config.getProfile("##newProf##").webUpd = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("webUser"))
                            {
                                config.getProfile("##newProf##").webUser = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("webPass"))
                            {
                                config.getProfile("##newProf##").webPass = decrypt(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("webFtpPass"))
                            {
                                config.getProfile("##newProf##").webFtpPass = decrypt(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("webFtpUser"))
                            {
                                config.getProfile("##newProf##").webFtpUser = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("webPoll"))
                            {
                                config.getProfile("##newProf##").webPoll = Convert.ToInt32(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("webInstance"))
                            {
                                config.getProfile("##newProf##").webInstance = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("webImageRoot"))
                            {
                                config.getProfile("##newProf##").webImageRoot = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("webImageFileName"))
                            {
                                config.getProfile("##newProf##").webImageFileName = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("soundAlert"))
                            {
                                config.getProfile("##newProf##").soundAlert = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("soundAlertOn"))
                            {
                                config.getProfile("##newProf##").soundAlertOn = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("logsKeep"))
                            {
                                config.getProfile("##newProf##").logsKeep = Convert.ToInt32(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("logsKeepChk"))
                            {
                                config.getProfile("##newProf##").logsKeepChk = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("imageParentFolderCust"))
                            {
                                config.getProfile("##newProf##").imageParentFolderCust = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("imageFolderCust"))
                            {
                                config.getProfile("##newProf##").imageFolderCust = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("thumbFolderCust"))
                            {
                                config.getProfile("##newProf##").thumbFolderCust = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("imageLocCust"))
                            {
                                config.getProfile("##newProf##").imageLocCust = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("startTeboCamMinimized"))
                            {
                                config.getProfile("##newProf##").startTeboCamMinimized = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("internetCheck"))
                            {
                                config.getProfile("##newProf##").internetCheck = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("toolTips"))
                            {
                                config.getProfile("##newProf##").toolTips = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("alertCompression"))
                            {
                                config.getProfile("##newProf##").alertCompression = Convert.ToInt32(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("publishCompression"))
                            {
                                config.getProfile("##newProf##").publishCompression = Convert.ToInt32(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("pingCompression"))
                            {
                                config.getProfile("##newProf##").pingCompression = Convert.ToInt32(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("onlineCompression"))
                            {
                                config.getProfile("##newProf##").onlineCompression = Convert.ToInt32(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("alertTimeStamp"))
                            {
                                config.getProfile("##newProf##").alertTimeStamp = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("alertTimeStampFormat"))
                            {
                                config.getProfile("##newProf##").alertTimeStampFormat = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("alertStatsStamp"))
                            {
                                config.getProfile("##newProf##").alertStatsStamp = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("alertTimeStampColour"))
                            {
                                config.getProfile("##newProf##").alertTimeStampColour = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("alertTimeStampPosition"))
                            {
                                config.getProfile("##newProf##").alertTimeStampPosition = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("alertTimeStampRect"))
                            {
                                config.getProfile("##newProf##").alertTimeStampRect = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("publishTimeStamp"))
                            {
                                config.getProfile("##newProf##").publishTimeStamp = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("publishTimeStampFormat"))
                            {
                                config.getProfile("##newProf##").publishTimeStampFormat = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("publishStatsStamp"))
                            {
                                config.getProfile("##newProf##").publishStatsStamp = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("publishTimeStampColour"))
                            {
                                config.getProfile("##newProf##").publishTimeStampColour = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("publishTimeStampPosition"))
                            {
                                config.getProfile("##newProf##").publishTimeStampPosition = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("publishTimeStampRect"))
                            {
                                config.getProfile("##newProf##").publishTimeStampRect = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("pingTimeStamp"))
                            {
                                config.getProfile("##newProf##").pingTimeStamp = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("pingTimeStampFormat"))
                            {
                                config.getProfile("##newProf##").pingTimeStampFormat = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("pingStatsStamp"))
                            {
                                config.getProfile("##newProf##").pingStatsStamp = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("pingTimeStampColour"))
                            {
                                config.getProfile("##newProf##").pingTimeStampColour = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("pingTimeStampPosition"))
                            {
                                config.getProfile("##newProf##").pingTimeStampPosition = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("pingTimeStampRect"))
                            {
                                config.getProfile("##newProf##").pingTimeStampRect = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("onlineTimeStamp"))
                            {
                                config.getProfile("##newProf##").onlineTimeStamp = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("onlineTimeStampFormat"))
                            {
                                config.getProfile("##newProf##").onlineTimeStampFormat = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("onlineStatsStamp"))
                            {
                                config.getProfile("##newProf##").onlineStatsStamp = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("onlineTimeStampColour"))
                            {
                                config.getProfile("##newProf##").onlineTimeStampColour = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("onlineTimeStampPosition"))
                            {
                                config.getProfile("##newProf##").onlineTimeStampPosition = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("onlineTimeStampRect"))
                            {
                                config.getProfile("##newProf##").onlineTimeStampRect = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("imageToframe"))
                            {
                                config.getProfile("##newProf##").imageToframe = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("cameraShow"))
                            {
                                config.getProfile("##newProf##").cameraShow = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("motionLevel"))
                            {
                                config.getProfile("##newProf##").motionLevel = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("selectedCam"))
                            {
                                config.getProfile("##newProf##").selectedCam = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("pulseFreq"))
                            {
                                config.getProfile("##newProf##").pulseFreq = Convert.ToDecimal(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("EmailIntelOn"))
                            {
                                config.getProfile("##newProf##").EmailIntelOn = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("emailIntelEmails"))
                            {
                                config.getProfile("##newProf##").emailIntelEmails = Convert.ToInt32(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("emailIntelMins"))
                            {
                                config.getProfile("##newProf##").emailIntelMins = Convert.ToInt32(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("EmailIntelStop"))
                            {
                                config.getProfile("##newProf##").EmailIntelStop = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("disCommOnline"))
                            {
                                config.getProfile("##newProf##").disCommOnline = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("disCommOnlineSecs"))
                            {
                                config.getProfile("##newProf##").disCommOnlineSecs = Convert.ToInt32(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("StatsToFileOn"))
                            {
                                config.getProfile("##newProf##").StatsToFileOn = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("StatsToFileLocation"))
                            {
                                config.getProfile("##newProf##").StatsToFileLocation = configData.ReadString();
                            }
                            if (configData.LocalName.Equals("StatsToFileTimeStamp"))
                            {
                                config.getProfile("##newProf##").StatsToFileTimeStamp = Convert.ToBoolean(configData.ReadString());
                            }
                            if (configData.LocalName.Equals("StatsToFileMb"))
                            {
                                config.getProfile("##newProf##").StatsToFileMb = Convert.ToDouble(configData.ReadString());
                            }



                        }

                    }

                    if (!newConfig)
                    {
                        config.getProfile("##newProf##").profileName = "main";
                        bubble.profileInUse = "main";
                    }


                    configData.Close();


                }
                catch (Exception e)
                {
                    configData.Close();
                    MessageBox.Show(e.ToString());
                    return 0;
                }

            }
            return 1;
            #endregion
        }




        #endregion

        #region ::::::::::::::::::::::::WriteFile::::::::::::::::::::::::

        public static void WriteFile(string file)
        {
                    
                   
            #region ::::::::::::::::::::::::Write Training::::::::::::::::::::::::
            if (file == "training")
            {
                try
                {
                    string fileName = bubble.xmlFolder + "training.xml";
                    XmlTextWriter trainingData = new XmlTextWriter(fileName, null);

                    trainingData.Formatting = Formatting.Indented;
                    trainingData.Indentation = 4;
                    trainingData.Namespaces = false;
                    trainingData.WriteStartDocument();

                    trainingData.WriteStartElement("", "training", "");

                    //###
                    foreach (double level in bubble.training)
                    {
                        trainingData.WriteStartElement("", "level", "");
                        trainingData.WriteString(level.ToString());
                        trainingData.WriteEndElement();
                    }

                    trainingData.WriteEndElement();
                    trainingData.WriteEndDocument();
                    trainingData.Flush();
                    trainingData.Close();

                }
                catch
                {

                }
            }
            #endregion
            #region ::::::::::::::::::::::::Write TestFile::::::::::::::::::::::::

            if (file == "test")
            {
                try
                {

                    string fileName = bubble.xmlFolder + testFile + ".xml";
                    XmlTextWriter testData = new XmlTextWriter(fileName, null);

                    testData.Formatting = Formatting.Indented;
                    testData.Indentation = 4;
                    testData.Namespaces = false;
                    testData.WriteStartDocument();

                    testData.WriteStartElement("", "TestData", "");
                    //###
                    testData.WriteStartElement("", "TestData", "");
                    testData.WriteString("Test data");
                    testData.WriteEndElement();

                    testData.WriteStartElement("", "TestData", "");
                    testData.WriteString("Test data");
                    testData.WriteEndElement();

                    testData.WriteStartElement("", "TestData", "");
                    testData.WriteString("Test data");
                    testData.WriteEndElement();
                    //###
                    testData.WriteEndElement();
                    testData.WriteEndDocument();
                    testData.Flush();
                    testData.Close();

                }
                catch
                {
                }
            }
            #endregion


            #region ::::::::::::::::::::::::Write Config::::::::::::::::::::::::



            if (file == "config")
            {
                try
                {

                    string fileName = bubble.xmlFolder + configFile + ".xml";
                    XmlTextWriter configData = new XmlTextWriter(fileName, null);

                    configData.Formatting = Formatting.Indented;
                    configData.Indentation = 0;
                    configData.Namespaces = false;
                    configData.WriteStartDocument();

                    configData.WriteStartElement("", "ConfigData", "");

                    //###

                    writeElement("version", bubble.version, configData);
                    writeElement("profileInUse", bubble.profileInUse, configData);
                    writeElement("newsSeq", bubble.newsSeq.ToString(), configData);
                    writeElement("mysqlDriver", bubble.mysqlDriver, configData);

                    config.getFirstProfile();

                    while (config.moreProfiles())
                    {

                        configData.Indentation = 4;

                        writeElement("profileStart", config.getProfile().profileName.ToLower(), configData);
                        writeElement("sentByName", config.getProfile().sentByName, configData);
                        writeElement("pingSubject", config.getProfile().pingSubject, configData);

                        configData.Indentation = 12;

                        //!!!!!!!!!!!!!!!!!!!!!!!!!!
                        //Webcam individual settings
                        //!!!!!!!!!!!!!!!!!!!!!!!!!!
                        foreach (cameraSpecificInfo infoI in CameraRig.camInfo)
                        {

                            if (infoI.profileName.ToLower() == config.getProfile().profileName.ToLower())
                            {

                                writeElement("webcam", infoI.webcam, configData);

                                configData.Indentation = 16;

                                writeElement("friendlyName", infoI.friendlyName, configData);
                                writeElement("alarmActive", infoI.alarmActive.ToString(), configData);
                                writeElement("publishActive", infoI.publishActive.ToString(), configData);
                                writeElement("displayButton", infoI.displayButton.ToString(), configData);
                                writeElement("areaDetection", infoI.areaDetection.ToString(), configData);
                                writeElement("areaDetectionWithin", infoI.areaDetectionWithin.ToString(), configData);
                                writeElement("areaOffAtMotion", infoI.areaOffAtMotion.ToString(), configData);
                                writeElement("rectX", infoI.rectX.ToString(), configData);
                                writeElement("rectY", infoI.rectY.ToString(), configData);
                                writeElement("rectWidth", infoI.rectWidth.ToString(), configData);
                                writeElement("rectHeight", infoI.rectHeight.ToString(), configData);
                                writeElement("movementVal", infoI.movementVal.ToString(), configData);
                                writeElement("timeSpike", infoI.timeSpike.ToString(), configData);
                                writeElement("toleranceSpike", infoI.toleranceSpike.ToString(), configData);
                                writeElement("lightSpike", infoI.lightSpike.ToString(), configData);
                                writeElement("pubImage", infoI.pubImage.ToString(), configData);
                                writeElement("pubTime", infoI.pubTime.ToString(), configData);
                                writeElement("pubHours", infoI.pubHours.ToString(), configData);
                                writeElement("pubMins", infoI.pubMins.ToString(), configData);
                                writeElement("pubSecs", infoI.pubSecs.ToString(), configData);
                                writeElement("publishWeb", infoI.publishWeb.ToString(), configData);
                                writeElement("publishLocal", infoI.publishLocal.ToString(), configData);
                                writeElement("timerOn", infoI.timerOn.ToString(), configData);
                                writeElement("fileURLPubWeb", infoI.fileURLPubWeb.ToString(), configData);
                                writeElement("filenamePrefixPubWeb", infoI.filenamePrefixPubWeb.ToString(), configData);
                                writeElement("cycleStampCheckedPubWeb", infoI.cycleStampCheckedPubWeb.ToString(), configData);
                                writeElement("startCyclePubWeb", infoI.startCyclePubWeb.ToString(), configData);
                                writeElement("endCyclePubWeb", infoI.endCyclePubWeb.ToString(), configData);
                                writeElement("currentCyclePubWeb", infoI.currentCyclePubWeb.ToString(), configData);
                                writeElement("stampAppendPubWeb", infoI.stampAppendPubWeb.ToString(), configData);
                                writeElement("fileDirAlertLoc", infoI.fileDirAlertLoc.ToString(), configData);
                                writeElement("fileDirAlertCust", infoI.fileDirAlertCust.ToString(), configData);
                                writeElement("fileDirPubLoc", infoI.fileDirPubLoc.ToString(), configData);
                                writeElement("fileDirPubCust", infoI.fileDirPubCust.ToString(), configData);
                                writeElement("filenamePrefixPubLoc", infoI.filenamePrefixPubLoc.ToString(), configData);
                                writeElement("cycleStampCheckedPubLoc", infoI.cycleStampCheckedPubLoc.ToString(), configData);
                                writeElement("startCyclePubLoc", infoI.startCyclePubLoc.ToString(), configData);
                                writeElement("endCyclePubLoc", infoI.endCyclePubLoc.ToString(), configData);
                                writeElement("currentCyclePubLoc", infoI.currentCyclePubLoc.ToString(), configData);
                                writeElement("stampAppendPubLoc", infoI.stampAppendPubLoc.ToString(), configData);
                                writeElement("ipWebcamAddress", infoI.ipWebcamAddress.ToString(), configData);
                                writeElement("ipWebcamUser", encrypt(infoI.ipWebcamUser.ToString()), configData);
                                writeElement("ipWebcamPassword", encrypt(infoI.ipWebcamPassword.ToString()), configData);

                                configData.Indentation = 12;

                            }

                        }

                        //!!!!!!!!!!!!!!!!!!!!!!!!!!
                        //Webcam individual settings
                        //!!!!!!!!!!!!!!!!!!!!!!!!!!

                        configData.Indentation = 8;

                        writeElement("freezeGuard", config.getProfile().freezeGuard.ToString(), configData);
                        writeElement("freezeGuardWindowShow", config.getProfile().freezeGuardWindowShow.ToString(), configData);
                        writeElement("updatesNotify", config.getProfile().updatesNotify.ToString(), configData);
                        writeElement("countdownNow", config.getProfile().countdownNow.ToString(), configData);
                        writeElement("activatecountdown", config.getProfile().activatecountdown.ToString(), configData);
                        writeElement("activatecountdownTime", config.getProfile().activatecountdownTime, configData);
                        writeElement("countdownTime", config.getProfile().countdownTime.ToString(), configData);
                        writeElement("alert", bubble.Alert.on.ToString(), configData);
                        writeElement("maxImagesToEmail", config.getProfile().maxImagesToEmail.ToString(), configData);
                        writeElement("captureMovementImages", config.getProfile().captureMovementImages.ToString(), configData);
                        writeElement("sendNotifyEmail", config.getProfile().sendNotifyEmail.ToString(), configData);
                        writeElement("ping", config.getProfile().ping.ToString(), configData);
                        writeElement("pingAll", config.getProfile().pingAll.ToString(), configData);
                        writeElement("pingInterval", config.getProfile().pingInterval.ToString(), configData);
                        writeElement("sendFullSizeImages", config.getProfile().sendFullSizeImages.ToString(), configData);
                        writeElement("sendThumbnailImages", config.getProfile().sendThumbnailImages.ToString(), configData);
                        writeElement("sendMosaicImages", config.getProfile().sendMosaicImages.ToString(), configData);
                        writeElement("mosaicImagesPerRow", config.getProfile().mosaicImagesPerRow.ToString(), configData);
                        writeElement("loadImagesToFtp", config.getProfile().loadImagesToFtp.ToString(), configData);
                        //###
                        writeElement("baselineVal", config.getProfile().baselineVal.ToString(), configData);
                        writeElement("imageSaveInterval", config.getProfile().imageSaveInterval.ToString(), configData);
                        writeElement("filenamePrefix", config.getProfile().filenamePrefix, configData);
                        writeElement("cycleStampChecked", config.getProfile().cycleStampChecked.ToString(), configData);
                        writeElement("startCycle", config.getProfile().startCycle.ToString(), configData);
                        writeElement("endCycle", config.getProfile().endCycle.ToString(), configData);
                        writeElement("currentCycle", config.getProfile().currentCycle.ToString(), configData);
                        writeElement("emailNotifyInterval", config.getProfile().emailNotifyInterval.ToString(), configData);
                        //###
                        writeElement("emailUser", config.getProfile().emailUser, configData);
                        writeElement("emailPassword", encrypt(config.getProfile().emailPass), configData);
                        writeElement("lockdownPassword", encrypt(config.getProfile().lockdownPassword), configData);
                        writeElement("lockdownOn", config.getProfile().lockdownOn.ToString(), configData);
                        writeElement("smtpHost", config.getProfile().smtpHost, configData);
                        writeElement("stmpPort", config.getProfile().smtpPort.ToString(), configData);
                        writeElement("ssl", config.getProfile().EnableSsl.ToString(), configData);
                        writeElement("sendTo", config.getProfile().sendTo, configData);
                        writeElement("replyTo", config.getProfile().replyTo, configData);
                        writeElement("sentBy", config.getProfile().sentBy, configData);
                        writeElement("mailSubject", config.getProfile().mailSubject, configData);
                        writeElement("mailBody", config.getProfile().mailBody, configData);
                        //###
                        writeElement("ftpUser", config.getProfile().ftpUser, configData);
                        writeElement("ftpPass", encrypt(config.getProfile().ftpPass), configData);
                        writeElement("ftpRoot", config.getProfile().ftpRoot, configData);
                        writeElement("pubImage", config.getProfile().pubImage.ToString(), configData);
                        writeElement("pubFtpUser", config.getProfile().pubFtpUser, configData);
                        writeElement("pubFtpPass", encrypt(config.getProfile().pubFtpPass), configData);
                        writeElement("pubFtpRoot", config.getProfile().pubFtpRoot, configData);
                        writeElement("timerOn", config.getProfile().timerOn.ToString(), configData);
                        writeElement("timerStartPub", config.getProfile().timerStartPub, configData);
                        writeElement("timerEndPub", config.getProfile().timerEndPub, configData);
                        writeElement("timerOnMov", config.getProfile().timerOnMov.ToString(), configData);
                        writeElement("scheduleOnAtStart", config.getProfile().scheduleOnAtStart.ToString(), configData);
                        writeElement("activateAtEveryStartup", config.getProfile().activateAtEveryStartup.ToString(), configData);
                        writeElement("timerStartMov", config.getProfile().timerStartMov, configData);
                        writeElement("timerEndMov", config.getProfile().timerEndMov, configData);
                        writeElement("webUpd", config.getProfile().webUpd.ToString(), configData);
                        writeElement("webUser", config.getProfile().webUser, configData);
                        writeElement("webPass", encrypt(config.getProfile().webPass), configData);
                        writeElement("webFtpPass", encrypt(config.getProfile().webFtpPass), configData);
                        writeElement("webFtpUser", config.getProfile().webFtpUser, configData);
                        writeElement("webPoll", config.getProfile().webPoll.ToString(), configData);
                        writeElement("webInstance", config.getProfile().webInstance.ToString(), configData);
                        writeElement("webImageRoot", config.getProfile().webImageRoot.ToString(), configData);
                        writeElement("webImageFileName", config.getProfile().webImageFileName.ToString(), configData);
                        writeElement("soundAlert", config.getProfile().soundAlert, configData);
                        writeElement("soundAlertOn", config.getProfile().soundAlertOn.ToString(), configData);
                        writeElement("logsKeep", config.getProfile().logsKeep.ToString(), configData);
                        writeElement("logsKeepChk", config.getProfile().logsKeepChk.ToString(), configData);
                        writeElement("imageParentFolderCust", config.getProfile().imageParentFolderCust, configData);
                        writeElement("imageFolderCust", config.getProfile().imageFolderCust, configData);
                        writeElement("thumbFolderCust", config.getProfile().thumbFolderCust, configData);
                        writeElement("imageLocCust", config.getProfile().imageLocCust.ToString(), configData);
                        writeElement("startTeboCamMinimized", config.getProfile().startTeboCamMinimized.ToString(), configData);
                        writeElement("internetCheck", config.getProfile().internetCheck, configData);
                        writeElement("toolTips", config.getProfile().toolTips.ToString(), configData);
                        writeElement("alertCompression", config.getProfile().alertCompression.ToString(), configData);
                        writeElement("publishCompression", config.getProfile().publishCompression.ToString(), configData);
                        writeElement("pingCompression", config.getProfile().pingCompression.ToString(), configData);
                        writeElement("onlineCompression", config.getProfile().onlineCompression.ToString(), configData);
                        writeElement("alertTimeStamp", config.getProfile().alertTimeStamp.ToString(), configData);
                        writeElement("alertTimeStampFormat", config.getProfile().alertTimeStampFormat, configData);
                        writeElement("alertStatsStamp", config.getProfile().alertStatsStamp.ToString(), configData);
                        writeElement("alertTimeStampColour", config.getProfile().alertTimeStampColour, configData);
                        writeElement("alertTimeStampPosition", config.getProfile().alertTimeStampPosition, configData);
                        writeElement("alertTimeStampRect", config.getProfile().alertTimeStampRect.ToString(), configData);
                        writeElement("publishTimeStamp", config.getProfile().publishTimeStamp.ToString(), configData);
                        writeElement("publishTimeStampFormat", config.getProfile().publishTimeStampFormat, configData);
                        writeElement("publishStatsStamp", config.getProfile().publishStatsStamp.ToString(), configData);
                        writeElement("publishTimeStampColour", config.getProfile().publishTimeStampColour, configData);
                        writeElement("publishTimeStampPosition", config.getProfile().publishTimeStampPosition, configData);
                        writeElement("publishTimeStampRect", config.getProfile().publishTimeStampRect.ToString(), configData);
                        writeElement("pingTimeStamp", config.getProfile().pingTimeStamp.ToString(), configData);
                        writeElement("pingTimeStampFormat", config.getProfile().pingTimeStampFormat, configData);
                        writeElement("pingStatsStamp", config.getProfile().pingStatsStamp.ToString(), configData);
                        writeElement("pingTimeStampColour", config.getProfile().pingTimeStampColour, configData);
                        writeElement("pingTimeStampPosition", config.getProfile().pingTimeStampPosition, configData);
                        writeElement("pingTimeStampRect", config.getProfile().pingTimeStampRect.ToString(), configData);
                        writeElement("onlineTimeStamp", config.getProfile().onlineTimeStamp.ToString(), configData);
                        writeElement("onlineTimeStampFormat", config.getProfile().onlineTimeStampFormat, configData);
                        writeElement("onlineStatsStamp", config.getProfile().onlineStatsStamp.ToString(), configData);
                        writeElement("onlineTimeStampColour", config.getProfile().onlineTimeStampColour, configData);
                        writeElement("onlineTimeStampPosition", config.getProfile().onlineTimeStampPosition, configData);
                        writeElement("onlineTimeStampRect", config.getProfile().onlineTimeStampRect.ToString(), configData);
                        writeElement("publishLocal", config.getProfile().publishLocal.ToString(), configData);
                        writeElement("publishWeb", config.getProfile().publishWeb.ToString(), configData);
                        writeElement("imageToframe", config.getProfile().imageToframe.ToString(), configData);
                        writeElement("cameraShow", config.getProfile().cameraShow.ToString(), configData);
                        writeElement("motionLevel", config.getProfile().motionLevel.ToString(), configData);
                        writeElement("selectedCam", config.getProfile().selectedCam, configData);
                        writeElement("pulseFreq", config.getProfile().pulseFreq.ToString(), configData);
                        writeElement("EmailIntelOn", config.getProfile().EmailIntelOn.ToString(), configData);
                        writeElement("emailIntelEmails", config.getProfile().emailIntelEmails.ToString(), configData);
                        writeElement("emailIntelMins", config.getProfile().emailIntelMins.ToString(), configData);
                        writeElement("EmailIntelStop", config.getProfile().EmailIntelStop.ToString(), configData);
                        writeElement("disCommOnline", config.getProfile().disCommOnline.ToString(), configData);
                        writeElement("disCommOnlineSecs", config.getProfile().disCommOnlineSecs.ToString(), configData);
                        //###
                        writeElement("StatsToFileOn", config.getProfile().StatsToFileOn.ToString(), configData);
                        writeElement("StatsToFileLocation", config.getProfile().StatsToFileLocation.ToString(), configData);
                        writeElement("StatsToFileTimeStamp", config.getProfile().StatsToFileTimeStamp.ToString(), configData);
                        writeElement("StatsToFileMb", config.getProfile().StatsToFileMb.ToString(), configData);


                        //******************************
                        //Do not put anything after this
                        //******************************
                        configData.Indentation = 4;
                        writeElement("profileEnd", config.getProfile().profileName.ToLower(), configData);
                        //******************************
                        //Do not put anything after this
                        //******************************

                    }//while (config.getNextProfile)
                    //###
                    configData.WriteEndElement();
                    configData.WriteEndDocument();
                    configData.Flush();
                    configData.Close();

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }




            #endregion


        }

        private static void writeElement(string tag, string element, XmlTextWriter writer)
        {

            writer.WriteStartElement("", tag, "");
            writer.WriteString(element);
            writer.WriteEndElement();

        }



        #endregion

        private static string encrypt(string inStr)
        {

            if (inStr.Length < 2) return "";

            try
            {
                crypt crypt = new crypt();

                string tmpStr = crypt.EncryptToString(inStr);
                tmpStr = "a" + tmpStr;
                return tmpStr;
            }
            catch { return ""; }

        }

        private static string decrypt(string inStr)
        {

            if (inStr.Length < 2) return "";

            try
            {
                crypt crypt = new crypt();

                if (LeftRightMid.Left(inStr, 1) == "a")
                {
                    inStr = inStr.Remove(0, 1);
                    return crypt.DecryptString(inStr);
                }
                else
                {
                    return ConvertFromAscii(inStr);
                }
            }
            catch { return ""; }


        }

        private static string ConvertToAscii(string inStr)
        {
            string tmpStr = "";

            foreach (char a in inStr)
            {
                tmpStr += Convert.ToInt32(a) + ".";
            }

            return tmpStr;

        }

        private static string ConvertFromAscii(string inStr)
        {
            string tmpStra = "";
            string tmpStrb = "";

            foreach (char a in inStr)
            {
                if (a.ToString() != ".")
                {
                    tmpStrb += a.ToString();
                }
                else
                {
                    tmpStra += Convert.ToChar(int.Parse(tmpStrb)).ToString();
                    tmpStrb = "";
                }

            }

            return tmpStra;

        }



    }
}
