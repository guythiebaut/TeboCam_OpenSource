
using Ionic.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
//using Microsoft.VisualBasic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.SessionState;
using System.Windows.Forms;
using System.Xml;

namespace TeboCam
{

    public class FileManager
    {
        static BackgroundWorker bw = new BackgroundWorker();

        public static string configFile = "configData";
        //private static List<string> DeletionRegex = new List<string>();

        //public static string graphFile = "graph";
        //public static string logFile = "log";
        public static string keyFile = "licence";

        public static string testFile = "test";
        public static IException tebowebException;

        public const string LongDateTimeFormat = "yyyyMMddHHmmssfff";

        #region ::::::::::::::::::::::::backupFile::::::::::::::::::::::::

        #endregion


        #region ::::::::::::::::::::::::readXmlFile::::::::::::::::::::::::


        #endregion




        #region ::::::::::::::::::::::::clearLog::::::::::::::::::::::::

        public static void clearLog()
        {
            string timeStamp = DateTime.Now.ToString(LongDateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
            TebocamState.log.AddLine("Making copy of log file with time stamp - " + timeStamp);
            WriteFile("log");
            File.Copy(TebocamState.xmlFolder + "LogData.xml", TebocamState.logFolder + @"\log_" + timeStamp + ".xml");
            TebocamState.log.Clear();
            TebocamState.log.AddLine("Previous log file archived.");
            WriteFile("log");
        }

        #endregion

        #region ::::::::::::::::::::::::clearFiles::::::::::::::::::::::::


        public static void clearFiles(string folder)
        {
            TebocamState.log.AddLine("Getting list of computer files from " + folder);
            string[] files = Directory.GetFiles(folder);
            TebocamState.log.AddLine("Starting delete of computer files from " + folder);

            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception e)
                {
                    TebocamState.tebowebException.LogException(e);
                    TebocamState.log.AddLine("Error in deleting files from " + folder);
                }
            }
            TebocamState.log.AddLine("Deletion of computer files completed from " + folder);
        }

        #endregion

        #region ::::::::::::::::::::::::clearFtp::::::::::::::::::::::::
        private static void clearFtpWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var regexList = ImageFileNameRegexList();
                TebocamState.log.AddLine("Getting list of web files.");
                var ftpFiles = ftp.GetFileList();
                TebocamState.log.AddLine("Starting delete of web files via ftp.");
                int tmpInt = 0;

                foreach (string img in ftpFiles)
                {
                    if (RegexMatch(img, regexList)) tmpInt++;
                }
                TebocamState.log.AddLine(tmpInt.ToString() + " web files to delete via ftp.");

