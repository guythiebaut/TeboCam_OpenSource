using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;

namespace TeboCam
{
    public class Publisher
    {
        public event EventHandler redrawGraph;
        public event EventHandler pulseEvent;
        public delegate void pubPictureDelegate(ImagePub.ImagePubArgs a);


        public bool keepPublishing;
        public  static bool pubError;

        Graph graph;
        long lastProcessedTime;
        IException tebowebException;
        IMail mail;
        string tmpFolder;
        string thumbFolder;
        string tmbPrefix;
        string imageFolder;
        string xmlFolder;
        string mosaicFile;
        string profileInUse;
        Configuration configuration;
        Log log;
        ArrayList moveStats;
        Int64 testImagePublishLast = 0;
        int testImagePublishCount = 0;
        static bool haveTheFlag = false;

        public Publisher(ref Graph gp, IMail imail,
                        string tempFolder, string tmbPrfx, string thumbFld,
                        string imageFld, string xmlFld, string mosaicFl,
                        string profile, Configuration config, Log lg,
                        ArrayList move)
        {
            graph = gp;
            mail = imail;
            tmpFolder = tempFolder;
            thumbFolder = thumbFld;
            tmbPrefix = tmbPrfx;
            imageFolder = imageFld;
            xmlFolder = xmlFld;
            mosaicFile = mosaicFl;
            profileInUse = profile;
            configuration = config;
            log = lg;
            moveStats = move;
        }

        public void moveStatsInitialise()
        {
            for (int i = 0; i < 12; i++)
            {
                moveStats.Add(0);
            }
        }

