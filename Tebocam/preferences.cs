using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using teboweb;
//using SharpAvi;
using System.Threading;
using System.IO;
using System.Diagnostics;
using TeboWeb;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using AForge.Video.DirectShow;
using System.Threading.Tasks;
//using SharpAvi.Codecs;
//using SharpAvi.Output;

enum enumCommandLine
{
    profile = 0,
    alert = 1,
    restart = 2,
    close = 3,
    none = 9
};

namespace TeboCam
{
    public delegate void formDelegate(ArrayList i);
    public delegate void formDelegateList(List<List<object>> i);
    public delegate void FilePrefixformDelegate(FilePrefixSettingsResultDto i);


    public partial class Preferences : Form
    {
        //http://msdn.microsoft.com/en-us/library/aa984408(v=vs.71).aspx
        System.Resources.ResourceManager resourceManager;

        public List<GroupCameraButton> NotConnectedCameras = new List<GroupCameraButton>();
        public List<GroupCameraButton> PublishButtonGroupInstance = new List<GroupCameraButton>();
        public CameraButtonsCntl ButtonCameraControl = new CameraButtonsCntl();
        public CameraButtonsCntl ButtonPublishControl = new CameraButtonsCntl();

        private Queue CommandQueue = new Queue();

        public CameraAlarm cameraAlarm;
        public Ping pinger;
        public Publisher publisher;
        public Pulse pulse;
        webcamConfig webcamConfig;

        EmailHostSettingsCntl emailHostSettings;
        FtpSettingsCntl ftpSettings;
        EmailSettingsCntl emailSettings;
        AlertTimeSettingsCntl alertTimeSettings;
        NotificationSettingsCntl notificationSettings;
        ProfilesCntl profilesSettings;
        FreezeGuardCntl freezeGuardSettings;
        EmailIntelligenceCntl emailIntelligenceSettings;
        MovementStatisticsCntl movementStatisticsSettings;
        PublishSettingsCntl publishSettings;
        AlertFilenameCntl alertFilenameSettings;
        ImagesSavedFolderCntl imagesSavedFolderSettings;
        SecurityLockdownCntl securityLockdownSettings;
        GenerateWebpageCntl generateWebpageSettings;
        UpdateOptionsCntl updateOptionsSettings;
        LogFileManagementCntl logFileManagementSettings;
        FrameRateCntl frameRateSettings;
        InternetConnectionCheckCntl internetConnectionCheckSettings;
        MiscCntl miscSettings;
        LogCntl logControl;
        MotionAlarmCntl motionAlarmSettings;
        internal OnlineCntl onlineSettings;

        WaitForCam waitForCamera;
        OpenVideo openVideo;

        private bool connectedToInternet = false;

        public static event EventHandler pulseEvent;
        public static event EventHandler pulseStopEvent;
        public static event EventHandler pulseStartEvent;
        public static event EventHandler TimeChange;
        public static event ImagePub.ImagePubEventHandler pubPicture;

        public static event EventHandler publishSwitch;

        public delegate void ListPubEventHandler(object source, ListArgs e);
        public static event ListPubEventHandler statusUpdate;

        public Point CurrentTopLeft = new Point();
        public Point CurrentBottomRight = new Point();
        public int RectangleHeight = new int();
        public int RectangleWidth = new int();


        static BackgroundWorker bw = new BackgroundWorker();
        static BackgroundWorker cw = new BackgroundWorker();
        static BackgroundWorker worker = new BackgroundWorker();

        private bool showLevel = false;

        private int secondsToTrainStart;

        private const int statLength = 15;
        private int statIndex = 0;
        private int[] statCount = new int[statLength];

        private int intervalsToSave = 0;

        //private AviWriter writer = null;
        private bool saveOnMotion = false;

        public bool checkForMotion = false;

        //public int frameCount;
        public int framePrevious = 0;

        private LevelControl LevelControlBox = new LevelControl();

        //private FilterInfoCollection filters;

        public Graph graph = new Graph();
        private Log log = new Log();
        //public IException tebowebException;
        private Configuration configuration = new Configuration();
        private bool Loading;
        private bool webcamAttached = false;
        private bool publishFirst = true;
        public const string tebowebUrl = sensitiveInfo.tebowebUrl;
        public const string versionDt = sensitiveInfo.versionDt;
        public static string version = Double.Parse(sensitiveInfo.ver, new System.Globalization.CultureInfo("en-GB")).ToString();
        public static bool devMachine = false;
        public static string postProcessCommand = "";
        public static bool updaterInstall = false;
        public static string processToEnd = sensitiveInfo.processToEnd;
        public static string postProcess = Application.StartupPath + @"\" + processToEnd + ".exe";
        public static string updater = Application.StartupPath + @"\update.exe";
        public static bool countingdownstop = false;
        public static bool countingdown = false;
        public static double newsSeq = 0;
        private IMail email = new mailOLD();
        public static bool keepWorking;
        public static bool lockdown = false;
        public static string devMachineFile = sensitiveInfo.devMachineFile;
        public static string databaseTrialFile = sensitiveInfo.databaseTrialFile;
        public static string ApiConnectFile = sensitiveInfo.dbaseConnectFile;
        public static string lastTime = "00:00";
        public static string upd_url = "";
        public static string upd_file = "";
        public static bool pulseRestart = false;

        [STAThread]

        static void Main()
        {
            Application.Run(new Preferences());
        }

        public Preferences()
        {
            InitializeComponent();
            resourceManager = new System.Resources.ResourceManager("tebocam.Preferences", this.GetType().Assembly);
        }

        private void testAtStart()
        {
            ConfigurationHelper.AddProfile();
            configApplication data = ConfigurationHelper.getProfile("main");
        }


        private void workerProcess(object sender, DoWorkEventArgs e)
        {
            pulseEvent -= new EventHandler(pulseProcess);
            pulseEvent += new EventHandler(pulseProcess);
            PulseEvents.pulseEvent -= new EventHandler(pulseProcess);
            PulseEvents.pulseEvent += new EventHandler(pulseProcess);
            Movement.pulseEvent -= new EventHandler(pulseProcess);
            Movement.pulseEvent += new EventHandler(pulseProcess);

            pulseStopEvent -= new EventHandler(pulseStop);
            pulseStopEvent += new EventHandler(pulseStop);
            pulseStartEvent -= new EventHandler(pulseStart);
            pulseStartEvent += new EventHandler(pulseStart);

            pulseEvent(null, new EventArgs());
            ApiProcess.mail = email;


            teboDebug.filePath = TebocamState.logFolder;
            teboDebug.fileName = "debug.txt";//string.Format("debug_{0}.txt", DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture));

            teboDebug.openFile();

            teboDebug.writeline("workerProcess starting");

            long keepWorkingSequence = 0;

            //publisher = new Publisher(graph, email,
            //           TebocamState.tmpFolder, TebocamState.thumbPrefix, TebocamState.thumbFolder,
            //           TebocamState.imageFolder, TebocamState.xmlFolder, TebocamState.mosaicFile,
            //           ConfigurationHelper.GetCurrentProfileName(), configuration, log,
            //          Movement.moveStats, ImagePub.PubPicture);

            //publisher.redrawGraph += new EventHandler(drawGraph);
            //publisher.pulseEvent += new EventHandler(pulseProcess);

            pinger = new Ping(email, log, graph, TebocamState.tmpFolder,
                              TebocamState.xmlFolder, ConfigurationHelper.GetCurrentProfileName(), drawGraphPing);
            pinger.pulseEvent += new EventHandler(pulseProcess);
            pinger.redrawGraph += new EventHandler(drawGraph);
            cameraWindow.ping = pinger;

            while (keepWorking)
            {
                try
                {
                    if (keepWorkingSequence > 1000) keepWorkingSequence = 0;
                    keepWorkingSequence++;

                    pulseEvent(null, new EventArgs());
                    changeTheTime();

                    teboDebug.writeline("workerProcess calling CheckAndRunScheduledOperations");
                    CheckAndRunScheduledOperations();
                    teboDebug.writeline("workerProcess calling movementAddImages");
                    Movement.movementAddImages();
                    teboDebug.writeline("workerProcess calling publishImage");
                    publisher.publishImage();
                    teboDebug.writeline("workerProcess calling webUpdate");
                    ApiProcess.webUpdate(this);
                    teboDebug.writeline("workerProcess calling movementPublish");
                    publisher.movementPublish();

                    //we only really want to do the following every ten passes through this loop
                    if (keepWorkingSequence % 5 == 0) teboDebug.writeline("workerProcess calling ping");
                    if (keepWorkingSequence % 5 == 0) pinger.Send(webcamAttached, new Size(pictureBox1.ClientRectangle.Width, pictureBox1.ClientRectangle.Height));
                    if (keepWorkingSequence % 10 == 0)
                        teboDebug.writeline("workerProcess calling connectCamerasMissingAtStartup()");
                    if (keepWorkingSequence % 10 == 0) connectCamerasMissingAtStartup();
                    if (keepWorkingSequence % 10 == 0) teboDebug.writeline("workerProcess calling frameRate");
                    if (keepWorkingSequence % 10 == 0) frameRate();
                    if (keepWorkingSequence % 10 == 0) teboDebug.writeline("workerProcess calling cameraReconnectIfLost()");
                    if (keepWorkingSequence % 10 == 0) reconnectLostCameras();

                    teboDebug.writeline("workerProcess sleeping");
                    Thread.Sleep(250);
                }
                catch (Exception ex)
                {
                    TebocamState.tebowebException.LogException(ex);
                }
            }

            e.Cancel = true;
        }