                //List of all files on ftp site
                foreach (string img in ftpFiles)
                {
                    //if the prefix and suffix correspond to TeboCam image files then delete this file
                    if (RegexMatch(img, regexList))
                    {
                        ftp.DeleteFTP(img, ConfigurationHelper.GetCurrentProfile().ftpRoot, ConfigurationHelper.GetCurrentProfile().ftpUser, ConfigurationHelper.GetCurrentProfile().ftpPass, false);
                        tmpInt--;
                        TebocamState.log.AddLine(tmpInt.ToString() + " web files left to delete via ftp.");
                    }
                }
                TebocamState.log.AddLine("Deletion of web files via ftp completed.");
            }
            catch (Exception ex)
            {
                TebocamState.tebowebException.LogException(ex);
                TebocamState.log.AddLine("Error in deleting web files.");
            }
        }


        public static string fileNameSet(string filenamePrefix, int cycleType, long startCycle, long endCycle, ref long currCycle, bool appendStamp, bool includeMotionLevel, double motionLevel)
        {
            var fileName = string.Empty;
            var level = (int)(motionLevel * 100);
            var motionLev = includeMotionLevel ? $"-lev-{level}" : string.Empty;

            if (!appendStamp)
            {
                fileName = filenamePrefix.Trim() + TebocamState.ImgSuffix;
            }
            else
            {
                switch (cycleType)
                {
                    case 1:
                        fileName = filenamePrefix.Trim() + currCycle.ToString() + motionLev + TebocamState.ImgSuffix;
                        currCycle = currCycle >= endCycle ? startCycle : currCycle++;
                        break;
                    default:
                        var stamp = DateTime.Now.ToString(LongDateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
                        fileName = filenamePrefix.Trim() + stamp + motionLev + TebocamState.ImgSuffix;
                        break;
                }
            }
            return fileName;
        }



        public static bool RegexMatch(string filename, List<string> regexList)
        {
            return regexList.Any(x => regex.match(x, filename));
        }

        public static List<string> ImageFileNameRegexList()
        {
            return new List<string>
            {
                //starts with config.GetCurrentProfile().filenamePrefix
                //followed by any number of characters
                //ends with {TebocamState.ImgSuffix} matching the first character literally (in this case a dot ".")
                $@"^{ConfigurationHelper.GetCurrentProfile().filenamePrefix}.*\{TebocamState.ImgSuffix}$",
                $@"^{TebocamState.apiCameraImgPrefix}.*\{TebocamState.ImgSuffix}$",
                $@"^{TebocamState.apiGraphImgPrefix}.*\{TebocamState.ImgSuffix}$"
            };
        }

        //public static void clearFtp(TeboCamDelegates.RunWorkerCompletedDelegate thenRun = null)
        public static void clearFtp(TeboCamDelegates.EventDelegate<RunWorkerCompletedEventArgs> thenRun = null)
        {
            bw.WorkerSupportsCancellation = true;
            var regexStrings = ImageFileNameRegexList();
            var args = new DoWorkEventArgs(regexStrings);
            bw.DoWork -= new DoWorkEventHandler(clearFtpWork);
            bw.DoWork += new DoWorkEventHandler(clearFtpWork);

            if (thenRun != null)
            {
                bw.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(thenRun);
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(thenRun);
            }

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
                    string fileName = TebocamState.xmlFolder + "training.xml";
                    XmlTextWriter trainingData = new XmlTextWriter(fileName, null);

                    trainingData.Formatting = Formatting.Indented;
                    trainingData.Indentation = 4;
                    trainingData.Namespaces = false;
                    trainingData.WriteStartDocument();

                    trainingData.WriteStartElement("", "training", "");

                    //###
                    foreach (double level in TebocamState.training)
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
                catch (Exception e)
                {
                    TebocamState.tebowebException.LogException(e);
                }
            }
            #endregion
            #region ::::::::::::::::::::::::Write TestFile::::::::::::::::::::::::

            if (file == "test")
            {
                try
                {

                    string fileName = TebocamState.xmlFolder + testFile + ".xml";
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
                catch (Exception e)
                {
                    TebocamState.tebowebException.LogException(e);
                }
            }
            #endregion


            #region ::::::::::::::::::::::::Write Config::::::::::::::::::::::::
            #endregion


        }



        #endregion



        public static void ConvertOldProfileIfExists(Configuration config)
        {

            string fileName = TebocamState.xmlFolder + "config.xml";

            if (!File.Exists(fileName)) { return; }

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


                    if (configFile.LocalName.Equals("alarmActive")) { webcamInfo.alarmActive = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("publishActive")) { webcamInfo.publishActive = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("friendlyName")) { webcamInfo.friendlyName = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("displayButton")) { webcamInfo.displayButton = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("pubTime")) { webcamInfo.pubTime = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("pubHours")) { webcamInfo.pubHours = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("pubMins")) { webcamInfo.pubMins = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("pubSecs")) { webcamInfo.pubSecs = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("publishWeb")) { webcamInfo.publishWeb = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("publishLocal")) { webcamInfo.publishLocal = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("timerOn")) { webcamInfo.timerOn = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("fileURLPubWeb")) { webcamInfo.fileURLPubWeb = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("filenamePrefixPubWeb")) { webcamInfo.filenamePrefixPubWeb = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("cycleStampCheckedPubWeb")) { webcamInfo.cycleStampCheckedPubWeb = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("startCyclePubWeb")) { webcamInfo.startCyclePubWeb = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("endCyclePubWeb")) { webcamInfo.endCyclePubWeb = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("currentCyclePubWeb")) { webcamInfo.currentCyclePubWeb = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("stampAppendPubWeb")) { webcamInfo.stampAppendPubWeb = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("fileAlertPubLoc")) { webcamInfo.fileDirAlertLoc = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("fileAlertPubCust")) { webcamInfo.fileDirAlertCust = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("fileDirPubLoc")) { webcamInfo.fileDirPubLoc = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("fileDirPubCust")) { webcamInfo.fileDirPubCust = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("filenamePrefixPubLoc")) { webcamInfo.filenamePrefixPubLoc = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("cycleStampCheckedPubLoc")) { webcamInfo.cycleStampCheckedPubLoc = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("startCyclePubLoc")) { webcamInfo.startCyclePubLoc = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("endCyclePubLoc")) { webcamInfo.endCyclePubLoc = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("currentCyclePubLoc")) { webcamInfo.currentCyclePubLoc = Convert.ToInt32(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("stampAppendPubLoc")) { webcamInfo.stampAppendPubLoc = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("ipWebcamAddress")) { webcamInfo.ipWebcamAddress = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("ipWebcamUser")) { webcamInfo.ipWebcamUser = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("ipWebcamPassword")) { webcamInfo.ipWebcamPassword = configFile.ReadString(); }

                    if (configFile.LocalName.Equals("areaDetection"))
                    {
                        appInfo.areaDetection = Convert.ToBoolean(configFile.ReadString());
                        webcamInfo.areaDetection = appInfo.areaDetection;
                    }
                    if (configFile.LocalName.Equals("areaDetectionWithin"))
                    {
                        appInfo.areaDetectionWithin = Convert.ToBoolean(configFile.ReadString());
                        webcamInfo.areaDetectionWithin = appInfo.areaDetectionWithin;
                    }
                    if (configFile.LocalName.Equals("areaOffAtMotion"))
                    {
                        appInfo.areaOffAtMotion = Convert.ToBoolean(configFile.ReadString());
                        webcamInfo.areaOffAtMotion = appInfo.areaOffAtMotion;
                    }
                    if (configFile.LocalName.Equals("rectX"))
                    {
                        appInfo.rectX = Convert.ToInt32(configFile.ReadString());
                        webcamInfo.rectX = appInfo.rectX;
                    }
                    if (configFile.LocalName.Equals("rectY"))
                    {
                        appInfo.rectY = Convert.ToInt32(configFile.ReadString());
                        webcamInfo.rectY = appInfo.rectY;
                    }
                    if (configFile.LocalName.Equals("rectWidth"))
                    {
                        appInfo.rectWidth = Convert.ToInt32(configFile.ReadString());
                        webcamInfo.rectWidth = appInfo.rectWidth;
                    }
                    if (configFile.LocalName.Equals("rectHeight"))
                    {
                        appInfo.rectHeight = Convert.ToInt32(configFile.ReadString());
                        webcamInfo.rectHeight = appInfo.rectHeight;
                    }
                    if (configFile.LocalName.Equals("movementVal"))
                    {
                        webcamInfo.movementVal = Convert.ToDouble(configFile.ReadString());
                    }
                    if (configFile.LocalName.Equals("timeSpike"))
                    {
                        webcamInfo.timeSpike = Convert.ToInt32(configFile.ReadString());
                    }
                    if (configFile.LocalName.Equals("toleranceSpike"))
                    {
                        webcamInfo.toleranceSpike = Convert.ToInt32(configFile.ReadString());
                    }
                    if (configFile.LocalName.Equals("lightSpike"))
                    {
                        webcamInfo.lightSpike = Convert.ToBoolean(configFile.ReadString());
                    }

                    //if (configFile.LocalName.Equals("mysqlDriver")) { config.authenticationEndpoint = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("newsSeq")) { config.newsSeq = Convert.ToDouble(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("version")) { config.version = configFile.ReadString(); }
                    if (configFile.LocalName.Equals("freezeGuard")) { appInfo.freezeGuard = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("freezeGuardWindowShow")) { appInfo.freezeGuardWindowShow = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("updatesNotify")) { appInfo.updatesNotify = Convert.ToBoolean(configFile.ReadString()); }
                    if (configFile.LocalName.Equals("updateDebugLocation")) { appInfo.updateDebugLocation = configFile.ReadString(); }
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
                TebocamState.tebowebException.LogException(e);
                configFile.Close();
                MessageBox.Show(e.ToString());
                return;
            }
            configFile.Close();
            config.WriteXmlFile(TebocamState.xmlFolder + FileManager.configFile + ".xml", config);

            string moveToFileName = string.Format("configPre_3.2411_{0}.xml", DateTime.Now.ToString(LongDateTimeFormat));
            File.Move(fileName, TebocamState.xmlFolder + moveToFileName);
            File.Delete(TebocamState.xmlFolder + "config.bak");
            return;
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
            catch (Exception e)
            {
                TebocamState.tebowebException.LogException(e);
                return string.Empty;
            }
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

        public static void CreateDirIfNotExists(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public static bool unZip(string file, string unZipTo)//, bool deleteZipOnCompletion)
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

        public static event EventHandler cycleChanged;

        public static string pictureFile()
        {
            string fileName;

            switch (ConfigurationHelper.GetCurrentProfile().cycleStampChecked)
            {
                case 1:
                    fileName = ConfigurationHelper.GetCurrentProfile().filenamePrefix.Trim() + ConfigurationHelper.GetCurrentProfile().currentCycle.ToString() + TebocamState.ImgSuffix;
                    if (ConfigurationHelper.GetCurrentProfile().currentCycle >= ConfigurationHelper.GetCurrentProfile().endCycle)
                    {
                        ConfigurationHelper.GetCurrentProfile().currentCycle = ConfigurationHelper.GetCurrentProfile().startCycle;
                    }
                    else
                    {
                        ConfigurationHelper.GetCurrentProfile().currentCycle++;
                    }
                    cycleChanged(null, new EventArgs());
                    break;
                case 2:
                    string stampA = DateTime.Now.ToString(LongDateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
                    fileName = ConfigurationHelper.GetCurrentProfile().filenamePrefix.Trim() + stampA + TebocamState.ImgSuffix;
                    break;
                default:
                    string stampB = DateTime.Now.ToString(LongDateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
                    fileName = ConfigurationHelper.GetCurrentProfile().filenamePrefix.Trim() + stampB + TebocamState.ImgSuffix;
                    break;
            }

            return fileName;
        }
    }
}
