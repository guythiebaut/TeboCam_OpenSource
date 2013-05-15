
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
    class pre262
    {
        public static string friendlyName = "Camera";
        public static bool alarmActive = false;
        public static bool publishActive = false;
        public static bool areaDetection = false;
        public static bool areaDetectionWithin = false;
        public static bool areaOffAtMotion = false;
        public static int rectX = 0;
        public static int rectY = 0;
        public static int rectWidth = 0;
        public static int rectHeight = 0;
        public static int displayButton = 1;
        public static double movementVal = 0;
        public static string fileDirAlertLoc = "";
        public static bool fileDirAlertCust = false;

        public static bool pubImage = false;
        public static int pubTime = 2;
        public static bool pubHours = false;
        public static bool pubMins = true;
        public static bool pubSecs = false;
        public static bool publishWeb = false;
        public static bool publishLocal = false;
        public static bool timerOn = false;

        public static string fileURLPubWeb = "";
        public static string filenamePrefixPubWeb = "webcamPublish";
        public static int cycleStampCheckedPubWeb = 1;
        public static int startCyclePubWeb = 1;
        public static int endCyclePubWeb = 999;
        public static int currentCyclePubWeb = 1;
        public static bool stampAppendPubWeb = false;


        public static string fileDirPubLoc = "";
        public static bool fileDirPubCust = false;
        public static string filenamePrefixPubLoc = "webcamPublish";
        public static int cycleStampCheckedPubLoc = 1;
        public static int startCyclePubLoc = 1;
        public static int endCyclePubLoc = 999;
        public static int currentCyclePubLoc = 1;
        public static bool stampAppendPubLoc = false;


        public static string ipWebcamAddress = "";
        public static string ipWebcamUser = "";
        public static string ipWebcamPassword = "";



    }

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

            if (file == "graph")
            {
                result = ReadFile(file, bubble.xmlFolder + graphFile + suffix);
            }

            if (file == "log")
            {
                result = ReadFile(file, bubble.xmlFolder + logFile + suffix);
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
                            ftp.DeleteFTP(img, config.getProfile(bubble.profileInUse).ftpRoot, config.getProfile(bubble.profileInUse).ftpUser, config.getProfile(bubble.profileInUse).ftpPass);
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
            #region ::::::::::::::::::::::::Read Graph::::::::::::::::::::::::
            if (file == "graph")
            {
                int histVal = 0;
                bool firstDate = true;
                ArrayList histVals = new ArrayList();
                string date = "";
                string fileName = path;//bubble.xmlFolder + graphFile + ".xml";

                XmlTextReader graphData = new XmlTextReader(fileName);

                try
                {

                    while (graphData.Read())
                    {
                        if (graphData.NodeType == XmlNodeType.Element)
                        {

                            if (graphData.LocalName.Equals("date"))
                            {
                                if (!firstDate)
                                {
                                    Graph.updateGraphHist(date, histVals);
                                }
                                date = graphData.ReadString();
                                histVals = new ArrayList();
                                firstDate = false;
                                for (int i = 0; i < 12; i++)
                                {
                                    histVals.Add(0);
                                }

                            }

                            if (graphData.LocalName.Equals("val"))
                            {
                                int tmpInt = 0;
                                string tmpStr = graphData.ReadString();
                                int colonPos = tmpStr.IndexOf(":", 0);
                                int pos = Convert.ToInt32(LeftRightMid.Left(tmpStr, colonPos));
                                int val = Convert.ToInt32(LeftRightMid.Right(tmpStr, (tmpStr.Length - (colonPos + 1))));

                                histVals[pos] = val;

                                //histVals.Add(Convert.ToInt32(graphData.ReadString()));
                                histVal++;
                            }


                        }
                    }
                    Graph.updateGraphHist(date, histVals);
                    histVal = 0;
                    graphData.Close();
                    return 1;
                }
                catch (Exception e)
                {
                    graphData.Close();
                    WriteFile("graphInit");
                    MessageBox.Show(e.ToString());
                    return 0;
                }
            }
            #endregion

            #region ::::::::::::::::::::::::Read Log::::::::::::::::::::::::

            if (file == "log")
            {
                string fileName = path;// bubble.xmlFolder + logFile + ".xml";

                XmlTextReader logData = new XmlTextReader(fileName);

                try
                {

                    while (logData.Read())
                    {
                        if (logData.NodeType == XmlNodeType.Element)
                        {
                            //###
                            if (logData.LocalName.Equals("log"))
                            {
                                bubble.log.Add(logData.ReadString());
                            }
                        }
                    }
                    logData.Close();
                }
                catch (Exception e)
                {
                    logData.Close();
                    WriteFile("logInit");
                    MessageBox.Show(e.ToString());
                    return 0;
                }
            }
            #endregion
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

                                if (decimal.Parse(profileVer) <= 2.62m)//m forces number to be interpreted as decimal
                                {
                                    CameraRig.addInfo("alarmActive", true);
                                    CameraRig.addInfo("publishActive", true);
                                    CameraRig.addInfo("areaDetection", pre262.areaDetection);
                                    CameraRig.addInfo("areaDetectionWithin", pre262.areaDetectionWithin);
                                    CameraRig.addInfo("areaOffAtMotion", pre262.areaOffAtMotion);
                                    CameraRig.addInfo("rectX", pre262.rectX);
                                    CameraRig.addInfo("rectY", pre262.rectY);
                                    CameraRig.addInfo("rectWidth", pre262.rectWidth);
                                    CameraRig.addInfo("rectHeight", pre262.rectHeight);
                                    CameraRig.addInfo("movementVal", pre262.movementVal);
                                    CameraRig.addInfo("displayButton", pre262.displayButton);

                                    CameraRig.addInfo("pubImage", pre262.pubImage);
                                    CameraRig.addInfo("pubTime", pre262.pubTime);
                                    CameraRig.addInfo("pubHours", pre262.pubHours);
                                    CameraRig.addInfo("pubMins", pre262.pubMins);
                                    CameraRig.addInfo("pubSecs", pre262.pubSecs);
                                    CameraRig.addInfo("publishWeb", pre262.publishWeb);
                                    CameraRig.addInfo("publishLocal", pre262.publishLocal);
                                    CameraRig.addInfo("timerOn", pre262.timerOn);
                                    CameraRig.addInfo("fileURLPubWeb", pre262.fileURLPubWeb);
                                    CameraRig.addInfo("filenamePrefixPubWeb", pre262.filenamePrefixPubWeb);
                                    CameraRig.addInfo("cycleStampCheckedPubWeb", pre262.cycleStampCheckedPubWeb);
                                    CameraRig.addInfo("startCyclePubWeb", pre262.startCyclePubWeb);
                                    CameraRig.addInfo("endCyclePubWeb", pre262.endCyclePubWeb);
                                    CameraRig.addInfo("currentCyclePubWeb", pre262.currentCyclePubWeb);
                                    CameraRig.addInfo("stampAppendPubWeb", pre262.stampAppendPubWeb);
                                    CameraRig.addInfo("fileDirAlertLoc", pre262.fileDirAlertLoc);
                                    CameraRig.addInfo("fileDirAlertCust", pre262.fileDirAlertCust);
                                    CameraRig.addInfo("fileDirPubLoc", pre262.fileDirPubLoc);
                                    CameraRig.addInfo("fileDirPubCust", pre262.fileDirPubCust);
                                    CameraRig.addInfo("filenamePrefixPubLoc", pre262.filenamePrefixPubLoc);
                                    CameraRig.addInfo("cycleStampCheckedPubLoc", pre262.cycleStampCheckedPubLoc);
                                    CameraRig.addInfo("startCyclePubLoc", pre262.startCyclePubLoc);
                                    CameraRig.addInfo("endCyclePubLoc", pre262.endCyclePubLoc);
                                    CameraRig.addInfo("currentCyclePubLoc", pre262.currentCyclePubLoc);
                                    CameraRig.addInfo("stampAppendPubLoc", pre262.stampAppendPubLoc);

                                    CameraRig.addInfo("ipWebcamAddress", pre262.ipWebcamAddress);
                                    CameraRig.addInfo("ipWebcamUser", pre262.ipWebcamUser);
                                    CameraRig.addInfo("ipWebcamPassword", pre262.ipWebcamPassword);


                                }

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

                            if (configData.LocalName.Equals("webcam"))
                            {
                                config.getProfile("##newProf##").webcam = configData.ReadString();
                                CameraRig.addInfo("webcam", config.getProfile("##newProf##").webcam);
                                CameraRig.addInfo("profileName", profileName.ToLower());
                            }


                            if (decimal.Parse(profileVer) > 2.62m)//m forces number to be interpreted as decimal
                            {

                                if (configData.LocalName.Equals("alarmActive"))
                                {
                                    CameraRig.addInfo("alarmActive", Convert.ToBoolean(configData.ReadString()));
                                }
                                if (configData.LocalName.Equals("publishActive"))
                                {
                                    CameraRig.addInfo("publishActive", Convert.ToBoolean(configData.ReadString()));
                                }
                                if (configData.LocalName.Equals("friendlyName"))
                                {
                                    CameraRig.addInfo("friendlyName", configData.ReadString());
                                }
                                if (configData.LocalName.Equals("displayButton"))
                                {
                                    CameraRig.addInfo("displayButton", Convert.ToInt32(configData.ReadString()));
                                }
                                if (configData.LocalName.Equals("areaDetection"))
                                {
                                    config.getProfile("##newProf##").areaDetection = Convert.ToBoolean(configData.ReadString());
                                    CameraRig.addInfo("areaDetection", config.getProfile("##newProf##").areaDetection);
                                }
                                if (configData.LocalName.Equals("areaDetectionWithin"))
                                {
                                    config.getProfile("##newProf##").areaDetectionWithin = Convert.ToBoolean(configData.ReadString());
                                    CameraRig.addInfo("areaDetectionWithin", config.getProfile("##newProf##").areaDetectionWithin);
                                }
                                if (configData.LocalName.Equals("areaOffAtMotion"))
                                {
                                    config.getProfile("##newProf##").areaOffAtMotion = Convert.ToBoolean(configData.ReadString());
                                    CameraRig.addInfo("areaOffAtMotion", config.getProfile("##newProf##").areaOffAtMotion);
                                }
                                if (configData.LocalName.Equals("rectX"))
                                {
                                    config.getProfile("##newProf##").rectX = Convert.ToInt32(configData.ReadString());
                                    CameraRig.addInfo("rectX", config.getProfile("##newProf##").rectX);
                                }
                                if (configData.LocalName.Equals("rectY"))
                                {
                                    config.getProfile("##newProf##").rectY = Convert.ToInt32(configData.ReadString());
                                    CameraRig.addInfo("rectY", config.getProfile("##newProf##").rectY);
                                }
                                if (configData.LocalName.Equals("rectWidth"))
                                {
                                    config.getProfile("##newProf##").rectWidth = Convert.ToInt32(configData.ReadString());
                                    CameraRig.addInfo("rectWidth", config.getProfile("##newProf##").rectWidth);
                                }
                                if (configData.LocalName.Equals("rectHeight"))
                                {
                                    config.getProfile("##newProf##").rectHeight = Convert.ToInt32(configData.ReadString());
                                    CameraRig.addInfo("rectHeight", config.getProfile("##newProf##").rectHeight);
                                }
                                if (configData.LocalName.Equals("movementVal"))
                                {
                                    config.getProfile("##newProf##").movementVal = Convert.ToDouble(configData.ReadString());
                                    CameraRig.addInfo("movementVal", config.getProfile("##newProf##").movementVal);
                                }



                                if (configData.LocalName.Equals("pubTime"))
                                {
                                    CameraRig.addInfo("pubTime", Convert.ToInt32(configData.ReadString()));
                                }
                                if (configData.LocalName.Equals("pubHours"))
                                {
                                    CameraRig.addInfo("pubHours", Convert.ToBoolean(configData.ReadString()));
                                }
                                if (configData.LocalName.Equals("pubMins"))
                                {
                                    CameraRig.addInfo("pubMins", Convert.ToBoolean(configData.ReadString()));
                                }
                                if (configData.LocalName.Equals("pubSecs"))
                                {
                                    CameraRig.addInfo("pubSecs", Convert.ToBoolean(configData.ReadString()));
                                }
                                if (configData.LocalName.Equals("publishWeb"))
                                {
                                    CameraRig.addInfo("publishWeb", Convert.ToBoolean(configData.ReadString()));
                                }
                                if (configData.LocalName.Equals("publishLocal"))
                                {
                                    CameraRig.addInfo("publishLocal", Convert.ToBoolean(configData.ReadString()));
                                }
                                if (configData.LocalName.Equals("timerOn"))
                                {
                                    CameraRig.addInfo("timerOn", Convert.ToBoolean(configData.ReadString()));
                                }
                                if (configData.LocalName.Equals("fileURLPubWeb"))
                                {
                                    CameraRig.addInfo("fileURLPubWeb", configData.ReadString());
                                }
                                if (configData.LocalName.Equals("filenamePrefixPubWeb"))
                                {
                                    CameraRig.addInfo("filenamePrefixPubWeb", configData.ReadString());
                                }
                                if (configData.LocalName.Equals("cycleStampCheckedPubWeb"))
                                {
                                    CameraRig.addInfo("cycleStampCheckedPubWeb", Convert.ToInt32(configData.ReadString()));
                                }
                                if (configData.LocalName.Equals("startCyclePubWeb"))
                                {
                                    CameraRig.addInfo("startCyclePubWeb", Convert.ToInt32(configData.ReadString()));
                                }
                                if (configData.LocalName.Equals("endCyclePubWeb"))
                                {
                                    CameraRig.addInfo("endCyclePubWeb", Convert.ToInt32(configData.ReadString()));
                                }
                                if (configData.LocalName.Equals("currentCyclePubWeb"))
                                {
                                    CameraRig.addInfo("currentCyclePubWeb", Convert.ToInt32(configData.ReadString()));
                                }
                                if (configData.LocalName.Equals("stampAppendPubWeb"))
                                {
                                    CameraRig.addInfo("stampAppendPubWeb", Convert.ToBoolean(configData.ReadString()));
                                }
                                if (configData.LocalName.Equals("fileAlertPubLoc"))
                                {
                                    CameraRig.addInfo("fileAlertPubLoc", configData.ReadString());
                                }
                                if (configData.LocalName.Equals("fileAlertPubCust"))
                                {
                                    CameraRig.addInfo("fileAlertPubCust", Convert.ToBoolean(configData.ReadString()));
                                }
                                if (configData.LocalName.Equals("fileDirPubLoc"))
                                {
                                    CameraRig.addInfo("fileDirPubLoc", configData.ReadString());
                                }
                                if (configData.LocalName.Equals("fileDirPubCust"))
                                {
                                    CameraRig.addInfo("fileDirPubCust", Convert.ToBoolean(configData.ReadString()));
                                }
                                if (configData.LocalName.Equals("filenamePrefixPubLoc"))
                                {
                                    CameraRig.addInfo("filenamePrefixPubLoc", configData.ReadString());
                                }
                                if (configData.LocalName.Equals("cycleStampCheckedPubLoc"))
                                {
                                    CameraRig.addInfo("cycleStampCheckedPubLoc", Convert.ToInt32(configData.ReadString()));
                                }
                                if (configData.LocalName.Equals("startCyclePubLoc"))
                                {
                                    CameraRig.addInfo("startCyclePubLoc", Convert.ToInt32(configData.ReadString()));
                                }
                                if (configData.LocalName.Equals("endCyclePubLoc"))
                                {
                                    CameraRig.addInfo("endCyclePubLoc", Convert.ToInt32(configData.ReadString()));
                                }
                                if (configData.LocalName.Equals("currentCyclePubLoc"))
                                {
                                    CameraRig.addInfo("currentCyclePubLoc", Convert.ToInt32(configData.ReadString()));
                                }
                                if (configData.LocalName.Equals("stampAppendPubLoc"))
                                {
                                    CameraRig.addInfo("stampAppendPubLoc", Convert.ToBoolean(configData.ReadString()));
                                }
                                if (configData.LocalName.Equals("ipWebcamAddress"))
                                {
                                    CameraRig.addInfo("ipWebcamAddress", configData.ReadString());
                                }
                                if (configData.LocalName.Equals("ipWebcamUser"))
                                {
                                    CameraRig.addInfo("ipWebcamUser",decrypt( configData.ReadString()));
                                }
                                if (configData.LocalName.Equals("ipWebcamPassword"))
                                {
                                    CameraRig.addInfo("ipWebcamPassword", decrypt(configData.ReadString()));
                                }




                            }
                            else
                            {
                                if (configData.LocalName.Equals("friendlyName"))
                                {
                                    pre262.friendlyName = configData.ReadString();
                                }
                                if (configData.LocalName.Equals("areaDetection"))
                                {
                                    config.getProfile("##newProf##").areaDetection = Boolean.Parse(configData.ReadString());
                                    pre262.areaDetection = Convert.ToBoolean(config.getProfile("##newProf##").areaDetection);
                                }
                                if (configData.LocalName.Equals("areaDetectionWithin"))
                                {
                                    config.getProfile("##newProf##").areaDetectionWithin = Convert.ToBoolean(configData.ReadString());
                                    pre262.areaDetectionWithin = Convert.ToBoolean(config.getProfile("##newProf##").areaDetectionWithin);
                                }
                                if (configData.LocalName.Equals("areaOffAtMotion"))
                                {
                                    config.getProfile("##newProf##").areaOffAtMotion = Convert.ToBoolean(configData.ReadString());
                                    pre262.areaOffAtMotion = Convert.ToBoolean(config.getProfile("##newProf##").areaOffAtMotion);
                                }
                                if (configData.LocalName.Equals("rectX"))
                                {
                                    config.getProfile("##newProf##").rectX = Convert.ToInt32(configData.ReadString());
                                    pre262.rectX = Convert.ToInt32(config.getProfile("##newProf##").rectX);
                                }
                                if (configData.LocalName.Equals("rectY"))
                                {
                                    config.getProfile("##newProf##").rectY = Convert.ToInt32(configData.ReadString());
                                    pre262.rectY = Convert.ToInt32(config.getProfile("##newProf##").rectY);
                                }
                                if (configData.LocalName.Equals("rectWidth"))
                                {
                                    config.getProfile("##newProf##").rectWidth = Convert.ToInt32(configData.ReadString());
                                    pre262.rectWidth = Convert.ToInt32(config.getProfile("##newProf##").rectWidth);
                                }
                                if (configData.LocalName.Equals("rectHeight"))
                                {
                                    config.getProfile("##newProf##").rectHeight = Convert.ToInt32(configData.ReadString());
                                    pre262.rectHeight = Convert.ToInt32(config.getProfile("##newProf##").rectHeight);
                                }
                                if (configData.LocalName.Equals("movementVal"))
                                {
                                    config.getProfile("##newProf##").movementVal = Convert.ToDouble(configData.ReadString());
                                    pre262.movementVal = Convert.ToDouble(config.getProfile("##newProf##").movementVal);
                                }


                                if (configData.LocalName.Equals("pubTime"))
                                {
                                    pre262.pubTime = Convert.ToInt32(configData.ReadString());
                                }
                                if (configData.LocalName.Equals("pubHours"))
                                {
                                    pre262.pubHours = Convert.ToBoolean(configData.ReadString());
                                }
                                if (configData.LocalName.Equals("pubMins"))
                                {
                                    pre262.pubMins = Convert.ToBoolean(configData.ReadString());
                                }
                                if (configData.LocalName.Equals("pubSecs"))
                                {
                                    pre262.pubSecs = Convert.ToBoolean(configData.ReadString());
                                }
                                if (configData.LocalName.Equals("publishWeb"))
                                {
                                    pre262.publishWeb = Convert.ToBoolean(configData.ReadString());
                                }
                                if (configData.LocalName.Equals("publishLocal"))
                                {
                                    pre262.publishLocal = Convert.ToBoolean(configData.ReadString());
                                }
                                if (configData.LocalName.Equals("timerOn"))
                                {
                                    pre262.timerOn = Convert.ToBoolean(configData.ReadString());
                                }
                                if (configData.LocalName.Equals("fileURLPubWeb"))
                                {
                                    pre262.fileURLPubWeb = configData.ReadString();
                                }
                                if (configData.LocalName.Equals("filenamePrefixPubWeb"))
                                {
                                    pre262.filenamePrefixPubWeb = configData.ReadString();
                                }
                                if (configData.LocalName.Equals("cycleStampCheckedPubWeb"))
                                {
                                    pre262.cycleStampCheckedPubWeb = Convert.ToInt32(configData.ReadString());
                                }
                                if (configData.LocalName.Equals("startCyclePubWeb"))
                                {
                                    pre262.startCyclePubWeb = Convert.ToInt32(configData.ReadString());
                                }
                                if (configData.LocalName.Equals("endCyclePubWeb"))
                                {
                                    pre262.endCyclePubWeb = Convert.ToInt32(configData.ReadString());
                                }
                                if (configData.LocalName.Equals("currentCyclePubWeb"))
                                {
                                    pre262.currentCyclePubWeb = Convert.ToInt32(configData.ReadString());
                                }
                                if (configData.LocalName.Equals("stampAppendPubWeb"))
                                {
                                    pre262.stampAppendPubWeb = Convert.ToBoolean(configData.ReadString());
                                }
                                if (configData.LocalName.Equals("fileDirAlertLoc"))
                                {
                                    pre262.fileDirAlertLoc = configData.ReadString();
                                }
                                if (configData.LocalName.Equals("fileDirAlertCust"))
                                {
                                    pre262.fileDirAlertCust = Convert.ToBoolean(configData.ReadString());
                                }
                                if (configData.LocalName.Equals("fileDirPubLoc"))
                                {
                                    pre262.fileDirPubLoc = configData.ReadString();
                                }
                                if (configData.LocalName.Equals("fileDirPubCust"))
                                {
                                    pre262.fileDirPubCust = Convert.ToBoolean(configData.ReadString());
                                }
                                if (configData.LocalName.Equals("filenamePrefixPubLoc"))
                                {
                                    pre262.filenamePrefixPubLoc = configData.ReadString();
                                }
                                if (configData.LocalName.Equals("cycleStampCheckedPubLoc"))
                                {
                                    pre262.cycleStampCheckedPubLoc = Convert.ToInt32(configData.ReadString());
                                }
                                if (configData.LocalName.Equals("startCyclePubLoc"))
                                {
                                    pre262.startCyclePubLoc = Convert.ToInt32(configData.ReadString());
                                }
                                if (configData.LocalName.Equals("endCyclePubLoc"))
                                {
                                    pre262.endCyclePubLoc = Convert.ToInt32(configData.ReadString());
                                }
                                if (configData.LocalName.Equals("currentCyclePubLoc"))
                                {
                                    pre262.currentCyclePubLoc = Convert.ToInt32(configData.ReadString());
                                }
                                if (configData.LocalName.Equals("stampAppendPubLoc"))
                                {
                                    pre262.stampAppendPubLoc = Convert.ToBoolean(configData.ReadString());
                                }
                                if (configData.LocalName.Equals("ipWebcamAddress"))
                                {
                                    pre262.ipWebcamAddress = configData.ReadString();
                                }
                                if (configData.LocalName.Equals("ipWebcamUser"))
                                {
                                    pre262.ipWebcamUser = decrypt(configData.ReadString());
                                }
                                if (configData.LocalName.Equals("ipWebcamPassword"))
                                {
                                    pre262.ipWebcamPassword = decrypt(configData.ReadString());
                                }


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
                            if (configData.LocalName.Equals("pingInterval"))
                            {
                                config.getProfile("##newProf##").pingInterval = Convert.ToInt32(configData.ReadString());
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

            #region ::::::::::::::::::::::::Write Graph::::::::::::::::::::::::

            if (file == "graphInit")
            {
                try
                {
                    string fileName = bubble.xmlFolder + graphFile + ".xml";
                    XmlTextWriter graphData = new XmlTextWriter(fileName, null);

                    graphData.Formatting = Formatting.Indented;
                    graphData.Indentation = 4;
                    graphData.Namespaces = false;
                    graphData.WriteStartDocument();

                    graphData.WriteStartElement("", "graph", "");

                    graphData.WriteStartElement("", "date", "");
                    graphData.WriteString("19700511");
                    graphData.WriteEndElement();

                    graphData.WriteStartElement("", "val", "");
                    graphData.WriteString("3:10");
                    graphData.WriteEndElement();

                    graphData.WriteEndElement();
                    graphData.WriteEndDocument();
                    graphData.Flush();
                    graphData.Close();

                }
                catch
                {
                }
            }

            if (file == "graph")
            {
                try
                {
                    string fileName = bubble.xmlFolder + graphFile + ".xml";
                    XmlTextWriter graphData = new XmlTextWriter(fileName, null);

                    graphData.Formatting = Formatting.Indented;
                    graphData.Indentation = 4;
                    graphData.Namespaces = false;
                    graphData.WriteStartDocument();

                    graphData.WriteStartElement("", "graph", "");

                    ArrayList dates = Graph.getGraphDates();

                    foreach (string date in dates)
                    {

                        ArrayList vals = Graph.getGraphHist(date);
                        bool hasVals = false;

                        foreach (int val in vals)
                        {
                            if (val > 0)
                            {
                                hasVals = true;
                            }
                        }

                        if (hasVals)
                        {

                            graphData.WriteStartElement("", "date", "");
                            graphData.WriteString(date);
                            graphData.WriteEndElement();

                            int valIdx = 0;

                            foreach (int val in vals)
                            {
                                if (val > 0)
                                {
                                    graphData.WriteStartElement("", "val", "");
                                    graphData.WriteString(valIdx.ToString() + ":" + val.ToString());
                                    graphData.WriteEndElement();
                                }
                                valIdx++;
                            }

                        }

                    }

                    graphData.WriteEndElement();
                    graphData.WriteEndDocument();
                    graphData.Flush();
                    graphData.Close();

                }
                catch
                {
                }
            }
            #endregion

            #region ::::::::::::::::::::::::Write Log::::::::::::::::::::::::

            if (file == "logInit")
            {
                try
                {
                    string fileName = bubble.xmlFolder + logFile + ".xml";
                    XmlTextWriter logData = new XmlTextWriter(fileName, null);

                    logData.Formatting = Formatting.Indented;
                    logData.Indentation = 4;
                    logData.Namespaces = false;
                    logData.WriteStartDocument();

                    logData.WriteStartElement("", "log", "");

                    logData.WriteEndElement();
                    logData.WriteEndDocument();
                    logData.Flush();
                    logData.Close();

                }
                catch
                {
                }
            }
            if (file == "log")
            {
                try
                {
                    string fileName = bubble.xmlFolder + logFile + ".xml";
                    XmlTextWriter logData = new XmlTextWriter(fileName, null);

                    logData.Formatting = Formatting.Indented;
                    logData.Indentation = 4;
                    logData.Namespaces = false;
                    logData.WriteStartDocument();

                    logData.WriteStartElement("", "log", "");

                    //###
                    foreach (string logEntry in bubble.log)
                    {
                        logData.WriteStartElement("", "log", "");
                        logData.WriteString(logEntry.ToString());
                        logData.WriteEndElement();
                    }

                    logData.WriteEndElement();
                    logData.WriteEndDocument();
                    logData.Flush();
                    logData.Close();

                }
                catch
                {
                }
            }
            #endregion
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

                    configData.WriteStartElement("", "version", "");
                    configData.WriteString(bubble.version);
                    configData.WriteEndElement();

                    configData.WriteStartElement("", "profileInUse", "");
                    configData.WriteString(bubble.profileInUse);
                    configData.WriteEndElement();

                    configData.WriteStartElement("", "newsSeq", "");
                    configData.WriteString(bubble.newsSeq.ToString());
                    configData.WriteEndElement();

                    configData.WriteStartElement("", "mysqlDriver", "");
                    configData.WriteString(bubble.mysqlDriver);
                    configData.WriteEndElement();



                    config.getFirstProfile();

                    while (config.moreProfiles())
                    {

                        configData.Indentation = 4;

                        configData.WriteStartElement("", "profileStart", "");
                        configData.WriteString(config.getProfile().profileName.ToLower());
                        configData.WriteEndElement();

                        configData.Indentation = 8;

                        configData.WriteStartElement("", "sentByName", "");
                        configData.WriteString(config.getProfile().sentByName);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "pingSubject", "");
                        configData.WriteString(config.getProfile().pingSubject);
                        configData.WriteEndElement();

                        configData.Indentation = 12;

                        //!!!!!!!!!!!!!!!!!!!!!!!!!!
                        //Webcam individual settings
                        //!!!!!!!!!!!!!!!!!!!!!!!!!!
                        foreach (info infoI in CameraRig.camInfo)
                        {

                            if (infoI.profileName.ToLower() == config.getProfile().profileName.ToLower())
                            {

                                configData.WriteStartElement("", "webcam", "");
                                configData.WriteString(infoI.webcam);
                                configData.WriteEndElement();

                                configData.Indentation = 16;

                                configData.WriteStartElement("", "friendlyName", "");
                                configData.WriteString(infoI.friendlyName);
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "alarmActive", "");
                                configData.WriteString(infoI.alarmActive.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "publishActive", "");
                                configData.WriteString(infoI.publishActive.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "displayButton", "");
                                configData.WriteString(infoI.displayButton.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "areaDetection", "");
                                configData.WriteString(infoI.areaDetection.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "areaDetectionWithin", "");
                                configData.WriteString(infoI.areaDetectionWithin.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "areaOffAtMotion", "");
                                configData.WriteString(infoI.areaOffAtMotion.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "rectX", "");
                                configData.WriteString(infoI.rectX.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "rectY", "");
                                configData.WriteString(infoI.rectY.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "rectWidth", "");
                                configData.WriteString(infoI.rectWidth.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "rectHeight", "");
                                configData.WriteString(infoI.rectHeight.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "movementVal", "");
                                configData.WriteString(infoI.movementVal.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "pubImage", "");
                                configData.WriteString(infoI.pubImage.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "pubTime", "");
                                configData.WriteString(infoI.pubTime.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "pubHours", "");
                                configData.WriteString(infoI.pubHours.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "pubMins", "");
                                configData.WriteString(infoI.pubMins.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "pubSecs", "");
                                configData.WriteString(infoI.pubSecs.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "publishWeb", "");
                                configData.WriteString(infoI.publishWeb.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "publishLocal", "");
                                configData.WriteString(infoI.publishLocal.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "timerOn", "");
                                configData.WriteString(infoI.timerOn.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "fileURLPubWeb", "");
                                configData.WriteString(infoI.fileURLPubWeb.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "filenamePrefixPubWeb", "");
                                configData.WriteString(infoI.filenamePrefixPubWeb.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "cycleStampCheckedPubWeb", "");
                                configData.WriteString(infoI.cycleStampCheckedPubWeb.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "startCyclePubWeb", "");
                                configData.WriteString(infoI.startCyclePubWeb.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "endCyclePubWeb", "");
                                configData.WriteString(infoI.endCyclePubWeb.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "currentCyclePubWeb", "");
                                configData.WriteString(infoI.currentCyclePubWeb.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "stampAppendPubWeb", "");
                                configData.WriteString(infoI.stampAppendPubWeb.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "fileDirAlertLoc", "");
                                configData.WriteString(infoI.fileDirAlertLoc.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "fileDirAlertCust", "");
                                configData.WriteString(infoI.fileDirAlertCust.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "fileDirPubLoc", "");
                                configData.WriteString(infoI.fileDirPubLoc.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "fileDirPubCust", "");
                                configData.WriteString(infoI.fileDirPubCust.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "filenamePrefixPubLoc", "");
                                configData.WriteString(infoI.filenamePrefixPubLoc.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "cycleStampCheckedPubLoc", "");
                                configData.WriteString(infoI.cycleStampCheckedPubLoc.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "startCyclePubLoc", "");
                                configData.WriteString(infoI.startCyclePubLoc.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "endCyclePubLoc", "");
                                configData.WriteString(infoI.endCyclePubLoc.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "currentCyclePubLoc", "");
                                configData.WriteString(infoI.currentCyclePubLoc.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "stampAppendPubLoc", "");
                                configData.WriteString(infoI.stampAppendPubLoc.ToString());
                                configData.WriteEndElement();
                                                                
                                configData.WriteStartElement("", "ipWebcamAddress", "");
                                configData.WriteString(infoI.ipWebcamAddress.ToString());
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "ipWebcamUser", "");
                                configData.WriteString(encrypt(infoI.ipWebcamUser.ToString()));
                                configData.WriteEndElement();

                                configData.WriteStartElement("", "ipWebcamPassword", "");
                                configData.WriteString(encrypt(infoI.ipWebcamPassword.ToString()));
                                configData.WriteEndElement();
                                
                                configData.Indentation = 12;

                            }

                        }

                        //!!!!!!!!!!!!!!!!!!!!!!!!!!
                        //Webcam individual settings
                        //!!!!!!!!!!!!!!!!!!!!!!!!!!

                        configData.Indentation = 8;

                        configData.WriteStartElement("", "freezeGuard", "");
                        configData.WriteString(config.getProfile().freezeGuard.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "freezeGuardWindowShow", "");
                        configData.WriteString(config.getProfile().freezeGuardWindowShow.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "updatesNotify", "");
                        configData.WriteString(config.getProfile().updatesNotify.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "countdownNow", "");
                        configData.WriteString(config.getProfile().countdownNow.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "activatecountdown", "");
                        configData.WriteString(config.getProfile().activatecountdown.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "activatecountdownTime", "");
                        configData.WriteString(config.getProfile().activatecountdownTime);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "countdownTime", "");
                        configData.WriteString(config.getProfile().countdownTime.ToString());
                        configData.WriteEndElement();


                        configData.WriteStartElement("", "alert", "");
                        configData.WriteString(bubble.Alert.on.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "maxImagesToEmail", "");
                        configData.WriteString(config.getProfile().maxImagesToEmail.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "sendNotifyEmail", "");
                        configData.WriteString(config.getProfile().sendNotifyEmail.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "ping", "");
                        configData.WriteString(config.getProfile().ping.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "pingInterval", "");
                        configData.WriteString(config.getProfile().pingInterval.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "sendFullSizeImages", "");
                        configData.WriteString(config.getProfile().sendFullSizeImages.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "sendThumbnailImages", "");
                        configData.WriteString(config.getProfile().sendThumbnailImages.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "sendMosaicImages", "");
                        configData.WriteString(config.getProfile().sendMosaicImages.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "mosaicImagesPerRow", "");
                        configData.WriteString(config.getProfile().mosaicImagesPerRow.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "loadImagesToFtp", "");
                        configData.WriteString(config.getProfile().loadImagesToFtp.ToString());
                        configData.WriteEndElement();
                        //###
                        configData.WriteStartElement("", "baselineVal", "");
                        configData.WriteString(config.getProfile().baselineVal.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "imageSaveInterval", "");
                        configData.WriteString(config.getProfile().imageSaveInterval.ToString());
                        configData.WriteEndElement();


                        configData.WriteStartElement("", "filenamePrefix", "");
                        configData.WriteString(config.getProfile().filenamePrefix);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "cycleStampChecked", "");
                        configData.WriteString(config.getProfile().cycleStampChecked.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "startCycle", "");
                        configData.WriteString(config.getProfile().startCycle.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "endCycle", "");
                        configData.WriteString(config.getProfile().endCycle.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "currentCycle", "");
                        configData.WriteString(config.getProfile().currentCycle.ToString());
                        configData.WriteEndElement();


                        configData.WriteStartElement("", "emailNotifyInterval", "");
                        configData.WriteString(config.getProfile().emailNotifyInterval.ToString());
                        configData.WriteEndElement();
                        //###
                        configData.WriteStartElement("", "emailUser", "");
                        configData.WriteString(config.getProfile().emailUser);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "emailPassword", "");
                        configData.WriteString(encrypt(config.getProfile().emailPass));
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "smtpHost", "");
                        configData.WriteString(config.getProfile().smtpHost);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "stmpPort", "");
                        configData.WriteString(config.getProfile().smtpPort.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "ssl", "");
                        configData.WriteString(config.getProfile().EnableSsl.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "sendTo", "");
                        configData.WriteString(config.getProfile().sendTo);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "replyTo", "");
                        configData.WriteString(config.getProfile().replyTo);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "sentBy", "");
                        configData.WriteString(config.getProfile().sentBy);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "mailSubject", "");
                        configData.WriteString(config.getProfile().mailSubject);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "mailBody", "");
                        configData.WriteString(config.getProfile().mailBody);
                        configData.WriteEndElement();
                        //###
                        configData.WriteStartElement("", "ftpUser", "");
                        configData.WriteString(config.getProfile().ftpUser);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "ftpPass", "");
                        configData.WriteString(encrypt(config.getProfile().ftpPass));
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "ftpRoot", "");
                        configData.WriteString(config.getProfile().ftpRoot);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "pubImage", "");
                        configData.WriteString(config.getProfile().pubImage.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "pubFtpUser", "");
                        configData.WriteString(config.getProfile().pubFtpUser);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "pubFtpPass", "");
                        configData.WriteString(encrypt(config.getProfile().pubFtpPass));
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "pubFtpRoot", "");
                        configData.WriteString(config.getProfile().pubFtpRoot);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "timerOn", "");
                        configData.WriteString(config.getProfile().timerOn.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "timerStartPub", "");
                        configData.WriteString(config.getProfile().timerStartPub);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "timerEndPub", "");
                        configData.WriteString(config.getProfile().timerEndPub);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "timerOnMov", "");
                        configData.WriteString(config.getProfile().timerOnMov.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "scheduleOnAtStart", "");
                        configData.WriteString(config.getProfile().scheduleOnAtStart.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "activateAtEveryStartup", "");
                        configData.WriteString(config.getProfile().activateAtEveryStartup.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "timerStartMov", "");
                        configData.WriteString(config.getProfile().timerStartMov);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "timerEndMov", "");
                        configData.WriteString(config.getProfile().timerEndMov);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "webUpd", "");
                        configData.WriteString(config.getProfile().webUpd.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "webUser", "");
                        configData.WriteString(config.getProfile().webUser);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "webPass", "");
                        configData.WriteString(encrypt(config.getProfile().webPass));
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "webFtpPass", "");
                        configData.WriteString(encrypt(config.getProfile().webFtpPass));
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "webFtpUser", "");
                        configData.WriteString(config.getProfile().webFtpUser);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "webPoll", "");
                        configData.WriteString(config.getProfile().webPoll.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "webInstance", "");
                        configData.WriteString(config.getProfile().webInstance.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "webImageRoot", "");
                        configData.WriteString(config.getProfile().webImageRoot.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "webImageFileName", "");
                        configData.WriteString(config.getProfile().webImageFileName.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "soundAlert", "");
                        configData.WriteString(config.getProfile().soundAlert);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "soundAlertOn", "");
                        configData.WriteString(config.getProfile().soundAlertOn.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "logsKeep", "");
                        configData.WriteString(config.getProfile().logsKeep.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "logsKeepChk", "");
                        configData.WriteString(config.getProfile().logsKeepChk.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "imageParentFolderCust", "");
                        configData.WriteString(config.getProfile().imageParentFolderCust);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "imageFolderCust", "");
                        configData.WriteString(config.getProfile().imageFolderCust);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "thumbFolderCust", "");
                        configData.WriteString(config.getProfile().thumbFolderCust);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "imageLocCust", "");
                        configData.WriteString(config.getProfile().imageLocCust.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "startTeboCamMinimized", "");
                        configData.WriteString(config.getProfile().startTeboCamMinimized.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "internetCheck", "");
                        configData.WriteString(config.getProfile().internetCheck);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "toolTips", "");
                        configData.WriteString(config.getProfile().toolTips.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "alertCompression", "");
                        configData.WriteString(config.getProfile().alertCompression.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "publishCompression", "");
                        configData.WriteString(config.getProfile().publishCompression.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "pingCompression", "");
                        configData.WriteString(config.getProfile().pingCompression.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "onlineCompression", "");
                        configData.WriteString(config.getProfile().onlineCompression.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "alertTimeStamp", "");
                        configData.WriteString(config.getProfile().alertTimeStamp.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "alertTimeStampFormat", "");
                        configData.WriteString(config.getProfile().alertTimeStampFormat);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "alertStatsStamp", "");
                        configData.WriteString(config.getProfile().alertStatsStamp.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "alertTimeStampColour", "");
                        configData.WriteString(config.getProfile().alertTimeStampColour);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "alertTimeStampPosition", "");
                        configData.WriteString(config.getProfile().alertTimeStampPosition);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "alertTimeStampRect", "");
                        configData.WriteString(config.getProfile().alertTimeStampRect.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "publishTimeStamp", "");
                        configData.WriteString(config.getProfile().publishTimeStamp.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "publishTimeStampFormat", "");
                        configData.WriteString(config.getProfile().publishTimeStampFormat);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "publishStatsStamp", "");
                        configData.WriteString(config.getProfile().publishStatsStamp.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "publishTimeStampColour", "");
                        configData.WriteString(config.getProfile().publishTimeStampColour);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "publishTimeStampPosition", "");
                        configData.WriteString(config.getProfile().publishTimeStampPosition);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "publishTimeStampRect", "");
                        configData.WriteString(config.getProfile().publishTimeStampRect.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "pingTimeStamp", "");
                        configData.WriteString(config.getProfile().pingTimeStamp.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "pingTimeStampFormat", "");
                        configData.WriteString(config.getProfile().pingTimeStampFormat);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "pingStatsStamp", "");
                        configData.WriteString(config.getProfile().pingStatsStamp.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "pingTimeStampColour", "");
                        configData.WriteString(config.getProfile().pingTimeStampColour);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "pingTimeStampPosition", "");
                        configData.WriteString(config.getProfile().pingTimeStampPosition);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "pingTimeStampRect", "");
                        configData.WriteString(config.getProfile().pingTimeStampRect.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "onlineTimeStamp", "");
                        configData.WriteString(config.getProfile().onlineTimeStamp.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "onlineTimeStampFormat", "");
                        configData.WriteString(config.getProfile().onlineTimeStampFormat);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "onlineStatsStamp", "");
                        configData.WriteString(config.getProfile().onlineStatsStamp.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "onlineTimeStampColour", "");
                        configData.WriteString(config.getProfile().onlineTimeStampColour);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "onlineTimeStampPosition", "");
                        configData.WriteString(config.getProfile().onlineTimeStampPosition);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "onlineTimeStampRect", "");
                        configData.WriteString(config.getProfile().onlineTimeStampRect.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "publishLocal", "");
                        configData.WriteString(config.getProfile().publishLocal.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "publishWeb", "");
                        configData.WriteString(config.getProfile().publishWeb.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "imageToframe", "");
                        configData.WriteString(config.getProfile().imageToframe.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "cameraShow", "");
                        configData.WriteString(config.getProfile().cameraShow.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "motionLevel", "");
                        configData.WriteString(config.getProfile().motionLevel.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "selectedCam", "");
                        configData.WriteString(config.getProfile().selectedCam);
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "pulseFreq", "");
                        configData.WriteString(config.getProfile().pulseFreq.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "EmailIntelOn", "");
                        configData.WriteString(config.getProfile().EmailIntelOn.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "emailIntelEmails", "");
                        configData.WriteString(config.getProfile().emailIntelEmails.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "emailIntelMins", "");
                        configData.WriteString(config.getProfile().emailIntelMins.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "EmailIntelStop", "");
                        configData.WriteString(config.getProfile().EmailIntelStop.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "disCommOnline", "");
                        configData.WriteString(config.getProfile().disCommOnline.ToString());
                        configData.WriteEndElement();

                        configData.WriteStartElement("", "disCommOnlineSecs", "");
                        configData.WriteString(config.getProfile().disCommOnlineSecs.ToString());
                        configData.WriteEndElement();


                        //******************************
                        //Do not put anything after this
                        //******************************
                        configData.Indentation = 4;
                        configData.WriteStartElement("", "profileEnd", "");
                        configData.WriteString(config.getProfile().profileName.ToLower());
                        configData.WriteEndElement();
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