        public static void changeTheTime()
        {
            string tmpStr = DateTime.Now.ToString("HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            if (tmpStr != lastTime)
            {
                lastTime = tmpStr;
                TimeChange(null, new EventArgs());
            }
        }

        private void ButtonCameraDelegation(int id, Button cameraButton, Button activeButton, bool activate = false)
        {
            cameraSwitch(id, true, false);
        }
        private void ButtonPublishDelegation(int id, Button cameraButton, Button activeButton, bool activate = false)
        {
            pubcam(id);
        }

        private void ButtonActiveDelegation(int id, Button cameraButton, Button activeButton, bool activate)
        {
            selcam(id, activate);
        }

        private void filesInit()
        {
            if (!File.Exists(TebocamState.xmlFolder + "GraphData.xml"))
            {
                //FileManager.WriteFile("graphInit"); #FIX
                //FileManager.backupFile("graph");#FIX
                new Graph().WriteXMLFile(TebocamState.xmlFolder + "GraphData.xml", graph);
                new Graph().WriteXMLFile(TebocamState.xmlFolder + "GraphData.bak", graph);
            }

            if (!File.Exists(TebocamState.xmlFolder + "LogData" + ".xml"))
            {
                //FileManager.WriteFile("logInit");
                //FileManager.backupFile("log");

                if (log.Lines.Count == 0)
                {
                    log.AddLine("Initialising");
                }

                new Log().WriteXMLFile(TebocamState.xmlFolder + "LogData" + ".xml", log);
                new Log().WriteXMLFile(TebocamState.xmlFolder + "LogData" + ".bak", log);
            }

            //if the old style config file exists read it otherwise deserialise file into class and delete the old config file
            FileManager.ConvertOldProfileIfExists(configuration);

            if (!File.Exists(TebocamState.xmlFolder + FileManager.configFile + ".xml"))
            {
                Configuration config = new Configuration();
                configApplication configApp = new configApplication();
                config.appConfigs.Add(configApp);
                config.WriteXmlFile(TebocamState.xmlFolder + FileManager.configFile + ".xml", configuration);
                config.WriteXmlFile(TebocamState.xmlFolder + FileManager.configFile + ".bak", configuration);
            }

        }

        private enumCommandLine commandLine()
        {

            //Example command line parameters
            // /alert now
            // /alert time 1935
            // /alert seconds 180
            // /profile daytime_monitor  
            //Example command line parameters

            enumCommandLine result = enumCommandLine.none;
            string commandLine = "";
            bool activate = false;
            bool restart = false;
            bool profile = false;
            bool time = false;
            bool seconds = false;
            bool acceptString = false;

            foreach (string arg in Environment.GetCommandLineArgs())
            {
                if (LeftRightMid.Left(arg, 1) == "/") { acceptString = true; }

                if (acceptString)
                {
                    commandLine = arg.ToLower().Trim();
                    //profile must not contain any spaces within it
                    //second time through pick up the profile to use
                    if (profile && ConfigurationHelper.profileExists(commandLine))
                    {
                        ConfigurationHelper.SetCurrentProfileName(commandLine);
                        ConfigurationHelper.LoadCurrentProfile(commandLine);
                    }
                    //second time through pick up the profile to use
                    //profile must not contain any spaces within it

                    if (commandLine == "/profile")
                    {
                        profile = true;
                        result = enumCommandLine.profile;
                    }
                    if (commandLine == "/alert")
                    {
                        activate = true;
                        result = enumCommandLine.alert;
                    }
                    if (commandLine == "/restart")
                    {
                        result = enumCommandLine.restart;
                        restart = true;
                        pulseRestart = true;
                    }
                    if (commandLine == "/close")
                    {
                        result = enumCommandLine.close;
                        return result;
                    }

                    if (activate || profile)
                    {
                        ConfigurationHelper.GetCurrentProfile().countdownNow = false;
                        ConfigurationHelper.GetCurrentProfile().countdownTime = false;
                    }

                    if (activate && commandLine == "time") { time = true; }
                    if (activate && commandLine == "seconds") { seconds = true; }
                    if (activate && commandLine == "now") { ConfigurationHelper.GetCurrentProfile().countdownNow = true; }

                    if (time && commandLine != "time")
                    {
                        ConfigurationHelper.GetCurrentProfile().activatecountdownTime = commandLine;
                        ConfigurationHelper.GetCurrentProfile().countdownTime = true;
                        motionAlarmSettings.GetNumericUpDown1().Value = Convert.ToDecimal(LeftRightMid.Left(commandLine, 2));
                        motionAlarmSettings.GetNumericUpDown2().Value = Convert.ToDecimal(LeftRightMid.Right(commandLine, 2));
                        motionAlarmSettings.GetBttnNow().Checked = false;
                        motionAlarmSettings.GetBttnTime().Checked = true;
                        motionAlarmSettings.GetBttnSeconds().Checked = false;
                    }

                    if (seconds && commandLine != "seconds")
                    {
                        ConfigurationHelper.GetCurrentProfile().activatecountdown = Convert.ToInt32(commandLine);
                        motionAlarmSettings.GetActCountdown().Text = commandLine;
                        motionAlarmSettings.GetBttnNow().Checked = false;
                        motionAlarmSettings.GetBttnTime().Checked = false;
                        motionAlarmSettings.GetBttnSeconds().Checked = true;
                    }

                    if (restart && commandLine == "active")
                    {
                        ConfigurationHelper.GetCurrentProfile().activatecountdown = 30;
                        motionAlarmSettings.GetActCountdown().Text = "30";
                        motionAlarmSettings.GetBttnNow().Checked = false;
                        motionAlarmSettings.GetBttnTime().Checked = false;
                        motionAlarmSettings.GetBttnSeconds().Checked = true;
                        activate = true;
                    }

                    if ((activate && (commandLine == "now" || commandLine == "activate")))
                    {
                        motionAlarmSettings.GetBttnNow().Checked = true;
                        motionAlarmSettings.GetBttnTime().Checked = false;
                        motionAlarmSettings.GetBttnSeconds().Checked = false;
                    }
                }
            }

            if (activate)
            {
                ConfigurationHelper.GetCurrentProfile().AlertOnStartup = true;
                motionAlarmSettings.GetBttnMotionActive().Checked = true;

                Queue.QueueItem item = new Queue.QueueItem
                {
                    Instruction = "selcam",
                    Parms = new List<string> { "all" }
                };
                CommandQueue.QueueItems.Add(item);
            }

            return result;
        }

        private void SetUpPublisher()
        {
            publisher = new Publisher(ref graph, email,
            TebocamState.tmpFolder, TebocamState.thumbPrefix, TebocamState.thumbFolder,
            TebocamState.imageFolder, TebocamState.xmlFolder, TebocamState.mosaicFile,
            ConfigurationHelper.GetCurrentProfileName(), configuration, log,
            Movement.moveStats);
            publisher.redrawGraph += new EventHandler(drawGraph);
            publisher.pulseEvent += new EventHandler(pulseProcess);
        }

        private void preferences_Load(object sender, EventArgs e)
        {
            string exceptionFileSuffix = DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            TebocamState.tebowebException = new TebowebException(TebocamState.exceptionFolder, $"TebowebException_{exceptionFileSuffix}.txt");
            installationClean();
            filesInit();
            StartPosition = FormStartPosition.Manual;
            //installationClean();
            LevelControlBox.Left = 6;
            LevelControlBox.Top = 35;
            this.Webcam.Controls.Add(LevelControlBox);
            CameraRig.camSelInit();
            var publishCams = new Movement.publishCams(9);
            devMachine = File.Exists(Application.StartupPath + devMachineFile);
            ApiProcess.LicensedToConnectToApi = File.Exists(Application.StartupPath + ApiConnectFile);

            if (!ApiProcess.LicensedToConnectToApi) tabControl1.TabPages.Remove(Online); ;

            var updaterPrefix = sensitiveInfo.updaterPrefix;
            update.updateMe(updaterPrefix, Application.StartupPath + @"\");
            ThumbsPrepare();

            try
            {
                log = new Log().ReadXMLFile(TebocamState.xmlFolder + "LogData" + ".xml");
                log.WriteXMLFile(TebocamState.xmlFolder + "LogData" + ".bak", log);
            }
            catch (Exception ex)
            {
                TebocamState.tebowebException.LogException(ex);
                log = new Log().ReadXMLFile(TebocamState.xmlFolder + "LogData" + ".bak");
            }

            log.LogAdded += new EventHandler(log_add);
            TebocamState.log = log;
            Serialization.tebowebException = TebocamState.tebowebException;
            //tebowebException = TebocamState.tebowebException;
            statistics.tebowebException = TebocamState.tebowebException;
            //cameraWindow.TebocamState.tebowebException = TebocamState.tebowebException;
            update.tebowebException = TebocamState.tebowebException;
            webdata.tebowebException = TebocamState.tebowebException;
            FileManager.tebowebException = TebocamState.tebowebException;
            LevelControlBox.tebowebException = TebocamState.tebowebException;
            ImageProcessor.tebowebException = TebocamState.tebowebException;
            email.SetExceptionHandler(TebocamState.tebowebException);
            LeftRightMid.tebowebException = TebocamState.tebowebException;
            ftp.tebowebException = TebocamState.tebowebException;
            ftp.log = TebocamState.log;
            graph.tebowebException = TebocamState.tebowebException;
            ImageThumbs.tebowebException = TebocamState.tebowebException;
            statusUpdate += new ListPubEventHandler(statusBarUpdate);
            TimeChange += new EventHandler(time_change);
            publishSwitch += new EventHandler(publish_switch);
            Movement.motionLevelChanged += new EventHandler(drawLevel);
            Movement.motionDetectionActivate += new EventHandler(motionDetectionActivate);
            Movement.motionDetectionInactivate += new EventHandler(motionDetectionInactivate);

            try
            {
                configuration = new Configuration().ReadXmlFile(TebocamState.xmlFolder + FileManager.configFile + ".xml");
                configuration.WriteXmlFile(TebocamState.xmlFolder + FileManager.configFile + ".bak", configuration);
            }
            catch (Exception ex)
            {
                TebocamState.tebowebException.LogException(ex);
                configuration = new Configuration().ReadXmlFile(TebocamState.xmlFolder + FileManager.configFile + ".bak");
            }

            if (configuration.appConfigs.Count == 0)
            {
                configApplication config = new configApplication();
                configuration.appConfigs.Add(config);
            }

            TebocamState.configuration = configuration;
            ConfigurationHelper.LoadProfiles(TebocamState.configuration.appConfigs);

            cameraAlarm = new CameraAlarm(log, TebocamState.tebowebException,
                              TebocamState.thumbPrefix,
                              TebocamState.thumbFolder, Movement.moveStats);

            openVideo = new OpenVideo(cameraAlarm,
                                      configuration,
                                      NotConnectedCameras,
                                      CommandQueue,
                                      camButtonSetColours,
                                      cameraSwitch,
                                      SetWebCamAttached,
                                      SetbtnConfigWebcam,
                                      pubcam);


            waitForCamera = new WaitForCam(configuration, openVideo.OpenVideoSource, pubcam);
            CameraRig.profiles = configuration.appConfigs;
            // Todo crashes here on fresh startup with null profile name
            ConfigurationHelper.SetCurrentProfileName(configuration.profileInUse);
            ConfigurationHelper.LoadCurrentProfile(configuration.profileInUse);
            newsSeq = configuration.newsSeq;
            PopulateTabsWithUserControls();
            profilesSettings.ProfileListRefresh(ConfigurationHelper.GetCurrentProfileName());
            profilesSettings.ProfileListSelectFirst();
            connectedToInternet = Internet.internetConnected(ConfigurationHelper.GetCurrentProfile().internetCheck);
            notConnected.Visible = !connectedToInternet;
            updateOptionsSettings.GetBttInstallUpdateAdmin().Visible = false;
            bttnUpdateFooter.Visible = false;
            Loading = true;

            if (devMachine)
            {
                updateOptionsSettings.GetBttInstallUpdateAdmin().Visible = true;
                bttnUpdateFooter.Visible = true;
            }
            else
            {
                tabControl1.TabPages.Remove(Test);
            }

            try
            {
                graph = new Graph().ReadXMLFile(TebocamState.xmlFolder + "GraphData.xml");
            }
            catch (Exception ex)
            {
                try
                {
                    TebocamState.tebowebException.LogException(ex);
                    graph = new Graph().ReadXMLFile(TebocamState.xmlFolder + "GraphData.bak");
                }
                catch (Exception ex2)
                {
                    TebocamState.tebowebException.LogException(ex2);
                    new Graph().WriteXMLFile(TebocamState.xmlFolder + "GraphData.xml", graph);
                    new Graph().WriteXMLFile(TebocamState.xmlFolder + "GraphData.bak", graph);
                }
            }

            SetUpPublisher();
            graph.WriteXMLFile(TebocamState.xmlFolder + "GraphData.bak", graph);

            //allows testing of movement detection
            //AdminControl();

            //clear out thumb nail images
            FileManager.clearFiles(TebocamState.thumbFolder);
            LevelControlBox.levelDraw(0);
            Movement.moveStatsInitialise();
            graph.updateGraphHist(time.currentDateYYYYMMDD(), Movement.moveStats);

            if (!ConfigurationHelper.GetCurrentProfile().captureMovementImages)
            {
                graphDate(DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture), "Image capture switched off");
            }
            else
            {
                DrawGraph();
            }

            //Apply command line values
            enumCommandLine commlineResults = commandLine();
            pnlStartupOptions.Visible = commlineResults <= enumCommandLine.alert;

            if (commlineResults == enumCommandLine.close)
            {
                //20190623 commented out as this seems to crash tebocam on exit
                SetAPiInstanceToOff();
                CloseAllTeboCamPocesses();
                return;
            }

            TebocamState.log.AddLine("Starting TeboCam");
            FileManager.clearLog();

            if (ConfigurationHelper.GetCurrentProfile().webcam != null)
            {
                cw.DoWork -= new DoWorkEventHandler(waitForCamera.wait);
                cw.DoWork += new DoWorkEventHandler(waitForCamera.wait);
                cw.WorkerSupportsCancellation = true;
                cw.RunWorkerAsync();
            }

            string test = time.currentTime();
            Loading = false;
            updateOptionsSettings.GetLblCurVer().Text += version;
            string onlineVersion = version;

            ///#if !DEBUG
            updates(ref onlineVersion);
            //#endif

            if (ConfigurationHelper.GetCurrentProfile().logsKeepChk) clearOldLogs();

            if (!ConfigurationHelper.GetCurrentProfile().AlertOnStartup && ConfigurationHelper.GetCurrentProfile().updatesNotify
                && connectedToInternet
                && Convert.ToDecimal(onlineVersion) > Convert.ToDecimal(version)
                && !ConfigurationHelper.GetCurrentProfile().startTeboCamMinimized)
            {
                string tmpStr = "";

                //Name Spaces Required
                ////http://msdn.microsoft.com/en-us/library/aa984408(v=vs.71).aspx
                //System.Resources.ResourceManager resourceManager = new System.Resources.ResourceManager("tebocam.Preferences", this.GetType().Assembly);
                //tmpStr = resourceManager.GetString("updateAvailableMessage");

                //You do not have the most recent version available.

                //The most recent version can installed automatically
                //by clicking on the update button at the bottom of the screen 
                //or on the Admin tab.

                //To stop this message appearing in future uncheck the 
                //'Notify when updates are available' box in the Admin tab.
                tmpStr = "You do not have the most recent version available" + Environment.NewLine + Environment.NewLine;
                tmpStr += $"This version: {version}" + Environment.NewLine;
                tmpStr += $"Most recent version available: {onlineVersion}" + Environment.NewLine + Environment.NewLine;
                tmpStr += "The most recent version can be installed automatically" + Environment.NewLine;
                tmpStr += "by clicking on the update button at the bottom of the screen or on the Admin tab" + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                tmpStr += "To stop this message appearing in future - uncheck the" + Environment.NewLine;
                tmpStr += "'Notify when updates are available' box in the Admin tab.";
                MessageBox.Show(tmpStr, "Update Available");
            }

            tabControl1.TabPages[0].Controls.Add(ButtonCameraControl);
            //this.Webcam.Controls.Add(ButtonCameraControl);
            ButtonCameraControl.Location = new Point(btnMonitor.Right + 2, btnMonitor.Top);
            ButtonCameraControl.BringToFront();
            publishSettings.GetGroupBox17().Controls.Add(ButtonPublishControl);
            ButtonPublishControl.Location = new Point(2, 10);
            ButtonPublishControl.BringToFront();
            this.Webcam.Invalidate();


            for (int i = 0; i < 9; i++)
            {
                ButtonCameraControl.AddButton(NotConnectedCameras, ButtonCameraDelegation, ButtonActiveDelegation, true, new Size(20, 20), new Size(20, 10));
                ButtonPublishControl.AddButton(PublishButtonGroupInstance, ButtonPublishDelegation, ButtonActiveDelegation, false, new Size(20, 20), new Size(20, 10));
            }

            notificationSettings.SetPlSnd(ConfigurationHelper.GetCurrentProfile().soundAlert != "");

            if (ConfigurationHelper.GetCurrentProfile().freezeGuard)
            {
                decimal intervalRatio = 1m / 0.75m;//gives a result of 1.33333...
                string restartCommand = motionAlarmSettings.GetBttnMotionActive().Checked ? "restart active" : "restart inactive";
                decimal checkInterval = ConfigurationHelper.GetCurrentProfile().pulseFreq / intervalRatio;

                pulse = new Pulse(ConfigurationHelper.GetCurrentProfile().pulseFreq,//1m,
                                  checkInterval,// 0.75m,
                                  TebocamState.tmpFolder,
                                  "pulse.xml",
                                  processToEnd,
                                  Application.StartupPath + @"\TeboCam.exe",
                                  TebocamState.pulseApp,
                                  TebocamState.logFolder,
                                  restartCommand,
                                  pulseRestart);

                pulse.tebowebException = TebocamState.tebowebException;
            }

            cw = null;
            keepWorking = true;

            worker.WorkerSupportsCancellation = true;
            worker.DoWork -= new DoWorkEventHandler(workerProcess);
            worker.DoWork += new DoWorkEventHandler(workerProcess);
            worker.RunWorkerAsync();

            this.Enabled = false;

            if (lockdown)
            {
                while (1 == 1)
                {
                    if (Prompt.ShowDialog("Password", "Enter password to unlock") == ConfigurationHelper.GetCurrentProfile().lockdownPassword)
                    {
                        this.Enabled = true;
                        break;
                    }
                }
            }

            this.Enabled = true;
        }

        void DrawGraph()
        {
            drawGraph(this, null);
        }

        void SetWebCamAttached(bool val)
        {
            webcamAttached = val;
        }

        void SetbtnConfigWebcam(bool val)
        {
            btnConfigWebcam.SynchronisedInvoke(() => btnConfigWebcam.Enabled = val);
        }

        void PopulateTabsWithUserControls()
        {
            AlertTimeSettings();
            EmailSettings();
            FtpSettings();
            EmailHostSettings();
            NotificationSettings();
            ProfilesSettings();
            FreezeGuardSettings();
            EmailIntelligenceSettings();
            MovementStatisticsSettings();
            PublishSettings();
            AlertFilenameSettings();
            ImagesSavedFolderSettings();
            SecurityLockdownSettings();
            GenerateWebpageSettings();
            UpdateOptionsSettings();
            LogFileManagementSettings();
            FrameRateSettings();
            InternetConnectionCheckSettings();
            MiscSettings();
            OnlineSettings();
            LogControl();
            MotionAlarmSettings();
        }

        void AddControl(Control controlToAdd, Control.ControlCollection addTo, Point position, bool bringToFront = true)
        {
            addTo.Add(controlToAdd);
            controlToAdd.Location = position;

            if (bringToFront)
            {
                controlToAdd.BringToFront();
            }
        }

        void MotionAlarmSettings()
        {
            motionAlarmSettings = new MotionAlarmCntl(selcam,
                                                      SetActCount,
                                                      activeCountdown,
                                                      SetCountingdownstop,
                                                      scheduleSet,
                                                      GetLoading,
                                                      GetCountingdown,
                                                      bw,
                                                      NotConnectedCameras
                                                      );
            AddControl(motionAlarmSettings, Webcam.Controls, new Point(6, 398));
        }

        void LogControl()
        {
            logControl = new LogCntl(log);
            AddControl(logControl, Webcam.Controls, new Point(511, 343));
        }

        void NotificationSettings()
        {
            notificationSettings = new NotificationSettingsCntl(pinger,
                                                                configuration,
                                                                alertTimeSettings,
                                                                graphDate,
                                                                DrawGraph);
            AddControl(notificationSettings, Alerts.Controls, new Point(6, 6));
        }

        void OnlineSettings()
        {
            onlineSettings = new OnlineCntl();
            AddControl(onlineSettings, Online.Controls, new Point(12, 14));
        }

        void MiscSettings()
        {
            miscSettings = new MiscCntl();
            AddControl(miscSettings, Admin.Controls, new Point(505, 454));
        }

        void InternetConnectionCheckSettings()
        {
            internetConnectionCheckSettings = new InternetConnectionCheckCntl();
            AddControl(internetConnectionCheckSettings, Admin.Controls, new Point(16, 252));
        }

        void FrameRateSettings()
        {
            frameRateSettings = new FrameRateCntl();
            AddControl(frameRateSettings, Admin.Controls, new Point(10, 461));
        }

        void LogFileManagementSettings()
        {
            var fileInfo = new FileInfoClass(TebocamState.tebowebException);
            var exceptionId = "1";
            var logId = "2";
            var imageId = "3";
            var thumbId = "4";
            var logFilePattern = "log_*.xml";
            var exceptionFilePattern = "TebowebException_*.txt";
            var imageFilePattern = $"*{TebocamState.ImgSuffix}";
            var thumbFilePattern = $"{TebocamState.thumbPrefix}{imageFilePattern}";

            fileInfo.AddFileType(exceptionId, "Exception files");
            fileInfo.AddFileType(logId, "Log files");
            fileInfo.AddFileType(imageId, "Full size image files");
            fileInfo.AddFileType(thumbId, "Thumbnail image files");

            fileInfo.AddFileDirectory(exceptionId, TebocamState.exceptionFolder);
            fileInfo.AddFileDirectory(logId, TebocamState.logFolder);
            fileInfo.AddFileDirectory(imageId, TebocamState.imageFolder);
            fileInfo.AddFileDirectory(thumbId, TebocamState.thumbFolder);

            fileInfo.AddFileNamePattern(exceptionId, exceptionFilePattern);
            fileInfo.AddFileNamePattern(logId, logFilePattern);
            fileInfo.AddFileNamePattern(imageId, imageFilePattern);
            fileInfo.AddFileNamePattern(thumbId, thumbFilePattern);

            logFileManagementSettings = new LogFileManagementCntl(fileInfo);
            AddControl(logFileManagementSettings, Admin.Controls, new Point(16, 157));
        }

        void UpdateOptionsSettings()
        {
            updateOptionsSettings = new UpdateOptionsCntl(UpdaterInstall,
                                                          KeepWorking,
                                                          SetAPiInstanceToOff);
            AddControl(updateOptionsSettings, Admin.Controls, new Point(10, 318));
        }

        void GenerateWebpageSettings()
        {
            generateWebpageSettings = new GenerateWebpageCntl();
            AddControl(generateWebpageSettings, Admin.Controls, new Point(16, 10));
        }

        void SecurityLockdownSettings()
        {
            securityLockdownSettings = new SecurityLockdownCntl(lockTebocam, LockDown);
            AddControl(securityLockdownSettings, Admin.Controls, new Point(505, 311));
        }

        void ImagesSavedFolderSettings()
        {
            imagesSavedFolderSettings = new ImagesSavedFolderCntl();
            AddControl(imagesSavedFolderSettings, Alerts.Controls, new Point(307, 122));
        }

        void AlertFilenameSettings()
        {
            alertFilenameSettings = new AlertFilenameCntl(filePrefixSet);
            AddControl(alertFilenameSettings, Alerts.Controls, new Point(307, 6));
        }

        void PublishSettings()
        {
            publishSettings = new PublishSettingsCntl(GetPublishButtonGroupInstance,
                                                      filePrefixSet,
                                                      scheduleSet,
                                                      publisher,
                                                      SetPublishFirst);
            AddControl(publishSettings, Publish.Controls, new Point(3, 3));
        }

        public List<GroupCameraButton> GetPublishButtonGroupInstance()
        {
            return PublishButtonGroupInstance;
        }

        void MovementStatisticsSettings()
        {
            movementStatisticsSettings = new MovementStatisticsCntl();
            AddControl(movementStatisticsSettings, Alerts.Controls, new Point(312, 409));
        }

        void EmailIntelligenceSettings()
        {
            emailIntelligenceSettings = new EmailIntelligenceCntl();
            AddControl(emailIntelligenceSettings, Alerts.Controls, new Point(312, 209));
        }

        void FreezeGuardSettings()
        {
            freezeGuardSettings = new FreezeGuardCntl(PulseStop);
            AddControl(freezeGuardSettings, Admin.Controls, new Point(505, 146));
        }

        void ProfilesSettings()
        {
            profilesSettings = new ProfilesCntl(populateFromProfile,
                                                cameraNewProfile,
                                                saveChanges,
                                                configuration);
            AddControl(profilesSettings, Admin.Controls, new Point(505, 10));
        }

        void AlertTimeSettings()
        {
            alertTimeSettings = new AlertTimeSettingsCntl();
            AddControl(alertTimeSettings, Alerts.Controls, new Point(6, 375));
        }

        void EmailSettings()
        {
            emailSettings = new EmailSettingsCntl(email);
            AddControl(emailSettings, Email_Ftp.Controls, new Point(376, 12));
        }

        void FtpSettings()
        {
            ftpSettings = new FtpSettingsCntl(TebocamState.log);
            AddControl(ftpSettings, Email_Ftp.Controls, new Point(23, 302));
        }

        void EmailHostSettings()
        {
            emailHostSettings = new EmailHostSettingsCntl(email);
            AddControl(emailHostSettings, Email_Ftp.Controls, new Point(23, 12));
        }

        void SetActCount(bool val)
        {
            actCount.Visible = val;
        }

        void SetCountingdownstop(bool val)
        {
            countingdownstop = val;
        }

        bool GetLoading()
        {
            return Loading;
        }

        bool GetCountingdown()
        {
            return countingdown;
        }

        void UpdaterInstall(bool val)
        {
            updaterInstall = val;
            if (updaterInstall)
            {
                CloseTebocam();
            }
        }

        void KeepWorking(bool val)
        {
            keepWorking = val;
        }

        void LockDown(bool val)
        {
            lockdown = val;
        }

        void SetPublishFirst(bool val)
        {
            publishFirst = val;
        }

        private void updates(ref string onlineVersion)
        {
            var updateDateInfo = update.check_for_updates(devMachine, ref newsSeq, ref configuration, ref newsInfo);
            onlineVersion = Double.Parse(updateDateInfo.version, new System.Globalization.CultureInfo("en-GB")).ToString();

            if (decimal.Parse(onlineVersion) == 0)
            {
                updateOptionsSettings.GetLblVerAvail().Text = "Unable to determine the most up-to-date version.";
            }
            else
            {
                if (decimal.Parse(version) >= decimal.Parse(onlineVersion))
                {
                    updateOptionsSettings.GetLblVerAvail().Text = "You have the most up-to-date version.";
                }
                else
                {
                    updateOptionsSettings.GetLblVerAvail().Text = "Most recent version available: " + onlineVersion;
                    updateOptionsSettings.GetBttInstallUpdateAdmin().Visible = true;
                    bttnUpdateFooter.Visible = true;
                }

                upd_url = updateDateInfo.downloadFileUrl;
                upd_file = updateDateInfo.downloadFile;
            }

            //pass the version of the update available to statusUpdate
            ListArgs a = new ListArgs();
            List<object> b = new List<object>();
            b.Add(onlineVersion);
            a.list = b;
            statusUpdate(null, a);
        }

        private void CloseAllTeboCamPocesses()
        {
            int myProcessID = Process.GetCurrentProcess().Id;
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                if (process.ProcessName == processToEnd && process.Id != myProcessID) process.Kill();
            }
        }