        public void movementPublish()
        {
            bool spamStopEmail = false;
            int emailToProcess = Movement.imagesFromMovement.emailToProcess();
            int ftpToProcess = Movement.imagesFromMovement.ftpToProcess();
            teboDebug.writeline(teboDebug.movementPublishVal + 1);
            pulseEvent(null, new EventArgs());

            if (!graph.dataExistsForDate(time.currentDateYYYYMMDD()))
            {
                teboDebug.writeline(teboDebug.movementPublishVal + 2);
                moveStats.Clear();
                moveStatsInitialise();
                graph.updateGraphHist(time.currentDateYYYYMMDD(), moveStats);
            }

            //we have images to process however the option is set to not load to ftp site and not email images
            if (ftpToProcess + emailToProcess > 0
                && !ConfigurationHelper.GetCurrentProfile().sendNotifyEmail
                && !ConfigurationHelper.GetCurrentProfile().loadImagesToFtp)
            {
                teboDebug.writeline(teboDebug.movementPublishVal + 3);
                log.AddLine( "Email and ftp set to OFF(see images folder), files created: " + emailToProcess.ToString());
                Movement.imagesFromMovement.listsClear(Movement.imagesFromMovement.TypeEnum.All);
                graph.updateGraphHist(time.currentDateYYYYMMDD(), moveStats);
                if (graph.graphCurrentDate == time.currentDateYYYYMMDD()) { redrawGraph(null, new EventArgs()); }
            }

            //*************************************************************************************************
            //Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp 
            //*************************************************************************************************
            //we have images to load to the ftp site and the option is set to load to ftp site
            if (ConfigurationHelper.GetCurrentProfile().loadImagesToFtp && ftpToProcess > 0)
            {
                //ftp images - start
                if (ConfigurationHelper.GetCurrentProfile().loadImagesToFtp)
                {
                    teboDebug.writeline(teboDebug.movementPublishVal + 5);
                    try
                    {
                        teboDebug.writeline(teboDebug.movementPublishVal + 6);
                        pulseEvent(null, new EventArgs());
                        int pulseCount = 0;

                        foreach (var item in Movement.imagesFromMovement.imageList)
                        {
                            if (!item.ftp)
                            {
                                item.ftp = true;
                                teboDebug.writeline(teboDebug.movementPublishVal + 7);
                                log.AddLine( "Uploading to ftp site");
                                ftp.Upload(imageFolder + item.fileName, ConfigurationHelper.GetCurrentProfile().ftpRoot, ConfigurationHelper.GetCurrentProfile().ftpUser, ConfigurationHelper.GetCurrentProfile().ftpPass, 0);
                                pulseCount++;

                                if (pulseCount > 4)
                                {
                                    pulseCount = 0;
                                    pulseEvent(null, new EventArgs());
                                }
                            }
                        }

                        if (!ConfigurationHelper.GetCurrentProfile().sendNotifyEmail) Movement.imagesFromMovement.listsClear(Movement.imagesFromMovement.TypeEnum.Ftp);
                    }
                    catch (Exception e)
                    {
                        TebocamState.tebowebException.LogException(e);
                    }
                }
            }
            //*************************************************************************************************
            //Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp Ftp 
            //*************************************************************************************************

            //*************************************************************************************************
            //Email Email Email Email Email Email Email Email Email Email Email Email Email Email Email Email 
            //*************************************************************************************************

            //Images to process are more than will fit in one email
            //or we have images to process and the email notify interval time has passed
            if (
                ConfigurationHelper.GetCurrentProfile().sendNotifyEmail &&
                (emailToProcess >= ConfigurationHelper.GetCurrentProfile().maxImagesToEmail ||
                (emailToProcess > 0 && (time.secondsSinceStart() - lastProcessedTime) >
                ConfigurationHelper.GetCurrentProfile().emailNotifyInterval))
                && !mail.SpamAlert(ConfigurationHelper.GetCurrentProfile().emailIntelEmails,
                                   ConfigurationHelper.GetCurrentProfile().emailIntelMins,
                                   ConfigurationHelper.GetCurrentProfile().EmailIntelOn,
                                   time.secondsSinceStart())
                )
            {
                if (mail.SpamIsStopped())
                {
                    mail.StopSpam(false);
                    spamStopEmail = true;
                }

                teboDebug.writeline(teboDebug.movementPublishVal + 4);
                log.AddLine( "Images to process: " + emailToProcess.ToString());
                graph.updateGraphHist(time.currentDateYYYYMMDD(), moveStats);
                if (graph.graphCurrentDate == time.currentDateYYYYMMDD()) { redrawGraph(null, new EventArgs()); }

                if (ConfigurationHelper.GetCurrentProfile().sendNotifyEmail)
                {
                    teboDebug.writeline(teboDebug.movementPublishVal + 9);
                    teboDebug.writeline(teboDebug.movementPublishVal + 10);
                    mail.clearAttachments();
                    //the time trigger has caused these emails to be sent
                    //or the despamificator has been switched on and the time has elapsed with the mosaic option selected

                    if (emailToProcess < ConfigurationHelper.GetCurrentProfile().maxImagesToEmail || (spamStopEmail && !ConfigurationHelper.GetCurrentProfile().EmailIntelStop))
                    {
                        teboDebug.writeline(teboDebug.movementPublishVal + 12);

                        //send mosaic
                        if (ConfigurationHelper.GetCurrentProfile().sendMosaicImages || (spamStopEmail && !ConfigurationHelper.GetCurrentProfile().EmailIntelStop))
                        {
                            foreach (var item in Movement.imagesFromMovement.imageList)
                            {
                                if (!item.email)
                                {
                                    item.email = true;
                                    ImageProcessor.mosaic.addToList(thumbFolder + tmbPrefix + item.fileName);
                                }
                            }

                            string rand = new Random(time.secondsSinceStart()).Next(99999).ToString();
                            pulseEvent(null, new EventArgs());

                            if (!spamStopEmail)
                            {
                                ImageProcessor.mosaic.saveMosaicAsJpg(ConfigurationHelper.GetCurrentProfile().mosaicImagesPerRow,
                                                    thumbFolder + rand + mosaicFile,
                                                    ConfigurationHelper.GetCurrentProfile().alertCompression);
                            }
                            else
                            {
                                ImageProcessor.mosaic.saveMosaicAsJpg(10,
                                                    thumbFolder + rand + mosaicFile,
                                                    ConfigurationHelper.GetCurrentProfile().alertCompression);
                            }

                            if (File.Exists(thumbFolder + rand + mosaicFile))
                            {
                                mail.addAttachment(thumbFolder + rand + mosaicFile);
                            }
                        }

                        //send thumbs or fullsize
                        else
                        {
                            teboDebug.writeline(teboDebug.movementPublishVal + 13);

                            foreach (var item in Movement.imagesFromMovement.imageList)
                            {
                                if (!item.email)
                                {
                                    item.email = true;

                                    if (ConfigurationHelper.GetCurrentProfile().sendThumbnailImages && File.Exists(string.Format("{0}{1}{2}", thumbFolder, tmbPrefix, item.fileName)))
                                    {
                                        mail.addAttachment(string.Format("{0}{1}{2}", thumbFolder, tmbPrefix, item.fileName));
                                    }

                                    if (ConfigurationHelper.GetCurrentProfile().sendFullSizeImages && File.Exists(string.Format("{0}{1}", imageFolder, item.fileName)))
                                    {
                                        mail.addAttachment(string.Format("{0}{1}", imageFolder, item.fileName));
                                    }
                                }
                            }

                            pulseEvent(null, new EventArgs());
                        }

                        teboDebug.writeline(teboDebug.movementPublishVal + 14);

                    }
                    //the quantity trigger has caused these emails to be sent 
                    else
                    {
                        teboDebug.writeline(teboDebug.movementPublishVal + 15);

                        //send mosaic
                        if (ConfigurationHelper.GetCurrentProfile().sendMosaicImages)
                        {
                            int imagesProcessed = 0;

                            foreach (var item in Movement.imagesFromMovement.imageList)
                            {
                                if (!item.email)
                                {
                                    item.email = true;
                                    ImageProcessor.mosaic.addToList(string.Format("{0}{1}{2}", thumbFolder, tmbPrefix, item.fileName));
                                    imagesProcessed++;

                                    if (imagesProcessed >= (int)(ConfigurationHelper.GetCurrentProfile().maxImagesToEmail))
                                    {
                                        break;
                                    }
                                }
                            }

                            string rand = new Random(time.secondsSinceStart()).Next(99999).ToString();
                            pulseEvent(null, new EventArgs());

                            ImageProcessor.mosaic.saveMosaicAsJpg(ConfigurationHelper.GetCurrentProfile().mosaicImagesPerRow,
                                                thumbFolder + rand + mosaicFile,
                                                ConfigurationHelper.GetCurrentProfile().alertCompression);

                            if (File.Exists(string.Format("{0}{1}{2}", thumbFolder, rand, mosaicFile)))
                            {
                                mail.addAttachment(string.Format("{0}{1}{2}", thumbFolder, rand, mosaicFile));
                            }
                        }
                        //send thumbs or fullsize
                        else
                        {
                            teboDebug.writeline(teboDebug.movementPublishVal + 16);
                            int imagesProcessed = 0;

                            foreach (var item in Movement.imagesFromMovement.imageList)
                            {
                                if (!item.email)
                                {
                                    item.email = true;
                                    imagesProcessed++;

                                    if (File.Exists(string.Format("{0}{1}{2}", thumbFolder, tmbPrefix, item.fileName)))
                                    {
                                        mail.addAttachment(string.Format("{0}{1}{2}", thumbFolder, tmbPrefix, item.fileName));
                                    }

                                    if (File.Exists(string.Format("{0}{1}", imageFolder, item.fileName)))
                                    {
                                        mail.addAttachment(string.Format("{0}{1}", imageFolder, item.fileName));
                                    }

                                    if (imagesProcessed >= (int)(ConfigurationHelper.GetCurrentProfile().maxImagesToEmail))
                                    {
                                        break;
                                    }
                                }
                            }

                            pulseEvent(null, new EventArgs());
                            teboDebug.writeline(teboDebug.movementPublishVal + 17);
                        }
                    }

                    try
                    {
                        teboDebug.writeline(teboDebug.movementPublishVal + 18);
                        graph.graphSeq++;
                        GraphToSave.graphBitmap.Save(tmpFolder + "graphCurrent" + graph.graphSeq.ToString() + ".jpg", ImageFormat.Jpeg);
                    }
                    catch (Exception e)
                    {
                        TebocamState.tebowebException.LogException(e);
                        log.AddLine( "Error saving graph for emailing;");
                    }

                    teboDebug.writeline(teboDebug.movementPublishVal + 19);
                    pulseEvent(null, new EventArgs());

                    if (File.Exists(tmpFolder + "graphCurrent" + graph.graphSeq.ToString() + ".jpg"))
                    {
                        mail.addAttachment(tmpFolder + "graphCurrent" + graph.graphSeq.ToString() + ".jpg");
                    }

                    log.AddLine( "graphCurrent" + graph.graphSeq.ToString() + ".jpg" + " added to email");
                    Thread.Sleep(500);
                    log.AddLine( "Sending Email");

                    var eml = new EmailFields()
                    {
                        SentBy = ConfigurationHelper.GetCurrentProfile().sentBy,
                        SentByName = ConfigurationHelper.GetCurrentProfile().sentByName,
                        SendTo = ConfigurationHelper.GetCurrentProfile().sendTo,
                        Subject = ConfigurationHelper.GetCurrentProfile().mailSubject,
                        BodyText = ConfigurationHelper.GetCurrentProfile().mailBody,
                        ReplyTo = ConfigurationHelper.GetCurrentProfile().replyTo,
                        Attachments = (ConfigurationHelper.GetCurrentProfile().sendThumbnailImages || ConfigurationHelper.GetCurrentProfile().sendFullSizeImages || ConfigurationHelper.GetCurrentProfile().sendMosaicImages),
                        CurrentTime = time.secondsSinceStart(),
                        User = ConfigurationHelper.GetCurrentProfile().emailUser,
                        Password = ConfigurationHelper.GetCurrentProfile().emailPass,
                        SmtpHost = ConfigurationHelper.GetCurrentProfile().smtpHost,
                        SmtpPort = ConfigurationHelper.GetCurrentProfile().smtpPort,
                        EnableSsl = ConfigurationHelper.GetCurrentProfile().EnableSsl
                    };
                    mail.sendEmail(eml);

                    if (!ConfigurationHelper.GetCurrentProfile().loadImagesToFtp) Movement.imagesFromMovement.listsClear(Movement.imagesFromMovement.TypeEnum.Email);
                    teboDebug.writeline(teboDebug.movementPublishVal + 20);
                    pulseEvent(null, new EventArgs());
                    lastProcessedTime = time.secondsSinceStart();
                    log.WriteXMLFile(xmlFolder + "LogData" + ".xml", log);
                    log.AddLine( "Log data saved.");
                    graph.WriteXMLFile(xmlFolder + "GraphData.xml", graph);
                    log.AddLine( "Graph data saved.");
                    log.AddLine( "Config data saved.");
                    configuration.WriteXmlFile(xmlFolder + FileManager.configFile + ".xml", configuration);
                    Thread.Sleep(500);

                    if (spamStopEmail)
                    {
                        spamStopEmail = false;
                    }
                }
            }
            //*************************************************************************************************
            //Email Email Email Email Email Email Email Email Email Email Email Email Email Email Email Email 
            //*************************************************************************************************
            if (ConfigurationHelper.GetCurrentProfile().loadImagesToFtp && ConfigurationHelper.GetCurrentProfile().sendNotifyEmail)
            {
                Movement.imagesFromMovement.listsClear(Movement.imagesFromMovement.TypeEnum.FtpAndEMail);
            }

            teboDebug.writeline(teboDebug.movementPublishVal + 21);
            pulseEvent(null, new EventArgs());
            teboDebug.writeline(teboDebug.movementPublishVal + 22);
            Thread.Sleep(1000);
        }

