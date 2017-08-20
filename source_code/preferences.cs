using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using teboweb;
//using VideoSource;

using Tiger.Video.VFW;
using System.Threading;
using System.IO;
using System.Diagnostics;
using TeboWeb;
using System.Linq;
//using dshow;
//using dshow.Core;

using System.Net;
using System.Net.NetworkInformation;
//using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Vision.Motion;



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


    public partial class preferences : Form
    {


        //http://msdn.microsoft.com/en-us/library/aa984408(v=vs.71).aspx
        System.Resources.ResourceManager resourceManager;

        public List<CameraButtonGroup> CameraButtonGroupInstance = new List<CameraButtonGroup>();
        public List<CameraButtonGroup> PublishButtonGroupInstance = new List<CameraButtonGroup>();
        public CameraButtonsCntl ButtonCameraControl = new CameraButtonsCntl();
        public CameraButtonsCntl ButtonPublishControl = new CameraButtonsCntl();

        public Pulse pulse;

        public configApplication configInfo = new configApplication(new crypt());

        public static event EventHandler pulseEvent;
        public static event EventHandler pulseStopEvent;
        public static event EventHandler pulseStartEvent;

        public static event EventHandler publishSwitch;

        public static event ListPubEventHandler statusUpdate;

        public static System.Drawing.Bitmap myBitmap;
        //public static System.Drawing.Bitmap levelBitmap;

        private ArrayList lineXpos = new ArrayList();

        public Point CurrentTopLeft = new Point();
        public Point CurrentBottomRight = new Point();
        public int RectangleHeight = new int();
        public int RectangleWidth = new int();


        static BackgroundWorker bw = new BackgroundWorker();
        static BackgroundWorker cw = new BackgroundWorker();
        //static BackgroundWorker ew = new BackgroundWorker();
        static BackgroundWorker worker = new BackgroundWorker();

        private bool showLevel = false;

        private int secondsToTrainStart;

        // statistics
        private const int statLength = 15;
        private int statIndex = 0;
        private int[] statCount = new int[statLength];

        private int intervalsToSave = 0;


        private AVIWriter writer = null;
        private bool saveOnMotion = false;

        public bool checkForMotion = false;

        public int frameCount;
        public int framePrevious = 0;

        private LevelControl LevelControlBox = new LevelControl();

        private FilterInfoCollection filters;

        private Graph graph = new Graph();
        private Log log = new Log();
        private Configuration configuration = new Configuration();

        [STAThread]

        static void Main()
        {
            Application.Run(new preferences());
        }

        public preferences()
        {

            InitializeComponent();

            resourceManager = new System.Resources.ResourceManager("tebocam.preferences", this.GetType().Assembly);

        }

        private void testAtStart()
        {

            config.addProfile();
            configApplication data = config.getProfile("main");

        }


        private void workerProcess(object sender, DoWorkEventArgs e)
        {

            pulseEvent -= new EventHandler(pulseProcess);
            pulseEvent += new EventHandler(pulseProcess);
            bubble.pulseEvent -= new EventHandler(pulseProcess);
            bubble.pulseEvent += new EventHandler(pulseProcess);

            pulseStopEvent -= new EventHandler(pulseStop);
            pulseStopEvent += new EventHandler(pulseStop);
            pulseStartEvent -= new EventHandler(pulseStart);
            pulseStartEvent += new EventHandler(pulseStart);

            pulseEvent(null, new EventArgs());

            bubble.pingLast = time.secondsSinceStart();

            //bubble.logAddLine("Work process started.");
            //bubble.logAddLine("KeepWorking value: " + bubble.keepWorking.ToString());

            teboDebug.filePath = bubble.logFolder;
            teboDebug.fileName = "debug.txt";//string.Format("debug_{0}.txt", DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture));

            //teboDebug.debugToFile = true;
            teboDebug.openFile();

            teboDebug.writeline("workerProcess starting");


            tabControl1.TabPages[6].Controls.Add(ButtonCameraControl);
            ButtonCameraControl.Location = new Point(431, 11);
            tabControl1.TabPages[6].Controls.Add(ButtonPublishControl);
            ButtonPublishControl.Location = new Point(ButtonCameraControl.Location.X, ButtonCameraControl.Location.Y + 100);


            for (int i = 0; i < 9; i++)
            {
                ButtonCameraControl.AddButton(CameraButtonGroupInstance, ButtonCameraDelegation, ButtonActiveDelegation, true);
                ButtonPublishControl.AddButton(PublishButtonGroupInstance, ButtonPublishDelegation, ButtonActiveDelegation, false);
            }


            //teboDebug.debugOn = true;

            long keepWorkingSequence = 0;

            while (bubble.keepWorking)
            {
                try
                {

                    if (keepWorkingSequence > 1000) keepWorkingSequence = 0;
                    keepWorkingSequence++;

                    pulseEvent(null, new EventArgs());
                    bubble.changeTheTime();

                    teboDebug.writeline("workerProcess calling scheduler()");
                    scheduler();
                    teboDebug.writeline("workerProcess calling movementAddImages");
                    bubble.movementAddImages();
                    teboDebug.writeline("workerProcess calling publishImage");
                    bubble.publishImage();
                    teboDebug.writeline("workerProcess calling webUpdate");
                    bubble.webUpdate();
                    teboDebug.writeline("workerProcess calling movementPublish");
                    bubble.movementPublish();

                    //we only really want to do the followin every ten passes through this loop
                    if (keepWorkingSequence % 5 == 0) teboDebug.writeline("workerProcess calling ping");
                    if (keepWorkingSequence % 5 == 0) bubble.ping();
                    if (keepWorkingSequence % 10 == 0) teboDebug.writeline("workerProcess calling connectCamerasMissingAtStartup()");
                    if (keepWorkingSequence % 10 == 0) connectCamerasMissingAtStartup();
                    if (keepWorkingSequence % 10 == 0) teboDebug.writeline("workerProcess calling frameRate");
                    if (keepWorkingSequence % 10 == 0) frameRate();
                    if (keepWorkingSequence % 10 == 0) teboDebug.writeline("workerProcess calling cameraReconnectIfLost()");
                    if (keepWorkingSequence % 10 == 0) reconnectLostCameras();


                    teboDebug.writeline("workerProcess sleeping");
                    Thread.Sleep(250);

                }
                catch { }
            }

            e.Cancel = true;

        }

        private void ButtonCameraDelegation(int id, Button cameraButton, Button activeButton)
        {
            cameraSwitch(id, true, false);
        }
        private void ButtonPublishDelegation(int id, Button cameraButton, Button activeButton)
        {
            pubcam(cameraButton, id);
        }

        private void ButtonActiveDelegation(int id, Button cameraButton, Button activeButton)
        {

        }

        private void filesInit()
        {

            if (!File.Exists(bubble.xmlFolder + "GraphData.xml"))
            {
                //FileManager.WriteFile("graphInit"); #FIX
                //FileManager.backupFile("graph");#FIX
                new Graph().WriteXMLFile(bubble.xmlFolder + "GraphData.xml", graph);
                new Graph().WriteXMLFile(bubble.xmlFolder + "GraphData.bak", graph);
            }

            if (!File.Exists(bubble.xmlFolder + "LogData" + ".xml"))
            {
                //FileManager.WriteFile("logInit");
                //FileManager.backupFile("log");
                new Log().WriteXMLFile(bubble.xmlFolder + "LogData" + ".xml", log);
                new Log().WriteXMLFile(bubble.xmlFolder + "LogData" + ".bak", log);
            }



            //if the old style config file exists read it otherwise deserialise file into class and delete the old config file
            FileManager.ConvertOldProfileIfExists(configuration);

            if (!File.Exists(bubble.xmlFolder + FileManager.configFile + ".xml"))
            {
                //bubble.configDataInit();
                //FileManager.WriteFile("config");
                //FileManager.backupFile("config");
                //config.WebcamSettingsConfigDataPopulate();
                new Configuration().WriteXMLFile(bubble.xmlFolder + FileManager.configFile + ".xml", configuration);
                new Configuration().WriteXMLFile(bubble.xmlFolder + FileManager.configFile + ".bak", configuration);

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
                    //System.Diagnostics.Debug.WriteLine(arg);

                    //profile must not contain any spaces within it
                    //second time through pick up the profile to use
                    if (profile && config.profileExists(commandLine)) { bubble.profileInUse = commandLine; }
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
                        bubble.pulseRestart = true;
                    }
                    if (commandLine == "/close")
                    {
                        result = enumCommandLine.close;
                        return result;
                    }

                    //if (restart && commandLine == "active") activate = true; ;
                    //if (restart && commandLine == "inactive") activate = false;

                    if (activate || profile)
                    {
                        config.getProfile(bubble.profileInUse).countdownNow = false;
                        config.getProfile(bubble.profileInUse).countdownTime = false;
                    }

                    if (activate && commandLine == "time") { time = true; }
                    if (activate && commandLine == "seconds") { seconds = true; }
                    if (activate && commandLine == "now") { config.getProfile(bubble.profileInUse).countdownNow = true; }

                    if (time && commandLine != "time")
                    {
                        config.getProfile(bubble.profileInUse).activatecountdownTime = commandLine;
                        config.getProfile(bubble.profileInUse).countdownTime = true;
                        numericUpDown1.Value = Convert.ToDecimal(LeftRightMid.Left(commandLine, 2));
                        numericUpDown2.Value = Convert.ToDecimal(LeftRightMid.Right(commandLine, 2));
                        bttnNow.Checked = false;
                        bttnTime.Checked = true;
                        bttnSeconds.Checked = false;
                    }

                    if (seconds && commandLine != "seconds")
                    {
                        config.getProfile(bubble.profileInUse).activatecountdown = Convert.ToInt32(commandLine);
                        actCountdown.Text = commandLine;
                        bttnNow.Checked = false;
                        bttnTime.Checked = false;
                        bttnSeconds.Checked = true;
                    }

                    if (restart && commandLine == "active")
                    {


                        config.getProfile(bubble.profileInUse).activatecountdown = 30;
                        actCountdown.Text = "30";
                        bttnNow.Checked = false;
                        bttnTime.Checked = false;
                        bttnSeconds.Checked = true;
                        activate = true;

                    }

                    if ((activate && (commandLine == "now" || commandLine == "activate")))
                    {

                        bttnNow.Checked = true;
                        bttnTime.Checked = false;
                        bttnSeconds.Checked = false;

                    }




                }

            }

            if (activate)
            {
                config.getProfile(bubble.profileInUse).AlertOnStartup = true;
                bttnMotionActive.Checked = true;
            }

            return result;

        }


        private void preferences_Load(object sender, EventArgs e)
        {

            installationClean();

            LevelControlBox.Left = 6;
            LevelControlBox.Top = 35;
            this.Webcam.Controls.Add(LevelControlBox);

            CameraRig.camSelInit();

            publishCams publishCams = new publishCams(9);


            bubble.devMachine = File.Exists(Application.StartupPath + bubble.devMachineFile);
            bubble.databaseConnect = File.Exists(Application.StartupPath + bubble.dbaseConnectFile);

            if (bubble.devMachine)
            {

                bttInstallUpdateAdmin.Visible = true;
                bttnUpdateFooter.Visible = true;

            }
            else
            {
                tabControl1.TabPages.Remove(Test);
            }


            //ImageDisplayer displayer = new ImageDisplayer(Test.Width - 20, Test.Height - 20, 100, 100);
            //Test.Controls.Add(displayer);
            //displayer.AddImage(@"C:\Users\Guy\Desktop\temp\testimages\g_1.jpg", 50);
            //displayer.AddImage(@"C:\Users\Guy\Desktop\temp\testimages\g_2.jpg", 50);
            //displayer.AddImage(@"C:\Users\Guy\Desktop\temp\testimages\g_3.jpg", 50);
            //displayer.AddImage(@"C:\Users\Guy\Desktop\temp\testimages\g_4.jpg", 50);
            //displayer.AddImage(@"C:\Users\Guy\Desktop\temp\testimages\g_5.jpg", 50);
            //displayer.AddImage(@"C:\Users\Guy\Desktop\temp\testimages\g_6.jpg", 50);





            if (!bubble.databaseConnect) tabControl1.TabPages.Remove(Online); ;

            //updaterUpdate();
            update.updateMe(bubble.updaterPrefix, Application.StartupPath + @"\");

            ThumbsPrepare();

            bttInstallUpdateAdmin.Visible = false;
            bttnUpdateFooter.Visible = false;

            bubble.Loading = true;


            statusUpdate += new ListPubEventHandler(statusBarUpdate);
            bubble.LogAdded += new EventHandler(log_add);
            bubble.TimeChange += new EventHandler(time_change);
            publishSwitch += new EventHandler(publish_switch);

            bubble.redrawGraph += new EventHandler(drawGraph);
            bubble.pingGraph += new EventHandler(drawGraphPing);
            bubble.motionLevelChanged += new EventHandler(drawLevel);

            bubble.motionDetectionActivate += new EventHandler(motionDetectionActivate);
            bubble.motionDetectionInactivate += new EventHandler(motionDetectionInactivate);

            filesInit();


            try
            {
                configuration = new Configuration().ReadXMLFile(bubble.xmlFolder + FileManager.configFile + ".xml");
                configuration.WriteXMLFile(bubble.xmlFolder + FileManager.configFile + ".bak", configuration);
            }
            catch (Exception)
            {
                //#fix this will need uncommenting when data is read from serialised class
                configuration = new Configuration().ReadXMLFile(bubble.xmlFolder + FileManager.configFile + ".bak");
            }

            if (configuration.appConfigs.Count == 0)
            {
                configApplication config = new configApplication();
                configuration.appConfigs.Add(config);
            }


            bubble.configuration = configuration;
            bubble.profileInUse = configuration.profileInUse;
            bubble.mysqlDriver = configuration.mysqlDriver;
            bubble.newsSeq = configuration.newsSeq;

            //#todo fails here
            //            if (FileManager.readXmlFile("config", false))
            //            {
            //                FileManager.backupFile("config");
            //            }
            //            else
            //            {
            //                FileManager.readXmlFile("config", true);
            //            }


            //if (FileManager.readXmlFile("log", false))
            //{
            //    FileManager.backupFile("log");
            //}
            //else
            //{
            //    FileManager.readXmlFile("log", true);
            //}

            try
            {
                log = new Log().ReadXMLFile(bubble.xmlFolder + "LogData" + ".xml");
                log.WriteXMLFile(bubble.xmlFolder + "LogData" + ".bak", log);
            }
            catch (Exception)
            {
                log = new Log().ReadXMLFile(bubble.xmlFolder + "LogData" + ".bak");
            }

            bubble.log = log;

            config.WebcamSettingsPopulate();
            profileListRefresh(bubble.profileInUse);

            bubble.connectedToInternet = bubble.internetConnected(config.getProfile(bubble.profileInUse).internetCheck);
            notConnected.Visible = !bubble.connectedToInternet;

            //Apply command line values
            enumCommandLine commlineResults = commandLine();
            pnlStartupOptions.Visible = commlineResults <= enumCommandLine.alert;

            if (commlineResults == enumCommandLine.close)
            {

                CloseAllTeboCamPocesses();
                Close();
                return;


            }

            //if (FileManager.readXmlFile("graph", false))
            //{
            //    FileManager.backupFile("graph");
            //}
            //else
            //{
            //    FileManager.readXmlFile("graph", true);
            //}

            //graph = new Graph();

            try
            {
                graph = new Graph().ReadXMLFile(bubble.xmlFolder + "GraphData.xml");
                graph.WriteXMLFile(bubble.xmlFolder + "GraphData.bak", graph);
            }
            catch (Exception)
            {
                graph = new Graph().ReadXMLFile(bubble.xmlFolder + "GraphData.bak");
            }

            bubble.graph = graph;



            //clear out thumb nail images
            FileManager.clearFiles(bubble.thumbFolder);

            //levelDraw(0);
            LevelControlBox.levelDraw(0);

            bubble.moveStatsInitialise();
            graph.updateGraphHist(time.currentDate(), bubble.moveStats);


            if (!captureMovementImages.Checked)
            {

                graphDate(DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture), "Image capture switched off");

            }
            else
            {

                drawGraph(this, null);

            }




            bubble.logAddLine("Starting TeboCam");
            FileManager.clearLog();

            if (config.getProfile(bubble.profileInUse).webcam != null)
            {
                cw.DoWork -= new DoWorkEventHandler(waitForCam);
                cw.DoWork += new DoWorkEventHandler(waitForCam);
                cw.WorkerSupportsCancellation = true;
                cw.RunWorkerAsync();
            }

            bubble.keepWorking = true;
            bubble.workInit(true);

            string test = time.currentTime();

            bubble.Loading = false;

            lblCurVer.Text += bubble.version;

            List<string> updateDat = new List<string>();

            //#if !DEBUG

            updateDat = check_for_updates();

            //#endif
            string onlineVersion = Double.Parse(updateDat[1], new System.Globalization.CultureInfo("en-GB")).ToString();


            if (decimal.Parse(onlineVersion) == 0)
            { lblVerAvail.Text = "Unable to determine the most up-to-date version."; }
            else
            {
                if (decimal.Parse(bubble.version) >= decimal.Parse(onlineVersion))
                { lblVerAvail.Text = "You have the most up-to-date version."; }
                else
                {
                    lblVerAvail.Text = "Most recent version available: " + onlineVersion;
                    bttInstallUpdateAdmin.Visible = true;
                    bttnUpdateFooter.Visible = true;
                }

                bubble.upd_url = updateDat[2];
                bubble.upd_file = updateDat[3];



            }



            //pass the version of the update available to statusUpdate
            ListArgs a = new ListArgs();
            List<object> b = new List<object>();
            b.Add(onlineVersion);
            a.list = b;
            statusUpdate(null, a);


            if (config.getProfile(bubble.profileInUse).logsKeepChk) clearOldLogs();


            if (!config.getProfile(bubble.profileInUse).AlertOnStartup && config.getProfile(bubble.profileInUse).updatesNotify
                && bubble.connectedToInternet
                && Convert.ToDecimal(onlineVersion) > Convert.ToDecimal(bubble.version)
                && !config.getProfile(bubble.profileInUse).startTeboCamMinimized
                              )
            {
                string tmpStr = "";


                //Name Spaces Required
                ////http://msdn.microsoft.com/en-us/library/aa984408(v=vs.71).aspx
                //System.Resources.ResourceManager resourceManager = new System.Resources.ResourceManager("tebocam.preferences", this.GetType().Assembly);
                //tmpStr = resourceManager.GetString("updateAvailableMessage");

                //You do not have the most recent version available.


                //The most recent version can installed automatically
                //by clicking on the update button at the bottom of the screen 
                //or on the Admin tab.

                //To stop this message appearing in future uncheck the 
                //'Notify when updates are available' box in the Admin tab.


                tmpStr = "You do not have the most recent version available" + Environment.NewLine + Environment.NewLine;
                tmpStr += "This version: " + bubble.version + Environment.NewLine;
                tmpStr += "Most recent version available: " + onlineVersion + Environment.NewLine + Environment.NewLine;
                tmpStr += "The most recent version can be installed automatically" + Environment.NewLine;
                tmpStr += "by clicking on the update button at the bottom of the screen or on the Admin tab" + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                tmpStr += "To stop this message appearing in future - uncheck the" + Environment.NewLine;
                tmpStr += "'Notify when updates are available' box in the Admin tab.";
                MessageBox.Show(tmpStr, "Update Available");

            }

            plSnd.Enabled = config.getProfile(bubble.profileInUse).soundAlert != "";

            if (config.getProfile(bubble.profileInUse).freezeGuard)
            {

                decimal intervalRatio = 1m / 0.75m;//gives a result of 1.33333...
                string restartCommand = bttnMotionActive.Checked ? "restart active" : "restart inactive";

                decimal checkInterval = config.getProfile(bubble.profileInUse).pulseFreq / intervalRatio;

                pulse = new Pulse(config.getProfile(bubble.profileInUse).pulseFreq,//1m,
                                  checkInterval,// 0.75m,
                                  bubble.tmpFolder,
                                  "pulse.xml",
                                  bubble.processToEnd,
                                  Application.StartupPath + @"\TeboCam.exe",
                                  bubble.pulseApp,
                                  bubble.logFolder,
                                  restartCommand,
                                  bubble.pulseRestart);

            }

            cw = null;

            worker.WorkerSupportsCancellation = true;
            worker.DoWork -= new DoWorkEventHandler(workerProcess);
            worker.DoWork += new DoWorkEventHandler(workerProcess);
            worker.RunWorkerAsync();

            this.Enabled = false;

            if (bubble.lockdown)
            {

                while (1 == 1)
                {

                    if (Prompt.ShowDialog("Password", "Enter password to unlock") == config.getProfile(bubble.profileInUse).lockdownPassword)
                    {

                        this.Enabled = true;
                        break;

                    }


                }

            }

            this.Enabled = true;

        }


        private void CloseAllTeboCamPocesses()
        {

            int myProcessID = Process.GetCurrentProcess().Id;
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                if (process.ProcessName == bubble.processToEnd && process.Id != myProcessID) process.Kill();
            }
        }


        private void preferences_Loaded(object sender, EventArgs e)
        {

            if (config.getProfile(bubble.profileInUse).startTeboCamMinimized)
            {
                WindowState = FormWindowState.Minimized;
                Hide();
            }

        }


        private void pulseProcess(object sender, System.EventArgs e)
        {
            if (config.getProfile(bubble.profileInUse).freezeGuard) pulse.Beat(bttnMotionActive.Checked ? "restart active" : "restart inactive");
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

            if (Convert.ToDecimal(e._list[0]) > Convert.ToDecimal(bubble.version))
            {
                statusStrip.BackColor = Color.LemonChiffon;
                StatusStripLabel.ForeColor = Color.Black;
                StripStatusLabel.Text = "TeboCam - Version " + bubble.version + " - TeboWeb " + bubble.versionDt + " :::: Most recent version " + e._list[0].ToString() + " available as auto-install";

            }
            else
            {
                statusStrip.BackColor = System.Drawing.SystemColors.Control;
                StatusStripLabel.ForeColor = Color.Black;
                StripStatusLabel.Text = "TeboCam - Version " + bubble.version + " - Copyright TeboWeb " + bubble.versionDt;
            }
        }



        private void waitForCam(object sender, DoWorkEventArgs e)
        {

            bool nocam;
            List<cameraSpecificInfo> expectedCameras = CameraRig.cameraCredentialsListedUnderProfile(bubble.profileInUse);

            //*****************************
            //IP Webcams
            //*****************************

            //find if any webcams are present
            for (int i = 0; i < expectedCameras.Count; i++)
            {
                //we have an ip webcam in the profile
                if (expectedCameras[i].ipWebcamAddress != string.Empty)
                {
                    IPAddress parsedIPAddress;
                    Uri parsedUri;
                    //check that the url resolves
                    if (Uri.TryCreate(expectedCameras[i].ipWebcamAddress, UriKind.Absolute, out parsedUri) && IPAddress.TryParse(parsedUri.DnsSafeHost, out parsedIPAddress))
                    {
                        Ping pingSender = new Ping();
                        PingReply reply = pingSender.Send(parsedIPAddress);
                        //is ip webcam running?
                        if (reply.Status == IPStatus.Success)
                        {
                            AForge.Video.MJPEGStream stream = new AForge.Video.MJPEGStream(expectedCameras[i].ipWebcamAddress);

                            if (expectedCameras[i].ipWebcamUser != string.Empty)
                            {
                                stream.Login = expectedCameras[i].ipWebcamUser;
                                stream.Password = expectedCameras[i].ipWebcamPassword;
                            }

                            Camera cam = OpenVideoSource(null, stream, true, -1);
                            cam.frameRateTrack = config.getProfile(bubble.profileInUse).framerateTrack;

                        }
                    }
                }
            }

            //*****************************
            //IP Webcams
            //*****************************




            //*****************************
            //USB Webcams
            //*****************************

            nocam = false;

            try
            {
                filters = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (filters.Count == 0) nocam = true;
            }
            catch (ApplicationException)
            {
                nocam = true;
            }


            //we have camera(s) attached so let's connect it/them
            if (!nocam)
            {
                for (int i = 0; i < filters.Count; i++)
                {
                    for (int c = 0; c < expectedCameras.Count; c++)
                    {
                        if (expectedCameras[c].ipWebcamAddress == string.Empty && filters[i].MonikerString == expectedCameras[c].webcam)
                        {
                            Thread.Sleep(1000);
                            VideoCaptureDevice localSource = new VideoCaptureDevice(expectedCameras[c].webcam);
                            Camera cam = OpenVideoSource(localSource, null, false, -1);
                            cam.frameRateTrack = config.getProfile(bubble.profileInUse).framerateTrack;
                        }
                    }
                }
            }

            //*****************************
            //USB Webcams
            //*****************************

            CameraRig.ConnectedCameras.ForEach(x => x.cam.frameRateTrack = config.getProfile(bubble.profileInUse).framerateTrack);

        }


        private List<string> check_for_updates()
        {

            List<string> updateDat = new List<string>();

            string versionFile = "";

            //set version file depending on machine installation
            if (!bubble.devMachine)
            {
                versionFile = sensitiveInfo.versionFile;
            }
            else
            {
                versionFile = sensitiveInfo.versionFileDev;
            }

            //get the update information into a List
            updateDat = update.getUpdateInfo(sensitiveInfo.downloadsURL, versionFile, bubble.resourceDownloadFolder, 1, true);

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
                if (Int32.Parse(updateDat[4]) > bubble.newsSeq)
                {

                    update.installUpdateNow(updateDat[5], updateDat[6], bubble.resourceDownloadFolder, true);

                    try
                    {

                        //move all the unzipped files out of the download folder into the parent resource folder
                        //leave the zip file where it is to be deleted with the resource download folder
                        DirectoryInfo di = new DirectoryInfo(bubble.resourceDownloadFolder);
                        FileInfo[] files = di.GetFiles();

                        foreach (FileInfo fi in files)
                        {
                            if (fi.Name != updateDat[6]) File.Copy(bubble.resourceDownloadFolder + fi.Name, bubble.resourceFolder + fi.Name, true);
                        }

                        bubble.newsSeq = Int32.Parse(updateDat[4]);
                        configuration.newsSeq = Int32.Parse(updateDat[4]);
                        newsInfo.BackColor = Color.Gold;
                    }
                    catch { return updateDat; }

                    if (Directory.Exists(bubble.resourceDownloadFolder))
                    {
                        try
                        {
                            Directory.Delete(bubble.resourceDownloadFolder, true);
                        }
                        catch { return updateDat; }
                    }

                }

            }

            return updateDat;

        }


        private void emailUser_TextChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).emailUser = emailUser.Text;
        }

        private void emailPass_TextChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).emailPass = emailPass.Text;
        }

        private void smtpHost_TextChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).smtpHost = smtpHost.Text;
        }

        private void sendTo_TextChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).sendTo = sendTo.Text;
        }

        private void SSL_CheckedChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).EnableSsl = SSL.Checked;
        }

        private void replyTo_TextChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).replyTo = replyTo.Text;
        }

        private void sentBy_TextChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).sentBy = sentBy.Text;
        }

        private void mailSubject_TextChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).mailSubject = mailSubject.Text;
        }

        private void mailBody_TextChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).mailBody = mailBody.Text;
        }

        private void ftpUser_TextChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).ftpUser = ftpUser.Text;
        }

        private void ftpPass_TextChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).ftpPass = ftpPass.Text;
        }

        private void ftpRoot_TextChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).ftpRoot = ftpRoot.Text;
        }

        private void sendTest_Click(object sender, EventArgs e)
        {
            mail.sendEmail(config.getProfile(bubble.profileInUse).sentBy,
                           config.getProfile(bubble.profileInUse).sendTo,
                           config.getProfile(bubble.profileInUse).mailSubject + " - Testing email credentials",
                           config.getProfile(bubble.profileInUse).mailBody,
                           config.getProfile(bubble.profileInUse).replyTo,
                           bubble.attachments,
                           time.secondsSinceStart(),
                           config.getProfile(bubble.profileInUse).emailUser,
                           config.getProfile(bubble.profileInUse).emailPass,
                           config.getProfile(bubble.profileInUse).smtpHost,
                           config.getProfile(bubble.profileInUse).smtpPort,
                           config.getProfile(bubble.profileInUse).EnableSsl);
        }
        private void Email_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ftp.Upload(@"C:\TestImageNewWebcam.jpg", config.getProfile(bubble.profileInUse).ftpRoot, config.getProfile(bubble.profileInUse).ftpUser, config.getProfile(bubble.profileInUse).ftpPass);
        }

        private void emailUser_Leave(object sender, EventArgs e)
        {
            if (!mail.validEmail(emailUser.Text))
            {
                bubble.messageAlert("'Email User' is not a valid email address", "Invalid Email");
                emailUser.BackColor = Color.Red;
            }
            else
            {
                emailUser.BackColor = Color.LemonChiffon;
            }
        }

        private void sendTo_Leave(object sender, EventArgs e)
        {

            bool validEmails = true;

            string[] emails = sendTo.Text.Split(';');

            foreach (string email in emails)
            {

                if (!mail.validEmail(email))
                {
                    validEmails = false;
                    break;
                }

            }


            if (!validEmails)
            {
                bubble.messageAlert("'Send Email To' contains valid email address", "Invalid Email");
                sendTo.BackColor = Color.Red;
            }
            else
            {
                sendTo.BackColor = Color.LemonChiffon;
            }
        }

        private void replyTo_Leave(object sender, EventArgs e)
        {
            if (!mail.validEmail(replyTo.Text))
            {
                bubble.messageAlert("'Reply To' is not a valid email address", "Invalid Email");
                replyTo.BackColor = Color.Red;
            }
            else
            {
                replyTo.BackColor = Color.LemonChiffon;
            }
        }

        private void sentBy_Leave(object sender, EventArgs e)
        {
            if (!mail.validEmail(sentBy.Text))
            {
                bubble.messageAlert("'Sent By' is not a valid email address", "Invalid Email");
                sentBy.BackColor = Color.Red;
            }
            else
            {
                sentBy.BackColor = Color.LemonChiffon;
            }
        }


        private void sendFullSize_CheckedChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).sendFullSizeImages = sendFullSize.Checked;
            if (sendFullSize.Checked)
            {
                sendThumb.Checked = false;
                config.getProfile(bubble.profileInUse).sendThumbnailImages = false;
                sendMosaic.Checked = false;
                config.getProfile(bubble.profileInUse).sendMosaicImages = false;
            }
            imageFileInterval.Enabled = sendFullSize.Checked || sendThumb.Checked || sendMosaic.Checked || loadToFtp.Checked;
            sendEmail.Checked = config.getProfile(bubble.profileInUse).sendThumbnailImages || config.getProfile(bubble.profileInUse).sendFullSizeImages || config.getProfile(bubble.profileInUse).sendMosaicImages;
            config.getProfile(bubble.profileInUse).sendNotifyEmail = sendEmail.Checked;
        }

        private void sendThumb_CheckedChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).sendThumbnailImages = sendThumb.Checked;
            if (sendThumb.Checked)
            {
                sendFullSize.Checked = false;
                config.getProfile(bubble.profileInUse).sendFullSizeImages = false;
                sendMosaic.Checked = false;
                config.getProfile(bubble.profileInUse).sendMosaicImages = false;
            }
            imageFileInterval.Enabled = sendFullSize.Checked || sendThumb.Checked || sendMosaic.Checked || loadToFtp.Checked;
            sendEmail.Checked = config.getProfile(bubble.profileInUse).sendThumbnailImages || config.getProfile(bubble.profileInUse).sendFullSizeImages || config.getProfile(bubble.profileInUse).sendMosaicImages;
            config.getProfile(bubble.profileInUse).sendNotifyEmail = sendEmail.Checked;
        }

        private void sendMosaic_CheckedChanged(object sender, EventArgs e)
        {

            config.getProfile(bubble.profileInUse).sendMosaicImages = sendMosaic.Checked;
            if (sendMosaic.Checked)
            {
                sendFullSize.Checked = false;
                config.getProfile(bubble.profileInUse).sendFullSizeImages = false;
                sendThumb.Checked = false;
                config.getProfile(bubble.profileInUse).sendThumbnailImages = false;
            }
            imageFileInterval.Enabled = sendFullSize.Checked || sendThumb.Checked || sendMosaic.Checked || loadToFtp.Checked;
            sendEmail.Checked = config.getProfile(bubble.profileInUse).sendThumbnailImages || config.getProfile(bubble.profileInUse).sendFullSizeImages || config.getProfile(bubble.profileInUse).sendMosaicImages;
            config.getProfile(bubble.profileInUse).sendNotifyEmail = sendEmail.Checked;
            mosaicImagesPerRow.Enabled = sendMosaic.Checked;

        }

        private void captureMovementImages_CheckedChanged(object sender, EventArgs e)
        {

            config.getProfile(bubble.profileInUse).captureMovementImages = captureMovementImages.Checked;

            if (!captureMovementImages.Checked)
            {

                graphDate(DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture), "Image capture switched off");

                sendFullSize.Checked = false;
                sendThumb.Checked = false;
                sendMosaic.Checked = false;
                loadToFtp.Checked = false;

            }
            else
            {

                drawGraph(this, null);

            }


        }


        private void sendEmail_CheckedChanged(object sender, EventArgs e)
        {

            emailNotifInterval.Enabled = sendEmail.Checked;
            config.getProfile(bubble.profileInUse).sendNotifyEmail = sendEmail.Checked;

            if (!sendEmail.Checked)
            {
                sendThumb.Checked = false;
                config.getProfile(bubble.profileInUse).sendThumbnailImages = false;
                sendFullSize.Checked = false;
                config.getProfile(bubble.profileInUse).sendFullSizeImages = false;
                sendMosaic.Checked = false;
                config.getProfile(bubble.profileInUse).sendMosaicImages = false;
            }

        }

        private void loadToFtp_CheckedChanged(object sender, EventArgs e)
        {
            imageFileInterval.Enabled = sendFullSize.Checked || sendThumb.Checked || loadToFtp.Checked;
            config.getProfile(bubble.profileInUse).loadImagesToFtp = loadToFtp.Checked;
        }



        #region camera_code

        private void button4_Click(object sender, EventArgs e)
        {
            openNewCamera();
        }



        private void openNewCamera()
        {

            string tmpStr = config.getProfile(bubble.profileInUse).webcam;

            CaptureDeviceForm form = new CaptureDeviceForm(tmpStr, toolTip1.Active);

            if (form.ShowDialog(this) == DialogResult.OK)
            {

                if (form.Device.ipCam)
                {

                    IPAddress parsedIPAddress;
                    Uri parsedUri;

                    //check that the url resolves
                    if (Uri.TryCreate(form.Device.address, UriKind.Absolute, out parsedUri) && IPAddress.TryParse(parsedUri.DnsSafeHost, out parsedIPAddress))
                    {

                        Ping pingSender = new Ping();
                        PingReply reply = pingSender.Send(parsedIPAddress);

                        //the ip webcam is running
                        if (reply.Status == IPStatus.Success)
                        {

                            CameraRig.updateInfo(bubble.profileInUse, form.Device.address, CameraRig.infoEnum.ipWebcamAddress, form.Device.address);

                            AForge.Video.MJPEGStream stream = new AForge.Video.MJPEGStream(form.Device.address);

                            if (form.Device.user != string.Empty)
                            {

                                stream.Login = form.Device.user;
                                stream.Password = form.Device.password;

                                CameraRig.updateInfo(bubble.profileInUse, form.Device.address, CameraRig.infoEnum.ipWebcamUser, form.Device.user);
                                CameraRig.updateInfo(bubble.profileInUse, form.Device.address, CameraRig.infoEnum.ipWebcamPassword, form.Device.password);

                            }

                            Camera cam = OpenVideoSource(null, stream, true, -1);
                            cam.frameRateTrack = config.getProfile(bubble.profileInUse).framerateTrack;

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

                        //config.getProfile(bubble.profileInUse).webcam = form.Device.address;

                        // open camera
                        Camera cam = OpenVideoSource(localSource, null, false, -1);
                        cam.frameRateTrack = config.getProfile(bubble.profileInUse).framerateTrack;

                    }

                }


            }
        }

        // Open video source

        //#ToDo check adding new cameras after removing
        private Camera OpenVideoSource(VideoCaptureDevice source, AForge.Video.MJPEGStream ipStream, Boolean ip, int cameraNo)//(VideoCaptureDevice source)
        {


            MotionDetector detector = new MotionDetector(new SimpleBackgroundModelingDetector());

            string camSource;

            // create camera
            Camera camera;

            if (!ip)
            {

                camSource = source.Source;
                camera = new Camera(source, detector, camSource);

            }
            else
            {

                camSource = ipStream.Source;
                camera = new Camera(ipStream, detector, camSource);

            }

            camera.motionLevelEvent -= new motionLevelEventHandler(bubble.motionEvent);
            camera.motionLevelEvent += new motionLevelEventHandler(bubble.motionEvent);

            // start camera
            camera.Start();
            camera.ConnectedAt = DateTime.Now;

            ConnectedCamera connectedCamera = new ConnectedCamera();
            connectedCamera.cameraName = camSource;//source.Source;
            connectedCamera.cam = camera;
            bool existsInInfo = CameraRig.CamsInfo.Where(x => x.profileName == bubble.profileInUse && x.webcam == camSource).Count() > 1;
            connectedCamera.friendlyName = existsInInfo ? CameraRig.rigInfoGet(bubble.profileInUse, camSource, CameraRig.infoEnum.friendlyName).ToString() : string.Empty;
            connectedCamera.cam.camNo = cameraNo == -1 ? CameraRig.cameraCount() : cameraNo;
            //CameraRig.addCamera(rig_item);
            CameraRig.ConnectedCameras.Add(connectedCamera);
            int newCameraIdx = CameraRig.cameraCount() - 1;

            //int curCam = connectedCamera.cam.camNo;// CameraRig.cameraCount() - 1;
            CameraRig.CurrentlyDisplayingCamera = newCameraIdx;

            config.getProfile(bubble.profileInUse).webcam = camSource;

            //populate or update rig info
            CameraRig.rigInfoPopulate(config.getProfile(bubble.profileInUse).profileName, newCameraIdx);

            //CameraRig.ConnectedCameras[curCam].cam.camNo = curCam;

            //get desired button or first available button
            //int desiredButton = CameraRig.ConnectedCameras[newCameraIdx].displayButton;
            //check if the desxt frired button is free and return the next button if one is available

            var firstFreeButton = CameraButtonGroupInstance.Where(x => x.CameraButtonState == CameraButtonGroup.ButtonState.NotConnected).First();
            int camButton = firstFreeButton != null ? firstFreeButton.id : 999;
            bool freeCamsExist = camButton != 999;

            //if a free camera button exists assign the camera
            if (freeCamsExist)
            {
                CameraRig.ConnectedCameras[newCameraIdx].displayButton = camButton;
            }

            //update info for camera
            CameraRig.updateInfo(bubble.profileInUse, config.getProfile(bubble.profileInUse).webcam, CameraRig.infoEnum.displayButton, camButton);

            if (config.getProfile(bubble.profileInUse).selectedCam == "")
            {
                cameraSwitch(CameraRig.ConnectedCameras[newCameraIdx].displayButton, false, false);
            }
            else
            {
                if (config.getProfile(bubble.profileInUse).selectedCam == camSource)
                {
                    cameraSwitch(CameraRig.ConnectedCameras[newCameraIdx].displayButton, false, false);
                }
            }

            camButtonSetColours();

            if (CameraRig.ConnectedCameras[newCameraIdx].cam.alarmActive)
            {

                if (CameraRig.ConnectedCameras[newCameraIdx].displayButton == 1) selcam(this.bttncam1sel, 1);
                if (CameraRig.ConnectedCameras[newCameraIdx].displayButton == 2) selcam(this.bttncam2sel, 2);
                if (CameraRig.ConnectedCameras[newCameraIdx].displayButton == 3) selcam(this.bttncam3sel, 3);
                if (CameraRig.ConnectedCameras[newCameraIdx].displayButton == 4) selcam(this.bttncam4sel, 4);
                if (CameraRig.ConnectedCameras[newCameraIdx].displayButton == 5) selcam(this.bttncam5sel, 5);
                if (CameraRig.ConnectedCameras[newCameraIdx].displayButton == 6) selcam(this.bttncam6sel, 6);
                if (CameraRig.ConnectedCameras[newCameraIdx].displayButton == 7) selcam(this.bttncam7sel, 7);
                if (CameraRig.ConnectedCameras[newCameraIdx].displayButton == 8) selcam(this.bttncam8sel, 8);
                if (CameraRig.ConnectedCameras[newCameraIdx].displayButton == 9) selcam(this.bttncam9sel, 9);
            }

            if (CameraRig.ConnectedCameras[newCameraIdx].cam.publishActive)
            {
                if (CameraRig.ConnectedCameras[newCameraIdx].displayButton == 1) pubcam(this.bttncam1pub, 1);
                if (CameraRig.ConnectedCameras[newCameraIdx].displayButton == 2) pubcam(this.bttncam2pub, 2);
                if (CameraRig.ConnectedCameras[newCameraIdx].displayButton == 3) pubcam(this.bttncam3pub, 3);
                if (CameraRig.ConnectedCameras[newCameraIdx].displayButton == 4) pubcam(this.bttncam4pub, 4);
                if (CameraRig.ConnectedCameras[newCameraIdx].displayButton == 5) pubcam(this.bttncam5pub, 5);
                if (CameraRig.ConnectedCameras[newCameraIdx].displayButton == 6) pubcam(this.bttncam6pub, 6);
                if (CameraRig.ConnectedCameras[newCameraIdx].displayButton == 7) pubcam(this.bttncam7pub, 7);
                if (CameraRig.ConnectedCameras[newCameraIdx].displayButton == 8) pubcam(this.bttncam8pub, 8);
                if (CameraRig.ConnectedCameras[newCameraIdx].displayButton == 9) pubcam(this.bttncam9pub, 9);
            }

            CameraRig.alert(bubble.Alert.on);
            CameraRig.ConnectedCameras[newCameraIdx].cam.exposeArea = bubble.exposeArea;
            CameraRig.ConnectedCameras[newCameraIdx].cam.motionAlarm -= new alarmEventHandler(bubble.camera_Alarm);
            CameraRig.ConnectedCameras[newCameraIdx].cam.motionAlarm += new alarmEventHandler(bubble.camera_Alarm);
            bubble.webcamAttached = true;
            button23.SynchronisedInvoke(() => button23.Enabled = CameraRig.camerasAreConnected());
            return camera;
        }

        //#todo recor movie
        // On new frame
        private void camera_NewFrame(object sender, System.EventArgs e)
        {

            frameCount++;

            if ((intervalsToSave != 0) && (saveOnMotion == true))
            {

                //lets save the frame
                if (1 == 2)//writer == null)
                {
                    // create file name
                    DateTime date = DateTime.Now;
                    String fileName = String.Format("{0}-{1}-{2} {3}-{4}-{5}.avi", date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);

                    try
                    {
                        // create AVI writer
                        writer = new AVIWriter("wmv3");
                        // open AVI file
                        writer.Open(@"C:\" + fileName, cameraWindow.Camera.Width, cameraWindow.Camera.Height);
                    }
                    catch (ApplicationException ex)
                    {
                        if (writer != null)
                        {
                            writer.Dispose();
                            writer = null;
                        }
                    }
                }

            }
        }

        #endregion


        private void bttnClearAll_Click(object sender, EventArgs e)
        {

            ArrayList ftpFiles = ftp.GetFileList();

            if (bubble.messageQuestionConfirm("Click on yes to delete all saved image files.", "Delete all TeboCam image files?") == DialogResult.Yes)
            {
                if (clrImg.Checked)
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



                    FileManager.clearFiles(bubble.thumbFolder);
                    FileManager.clearFiles(bubble.imageFolder);

                    lblAdminMes.Text = "Image computer files deleted";
                    bubble.logAddLine("Image files on computer deleted.");
                }
                if (clrFtp.Checked)
                {
                    if (config.getProfile(bubble.profileInUse).filenamePrefix.Trim() != "")
                    {
                        FileManager.clearFtp();
                        lblAdminMes.Text = "Image web files deleted";
                    }
                    else
                    {
                        string tmpStr;
                        tmpStr = "No images deleted as the filename prefix is empty." + Environment.NewLine;
                        tmpStr += "Which means a risk of deleting the wrong files." + Environment.NewLine + Environment.NewLine;
                        tmpStr += "To remedy this ensure the filename prefix is populated";
                        bubble.messageInform(tmpStr, "Cannot delete Website files");
                        bubble.logAddLine("Cannot delete image files on website due to empty filename prefix.");
                    }


                }
            }

        }






        private void activeCountdown(object sender, DoWorkEventArgs e)
        {

            //SetCheckBox(bttnMotionSchedule, false);
            bttnMotionSchedule.SynchronisedInvoke(() => bttnMotionSchedule.Checked = false);

            bubble.countingdown = true;
            int tmpInt = 0;
            int countdown = 0;
            int lastCount = 0;

            string tmpStr;

            //Time radio button is selected
            if (config.getProfile(bubble.profileInUse).countdownTime)
            {

                int startTime = time.timeInSeconds(config.getProfile(bubble.profileInUse).activatecountdownTime);
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
                if (config.getProfile(bubble.profileInUse).countdownNow)
                {
                    tmpStr = "0";
                }

                //Seconds radio button is selected
                else
                {
                    tmpStr = actCountdown.Text.Trim();
                }
            }


            if (bubble.IsNumeric(tmpStr))
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
                bubble.logAddLine("Motion countdown started: " + tmpInt.ToString() + " seconds until start.");
                txtMess.SynchronisedInvoke(() => txtMess.Text = "Counting Down...");
                //SetInfo(txtMess, "Counting Down...");
            }

            //This is the loop that checks on the countdown
            while (tmpInt > 0 && !bubble.countingdownstop)
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
            bubble.countingdown = false;
            if (!bubble.countingdownstop)
            {
                bubble.Alert.on = true;
                bubble.logAddLine("Motion detection activated");
            }

            txtMess.SynchronisedInvoke(() => txtMess.Text = string.Empty);
            //SetInfo(txtMess, "");
            databaseUpdate(false);

        }

        private void databaseUpdate(bool off)
        {


            if (bubble.DatabaseCredentialsCorrect)
            {

                string user = config.getProfile(bubble.profileInUse).webUser;
                string instance = config.getProfile(bubble.profileInUse).webInstance;
                string update_result = "";

                if (off)
                {
                    update_result = database.database_update_data(bubble.mysqlDriver, user, instance, "statusoff", bubble.logForSql()) + " records affected.";
                }
                else
                {
                    if (bubble.Alert.on)
                    {
                        update_result = database.database_update_data(bubble.mysqlDriver, user, instance, "statusactive", bubble.logForSql()) + " records affected.";
                    }
                    else
                    {
                        update_result = database.database_update_data(bubble.mysqlDriver, user, instance, "statusinactive", bubble.logForSql()) + " records affected.";

                    }
                }
            }

        }


        private void bttnMotionActive_CheckedChanged(object sender, EventArgs e)
        {

            bubble.areaOffAtMotionInit();

            if (bttnMotionActive.Checked)
            {
                if (!bubble.countingdown)
                {

                    //about to go to active motion detection
                    //however no camera is selected as active
                    //so activate all cameras.
                    //as we could choose an incorrect camera.
                    if (!licence.aCameraIsSelected())
                    {

                        var availableCameraButtons = CameraButtonGroupInstance.Where(x => x.CameraButtonState != CameraButtonGroup.ButtonState.NotConnected).ToList();

                        foreach (var cameraButton in availableCameraButtons)
                        {

                            switch (cameraButton.id)
                            {

                                case 1:
                                    selcam(this.bttncam1sel, 1);
                                    break;
                                case 2:
                                    selcam(this.bttncam2sel, 2);
                                    break;
                                case 3:
                                    selcam(this.bttncam3sel, 3);
                                    break;
                                case 4:
                                    selcam(this.bttncam4sel, 4);
                                    break;
                                case 5:
                                    selcam(this.bttncam5sel, 5);
                                    break;
                                case 6:
                                    selcam(this.bttncam6sel, 6);
                                    break;
                                case 7:
                                    selcam(this.bttncam7sel, 7);
                                    break;
                                case 8:
                                    selcam(this.bttncam8sel, 8);
                                    break;
                                case 9:
                                    selcam(this.bttncam9sel, 9);
                                    break;

                            }
                        }

                    }

                    actCount.Visible = true;
                    bubble.countingdownstop = false;
                    bw.DoWork -= new DoWorkEventHandler(activeCountdown);
                    bw.DoWork += new DoWorkEventHandler(activeCountdown);
                    bw.WorkerSupportsCancellation = true;
                    bw.RunWorkerAsync();
                }
            }
            else
            {
                //20130427 restored as the scheduleOnAtStart property now takes care of reactivating at start up
                //if (bttnMotionSchedule.Checked) SetCheckBox(bttnMotionSchedule, false);
                bttnMotionSchedule.SynchronisedInvoke(() => bttnMotionSchedule.Checked = false);

                bubble.countingdownstop = true;
                bubble.Alert.on = false;
                bubble.logAddLine("Motion detection inactivated");
            }


        }




        // On add to log
        private void log_add(object sender, System.EventArgs e)
        {
            string msg = bubble.log.Lines[(int)bubble.log.Count() - 1].Message;
            string dt = DateTime.Now.ToString("yyyy/MM/dd-HH:mm:ss:fff", System.Globalization.CultureInfo.InvariantCulture);
            txtLog.SynchronisedInvoke(() => txtLog.Text = string.Format("{0} [{1}]", msg, dt) + "\n" + txtLog.Text);
        }


        private void actCountdown_TextChanged(object sender, EventArgs e)
        {
            if (bubble.IsNumeric(actCountdown.Text))
            {
                config.getProfile(bubble.profileInUse).activatecountdown = Convert.ToInt32(actCountdown.Text);
            }
            else
            {
                actCountdown.Text = "0";
                config.getProfile(bubble.profileInUse).activatecountdown = 0;
            }
        }

        private void drawLevel(object sender, System.EventArgs e)
        {
            if (showLevel)
            {
                LevelControlBox.levelDraw(bubble.motionLevel);
            }
        }







        private void drawGraph(object sender, EventArgs e)
        {
            graphDate(DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));
        }

        private void drawGraphPing(object sender, EventArgs e)
        {
            graphDate(bubble.pingGraphDate);
        }


        private void graphDate(string graphDate, string displayText = "")
        {
            try
            {

                bubble.graphCurrentDate = graphDate;

                bool movement = false;

                int windowHeight = pictureBox1.ClientRectangle.Height;
                int windowWidth = pictureBox1.ClientRectangle.Width;
                int timeSep = (int)((windowWidth - 80) / 12) + 5;
                int curPos = timeSep - 20;

                Graphics graphicsObj;
                myBitmap = new Bitmap(windowWidth, windowHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                graphicsObj = Graphics.FromImage(myBitmap);
                Pen linePen = new Pen(System.Drawing.Color.LemonChiffon, 3);
                Pen thinPen = new Pen(System.Drawing.Color.LemonChiffon, 1);
                Pen redPen = new Pen(System.Drawing.Color.Red, 4);

                string title = "";

                if (graphDate != null)
                {
                    title = LeftRightMid.Right(graphDate, 2) + "/" + LeftRightMid.Mid(graphDate, 4, 2) + "/" + LeftRightMid.Left(graphDate, 4);
                }
                graphicsObj.DrawString(title, new Font("Tahoma", 8, FontStyle.Bold), Brushes.LemonChiffon, new PointF(5, 5));

                int lineLength = 170;
                int lineWidth = 10;
                int lineHeight = lineLength;
                int topHt = 150;
                int startY = 0;
                int xPos = 0;
                double heightMod = 0;


                if (displayText == string.Empty)
                {


                    for (int i = 0; i < 24; i++)
                    {

                        int cellIdx = Convert.ToInt32((int)Math.Floor((decimal)(i / 2)));
                        //Draw times
                        string tpmStr1 = i.ToString() + ":00";
                        string tpmStr2 = Convert.ToString(i + 1) + ":59";

                        if (i < 10)
                        {
                            tpmStr1 = "0" + i.ToString() + ":00";
                        }
                        if (i + 1 < 10)
                        {
                            tpmStr2 = "0" + Convert.ToString(i + 1) + ":59";
                        }

                        graphicsObj.DrawString(tpmStr1 + "\n" + tpmStr2, new Font("Tahoma", 8), Brushes.LemonChiffon, new PointF(curPos, windowHeight - 30));

                        int currHour = Convert.ToInt32(LeftRightMid.Left(time.currentTime(), 2));
                        int currHourIdx = Convert.ToInt32((int)Math.Floor((decimal)(currHour / 2)));
                        if (graphDate == time.currentDate() && cellIdx == currHourIdx)
                        {
                            Rectangle rect1 = new Rectangle(curPos - 2, windowHeight - 31, 35, 27);
                            graphicsObj.DrawRectangle(thinPen, rect1);
                        }

                        startY = (windowHeight - lineLength) - 35;
                        xPos = curPos + 10;
                        lineXpos.Add(xPos);

                        if (graphDate != null)
                        {

                            //draw lines
                            ArrayList graphData = graph.getGraphHist(graphDate);
                            if (graphData == null)
                            {
                                return;
                            }

                            string val = "";
                            val = bubble.graphVal(graphData, cellIdx).ToString();



                            if (val != "nil")
                            {

                                int maxVal = 0;
                                int lastV = 0;
                                int regVals = 0;
                                foreach (int v in graphData)
                                {
                                    if (v > 0) regVals++;
                                    if (v > lastV)
                                    {
                                        maxVal = v;
                                        lastV = v;
                                    }

                                }
                                if (regVals > 1)
                                {
                                    heightMod = (double)topHt / (double)maxVal;
                                }
                                else
                                {
                                    heightMod = 0.5 * ((double)topHt / (double)maxVal);
                                }

                                int gVal = (int)graphData[cellIdx];
                                lineHeight = Convert.ToInt32(Math.Floor((double)gVal * heightMod));
                                startY = startY + lineLength - lineHeight;


                                movement = true;

                                //yellow outline
                                Rectangle rectangleObj = new Rectangle(xPos, startY, lineWidth, lineHeight);
                                graphicsObj.DrawRectangle(linePen, rectangleObj);

                                //red filler
                                Rectangle rectangleObj2 = new Rectangle(xPos + 4, startY + 4, 3, lineHeight - 7);
                                graphicsObj.DrawRectangle(redPen, rectangleObj2);

                                string thisVal = graph.getGraphVal(graphDate, cellIdx);

                                graphicsObj.DrawString(thisVal, new Font("Tahoma", 8), Brushes.LemonChiffon, new PointF(curPos + 7, lineLength - (lineHeight)));
                            }

                        }

                        //increment i for next two hour slot
                        i += 1;
                        curPos += timeSep;


                    }

                }

                if (!movement && displayText == string.Empty)
                {

                    graphicsObj.DrawString("No Movement Detected", new Font("Tahoma", 20), Brushes.LemonChiffon, new PointF(80, windowHeight - 140));

                }

                if (displayText != string.Empty)
                {

                    graphicsObj.DrawString(displayText, new Font("Tahoma", 20), Brushes.LemonChiffon, new PointF(80, windowHeight - 140));

                }



                Bitmap tmpBit = new Bitmap(myBitmap);
                bubble.graphCurrent = tmpBit;
                graphicsObj.Dispose();
                pictureBox1.Invalidate();
            }
            catch { }
        }




        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphicsObj = e.Graphics;

            if (myBitmap != null)
            {
                graphicsObj.DrawImage(myBitmap, 0, 0, myBitmap.Width, myBitmap.Height);
            }


        }

        //private void levelbox_Paint(object sender, PaintEventArgs e)
        //{

        //    Graphics levelObj = e.Graphics;
        //    try
        //    {
        //        if (levelBitmap != null)
        //        {
        //            levelObj.DrawImage(levelBitmap, 0, 0, levelBitmap.Width, levelBitmap.Height);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        bubble.logAddLine("Error drawing level bitmap");
        //    }

        //}

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



        private void preferences_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                teboDebug.closeFile();

                databaseUpdate(true);

                tabControl1.SelectedIndex = 0;
                Invalidate();

                if (bubble.fileBusy)
                {

                    bubble.logAddLine("Waiting for file processing to finish before exiting...");
                    bubble.logAddLine("Application will remain frozen until exit...");

                    int tmpInt = 0;

                    while (bubble.fileBusy)
                    {
                        Thread.Sleep(500);
                        Invalidate();
                        tmpInt++;
                        if (tmpInt > 20) break;
                    }

                }

                bubble.keepWorking = false;
                bubble.logAddLine("Stopping TeboCam");
                Invalidate();
                if (bttnMotionActive.Checked
                    || bttnMotionAtStartup.Checked
                    || bttnActivateAtEveryStartup.Checked)
                {
                    bubble.Alert.on = true;
                }

                //FileManager.WriteFile("config");
                config.WebcamSettingsConfigDataPopulate();
                configuration.WriteXMLFile(bubble.xmlFolder + FileManager.configFile + ".xml", configuration);
                bubble.logAddLine("Config data saved.");
                //FileManager.WriteFile("graph");#FIX
                graph.WriteXMLFile(bubble.xmlFolder + "GraphData.xml", graph);
                bubble.logAddLine("Graph data saved.");
                bubble.logAddLine("Saving log data and closing.");
                log.WriteXMLFile(bubble.xmlFolder + "LogData" + ".xml", log);

                bttnMotionInactive.Checked = true;
                bttnMotionActive.Checked = false;
                bttnMotionAtStartup.Checked = false;

                Invalidate();
                bubble.workInit(false);
                closeAllCameras();
                bubble.logAddLine("Application will remain frozen until exit.");
                Invalidate();

                if (bubble.databaseConnect && bubble.DatabaseCredentialsCorrect && config.getProfile(bubble.profileInUse).webUpd)
                {
                    string user = config.getProfile(bubble.profileInUse).webUser;
                    string instance = config.getProfile(bubble.profileInUse).webInstance;
                    string update_result = "";

                    update_result = database.database_update_data(bubble.mysqlDriver, user, instance, "statusoff", bubble.logForSql()) + " records affected.";
                    update_result = database.database_update_data(bubble.mysqlDriver, user, instance, "log", bubble.logForSql()) + " records affected.";
                    update_result = database.database_update_data(bubble.mysqlDriver, user, instance, "reset", time.currentDateTimeSql()) + " records affected.";
                }


                if (bubble.updaterInstall)
                {
                    bubble.logAddLine("Preparing for installation...");
                }
                Application.DoEvents();

                int secs = time.secondsSinceStart();

                if (config.getProfile(bubble.profileInUse).freezeGuard)
                {

                    pulse.stopCheck(bubble.pulseProcessName);

                }

                //killPulseCheck();
                Thread.Sleep(6000);


                if (bubble.updaterInstall)
                {

                    Application.DoEvents();
                    //bubble.postProcessCommand = " /profile " + bubble.profileInUse;
                    bubble.postProcessCommand = "profile " + bubble.profileInUse;

                    update.installUpdateRestart(bubble.upd_url,
                                                bubble.upd_file,
                                                bubble.destinationFolder,
                                                bubble.processToEnd,
                                                bubble.postProcess,
                                                bubble.postProcessCommand,
                                                bubble.updater,
                                                true,
                                                "");


                }

            }
            catch { }

        }


        private void closeAllCameras()
        {

            foreach (ConnectedCamera rigI in CameraRig.ConnectedCameras)
            {
                Camera camera = rigI.cam;
                camera.SignalToStop();
                camera.WaitForStop();
            }

            if (writer != null)
            {
                writer.Dispose();
                writer = null;
            }
            intervalsToSave = 0;

        }

        private void calendar_DateSelected(object sender, DateRangeEventArgs e)
        {

            string dateSelected = calendar.SelectionStart.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

            if (!captureMovementImages.Checked)
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


        private void ping_CheckedChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).ping = ping.Checked;
            bubble.pings = 0;
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

            config.getProfile(bubble.profileInUse).pingAll = rdPingAllCameras.Checked;

        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            while (bubble.fileBusy) { }
            bubble.emailTestOk = 9;

            mail.sendEmail(config.getProfile(bubble.profileInUse).sentBy,
                           config.getProfile(bubble.profileInUse).sendTo,
                           "TeboCam Test",
                           "This is a test email from TeboCam",
                           config.getProfile(bubble.profileInUse).replyTo,
                           false,
                           time.secondsSinceStart(),
                           config.getProfile(bubble.profileInUse).emailUser,
                           config.getProfile(bubble.profileInUse).emailPass,
                           config.getProfile(bubble.profileInUse).smtpHost,
                           config.getProfile(bubble.profileInUse).smtpPort,
                           config.getProfile(bubble.profileInUse).EnableSsl);

            while (bubble.emailTestOk == 9) { }
            if (bubble.emailTestOk == 1)
            {
                bubble.messageInform("It looks like the email test was successful", "Check your email");
            }
            else
            {
                bubble.messageAlert("It looks like the email test was unsuccessful", "Check your email settings");
            }
            bubble.emailTestOk = 0;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown2.Value == 60) { numericUpDown2.Value = 0; }
            if (numericUpDown2.Value == -1) { numericUpDown2.Value = 59; }
            if (!bubble.Loading)
            {
                config.getProfile(bubble.profileInUse).activatecountdownTime = numericUpDown1.Value.ToString().PadLeft(2, '0') + numericUpDown2.Value.ToString().PadLeft(2, '0');
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value == 24) { numericUpDown1.Value = 0; }
            if (numericUpDown1.Value == -1) { numericUpDown1.Value = 23; }
            if (!bubble.Loading)
            {
                config.getProfile(bubble.profileInUse).activatecountdownTime = numericUpDown1.Value.ToString().PadLeft(2, '0') + numericUpDown2.Value.ToString().PadLeft(2, '0');
            }

        }

        private void bttnTime_CheckedChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).countdownTime = bttnTime.Checked;
        }



        private void time_change(object sender, System.EventArgs e)
        {
            //SetLabel(lblTime, bubble.lastTime);
            lblTime.SynchronisedInvoke(() => lblTime.Text = bubble.lastTime);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            bubble.testFtp = true;
            bubble.testFtpError = false;
            FileManager.WriteFile("test");
            bubble.logAddLine("ftp test: uploading file");
            ftp.Upload(bubble.xmlFolder + FileManager.testFile + ".xml", config.getProfile(bubble.profileInUse).ftpRoot, config.getProfile(bubble.profileInUse).ftpUser, config.getProfile(bubble.profileInUse).ftpPass);

            if (!bubble.testFtpError)
            {
                bubble.logAddLine("ftp test: deleting file");
                ftp.DeleteFTP(FileManager.testFile + ".xml", config.getProfile(bubble.profileInUse).ftpRoot, config.getProfile(bubble.profileInUse).ftpUser, config.getProfile(bubble.profileInUse).ftpPass, false);
                if (bubble.testFtpError)
                {
                    bubble.logAddLine("Error with test ftp: deleting file");
                    bubble.messageInform("Error with test ftp: deleting file", "Error");
                }
            }
            else
            {
                bubble.logAddLine("Error with test ftp: uploading file");
                bubble.messageInform("Error with test ftp: uploading file", "Error");
            }

            if (!bubble.testFtpError)
            {
                bubble.messageInform("Ftp test was successful!", "Success");
            }

            bubble.testFtp = false;
            bubble.testFtpError = false;
        }


        private void label39_Click(object sender, EventArgs e)
        {

        }

        private void bttnNow_CheckedChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).countdownNow = bttnNow.Checked;
        }

        private void updateNotify_CheckedChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).updatesNotify = updateNotify.Checked;
        }



        private void openWebPageOLD(object sender, DoWorkEventArgs e)
        {

            bubble.openInternetBrowserAt(bubble.tebowebUrl);

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
                        if (ctrl.Name == picsForWindow[i, 0])
                        {
                            if (File.Exists(picsForWindow[i, 1]))
                            {
                                ((PictureBox)(ctrl)).ImageLocation = picsForWindow[i, 1];
                                picFound = true;
                                break;
                            }
                        }
                    }
                    if (!picFound) { ((PictureBox)(ctrl)).ImageLocation = ""; }
                }
            }

        }


        private int imageFilesCount()
        {
            Int32 fileCount = 0;

            DirectoryInfo diA = new DirectoryInfo(bubble.imageFolder);
            FileInfo[] imageFilesA = diA.GetFiles("*" + bubble.ImgSuffix);
            fileCount += imageFilesA.Length;
            DirectoryInfo diB = new DirectoryInfo(bubble.thumbFolder);
            FileInfo[] imageFilesB = diB.GetFiles("*" + bubble.ImgSuffix);
            fileCount += imageFilesB.Length;

            return fileCount;
        }


        private int imageFilesCountWeb()
        {
            ArrayList webFiles = ftp.GetFileList();
            return webFiles.Count;
        }



        private void button7_Click(object sender, EventArgs e)
        {
            lblCountOnComputer.Text = "Computer: " + imageFilesCount().ToString();
            lblCountOnWeb.Text = "Website: " + imageFilesCountWeb().ToString();
            Invalidate();
        }


        private void installationClean()
        {
            //Create folders if they do not exist
            if (!Directory.Exists(bubble.imageParentFolder))
            {
                Directory.CreateDirectory(bubble.imageParentFolder);
            }
            if (!Directory.Exists(bubble.imageFolder))
            {
                Directory.CreateDirectory(bubble.imageFolder);
            }
            if (!Directory.Exists(bubble.thumbFolder))
            {
                Directory.CreateDirectory(bubble.thumbFolder);
            }
            if (!Directory.Exists(bubble.resourceFolder))
            {
                Directory.CreateDirectory(bubble.resourceFolder);
            }

            string configXml = bubble.xmlFolder + "config.xml";
            if (!Directory.Exists(bubble.vaultFolder) && File.Exists(configXml))
            {
                Directory.CreateDirectory(bubble.vaultFolder);
                string configVlt = bubble.vaultFolder + "config262.xml";
                if (!File.Exists(configVlt))
                {
                    File.Copy(configXml, configVlt, true);
                }
            }



            if (!Directory.Exists(bubble.tmpFolder))
            {
                Directory.CreateDirectory(bubble.tmpFolder);
            }

            DirectoryInfo diTmp = new DirectoryInfo(bubble.tmpFolder);
            FileInfo[] imageFilesTmp = diTmp.GetFiles();
            foreach (FileInfo fi in imageFilesTmp)
            {
                File.Delete(fi.FullName);
            }


            if (!Directory.Exists(bubble.logFolder))
            {
                Directory.CreateDirectory(bubble.logFolder);
            }

            if (!Directory.Exists(bubble.xmlFolder))
            {
                Directory.CreateDirectory(bubble.xmlFolder);
                DirectoryInfo diApp = new DirectoryInfo(Application.StartupPath);
                FileInfo[] xmlFilesApp = diApp.GetFiles("*.xml");
                foreach (FileInfo fi in xmlFilesApp)
                {
                    if (fi.FullName != "Ionic.Zip.xml")
                    {
                        File.Move(fi.FullName, bubble.xmlFolder + fi.Name);
                    }
                }
            }


            if (File.Exists(bubble.xmlFolder + "processed.xml")) { File.Delete(bubble.xmlFolder + "processed.xml"); };

            if (File.Exists(Application.StartupPath + bubble.databaseTrialFile)) { File.Move(Application.StartupPath + bubble.databaseTrialFile, Application.StartupPath + bubble.dbaseConnectFile); };


        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown3.Value >= numericUpDown4.Value) { numericUpDown4.Value = numericUpDown3.Value + 1; }

        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown4.Value <= numericUpDown3.Value) { numericUpDown4.Value = numericUpDown3.Value + 1; }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SaveFileDialog test = new SaveFileDialog();

            test.Title = "Save WebPage...";
            test.DefaultExt = "html";
            test.AddExtension = true;
            test.Filter = "html files (*.html)|*.html|All files (*.*)|*.*";
            test.FileName = "index";
            test.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (test.ShowDialog() == DialogResult.OK)
            {
                string tmpStr = test.FileName;
                webPage.writePage(config.getProfile(bubble.profileInUse).filenamePrefix, bubble.ImgSuffix, Convert.ToInt32(numericUpDown3.Value), Convert.ToInt32(numericUpDown4.Value), tmpStr);
            }

        }


        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).sentByName = sentByName.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).pingSubject = pingSubject.Text;
        }

        private void populateFromProfile(string profileName)
        {
            configApplication data = config.getProfile(profileName);

            config.getProfile(bubble.profileInUse).areaDetection = data.areaDetection;
            config.getProfile(bubble.profileInUse).areaDetectionWithin = data.areaDetectionWithin;
            config.getProfile(bubble.profileInUse).baselineVal = data.baselineVal;
            config.getProfile(bubble.profileInUse).rectHeight = data.rectHeight;
            config.getProfile(bubble.profileInUse).rectWidth = data.rectWidth;


            config.getProfile(bubble.profileInUse).rectX = data.rectX;
            config.getProfile(bubble.profileInUse).rectY = data.rectY;
            config.getProfile(bubble.profileInUse).webcam = data.webcam;


            actCountdown.Text = data.activatecountdown.ToString();

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
                bttnMotionActive.Checked = false;
                bttnMotionInactive.Checked = true;
            }
            else
            {
                bttnMotionActive.Checked = data.AlertOnStartup;
                bttnMotionInactive.Checked = !data.AlertOnStartup;
            }


            //maintain the order of bttnSeconds.Checked, bttnTime.Checked, bttnNow.Checked
            bttnSeconds.Checked = !data.countdownTime;
            bttnTime.Checked = data.countdownTime;
            bttnNow.Checked = data.countdownNow;
            //maintain the order of bttnSeconds.Checked, bttnTime.Checked, bttnNow.Checked

            //20101023 legacy code - cycleStamp replaced by cycleStampChecked
            if (data.cycleStamp) data.cycleStampChecked = 1;
            //20101023 legacy code - cycleStamp replaced by cycleStampChecked

            emailNotifInterval.Text = data.emailNotifyInterval.ToString();
            emailPass.Text = data.emailPass;
            txtLockdownPassword.Text = data.lockdownPassword;
            rdLockdownOn.Checked = data.lockdownOn;
            bubble.lockdown = data.lockdownOn;
            btnSecurityLockdownOn.Enabled = rdLockdownOn.Checked;
            emailUser.Text = data.emailUser;

            ftpPass.Text = data.ftpPass;
            ftpRoot.Text = data.ftpRoot;
            ftpUser.Text = data.ftpUser;
            imageFileInterval.Text = data.imageSaveInterval.ToString();
            lblImgPref.Text = "Image Prefix: " + data.filenamePrefix + "   e.g " + data.filenamePrefix + "1" + bubble.ImgSuffix;
            loadToFtp.Checked = data.loadImagesToFtp;
            mailBody.Text = data.mailBody;

            mailSubject.Text = data.mailSubject;
            maxImagesToEmail.Text = data.maxImagesToEmail.ToString();

            numericUpDown1.Value = Convert.ToDecimal(LeftRightMid.Left(data.activatecountdownTime, 2));
            numericUpDown2.Value = Convert.ToDecimal(LeftRightMid.Right(data.activatecountdownTime, 2));


            pubTimerOn.Checked = data.timerOn;

            if (pubTimerOn.Checked)
            {
                lblstartpub.ForeColor = Color.Black;
                lblendpub.ForeColor = Color.Black;
            }
            else
            {
                lblstartpub.ForeColor = System.Drawing.SystemColors.Control;
                lblendpub.ForeColor = System.Drawing.SystemColors.Control;
            }

            bttnMotionSchedule.Checked = data.timerOnMov;
            bttnMotionScheduleOnAtStart.Checked = data.scheduleOnAtStart;

            //the schedule on at start box is checked so we set the schedule on if it is not on
            if (!data.timerOnMov && data.scheduleOnAtStart)
            {

                bttnMotionSchedule.Checked = true;

            }

            bttnActivateAtEveryStartup.Checked = data.activateAtEveryStartup;

            if (bttnMotionSchedule.Checked)
            {
                lblstartmov.ForeColor = Color.Black;
                lblendmov.ForeColor = Color.Black;
            }
            else
            {
                lblstartmov.ForeColor = System.Drawing.SystemColors.Control;
                lblendmov.ForeColor = System.Drawing.SystemColors.Control;
            }



            ping.Checked = data.ping;
            pingMins.Text = data.pingInterval.ToString();
            pingSubject.Text = data.pingSubject;
            replyTo.Text = data.replyTo;

            if (data.sendThumbnailImages)
            {
                sendThumb.Checked = data.sendThumbnailImages;
                sendFullSize.Checked = data.sendFullSizeImages;
                sendMosaic.Checked = data.sendMosaicImages;
            }
            else if (data.sendFullSizeImages)
            {
                sendFullSize.Checked = data.sendFullSizeImages;
                sendThumb.Checked = data.sendThumbnailImages;
                sendMosaic.Checked = data.sendMosaicImages;
            }
            else if (data.sendMosaicImages)
            {
                sendMosaic.Checked = data.sendMosaicImages;
                sendFullSize.Checked = data.sendFullSizeImages;
                sendThumb.Checked = data.sendThumbnailImages;
            }

            mosaicImagesPerRow.Text = data.mosaicImagesPerRow.ToString();



            sendEmail.Checked = data.sendNotifyEmail;


            sendTo.Text = data.sendTo;
            sentBy.Text = data.sentBy;
            sentByName.Text = data.sentByName;
            smtpHost.Text = data.smtpHost;
            smtpPort.Text = data.smtpPort.ToString();
            SSL.Checked = (bool)data.EnableSsl;
            updateNotify.Checked = data.updatesNotify;

            pubImage.Checked = data.pubImage;
            if (decimal.Parse(data.profileVersion) < 2.6m)//m forces number to be interpreted as decimal
            {
                data.publishWeb = data.pubImage;
            }
            pubFtpUser.Text = data.pubFtpUser;
            pubFtpPass.Text = data.pubFtpPass;
            pubFtpRoot.Text = data.pubFtpRoot;

            if (data.motionLevel)
            {
                showLevel = true;
                levelShow.Image = TeboCam.Properties.Resources.nolevel;
            }
            else
            {
                showLevel = false;
                levelShow.Image = TeboCam.Properties.Resources.level;
                //levelDraw(0);
                LevelControlBox.levelDraw(0);
            }

            //lbl0Perc.Visible = showLevel;
            //lbl25Perc.Visible = showLevel;
            //lbl50Perc.Visible = showLevel;
            //lbl75Perc.Visible = showLevel;
            //lbl100Perc.Visible = showLevel;

            LevelControlBox.Visible = showLevel;

            webUpd.Checked = data.webUpd;
            sqlUser.Text = data.webUser;
            sqlPwd.Text = data.webPass;
            sqlPoll.Text = data.webPoll.ToString();
            sqlConString.Text = bubble.mysqlDriver;
            sqlInstance.Text = data.webInstance;
            sqlImageRoot.Text = data.webImageRoot;
            sqlImageFilename.Text = data.webImageFileName;
            SqlFtpUser.Text = data.webFtpUser;
            SqlFtpPwd.Text = data.webFtpPass;

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

            EmailIntelOn.Checked = data.EmailIntelOn;
            emailIntelEmails.Text = data.emailIntelEmails.ToString();
            emailIntelMins.Text = data.emailIntelMins.ToString();
            EmailIntelStop.Checked = data.EmailIntelStop;
            EmailIntelMosaic.Checked = !data.EmailIntelStop;

            rdStatsToFileOn.Checked = data.StatsToFileOn;
            pnlStatsToFile.Enabled = rdStatsToFileOn.Checked;
            chkStatsToFileTimeStamp.Checked = data.StatsToFileTimeStamp;
            txtStatsToFileMb.Text = data.StatsToFileMb.ToString();

            disCommOnline.Checked = data.disCommOnline;
            disCommOnlineSecs.Text = data.disCommOnlineSecs.ToString();
            disCommOnlineSecs.Enabled = disCommOnline.Checked;

            plSnd.Checked = data.soundAlertOn;
            logsKeep.Text = data.logsKeep.ToString();
            logsKeepChk.Checked = data.logsKeepChk;
            freezeGuardOn.Checked = data.freezeGuard;
            freezeGuardWindow.Checked = data.freezeGuardWindowShow;
            pulseFreq.Text = data.pulseFreq.ToString();
            numFrameRateCalcOver.Value = data.framesSecsToCalcOver;
            chkFrameRateTrack.Checked = data.framerateTrack;

            radioButton11.Checked = data.imageLocCust;

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


            lblstartpub.Text = "Scheduled start: " + LeftRightMid.Left(data.timerStartPub, 2) + ":" + LeftRightMid.Right(data.timerStartPub, 2);
            lblendpub.Text = "Scheduled end: " + LeftRightMid.Left(data.timerEndPub, 2) + ":" + LeftRightMid.Right(data.timerEndPub, 2);

            lblstartmov.Text = "Start: " + LeftRightMid.Left(data.timerStartMov, 2) + ":" + LeftRightMid.Right(data.timerStartMov, 2);
            lblendmov.Text = "End: " + LeftRightMid.Left(data.timerEndMov, 2) + ":" + LeftRightMid.Right(data.timerEndMov, 2);

            captureMovementImages.Checked = data.captureMovementImages;


            if (!captureMovementImages.Checked)
            {

                graphDate(DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture), "Image capture switched off");

            }


            txtInternetConnection.Text = data.internetCheck;

            if (!data.toolTips)
            { bttnToolTips.Text = "Turn ON Tool Tips"; }
            else
            { bttnToolTips.Text = "Turn OFF Tool Tips"; }

            toolTip1.Active = data.toolTips;

            startMinimized.Checked = data.startTeboCamMinimized;

            bubble.imageParentFolder = bubble.imageParentFolder = Application.StartupPath + @"\images\";
            bubble.imageFolder = bubble.imageParentFolder + @"fullSize\";
            bubble.thumbFolder = bubble.imageParentFolder + @"thumb\";

            if (radioButton11.Checked)
            {

                if (
                       Directory.Exists(config.getProfile(bubble.profileInUse).imageParentFolderCust)
                    && Directory.Exists(config.getProfile(bubble.profileInUse).imageFolderCust)
                    && Directory.Exists(config.getProfile(bubble.profileInUse).thumbFolderCust)
                    )
                {

                    bubble.imageParentFolder = config.getProfile(bubble.profileInUse).imageParentFolderCust;
                    bubble.imageFolder = config.getProfile(bubble.profileInUse).imageFolderCust;
                    bubble.thumbFolder = config.getProfile(bubble.profileInUse).thumbFolderCust;

                }
            }
            else
            {
                config.getProfile(bubble.profileInUse).imageParentFolderCust = bubble.imageParentFolder;
                config.getProfile(bubble.profileInUse).imageFolderCust = bubble.imageFolder;
                config.getProfile(bubble.profileInUse).thumbFolderCust = bubble.thumbFolder;
            }



        }

        private void clearOldLogs()
        {

            int deleteDate = Convert.ToInt32(DateTime.Now.AddDays(-config.getProfile(bubble.profileInUse).logsKeep).ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));

            DirectoryInfo dInfo = new DirectoryInfo(bubble.logFolder);
            FileInfo[] logFiles = dInfo.GetFiles("log_" + "*.xml");
            int fileCount = logFiles.Length;

            foreach (FileInfo file in logFiles)
            {

                int fileDate = Convert.ToInt32(LeftRightMid.Mid(file.Name, 4, 8));

                if (fileDate < deleteDate)
                {

                    File.Delete(bubble.logFolder + file.Name);

                }

            }

        }

        private void button12_Click(object sender, EventArgs e)
        {
            string newProfile = bubble.InputBox("Profile Name", "New Profile", "").Trim().Replace(" ", "");

            if (newProfile.Trim() != "")
            {

                config.addProfile(newProfile);
                profileListRefresh(bubble.profileInUse);

            }
            else
            {

                MessageBox.Show("Profile name must have 1 or more characters.", "Error");

            }

        }

        private void button10_Click(object sender, EventArgs e)
        {

            string origName = profileList.SelectedItem.ToString();
            string tmpStr = bubble.InputBox("New Profile Name", "Rename Profile", "").Trim().Replace(" ", "");

            if (tmpStr.Trim() != "")
            {

                config.renameProfile(origName, tmpStr);
                CameraRig.renameProfile(origName, tmpStr);
                bubble.profileInUse = tmpStr;
                profileListRefresh(bubble.profileInUse);

            }
            else
            {

                MessageBox.Show("Profile name must have 1 or more characters.", "Error");

            }

        }

        private void button11_Click(object sender, EventArgs e)
        {
            config.deleteProfile(profileList.SelectedItem.ToString());
            profileList.Items.Clear();

            foreach (string profile in config.getProfileList())
            {
                profileList.Items.Add(profile);
            }

            profileList.SelectedIndex = 0;
        }

        private void profileChanged(object sender, EventArgs e)
        {

            saveChanges();
            bubble.profileInUse = profileList.SelectedItem.ToString();
            configuration.profileInUse = profileList.SelectedItem.ToString();
            populateFromProfile(configuration.profileInUse);
            cameraNewProfile();

        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            string tmpStr = bubble.InputBox("New Profile Name", "Copy Profile", "").ToLower().Replace(" ", "");

            if (tmpStr.Trim() != "")
            {
                config.copyProfile(profileList.SelectedItem.ToString(), tmpStr);
                profileListRefresh(bubble.profileInUse);
            }
            else
            {
                MessageBox.Show("Profile name must have 1 or more characters.", "Error");
            }
        }


        private void profileListRefresh(string selectProfile)
        {
            profileList.Items.Clear();
            int tmpInt = 0;

            foreach (string profile in config.getProfileList())
            {
                profileList.Items.Add(profile);
                if (profile == selectProfile) tmpInt = profileList.Items.Count - 1;
            }

            profileList.SelectedIndex = tmpInt;
        }


        private void preferences_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized) Hide();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            Hide();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            bttnMotionActive.Checked = true;
            bttnMotionInactive.Checked = false;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            bttnMotionInactive.Checked = true;
            bttnMotionActive.Checked = false;
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void hideLog_Click(object sender, EventArgs e)
        {
            if (txtLog.Visible)
            {
                txtLog.Visible = false;
                hideLog.Text = "Show Log";
            }
            else
            {
                txtLog.Visible = true;
                hideLog.Text = "Hide Log";
            }
        }



        private void pubImage_CheckedChanged(object sender, EventArgs e)
        {

            config.getProfile(bubble.profileInUse).pubImage = pubImage.Checked;

            if (pubImage.Checked)
            {
                bubble.publishFirst = true;
                bubble.keepPublishing = true;
            }
            else
            {
                pubTimerOn.Checked = false;
                bubble.keepPublishing = false;
            }

        }

        private void pubTime_Leave(object sender, EventArgs e)
        {
            pubTime.Text = bubble.verifyInt(pubTime.Text.ToString(), 1, 99999, "1");
            if (CameraRig.ConnectedCameras.Count > 0)
            {
                var publishSelectedButton = PublishButtonGroupInstance.Where(x => x.CameraButtonState == CameraButtonGroup.ButtonState.ConnectedAndActive).First();
                var publishingCamera = CameraRig.ConnectedCameras.Where(x => x.displayButton == publishSelectedButton.id).First();
                CameraRig.updateInfo(bubble.profileInUse, publishingCamera.cameraName, CameraRig.infoEnum.pubTime, Convert.ToInt32(pubTime.Text));
                CameraRig.updateInfo(bubble.profileInUse, publishingCamera.cameraName, CameraRig.infoEnum.publishFirst, true);
            }
        }

        private void pubHours_CheckedChanged(object sender, EventArgs e)
        {
            if (CameraRig.ConnectedCameras.Count > 0)
            {
                var publishSelectedButton = PublishButtonGroupInstance.Where(x => x.CameraButtonState == CameraButtonGroup.ButtonState.ConnectedAndActive).First();
                var publishingCamera = CameraRig.ConnectedCameras.Where(x => x.displayButton == publishSelectedButton.id).First();
                CameraRig.updateInfo(bubble.profileInUse, publishingCamera.cameraName, CameraRig.infoEnum.pubHours, pubHours.Checked);
                CameraRig.updateInfo(bubble.profileInUse, publishingCamera.cameraName, CameraRig.infoEnum.publishFirst, true);
            }

        }

        private void pubMins_CheckedChanged(object sender, EventArgs e)
        {
            if (CameraRig.ConnectedCameras.Count > 0)
            {
                var publishSelectedButton = PublishButtonGroupInstance.Where(x => x.CameraButtonState == CameraButtonGroup.ButtonState.ConnectedAndActive).First();
                var publishingCamera = CameraRig.ConnectedCameras.Where(x => x.displayButton == publishSelectedButton.id).First();
                CameraRig.updateInfo(bubble.profileInUse, publishingCamera.cameraName, CameraRig.infoEnum.pubMins, pubMins.Checked);
                CameraRig.updateInfo(bubble.profileInUse, publishingCamera.cameraName, CameraRig.infoEnum.publishFirst, true);
            }
        }

        private void pubSecs_CheckedChanged(object sender, EventArgs e)
        {
            if (CameraRig.ConnectedCameras.Count > 0)
            {
                var publishSelectedButton = PublishButtonGroupInstance.Where(x => x.CameraButtonState == CameraButtonGroup.ButtonState.ConnectedAndActive).First();
                var publishingCamera = CameraRig.ConnectedCameras.Where(x => x.displayButton == publishSelectedButton.id).First();
                CameraRig.updateInfo(bubble.profileInUse, publishingCamera.cameraName, CameraRig.infoEnum.pubSecs, pubSecs.Checked);
                CameraRig.updateInfo(bubble.profileInUse, publishingCamera.cameraName, CameraRig.infoEnum.publishFirst, true);
            }
        }

        private void pubLocal_CheckedChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).publishLocal = pubToLocal.Checked;
            if (CameraRig.ConnectedCameras.Count > 0)
            {
                var publishSelectedButton = PublishButtonGroupInstance.Where(x => x.CameraButtonState == CameraButtonGroup.ButtonState.ConnectedAndActive).First();
                var publishingCamera = CameraRig.ConnectedCameras.Where(x => x.displayButton == publishSelectedButton.id).First();
                CameraRig.updateInfo(bubble.profileInUse, publishingCamera.cameraName, CameraRig.infoEnum.publishLocal, pubToLocal.Checked);
                CameraRig.updateInfo(bubble.profileInUse, publishingCamera.cameraName, CameraRig.infoEnum.publishFirst, true);
            }
        }

        private void pubWeb_CheckedChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).publishWeb = pubToWeb.Checked;

            if (CameraRig.ConnectedCameras.Count > 0)
            {
                var publishSelectedButton = PublishButtonGroupInstance.Where(x => x.CameraButtonState == CameraButtonGroup.ButtonState.ConnectedAndActive).First();
                var publishingCamera = CameraRig.ConnectedCameras.Where(x => x.displayButton == publishSelectedButton.id).First();
                CameraRig.updateInfo(bubble.profileInUse, publishingCamera.cameraName, CameraRig.infoEnum.publishWeb, pubToWeb.Checked);
                CameraRig.updateInfo(bubble.profileInUse, publishingCamera.cameraName, CameraRig.infoEnum.publishFirst, true);
            }
        }

        private void pubFtpUser_TextChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).pubFtpUser = pubFtpUser.Text;
        }

        private void pubFtpPass_TextChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).pubFtpPass = pubFtpPass.Text;
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).pubFtpRoot = pubFtpRoot.Text;
        }

        private void pubFtpCopy(object sender, EventArgs e)
        {

            string tmpUser = "";
            string tmpPass = "";
            string tmpRoot = "";

            tmpUser = config.getProfile(bubble.profileInUse).ftpUser;
            tmpPass = config.getProfile(bubble.profileInUse).ftpPass;
            tmpRoot = config.getProfile(bubble.profileInUse).ftpRoot;

            pubFtpUser.Text = tmpUser;
            pubFtpPass.Text = tmpPass;
            pubFtpRoot.Text = tmpRoot;


            config.getProfile(bubble.profileInUse).pubFtpUser = tmpUser;
            config.getProfile(bubble.profileInUse).pubFtpPass = tmpPass;
            config.getProfile(bubble.profileInUse).pubFtpRoot = tmpRoot;



        }

        private void button5_Click_1(object sender, EventArgs e)
        {

            bubble.openInternetBrowserAt(bubble.tebowebUrl);

        }

        private void pubTimerOn_CheckedChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).timerOn = pubTimerOn.Checked;

            if (CameraRig.ConnectedCameras.Count > 0)
            {
                int pubButton = PublishButtonGroupInstance.Where(x => x.CameraButtonState == CameraButtonGroup.ButtonState.ConnectedAndActive).First().id;
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.timerOn, pubTimerOn.Checked);
            }

            if (pubTimerOn.Checked)
            {
                lblstartpub.ForeColor = Color.Black;
                lblendpub.ForeColor = Color.Black;
            }
            else
            {
                lblstartpub.ForeColor = System.Drawing.SystemColors.Control;
                lblendpub.ForeColor = System.Drawing.SystemColors.Control;
            }


        }


        private void bttnMotionSchedule_CheckedChanged(object sender, EventArgs e)
        {

            config.getProfile(bubble.profileInUse).timerOnMov = bttnMotionSchedule.Checked;

            if (bttnMotionSchedule.Checked)
            {
                lblstartmov.ForeColor = Color.Black;
                lblendmov.ForeColor = Color.Black;
            }
            else
            {
                lblstartmov.ForeColor = System.Drawing.SystemColors.Control;
                lblendmov.ForeColor = System.Drawing.SystemColors.Control;
            }

        }


        private void bttnMotionScheduleOnAtStart_CheckedChanged(object sender, EventArgs e)
        {

            config.getProfile(bubble.profileInUse).scheduleOnAtStart = bttnMotionScheduleOnAtStart.Checked;

        }


        private void bttnActivateAtEveryStartup_CheckedChanged(object sender, EventArgs e)
        {

            config.getProfile(bubble.profileInUse).activateAtEveryStartup = bttnActivateAtEveryStartup.Checked;

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
                List<Camera.FrameRateInfo> frameInfo = CameraRig.ConnectedCameras[i].cam.frames.framesInfo;

                if (frameInfo.Count == 0)
                {
                    lblFrameRate.SynchronisedInvoke(() => lblFrameRate.Text = "0");
                    CameraRig.ConnectedCameras[i].cam.FrameRateCalculated = 0;
                    return;
                }

                DateTime start = frameInfo.First().dateTime;
                DateTime end = frameInfo.Last().dateTime;
                int frames = frameInfo.Count;
                int secondsElapsed = (int)(end - start).TotalSeconds;
                int avgFrames = secondsElapsed > 0 ? frames / secondsElapsed : 0;
                CameraRig.ConnectedCameras[i].cam.FrameRateCalculated = avgFrames;
                CameraRig.ConnectedCameras[i].cam.frames.purge(config.getProfile(bubble.profileInUse).framesSecsToCalcOver);
            }

            lblFrameRate.SynchronisedInvoke(() => lblFrameRate.Text = CameraRig.ConnectedCameras[CameraRig.CurrentlyDisplayingCamera].cam.FrameRateCalculated.ToString());
        }

        private void reconnectLostCameras()
        {
            for (int i = CameraRig.ConnectedCameras.Count() - 1; i >= 0; i--)
            {
                ConnectedCamera camera = CameraRig.ConnectedCameras[i];

                //let's drop and reconnect cameras providing zero framerates 
                if (camera.cam.frameRateTrack &&
                    camera.cam.FrameRateCalculated == 0 &&
                    camera.cam.frames.LastFrameSecondsAgo() > 10 &&
                    (DateTime.Now - camera.cam.ConnectedAt).TotalSeconds > 10)
                {
                    //get the camera source name
                    //then see if camera is in the available sources
                    filters = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                    foreach (FilterInfo filter in filters)
                    {
                        if (filter.MonikerString == camera.cameraName)
                        {
                            CameraButtonGroupInstance.Where(x => x.id == camera.displayButton).First().CameraButtonState = CameraButtonGroup.ButtonState.NotConnected;
                            int camNo = camera.cam.camNo;
                            string friendlyName = camera.friendlyName;
                            //drop the camera                 
                            CameraRig.cameraRemove(i);
                            //reconnect the camera
                            VideoCaptureDevice localSource = new VideoCaptureDevice(filter.MonikerString);
                            Camera cam = OpenVideoSource(localSource, null, false, camNo);
                            cam.frameRateTrack = config.getProfile(bubble.profileInUse).framerateTrack;
                            bubble.logAddLine(string.Format("Reconnecting lost [{0}] camera no. {1}.", friendlyName, cam.camNo.ToString()));
                        }
                    }
                }
            }

        }

        private void connectCamerasMissingAtStartup()
        {
            List<cameraSpecificInfo> expectedCameras = CameraRig.cameraCredentialsListedUnderProfile(bubble.profileInUse);

            foreach (cameraSpecificInfo expectedCamera in expectedCameras)
            {
                bool cameraConnected = CameraRig.ConnectedCameras.Where(x => x.cameraName == expectedCamera.webcam).Count() > 0;
                if (!cameraConnected)
                {
                    bool cameraAvailable = false;
                    filters = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                    foreach (FilterInfo filter in filters)
                    {
                        if (filter.MonikerString == expectedCamera.webcam)
                        {
                            cameraAvailable = true;
                            break;
                        }
                    }
                    if (cameraAvailable)
                    {
                        VideoCaptureDevice localSource = new VideoCaptureDevice(expectedCamera.webcam);
                        Camera cam = OpenVideoSource(localSource, null, false, -1);
                        cam.frameRateTrack = config.getProfile(bubble.profileInUse).framerateTrack;
                        bubble.logAddLine(string.Format("Connecting [{0}] camera not found at startup.", expectedCamera.friendlyName));
                    }
                }
            }
        }

        private void scheduler()
        {


            scheduleClass.scheduleAction checkScheduleResult = scheduleClass.scheduleAction.no_action;

            if (pubTimerOn.Checked || bttnMotionSchedule.Checked)
            {

                if (pubTimerOn.Checked)
                {

                    checkScheduleResult = scheduleStart(config.getProfile(bubble.profileInUse).timerStartPub,
                                                        config.getProfile(bubble.profileInUse).timerEndPub,
                                                        bubble.keepPublishing);


                    switch (checkScheduleResult)
                    {
                        case scheduleClass.scheduleAction.start:
                            publishSwitch(null, new EventArgs());
                            bubble.logAddLine("Web publish scheduled time start.");
                            break;
                        case scheduleClass.scheduleAction.end:
                            publishSwitch(null, new EventArgs());
                            bubble.logAddLine("Web publish scheduled time end.");
                            break;
                    }



                }

                if (bttnMotionSchedule.Checked)
                {

                    checkScheduleResult = scheduleStart(config.getProfile(bubble.profileInUse).timerStartMov,
                                                        config.getProfile(bubble.profileInUse).timerEndMov,
                                                        bubble.Alert.on);

                    switch (checkScheduleResult)
                    {
                        case scheduleClass.scheduleAction.start:
                            motionDetectionActivate(null, new EventArgs());
                            bubble.logAddLine("Motion active scheduled time start.");
                            break;
                        case scheduleClass.scheduleAction.end:
                            if (bttnMotionActive.Checked)
                            {
                                motionDetectionInactivate(null, new EventArgs());
                                bubble.logAddLine("Motion active scheduled time end.");
                            }
                            break;
                    }


                }


            }


        }



        private void publish_switch(object sender, System.EventArgs e)
        {
            pubImage.SynchronisedInvoke(() => pubImage.Checked = !bubble.keepPublishing);
            pubTimerOn.SynchronisedInvoke(() => pubTimerOn.Checked = true);
            //SetCheckBox(pubImage, !bubble.keepPublishing);
            //SetCheckBox(pubTimerOn, true);
        }

        private void smtpPort_Leave(object sender, EventArgs e)
        {
            smtpPort.Text = bubble.verifyInt(smtpPort.Text, 0, 99999, "25");
            config.getProfile(bubble.profileInUse).smtpPort = Convert.ToInt32(smtpPort.Text);
        }

        private void tabControl1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == "Images")
            {
                updateThumbs();
            }

            onlineVal.Enabled = bubble.DatabaseCredentialsCorrect;
            rdOnlinejpg.Enabled = bubble.DatabaseCredentialsCorrect;
            alertVal.Text = config.getProfile(bubble.profileInUse).alertCompression.ToString();
            pingVal.Text = config.getProfile(bubble.profileInUse).pingCompression.ToString();
            publishVal.Text = config.getProfile(bubble.profileInUse).publishCompression.ToString();
            onlineVal.Text = config.getProfile(bubble.profileInUse).onlineCompression.ToString();

        }


        private void sqlUser_Leave(object sender, EventArgs e)
        {
            if (sqlUser.Text != config.getProfile(bubble.profileInUse).webUser)
            {
                bubble.logAddLine("Web database credentials user changed.");
                config.getProfile(bubble.profileInUse).webUser = sqlUser.Text;
                bubble.DatabaseCredChkCount = 0;
                bubble.DatabaseCredentialsCorrect = false;
                bubble.webUpdLastChecked = 0;
                bubble.webFirstTimeThru = true;
            }
        }

        private void sqlPwd_Leave(object sender, EventArgs e)
        {
            if (sqlPwd.Text != config.getProfile(bubble.profileInUse).webPass)
            {
                bubble.logAddLine("Web database credentials password changed.");
                config.getProfile(bubble.profileInUse).webPass = sqlPwd.Text;
                bubble.DatabaseCredChkCount = 0;
                bubble.DatabaseCredentialsCorrect = false;
                bubble.webUpdLastChecked = 0;
                bubble.webFirstTimeThru = true;

            }
        }


        private void motionDetectionActivate(object sender, System.EventArgs e)
        {
            //inactivate motion detection first in case a countdown is taking place
            bttnMotionInactive.SynchronisedInvoke(() => bttnMotionInactive.Checked = true);
            bttnMotionActive.SynchronisedInvoke(() => bttnMotionActive.Checked = false);
            //SetRadioButton(bttnMotionInactive, true);
            //SetRadioButton(bttnMotionActive, false);

            Thread.Sleep(4000);

            //now activate motion detection
            bttnNow.SynchronisedInvoke(() => bttnNow.Checked = true);
            bttnTime.SynchronisedInvoke(() => bttnTime.Checked = false);
            bttnSeconds.SynchronisedInvoke(() => bttnSeconds.Checked = false);
            bttnMotionActive.SynchronisedInvoke(() => bttnMotionActive.Checked = true);
            bttnMotionInactive.SynchronisedInvoke(() => bttnMotionInactive.Checked = false);

            //SetRadioButton(bttnNow, true);
            //SetRadioButton(bttnTime, false);
            //SetRadioButton(bttnSeconds, false);

            //SetRadioButton(bttnMotionActive, true);
            //SetRadioButton(bttnMotionInactive, false);

            //state.motionDetectionActive = true;

        }

        private void motionDetectionInactivate(object sender, System.EventArgs e)
        {

            //20130427 restored as the scheduleOnAtStart property now takes care of reactivating at start up
            //if (bttnMotionSchedule.Checked) SetCheckBox(bttnMotionSchedule, false);
            if (bttnMotionSchedule.Checked) bttnMotionSchedule.SynchronisedInvoke(() => bttnMotionSchedule.Checked = false);

            bttnMotionInactive.SynchronisedInvoke(() => bttnMotionInactive.Checked = true);
            bttnMotionActive.SynchronisedInvoke(() => bttnMotionActive.Checked = false);

            //SetRadioButton(bttnMotionInactive, true);
            //SetRadioButton(bttnMotionActive, false);

            //state.motionDetectionActive = false;
        }

        private void maxImagesToEmail_Leave(object sender, EventArgs e)
        {
            maxImagesToEmail.Text = bubble.verifyInt(maxImagesToEmail.Text, 1, 9999, "1");
            config.getProfile(bubble.profileInUse).maxImagesToEmail = Convert.ToInt64(maxImagesToEmail.Text);
        }

        private void pingMins_Leave(object sender, EventArgs e)
        {
            pingMins.Text = bubble.verifyInt(pingMins.Text, 1, 9999, "1");
            config.getProfile(bubble.profileInUse).pingInterval = Convert.ToInt32(pingMins.Text);
        }

        private void emailNotifInterval_Leave(object sender, EventArgs e)
        {
            emailNotifInterval.Text = bubble.verifyInt(emailNotifInterval.Text, 1, 9999, "1");
            config.getProfile(bubble.profileInUse).emailNotifyInterval = Convert.ToInt64(emailNotifInterval.Text);
        }

        private void imageFileInterval_Leave(object sender, EventArgs e)
        {
            imageFileInterval.Text = bubble.verifyDouble(imageFileInterval.Text, 0.1, 9999, "1");
            config.getProfile(bubble.profileInUse).imageSaveInterval = Convert.ToDouble(imageFileInterval.Text);
        }

        private void sqlPoll_Leave(object sender, EventArgs e)
        {
            int tmpInt = config.getProfile(bubble.profileInUse).webPoll;
            sqlPoll.Text = bubble.verifyInt(sqlPoll.Text, 30, 9999, "30");

            if (Convert.ToInt32(sqlPoll.Text) != tmpInt)
            {
                config.getProfile(bubble.profileInUse).webPoll = Convert.ToInt32(sqlPoll.Text);
                bubble.webUpdLastChecked = 0;
                bubble.webFirstTimeThru = true;
            }

        }

        private void sqlConString_Leave(object sender, EventArgs e)
        {
            bubble.mysqlDriver = sqlConString.Text;
            configuration.mysqlDriver = sqlConString.Text;
        }

        private void webUpd_CheckedChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).webUpd = webUpd.Checked;
            bubble.webUpdLastChecked = 0;
            bubble.webFirstTimeThru = true;
        }



        private void plSnd_CheckedChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).soundAlertOn = plSnd.Checked;
            //FileManager.WriteFile("config");
            config.WebcamSettingsConfigDataPopulate();
            configuration.WriteXMLFile(bubble.xmlFolder + FileManager.configFile + ".xml", configuration);
            bubble.logAddLine("Config data saved.");
        }


        private void button13_Click(object sender, EventArgs e)
        {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "wav files (*.wav)|*.wav|wav files (*.*)|*.*";

            string soundFile = config.getProfile(bubble.profileInUse).soundAlert;

            if (soundFile != "")
            {
                dialog.InitialDirectory = Path.GetDirectoryName(soundFile);
                dialog.FileName = Path.GetFileName(soundFile);
            }

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                config.getProfile(bubble.profileInUse).soundAlert = dialog.FileName;
                //FileManager.WriteFile("config");
                config.WebcamSettingsConfigDataPopulate();
                configuration.WriteXMLFile(bubble.xmlFolder + FileManager.configFile + ".xml", configuration);
                bubble.logAddLine("Config data saved.");
            }

            plSnd.Enabled = config.getProfile(bubble.profileInUse).soundAlert != "";

        }

        private void sndTest_Click(object sender, EventArgs e)
        {

            string soundFile = config.getProfile(bubble.profileInUse).soundAlert;

            if (soundFile != "")
            {
                bubble.ringMyBell(true);
            }

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
            catch
            {
                contents.Clear();
                contents.Add("Unable to retrieve information.");
                return contents;
            }
        }




        private void newsInfo_Click(object sender, EventArgs e)
        {

            ArrayList news = new ArrayList();
            ArrayList info = new ArrayList();
            ArrayList whatsNew = new ArrayList();
            ArrayList license = new ArrayList();

            license = readTextFileintoArrayList(bubble.resourceFolder + "license.txt");
            info = readTextFileintoArrayList(bubble.resourceFolder + "tebocaminfo.txt");
            news = readTextFileintoArrayList(bubble.resourceFolder + "tebocamnews.txt");
            whatsNew = readTextFileintoArrayList(bubble.resourceFolder + "tebocamwhatsnew.txt");

            newsInfo.BackColor = System.Drawing.SystemColors.Control;
            News form = new News(news, info, whatsNew, license);
            form.Show();

        }


        private void logsKeep_Leave(object sender, EventArgs e)
        {

            logsKeep.Text = bubble.verifyInt(logsKeep.Text, 1, 99999, "30");
            config.getProfile(bubble.profileInUse).logsKeep = Convert.ToInt32(logsKeep.Text);

        }

        private void logsKeepChk_CheckedChanged_1(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).logsKeepChk = logsKeepChk.Checked;
        }

        private void freezeGuardOn_CheckedChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).freezeGuard = freezeGuardOn.Checked;
        }

        private void freezeGuardWindow_CheckedChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).freezeGuardWindowShow = freezeGuardWindow.Checked;
        }

        private void sqlImageRoot_Leave(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).webImageRoot = sqlImageRoot.Text;
            bubble.webFirstTimeThru = true;
        }

        private void sqlImageFilename_Leave(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).webImageFileName = sqlImageFilename.Text;
            bubble.webFirstTimeThru = true;
        }

        private void sqlInstance_Leave(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).webInstance = sqlInstance.Text;
        }

        private void SqlFtpUser_Leave(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).webFtpUser = SqlFtpUser.Text;
            bubble.webFirstTimeThru = true;
        }

        private void SqlFtpPwd_Leave(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).webFtpPass = SqlFtpPwd.Text;
            bubble.webFirstTimeThru = true;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            string tmpUser = "";
            string tmpPass = "";
            string tmpRoot = "";

            tmpUser = config.getProfile(bubble.profileInUse).ftpUser;
            tmpPass = config.getProfile(bubble.profileInUse).ftpPass;
            tmpRoot = config.getProfile(bubble.profileInUse).ftpRoot;

            SqlFtpUser.Text = tmpUser;
            SqlFtpPwd.Text = tmpPass;
            sqlImageRoot.Text = tmpRoot;

            config.getProfile(bubble.profileInUse).webFtpUser = tmpUser;
            config.getProfile(bubble.profileInUse).webFtpPass = tmpPass;
            config.getProfile(bubble.profileInUse).webImageRoot = tmpRoot;

        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).imageLocCust = radioButton11.Checked;
            button21.Enabled = radioButton11.Checked;

            if (!radioButton11.Checked)
            {

                //button21.Enabled = false;

                config.getProfile(bubble.profileInUse).imageParentFolderCust = bubble.imageParentFolder = Application.StartupPath + @"\images\";
                config.getProfile(bubble.profileInUse).imageFolderCust = bubble.imageParentFolder + @"fullSize\";
                config.getProfile(bubble.profileInUse).thumbFolderCust = bubble.imageParentFolder + @"thumb\";

                bubble.imageParentFolder = config.getProfile(bubble.profileInUse).imageParentFolderCust;
                bubble.imageFolder = config.getProfile(bubble.profileInUse).imageFolderCust;
                bubble.thumbFolder = config.getProfile(bubble.profileInUse).thumbFolderCust;

            }



        }

        private void customImageLocation_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = config.getProfile(bubble.profileInUse).imageParentFolderCust;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string parent = dialog.SelectedPath;


                if ((parent.Length < 7) || LeftRightMid.Right(parent, 7) != @"\images")
                {

                    config.getProfile(bubble.profileInUse).imageParentFolderCust = parent + @"\images\";
                    config.getProfile(bubble.profileInUse).imageFolderCust = config.getProfile(bubble.profileInUse).imageParentFolderCust + @"fullSize\";
                    config.getProfile(bubble.profileInUse).thumbFolderCust = config.getProfile(bubble.profileInUse).imageParentFolderCust + @"thumb\";

                    bubble.imageParentFolder = config.getProfile(bubble.profileInUse).imageParentFolderCust;
                    bubble.imageFolder = config.getProfile(bubble.profileInUse).imageFolderCust;
                    bubble.thumbFolder = config.getProfile(bubble.profileInUse).thumbFolderCust;


                    if (!Directory.Exists(config.getProfile(bubble.profileInUse).imageParentFolderCust))
                    {
                        Directory.CreateDirectory(config.getProfile(bubble.profileInUse).imageParentFolderCust);
                    }
                    if (!Directory.Exists(config.getProfile(bubble.profileInUse).imageFolderCust))
                    {
                        Directory.CreateDirectory(config.getProfile(bubble.profileInUse).imageFolderCust);
                    }
                    if (!Directory.Exists(config.getProfile(bubble.profileInUse).thumbFolderCust))
                    {
                        Directory.CreateDirectory(config.getProfile(bubble.profileInUse).thumbFolderCust);
                    }

                }

            }


        }





        private void button24_Click(object sender, EventArgs e)
        {
            saveChanges();
        }

        private void saveChanges()
        {
            //FileManager.WriteFile("config");
            config.WebcamSettingsConfigDataPopulate();
            configuration.WriteXMLFile(bubble.xmlFolder + FileManager.configFile + ".xml", configuration);
            bubble.logAddLine("Config data saved.");
        }

        private void startMinimized_CheckedChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).startTeboCamMinimized = startMinimized.Checked;
        }

        private void txtInternetConnection_Leave(object sender, EventArgs e)
        {

            if (txtInternetConnection.Text.Trim() == "") txtInternetConnection.Text = "www.google.com";
            config.getProfile(bubble.profileInUse).internetCheck = txtInternetConnection.Text;

        }


        private void bttnToolTips_Click_1(object sender, EventArgs e)
        {

            toolTip1.Active = !toolTip1.Active;

            if (!toolTip1.Active)
            { bttnToolTips.Text = "Turn ON Tool Tips"; }
            else
            { bttnToolTips.Text = "Turn OFF Tool Tips"; }

            config.getProfile(bubble.profileInUse).toolTips = toolTip1.Active;

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

            if (i[0].ToString() == "Alert")
            {
                config.getProfile(bubble.profileInUse).alertCompression = Convert.ToInt32(i[1].ToString());
                //SetLabel(alertVal, i[1].ToString());
                alertVal.SynchronisedInvoke(() => alertVal.Text = i[1].ToString());
            }

            if (i[0].ToString() == "Publish")
            {
                config.getProfile(bubble.profileInUse).publishCompression = Convert.ToInt32(i[1].ToString());
                //SetLabel(publishVal, i[1].ToString());
                publishVal.SynchronisedInvoke(() => publishVal.Text = i[1].ToString());

            }

            if (i[0].ToString() == "Ping")
            {
                config.getProfile(bubble.profileInUse).pingCompression = Convert.ToInt32(i[1].ToString());
                //SetLabel(pingVal, i[1].ToString());
                pingVal.SynchronisedInvoke(() => pingVal.Text = i[1].ToString());
            }

            if (i[0].ToString() == "Online")
            {
                config.getProfile(bubble.profileInUse).onlineCompression = Convert.ToInt32(i[1].ToString());
                //SetLabel(onlineVal, i[1].ToString());
                onlineVal.SynchronisedInvoke(() => onlineVal.Text = i[1].ToString());
            }

        }

        private void timeStampMth(List<List<object>> stampList)
        {

            foreach (List<object> item in stampList)
            {

                if (item[0].ToString() == "Online")
                {

                    config.getProfile(bubble.profileInUse).onlineTimeStamp = Convert.ToBoolean(item[1]);
                    config.getProfile(bubble.profileInUse).onlineTimeStampFormat = item[2].ToString();
                    config.getProfile(bubble.profileInUse).onlineTimeStampColour = item[3].ToString();
                    config.getProfile(bubble.profileInUse).onlineTimeStampPosition = item[4].ToString();
                    config.getProfile(bubble.profileInUse).onlineTimeStampRect = Convert.ToBoolean(item[5]);
                    config.getProfile(bubble.profileInUse).onlineStatsStamp = Convert.ToBoolean(item[7]);

                }

                if (item[0].ToString() == "Publish")
                {

                    config.getProfile(bubble.profileInUse).publishTimeStamp = Convert.ToBoolean(item[1]);
                    config.getProfile(bubble.profileInUse).publishTimeStampFormat = item[2].ToString();
                    config.getProfile(bubble.profileInUse).publishTimeStampColour = item[3].ToString();
                    config.getProfile(bubble.profileInUse).publishTimeStampPosition = item[4].ToString();
                    config.getProfile(bubble.profileInUse).publishTimeStampRect = Convert.ToBoolean(item[5]);
                    config.getProfile(bubble.profileInUse).publishStatsStamp = Convert.ToBoolean(item[7]);

                }

                if (item[0].ToString() == "Ping")
                {

                    config.getProfile(bubble.profileInUse).pingTimeStamp = Convert.ToBoolean(item[1]);
                    config.getProfile(bubble.profileInUse).pingTimeStampFormat = item[2].ToString();
                    config.getProfile(bubble.profileInUse).pingTimeStampColour = item[3].ToString();
                    config.getProfile(bubble.profileInUse).pingTimeStampPosition = item[4].ToString();
                    config.getProfile(bubble.profileInUse).pingTimeStampRect = Convert.ToBoolean(item[5]);
                    config.getProfile(bubble.profileInUse).pingStatsStamp = Convert.ToBoolean(item[7]);

                }


                if (item[0].ToString() == "Alert")
                {

                    config.getProfile(bubble.profileInUse).alertTimeStamp = Convert.ToBoolean(item[1]);
                    config.getProfile(bubble.profileInUse).alertTimeStampFormat = item[2].ToString();
                    config.getProfile(bubble.profileInUse).alertTimeStampColour = item[3].ToString();
                    config.getProfile(bubble.profileInUse).alertTimeStampPosition = item[4].ToString();
                    config.getProfile(bubble.profileInUse).alertTimeStampRect = Convert.ToBoolean(item[5]);
                    config.getProfile(bubble.profileInUse).alertStatsStamp = Convert.ToBoolean(item[7]);

                }




            }


        }

        private void timeStampMthOld(ArrayList i)
        {

            if (i[0].ToString() == "Online")
            {
                config.getProfile(bubble.profileInUse).onlineTimeStamp = Convert.ToBoolean(i[1]);
                config.getProfile(bubble.profileInUse).onlineTimeStampFormat = i[2].ToString();
                config.getProfile(bubble.profileInUse).onlineTimeStampColour = i[3].ToString();
                config.getProfile(bubble.profileInUse).onlineTimeStampPosition = i[4].ToString();
                config.getProfile(bubble.profileInUse).onlineTimeStampRect = Convert.ToBoolean(i[5]);
                config.getProfile(bubble.profileInUse).onlineStatsStamp = Convert.ToBoolean(i[6]);
            }

            if (i[0].ToString() == "Publish")
            {
                config.getProfile(bubble.profileInUse).publishTimeStamp = Convert.ToBoolean(i[1]);
                config.getProfile(bubble.profileInUse).publishTimeStampFormat = i[2].ToString();
                config.getProfile(bubble.profileInUse).publishTimeStampColour = i[3].ToString();
                config.getProfile(bubble.profileInUse).publishTimeStampPosition = i[4].ToString();
                config.getProfile(bubble.profileInUse).publishTimeStampRect = Convert.ToBoolean(i[5]);
                config.getProfile(bubble.profileInUse).publishStatsStamp = Convert.ToBoolean(i[6]);
            }

            if (i[0].ToString() == "Ping")
            {
                config.getProfile(bubble.profileInUse).pingTimeStamp = Convert.ToBoolean(i[1]);
                config.getProfile(bubble.profileInUse).pingTimeStampFormat = i[2].ToString();
                config.getProfile(bubble.profileInUse).pingTimeStampColour = i[3].ToString();
                config.getProfile(bubble.profileInUse).pingTimeStampPosition = i[4].ToString();
                config.getProfile(bubble.profileInUse).pingTimeStampRect = Convert.ToBoolean(i[5]);
                config.getProfile(bubble.profileInUse).pingStatsStamp = Convert.ToBoolean(i[6]);
            }

            if (i[0].ToString() == "Alert")
            {
                config.getProfile(bubble.profileInUse).alertTimeStamp = Convert.ToBoolean(i[1]);
                config.getProfile(bubble.profileInUse).alertTimeStampFormat = i[2].ToString();
                config.getProfile(bubble.profileInUse).alertTimeStampColour = i[3].ToString();
                config.getProfile(bubble.profileInUse).alertTimeStampPosition = i[4].ToString();
                config.getProfile(bubble.profileInUse).alertTimeStampRect = Convert.ToBoolean(i[5]);
                config.getProfile(bubble.profileInUse).alertStatsStamp = Convert.ToBoolean(i[6]);
            }

        }


        private void button16_Click_1(object sender, EventArgs e)
        {

            ArrayList i = new ArrayList();

            if (rdAlertjpg.Checked)
            {
                i.Add("Alert");
                i.Add(config.getProfile(bubble.profileInUse).alertCompression);
            }
            if (rdPingjpg.Checked)
            {
                i.Add("Ping");
                i.Add(config.getProfile(bubble.profileInUse).pingCompression);
            }
            if (rdPublishjpg.Checked)
            {
                i.Add("Publish");
                i.Add(config.getProfile(bubble.profileInUse).publishCompression);
            }
            if (rdOnlinejpg.Checked)
            {
                i.Add("Online");
                i.Add(config.getProfile(bubble.profileInUse).onlineCompression);
            }

            i.Add(config.getProfile(bubble.profileInUse).toolTips);
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
            alertList.Add(config.getProfile(bubble.profileInUse).alertTimeStamp);
            alertList.Add(config.getProfile(bubble.profileInUse).alertTimeStampFormat);
            alertList.Add(config.getProfile(bubble.profileInUse).alertTimeStampColour);
            alertList.Add(config.getProfile(bubble.profileInUse).alertTimeStampPosition);
            alertList.Add(config.getProfile(bubble.profileInUse).alertTimeStampRect);
            alertList.Add(false);
            alertList.Add(config.getProfile(bubble.profileInUse).alertStatsStamp);

            pingList.Add("Ping");
            pingList.Add(config.getProfile(bubble.profileInUse).pingTimeStamp);
            pingList.Add(config.getProfile(bubble.profileInUse).pingTimeStampFormat);
            pingList.Add(config.getProfile(bubble.profileInUse).pingTimeStampColour);
            pingList.Add(config.getProfile(bubble.profileInUse).pingTimeStampPosition);
            pingList.Add(config.getProfile(bubble.profileInUse).pingTimeStampRect);
            pingList.Add(true);
            pingList.Add(config.getProfile(bubble.profileInUse).pingStatsStamp);

            publishList.Add("Publish");
            publishList.Add(config.getProfile(bubble.profileInUse).publishTimeStamp);
            publishList.Add(config.getProfile(bubble.profileInUse).publishTimeStampFormat);
            publishList.Add(config.getProfile(bubble.profileInUse).publishTimeStampColour);
            publishList.Add(config.getProfile(bubble.profileInUse).publishTimeStampPosition);
            publishList.Add(config.getProfile(bubble.profileInUse).publishTimeStampRect);
            publishList.Add(true);
            publishList.Add(config.getProfile(bubble.profileInUse).publishStatsStamp);

            onlineList.Add("Online");
            onlineList.Add(config.getProfile(bubble.profileInUse).onlineTimeStamp);
            onlineList.Add(config.getProfile(bubble.profileInUse).onlineTimeStampFormat);
            onlineList.Add(config.getProfile(bubble.profileInUse).onlineTimeStampColour);
            onlineList.Add(config.getProfile(bubble.profileInUse).onlineTimeStampPosition);
            onlineList.Add(config.getProfile(bubble.profileInUse).onlineTimeStampRect);
            onlineList.Add(false);
            onlineList.Add(config.getProfile(bubble.profileInUse).onlineStatsStamp);

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

            if (!bubble.drawMode)
            {
                imageInFrame_Click(this, null);
            }


        }


        private void imageInFrame_Click(object sender, EventArgs e)
        {
            if (config.getProfile(bubble.profileInUse).imageToframe)
            {
                imageInFrame.Image = TeboCam.Properties.Resources.arrowIn;
                config.getProfile(bubble.profileInUse).imageToframe = false;
                cameraWindow.imageToFrame = false;
                panel1.AutoScroll = true;
            }
            else
            {
                imageInFrame.Image = TeboCam.Properties.Resources.arrowOut;
                config.getProfile(bubble.profileInUse).imageToframe = true;
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
                config.getProfile(bubble.profileInUse).cameraShow = true;
            }
            else
            {
                cameraShow.Image = TeboCam.Properties.Resources.landscape;
                config.getProfile(bubble.profileInUse).cameraShow = false;
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

            config.getProfile(bubble.profileInUse).motionLevel = showLevel;
            LevelControlBox.Visible = showLevel;
        }





        private void bttncam1_Click(object sender, EventArgs e)
        {
            cameraSwitch(1, true, false);
        }
        private void bttncam2_Click(object sender, EventArgs e)
        {
            cameraSwitch(2, true, false);
        }
        private void bttncam3_Click(object sender, EventArgs e)
        {
            cameraSwitch(3, true, false);
        }
        private void bttncam4_Click(object sender, EventArgs e)
        {
            cameraSwitch(4, true, false);
        }
        private void bttncam5_Click(object sender, EventArgs e)
        {
            cameraSwitch(5, true, false);
        }
        private void bttncam6_Click(object sender, EventArgs e)
        {
            cameraSwitch(6, true, false);
        }
        private void bttncam7_Click(object sender, EventArgs e)
        {
            cameraSwitch(7, true, false);
        }
        private void bttncam8_Click(object sender, EventArgs e)
        {
            cameraSwitch(8, true, false);
        }
        private void bttncam9_Click(object sender, EventArgs e)
        {
            cameraSwitch(9, true, false);
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
            bool canClick = CameraButtonGroupInstance.Where(x => x.id == button && x.CameraButtonState == CameraButtonGroup.ButtonState.ConnectedAndActive).Count() > 0;
            if (!canClick) return false;

            var connected = CameraButtonGroupInstance.Where(x => x.CameraButtonState != CameraButtonGroup.ButtonState.NotConnected).ToList();
            connected.ForEach(x => x.CameraButtonState = CameraButtonGroup.ButtonState.ConnectedAndInactive);
            var newActiveButton = CameraButtonGroupInstance.Where(x => x.id == button).First();
            newActiveButton.CameraButtonState = CameraButtonGroup.ButtonState.ConnectedAndInactive;
            return true;

        }


        private void cameraSwitch(int button, bool refresh, bool load)
        {

            int camId = CameraRig.idxFromButton(button);



            //ToDo here camButtons.camClick(button) returns false
            if (load || !load && camButtons.camClick(button))
            {
                if (load || !load && CameraRig.cameraExists(camId))
                {

                    CameraRig.CurrentlyDisplayingCamera = camId;
                    CameraRig.ConnectedCameras[camId].cam.MotionDetector.Reset();
                    //set the camerawindow bitmap
                    cameraWindow.Camera = CameraRig.ConnectedCameras[camId].cam;
                    lblCameraName.SynchronisedInvoke(() => lblCameraName.Visible = CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.ConnectedCameras[camId].cameraName, CameraRig.infoEnum.friendlyName).ToString().Trim() != string.Empty);
                    lblCameraName.SynchronisedInvoke(() => lblCameraName.Text = CameraRig.ConnectedCameras[camId].friendlyName);
                    config.getProfile(bubble.profileInUse).selectedCam = CameraRig.ConnectedCameras[camId].cameraName;

                    if (refresh) cameraWindow.Refresh();
                    camButtonSetColours();
                }

            }
        }

        private void bttncam1sel_Click(object sender, EventArgs e) { selcam(this.bttncam1sel, 1); }
        private void bttncam2sel_Click(object sender, EventArgs e) { selcam(this.bttncam2sel, 2); }
        private void bttncam3sel_Click(object sender, EventArgs e) { selcam(this.bttncam3sel, 3); }
        private void bttncam4sel_Click(object sender, EventArgs e) { selcam(this.bttncam4sel, 4); }
        private void bttncam5sel_Click(object sender, EventArgs e) { selcam(this.bttncam5sel, 5); }
        private void bttncam6sel_Click(object sender, EventArgs e) { selcam(this.bttncam6sel, 6); }
        private void bttncam7sel_Click(object sender, EventArgs e) { selcam(this.bttncam7sel, 7); }
        private void bttncam8sel_Click(object sender, EventArgs e) { selcam(this.bttncam8sel, 8); }
        private void bttncam9sel_Click(object sender, EventArgs e) { selcam(this.bttncam9sel, 9); }



        private void selcam(Button btn, int button)
        {

            int cam = CameraRig.idxFromButton(button);
            //bool currentlyPublishing = PublishButtonGroupInstance.Where(x => x.id == button && x.CameraButtonState == CameraButtonGroup.ButtonState.ConnectedAndActive).Count() > 0;

            camButtons.ButtonColourEnum result = camButtons.motionSenseClick(button);

            if (result == camButtons.ButtonColourEnum.grey)
            {

                licence.deselectCam(cam + 1);
                btn.BackColor = Color.Silver;
                CameraRig.ConnectedCameras[cam].cam.alarmActive = false;
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.ConnectedCameras[cam].cameraName, CameraRig.infoEnum.alarmActive, false);
                CameraRig.ConnectedCameras[cam].cam.detectionOn = false;

            }
            if (result == camButtons.ButtonColourEnum.green)
            {

                licence.selectCam(cam + 1);
                btn.BackColor = Color.LawnGreen;
                CameraRig.ConnectedCameras[cam].cam.alarmActive = true;
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.ConnectedCameras[cam].cameraName, CameraRig.infoEnum.alarmActive, true);
                CameraRig.ConnectedCameras[cam].cam.detectionOn = true;

            }

            camButtonSetColours();

        }


        private void publishRefresh(int button)
        {

            int pubButton = CameraRig.idxFromButton(button);
            pubTime.Text = CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.pubTime).ToString();
            pubHours.Checked = (bool)CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.pubHours);
            pubMins.Checked = (bool)CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.pubMins);
            pubSecs.Checked = (bool)CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.pubSecs);
            pubToWeb.Checked = (bool)CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.publishWeb);
            pubToLocal.Checked = (bool)CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.publishLocal);
            pubTimerOn.Checked = (bool)CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.timerOn);
        }

        private void pubcam(Button btn, int button)
        {
            
            if (CameraButtonGroupInstance.Where(x => x.id == button && x.CameraButtonState != CameraButtonGroup.ButtonState.NotConnected).Count() > 0)
            {

                int cam = CameraRig.idxFromButton(button);
                //unpublish other cameras
                PublishButtonGroupInstance.ForEach(x => x.CameraButtonState = CameraButtonGroup.ButtonState.NotConnected);
                var selectedButton = PublishButtonGroupInstance.Where(x => x.id == button).First();
                selectedButton.CameraButtonState = CameraButtonGroup.ButtonState.ConnectedAndActive;

                foreach (ConnectedCamera item in CameraRig.ConnectedCameras)
                {

                    if (item.displayButton != button)
                    {

                        item.cam.publishActive = false;
                        CameraRig.updateInfo(bubble.profileInUse, item.cameraName, CameraRig.infoEnum.publishActive, false);

                    }

                }

                bool currentlyPublishing = PublishButtonGroupInstance.Where(x => x.id == button && x.CameraButtonState == CameraButtonGroup.ButtonState.ConnectedAndActive).Count() > 0;


                if (!currentlyPublishing)
                {
                    PublishButtonGroupInstance.Where(x => x.id == button && x.CameraButtonState == CameraButtonGroup.ButtonState.ConnectedAndActive).First().CameraButtonState = CameraButtonGroup.ButtonState.ConnectedAndInactive;
                    CameraRig.ConnectedCameras[cam].cam.publishActive = false;
                    CameraRig.updateInfo(bubble.profileInUse, CameraRig.ConnectedCameras[cam].cameraName, CameraRig.infoEnum.publishActive, false);
                }
                else
                {
                    PublishButtonGroupInstance.Where(x => x.id == button && x.CameraButtonState == CameraButtonGroup.ButtonState.ConnectedAndActive).First().CameraButtonState = CameraButtonGroup.ButtonState.ConnectedAndActive;
                    CameraRig.ConnectedCameras[cam].cam.publishActive = true;
                    CameraRig.updateInfo(bubble.profileInUse, CameraRig.ConnectedCameras[cam].cameraName, CameraRig.infoEnum.publishActive, true);
                }

                camButtonSetColours();
                publishRefresh(button);

            }

        }


        private void camButtonSetColours()
        {

            foreach (var buttonGroup in CameraButtonGroupInstance)
            {
                //display camera buttons
                if (buttonGroup.id == CameraRig.ConnectedCameras[CameraRig.CurrentlyDisplayingCamera].displayButton)
                {
                    buttonGroup.CameraButtonIsActive();
                }
                else
                {
                    if (CameraRig.CameraConnectedToButton(buttonGroup.id))
                    {
                        buttonGroup.CameraButtonIsInactive();
                    }
                    else
                    {
                        buttonGroup.CameraButtonIsNotConnected();
                    }
                }

                //activate motion detection camera buttons
                bool detectionOn = CameraRig.ConnectedCameras.Where(x => x.displayButton == buttonGroup.id && x.cam.alarmActive).Count() > 0;
                if (detectionOn)
                {
                    buttonGroup.ActiveButtonIsActive();
                }
                else
                {
                    buttonGroup.ActiveButtonIsInactive();
                }
            }

            //publish camera buttons
            foreach (var buttonGroup in PublishButtonGroupInstance)
            {
                bool publishOn = CameraRig.ConnectedCameras.Where(x => x.displayButton == buttonGroup.id && x.cam.publishActive).Count() > 0;
                if (publishOn)
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
            CameraButtonGroupInstance.ForEach(x => x.CameraButtonIsNotConnected());
        }


        private void bttnCamProp_Click(object sender, EventArgs e)
        {
            VideoCaptureDevice localSource = new VideoCaptureDevice(CameraRig.ConnectedCameras[CameraRig.CurrentlyDisplayingCamera].cameraName);
            localSource.DisplayPropertyPage(IntPtr.Zero); // non-modal
        }

        private void button19_Click_1(object sender, EventArgs e)
        {

            if (!Directory.Exists(bubble.vaultFolder)) Directory.CreateDirectory(bubble.vaultFolder);

            string configVlt = bubble.vaultFolder + FileManager.configFile + "_" + DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture) + ".xml";
            string configXml = bubble.xmlFolder + FileManager.configFile + ".xml";

            File.Copy(configXml, configVlt, true);
            MessageBox.Show(FileManager.configFile + ".xml has been successfully vaulted in the vault folder.", "File Vaulted");




        }

        private void button23_Click_1(object sender, EventArgs e)
        {
            ArrayList i = new ArrayList();
            i.Add(config.getProfile(bubble.profileInUse).toolTips);
            i.Add(CameraRig.CurrentlyDisplayingCamera);
            i.Add(panel1.AutoScroll);
            i.Add(camButtons.buttons());
            bubble.motionLevelChanged -= new EventHandler(drawLevel);
            LevelControlBox.levelDraw(0);
            webcamConfig webcamConfig = new webcamConfig(new formDelegate(webcamConfigCompleted), i);
            webcamConfig.StartPosition = FormStartPosition.CenterScreen;
            webcamConfig.ShowDialog();
        }



        private void webcamConfigCompleted(ArrayList i)
        {

            //System.Diagnostics.Debug.WriteLine(CameraRig.cameraCount());


            bubble.motionLevelChanged -= new EventHandler(drawLevel);
            bubble.motionLevelChanged += new EventHandler(drawLevel);



            if (CameraRig.cameraCount() > 0)
            {

                //give the interface some time to refresh
                Thread.Sleep(250);
                //give the interface some time to refresh
                cameraSwitch(CameraRig.ConnectedCameras[CameraRig.drawCam].displayButton, true, true);

            }

            panel1.AutoScroll = (bool)i[1];
            i.Clear();

            bubble.motionLevelChanged -= new EventHandler(drawLevel);
            bubble.motionLevelChanged += new EventHandler(drawLevel);

            camButtonSetColours();

        }




        private void bttncam1pub_Click(object sender, EventArgs e)
        {
            pubcam(bttncam1pub, 1);
        }
        private void bttncam2pub_Click(object sender, EventArgs e)
        {
            pubcam(bttncam2pub, 2);
        }
        private void bttncam3pub_Click(object sender, EventArgs e)
        {
            pubcam(bttncam3pub, 3);
        }
        private void bttncam4pub_Click(object sender, EventArgs e)
        {
            pubcam(bttncam4pub, 4);
        }
        private void bttncam5pub_Click(object sender, EventArgs e)
        {
            pubcam(bttncam5pub, 5);
        }
        private void bttncam6pub_Click(object sender, EventArgs e)
        {
            pubcam(bttncam6pub, 6);
        }
        private void bttncam7pub_Click(object sender, EventArgs e)
        {
            pubcam(bttncam7pub, 7);
        }
        private void bttncam8pub_Click(object sender, EventArgs e)
        {
            pubcam(bttncam8pub, 8);
        }
        private void bttncam9pub_Click(object sender, EventArgs e)
        {
            pubcam(bttncam9pub, 9);
        }




        public void cameraNewProfile()
        {

            closeAllCameras();
            CameraRig.clear();
            camReset();

            //check if cw is null as we may currently be loading the form 
            //and cw may be in progress
            if (config.getProfile(bubble.profileInUse).webcam != null && cw == null)
            {
                BackgroundWorker profChWorker = new BackgroundWorker();
                profChWorker.DoWork -= new DoWorkEventHandler(waitForCam);
                profChWorker.DoWork += new DoWorkEventHandler(waitForCam);
                profChWorker.WorkerSupportsCancellation = true;
                profChWorker.RunWorkerAsync();
                profChWorker = null;
            }


        }


        private void button15_Click(object sender, EventArgs e)
        {

            if (button15.Text == "Test FreezeGuard")
            {

                pulseStopEvent(null, new EventArgs());
                button15.Text = "Pulse Stopped";

            }
            else
            {

                pulseStartEvent(null, new EventArgs());
                button15.Text = "Test FreezeGuard";

            }


        }

        private void infoMode_CheckedChanged(object sender, EventArgs e)
        {
            teboDebug.debugOn = infoMode.Checked;
        }

        private void button34_Click(object sender, EventArgs e)
        {

        }



        private void bttnSetPrefixPublish_Click(object sender, EventArgs e)
        {

            if (CameraRig.ConnectedCameras.Count > 0)
            {

                ArrayList i = new ArrayList();

                int pubButton = PublishButtonGroupInstance.Where(x => x.CameraButtonState == CameraButtonGroup.ButtonState.ConnectedAndActive).First().id;

                i.Add("Publish Web");
                i.Add(config.getProfile(bubble.profileInUse).toolTips);
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.filenamePrefixPubWeb));
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.cycleStampCheckedPubWeb));
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.startCyclePubWeb));
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.endCyclePubWeb));
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.currentCyclePubWeb));
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.stampAppendPubWeb));
                i.Add(true);
                i.Add("");
                i.Add("");
                i.Add(false);
                i.Add(false);

                //i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "fileURLPubWeb"));
                //i.Add(config.getProfile(bubble.profileInUse).pubFtpRoot);
                //i.Add(false);


                fileprefix fileprefix = new fileprefix(new formDelegate(filePrefixSet), i);
                fileprefix.StartPosition = FormStartPosition.CenterScreen;
                fileprefix.ShowDialog();

            }

        }



        private void filePrefixSet(ArrayList i)
        {
            int pubButton = PublishButtonGroupInstance.Where(x => x.CameraButtonState == CameraButtonGroup.ButtonState.ConnectedAndActive).First().id;
            CameraRig.updateInfo(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.publishFirst, true);

            if (i[0].ToString() == "Publish Web")
            {

                CameraRig.updateInfo(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.filenamePrefixPubWeb, i[1].ToString());
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.cycleStampCheckedPubWeb, Convert.ToInt32(i[2]));
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.startCyclePubWeb, Convert.ToInt32(i[3]));
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.endCyclePubWeb, Convert.ToInt32(i[4]));
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.currentCyclePubWeb, Convert.ToInt32(i[5]));
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.stampAppendPubWeb, Convert.ToBoolean(i[6]));

            }

            if (i[0].ToString() == "Publish Local")
            {

                CameraRig.updateInfo(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.filenamePrefixPubLoc, i[1].ToString());
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.cycleStampCheckedPubLoc, Convert.ToInt32(i[2]));
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.startCyclePubLoc, Convert.ToInt32(i[3]));
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.endCyclePubLoc, Convert.ToInt32(i[4]));
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.currentCyclePubLoc, Convert.ToInt32(i[5]));
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.stampAppendPubLoc, Convert.ToBoolean(i[6]));
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.fileDirPubLoc, i[7].ToString());
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.fileDirPubCust, Convert.ToBoolean(i[8]));


            }

            if (i[0].ToString() == "Alert")
            {

                config.getProfile(bubble.profileInUse).filenamePrefix = i[1].ToString();
                config.getProfile(bubble.profileInUse).cycleStampChecked = Convert.ToInt32(i[2]);
                config.getProfile(bubble.profileInUse).startCycle = Convert.ToInt32(i[3]);
                config.getProfile(bubble.profileInUse).endCycle = Convert.ToInt32(i[4]);
                config.getProfile(bubble.profileInUse).currentCycle = Convert.ToInt32(i[5]);

                lblImgPref.Text = "Image Prefix: " + i[1].ToString() + "   e.g " + i[1].ToString() + "1" + bubble.ImgSuffix;

            }

        }


        private void button36_Click(object sender, EventArgs e)
        {

            if (CameraRig.ConnectedCameras.Count > 0)
            {
                ArrayList i = new ArrayList();
                int pubButton = PublishButtonGroupInstance.Where(x => x.CameraButtonState == CameraButtonGroup.ButtonState.ConnectedAndActive).First().id;

                i.Add("Publish Local");
                i.Add(config.getProfile(bubble.profileInUse).toolTips);
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.filenamePrefixPubLoc));
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.cycleStampCheckedPubLoc));
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.startCyclePubLoc));
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.endCyclePubLoc));
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.currentCyclePubLoc));
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.stampAppendPubLoc));
                i.Add(true);
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.fileDirPubLoc));
                i.Add(bubble.imageFolder);
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.ConnectedCameras[pubButton].cameraName, CameraRig.infoEnum.fileDirPubCust));
                i.Add(true);



                fileprefix fileprefix = new fileprefix(new formDelegate(filePrefixSet), i);
                fileprefix.StartPosition = FormStartPosition.CenterScreen;
                fileprefix.ShowDialog();

            }

        }


        //private void button35_ClickNEW(object sender, EventArgs e)
        //{

        //    int alertButton = CameraRig.idxFromButton(camButtons.firstActiveButton());

        //    ArrayList i = new ArrayList();

        //    i.Add("Alert");
        //    i.Add(config.getProfile(bubble.profileInUse).toolTips);
        //    i.Add(config.getProfile(bubble.profileInUse).filenamePrefix);
        //    i.Add(config.getProfile(bubble.profileInUse).cycleStampChecked);
        //    i.Add(config.getProfile(bubble.profileInUse).startCycle);
        //    i.Add(config.getProfile(bubble.profileInUse).endCycle);
        //    i.Add(config.getProfile(bubble.profileInUse).currentCycle);
        //    i.Add(true);
        //    i.Add(true);
        //    i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.ConnectedCameras[alertButton].cameraName, CameraRig.infoEnum.fileDirAlertLoc));
        //    i.Add(bubble.imageFolder);
        //    i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.ConnectedCameras[alertButton].cameraName, CameraRig.infoEnum.fileDirAlertCust));
        //    i.Add(true);

        //    fileprefix fileprefix = new fileprefix(new formDelegate(filePrefixSet), i);
        //    fileprefix.StartPosition = FormStartPosition.CenterScreen;
        //    fileprefix.ShowDialog();

        //}

        private void button35_Click(object sender, EventArgs e)
        {

            ArrayList i = new ArrayList();

            i.Add("Alert");
            i.Add(config.getProfile(bubble.profileInUse).toolTips);
            i.Add(config.getProfile(bubble.profileInUse).filenamePrefix);
            i.Add(config.getProfile(bubble.profileInUse).cycleStampChecked);
            i.Add(config.getProfile(bubble.profileInUse).startCycle);
            i.Add(config.getProfile(bubble.profileInUse).endCycle);
            i.Add(config.getProfile(bubble.profileInUse).currentCycle);
            i.Add(true);
            i.Add(false);
            i.Add("");
            i.Add("");
            i.Add(false);
            i.Add(false);

            fileprefix fileprefix = new fileprefix(new formDelegate(filePrefixSet), i);
            fileprefix.StartPosition = FormStartPosition.CenterScreen;
            fileprefix.ShowDialog();

        }



        private void button37_Click(object sender, EventArgs e)
        {

            ArrayList i = new ArrayList();

            i.Add("Publish");
            i.Add(config.getProfile(bubble.profileInUse).toolTips);
            i.Add(config.getProfile(bubble.profileInUse).timerStartPub);
            i.Add(config.getProfile(bubble.profileInUse).timerEndPub);

            schedule schedule = new schedule(new formDelegate(scheduleSet), i);
            schedule.StartPosition = FormStartPosition.CenterScreen;
            schedule.ShowDialog();

        }

        private void button38_Click(object sender, EventArgs e)
        {

            ArrayList i = new ArrayList();

            i.Add("Alert");
            i.Add(config.getProfile(bubble.profileInUse).toolTips);
            i.Add(config.getProfile(bubble.profileInUse).timerStartMov);
            i.Add(config.getProfile(bubble.profileInUse).timerEndMov);

            schedule schedule = new schedule(new formDelegate(scheduleSet), i);
            schedule.StartPosition = FormStartPosition.CenterScreen;
            schedule.ShowDialog();

        }

        private void scheduleSet(ArrayList i)
        {

            if (i[0].ToString() == "Publish")
            {
                config.getProfile(bubble.profileInUse).timerStartPub = i[1].ToString();
                config.getProfile(bubble.profileInUse).timerEndPub = i[2].ToString();
                lblstartpub.Text = "Scheduled start: " + LeftRightMid.Left(i[1].ToString(), 2) + ":" + LeftRightMid.Right(i[1].ToString(), 2);
                lblendpub.Text = "Scheduled end: " + LeftRightMid.Left(i[2].ToString(), 2) + ":" + LeftRightMid.Right(i[2].ToString(), 2);
                Invalidate();
            }

            if (i[0].ToString() == "Alert")
            {
                config.getProfile(bubble.profileInUse).timerStartMov = i[1].ToString();
                config.getProfile(bubble.profileInUse).timerEndMov = i[2].ToString();
                lblstartmov.Text = "Start: " + LeftRightMid.Left(i[1].ToString(), 2) + ":" + LeftRightMid.Right(i[1].ToString(), 2);
                lblendmov.Text = "End: " + LeftRightMid.Left(i[2].ToString(), 2) + ":" + LeftRightMid.Right(i[2].ToString(), 2);
                Invalidate();
            }

        }


        private void mosaicImagesPerRow_Leave(object sender, EventArgs e)
        {
            mosaicImagesPerRow.Text = bubble.verifyInt(mosaicImagesPerRow.Text, 1, 100, "1");
            config.getProfile(bubble.profileInUse).mosaicImagesPerRow = Convert.ToInt32(mosaicImagesPerRow.Text);
        }


        private void pulseFreq_Leave(object sender, EventArgs e)
        {
            decimal result = 1m;

            if (!bubble.IsDecimal(pulseFreq.Text))
            {

                pulseFreq.Text = "0.5";

            }
            else
            {

                result = Convert.ToDecimal(pulseFreq.Text);

                if (result > 1m || result < 0.1m)
                {
                    result = 1m;
                }

                pulseFreq.Text = result.ToString();

            }

            config.getProfile(bubble.profileInUse).pulseFreq = result;


        }



        private void bttnUpdateFooter_Click(object sender, EventArgs e)
        {
            bttInstallUpdateAdmin_Click(null, null);
        }

        private void bttInstallUpdateAdmin_Click(object sender, EventArgs e)
        {
            bubble.updaterInstall = true;
            bubble.keepWorking = false;
            Close();
        }

        private void EmailIntelOn_CheckedChanged(object sender, EventArgs e)
        {

            emailIntelPanel.Enabled = EmailIntelOn.Checked;
            config.getProfile(bubble.profileInUse).EmailIntelOn = EmailIntelOn.Checked;

        }

        private void emailIntelEmails_Leave(object sender, EventArgs e)
        {

            emailIntelEmails.Text = bubble.verifyInt(emailIntelEmails.Text, 1, 9999, "1");
            config.getProfile(bubble.profileInUse).emailIntelEmails = Convert.ToInt32(emailIntelEmails.Text);

        }

        private void emailIntelMins_Leave(object sender, EventArgs e)
        {

            emailIntelMins.Text = bubble.verifyInt(emailIntelMins.Text, 1, 9999, "1");
            config.getProfile(bubble.profileInUse).emailIntelMins = Convert.ToInt32(emailIntelMins.Text);

        }

        private void EmailIntelStop_CheckedChanged(object sender, EventArgs e)
        {

            config.getProfile(bubble.profileInUse).EmailIntelStop = EmailIntelStop.Checked;

        }



        private void disCommOnlineSecs_Leave(object sender, EventArgs e)
        {

            disCommOnlineSecs.Text = bubble.verifyInt(disCommOnlineSecs.Text, 1, 86400, "1");
            config.getProfile(bubble.profileInUse).disCommOnlineSecs = Convert.ToInt32(disCommOnlineSecs.Text);

        }

        private void disCommOnline_CheckedChanged(object sender, EventArgs e)
        {

            disCommOnlineSecs.Enabled = disCommOnline.Checked;
            config.getProfile(bubble.profileInUse).disCommOnline = disCommOnline.Checked;

        }


        private void rdStatsToFileOn_CheckedChanged(object sender, EventArgs e)
        {

            pnlStatsToFile.Enabled = rdStatsToFileOn.Checked;
            config.getProfile(bubble.profileInUse).StatsToFileOn = rdStatsToFileOn.Checked;

            if (config.getProfile(bubble.profileInUse).StatsToFileLocation == string.Empty)
            {

                config.getProfile(bubble.profileInUse).StatsToFileLocation = Application.StartupPath + "\\" + "MovementStats.txt";

            }

        }

        private void btnStatsToFileLocation_Click(object sender, EventArgs e)
        {

            SaveFileDialog statsDialog = new SaveFileDialog();

            if (config.getProfile(bubble.profileInUse).StatsToFileLocation != string.Empty)
            {

                statsDialog.InitialDirectory = Path.GetDirectoryName(config.getProfile(bubble.profileInUse).StatsToFileLocation);
                statsDialog.FileName = Path.GetFileName(config.getProfile(bubble.profileInUse).StatsToFileLocation);

            }
            else
            {

                statsDialog.InitialDirectory = Application.StartupPath;
                statsDialog.FileName = "MovementStats";

            }


            statsDialog.Title = "Save statistics...";
            statsDialog.DefaultExt = "txt";
            statsDialog.AddExtension = true;
            statsDialog.Filter = "text files (*.txt)|*.txt|All files (*.*)|*.*";


            if (statsDialog.ShowDialog() == DialogResult.OK)
            {

                config.getProfile(bubble.profileInUse).StatsToFileLocation = statsDialog.FileName;
                statistics.fileName = string.Empty;

            }


        }

        private void chkStatsToFileTimeStamp_CheckedChanged(object sender, EventArgs e)
        {

            config.getProfile(bubble.profileInUse).StatsToFileTimeStamp = chkStatsToFileTimeStamp.Checked;

        }

        private void txtStatsToFileMb_Leave(object sender, EventArgs e)
        {

            txtStatsToFileMb.Text = bubble.verifyDouble(txtStatsToFileMb.Text, .01, double.MaxValue, "0.01");
            config.getProfile(bubble.profileInUse).StatsToFileMb = Convert.ToDouble(txtStatsToFileMb.Text);

        }

        private void txtLockdownPassword_Leave(object sender, EventArgs e)
        {

            config.getProfile(bubble.profileInUse).lockdownPassword = txtLockdownPassword.Text;

        }

        private void rdLockdownOff_CheckedChanged(object sender, EventArgs e)
        {

            config.getProfile(bubble.profileInUse).lockdownOn = !rdLockdownOff.Checked;
            bubble.lockdown = !rdLockdownOff.Checked;
            btnSecurityLockdownOn.Enabled = !rdLockdownOff.Checked;

        }

        private void btnSecurityLockdownOn_Click(object sender, EventArgs e)
        {


            WindowState = FormWindowState.Minimized;
            Hide();
            this.Enabled = false;

            while (1 == 1)
            {

                if (Prompt.ShowDialog("Password", "Enter password to unlock") == config.getProfile(bubble.profileInUse).lockdownPassword)
                {

                    this.Enabled = true;
                    break;

                }


            }


        }

        private void numFrameRateCalcOver_Leave(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).framesSecsToCalcOver = (int)numFrameRateCalcOver.Value;
        }

        private void chkFrameRateTrack_CheckedChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).framerateTrack = chkFrameRateTrack.Checked;
            CameraRig.ConnectedCameras.ForEach(x => x.cam.frameRateTrack = chkFrameRateTrack.Checked);
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