        private void preferences_Loaded(object sender, EventArgs e)
        {
            if (ConfigurationHelper.GetCurrentProfile().startTeboCamMinimized)
            {
                MinimiseTebocam(false);
            }
        }

        void PulseStop()
        {
            pulseStopEvent(null, new EventArgs());
        }

        private void pulseProcess(object sender, System.EventArgs e)
        {
            if (ConfigurationHelper.GetCurrentProfile().freezeGuard) pulse.Beat(motionAlarmSettings.GetBttnMotionActive().Checked ? "restart active" : "restart inactive");
        }

        private void pulseStop(object sender, System.EventArgs e)
        {
            pulse.StopPulse();
        }

        private void pulseStart(object sender, System.EventArgs e)
        {
            pulse.RestartPulse();
        }

        private void statusBarUpdate(object sender, ListArgs e)
        {
            if (Convert.ToDecimal(e._list[0]) > Convert.ToDecimal(version))
            {
                statusStrip.BackColor = Color.LemonChiffon;
                StatusStripLabel.ForeColor = Color.Black;
                StripStatusLabel.Text = "TeboCam - Version " + version + " - TeboWeb " + versionDt + " :::: Most recent version " + e._list[0].ToString() + " available as auto-install";
            }
            else
            {
                statusStrip.BackColor = System.Drawing.SystemColors.Control;
                StatusStripLabel.ForeColor = Color.Black;
                StripStatusLabel.Text = "TeboCam - Version " + version + " - Copyright TeboWeb " + versionDt;
            }
        }

        #region camera_code

        private void button4_Click(object sender, EventArgs e)
        {
            OpenNewCamera();
        }