        public void publishImage()
        {
            if (keepPublishing)
            {
                foreach (ConnectedCamera item in CameraRig.ConnectedCameras)
                {
                    bool pubToWeb = ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).publishWeb;
                    bool pubToLocal = ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).publishLocal;
                    bool pubThisOne = true;

                    //publish from this camera
                    if (pubThisOne && (pubToWeb || pubToLocal))
                    {
                        int timeMultiplier = 0;
                        int PubInterval = 0;
                        bool secs = ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).pubSecs;
                        bool mins = ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).pubMins;
                        bool hrs = ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).pubHours;

                        if (secs) timeMultiplier = 1;
                        if (mins) timeMultiplier = 60;
                        if (hrs) timeMultiplier = 3600;

                        PubInterval = timeMultiplier * ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).pubTime;

                        var a1 = time.secondsSinceStart();
                        var a2 = ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).lastPublished;
                        var a3 = PubInterval;

                        if (
                            Convert.ToBoolean(ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).publishFirst)
                            || (time.secondsSinceStart() - ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).lastPublished) >= PubInterval
                            )
                        {

                            pulseEvent(null, new EventArgs());
                            ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).publishFirst = false;

                            List<string> lst = new List<string>();

                            if (ConfigurationHelper.GetCurrentProfile().publishStatsStamp)
                            {

                                statistics.movementResults stats = new statistics.movementResults();
                                stats = statistics.statsForCam(item.camera.camNo, profileInUse, "Publish", item.camera.name);

                                lst.Add(stats.avgMvStart.ToString());
                                lst.Add(stats.avgMvLast.ToString());
                                lst.Add(stats.mvNow.ToString());
                                lst.Add(item.camera.alarmActive ? "On" : "Off");
                                string pubTime = ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).pubTime.ToString();

                                switch (timeMultiplier)
                                {
                                    case 1:
                                        lst.Add(pubTime + " Secs");
                                        break;
                                    case 60:
                                        lst.Add(pubTime + " Mins");
                                        break;
                                    case 3600:
                                        lst.Add(pubTime + " Hours");
                                        break;
                                    default:
                                        lst.Add(pubTime + " Secs");
                                        break;
                                }


                            }

                            var a = new ImagePub.ImagePubArgs();
                            a.option = "pub";
                            a.CamNo = item.camera.camNo;
                            a.lst = lst;

                            try {
                                take_picture_publish(a);
                                //pubPicture(a); 
                            }
                            catch (Exception e)
                            {
                                TebocamState.tebowebException.LogException(e);
                            }

                            if (!pubError)
                                try
                                {
                                    teboDebug.writeline(teboDebug.publishImageVal + 3);
                                    pulseEvent(null, new EventArgs());
                                    string pubFile = "";

                                    if (pubToLocal)
                                    {
                                        teboDebug.writeline(teboDebug.publishImageVal + 4);
                                        string locFile = "";
                                        long tmpCycleLoc = new long();
                                        tmpCycleLoc = ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).currentCyclePubLoc;
                                        string cameraPubLoc = ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).fileDirPubLoc;

                                        if (!Directory.Exists(cameraPubLoc))
                                        {
                                            Directory.CreateDirectory(cameraPubLoc);
                                        }

                                        //locFile = imageFolder +
                                        locFile = cameraPubLoc +
                                               FileManager.fileNameSet(ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).filenamePrefixPubLoc,
                                                              ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).cycleStampCheckedPubLoc,
                                                              ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).startCyclePubLoc,
                                                              ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).endCyclePubLoc,
                                                              ref tmpCycleLoc,
                                                              ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).stampAppendPubLoc,
                                                              ".jpg");


                                        ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).currentCyclePubLoc = Convert.ToInt32(tmpCycleLoc);
                                        teboDebug.writeline(teboDebug.publishImageVal + 5);
                                        File.Copy(tmpFolder + "pubPicture.jpg", locFile, true);
                                        pubFile = locFile;
                                    }

                                    if (pubToWeb)
                                    {
                                        teboDebug.writeline(teboDebug.publishImageVal + 6);
                                        string webFile = "";
                                        long tmpCycleWeb = new long();
                                        tmpCycleWeb = ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).currentCyclePubWeb;

                                        webFile = FileManager.fileNameSet(ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).filenamePrefixPubWeb,
                                                              ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).cycleStampCheckedPubWeb,
                                                              ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).startCyclePubWeb,
                                                              ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).endCyclePubWeb,
                                                              ref tmpCycleWeb,
                                                              ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).stampAppendPubWeb,
                                                              ".jpg");

                                        ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).currentCyclePubWeb = Convert.ToInt32(tmpCycleWeb);
                                        File.Copy(tmpFolder + "pubPicture.jpg", tmpFolder + webFile, true);
                                        ftp.DeleteFTP(webFile, ConfigurationHelper.GetCurrentProfile().pubFtpRoot, ConfigurationHelper.GetCurrentProfile().pubFtpUser, ConfigurationHelper.GetCurrentProfile().pubFtpPass, false);
                                        ftp.Upload(tmpFolder + webFile, ConfigurationHelper.GetCurrentProfile().pubFtpRoot, ConfigurationHelper.GetCurrentProfile().pubFtpUser, ConfigurationHelper.GetCurrentProfile().pubFtpPass, 0);
                                        pubFile = webFile;
                                    }

                                    teboDebug.writeline(teboDebug.publishImageVal + 7);
                                    File.Delete(tmpFolder + "pubPicture.jpg");
                                    ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).lastPublished = time.secondsSinceStart();
                                    log.AddLine( "Webcam image " + pubFile + " published.");
                                    pulseEvent(null, new EventArgs());
                                }
                                catch (Exception e)
                                {
                                    TebocamState.tebowebException.LogException(e);
                                    teboDebug.writeline(teboDebug.publishImageVal + 8);
                                    ConfigurationHelper.InfoForProfileWebcam(profileInUse, item.cameraName).lastPublished = time.secondsSinceStart();
                                }
                        }
                    }
                }
            }
        }


        public void publishTestMotion(int testInterval, int camNo)
        {

            if (TebocamState.testImagePublishFirst)
            {
                TebocamState.testImagePublishData.Clear();
                testImagePublishCount = 0;
                testImagePublishLast = 0;
            }

            if (TebocamState.testImagePublishFirst || (time.millisecondsSinceStart() - testImagePublishLast) >= testInterval)
            {
                testImagePublishCount++;
                var a = new ImagePub.ImagePubArgs();
                a.option = "tst" + "motionCalibration" + testImagePublishCount.ToString();
                a.CamNo = camNo;


                try
                {

                    take_picture_publish(a);
                    //pubPicture(a);
                    TebocamState.testImagePublishData.Add(testImagePublishCount);

                    int motLevel = Convert.ToInt32((int)Math.Floor(CameraRig.getCam(camNo).MotionDetector.MotionDetectionAlgorithm.MotionLevel * 100));
                    int reportLevel = motLevel >= 0 ? motLevel : 0;

                    TebocamState.testImagePublishData.Add(reportLevel);
                    TebocamState.testImagePublishData.Add(statistics.lowestValTime(camNo, 2000, ConfigurationHelper.GetCurrentProfileName(), time.millisecondsSinceStart()));
                    TebocamState.testImagePublishData.Add(LeftRightMid.Right(a.option + ".jpg", a.option.Length + 1));
                    TebocamState.testImagePublishData.Add(CameraRig.getCam(camNo).name);
                    TebocamState.testImagePublishData.Add(time.millisecondsSinceStart());


                    TebocamState.testImagePublishFirst = false;
                    testImagePublishLast = time.millisecondsSinceStart();

                }
                catch (Exception e)
                {
                    TebocamState.tebowebException.LogException(e);
                    if (TebocamState.testImagePublishData.Count == testImagePublishCount)
                    {

                        TebocamState.testImagePublishData.RemoveAt(TebocamState.testImagePublishData.Count - 1);
                        testImagePublishCount--;

                    }
                }

            }
        }

        //public static void take_picture_publish(object sender, ImagePub.ImagePubArgs e)
        public static void take_picture_publish(ImagePub.ImagePubArgs e)
        {
            haveTheFlag = true;

            string fName = "";
            string stamp = "";
            stamp = e.option;

            bool online = false;
            bool publish = false;
            bool test = false;

            if (stamp.Length >= 3)
            {
                publish = stamp == "pub";
                online = stamp == "onl";
                test = LeftRightMid.Left(stamp, 3) == "tst";
            }

            try
            {

                Bitmap imgBmp = null;
                int compression = 100;

                if (online)
                {
                    fName = $"{ConfigurationHelper.GetCurrentProfile().webImageFileName}{TebocamState.ImgSuffix}";

                    var stampArgs = new Movement.imageText();
                    stampArgs.bitmap = (Bitmap)CameraRig.getCam(e.CamNo).pubFrame.Clone();
                    stampArgs.type = "Online";
                    stampArgs.backingRectangle = ConfigurationHelper.GetCurrentProfile().onlineTimeStampRect;
                    stampArgs.backingRectangle = ConfigurationHelper.GetCurrentProfile().alertTimeStampRect;
                    stampArgs.position = ConfigurationHelper.GetCurrentProfile().onlineTimeStampPosition;
                    stampArgs.format = ConfigurationHelper.GetCurrentProfile().onlineTimeStampFormat;
                    stampArgs.colour = ConfigurationHelper.GetCurrentProfile().onlineTimeStampColour;

                    imgBmp = ConfigurationHelper.GetCurrentProfile().onlineTimeStamp ? ImageProcessor.TimeStampImage(stampArgs) : stampArgs.bitmap;
                    compression = ConfigurationHelper.GetCurrentProfile().onlineCompression;
                }

                if (publish)
                {
                    fName = $"pubPicture{TebocamState.ImgSuffix}";

                    var stampArgs = new Movement.imageText();
                    stampArgs.bitmap = (Bitmap)CameraRig.getCam(e.CamNo).pubFrame.Clone();
                    stampArgs.type = "Publish";
                    stampArgs.backingRectangle = ConfigurationHelper.GetCurrentProfile().publishTimeStampRect;
                    stampArgs.position = ConfigurationHelper.GetCurrentProfile().publishTimeStampPosition;
                    stampArgs.format = ConfigurationHelper.GetCurrentProfile().publishTimeStampFormat;
                    stampArgs.colour = ConfigurationHelper.GetCurrentProfile().publishTimeStampColour;
                    stampArgs.stats = e.lst;

                    imgBmp = ConfigurationHelper.GetCurrentProfile().publishTimeStamp ? ImageProcessor.TimeStampImage(stampArgs) : stampArgs.bitmap;
                    compression = ConfigurationHelper.GetCurrentProfile().publishCompression;
                }

                if (test)
                {
                    fName =  $"{LeftRightMid.Mid(stamp, 3, stamp.Length - 3)}{TebocamState.ImgSuffix}";

                    imgBmp = (Bitmap)CameraRig.getCam(e.CamNo).pubFrame.Clone();
                    compression = ConfigurationHelper.GetCurrentProfile().alertCompression;
                }

                ImageCodecInfo jgpEncoder = ImageProcessor.GetEncoder(ImageFormat.Jpeg);
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, compression);

                myEncoderParameters.Param[0] = myEncoderParameter;
                imgBmp.Save(TebocamState.tmpFolder + fName, jgpEncoder, myEncoderParameters);

                if (!test)
                {
                    Bitmap thumb = ImageProcessor.GetThumb(imgBmp);
                    thumb.Save(TebocamState.tmpFolder + TebocamState.thumbPrefix + fName, ImageFormat.Jpeg);
                    thumb.Dispose();
                }

                imgBmp.Dispose();
                TebocamState.log.AddLine("Image saved: " + fName);
                pubError = false;
                haveTheFlag = false;

            }
            catch (Exception ex)
            {
                TebocamState.tebowebException.LogException(ex);
                haveTheFlag = false;
                pubError = true;
                TebocamState.log.AddLine("Error in saving image: " + fName);
            }
        }
    }
}

