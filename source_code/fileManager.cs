
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
        public static string configFile = "configData";
        //public static string graphFile = "graph";
        //public static string logFile = "log";
        public static string keyFile = "licence";
        public static string testFile = "test";
        
        #region ::::::::::::::::::::::::backupFile::::::::::::::::::::::::

        #endregion


        #region ::::::::::::::::::::::::readXmlFile::::::::::::::::::::::::


        #endregion




        #region ::::::::::::::::::::::::clearLog::::::::::::::::::::::::
        public static void clearLog()
        {
            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);
            bubble.logAddLine("Making copy of log file with time stamp - " + timeStamp);
            WriteFile("log");
            File.Copy(bubble.xmlFolder + "LogData.xml", bubble.logFolder + @"\log_" + timeStamp + ".xml");
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
            #endregion


        }



        #endregion



        public static bool ConvertOldProfileIfExists(Configuration config)
        {

            string fileName = bubble.xmlFolder + "config.xml";

            if (!File.Exists(fileName)) { return true; }

            Interfaces.IEncryption crypt = new crypt();
            XmlTextReader configFile = new XmlTextReader(fileName);
            configApplication appInfo = new configApplication();
            configWebcam webcamInfo = new configWebcam();
            try
            {
                while (configFile.Read())
                {
                    //first line for this appInfo
                    if (configFile.LocalName.Equals("profileStart"))
                    {
                        appInfo = new configApplication();
                        config.appConfigs.Add(appInfo);
                        appInfo.profileName = configFile.ReadString();
                    }

                    //first line for this webcamInfo
                    if (configFile.LocalName.Equals("webcam"))
                    {
                        appInfo.webcam = configFile.ReadString();
                        webcamInfo = new configWebcam();
                        appInfo.camConfigs.Add(webcamInfo);
                        webcamInfo.webcam = appInfo.webcam;
                        webcamInfo.profileName = appInfo.profileName;
                    }


                    if (configFile.LocalName.Equals(CameraRig.infoEnum.alarmActive.ToString())) { webcamInfo.alarmActive = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.publishActive.ToString())) { webcamInfo.publishActive = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.friendlyName.ToString())) { webcamInfo.friendlyName = configFile.ReadString(); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.displayButton.ToString())) { webcamInfo.displayButton = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.pubTime.ToString())) { webcamInfo.pubTime = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.pubHours.ToString())) { webcamInfo.pubHours = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.pubMins.ToString())) { webcamInfo.pubMins = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.pubSecs.ToString())) { webcamInfo.pubSecs = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.publishWeb.ToString())) { webcamInfo.publishWeb = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.publishLocal.ToString())) { webcamInfo.publishLocal = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.timerOn.ToString())) { webcamInfo.timerOn = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.fileURLPubWeb.ToString())) { webcamInfo.fileURLPubWeb = configFile.ReadString(); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.filenamePrefixPubWeb.ToString())) { webcamInfo.filenamePrefixPubWeb = configFile.ReadString(); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.cycleStampCheckedPubWeb.ToString())) { webcamInfo.cycleStampCheckedPubWeb = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.startCyclePubWeb.ToString())) { webcamInfo.startCyclePubWeb = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.endCyclePubWeb.ToString())) { webcamInfo.endCyclePubWeb = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.currentCyclePubWeb.ToString())) { webcamInfo.currentCyclePubWeb = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.stampAppendPubWeb.ToString())) { webcamInfo.stampAppendPubWeb = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.fileAlertPubLoc.ToString())) { webcamInfo.fileDirAlertLoc = configFile.ReadString(); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.fileAlertPubCust.ToString())) { webcamInfo.fileDirAlertCust = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.fileDirPubLoc.ToString())) { webcamInfo.fileDirPubLoc = configFile.ReadString(); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.fileDirPubCust.ToString())) { webcamInfo.fileDirPubCust = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.filenamePrefixPubLoc.ToString())) { webcamInfo.filenamePrefixPubLoc = configFile.ReadString(); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.cycleStampCheckedPubLoc.ToString())) { webcamInfo.cycleStampCheckedPubLoc = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.startCyclePubLoc.ToString())) { webcamInfo.startCyclePubLoc = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.endCyclePubLoc.ToString())) { webcamInfo.endCyclePubLoc = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.currentCyclePubLoc.ToString())) { webcamInfo.currentCyclePubLoc = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.stampAppendPubLoc.ToString())) { webcamInfo.stampAppendPubLoc = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.ipWebcamAddress.ToString())) { webcamInfo.ipWebcamAddress = configFile.ReadString(); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.ipWebcamUser.ToString())) { webcamInfo.ipWebcamUser = configFile.ReadString(); }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.ipWebcamPassword.ToString())) { webcamInfo.ipWebcamPassword = configFile.ReadString(); }

                    if (configFile.LocalName.Equals(CameraRig.infoEnum.areaDetection.ToString()))
                    {
                        appInfo.areaDetection = Convert.ToBoolean(configFile.ReadString());
                        webcamInfo.areaDetection = appInfo.areaDetection;
                    }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.areaDetectionWithin.ToString()))
                    {
                        appInfo.areaDetectionWithin = Convert.ToBoolean(configFile.ReadString());
                        webcamInfo.areaDetectionWithin = appInfo.areaDetectionWithin;
                    }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.areaOffAtMotion.ToString()))
                    {
                        appInfo.areaOffAtMotion = Convert.ToBoolean(configFile.ReadString());
                        webcamInfo.areaOffAtMotion = appInfo.areaOffAtMotion;
                    }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.rectX.ToString()))
                    {
                        appInfo.rectX = Convert.ToInt32(configFile.ReadString());
                        webcamInfo.rectX = appInfo.rectX;
                    }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.rectY.ToString()))
                    {
                        appInfo.rectY = Convert.ToInt32(configFile.ReadString());
                        webcamInfo.rectY = appInfo.rectY;
                    }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.rectWidth.ToString()))
                    {
                        appInfo.rectWidth = Convert.ToInt32(configFile.ReadString());
                        webcamInfo.rectWidth = appInfo.rectWidth;
                    }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.rectHeight.ToString()))
                    {
                        appInfo.rectHeight = Convert.ToInt32(configFile.ReadString());
                        webcamInfo.rectHeight = appInfo.rectHeight;
                    }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.movementVal.ToString()))
                    {
                        appInfo.movementVal = Convert.ToDouble(configFile.ReadString());
                        webcamInfo.movementVal = appInfo.movementVal;
                    }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.timeSpike.ToString()))
                    {
                        appInfo.timeSpike = Convert.ToInt32(configFile.ReadString());
                        webcamInfo.timeSpike = appInfo.timeSpike;
                    }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.toleranceSpike.ToString()))
                    {
                        appInfo.toleranceSpike = Convert.ToInt32(configFile.ReadString());
                        webcamInfo.toleranceSpike = appInfo.toleranceSpike;
                    }
                    if (configFile.LocalName.Equals(CameraRig.infoEnum.lightSpike.ToString()))
                    {
                        appInfo.lightSpike = Convert.ToBoolean(configFile.ReadString());
                        webcamInfo.lightSpike = appInfo.lightSpike;
                    }

                    if (configFile.LocalName.Equals("mysqlDriver")) { config.mysqlDriver = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("newsSeq")) { config.newsSeq = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("version")) { config.version = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("freezeGuard")) { appInfo.freezeGuard = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("freezeGuardWindowShow")) { appInfo.freezeGuardWindowShow = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("updatesNotify")) { appInfo.updatesNotify = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("countdownNow")) { appInfo.countdownNow = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("cycleStamp")) { appInfo.cycleStamp = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("activatecountdown")) { appInfo.activatecountdown = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("activatecountdownTime")) { appInfo.activatecountdownTime = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("countdownTime")) { appInfo.countdownTime = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("alert")) { appInfo.AlertOnStartup = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("maxImagesToEmail")) { appInfo.maxImagesToEmail = Convert.ToInt64(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("ping")) { appInfo.ping = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("pingAll")) { appInfo.pingAll = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("pingInterval")) { appInfo.pingInterval = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("captureMovementImages")) { appInfo.captureMovementImages = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("sendNotifyEmail")) { appInfo.sendNotifyEmail = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("sendFullSizeImages")) { appInfo.sendFullSizeImages = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("sendThumbnailImages")) { appInfo.sendThumbnailImages = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("sendMosaicImages")) { appInfo.sendMosaicImages = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("mosaicImagesPerRow")) { appInfo.mosaicImagesPerRow = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("loadImagesToFtp")) { appInfo.loadImagesToFtp = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("baselineVal")) { appInfo.baselineVal = Convert.ToDouble(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("imageSaveInterval")) { appInfo.imageSaveInterval = Convert.ToDouble(configFile.ReadString()); }


                    if (configFile.LocalName.Equals("filenamePrefix"))
                    {
                        appInfo.filenamePrefix = configFile.ReadString();
                    }
                    if (configFile.LocalName.Equals("cycleStampChecked"))
                    {
                        appInfo.cycleStampChecked = Convert.ToInt32(configFile.ReadString());
                    }
                    if (configFile.LocalName.Equals("startCycle"))
                    {
                        appInfo.startCycle = Convert.ToInt64(configFile.ReadString());
                    }
                    if (configFile.LocalName.Equals("endCycle"))
                    {
                        appInfo.endCycle = Convert.ToInt64(configFile.ReadString());
                    }
                    if (configFile.LocalName.Equals("currentCycle"))
                    {
                        appInfo.currentCycle = Convert.ToInt64(configFile.ReadString());
                    }

                    if (configFile.LocalName.Equals("sentByName")) { appInfo.sentByName = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("emailNotifyInterval")) { appInfo.emailNotifyInterval = Convert.ToInt64(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("emailUser")) { appInfo.emailUser = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("emailPassword")) { appInfo.emailPass = decrypt(crypt, configFile.ReadString()); }
                    if (configFile.LocalName.Equals("smtpHost")) { appInfo.smtpHost = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("stmpPort")) { appInfo.smtpPort = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("ssl")) { appInfo.EnableSsl = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("sendTo")) { appInfo.sendTo = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("replyTo")) { appInfo.replyTo = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("sentBy")) { appInfo.sentBy = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("pingSubject")) { appInfo.pingSubject = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("mailSubject")) { appInfo.mailSubject = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("mailBody")) { appInfo.mailBody = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("ftpUser")) { appInfo.ftpUser = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("ftpPass")) { appInfo.ftpPass = decrypt(crypt, configFile.ReadString()); }
                    if (configFile.LocalName.Equals("ftpRoot")) { appInfo.ftpRoot = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("pubImage")) { appInfo.pubImage = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("pubFtpUser")) { appInfo.pubFtpUser = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("pubFtpPass")) { appInfo.pubFtpPass = decrypt(crypt, configFile.ReadString()); }
                    if (configFile.LocalName.Equals("pubFtpRoot")) { appInfo.pubFtpRoot = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("pubStamp")) { appInfo.pubStamp = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("pubStampDate")) { appInfo.pubStampDate = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("pubStampTime")) { appInfo.pubStampTime = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("pubStampDateTime")) { appInfo.pubStampDateTime = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("timerStartPub")) { appInfo.timerStartPub = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("timerEndPub")) { appInfo.timerEndPub = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("timerOnMov")) { appInfo.timerOnMov = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("scheduleOnAtStart")) { appInfo.scheduleOnAtStart = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("activateAtEveryStartup")) { appInfo.activateAtEveryStartup = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("timerStartMov")) { appInfo.timerStartMov = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("timerEndMov")) { appInfo.timerEndMov = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("webUpd")) { appInfo.webUpd = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("webUser")) { appInfo.webUser = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("webPass")) { appInfo.webPass = decrypt(crypt, configFile.ReadString()); }
                    if (configFile.LocalName.Equals("webFtpPass")) { appInfo.webFtpPass = decrypt(crypt, configFile.ReadString()); }
                    if (configFile.LocalName.Equals("webFtpUser")) { appInfo.webFtpUser = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("webPoll")) { appInfo.webPoll = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("webInstance")) { appInfo.webInstance = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("webImageRoot")) { appInfo.webImageRoot = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("webImageFileName")) { appInfo.webImageFileName = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("soundAlert")) { appInfo.soundAlert = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("soundAlertOn")) { appInfo.soundAlertOn = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("logsKeep")) { appInfo.logsKeep = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("logsKeepChk")) { appInfo.logsKeepChk = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("imageParentFolderCust")) { appInfo.imageParentFolderCust = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("imageFolderCust")) { appInfo.imageFolderCust = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("thumbFolderCust")) { appInfo.thumbFolderCust = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("imageLocCust")) { appInfo.imageLocCust = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("startTeboCamMinimized")) { appInfo.startTeboCamMinimized = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("internetCheck")) { appInfo.internetCheck = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("toolTips")) { appInfo.toolTips = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("alertCompression")) { appInfo.alertCompression = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("publishCompression")) { appInfo.publishCompression = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("pingCompression")) { appInfo.pingCompression = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("onlineCompression")) { appInfo.onlineCompression = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("alertTimeStamp")) { appInfo.alertTimeStamp = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("alertTimeStampFormat")) { appInfo.alertTimeStampFormat = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("alertStatsStamp")) { appInfo.alertStatsStamp = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("alertTimeStampColour")) { appInfo.alertTimeStampColour = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("alertTimeStampPosition")) { appInfo.alertTimeStampPosition = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("alertTimeStampRect")) { appInfo.alertTimeStampRect = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("publishTimeStamp")) { appInfo.publishTimeStamp = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("publishTimeStampFormat")) { appInfo.publishTimeStampFormat = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("publishStatsStamp")) { appInfo.publishStatsStamp = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("publishTimeStampColour")) { appInfo.publishTimeStampColour = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("publishTimeStampPosition")) { appInfo.publishTimeStampPosition = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("publishTimeStampRect")) { appInfo.publishTimeStampRect = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("pingTimeStamp")) { appInfo.pingTimeStamp = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("pingTimeStampFormat")) { appInfo.pingTimeStampFormat = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("pingStatsStamp")) { appInfo.pingStatsStamp = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("pingTimeStampColour")) { appInfo.pingTimeStampColour = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("pingTimeStampPosition")) { appInfo.pingTimeStampPosition = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("pingTimeStampRect")) { appInfo.pingTimeStampRect = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("onlineTimeStamp")) { appInfo.onlineTimeStamp = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("onlineTimeStampFormat")) { appInfo.onlineTimeStampFormat = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("onlineStatsStamp")) { appInfo.onlineStatsStamp = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("onlineTimeStampColour")) { appInfo.onlineTimeStampColour = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("onlineTimeStampPosition")) { appInfo.onlineTimeStampPosition = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("onlineTimeStampRect")) { appInfo.onlineTimeStampRect = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("imageToframe")) { appInfo.imageToframe = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("cameraShow")) { appInfo.cameraShow = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("motionLevel")) { appInfo.motionLevel = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("selectedCam")) { appInfo.selectedCam = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("pulseFreq")) { appInfo.pulseFreq = Convert.ToDecimal(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("EmailIntelOn")) { appInfo.EmailIntelOn = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("emailIntelEmails")) { appInfo.emailIntelEmails = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("emailIntelMins")) { appInfo.emailIntelMins = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("EmailIntelStop")) { appInfo.EmailIntelStop = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("disCommOnline")) { appInfo.disCommOnline = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("disCommOnlineSecs")) { appInfo.disCommOnlineSecs = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("StatsToFileOn")) { appInfo.StatsToFileOn = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("StatsToFileLocation")) { appInfo.StatsToFileLocation = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("StatsToFileTimeStamp")) { appInfo.StatsToFileTimeStamp = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("StatsToFileMb")) { appInfo.StatsToFileMb = Convert.ToDouble(configFile.ReadString()); }
                }
            }
            catch (Exception e)
            {
                configFile.Close();
                MessageBox.Show(e.ToString());
                return false;
            }
            configFile.Close();
            config.WriteXMLFile(bubble.xmlFolder + FileManager.configFile + ".xml", config);
            File.Move(fileName, bubble.xmlFolder + "configPre_3.2411.xml");
            File.Delete(bubble.xmlFolder + "config.bak");
            return true;
        }

        private static string decrypt(Interfaces.IEncryption crypt, string inStr)
        {

            if (inStr.Length < 2) return "";
            try
            {
                if (LeftRightMid.Left(inStr, 1) == "a")
                {
                    inStr = inStr.Remove(0, 1);
                    //return inStr;
                    return crypt.DecryptString(inStr);
                }
                else
                {
                    //return inStr;
                    return ConvertFromAscii(inStr);
                }
            }
            catch { return ""; }
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