        private void OpenNewCamera()
        {
            string tmpStr = ConfigurationHelper.GetCurrentProfile().webcam;
            CaptureDeviceForm form = new CaptureDeviceForm(tmpStr, toolTip1.Active);

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                if (form.Device.ipCam)
                {
                    IPAddress parsedIpAddress;
                    Uri parsedUri;

                    //check that the url resolves
                    if (Uri.TryCreate(form.Device.address, UriKind.Absolute, out parsedUri) && IPAddress.TryParse(parsedUri.DnsSafeHost, out parsedIpAddress))
                    {
                        var networkPinger = new System.Net.NetworkInformation.Ping();
                        PingReply reply = networkPinger.Send(parsedIpAddress);

                        //the ip webcam is running
                        if (reply.Status == IPStatus.Success)
                        {
                            if (!ConfigurationHelper.InfoForProfileWebcamExists(ConfigurationHelper.GetCurrentProfileName(), form.Device.address))
                            {
                                var configForWebcam = new configWebcam();
                                configForWebcam.webcam = form.Device.address;
                                configuration.appConfigs.First(x => x.profileName == ConfigurationHelper.GetCurrentProfileName()).camConfigs
                                    .Add(configForWebcam);
                            }

                            ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), form.Device.address)
                                .ipWebcamAddress = form.Device.address;
                            AForge.Video.MJPEGStream stream = new AForge.Video.MJPEGStream(form.Device.address);

                            if (form.Device.user != string.Empty)
                            {
                                stream.Login = form.Device.user;
                                stream.Password = form.Device.password;
                                ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), form.Device.address).ipWebcamUser = form.Device.user;
                                ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), form.Device.address).ipWebcamPassword = form.Device.password;
                                saveChanges();
                            }

                            Camera cam = openVideo.OpenVideoSource(null, stream, true, -1);
                            cam.frameRateTrack = ConfigurationHelper.GetCurrentProfile().framerateTrack;
                        }
                        else
                        {
                            MessageBox.Show("The URL you have entered is not connecting to a webcam." + Environment.NewLine +
                                            "It may be that the webcam has not fully booted yet - it can take 1 minute on some webcams." + Environment.NewLine +
                                            "You may also have supplied an incorrect URL for the webcam.",
                                            "IP Webcam not detected", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        }
                    }
                    else
                    {
                        MessageBox.Show("The URL you have entered is not valid.", "Non Valid URL", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                }
                else
                {
                    if (!CameraRig.camerasAlreadySelected(form.Device.address))
                    {
                        // create video source
                        VideoCaptureDevice localSource = new VideoCaptureDevice(form.Device.address);
                        // open camera
                        Camera cam = openVideo.OpenVideoSource(localSource, null, false, -1);
                        cam.frameRateTrack = ConfigurationHelper.GetCurrentProfile().framerateTrack;
                    }
                }
            }
        }

        //https://github.com/baSSiLL/SharpAvi/wiki/Getting-Started
        private void camera_NewFrame(object sender, System.EventArgs e)
        {
            throw new NotImplementedException();

            //#TODO
            //Set up writer
            //write frames
            //close writer


            //frameCount++;

            //if ((intervalsToSave != 0) && (saveOnMotion == true))
            //{
            //    //lets save the frame
            //    if (1 == 2)//writer == null)
            //    {
            //        // create file name
            //        DateTime date = DateTime.Now;
            //        String fileName = String.Format("{0}-{1}-{2} {3}-{4}-{5}.avi", date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);

            //        try
            //        {
            //            // create AVI writer
            //            writer = new AviWriter("video.avi");
            //            var stream = writer.AddVideoStream();
            //            var codecs = Mpeg4VideoEncoderVcm.GetAvailableCodecs();
            //            FourCC selectedCodec = KnownFourCCs.Codecs.Xvid;
            //            var encoder = new Mpeg4VideoEncoderVcm(cameraWindow.Camera.Width, cameraWindow.Camera.Height,
            //                        30, // frame rate
            //                        0, // number of frames, if known beforehand, or zero
            //                        70, // quality, though usually ignored :(
            //                        selectedCodec // codecs preference
            //                        );
            //            stream.Codec = selectedCodec;

            //            // open AVI file
            //            writer.Open(@"C:\" + fileName, cameraWindow.Camera.Width, cameraWindow.Camera.Height);
            //        }
            //        catch (ApplicationException ex)
            //        {
            //            if (writer != null)
            //            {
            //                writer.Dispose();
            //                writer = null;
            //            }
            //        }
            //    }

            //}
        }

        #endregion

        private void DeleteImages()
        {
            ArrayList ftpFiles = ftp.GetFileList();

            if (MessageDialog.messageQuestionConfirm("Click on yes to delete all saved image files.", "Delete all TeboCam image files?") == DialogResult.Yes)
            {
                if (clrImg.Checked)
                {
                    try
                    {
                        foreach (Control ctrl in this.groupBox8.Controls)
                        {
                            if (ctrl is PictureBox)
                            {

                                ((PictureBox)(ctrl)).ImageLocation = "";
                                ((PictureBox)(ctrl)).Invalidate();
                            }
                        }

                        ImageThumbs.reset();
                    }
                    catch (Exception ex)
                    {
                        TebocamState.tebowebException.LogException(ex);
                    }

                    FileManager.clearFiles(TebocamState.thumbFolder);
                    FileManager.clearFiles(TebocamState.imageFolder);
                    lblAdminMes.Text = "Image computer files deleted";
                    TebocamState.log.AddLine("Image files on computer deleted.");
                }

                if (clrFtp.Checked)
                {
                    if (ConfigurationHelper.GetCurrentProfile().filenamePrefix.Trim() != "")
                    {
                        FileManager.InitialiseDeletionRegex(true);
                        FileManager.clearFtp();
                        lblAdminMes.Text = "Image web files deleted";
                    }
                    else
                    {
                        string tmpStr;
                        tmpStr = "No images deleted as the filename prefix is empty." + Environment.NewLine;
                        tmpStr += "Which means a risk of deleting the wrong files." + Environment.NewLine + Environment.NewLine;
                        tmpStr += "To remedy this ensure the filename prefix is populated";
                        MessageDialog.messageInform(tmpStr, "Cannot delete Website files");
                        TebocamState.log.AddLine("Cannot delete image files on website due to empty filename prefix.");
                    }
                }
            }
        }

        private void bttnClearAll_Click(object sender, EventArgs e)
        {
            DeleteImages();
        }

        private void activeCountdown(object sender, DoWorkEventArgs e)
        {

            //SetCheckBox(bttnMotionSchedule, false);
            motionAlarmSettings.GetBttnMotionSchedule().SynchronisedInvoke(() => motionAlarmSettings.GetBttnMotionSchedule().Checked = false);

            countingdown = true;
            int tmpInt = 0;
            int countdown = 0;
            int lastCount = 0;

            string tmpStr;

            //Time radio button is selected
            if (ConfigurationHelper.GetCurrentProfile().countdownTime)
            {

                int startTime = time.timeInSeconds(ConfigurationHelper.GetCurrentProfile().activatecountdownTime);
                int CurrTime = time.secondsSinceMidnight();

                if (CurrTime >= startTime)
                {
                    tmpStr = "0";
                }
                else
                {
                    tmpStr = Convert.ToString(startTime - CurrTime);
                }

            }
            else
            {
                //Now radio button is selected
                if (ConfigurationHelper.GetCurrentProfile().countdownNow)
                {
                    tmpStr = "0";
                }

                //Seconds radio button is selected
                else
                {
                    tmpStr = motionAlarmSettings.GetActCountdown().Text.Trim();
                }
            }

            if (Valid.IsNumeric(tmpStr))
            {
                countdown = Convert.ToInt32(tmpStr);
                actCount.SynchronisedInvoke(() => actCount.Text = Convert.ToString(countdown));
                //SetInfo(actCount, Convert.ToString(countdown));
            }

            tmpInt = countdown;
            lastCount = tmpInt;

            secondsToTrainStart = time.secondsSinceStart();

            if (tmpInt > 0)
            {
                TebocamState.log.AddLine("Motion countdown started: " + tmpInt.ToString() + " seconds until start.");
                txtMess.SynchronisedInvoke(() => txtMess.Text = "Counting Down...");
                //SetInfo(txtMess, "Counting Down...");
            }

            //This is the loop that checks on the countdown
            while (tmpInt > 0 && !countingdownstop)
            {
                tmpInt = countdown + ((int)secondsToTrainStart - time.secondsSinceStart());
                if (lastCount != tmpInt)
                {
                    actCount.SynchronisedInvoke(() => actCount.Text = Convert.ToString(tmpInt));
                    //SetInfo(actCount, Convert.ToString(tmpInt));
                    lastCount = tmpInt;
                }
                Thread.Sleep(500);//20100731 added to free up some processor time
            }
            //This is the loop that checks on the countdown

            actCount.SynchronisedInvoke(() => actCount.Text = string.Empty);
            //SetInfo(actCount, Convert.ToString(""));
            countingdown = false;
            if (!countingdownstop)
            {
                TebocamState.Alert.on = true;
                TebocamState.log.AddLine("Motion detection activated");
            }

            txtMess.SynchronisedInvoke(() => txtMess.Text = string.Empty);
            //SetInfo(txtMess, "");
            databaseUpdate(false);

        }

        private void databaseUpdate(bool off)
        {
            if (ApiProcess.apiCredentialsValidated)
            {
                if (off)
                {
                    API.UpdateInstance("Off");
                }
                else
                {
                    if (TebocamState.Alert.on)
                    {
                        API.UpdateInstance("Active");
                    }
                    else
                    {
                        API.UpdateInstance("Inactive");
                    }
                }
            }
        }

        // On add to log
        private void log_add(object sender, System.EventArgs e)
        {
            string msg = TebocamState.log.Lines.Last().Message;
            string dt = TebocamState.log.Lines.Last().DT.ToString("yyyy/MM/dd-HH:mm:ss:fff", System.Globalization.CultureInfo.InvariantCulture);
            logControl.GetTxtLog().SynchronisedInvoke(() => logControl.GetTxtLog().Text = $"{msg} [{dt}]\n{logControl.GetTxtLog().Text}");
        }

        private void actCountdown_TextChanged(object sender, EventArgs e)
        {

        }

        private void drawLevel(object sender, System.EventArgs e)
        {
            if (showLevel) LevelControlBox.levelDraw(Movement.motionLevel);
        }

        private void drawGraph(object sender, EventArgs e)
        {
            graphDate(DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));
        }

        private void drawGraphPing(string pingGraphDate)
        {
            graphDate(pingGraphDate);
        }

        private void graphDate(string graphDate, string displayText = "")
        {
            Size size = new Size(pictureBox1.ClientRectangle.Width, pictureBox1.ClientRectangle.Height);
            GraphToDisplay.graphBitmap = graph.GetGraphFromDate(graphDate, size, TebocamState.tebowebException, displayText);
            GraphToSave.graphBitmap = (Bitmap)GraphToDisplay.graphBitmap.Clone();
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Graphics graphicsObj = e.Graphics;

                if (GraphToDisplay.graphBitmap != null)
                {
                    Size imageSize = new Size();
                    lock (GraphToDisplay.graphBitmap)
                        imageSize = GraphToDisplay.graphBitmap.Size;

                    lock (GraphToDisplay.graphBitmap)
                        graphicsObj.DrawImage(GraphToDisplay.graphBitmap, 0, 0, imageSize.Width, imageSize.Height);
                }
            }
            catch (Exception ex)
            {
                TebocamState.tebowebException.LogException(ex);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            calendar_activate();
        }

        private void calendar_activate()
        {
            calendar.Visible = !calendar.Visible;

            if (calendar.Visible)
            {
                button6.Text = "Hide Calendar";
                ArrayList tmpList = graph.getGraphDates();

                foreach (string date in tmpList)
                {
                    calendar.AddBoldedDate(DateTime.ParseExact(date, "yyyyMMdd", null));
                }

                calendar.UpdateBoldedDates();
            }
            else
            {
                button6.Text = "Show Calendar";
            }
        }

        private async void SetAPiInstanceToOff()
        {
            //20190623 commented out as this seems to be crashing the application both for installs and closing
            //return;
            //https://dotnetcodr.com/2014/01/01/5-ways-to-start-a-task-in-net-c/
            //https://blogs.msdn.microsoft.com/benjaminperkins/2017/03/08/how-to-call-an-async-method-from-a-console-app-main-method/
            if (ApiProcess.apiCredentialsValidated)
            {
                //Task.Run(() => API.UpdateInstance("Off", true)).Wait();
                await Task.Run(() => API.UpdateInstance("Off"));
            }
        }

        private void preferences_FormClosing(object sender, FormClosingEventArgs e)
        {
            SetAPiInstanceToOff();
            CloseTebocam();
        }

        private void CloseTebocam()
        {
            try
            {
                keepWorking = false;
                teboDebug.closeFile();
                tabControl1.SelectedIndex = 0;
                TebocamState.log.AddLine("Waiting for file processing to finish before exiting...");
                TebocamState.log.AddLine("Application will remain frozen until exit...");
                TebocamState.log.AddLine("Stopping TeboCam");

                if (motionAlarmSettings.GetBttnMotionActive().Checked
                    || motionAlarmSettings.GetBttnMotionAtStartup().Checked
                    || motionAlarmSettings.GetBttnActivateAtEveryStartup().Checked)
                {
                    TebocamState.Alert.on = true;
                }

                configuration.WriteXmlFile(TebocamState.xmlFolder + FileManager.configFile + ".xml", configuration);
                TebocamState.log.AddLine("Config data saved.");
                graph.WriteXMLFile(TebocamState.xmlFolder + "GraphData.xml", graph);
                TebocamState.log.AddLine("Graph data saved.");
                TebocamState.log.AddLine("Saving log data and closing.");
                log.WriteXMLFile(TebocamState.xmlFolder + "LogData" + ".xml", log);
                motionAlarmSettings.GetBttnMotionInactive().Checked = true;
                motionAlarmSettings.GetBttnMotionActive().Checked = false;
                motionAlarmSettings.GetBttnMotionAtStartup().Checked = false;
                keepWorking = false;
                closeAllCameras();
                TebocamState.log.AddLine("Application will remain frozen until exit.");
                Application.DoEvents();
                int secs = time.secondsSinceStart();
                if (ConfigurationHelper.GetCurrentProfile().freezeGuard) pulse.stopCheck(TebocamState.pulseProcessName);
                Thread.Sleep(3000);

                if (updaterInstall)
                {
                    TebocamState.log.AddLine("Preparing for installation...");
                    Application.DoEvents();
                    postProcessCommand = "profile " + ConfigurationHelper.GetCurrentProfileName();
                    var destinationFolder = Application.StartupPath;

                    update.installUpdateRestart
                        (upd_url,
                        upd_file,
                        destinationFolder,
                        processToEnd,
                        postProcess,
                        postProcessCommand,
                        updater,
                        1,
                        ConfigurationHelper.GetCurrentProfile().updateDebugLocation);
                }

            }
            catch (Exception ex)
            {
                TebocamState.tebowebException.LogException(ex);
            }
        }


        private void closeAllCameras()
        {
            foreach (var rigCam in CameraRig.ConnectedCameras)
            {
                rigCam.camera.SignalToStop();
                rigCam.camera.WaitForStop();
            }

            //if (writer != null)
            //{
            //    throw new NotImplementedException();
            //    //writer.Dispose();
            //    //writer = null;
            //}

            intervalsToSave = 0;
        }

        private void calendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            string dateSelected = calendar.SelectionStart.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

            if (!ConfigurationHelper.GetCurrentProfile().captureMovementImages)
            {
                graphDate(dateSelected, "Image capture switched off");
                return;
            }

            if (graph.dataExistsForDate(dateSelected))
            {
                graphDate(dateSelected);
            }
            else
            {
                graphDate(null);
            }
        }

        private void time_change(object sender, System.EventArgs e)
        {
            motionAlarmSettings.GetLblTime().SynchronisedInvoke(() => motionAlarmSettings.GetLblTime().Text = lastTime);
        }

        private void ThumbsPrepare()
        {
            int tmpInt = 0;
            foreach (Control ctrl in this.groupBox8.Controls)
            {
                if (ctrl is PictureBox)
                {
                    ImageThumbs.thumbNames.Add(ctrl.Name);
                    tmpInt++;
                }
            }
            ImageThumbs.thumbNames.Sort();
            ImageThumbs.initPics(tmpInt);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                updateThumbs();
            }
        }

        private void picWindow_ValueChanged(object sender, EventArgs e)
        {
            updateThumbs();
        }

        public void updateThumbs()
        {
            string[,] picsForWindow = ImageThumbs.populateWindow(picWindow.Value);
            bool picFound = false;

            foreach (Control ctrl in this.groupBox8.Controls)
            {
                picFound = false;
                if (ctrl is PictureBox)
                {
                    for (int i = 0; i < ImageThumbs.picsInWindowCount; i++)
                    {
                        if (ctrl.Name == picsForWindow[i, 0] && File.Exists(picsForWindow[i, 1]))
                        {
                            ((PictureBox)(ctrl)).ImageLocation = picsForWindow[i, 1];
                            picFound = true;
                            break;
                        }
                    }
                    if (!picFound) ((PictureBox)(ctrl)).ImageLocation = "";
                }
            }

        }

        private int imageFilesCount()
        {
            DirectoryInfo imageInfo = new DirectoryInfo(TebocamState.imageFolder);
            FileInfo[] imageFiles = imageInfo.GetFiles("*" + TebocamState.ImgSuffix);
            DirectoryInfo thumbInfo = new DirectoryInfo(TebocamState.thumbFolder);
            FileInfo[] thumbFiles = thumbInfo.GetFiles("*" + TebocamState.ImgSuffix);
            var fileCount = imageFiles.Length + thumbFiles.Length;
            return fileCount;
        }

        private int imageFilesCountWeb()
        {
            ArrayList webFiles = ftp.GetFileList();
            return webFiles.Count;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            lblCountOnComputer.Text = $"Computer: {imageFilesCount().ToString()}";
            lblCountOnWeb.Text = $"Website: {imageFilesCountWeb().ToString()}";
            Invalidate();
        }

        private void installationClean()
        {
            FileManager.CreateDirIfNotExists(TebocamState.imageParentFolder);
            FileManager.CreateDirIfNotExists(TebocamState.imageFolder);
            FileManager.CreateDirIfNotExists(TebocamState.thumbFolder);
            FileManager.CreateDirIfNotExists(TebocamState.resourceFolder);
            FileManager.CreateDirIfNotExists(TebocamState.exceptionFolder);

            string configXml = TebocamState.xmlFolder + "config.xml";
            if (!Directory.Exists(TebocamState.vaultFolder) && File.Exists(configXml))
            {
                Directory.CreateDirectory(TebocamState.vaultFolder);
                string configVlt = TebocamState.vaultFolder + "config262.xml";
                if (!File.Exists(configVlt))
                {
                    File.Copy(configXml, configVlt, true);
                }
            }

            FileManager.CreateDirIfNotExists(TebocamState.tmpFolder);
            DirectoryInfo diTmp = new DirectoryInfo(TebocamState.tmpFolder);
            FileInfo[] imageFilesTmp = diTmp.GetFiles();
            foreach (FileInfo fi in imageFilesTmp)
            {
                File.Delete(fi.FullName);
            }

            FileManager.CreateDirIfNotExists(TebocamState.logFolder);

            if (!Directory.Exists(TebocamState.xmlFolder))
            {
                Directory.CreateDirectory(TebocamState.xmlFolder);
                DirectoryInfo diApp = new DirectoryInfo(Application.StartupPath);
                FileInfo[] xmlFilesApp = diApp.GetFiles("*.xml");
                foreach (FileInfo fi in xmlFilesApp)
                {
                    if (fi.FullName != "Ionic.Zip.xml") File.Move(fi.FullName, TebocamState.xmlFolder + fi.Name);
                }
            }

            if (File.Exists(TebocamState.xmlFolder + "processed.xml")) { File.Delete(TebocamState.xmlFolder + "processed.xml"); };
            if (File.Exists(Application.StartupPath + databaseTrialFile)) { File.Move(Application.StartupPath + databaseTrialFile, Application.StartupPath + ApiConnectFile); };
        }

        private void populateFromProfile(string profileName)
        {
            configApplication data = ConfigurationHelper.getProfile(profileName);
            ConfigurationHelper.GetCurrentProfile().areaDetection = data.areaDetection;
            ConfigurationHelper.GetCurrentProfile().areaDetectionWithin = data.areaDetectionWithin;
            ConfigurationHelper.GetCurrentProfile().baselineVal = data.baselineVal;
            ConfigurationHelper.GetCurrentProfile().webcam = data.webcam;
            motionAlarmSettings.GetActCountdown().Text = data.activatecountdown.ToString();
            bool cmdLine = false;
            foreach (string arg in Environment.GetCommandLineArgs())
            {
                if (LeftRightMid.Left(arg, 1) == "/")
                {
                    cmdLine = true;
                    break;
                }
            }

            if (cmdLine)
            {
                motionAlarmSettings.GetBttnMotionActive().Checked = false;
                motionAlarmSettings.GetBttnMotionInactive().Checked = true;
            }
            else
            {
                motionAlarmSettings.GetBttnMotionActive().Checked = data.AlertOnStartup;
                motionAlarmSettings.GetBttnMotionInactive().Checked = !data.AlertOnStartup;
            }

            //maintain the order of bttnSeconds.Checked, bttnTime.Checked, bttnNow.Checked
            motionAlarmSettings.GetBttnSeconds().Checked = !data.countdownTime;
            motionAlarmSettings.GetBttnTime().Checked = data.countdownTime;
            motionAlarmSettings.GetBttnNow().Checked = data.countdownNow;
            //maintain the order of bttnSeconds.Checked, bttnTime.Checked, bttnNow.Checked

            //20101023 legacy code - cycleStamp replaced by cycleStampChecked
            if (data.cycleStamp) data.cycleStampChecked = 1;
            //20101023 legacy code - cycleStamp replaced by cycleStampChecked

            alertTimeSettings.GetEmailNotifInterval().Text = data.emailNotifyInterval.ToString();
            securityLockdownSettings.GetTxtLockdownPassword().Text = data.lockdownPassword;
            securityLockdownSettings.GetRdLockdownOn().Checked = data.lockdownOn;
            securityLockdownSettings.GetBtnSecurityLockdownOn().Enabled = data.lockdownOn;
            lockdown = data.lockdownOn;

            alertTimeSettings.GetImageFileInterval().Text = data.imageSaveInterval.ToString();
            generateWebpageSettings.GetLblImgPref().Text = "Image Prefix: " + data.filenamePrefix + "   e.g " + data.filenamePrefix + "1" + TebocamState.ImgSuffix;
            notificationSettings.SetLoadToFtp(data.loadImagesToFtp);
            notificationSettings.SetMaxImagesToEmail(data.maxImagesToEmail);
            motionAlarmSettings.GetNumericUpDown1().Value = Convert.ToDecimal(LeftRightMid.Left(data.activatecountdownTime, 2));
            motionAlarmSettings.GetNumericUpDown2().Value = Convert.ToDecimal(LeftRightMid.Right(data.activatecountdownTime, 2));
            publishSettings.SetPubTimerOn(data.timerOn);

            if (publishSettings.GetPubTimerOn().Checked)
            {
                publishSettings.SetLblstartpub(Color.Black);
                publishSettings.SetLblendpub(Color.Black);
            }
            else
            {
                publishSettings.SetLblstartpub(System.Drawing.SystemColors.Control);
                publishSettings.SetLblendpub(System.Drawing.SystemColors.Control);
            }

            motionAlarmSettings.GetBttnMotionSchedule().Checked = data.timerOnMov;
            motionAlarmSettings.GetBttnMotionScheduleOnAtStart().Checked = data.scheduleOnAtStart;

            //the schedule on at start box is checked so we set the schedule on if it is not on
            if (!data.timerOnMov && data.scheduleOnAtStart)
            {
                motionAlarmSettings.GetBttnMotionSchedule().Checked = true;
            }

            motionAlarmSettings.GetBttnActivateAtEveryStartup().Checked = data.activateAtEveryStartup;

            if (motionAlarmSettings.GetBttnMotionSchedule().Checked)
            {
                motionAlarmSettings.GetLblstartmov().ForeColor = Color.Black;
                motionAlarmSettings.GetLblendmov().ForeColor = Color.Black;
            }
            else
            {
                motionAlarmSettings.GetLblstartmov().ForeColor = System.Drawing.SystemColors.Control;
                motionAlarmSettings.GetLblendmov().ForeColor = System.Drawing.SystemColors.Control;
            }

            notificationSettings.SetPing(data.ping);
            notificationSettings.SetPingMins(data.pingInterval);

            if (data.sendThumbnailImages)
            {
                notificationSettings.SetSendThumb(data.sendThumbnailImages);
                notificationSettings.SetSendFullSize(data.sendFullSizeImages);
                notificationSettings.SetSendMosaic(data.sendMosaicImages);
            }
            else if (data.sendFullSizeImages)
            {
                notificationSettings.SetSendFullSize(data.sendFullSizeImages);
                notificationSettings.SetSendThumb(data.sendThumbnailImages);
                notificationSettings.SetSendMosaic(data.sendMosaicImages);
            }
            else if (data.sendMosaicImages)
            {
                notificationSettings.SetSendMosaic(data.sendMosaicImages);
                notificationSettings.SetSendFullSize(data.sendFullSizeImages);
                notificationSettings.SetSendThumb(data.sendThumbnailImages);
            }

            notificationSettings.SetMosaicImagesPerRow(data.mosaicImagesPerRow);
            notificationSettings.SetSendEmail(data.sendNotifyEmail);

            emailHostSettings.SetPassword(data.emailPass);
            emailHostSettings.SetUser(data.emailUser);
            emailHostSettings.SetHost(data.smtpHost);
            emailHostSettings.SetPort(data.smtpPort.ToString());
            emailHostSettings.SetSsl((bool)data.EnableSsl);

            ftpSettings.SetUser(data.ftpUser);
            ftpSettings.SetPassword(data.ftpPass);
            ftpSettings.SetRoot(data.ftpRoot);

            emailSettings.SetMailBody(data.mailBody);
            emailSettings.SetMailSubject(data.mailSubject);
            emailSettings.SetPingSubject(data.pingSubject);
            emailSettings.SetReplyTo(data.replyTo);
            emailSettings.SetSentBy(data.sentBy);
            emailSettings.SetSendTo(data.sendTo);
            emailSettings.SetSentByName(data.sentByName);

            updateOptionsSettings.GetUpdateNotify().Checked = data.updatesNotify;
            updateOptionsSettings.GetUpdateDebugLocationn().Text = data.updateDebugLocation;

            publishSettings.SetPubImage(data.pubImage);
            if (decimal.Parse(data.profileVersion) < 2.6m) //m forces number to be interpreted as decimal
            {
                data.publishWeb = data.pubImage;
            }

            publishSettings.SetPubFtpUser(data.pubFtpUser);
            publishSettings.SetPubFtpPass(data.pubFtpPass);
            publishSettings.SetPubFtpRoot(data.pubFtpRoot);

            if (data.motionLevel)
            {
                showLevel = true;
                levelShow.Image = TeboCam.Properties.Resources.nolevel;
            }
            else
            {
                showLevel = false;
                levelShow.Image = TeboCam.Properties.Resources.level;
                LevelControlBox.levelDraw(0);
            }

            LevelControlBox.Visible = showLevel;
            onlineSettings.GetWebUpd().Checked = data.webUpd;
            onlineSettings.GetSqlUser().Text = data.webUser;
            onlineSettings.GetSqlPwd().Text = data.webPass;
            onlineSettings.GetSqlPoll().Text = data.webPoll.ToString();
            onlineSettings.GetTxtEndpoint().Text = data.AuthenticateEndpoint;
            onlineSettings.GetTxtEndpointLocal().Text = data.LocalAuthenticateEndpoint;
            if (!string.IsNullOrEmpty(data.AuthenticateEndpoint))
            {
                Uri uriRemote = new Uri(data.AuthenticateEndpoint);
                API.RemoteURI = uriRemote.Scheme + Uri.SchemeDelimiter + uriRemote.Authority;
            }

            if (!string.IsNullOrEmpty(data.LocalAuthenticateEndpoint))
            {
                Uri uriLocal = new Uri(data.LocalAuthenticateEndpoint);
                API.LocalURI = uriLocal.Scheme + Uri.SchemeDelimiter + uriLocal.Authority;
            }
            onlineSettings.GetTxtPickupDirectory().Text = data.PickupDirectory;
            onlineSettings.GetRdApiRemote().Checked = data.UseRemoteEndpoint;
            onlineSettings.GetRdApiLocal().Checked = !onlineSettings.GetRdApiRemote().Checked;
            API.UseRemoteURI = onlineSettings.GetRdApiRemote().Checked;
            onlineSettings.GetSqlInstance().Text = data.webInstance;
            onlineSettings.GetSqlImageRoot().Text = data.webImageRoot;
            onlineSettings.GetSqlImageFilename().Text = data.webImageFileName;
            onlineSettings.GetSqlFtpUser().Text = data.webFtpUser;
            onlineSettings.GetSqlFtpPwd().Text = data.webFtpPass;

            //20101026 convert old publish timestamp to current object
            if (data.pubStamp) data.publishTimeStamp = true;
            if (data.pubStampDate) data.publishTimeStampFormat = "ddmmyy";
            if (data.pubStampTime) data.publishTimeStampFormat = "hhmm";
            if (data.pubStampDateTime) data.publishTimeStampFormat = "ddmmyyhhmm";
            if (data.pubStamp || data.pubStampDate || data.pubStampTime || data.pubStampDateTime)
            {
                data.publishTimeStampColour = "red";
                data.publishTimeStampPosition = "tl";
            }
            //20101026 convert old publish timestamp to current object

            emailIntelligenceSettings.SetEmailIntelOn(data.EmailIntelOn);
            emailIntelligenceSettings.SetEmailIntelEmails(data.emailIntelEmails);
            emailIntelligenceSettings.SetEmailIntelMins(data.emailIntelMins);
            emailIntelligenceSettings.SetEmailIntelStop(data.EmailIntelStop);
            emailIntelligenceSettings.SetEmailIntelMosaic(!data.EmailIntelStop);

            movementStatisticsSettings.SetRdStatsToFileOn(data.StatsToFileOn);
            movementStatisticsSettings.SetPnlStatsToFile(data.StatsToFileOn);
            movementStatisticsSettings.SetChkStatsToFileTimeStamp(data.StatsToFileTimeStamp);
            movementStatisticsSettings.SetTxtStatsToFileMb(data.StatsToFileMb);

            onlineSettings.GetDisCommOnline().Checked = data.disCommOnline;
            onlineSettings.GetDisCommOnlineSecs().Text = data.disCommOnlineSecs.ToString();
            onlineSettings.GetDisCommOnlineSecs().Enabled = onlineSettings.GetDisCommOnline().Checked;

            notificationSettings.SetPlSnd(data.soundAlertOn);
            freezeGuardSettings.SetFreezeGuardOn(data.freezeGuard);
            freezeGuardSettings.SetFreezeGuardWindow(data.freezeGuardWindowShow);
            freezeGuardSettings.SetPulseFreq(data.pulseFreq.ToString());
            frameRateSettings.GetNumFrameRateCalcOver().Value = data.framesSecsToCalcOver;
            frameRateSettings.GetChkFrameRateTrack().Checked = data.framerateTrack;
            miscSettings.GetChkHideWhenMinimised().Checked = data.hideWhenMinimized;
            imagesSavedFolderSettings.GetRadioButton11().Checked = data.imageLocCust;

            if (data.imageToframe)
            {
                imageInFrame.Image = TeboCam.Properties.Resources.arrowOut;
                cameraWindow.imageToFrame = true;
                panel1.AutoScroll = false;
            }
            else
            {
                imageInFrame.Image = TeboCam.Properties.Resources.arrowIn;
                cameraWindow.imageToFrame = false;
                panel1.AutoScroll = true;
            }

            if (data.cameraShow)
            {
                cameraWindow.showCam = true;
                cameraShow.Image = TeboCam.Properties.Resources.nolandscape;
            }
            else
            {
                cameraWindow.showCam = false;
                cameraShow.Image = TeboCam.Properties.Resources.landscape;
            }

            publishSettings.SetLblstartpub("Scheduled start: " + LeftRightMid.Left(data.timerStartPub, 2) + ":" + LeftRightMid.Right(data.timerStartPub, 2));
            publishSettings.SetLblendpub("Scheduled end: " + LeftRightMid.Left(data.timerEndPub, 2) + ":" + LeftRightMid.Right(data.timerEndPub, 2));
            motionAlarmSettings.GetLblstartmov().Text = "Start: " + LeftRightMid.Left(data.timerStartMov, 2) + ":" + LeftRightMid.Right(data.timerStartMov, 2);
            motionAlarmSettings.GetLblendmov().Text = "End: " + LeftRightMid.Left(data.timerEndMov, 2) + ":" + LeftRightMid.Right(data.timerEndMov, 2);
            notificationSettings.SetCaptureMovementImages(data.captureMovementImages);

            if (!data.captureMovementImages)
            {
                graphDate(DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture), "Image capture switched off");
            }

            internetConnectionCheckSettings.GetTxtInternetConnection().Text = data.internetCheck;
            bttnToolTips.Text = data.toolTips ? "Turn OFF Tool Tips" : "Turn ON Tool Tips";
            toolTip1.Active = data.toolTips;
            startMinimized.Checked = data.startTeboCamMinimized;
            TebocamState.imageParentFolder = TebocamState.imageParentFolder = Application.StartupPath + @"\images\";
            TebocamState.imageFolder = TebocamState.imageParentFolder + @"fullSize\";
            TebocamState.thumbFolder = TebocamState.imageParentFolder + @"thumb\";

            if (imagesSavedFolderSettings.GetRadioButton11().Checked)
            {
                if (
                       Directory.Exists(ConfigurationHelper.GetCurrentProfile().imageParentFolderCust)
                    && Directory.Exists(ConfigurationHelper.GetCurrentProfile().imageFolderCust)
                    && Directory.Exists(ConfigurationHelper.GetCurrentProfile().thumbFolderCust)
                    )
                {
                    TebocamState.imageParentFolder = ConfigurationHelper.GetCurrentProfile().imageParentFolderCust;
                    TebocamState.imageFolder = ConfigurationHelper.GetCurrentProfile().imageFolderCust;
                    TebocamState.thumbFolder = ConfigurationHelper.GetCurrentProfile().thumbFolderCust;
                }
            }
            else
            {
                ConfigurationHelper.GetCurrentProfile().imageParentFolderCust = TebocamState.imageParentFolder;
                ConfigurationHelper.GetCurrentProfile().imageFolderCust = TebocamState.imageFolder;
                ConfigurationHelper.GetCurrentProfile().thumbFolderCust = TebocamState.thumbFolder;
            }
        }

        private void clearOldLogs()
        {
            int deleteDate = Convert.ToInt32(DateTime.Now.AddDays(-ConfigurationHelper.GetCurrentProfile().logsKeep).ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));
            DirectoryInfo dInfo = new DirectoryInfo(TebocamState.logFolder);
            FileInfo[] logFiles = dInfo.GetFiles("log_" + "*.xml");
            int fileCount = logFiles.Length;

            foreach (FileInfo file in logFiles)
            {
                int fileDate = Convert.ToInt32(LeftRightMid.Mid(file.Name, 4, 8));
                if (fileDate < deleteDate) File.Delete(TebocamState.logFolder + file.Name);
            }
        }

        private void preferences_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized && ConfigurationHelper.GetCurrentProfile().hideWhenMinimized)
            {
                MinimiseTebocam(false);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowTebocam();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            MinimiseTebocam(true);
        }

        private void MinimiseTebocam(bool hide)
        {
            WindowState = FormWindowState.Minimized;
            if (ConfigurationHelper.GetCurrentProfile().hideWhenMinimized || hide)
            {
                Hide();
            }
        }

        private void ShowTebocam()
        {
            WindowState = FormWindowState.Maximized;
            Show();
            this.BringToFront();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            SetAPiInstanceToOff();
        }

        private void rightClickMenuActivate_Click(object sender, EventArgs e)
        {
            motionAlarmSettings.GetBttnMotionActive().Checked = true;
            motionAlarmSettings.GetBttnMotionInactive().Checked = false;
        }

        private void rightClickInactivate_Click(object sender, EventArgs e)
        {
            motionAlarmSettings.GetBttnMotionInactive().Checked = true;
            motionAlarmSettings.GetBttnMotionActive().Checked = false;
        }

        private void rightClickMenuLock_Click(object sender, EventArgs e)
        {

        }

        private void lockTebocam()
        {
            MinimiseTebocam(false);
            this.Enabled = false;

            while (true)
            {

                if (Prompt.ShowDialog("Password", "Enter password to unlock") == ConfigurationHelper.GetCurrentProfile().lockdownPassword)
                {
                    this.Enabled = true;
                    ShowTebocam();
                    break;
                }
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            ShowTebocam();
        }

        private void hideLog_Click(object sender, EventArgs e)
        {
            if (logControl.GetTxtLog().Visible)
            {
                logControl.GetTxtLog().Visible = false;
                hideLog.Text = "Show Log";
            }
            else
            {
                logControl.GetTxtLog().Visible = true;
                hideLog.Text = "Hide Log";
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Internet.openInternetBrowserAt(tebowebUrl);
        }

        private void bttnMotionSchedule_CheckedChanged(object sender, EventArgs e)
        {

        }

        private scheduleClass.scheduleAction scheduleStart(string p_start, string p_end, bool currentlyActive)
        {
            //0 - do nothing
            //1 - start
            //2 - end

            int startTime = time.timeInSeconds(p_start);
            int endTime = time.timeInSeconds(p_end);
            int CurrTime = time.secondsSinceMidnight();
            scheduleClass.scheduleAction returnVal = scheduleClass.scheduleAction.no_action;
            bool zeroPassed = false;

            if (startTime < endTime)
            {
                //start 
                if (!currentlyActive && CurrTime >= startTime && CurrTime < endTime)
                {
                    returnVal = scheduleClass.scheduleAction.start;
                }

                //end 
                if (currentlyActive && (CurrTime >= endTime || CurrTime < startTime))
                {
                    returnVal = scheduleClass.scheduleAction.end;
                }
            }
            else
            {
                //start 
                if (!currentlyActive && CurrTime >= startTime)
                {
                    returnVal = scheduleClass.scheduleAction.start;
                    zeroPassed = false;
                }

                if (currentlyActive && CurrTime < startTime)
                {
                    zeroPassed = true;
                }

                //end 
                if (zeroPassed && CurrTime >= endTime)
                {
                    returnVal = scheduleClass.scheduleAction.end;
                    zeroPassed = false;

                }
            }
            return returnVal;
        }

        private void frameRate()
        {
            for (int i = 0; i < CameraRig.ConnectedCameras.Count(); i++)
            {
                List<Camera.FrameRateInfo> frameInfo = CameraRig.ConnectedCameras[i].camera.frames.framesInfo;

                if (frameInfo.Count == 0)
                {
                    lblFrameRate.SynchronisedInvoke(() => lblFrameRate.Text = "0");
                    CameraRig.ConnectedCameras[i].camera.FrameRateCalculated = 0;
                    return;
                }

                DateTime start = frameInfo.First().dateTime;
                DateTime end = frameInfo.Last().dateTime;
                int frames = frameInfo.Count;
                int secondsElapsed = (int)(end - start).TotalSeconds;
                int avgFrames = secondsElapsed > 0 ? frames / secondsElapsed : 0;
                CameraRig.ConnectedCameras[i].camera.FrameRateCalculated = avgFrames;
                CameraRig.ConnectedCameras[i].camera.frames.purge(ConfigurationHelper.GetCurrentProfile().framesSecsToCalcOver);
            }

            if (CameraRig.ConnectedCameras.Count > 0)
            {
                lblFrameRate.SynchronisedInvoke(() => lblFrameRate.Text = CameraRig.ConnectedCameras[CameraRig.CurrentlyDisplayingCamera].camera.FrameRateCalculated.ToString());
            }
        }

        private void reconnectLostCameras()
        {
            for (int i = CameraRig.ConnectedCameras.Count() - 1; i >= 0; i--)
            {
                ConnectedCamera camera = CameraRig.ConnectedCameras[i];

                //let's drop and reconnect cameras providing zero framerates 
                if (camera.camera.frameRateTrack &&
                    camera.camera.FrameRateCalculated == 0 &&
                    camera.camera.frames.LastFrameSecondsAgo() > 10 &&
                    (DateTime.Now - camera.camera.ConnectedAt).TotalSeconds > 10)
                {
                    //get the camera source name
                    //then see if camera is in the available sources
                    IEnumerable<FilterInfo> filters = new FilterInfoCollection(FilterCategory.VideoInputDevice).Cast<FilterInfo>();
                    var filter = filters.FirstOrDefault(x => x.MonikerString == camera.cameraName);

                    if (filters != null)
                    {
                        NotConnectedCameras.First(x => x.id == camera.displayButton).CameraButtonIsNotConnected();
                        var camNo = camera.camera.camNo;
                        var friendlyName = camera.friendlyName;
                        //drop the camera                 
                        CameraRig.cameraRemove(i, false);
                        //reconnect the camera
                        VideoCaptureDevice localSource = new VideoCaptureDevice(filter.MonikerString);
                        Camera cam = openVideo.OpenVideoSource(localSource, null, false, camNo);
                        cam.frameRateTrack = ConfigurationHelper.GetCurrentProfile().framerateTrack;
                        TebocamState.log.AddLine(string.Format("Reconnecting lost [{0}] camera no. {1}.", friendlyName, cam.camNo.ToString()));
                        selcam(camera.displayButton, true);

                        if (cam.publishActive)
                        {
                            pubcam(camera.displayButton);
                        }
                    }
                }
            }

        }

        private void connectCamerasMissingAtStartup()
        {
            var profile = configuration.appConfigs.Where(x => x.profileName == ConfigurationHelper.GetCurrentProfileName()).First();
            IEnumerable<FilterInfo> filters = new FilterInfoCollection(FilterCategory.VideoInputDevice).Cast<FilterInfo>();

            foreach (configWebcam expectedCamera in profile.camConfigs)
            {
                var cameraIsConnected = CameraRig.ConnectedCameras.Any(x => x.cameraName == expectedCamera.webcam);

                if (!cameraIsConnected)
                {
                    if (filters.Any(x => x.MonikerString == expectedCamera.webcam))
                    {
                        VideoCaptureDevice localSource = new VideoCaptureDevice(expectedCamera.webcam);
                        Camera cam = openVideo.OpenVideoSource(localSource, null, false, -1);
                        cam.frameRateTrack = ConfigurationHelper.GetCurrentProfile().framerateTrack;
                        TebocamState.log.AddLine(string.Format("Connecting [{0}] camera not found at startup.", expectedCamera.friendlyName));
                        selcam(expectedCamera.displayButton, true);

                        if (cam.publishActive)
                        {
                            pubcam(expectedCamera.displayButton);
                        }
                    }
                }
            }
        }

        private void CheckAndRunScheduledOperations()
        {
            var checkScheduleResult = scheduleClass.scheduleAction.no_action;

            if (publishSettings.GetPubTimerOn().Checked)
            {
                checkScheduleResult = scheduleStart(ConfigurationHelper.GetCurrentProfile().timerStartPub,
                                                    ConfigurationHelper.GetCurrentProfile().timerEndPub,
                                                    publisher.keepPublishing);

                switch (checkScheduleResult)
                {
                    case scheduleClass.scheduleAction.start:
                        publishSwitch(null, new EventArgs());
                        TebocamState.log.AddLine("Web publish scheduled time start.");
                        break;
                    case scheduleClass.scheduleAction.end:
                        publishSwitch(null, new EventArgs());
                        TebocamState.log.AddLine("Web publish scheduled time end.");
                        break;
                }

            }

            if (motionAlarmSettings.GetBttnMotionSchedule().Checked)
            {
                checkScheduleResult = scheduleStart(ConfigurationHelper.GetCurrentProfile().timerStartMov,
                                                    ConfigurationHelper.GetCurrentProfile().timerEndMov,
                                                    TebocamState.Alert.on);

                switch (checkScheduleResult)
                {
                    case scheduleClass.scheduleAction.start:
                        motionDetectionActivate(null, new EventArgs());
                        TebocamState.log.AddLine("Motion active scheduled time start.");
                        break;
                    case scheduleClass.scheduleAction.end:
                        if (motionAlarmSettings.GetBttnMotionActive().Checked)
                        {
                            motionDetectionInactivate(null, new EventArgs());
                            TebocamState.log.AddLine("Motion active scheduled time end.");
                        }
                        break;
                }
            }
        }

        private void publish_switch(object sender, System.EventArgs e)
        {
            publishSettings.GetPubImage().SynchronisedInvoke(() => publishSettings.GetPubImage().Checked = !publisher.keepPublishing);
            publishSettings.GetPubTimerOn().SynchronisedInvoke(() => publishSettings.GetPubTimerOn().Checked = true);
        }

        private void tabControl1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == "Images")
            {
                updateThumbs();
                onlineVal.Enabled = ApiProcess.apiCredentialsValidated;
                rdOnlinejpg.Enabled = ApiProcess.apiCredentialsValidated;
                alertVal.Text = ConfigurationHelper.GetCurrentProfile().alertCompression.ToString();
                pingVal.Text = ConfigurationHelper.GetCurrentProfile().pingCompression.ToString();
                publishVal.Text = ConfigurationHelper.GetCurrentProfile().publishCompression.ToString();
                onlineVal.Text = ConfigurationHelper.GetCurrentProfile().onlineCompression.ToString();
            }
        }

        private void motionDetectionActivate(object sender, System.EventArgs e)
        {
            //inactivate motion detection first in case a countdown is taking place
            motionAlarmSettings.GetBttnMotionInactive().SynchronisedInvoke(() => motionAlarmSettings.GetBttnMotionInactive().Checked = true);
            motionAlarmSettings.GetBttnMotionActive().SynchronisedInvoke(() => motionAlarmSettings.GetBttnMotionActive().Checked = false);

            Thread.Sleep(4000);

            //now activate motion detection
            motionAlarmSettings.GetBttnNow().SynchronisedInvoke(() => motionAlarmSettings.GetBttnNow().Checked = true);
            motionAlarmSettings.GetBttnTime().SynchronisedInvoke(() => motionAlarmSettings.GetBttnTime().Checked = false);
            motionAlarmSettings.GetBttnSeconds().SynchronisedInvoke(() => motionAlarmSettings.GetBttnSeconds().Checked = false);
            motionAlarmSettings.GetBttnMotionActive().SynchronisedInvoke(() => motionAlarmSettings.GetBttnMotionActive().Checked = true);
            motionAlarmSettings.GetBttnMotionInactive().SynchronisedInvoke(() => motionAlarmSettings.GetBttnMotionInactive().Checked = false);
        }

        private void motionDetectionInactivate(object sender, System.EventArgs e)
        {
            if (motionAlarmSettings.GetBttnMotionSchedule().Checked)
            {
                motionAlarmSettings.GetBttnMotionSchedule().SynchronisedInvoke(() => motionAlarmSettings.GetBttnMotionSchedule().Checked = false);
            }

            motionAlarmSettings.GetBttnMotionInactive().SynchronisedInvoke(() => motionAlarmSettings.GetBttnMotionInactive().Checked = true);
            motionAlarmSettings.GetBttnMotionActive().SynchronisedInvoke(() => motionAlarmSettings.GetBttnMotionActive().Checked = false);
        }

        private ArrayList readTextFileintoArrayList(string file)
        {
            string line = null;
            ArrayList contents = new ArrayList();

            try
            {
                TextReader tr = new StreamReader(file);
                while ((line = tr.ReadLine()) != null) { contents.Add(line); }
                tr.Close();
                return contents;
            }
            catch (Exception e)
            {
                TebocamState.tebowebException.LogException(e);
                contents.Clear();
                contents.Add("Unable to retrieve information.");
                return contents;
            }
        }


        private void newsInfo_Click(object sender, EventArgs e)
        {
            var license = readTextFileintoArrayList(TebocamState.resourceFolder + "license.txt");
            var info = readTextFileintoArrayList(TebocamState.resourceFolder + "tebocaminfo.txt");
            var news = readTextFileintoArrayList(TebocamState.resourceFolder + "tebocamnews.txt");
            var whatsNew = readTextFileintoArrayList(TebocamState.resourceFolder + "tebocamwhatsnew.txt");
            newsInfo.BackColor = SystemColors.Control;
            News form = new News(news, info, whatsNew, license);
            form.Show();
        }

        private void button24_Click(object sender, EventArgs e)
        {
            saveChanges();
        }

        private void saveChanges()
        {
            configuration.WriteXmlFile(TebocamState.xmlFolder + FileManager.configFile + ".xml", configuration);
            TebocamState.log.AddLine("Config data saved.");
        }

        private void startMinimized_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().startTeboCamMinimized = startMinimized.Checked;
        }

        private void bttnToolTips_Click_1(object sender, EventArgs e)
        {
            toolTip1.Active = !toolTip1.Active;
            bttnToolTips.Text = toolTip1.Active ? "Turn OFF Tool Tips" : "Turn ON Tool Tips";
            ConfigurationHelper.GetCurrentProfile().toolTips = toolTip1.Active;
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            calendar_activate();
        }

        public static List<Control> controls(Control ctrl)
        {
            List<Control> controlList = new List<Control>();

            foreach (Control childControl in ctrl.Controls)
            {
                {
                    // Recurse child controls.
                    controlList.AddRange(controls(childControl));
                    controlList.Add(childControl);
                }
            }
            return controlList;
        }

        private void jPegSetCompression(ArrayList i)
        {
            var compressionContext = i[0].ToString();
            var displayVal = i[1].ToString();
            var compression = Convert.ToInt32(displayVal);

            switch (compressionContext)
            {
                case "Alert":
                    ConfigurationHelper.GetCurrentProfile().alertCompression = compression;
                    alertVal.SynchronisedInvoke(() => alertVal.Text = displayVal);
                    break;
                case "Publish":
                    ConfigurationHelper.GetCurrentProfile().publishCompression = compression;
                    publishVal.SynchronisedInvoke(() => publishVal.Text = displayVal);
                    break;
                case "Ping":
                    ConfigurationHelper.GetCurrentProfile().pingCompression = compression;
                    pingVal.SynchronisedInvoke(() => pingVal.Text = displayVal);
                    break;
                case "Online":
                    ConfigurationHelper.GetCurrentProfile().onlineCompression = compression;
                    onlineVal.SynchronisedInvoke(() => onlineVal.Text = displayVal);
                    break;
                default:
                    break;
            }
        }

        private void timeStampMth(List<List<object>> stampList)
        {
            foreach (List<object> item in stampList)
            {
                if (item[0].ToString() == "Online")
                {
                    ConfigurationHelper.GetCurrentProfile().onlineTimeStamp = Convert.ToBoolean(item[1]);
                    ConfigurationHelper.GetCurrentProfile().onlineTimeStampFormat = item[2].ToString();
                    ConfigurationHelper.GetCurrentProfile().onlineTimeStampColour = item[3].ToString();
                    ConfigurationHelper.GetCurrentProfile().onlineTimeStampPosition = item[4].ToString();
                    ConfigurationHelper.GetCurrentProfile().onlineTimeStampRect = Convert.ToBoolean(item[5]);
                    ConfigurationHelper.GetCurrentProfile().onlineStatsStamp = Convert.ToBoolean(item[7]);
                }

                if (item[0].ToString() == "Publish")
                {
                    ConfigurationHelper.GetCurrentProfile().publishTimeStamp = Convert.ToBoolean(item[1]);
                    ConfigurationHelper.GetCurrentProfile().publishTimeStampFormat = item[2].ToString();
                    ConfigurationHelper.GetCurrentProfile().publishTimeStampColour = item[3].ToString();
                    ConfigurationHelper.GetCurrentProfile().publishTimeStampPosition = item[4].ToString();
                    ConfigurationHelper.GetCurrentProfile().publishTimeStampRect = Convert.ToBoolean(item[5]);
                    ConfigurationHelper.GetCurrentProfile().publishStatsStamp = Convert.ToBoolean(item[7]);
                }

                if (item[0].ToString() == "Ping")
                {
                    ConfigurationHelper.GetCurrentProfile().pingTimeStamp = Convert.ToBoolean(item[1]);
                    ConfigurationHelper.GetCurrentProfile().pingTimeStampFormat = item[2].ToString();
                    ConfigurationHelper.GetCurrentProfile().pingTimeStampColour = item[3].ToString();
                    ConfigurationHelper.GetCurrentProfile().pingTimeStampPosition = item[4].ToString();
                    ConfigurationHelper.GetCurrentProfile().pingTimeStampRect = Convert.ToBoolean(item[5]);
                    ConfigurationHelper.GetCurrentProfile().pingStatsStamp = Convert.ToBoolean(item[7]);
                }

                if (item[0].ToString() == "Alert")
                {
                    ConfigurationHelper.GetCurrentProfile().alertTimeStamp = Convert.ToBoolean(item[1]);
                    ConfigurationHelper.GetCurrentProfile().alertTimeStampFormat = item[2].ToString();
                    ConfigurationHelper.GetCurrentProfile().alertTimeStampColour = item[3].ToString();
                    ConfigurationHelper.GetCurrentProfile().alertTimeStampPosition = item[4].ToString();
                    ConfigurationHelper.GetCurrentProfile().alertTimeStampRect = Convert.ToBoolean(item[5]);
                    ConfigurationHelper.GetCurrentProfile().alertStatsStamp = Convert.ToBoolean(item[7]);
                }
            }
        }

        private void timeStampMthOld(ArrayList i)
        {

            if (i[0].ToString() == "Online")
            {
                ConfigurationHelper.GetCurrentProfile().onlineTimeStamp = Convert.ToBoolean(i[1]);
                ConfigurationHelper.GetCurrentProfile().onlineTimeStampFormat = i[2].ToString();
                ConfigurationHelper.GetCurrentProfile().onlineTimeStampColour = i[3].ToString();
                ConfigurationHelper.GetCurrentProfile().onlineTimeStampPosition = i[4].ToString();
                ConfigurationHelper.GetCurrentProfile().onlineTimeStampRect = Convert.ToBoolean(i[5]);
                ConfigurationHelper.GetCurrentProfile().onlineStatsStamp = Convert.ToBoolean(i[6]);
            }

            if (i[0].ToString() == "Publish")
            {
                ConfigurationHelper.GetCurrentProfile().publishTimeStamp = Convert.ToBoolean(i[1]);
                ConfigurationHelper.GetCurrentProfile().publishTimeStampFormat = i[2].ToString();
                ConfigurationHelper.GetCurrentProfile().publishTimeStampColour = i[3].ToString();
                ConfigurationHelper.GetCurrentProfile().publishTimeStampPosition = i[4].ToString();
                ConfigurationHelper.GetCurrentProfile().publishTimeStampRect = Convert.ToBoolean(i[5]);
                ConfigurationHelper.GetCurrentProfile().publishStatsStamp = Convert.ToBoolean(i[6]);
            }

            if (i[0].ToString() == "Ping")
            {
                ConfigurationHelper.GetCurrentProfile().pingTimeStamp = Convert.ToBoolean(i[1]);
                ConfigurationHelper.GetCurrentProfile().pingTimeStampFormat = i[2].ToString();
                ConfigurationHelper.GetCurrentProfile().pingTimeStampColour = i[3].ToString();
                ConfigurationHelper.GetCurrentProfile().pingTimeStampPosition = i[4].ToString();
                ConfigurationHelper.GetCurrentProfile().pingTimeStampRect = Convert.ToBoolean(i[5]);
                ConfigurationHelper.GetCurrentProfile().pingStatsStamp = Convert.ToBoolean(i[6]);
            }

            if (i[0].ToString() == "Alert")
            {
                ConfigurationHelper.GetCurrentProfile().alertTimeStamp = Convert.ToBoolean(i[1]);
                ConfigurationHelper.GetCurrentProfile().alertTimeStampFormat = i[2].ToString();
                ConfigurationHelper.GetCurrentProfile().alertTimeStampColour = i[3].ToString();
                ConfigurationHelper.GetCurrentProfile().alertTimeStampPosition = i[4].ToString();
                ConfigurationHelper.GetCurrentProfile().alertTimeStampRect = Convert.ToBoolean(i[5]);
                ConfigurationHelper.GetCurrentProfile().alertStatsStamp = Convert.ToBoolean(i[6]);
            }
        }

        private void button16_Click_1(object sender, EventArgs e)
        {
            ArrayList i = new ArrayList();

            if (rdAlertjpg.Checked)
            {
                i.Add("Alert");
                i.Add(ConfigurationHelper.GetCurrentProfile().alertCompression);
            }
            if (rdPingjpg.Checked)
            {
                i.Add("Ping");
                i.Add(ConfigurationHelper.GetCurrentProfile().pingCompression);
            }
            if (rdPublishjpg.Checked)
            {
                i.Add("Publish");
                i.Add(ConfigurationHelper.GetCurrentProfile().publishCompression);
            }
            if (rdOnlinejpg.Checked)
            {
                i.Add("Online");
                i.Add(ConfigurationHelper.GetCurrentProfile().onlineCompression);
            }

            i.Add(ConfigurationHelper.GetCurrentProfile().toolTips);
            image image = new image(new formDelegate(jPegSetCompression), i);
            image.StartPosition = FormStartPosition.CenterScreen;
            image.ShowDialog();
        }

        private void button18_Click_1(object sender, EventArgs e)
        {
            List<List<object>> stampList = new List<List<object>>();
            List<object> alertList = new List<object>();
            List<object> pingList = new List<object>();
            List<object> publishList = new List<object>();
            List<object> onlineList = new List<object>();
            alertList.Add("Alert");
            alertList.Add(ConfigurationHelper.GetCurrentProfile().alertTimeStamp);
            alertList.Add(ConfigurationHelper.GetCurrentProfile().alertTimeStampFormat);
            alertList.Add(ConfigurationHelper.GetCurrentProfile().alertTimeStampColour);
            alertList.Add(ConfigurationHelper.GetCurrentProfile().alertTimeStampPosition);
            alertList.Add(ConfigurationHelper.GetCurrentProfile().alertTimeStampRect);
            alertList.Add(false);
            alertList.Add(ConfigurationHelper.GetCurrentProfile().alertStatsStamp);
            pingList.Add("Ping");
            pingList.Add(ConfigurationHelper.GetCurrentProfile().pingTimeStamp);
            pingList.Add(ConfigurationHelper.GetCurrentProfile().pingTimeStampFormat);
            pingList.Add(ConfigurationHelper.GetCurrentProfile().pingTimeStampColour);
            pingList.Add(ConfigurationHelper.GetCurrentProfile().pingTimeStampPosition);
            pingList.Add(ConfigurationHelper.GetCurrentProfile().pingTimeStampRect);
            pingList.Add(true);
            pingList.Add(ConfigurationHelper.GetCurrentProfile().pingStatsStamp);
            publishList.Add("Publish");
            publishList.Add(ConfigurationHelper.GetCurrentProfile().publishTimeStamp);
            publishList.Add(ConfigurationHelper.GetCurrentProfile().publishTimeStampFormat);
            publishList.Add(ConfigurationHelper.GetCurrentProfile().publishTimeStampColour);
            publishList.Add(ConfigurationHelper.GetCurrentProfile().publishTimeStampPosition);
            publishList.Add(ConfigurationHelper.GetCurrentProfile().publishTimeStampRect);
            publishList.Add(true);
            publishList.Add(ConfigurationHelper.GetCurrentProfile().publishStatsStamp);
            onlineList.Add("Online");
            onlineList.Add(ConfigurationHelper.GetCurrentProfile().onlineTimeStamp);
            onlineList.Add(ConfigurationHelper.GetCurrentProfile().onlineTimeStampFormat);
            onlineList.Add(ConfigurationHelper.GetCurrentProfile().onlineTimeStampColour);
            onlineList.Add(ConfigurationHelper.GetCurrentProfile().onlineTimeStampPosition);
            onlineList.Add(ConfigurationHelper.GetCurrentProfile().onlineTimeStampRect);
            onlineList.Add(false);
            onlineList.Add(ConfigurationHelper.GetCurrentProfile().onlineStatsStamp);
            stampList.Add(alertList);
            stampList.Add(pingList);
            stampList.Add(publishList);
            stampList.Add(onlineList);
            timestamp timestamp = new timestamp(new formDelegateList(timeStampMth), stampList);
            timestamp.StartPosition = FormStartPosition.CenterScreen;
            timestamp.ShowDialog();
        }

        private void cameraWindow_DoubleClick(object sender, EventArgs e)
        {
            if (webcamConfig != null && !webcamConfig.DrawMode()) AdjustPanel(); ;
        }

        private void imageInFrame_Click(object sender, EventArgs e)
        {
            AdjustPanel();
        }

        private void AdjustPanel()
        {
            if (ConfigurationHelper.GetCurrentProfile().imageToframe)
            {
                imageInFrame.Image = TeboCam.Properties.Resources.arrowIn;
                ConfigurationHelper.GetCurrentProfile().imageToframe = false;
                cameraWindow.imageToFrame = false;
                panel1.AutoScroll = true;
            }
            else
            {
                imageInFrame.Image = TeboCam.Properties.Resources.arrowOut;
                ConfigurationHelper.GetCurrentProfile().imageToframe = true;
                cameraWindow.imageToFrame = true;
                panel1.AutoScroll = false;
            }
        }

        private void cameraShow_Click(object sender, EventArgs e)
        {
            cameraWindow.showCam = !cameraWindow.showCam;
            if (cameraWindow.showCam)
            {
                cameraShow.Image = TeboCam.Properties.Resources.nolandscape;
                ConfigurationHelper.GetCurrentProfile().cameraShow = true;
            }
            else
            {
                cameraShow.Image = TeboCam.Properties.Resources.landscape;
                ConfigurationHelper.GetCurrentProfile().cameraShow = false;
            }
        }

        private void levelShow_Click(object sender, EventArgs e)
        {
            if (!showLevel)
            {
                showLevel = true;
                levelShow.Image = TeboCam.Properties.Resources.nolevel;
            }
            else
            {
                showLevel = false;
                levelShow.Image = TeboCam.Properties.Resources.level;
                LevelControlBox.levelDraw(0);
            }

            ConfigurationHelper.GetCurrentProfile().motionLevel = showLevel;
            LevelControlBox.Visible = showLevel;
        }

        private static Point FindLocation(Control ctrl)
        {
            Point p;
            for (p = ctrl.Location; ctrl.Parent != null; ctrl = ctrl.Parent)
                p.Offset(ctrl.Parent.Location);
            return p;
        }

        private bool camClick(int button)
        {
            bool canClick = NotConnectedCameras.Any(x => x.id == button && x.CameraButtonState != GroupCameraButton.ButtonState.NotConnected);
            if (!canClick) return false;

            var connected = NotConnectedCameras.Where(x => x.CameraButtonState != GroupCameraButton.ButtonState.NotConnected).ToList();
            connected.ForEach(x => x.CameraButtonIsConnectedAndInactive());
            var newActiveButton = NotConnectedCameras.First(x => x.id == button);
            newActiveButton.CameraButtonIsConnectedAndInactive();
            return true;
        }


        private void cameraSwitch(int button, bool refresh, bool load)
        {
            int camId = CameraRig.idxFromButton(button);

            //ToDo here camButtons.camClick(button) returns false
            if (load || !load && camClick(button))
            {
                if (load || !load && CameraRig.cameraExists(camId))
                {

                    CameraRig.CurrentlyDisplayingCamera = camId;
                    CameraRig.ConnectedCameras[camId].camera.MotionDetector.Reset();

                    cameraWindow.Camera = CameraRig.ConnectedCameras[camId].camera;
                    lblCameraName.SynchronisedInvoke(() => lblCameraName.Visible = ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), CameraRig.ConnectedCameras[camId].cameraName).friendlyName.Trim() != string.Empty);
                    lblCameraName.SynchronisedInvoke(() => lblCameraName.Text = CameraRig.ConnectedCameras[camId].friendlyName);
                    ConfigurationHelper.GetCurrentProfile().selectedCam = CameraRig.ConnectedCameras[camId].cameraName;

                    if (refresh) cameraWindow.Refresh();
                    camButtonSetColours();
                }

            }
        }

        public GroupCameraButton.ButtonState motionSenseClick(int p_bttn)
        {
            if (NotConnectedCameras.Any(x => x.id == p_bttn && x.CameraButtonState != GroupCameraButton.ButtonState.NotConnected))
            {
                if (NotConnectedCameras.Any(x => x.id == p_bttn && x.ActiveButtonState == GroupCameraButton.ButtonState.NotConnected))
                {
                    return GroupCameraButton.ButtonState.NotConnected;
                }
                else
                {
                    return GroupCameraButton.ButtonState.ConnectedAndActive;
                }
            }
            return GroupCameraButton.ButtonState.ConnectedAndInactive;
        }

        private void selcam(int button, bool activateDetection)
        {
            int cam = CameraRig.idxFromButton(button);
            GroupCameraButton.ButtonState activeButtonState = motionSenseClick(button);

            if (activateDetection ||
                (activeButtonState == GroupCameraButton.ButtonState.NotConnected || activeButtonState == GroupCameraButton.ButtonState.ConnectedAndInactive))
            {
                licence.selectCam(cam + 1);
                NotConnectedCameras.First(x => x.id == button).ActiveButtonIsActive();
                CameraRig.ConnectedCameras[cam].camera.alarmActive = true;
                ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), CameraRig.ConnectedCameras[cam].cameraName).alarmActive = true;
                CameraRig.ConnectedCameras[cam].camera.detectionOn = true;
                camButtonSetColours();
                return;
            }

            if (activeButtonState == GroupCameraButton.ButtonState.ConnectedAndActive)
            {
                licence.deselectCam(cam + 1);
                NotConnectedCameras.First(x => x.id == button).ActiveButtonIsInactive();
                CameraRig.ConnectedCameras[cam].camera.alarmActive = false;
                ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), CameraRig.ConnectedCameras[cam].cameraName).alarmActive = false;
                CameraRig.ConnectedCameras[cam].camera.detectionOn = false;
                camButtonSetColours();
                return;
            }
            camButtonSetColours();
        }

        private void publishRefresh(int button)
        {
            int pubButton = CameraRig.idxFromButton(button);
            var record = ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), CameraRig.ConnectedCameras[pubButton].cameraName);
            publishSettings.SetPubTime(record.pubTime.ToString());
            publishSettings.SetPubHours(record.pubHours);
            publishSettings.SetPubMins(record.pubMins);
            publishSettings.SetPubSecs(record.pubSecs);
            publishSettings.SetPubToWeb(record.publishWeb);
            publishSettings.SetPubToLocal(record.publishLocal);
            publishSettings.SetPubTimerOn(record.timerOn);
        }

        private void pubcam(int button)
        {
            if (NotConnectedCameras.Where(x => x.id == button && x.CameraButtonState != GroupCameraButton.ButtonState.NotConnected).Count() > 0)
            {
                int cam = CameraRig.idxFromButton(button);
                PublishButtonGroupInstance.ForEach(x => x.CameraButtonIsNotConnected());
                PublishButtonGroupInstance.Where(x => x.id == button).First().CameraButtonIsActive();

                foreach (var item in PublishButtonGroupInstance)
                {
                    if (item.id == button)
                    {
                        item.CameraButtonIsActive();
                    }
                }

                foreach (ConnectedCamera item in CameraRig.ConnectedCameras)
                {
                    if (item.displayButton != button)
                    {
                        item.camera.publishActive = false;
                        ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), item.cameraName).publishActive = false;
                    }
                }

                bool currentlyPublishing = PublishButtonGroupInstance.Any(x => x.id == button && x.CameraButtonState == GroupCameraButton.ButtonState.ConnectedAndActive);

                if (!currentlyPublishing)
                {
                    PublishButtonGroupInstance.First(x => x.id == button && x.CameraButtonState == GroupCameraButton.ButtonState.ConnectedAndActive).CameraButtonIsConnectedAndInactive();
                    CameraRig.ConnectedCameras[cam].camera.publishActive = false;
                    ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), CameraRig.ConnectedCameras[cam].cameraName).publishActive = false;
                }
                else
                {
                    PublishButtonGroupInstance.First(x => x.id == button && x.CameraButtonState == GroupCameraButton.ButtonState.ConnectedAndActive).CameraButtonIsActive();
                    CameraRig.ConnectedCameras[cam].camera.publishActive = true;
                    ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), CameraRig.ConnectedCameras[cam].cameraName).publishActive = true;
                }

                camButtonSetColours();
                publishRefresh(button);
            }
        }

        private void camButtonSetColours()
        {
            foreach (var buttonGroup in NotConnectedCameras)
            {
                //display camera buttons
                if (buttonGroup.id == CameraRig.ConnectedCameras[CameraRig.CurrentlyDisplayingCamera].displayButton)
                {
                    buttonGroup.CameraButtonIsActive();
                }
                else
                {
                    if (CameraRig.CameraIsConnectedToButton(buttonGroup.id))
                    {
                        buttonGroup.CameraButtonIsConnectedAndInactive();
                    }
                    else
                    {
                        buttonGroup.CameraButtonIsNotConnected();
                    }
                }

                //activate motion detection camera buttons
                var detectionOn = CameraRig.ConnectedCameras.Any(x => x.displayButton == buttonGroup.id && x.camera.alarmActive);
                if (detectionOn)
                {
                    buttonGroup.ActiveButtonIsActive();
                }
                else
                {
                    buttonGroup.ActiveButtonIsInactive();
                }
            }
        }

        private void camReset()
        {
            NotConnectedCameras.ForEach(x => x.CameraButtonIsNotConnected());
        }

        private void bttnCamProp_Click(object sender, EventArgs e)
        {
            VideoCaptureDevice localSource = new VideoCaptureDevice(CameraRig.ConnectedCameras[CameraRig.CurrentlyDisplayingCamera].cameraName);
            localSource.DisplayPropertyPage(IntPtr.Zero); // non-modal
        }

        private void button23_Click_1(object sender, EventArgs e)
        {
            ArrayList i = new ArrayList();
            i.Add(ConfigurationHelper.GetCurrentProfile().toolTips);
            i.Add(CameraRig.CurrentlyDisplayingCamera);
            i.Add(panel1.AutoScroll);
            //i.Add(camButtons.buttons());
            Movement.motionLevelChanged -= new EventHandler(drawLevel);
            LevelControlBox.levelDraw(0);
            webcamConfig = new webcamConfig(new formDelegate(webcamConfigCompleted),
                                                         i,
                                                         NotConnectedCameras,
                                                         saveChanges,
                                                         publisher,
                                                         pinger);
            webcamConfig.StartPosition = FormStartPosition.CenterScreen;
            webcamConfig.ShowDialog();
        }

        private void webcamConfigCompleted(ArrayList i)
        {
            Movement.motionLevelChanged -= new EventHandler(drawLevel);
            Movement.motionLevelChanged += new EventHandler(drawLevel);

            if (CameraRig.cameraCount() > 0)
            {
                //give the interface some time to refresh
                Thread.Sleep(250);
                //give the interface some time to refresh
                cameraSwitch(CameraRig.ConnectedCameras[CameraRig.ConfigCam].displayButton, true, true);
            }

            panel1.AutoScroll = (bool)i[1];
            i.Clear();
            Movement.motionLevelChanged -= new EventHandler(drawLevel);
            Movement.motionLevelChanged += new EventHandler(drawLevel);
            camButtonSetColours();
        }

        public void cameraNewProfile()
        {
            closeAllCameras();
            CameraRig.clear();
            camReset();

            //check if cw is null as we may currently be loading the form 
            //and cw may be in progress
            if (ConfigurationHelper.GetCurrentProfile().webcam != null && cw == null)
            {
                BackgroundWorker profChWorker = new BackgroundWorker();
                profChWorker.DoWork -= new DoWorkEventHandler(waitForCamera.wait);
                profChWorker.DoWork += new DoWorkEventHandler(waitForCamera.wait);
                profChWorker.WorkerSupportsCancellation = true;
                profChWorker.RunWorkerAsync();
                profChWorker = null;
            }
        }

        private void filePrefixSet(FilePrefixSettingsResultDto result)
        {
            if (new List<string>() { "Publish Web", "Publish Local" }.Contains(result.FromString))
            {
                int pubButton = CameraRig.idxFromButton(PublishButtonGroupInstance.First(x => x.CameraButtonState == GroupCameraButton.ButtonState.ConnectedAndActive).id);
                ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), CameraRig.ConnectedCameras[pubButton].cameraName).publishFirst = true;

                if (result.FromString == "Publish Web")
                {
                    var record = ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), CameraRig.ConnectedCameras[pubButton].cameraName);
                    record.filenamePrefixPubWeb = result.FilenamePrefix;
                    record.cycleStampCheckedPubWeb = result.CycleStamp;
                    record.startCyclePubWeb = Convert.ToInt32(result.StartCycle);
                    record.endCyclePubWeb = Convert.ToInt32(result.EndCycle);
                    record.currentCyclePubWeb = Convert.ToInt32(result.CurrentCycle);
                    record.publishFirst = result.AppendToFilename;
                }

                if (result.FromString == "Publish Local")
                {
                    var record = ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), CameraRig.ConnectedCameras[pubButton].cameraName);
                    record.filenamePrefixPubLoc = result.FilenamePrefix;
                    record.cycleStampCheckedPubLoc = result.CycleStamp;
                    record.startCyclePubLoc = Convert.ToInt32(result.StartCycle);
                    record.endCyclePubLoc = Convert.ToInt32(result.EndCycle);
                    record.currentCyclePubLoc = Convert.ToInt32(result.CurrentCycle);
                    record.stampAppendPubLoc = result.AppendToFilename;
                    record.fileDirPubLoc = result.FileLoc;
                    record.fileDirPubCust = result.CustomLocation;
                }
            }

            if (result.FromString == "Alert")
            {
                var record = ConfigurationHelper.GetCurrentProfile();
                record.filenamePrefix = result.FilenamePrefix;
                record.cycleStampChecked = result.CycleStamp;
                record.startCycle = Convert.ToInt32(result.StartCycle);
                record.endCycle = Convert.ToInt32(result.EndCycle);
                record.currentCycle = Convert.ToInt32(result.CurrentCycle);
                generateWebpageSettings.GetLblImgPref().Text = $"Image Prefix: {result.FilenamePrefix}   e.g {result.FilenamePrefix}1{TebocamState.ImgSuffix}";
            }
        }

        private void scheduleSet(ArrayList i)
        {

            if (i[0].ToString() == "Publish")
            {
                ConfigurationHelper.GetCurrentProfile().timerStartPub = i[1].ToString();
                ConfigurationHelper.GetCurrentProfile().timerEndPub = i[2].ToString();
                publishSettings.SetLblstartpub($"Scheduled start: {LeftRightMid.Left(i[1].ToString(), 2)}:{LeftRightMid.Right(i[1].ToString(), 2)}");
                publishSettings.SetLblendpub($"Scheduled end: {LeftRightMid.Left(i[2].ToString(), 2)}:{LeftRightMid.Right(i[2].ToString(), 2)}");
                Invalidate();
            }

            if (i[0].ToString() == "Alert")
            {
                ConfigurationHelper.GetCurrentProfile().timerStartMov = i[1].ToString();
                ConfigurationHelper.GetCurrentProfile().timerEndMov = i[2].ToString();
                motionAlarmSettings.GetLblstartmov().Text = $"Start: {LeftRightMid.Left(i[1].ToString(), 2)}:{LeftRightMid.Right(i[1].ToString(), 2)}";
                motionAlarmSettings.GetLblendmov().Text = $"End: {LeftRightMid.Left(i[2].ToString(), 2)}:{LeftRightMid.Right(i[2].ToString(), 2)}";
                Invalidate();
            }
        }

        private void bttnUpdateFooter_Click(object sender, EventArgs e)
        {
            updateOptionsSettings.TriggerUpdate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<Camera> cameras = new List<Camera>();
            CameraRig.ConnectedCameras.ForEach(x => cameras.Add(x.camera));
            ControlRoomCntl controlRoom = new ControlRoomCntl(cameras, pinger);
            Form controlRoomForm = new Form()
            {
                FormBorderStyle = FormBorderStyle.FixedSingle,
                MinimizeBox = false,
                MaximizeBox = false,
                Size = new Size(controlRoom.Width + 10, controlRoom.Height + 30)
            };
            controlRoomForm.Controls.Add(controlRoom);
            controlRoomForm.ShowDialog();
        }

        public void AdminControl()
        {
            if (!devMachine) return;
            var adm = new AdminCntl(this);
            var border = 50;
            Form frm = new Form()
            {
                Width = adm.Width + border,
                Height = adm.Height + border,
                FormBorderStyle = FormBorderStyle.FixedSingle
            };
            frm.Controls.Add(adm);
            frm.Show();
        }

        private void btnMotionImage_Click(object sender, EventArgs e)
        {
            cameraWindow.MotionDisplay = !cameraWindow.MotionDisplay;
        }

        private void btnTestAccessCameras_Click(object sender, EventArgs e)
        {
            var test = CameraRig.ConnectedCameras;
        }

        private void btnCamInfo_Click(object sender, EventArgs e)
        {
            int camId = CameraRig.CurrentlyDisplayingCamera;
            Camera cam = CameraRig.ConnectedCameras[camId].camera;
            List<string> info = new List<string>() {string.Format("cam.alarmActive: {0}", cam.alarmActive),
                                                    string.Format("cam.alert: {0}", cam.alert),
                                                    string.Format("cam.detectionOn: {0}", cam.detectionOn),
                                                    string.Format("cam.movementVal: {0}", cam.movementVal)};
            LogInformatioRequest(string.Format("cam: {0}", camId), info);
        }

        private void LogInformatioRequest(string area, List<string> info)
        {
            TebocamState.log.AddLine($"******{area}******info request");
            foreach (var inf in info)
            {
                TebocamState.log.AddLine(inf);
            }
            TebocamState.log.AddLine($"******{area}******info request");
        }

        private void btn_TestUpdate_Click(object sender, EventArgs e)
        {
            sensitiveInfo.ver = "1";
        }


    }

    public class ListArgs : EventArgs
    {
        public List<object> _list;

        public List<object> list
        {
            get { return _list; }
            set { _list = value; }
        }
    }

    class scheduleClass
    {
        public enum scheduleAction
        {
            no_action,
            start,
            end
        };
    }
}
