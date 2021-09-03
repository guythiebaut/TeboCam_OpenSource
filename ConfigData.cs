using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TeboCam
{

    [Serializable]
    public class Configuration
    {

        public string version;
        public string profileInUse = "##newProf##";
        public double newsSeq;

        public List<configApplication> appConfigs = new List<configApplication>();

        public Configuration() { }

        public Configuration ReadXmlFile(string filename)
        {
            return Serialization.SerializeFromXmlFile<Configuration>(filename);
        }

        public void WriteXmlFile(string filename, Configuration config)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            Serialization.SerializeToXmlFile(filename, config);
        }

    }


    [Serializable]
    public class configApplication : ICloneable
    {
        [NonSerialized]
        private Interfaces.IEncryption crypt = new crypt();

        public configApplication()
        {
        }

        public configApplication(Interfaces.IEncryption Icrypt)
        {
            crypt = Icrypt;
        }

        public void CryptSet(Interfaces.IEncryption Icrypt)
        {
            crypt = Icrypt;
        }

        public string profileName = "##newProf##";
        public byte[] cryptKey;
        public byte[] cryptVector;
        public int activatecountdown = 15;
        public string activatecountdownTime = "0800";
        public bool AlertOnStartup = false;
        public bool areaDetection = false;
        public bool areaDetectionWithin = false;
        public double baselineVal = Double.Parse("0", new System.Globalization.CultureInfo("en-GB"));
        public bool countdownNow = false;
        public bool countdownTime = false;
        public long currentCycle = 1;
        public bool cycleStamp = false;
        public int cycleStampChecked = 1;
        public long emailNotifyInterval = 2;

        public string _lockdownPassword = string.Empty;
        [XmlIgnore]
        public string lockdownPassword
        {
            get { return crypt.DecryptString(_lockdownPassword); }
            set { _lockdownPassword = crypt.EncryptToString(value); }
        }

        public string _emailPass = string.Empty;
        [XmlIgnore]
        public string emailPass
        {
            get { return crypt.DecryptString(_emailPass); }
            set { _emailPass = crypt.EncryptToString(value); }
        }

        public string _ftpPass = string.Empty;
        [XmlIgnore]
        public string ftpPass
        {
            get { return crypt.DecryptString(_ftpPass); }
            set { _ftpPass = crypt.EncryptToString(value); }
        }

        public string _pubFtpPass = string.Empty;
        [XmlIgnore]
        public string pubFtpPass
        {
            get { return crypt.DecryptString(_pubFtpPass); }
            set { _pubFtpPass = crypt.EncryptToString(value); }
        }

        public string _webPass = string.Empty;
        [XmlIgnore]
        public string webPass
        {
            get { return crypt.DecryptString(_webPass); }
            set { _webPass = crypt.EncryptToString(value); }
        }

        public string _webFtpPass = string.Empty;
        [XmlIgnore]
        public string webFtpPass
        {
            get { return crypt.DecryptString(_webFtpPass); }
            set { _webFtpPass = crypt.EncryptToString(value); }
        }

        public bool lockdownOn = false;
        public string emailUser = "anyone@googlemail.com";
        public bool EnableSsl = true;
        public long endCycle = 999;
        public string filenamePrefix = "webcamImage";
        public string ftpRoot = "ftp.anyone.com/docs/webcam";
        public string ftpUser = "anyone.com";
        public double imageSaveInterval = Double.Parse("0.5", new System.Globalization.CultureInfo("en-GB"));
        public bool loadImagesToFtp = false;
        public string mailBody = "Movement detected - image(s) attached";
        public string mailSubject = "Webcam Warning From TeboCam";
        public long maxImagesToEmail = 10;
        //public double movementVal = Double.Parse("0.3", new System.Globalization.CultureInfo("en-GB"));
        //public int timeSpike = 100;
        //public int toleranceSpike = 50;
        public bool ping = false;
        public bool pingAll = true;
        public int pingInterval = 120;
        public string pingSubject = "WebCamPing";
        public int rectHeight = 80;
        public int rectWidth = 80;
        public int rectX = 20;
        public int rectY = 20;
        public string replyTo = "anyone@googlemail.com";
        public bool sendFullSizeImages = false;
        public bool captureMovementImages = true;
        public bool sendNotifyEmail = false;
        public string sendTo = "anyone@yahoo.com";
        public bool sendThumbnailImages = false;
        public bool sendMosaicImages = false;
        public int mosaicImagesPerRow = 10;
        public string sentBy = "anyone@googlemail.com";
        public string sentByName = "Webcam Warning";
        public string smtpHost = "smtp.googlemail.com";
        public int smtpPort = 25;
        public long startCycle = 1;
        public bool updatesNotify = true;
        public string webcam = string.Empty;
        public bool pubImage = false;
        public int pubTime = 2;
        public bool pubHours = false;
        public bool pubMins = true;
        public bool pubSecs = false;
        public string pubFtpUser = "anyone@googlemail.com";
        public string pubFtpRoot = "ftp.anyone.com/docs/webcam";
        public bool pubStampDate = false;
        public bool pubStampTime = false;
        public bool pubStampDateTime = false;
        public bool pubStamp = false;
        public bool timerOn = false;
        public bool timerOnMov = false;
        public bool scheduleOnAtStart = false;
        public bool activateAtEveryStartup = false;
        public string timerStartPub = "0500";
        public string timerEndPub = "2130";
        public string timerStartMov = "0500";
        public string timerEndMov = "2130";
        public bool webUpd = false;
        public string AuthenticateEndpoint = string.Empty;
        public string LocalAuthenticateEndpoint = string.Empty;
        public string PickupDirectory = string.Empty;
        public bool UseRemoteEndpoint = true;
        public string webUser = string.Empty;
        public int webPoll = 30;
        public string webInstance = "main";
        public string webFtpUser = string.Empty;
        public string webImageRoot = string.Empty;
        public string webImageFileName = "webImg";
        public string soundAlert = string.Empty;
        public bool soundAlertOn = false;
        public int logsKeep = 30;
        public bool logsKeepChk = false;
        public bool imageLocCust = false;
        public string imageParentFolderCust = string.Empty;
        public string imageFolderCust = string.Empty;
        public string thumbFolderCust = string.Empty;
        public bool areaOffAtMotion = false;
        public bool startTeboCamMinimized = false;
        public bool hideWhenMinimized = true;
        public string internetCheck = "www.google.com";
        public bool toolTips = true;
        public int alertCompression = 100;
        public int publishCompression = 100;
        public int pingCompression = 100;
        public bool alertTimeStamp = false;
        public int onlineCompression = 100;
        public string alertTimeStampFormat = "ddmmyy";
        public bool alertStatsStamp = false;
        public string alertTimeStampColour = "red";
        public string alertTimeStampPosition = "t1";
        public bool alertTimeStampRect = false;
        public bool publishTimeStamp = false;
        public string publishTimeStampFormat = "ddmmyy";
        public bool publishStatsStamp = false;
        public string publishTimeStampColour = "red";
        public string publishTimeStampPosition = "t1";
        public bool publishTimeStampRect = false;
        public bool pingTimeStamp = false;
        public string pingTimeStampFormat = "ddmmyy";
        public bool pingStatsStamp = false;
        public string pingTimeStampColour = "red";
        public string pingTimeStampPosition = "t1";
        public bool pingTimeStampRect = false;
        public bool onlineTimeStamp = false;
        public string onlineTimeStampFormat = "ddmmyy";
        public bool onlineStatsStamp = false;
        public string onlineTimeStampColour = "red";
        public string onlineTimeStampPosition = "t1";
        public bool onlineTimeStampRect = false;
        public bool publishLocal = false;
        public bool publishWeb = true;
        public bool imageToframe = true;
        public string profileVersion = "0";
        public bool cameraShow = true;
        public bool motionLevel = true;
        public bool freezeGuard = true;
        public bool freezeGuardWindowShow = false;
        public string selectedCam = string.Empty;
        public string filenamePrefixPubWeb = "webcamPublish";
        public int cycleStampCheckedPubWeb = 1;
        public long startCyclePubWeb = 1;
        public long endCyclePubWeb = 999;
        public long currentCyclePubWeb = 1;
        public bool stampAppendPubWeb = false;
        public string filenamePrefixPubLoc = "webcamPublish";
        public int cycleStampCheckedPubLoc = 1;
        public long startCyclePubLoc = 1;
        public long endCyclePubLoc = 999;
        public long currentCyclePubLoc = 1;
        public bool stampAppendPubLoc = false;
        public decimal pulseFreq = 0.5m;
        public bool EmailIntelOn = false;
        public int emailIntelEmails = 2;
        public int emailIntelMins = 1;
        public bool EmailIntelStop = false;
        public bool disCommOnline = false;
        public int disCommOnlineSecs = 3600;
        public string ipWebcamAddress = string.Empty;
        public string ipWebcamUser = string.Empty;
        public string ipWebcamPassword = string.Empty;
        public bool StatsToFileOn = false;
        public string StatsToFileLocation = string.Empty;
        public bool StatsToFileTimeStamp = false;
        public double StatsToFileMb = 10;
        public bool framerateTrack = true;
        public int framesSecsToCalcOver = 60;

        public List<configWebcam> camConfigs = new List<configWebcam>();

        public object Clone()
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, this);
            ms.Position = 0;
            object obj = bf.Deserialize(ms);
            ms.Close();
            return obj;
        }
    }

    [Serializable]
    public class configWebcam
    {
        public configWebcam() { }

        [NonSerialized]
        private Interfaces.IEncryption crypt = new crypt();

        public string profileName = "";
        public string webcam = "";

        public string friendlyName = string.Empty;
        public bool areaDetection = false;
        public bool areaDetectionWithin = false;
        public bool areaOffAtMotion = false;
        public bool alarmActive = false;

        //public bool alarmActive
        //{
        //    get { return true; }
        //    set { alarmActive = value; }
        //}

        public bool publishActive = false;
        public int rectX = 20;
        public int rectY = 20;
        public int rectWidth = 80;
        public int rectHeight = 80;
        public int displayButton = 1;
        public double movementVal = 0.99;
        public int timeSpike = 100;
        public int toleranceSpike = 50;
        public bool lightSpike = false;


        public bool pubImage = false;
        public int pubTime = 2;
        public bool pubHours = false;
        public bool pubMins = true;
        public bool pubSecs = false;
        public bool publishWeb = false;
        public bool publishLocal = false;
        public bool timerOn = false;

        public string fileURLPubWeb = "";
        public string filenamePrefixPubWeb = "webcamPublish";
        public int cycleStampCheckedPubWeb = 1;
        public int startCyclePubWeb = 1;
        public int endCyclePubWeb = 999;
        public int currentCyclePubWeb = 1;
        public bool stampAppendPubWeb = false;

        public string fileDirAlertLoc = TebocamState.imageFolder;
        public bool fileDirAlertCust = false;
        public string fileDirPubLoc = TebocamState.imageFolder;
        public bool fileDirPubCust = false;
        public string filenamePrefixPubLoc = "webcamPublish";
        public int cycleStampCheckedPubLoc = 1;
        public int startCyclePubLoc = 1;
        public int endCyclePubLoc = 999;
        public int currentCyclePubLoc = 1;
        public bool stampAppendPubLoc = false;

        public string ipWebcamAddress = "";
        public string ipWebcamUser = "";

        public string _ipWebcamPassword = string.Empty;
        [XmlIgnore]
        public string ipWebcamPassword
        {
            get { return crypt.DecryptString(_ipWebcamPassword); }
            set { _ipWebcamPassword = crypt.EncryptToString(value); }
        }

        //for monitoring publishing - does not need to be saved to xml file
        public bool publishFirst = true;
        public int lastPublished = 0;
        //for monitoring publishing - does not need to be saved to xml file        


    }

}
