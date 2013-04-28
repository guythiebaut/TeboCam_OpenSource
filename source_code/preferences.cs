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
//using dshow;
//using dshow.Core;

//using AForge.Video;
using AForge.Video.DirectShow;

enum enumCommandLine
{
    profile = 0,
    alert = 1,
    restart = 2,
    none = 9
};

namespace TeboCam
{

    public delegate void formDelegate(ArrayList i);
    public delegate void formDelegateList(List<List<object>> i);


    public partial class preferences : Form
    {


        public Pulse pulse;

        public configData configInfo = new configData();

        public static event EventHandler pulseEvent;
        public static event EventHandler pulseStopEvent;
        public static event EventHandler pulseStartEvent;

        public static event EventHandler publishSwitch;

        public static event ListPubEventHandler statusUpdate;

        public static System.Drawing.Bitmap myBitmap;
        public static System.Drawing.Bitmap levelBitmap;

        private ArrayList lineXpos = new ArrayList();

        public Point CurrentTopLeft = new Point();
        public Point CurrentBottomRight = new Point();
        public int RectangleHeight = new int();
        public int RectangleWidth = new int();


        static BackgroundWorker bw = new BackgroundWorker();
        static BackgroundWorker cw = new BackgroundWorker();
        static BackgroundWorker ew = new BackgroundWorker();
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

        private FilterInfoCollection filters;

        [STAThread]

        static void Main()
        {
            Application.Run(new preferences());
        }

        public preferences()
        {

            InitializeComponent();

        }

        private void testAtStart()
        {

            config.addProfile();
            configData data = config.getProfile("main");

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
            teboDebug.fileName = "debug.txt";

            teboDebug.debugToFile = true;
            teboDebug.openFile();

            teboDebug.writeline("workerProcess starting");

            while (bubble.keepWorking)
            {
                try
                {

                    pulseEvent(null, new EventArgs());
                    bubble.changeTheTime();

                    if (!CameraRig.reconfiguring)
                    {
                        cameraReconnectIfLost();//need to work on this for multicam
                    }

                    teboDebug.writeline("workerProcess calling scheduler()");
                    scheduler();
                    teboDebug.writeline("workerProcess calling ping");
                    bubble.ping();
                    teboDebug.writeline("workerProcess calling movementAddImages");
                    bubble.movementAddImages();
                    teboDebug.writeline("workerProcess calling publishImage");
                    bubble.publishImage();
                    teboDebug.writeline("workerProcess calling webUpdate");
                    bubble.webUpdate();
                    teboDebug.writeline("workerProcess calling movementPublish");
                    bubble.movementPublish();

                    teboDebug.writeline("workerProcess sleeping");
                    Thread.Sleep(250);

                }
                catch { }
            }

            e.Cancel = true;

        }

        private void filesInit()
        {

            if (!File.Exists(bubble.xmlFolder + FileManager.graphFile + ".xml"))
            {
                FileManager.WriteFile("graphInit");
                FileManager.backupFile("graph");
            }
            if (!File.Exists(bubble.xmlFolder + FileManager.logFile + ".xml"))
            {
                FileManager.WriteFile("logInit");
                FileManager.backupFile("log");
            }
            if (!File.Exists(bubble.xmlFolder + FileManager.configFile + ".xml"))
            {
                //bubble.configDataInit();
                FileManager.WriteFile("config");
                FileManager.backupFile("config");
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

            if (FileManager.readXmlFile("config", false))
            {
                FileManager.backupFile("config");
            }
            else
            {
                FileManager.readXmlFile("config", true);
            }
            if (FileManager.readXmlFile("log", false))
            {
                FileManager.backupFile("log");
            }
            else
            {
                FileManager.readXmlFile("log", true);
            }

            profileListRefresh(bubble.profileInUse);


            bubble.connectedToInternet = bubble.internetConnected(config.getProfile(bubble.profileInUse).internetCheck);
            notConnected.Visible = !bubble.connectedToInternet;

            //Apply command line values
            enumCommandLine commlineResults = commandLine();
            pnlStartupOptions.Visible = commlineResults <= enumCommandLine.alert;

            if (FileManager.readXmlFile("graph", false))
            {
                FileManager.backupFile("graph");
            }
            else
            {
                FileManager.readXmlFile("graph", true);
            }

            //clear out thumb nail images
            FileManager.clearFiles(bubble.thumbFolder);

            levelDraw(0);
            bubble.moveStatsInitialise();
            Graph.updateGraphHist(time.currentDate(), bubble.movStats);
            drawGraph(this, null);

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

            lblCurVer.Text = "This Version: " + bubble.version;

            List<string> updateDat = new List<string>();

            updateDat = check_for_updates();

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
                tmpStr = "You do not have the most recent version available" + Environment.NewLine + Environment.NewLine;
                tmpStr += "This version: " + bubble.version + Environment.NewLine;
                tmpStr += "Most recent version available: " + onlineVersion + Environment.NewLine + Environment.NewLine;
                tmpStr += "The most recent version can installed automatically" + Environment.NewLine;
                tmpStr += "by clicking on the update button at the bottom of the screen or on the Admin tab" + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                tmpStr += "To stop this message appearing in future" + Environment.NewLine;
                tmpStr += "uncheck the 'Notify when updates are available'" + Environment.NewLine;
                tmpStr += "box in the Admin tab.";
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

            if (config.getProfile(bubble.profileInUse).freezeGuard)
            {

                pulse.Beat(bttnMotionActive.Checked ? "restart active" : "restart inactive");

            }

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

            camButtons.initialize(9);



            bool nocam;


            List<string> camrigCams = new List<string>();
            camrigCams = CameraRig.camerasListedUnderProfile(bubble.profileInUse);

            nocam = false;

            try
            {
                filters = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (filters.Count == 0) throw new ApplicationException();
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

                    for (int c = 0; c < camrigCams.Count; c++)
                    {

                        if (filters[i].MonikerString == camrigCams[c])
                        {

                            Thread.Sleep(1000);
                            VideoCaptureDevice localSource = new VideoCaptureDevice(camrigCams[c]);
                            OpenVideoSource(localSource, null, false);
                            //remove camera from list of cameras to look for as we have now attached it
                            camrigCams.RemoveAt(c);

                        }

                    }

                }

            }

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
            openCamera();
        }



        private void openCamera()
        {

            string tmpStr = config.getProfile(bubble.profileInUse).webcam;

            CaptureDeviceForm form = new CaptureDeviceForm(tmpStr);

            if (form.ShowDialog(this) == DialogResult.OK)
            {

                if (form.Device.ipCam)
                {


                    //AForge.Video.MJPEGStream stream = new AForge.Video.MJPEGStream("http://192.111.1.1/mjpg/video.mjpg");
                    //stream.Source = "http://192.111.1.1/mjpg/video.mjpg";
                    VideoSource.MJPEGStream stream = new VideoSource.MJPEGStream();// ("http://192.111.1.1/mjpg/video.mjpg");
                    stream.VideoSource = "http://192.111.1.1/mjpg/video.mjpg";
                    stream.Login = "";
                    stream.Password = "";

                    //if (!string.IsNullOrEmpty(form.Device.name))
                    //{

                    //    stream.Login = "";
                    //    stream.Password = "";
                    //    //stream.Login = form.Device.name;
                    //    //stream.Password = form.Device.password;

                    //}

                    //Camera camera = new Camera(stream, detector);

                    OpenVideoSource(null, stream, true);

                }
                else
                {

                    if (!CameraRig.camerasAlreadySelected(form.Device.address))
                    {

                        // create video source
                        VideoCaptureDevice localSource = new VideoCaptureDevice(form.Device.address);

                        config.getProfile(bubble.profileInUse).webcam = form.Device.address;

                        // open it
                        OpenVideoSource(localSource, null, false);

                    }

                }


                //if (!CameraRig.camerasAlreadySelected(form.Device))
                //{

                //    // create video source
                //    VideoCaptureDevice localSource = new VideoCaptureDevice(form.Device);

                //    config.getProfile(bubble.profileInUse).webcam = form.Device;

                //    // open it
                //    OpenVideoSource(localSource, false);

                //}

            }
        }

        // Open video source


        //private void OpenVideoSource(VideoCaptureDevice source, AForge.Video.MJPEGStream ipStream, Boolean ip)//(VideoCaptureDevice source)
        private void OpenVideoSource(VideoCaptureDevice source, VideoSource.MJPEGStream ipStream, Boolean ip)//(VideoCaptureDevice source)
        {


            IMotionDetector detector = new MotionDetector3Optimized();

            // enable/disable motion alarm
            if (detector != null)
            {
                detector.MotionLevelCalculation = true;
            }

            //AForge.Video.DirectShow.VideoCaptureDevice camSource ;
            string camSource;

            // create camera
            Camera camera;

            //if (!ip)
            //{

            camSource = source.Source;
            camera = new Camera(source, detector);

            //}
            //else
            //{

            //    ////camSource = ipStream.Source;
            //    //camSource = ipStream.VideoSource;
            //    //camera = new Camera(ipStream, detector);

            //}

            camera.motionLevelEvent -= new motionLevelEventHandler(bubble.motionEvent);
            camera.motionLevelEvent += new motionLevelEventHandler(bubble.motionEvent);

            // start camera
            camera.Start();

            rigItem rig_it = new rigItem();
            rig_it.cameraName = camSource;//source.Source;
            rig_it.cam = camera;
            rig_it.cam.cam = CameraRig.cameraCount();
            CameraRig.addCamera(rig_it);
            int curCam = CameraRig.cameraCount() - 1;
            CameraRig.activeCam = curCam;

            config.getProfile(bubble.profileInUse).webcam = camSource;

            //populate or update rig info
            CameraRig.rigInfoPopulate(config.getProfile(bubble.profileInUse).profileName, curCam);

            CameraRig.rig[curCam].cam.cam = curCam;

            //get desired button or first available button
            int desiredButton = CameraRig.rig[curCam].displayButton;
            //check if the desired button is free and return the next free button if one is available
            int camButton = camButtons.availForClick(desiredButton, true);
            bool freeCamsExist = camButton != 999;

            //if a free camera button exists assign the camera
            if (freeCamsExist)
            {
                CameraRig.rig[curCam].displayButton = camButton;
            }

            //update info for camera
            CameraRig.updateInfo(bubble.profileInUse, config.getProfile(bubble.profileInUse).webcam, "displayButton", camButton);

            if (config.getProfile(bubble.profileInUse).selectedCam == "")
            {
                cameraSwitch(CameraRig.rig[curCam].displayButton, false, false);
            }
            else
            {
                if (config.getProfile(bubble.profileInUse).selectedCam == camSource)
                {
                    cameraSwitch(CameraRig.rig[curCam].displayButton, false, false);
                }
            }

            camButtonSetColours();

            if (CameraRig.rig[curCam].cam.alarmActive)
            {
                if (CameraRig.rig[curCam].displayButton == 1) selcam(this.bttncam1sel, 1);
                if (CameraRig.rig[curCam].displayButton == 2) selcam(this.bttncam2sel, 2);
                if (CameraRig.rig[curCam].displayButton == 3) selcam(this.bttncam3sel, 3);
                if (CameraRig.rig[curCam].displayButton == 4) selcam(this.bttncam4sel, 4);
                if (CameraRig.rig[curCam].displayButton == 5) selcam(this.bttncam5sel, 5);
                if (CameraRig.rig[curCam].displayButton == 6) selcam(this.bttncam6sel, 6);
                if (CameraRig.rig[curCam].displayButton == 7) selcam(this.bttncam7sel, 7);
                if (CameraRig.rig[curCam].displayButton == 8) selcam(this.bttncam8sel, 8);
                if (CameraRig.rig[curCam].displayButton == 9) selcam(this.bttncam9sel, 9);

            }

            if (CameraRig.rig[curCam].cam.publishActive)
            {
                if (CameraRig.rig[curCam].displayButton == 1) pubcam(this.bttncam1pub, 1);
                if (CameraRig.rig[curCam].displayButton == 2) pubcam(this.bttncam2pub, 2);
                if (CameraRig.rig[curCam].displayButton == 3) pubcam(this.bttncam3pub, 3);
                if (CameraRig.rig[curCam].displayButton == 4) pubcam(this.bttncam4pub, 4);
                if (CameraRig.rig[curCam].displayButton == 5) pubcam(this.bttncam5pub, 5);
                if (CameraRig.rig[curCam].displayButton == 6) pubcam(this.bttncam6pub, 6);
                if (CameraRig.rig[curCam].displayButton == 7) pubcam(this.bttncam7pub, 7);
                if (CameraRig.rig[curCam].displayButton == 8) pubcam(this.bttncam8pub, 8);
                if (CameraRig.rig[curCam].displayButton == 9) pubcam(this.bttncam9pub, 9);

            }



            CameraRig.rig[curCam].cam.MotionDetector.id = curCam;

            CameraRig.alert(bubble.Alert.on);
            CameraRig.rig[curCam].cam.MotionDetector.exposeArea = bubble.exposeArea;



            CameraRig.rig[curCam].cam.motionAlarm -= new alarmEventHandler(bubble.camera_Alarm);
            CameraRig.rig[curCam].cam.motionAlarm += new alarmEventHandler(bubble.camera_Alarm);

            bubble.webcamAttached = true;

            SetButtonEnabled(button23, CameraRig.camerasAttached());

        }

        // Close current file
        private void CloseFile()
        {
            bubble.webcamAttached = false;

            Camera camera = cameraWindow.Camera;

            if (camera != null)
            {
                // detach camera from camera window
                cameraWindow.Camera = null;

                // signal camera to stop
                camera.SignalToStop();
                // wait for the camera
                camera.WaitForStop();

                camera = null;

                if (camera.MotionDetector != null)
                    camera.MotionDetector.Reset();

            }

            if (writer != null)
            {
                writer.Dispose();
                writer = null;
            }
            intervalsToSave = 0;
        }


        // On alarm
        //private void camera_Alarm(object sender, System.EventArgs e)
        private void camera_Alarm(object sender, AlarmArgs e)
        {
            // save movie for 5 seconds after motion stops
            intervalsToSave = (int)(5 * (1000 / timer.Interval));
        }

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
                    String fileName = String.Format("{0}-{1}-{2} {3}-{4}-{5}.avi",
                        date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);

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

        # endregion


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


        delegate void SetRadioButtonCallback(RadioButton radio, bool btnChecked);
        public void SetRadioButton(RadioButton radio, bool btnChecked)
        {
            if (radio.InvokeRequired)
            {
                SetRadioButtonCallback d = new SetRadioButtonCallback(SetRadioButton);
                radio.Invoke(d, new object[] { radio, btnChecked });
            }
            else
            {
                radio.Checked = btnChecked;
                radio.Refresh();
                Invalidate();
            }
        }


        delegate void SetButtonEnableCallback(Button button, bool enabled);
        public void SetButtonEnabled(Button button, bool enabled)
        {
            if (button.InvokeRequired)
            {
                SetButtonEnableCallback d = new SetButtonEnableCallback(SetButtonEnabled);
                button.Invoke(d, new object[] { button, enabled });
            }
            else
            {
                button.Enabled = enabled;
                button.Refresh();
                Invalidate();
            }
        }

        delegate void SetTextBoxEnableCallback(TextBox textBox, bool enabled);
        public void SetTextBoxEnabled(TextBox textBox, bool enabled)
        {
            if (textBox.InvokeRequired)
            {
                SetTextBoxEnableCallback d = new SetTextBoxEnableCallback(SetTextBoxEnabled);
                textBox.Invoke(d, new object[] { textBox, enabled });
            }
            else
            {
                textBox.Enabled = enabled;
                textBox.Refresh();
                Invalidate();
            }
        }

        delegate void SetLabelCallback(Label label, string text);
        public void SetLabel(Label label, string text)
        {
            if (label.InvokeRequired)
            {
                SetLabelCallback d = new SetLabelCallback(SetLabel);
                label.Invoke(d, new object[] { label, text });
            }
            else
            {
                label.Text = text;
                label.Refresh();
                Invalidate();
            }
        }


        delegate void SetLabelVisibleCallback(Label label, bool visible);
        public void SetLabelVisible(Label label, bool visible)
        {
            if (label.InvokeRequired)
            {
                SetLabelVisibleCallback d = new SetLabelVisibleCallback(SetLabelVisible);
                label.Invoke(d, new object[] { label, visible });
            }
            else
            {
                label.Visible = visible;
                label.Refresh();
                Invalidate();
            }
        }



        delegate void SetTextCallback(TextBox txtBx, string text);
        public void SetInfo(TextBox txtBx, string text)
        {
            if (txtBx.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetInfo);
                txtBx.Invoke(d, new object[] { txtBx, text });
            }
            else
            {
                txtBx.Text = text;
                txtBx.Refresh();
                Invalidate();
            }
        }

        delegate void SetRichTextCallback(RichTextBox txtBx, string text);
        public void SetRichInfo(RichTextBox txtBx, string text)
        {
            if (txtBx.InvokeRequired)
            {
                SetRichTextCallback d = new SetRichTextCallback(SetRichInfo);
                txtBx.Invoke(d, new object[] { txtBx, text });
            }
            else
            {
                txtBx.Text = text + txtBx.Text;
                txtBx.Refresh();
                Invalidate();
            }
        }


        delegate void SetCheckBoxCallback(CheckBox chkBx, bool val);
        public void SetCheckBox(CheckBox chkBx, bool val)
        {
            if (chkBx.InvokeRequired)
            {
                SetCheckBoxCallback d = new SetCheckBoxCallback(SetCheckBox);
                chkBx.Invoke(d, new object[] { chkBx, val });
            }
            else
            {
                chkBx.Checked = val;
                chkBx.Refresh();
                Invalidate();
            }
        }








        private void activeCountdown(object sender, DoWorkEventArgs e)
        {

            SetCheckBox(bttnMotionSchedule, false);

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
                SetInfo(actCount, Convert.ToString(countdown));
            }

            tmpInt = countdown;
            lastCount = tmpInt;

            secondsToTrainStart = time.secondsSinceStart();

            if (tmpInt > 0)
            {
                bubble.logAddLine("Motion countdown started: " + tmpInt.ToString() + " seconds until start.");
                SetInfo(txtMess, "Counting Down...");
            }

            //This is the loop that checks on the countdown
            while (tmpInt > 0 && !bubble.countingdownstop)
            {
                tmpInt = countdown + ((int)secondsToTrainStart - time.secondsSinceStart());
                if (lastCount != tmpInt)
                {
                    SetInfo(actCount, Convert.ToString(tmpInt));
                    lastCount = tmpInt;
                }
                Thread.Sleep(500);//20100731 added to free up some processor time
            }
            //This is the loop that checks on the countdown

            SetInfo(actCount, Convert.ToString(""));
            bubble.countingdown = false;
            if (!bubble.countingdownstop)
            {
                bubble.Alert.on = true;
                bubble.logAddLine("Motion detection activated");
                            }

            SetInfo(txtMess, "");
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


                        foreach (int cam in camButtons.clickableButtons())
                        {

                            switch (cam)
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
                if (bttnMotionSchedule.Checked) SetCheckBox(bttnMotionSchedule, false);

                bubble.countingdownstop = true;
                bubble.Alert.on = false;
                bubble.logAddLine("Motion detection inactivated");
            }


        }




        // On add to log
        private void log_add(object sender, System.EventArgs e)
        {
            SetRichInfo(txtLog, bubble.log[bubble.log.Count - 1].ToString() + "\n");
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
                levelDraw(bubble.motionLevel);
            }
        }


        private void levelDraw(int val)
        {

            int sensePerc = 0;
            int lineStartX = 0;
            int lineStartY = 0;
            int lineLen = 0;
            int lineWid = 0;
            double onePct = 0;
            int greenLen = 0;
            int orangeLen = 0;
            int greenStart = 0;
            int orangeStart = 0;


            if (CameraRig.camerasAttached())
            {
                sensePerc = (int)Math.Floor(CameraRig.rig[CameraRig.activeCam].cam.movementVal * (double)100);
            }
            else
            {
                sensePerc = 100;
            }

            lineLen = levelbox.Size.Height;
            lineWid = levelbox.Size.Width;
            onePct = (double)lineLen / (double)100;
            greenLen = (int)Math.Floor((double)val * onePct);
            orangeLen = (int)Math.Floor(((double)100 - (double)val) * onePct);
            greenStart = (int)Math.Floor(((double)100 - (double)val) * onePct);
            orangeStart = (100 - val);

            System.Drawing.SolidBrush controlBrush = new System.Drawing.SolidBrush(System.Drawing.SystemColors.Control);
            System.Drawing.SolidBrush greenBrush = new System.Drawing.SolidBrush(System.Drawing.Color.GreenYellow);
            System.Drawing.SolidBrush orangeBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);

            levelBitmap = new Bitmap(lineWid, lineLen, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Graphics levelObj = Graphics.FromImage(levelBitmap);

            levelObj.FillRectangle(controlBrush, new Rectangle(lineStartX, lineStartY, lineWid, lineLen));

            if (val > sensePerc)
            {
                greenStart = (int)Math.Floor(((double)100 - (double)sensePerc) * onePct);
                greenLen = (int)Math.Floor((double)sensePerc * onePct);
                orangeStart = ((int)Math.Floor(((double)100 - (double)val) * onePct));
                orangeLen = (int)Math.Floor(((double)val - (double)sensePerc) * onePct);
                levelObj.FillRectangle(greenBrush, new Rectangle(lineStartX, greenStart, lineWid, greenLen));
                levelObj.FillRectangle(orangeBrush, new Rectangle(lineStartX, orangeStart, lineWid, orangeLen));
            }
            else
            {
                greenStart = (int)Math.Floor(((double)100 - (double)val) * onePct);
                greenLen = (int)Math.Floor((double)val * onePct);
                levelObj.FillRectangle(greenBrush, new Rectangle(lineStartX, greenStart, lineWid, greenLen));
            }

            controlBrush.Dispose();
            greenBrush.Dispose();
            orangeBrush.Dispose();
            levelObj.Dispose();
            levelbox.Invalidate();

        }






        private void drawGraph(object sender, EventArgs e)
        {
            graphDate(DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));
        }

        private void drawGraphPing(object sender, EventArgs e)
        {
            graphDate(bubble.pingGraphDate);
        }


        private void graphDate(string graphDate)
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
                        ArrayList graphData = Graph.getGraphHist(graphDate);

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

                            string thisVal = Graph.getGraphVal(graphDate, cellIdx);

                            graphicsObj.DrawString(thisVal, new Font("Tahoma", 8), Brushes.LemonChiffon, new PointF(curPos + 7, lineLength - (lineHeight)));
                        }

                    }

                    //increment i for next two hour slot
                    i += 1;
                    curPos += timeSep;


                }

                if (!movement)
                {
                    graphicsObj.DrawString("No Movement Detected", new Font("Tahoma", 20), Brushes.LemonChiffon, new PointF(80, windowHeight - 140));
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

        private void levelbox_Paint(object sender, PaintEventArgs e)
        {

            Graphics levelObj = e.Graphics;
            try
            {
                if (levelBitmap != null)
                {
                    levelObj.DrawImage(levelBitmap, 0, 0, levelBitmap.Width, levelBitmap.Height);
                }
            }
            catch (Exception)
            {
                bubble.logAddLine("Error drawing level bitmap");
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

                ArrayList tmpList = Graph.getGraphDates();

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

                FileManager.WriteFile("config");
                bubble.logAddLine("Config data saved.");
                FileManager.WriteFile("graph");
                bubble.logAddLine("Graph data saved.");
                bubble.logAddLine("Saving log data and closing.");
                FileManager.WriteFile("log");

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
                pulse.stopCheck(bubble.pulseProcessName);
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

            foreach (rigItem rigI in CameraRig.rig)
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
            if (Graph.dataExistsForDate(dateSelected))
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
            SetLabel(lblTime, bubble.lastTime);
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
                ftp.DeleteFTP(FileManager.testFile + ".xml", config.getProfile(bubble.profileInUse).ftpRoot, config.getProfile(bubble.profileInUse).ftpUser, config.getProfile(bubble.profileInUse).ftpPass);
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



        private void openWebPage(object sender, DoWorkEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(bubble.tebowebUrl);
            }
            catch (Exception)
            {
                bubble.logAddLine("Unable to connect to download site.");
            }
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


            //20120318 noopped 
            //Move files out of images folder to sub folders
            //DirectoryInfo diImg = new DirectoryInfo(bubble.imageParentFolder);
            //FileInfo[] imageFilesImg = diImg.GetFiles("*.*");
            //foreach (FileInfo fi in imageFilesImg)
            //{

            //    if (LeftRightMid.Left(fi.Name, bubble.tmbPrefix.Length) == bubble.tmbPrefix)
            //    {
            //        if (File.Exists(bubble.thumbFolder + fi.Name))
            //        {
            //            File.Delete(bubble.thumbFolder + fi.Name);
            //        }
            //        File.Move(fi.FullName, bubble.thumbFolder + fi.Name);
            //    }
            //    else
            //    {
            //        if (File.Exists(bubble.imageFolder + fi.Name))
            //        {
            //            File.Delete(bubble.imageFolder + fi.Name);
            //        }
            //        File.Move(fi.FullName, bubble.imageFolder + fi.Name);
            //    }
            //}
            //20120318 noopped 

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

        private void getProfile(string profileName)
        {
            configData data = config.getProfile(profileName);

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

            bool tmpBool = data.sendNotifyEmail;

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


            sendEmail.Checked = tmpBool;


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
                levelDraw(0);
            }

            lbl0Perc.Visible = showLevel;
            lbl25Perc.Visible = showLevel;
            lbl50Perc.Visible = showLevel;
            lbl75Perc.Visible = showLevel;
            lbl100Perc.Visible = showLevel;

            webUpd.Checked = data.webUpd;
            sqlUser.Text = data.webUser;
            sqlPwd.Text = data.webPass;
            sqlPoll.Text = data.webPoll.ToString();
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

            disCommOnline.Checked = data.disCommOnline;
            disCommOnlineSecs.Text = data.disCommOnlineSecs.ToString();
            disCommOnlineSecs.Enabled = disCommOnline.Checked;

            plSnd.Checked = data.soundAlertOn;
            logsKeep.Text = data.logsKeep.ToString();
            logsKeepChk.Checked = data.logsKeepChk;
            freezeGuardOn.Checked = data.freezeGuard;
            freezeGuardWindow.Checked = data.freezeGuardWindowShow;
            pulseFreq.Text = data.pulseFreq.ToString();

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
            ArrayList tmpList = config.getProfileList();

            foreach (string profile in tmpList)
            {
                profileList.Items.Add(profile);
            }

            profileList.SelectedIndex = 0;
        }

        private void profileChanged(object sender, EventArgs e)
        {
            saveChanges();
            bubble.profileInUse = profileList.SelectedItem.ToString();
            getProfile(bubble.profileInUse);
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
            ArrayList tmpList = config.getProfileList();
            int tmpInt = 0;

            foreach (string profile in tmpList)
            {
                profileList.Items.Add(profile);
                if (profile == selectProfile) { tmpInt = profileList.Items.Count - 1; }
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

            if (CameraRig.rig.Count > 0)
            {
                int pubButton = CameraRig.idxFromButton(camButtons.publishingButton());
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "pubTime", Convert.ToInt32(pubTime.Text));
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "publishFirst", true);
            }


        }

        private void pubHours_CheckedChanged(object sender, EventArgs e)
        {

            if (CameraRig.rig.Count > 0)
            {
                int pubButton = CameraRig.idxFromButton(camButtons.publishingButton());
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "pubHours", pubHours.Checked);
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "publishFirst", true);
            }

        }

        private void pubMins_CheckedChanged(object sender, EventArgs e)
        {

            if (CameraRig.rig.Count > 0)
            {
                int pubButton = CameraRig.idxFromButton(camButtons.publishingButton());
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "pubMins", pubMins.Checked);
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "publishFirst", true);
            }

        }

        private void pubSecs_CheckedChanged(object sender, EventArgs e)
        {

            if (CameraRig.rig.Count > 0)
            {
                int pubButton = CameraRig.idxFromButton(camButtons.publishingButton());
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "pubSecs", pubSecs.Checked);
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "publishFirst", true);
            }

        }

        private void pubLocal_CheckedChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).publishLocal = pubToLocal.Checked;
            if (CameraRig.rig.Count > 0)
            {
                int pubButton = CameraRig.idxFromButton(camButtons.publishingButton());
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "publishLocal", pubToLocal.Checked);
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "publishFirst", true);
            }
        }

        private void pubWeb_CheckedChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).publishWeb = pubToWeb.Checked;

            if (CameraRig.rig.Count > 0)
            {
                int pubButton = CameraRig.idxFromButton(camButtons.publishingButton());
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "publishWeb", pubToWeb.Checked);
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "publishFirst", true);
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
            ew.DoWork -= new DoWorkEventHandler(openWebPage);
            ew.DoWork += new DoWorkEventHandler(openWebPage);
            ew.WorkerSupportsCancellation = true;
            ew.RunWorkerAsync();
        }

        private void pubTimerOn_CheckedChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).timerOn = pubTimerOn.Checked;

            if (CameraRig.rig.Count > 0)
            {
                int pubButton = CameraRig.idxFromButton(camButtons.publishingButton());
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "timerOn", pubTimerOn.Checked);
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



        private void cameraReconnectIfLost()
        {
            bool oldCamPresent = false;


            //camera has been lost
            if (!bubble.drawMode && frameCount > 0 && frameCount == framePrevious)
            {

                List<string> camrigCams = new List<string>();
                camrigCams = CameraRig.camerasListedUnderProfile(bubble.profileInUse);


                //attempt to reconnect to camera
                try
                {
                    filters = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                    if (filters.Count == 0)
                        throw new ApplicationException();

                    // add all devices to combo
                    foreach (FilterInfo filter in filters)
                    {
                        if (filter.MonikerString == config.getProfile(bubble.profileInUse).webcam)
                        { oldCamPresent = true; }
                    }
                }
                catch
                { }

                if (oldCamPresent)
                {
                    VideoCaptureDevice localSource = new VideoCaptureDevice(config.getProfile(bubble.profileInUse).webcam);
                    OpenVideoSource(localSource, null, false);
                    Thread.Sleep(2000);
                }
            }
            framePrevious = frameCount;
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
            SetCheckBox(pubImage, !bubble.keepPublishing);
            SetCheckBox(pubTimerOn, true);
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
            //rdOnlinets.Enabled = bubble.DatabaseCredentialsCorrect;
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
            SetRadioButton(bttnMotionInactive, true);
            SetRadioButton(bttnMotionActive, false);

            Thread.Sleep(4000);

            //now activate motion detection
            SetRadioButton(bttnNow, true);
            SetRadioButton(bttnTime, false);
            SetRadioButton(bttnSeconds, false);

            SetRadioButton(bttnMotionActive, true);
            SetRadioButton(bttnMotionInactive, false);

            //state.motionDetectionActive = true;

        }

        private void motionDetectionInactivate(object sender, System.EventArgs e)
        {

            //20130427 restored as the scheduleOnAtStart property now takes care of reactivating at start up
            if (bttnMotionSchedule.Checked) SetCheckBox(bttnMotionSchedule, false);

            SetRadioButton(bttnMotionInactive, true);
            SetRadioButton(bttnMotionActive, false);

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


        private void webUpd_CheckedChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).webUpd = webUpd.Checked;
            bubble.webUpdLastChecked = 0;
            bubble.webFirstTimeThru = true;
        }



        private void plSnd_CheckedChanged(object sender, EventArgs e)
        {
            config.getProfile(bubble.profileInUse).soundAlertOn = plSnd.Checked;
            FileManager.WriteFile("config");
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
                FileManager.WriteFile("config");
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
            FileManager.WriteFile("config");
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
                SetLabel(alertVal, i[1].ToString());
            }

            if (i[0].ToString() == "Publish")
            {
                config.getProfile(bubble.profileInUse).publishCompression = Convert.ToInt32(i[1].ToString());
                SetLabel(publishVal, i[1].ToString());
            }

            if (i[0].ToString() == "Ping")
            {
                config.getProfile(bubble.profileInUse).pingCompression = Convert.ToInt32(i[1].ToString());
                SetLabel(pingVal, i[1].ToString());
            }

            if (i[0].ToString() == "Online")
            {
                config.getProfile(bubble.profileInUse).onlineCompression = Convert.ToInt32(i[1].ToString());
                SetLabel(onlineVal, i[1].ToString());
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

            //List<List<object>> jpegList = new List<List<object>>();

            //List<object> alertList = new List<object>();
            //List<object> pingList = new List<object>();
            //List<object> publishList = new List<object>();
            //List<object> onlineList = new List<object>();

            //alertList.Add("Alert");
            //alertList.Add(config.getProfile(bubble.profileInUse).alertCompression);
            //pingList.Add("Ping");
            //pingList.Add(config.getProfile(bubble.profileInUse).pingCompression);
            //publishList.Add("Publish");
            //publishList.Add(config.getProfile(bubble.profileInUse).publishCompression);
            //onlineList.Add("Online");
            //onlineList.Add(config.getProfile(bubble.profileInUse).onlineCompression);

            //jpegList.Add(alertList);
            //jpegList.Add(pingList);
            //jpegList.Add(publishList);
            //jpegList.Add(onlineList);

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



            //ArrayList i = new ArrayList();


            //if (rdAlertts.Checked)
            //{
            //    i.Add("Alert");
            //    i.Add(config.getProfile(bubble.profileInUse).alertTimeStamp);
            //    i.Add(config.getProfile(bubble.profileInUse).alertTimeStampFormat);
            //    i.Add(config.getProfile(bubble.profileInUse).alertTimeStampColour);
            //    i.Add(config.getProfile(bubble.profileInUse).alertTimeStampPosition);
            //    i.Add(config.getProfile(bubble.profileInUse).alertTimeStampRect);
            //    i.Add(false);
            //    i.Add(config.getProfile(bubble.profileInUse).alertStatsStamp);
            //}
            //if (rdPingts.Checked)
            //{
            //    i.Add("Ping");
            //    i.Add(config.getProfile(bubble.profileInUse).pingTimeStamp);
            //    i.Add(config.getProfile(bubble.profileInUse).pingTimeStampFormat);
            //    i.Add(config.getProfile(bubble.profileInUse).pingTimeStampColour);
            //    i.Add(config.getProfile(bubble.profileInUse).pingTimeStampPosition);
            //    i.Add(config.getProfile(bubble.profileInUse).pingTimeStampRect);
            //    i.Add(true);
            //    i.Add(config.getProfile(bubble.profileInUse).pingStatsStamp);
            //}
            //if (rdPublishts.Checked)
            //{
            //    i.Add("Publish");
            //    i.Add(config.getProfile(bubble.profileInUse).publishTimeStamp);
            //    i.Add(config.getProfile(bubble.profileInUse).publishTimeStampFormat);
            //    i.Add(config.getProfile(bubble.profileInUse).publishTimeStampColour);
            //    i.Add(config.getProfile(bubble.profileInUse).publishTimeStampPosition);
            //    i.Add(config.getProfile(bubble.profileInUse).publishTimeStampRect);
            //    i.Add(true);
            //    i.Add(config.getProfile(bubble.profileInUse).publishStatsStamp);
            //}
            //if (rdOnlinets.Checked)
            //{
            //    i.Add("Online");
            //    i.Add(config.getProfile(bubble.profileInUse).onlineTimeStamp);
            //    i.Add(config.getProfile(bubble.profileInUse).onlineTimeStampFormat);
            //    i.Add(config.getProfile(bubble.profileInUse).onlineTimeStampColour);
            //    i.Add(config.getProfile(bubble.profileInUse).onlineTimeStampPosition);
            //    i.Add(config.getProfile(bubble.profileInUse).onlineTimeStampRect);
            //    i.Add(false);
            //    i.Add(config.getProfile(bubble.profileInUse).onlineStatsStamp);
            //}

            //i.Add(config.getProfile(bubble.profileInUse).toolTips);
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
                levelDraw(0);

            }

            config.getProfile(bubble.profileInUse).motionLevel = showLevel;

            lbl0Perc.Visible = showLevel;
            lbl25Perc.Visible = showLevel;
            lbl50Perc.Visible = showLevel;
            lbl75Perc.Visible = showLevel;
            lbl100Perc.Visible = showLevel;

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



        private void cameraSwitch(int button, bool refresh, bool load)
        {

            int camId = CameraRig.idxFromButton(button);


            if (load || !load && camButtons.camClick(button))
            {
                if (load || !load && CameraRig.cameraExists(camId))
                {

                    CameraRig.activeCam = camId;
                    CameraRig.rig[camId].cam.MotionDetector.Reset();
                    cameraWindow.Camera = CameraRig.rig[camId].cam;
                    SetLabelVisible(lblCameraName, CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[camId].cameraName, "friendlyName").ToString().Trim() != "");
                    SetLabel(lblCameraName, CameraRig.rig[camId].friendlyName);
                    config.getProfile(bubble.profileInUse).selectedCam = CameraRig.rig[camId].cameraName;

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
            int result = camButtons.motionSenseClick(button);

            if (result == 0)
            {
                licence.deselectCam(cam + 1);
                btn.BackColor = Color.Silver;
                CameraRig.rig[cam].cam.alarmActive = false;
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[cam].cameraName, "alarmActive", false);
            }
            if (result == 1)
            {
                licence.selectCam(cam + 1);
                btn.BackColor = Color.LawnGreen;
                CameraRig.rig[cam].cam.alarmActive = true;
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[cam].cameraName, "alarmActive", true);
            }


        }


        private void publishRefresh(int button)
        {

            int pubButton = CameraRig.idxFromButton(button);

            pubTime.Text = CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "pubTime").ToString();
            pubHours.Checked = (bool)CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "pubHours");
            pubMins.Checked = (bool)CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "pubMins");
            pubSecs.Checked = (bool)CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "pubSecs");
            pubToWeb.Checked = (bool)CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "publishWeb");
            pubToLocal.Checked = (bool)CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "publishLocal");
            pubTimerOn.Checked = (bool)CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "timerOn");

        }

        private void pubcam(Button btn, int button)
        {

            if (camButtons.clickableButtons().Contains(button))
            {

                int cam = CameraRig.idxFromButton(button);

                //unpublish other cameras
                camButtons.publishClearExcept(button);
                foreach (rigItem item in CameraRig.rig)
                {

                    if (item.displayButton != button)
                    {

                        item.cam.publishActive = false;
                        CameraRig.updateInfo(bubble.profileInUse, item.cameraName, "publishActive", false);

                    }

                }


                int result = camButtons.publishClick(button);

                if (result == 0)
                {
                    btn.BackColor = Color.Silver;
                    CameraRig.rig[cam].cam.publishActive = false;
                    CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[cam].cameraName, "publishActive", false);
                }
                if (result == 1)
                {
                    btn.BackColor = Color.LawnGreen;
                    CameraRig.rig[cam].cam.publishActive = true;
                    CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[cam].cameraName, "publishActive", true);
                }


                camButtonSetColours();

                publishRefresh(button);

            }

        }



        private void camButtonSetColours()
        {

            List<Control> tmpControls = controls(this);
            List<Control> availControls = new List<Control>();
            for (int i = 0; i < tmpControls.Count; i++)
            {

                string tmpStr = tmpControls[i].Name.ToString();

                if (tmpControls[i] is Button
                    && tmpStr.Length > 7
                    && LeftRightMid.Left(tmpStr.ToLower(), 7) == "bttncam"
                    )
                {
                    availControls.Add(tmpControls[i]);
                }

            }

            foreach (Control ctrl in availControls)
            {

                if (LeftRightMid.Right(ctrl.Name.ToString(), 3) != "sel" && LeftRightMid.Right(ctrl.Name.ToString(), 3) != "pub")
                {

                    if (camButtons.buttonState(Convert.ToInt32(ctrl.Text)) == 0)
                    {
                        ctrl.BackColor = Color.Silver;
                    }
                    if (camButtons.buttonState(Convert.ToInt32(ctrl.Text)) == 1)
                    {
                        ctrl.BackColor = Color.LawnGreen;
                    }
                    if (camButtons.buttonState(Convert.ToInt32(ctrl.Text)) == 2)
                    {
                        ctrl.BackColor = Color.SkyBlue;
                    }

                }

                if (LeftRightMid.Right(ctrl.Name.ToString(), 3) == "sel")
                {

                    int selNum = Convert.ToInt32(LeftRightMid.Mid(ctrl.Name.ToString(), 7, 1));

                    bool buttonCam = camButtons.clickableButtons().Contains(selNum);

                    if (buttonCam)
                    {

                        if (CameraRig.rig[CameraRig.idxFromButton(selNum)].cam.alarmActive)
                        {
                            ctrl.BackColor = Color.LawnGreen;
                        }
                        else
                        {
                            ctrl.BackColor = Color.Silver;
                        }

                    }
                    else
                    {

                        ctrl.BackColor = Color.Silver;

                    }

                }


                if (LeftRightMid.Right(ctrl.Name.ToString(), 3) == "pub")
                {

                    int pubNum = Convert.ToInt32(LeftRightMid.Mid(ctrl.Name.ToString(), 7, 1));

                    bool buttonCam = camButtons.clickableButtons().Contains(pubNum);

                    if (buttonCam)
                    {

                        if (CameraRig.rig[CameraRig.idxFromButton(pubNum)].cam.publishActive)
                        {
                            ctrl.BackColor = Color.LawnGreen;
                        }
                        else
                        {
                            ctrl.BackColor = Color.Silver;
                        }

                    }
                    else
                    {

                        ctrl.BackColor = Color.Silver;

                    }

                }


            }

            tmpControls.Clear();
            availControls.Clear();

        }


        private void camReset()
        {
            List<Control> availControls = controls(this);

            for (int i = 0; i < availControls.Count; i++)
            {
                if (availControls[i] is Button) { }
                else { availControls.RemoveAt(i); }
            }

            foreach (Control ctrl in availControls)
            {
                if (ctrl.Name.ToString().Length > 7 && ctrl.Name.ToString().Length < 11 && LeftRightMid.Left(ctrl.Name.ToString(), 7) == "bttncam")
                {
                    {
                        ctrl.BackColor = Color.Silver;
                    }

                }
            }

            availControls.Clear();

        }










        private void bttnCamProp_Click(object sender, EventArgs e)
        {
            VideoCaptureDevice localSource = new VideoCaptureDevice(CameraRig.rig[CameraRig.activeCam].cameraName);
            localSource.DisplayPropertyPage(IntPtr.Zero); // non-modal
        }

        private void button19_Click_1(object sender, EventArgs e)
        {

            if (!Directory.Exists(bubble.vaultFolder)) Directory.CreateDirectory(bubble.vaultFolder);

            string configVlt = bubble.vaultFolder + "config_" + DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture) + ".xml";
            string configXml = bubble.xmlFolder + "config.xml";

            File.Copy(configXml, configVlt, true);
            MessageBox.Show("config.xml has been successfully vaulted in the vault folder.", "File Vaulted");




        }

        private void button23_Click_1(object sender, EventArgs e)
        {
            ArrayList i = new ArrayList();
            i.Add(config.getProfile(bubble.profileInUse).toolTips);
            i.Add(CameraRig.activeCam);
            i.Add(panel1.AutoScroll);
            i.Add(camButtons.buttons());

            cameraWindow.Camera = null;
            panel1.AutoScroll = false;
            cameraWindow.Invalidate();

            bubble.motionLevelChanged -= new EventHandler(drawLevel);
            levelDraw(0);

            webcamConfig webcamConfig = new webcamConfig(new formDelegate(webcamConfigCompleted), i);
            webcamConfig.StartPosition = FormStartPosition.CenterScreen;


            webcamConfig.ShowDialog();
        }


        private void webcamConfigCompleted(ArrayList i)
        {

            System.Diagnostics.Debug.WriteLine(CameraRig.cameraCount());

            if (CameraRig.cameraCount() > 0)
            {

                //give the interface some time to refresh
                Thread.Sleep(250);
                //give the interface some time to refresh
                cameraSwitch(CameraRig.rig[CameraRig.drawCam].displayButton, true, true);

            }

            panel1.AutoScroll = (bool)i[1];
            i.Clear();
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
            //camButtonSetColours();
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

            if (CameraRig.rig.Count > 0)
            {

                ArrayList i = new ArrayList();

                int pubButton = CameraRig.idxFromButton(camButtons.publishingButton());

                i.Add("Publish Web");
                i.Add(config.getProfile(bubble.profileInUse).toolTips);
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "filenamePrefixPubWeb"));
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "cycleStampCheckedPubWeb"));
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "startCyclePubWeb"));
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "endCyclePubWeb"));
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "currentCyclePubWeb"));
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "stampAppendPubWeb"));
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

            int pubButton = CameraRig.idxFromButton(camButtons.publishingButton());

            CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "publishFirst", true);

            if (i[0].ToString() == "Publish Web")
            {

                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "filenamePrefixPubWeb", i[1].ToString());
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "cycleStampCheckedPubWeb", Convert.ToInt32(i[2]));
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "startCyclePubWeb", Convert.ToInt32(i[3]));
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "endCyclePubWeb", Convert.ToInt32(i[4]));
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "currentCyclePubWeb", Convert.ToInt32(i[5]));
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "stampAppendPubWeb", Convert.ToBoolean(i[6]));

            }

            if (i[0].ToString() == "Publish Local")
            {

                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "filenamePrefixPubLoc", i[1].ToString());
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "cycleStampCheckedPubLoc", Convert.ToInt32(i[2]));
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "startCyclePubLoc", Convert.ToInt32(i[3]));
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "endCyclePubLoc", Convert.ToInt32(i[4]));
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "currentCyclePubLoc", Convert.ToInt32(i[5]));
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "stampAppendPubLoc", Convert.ToBoolean(i[6]));
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "fileDirPubLoc", i[7].ToString());
                CameraRig.updateInfo(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "fileDirPubCust", Convert.ToBoolean(i[8]));


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

            if (CameraRig.rig.Count > 0)
            {

                ArrayList i = new ArrayList();

                int pubButton = CameraRig.idxFromButton(camButtons.publishingButton());

                i.Add("Publish Local");
                i.Add(config.getProfile(bubble.profileInUse).toolTips);
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "filenamePrefixPubLoc"));
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "cycleStampCheckedPubLoc"));
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "startCyclePubLoc"));
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "endCyclePubLoc"));
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "currentCyclePubLoc"));
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "stampAppendPubLoc"));
                i.Add(true);
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "fileDirPubLoc"));
                i.Add(bubble.imageFolder);
                i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[pubButton].cameraName, "fileDirPubCust"));
                i.Add(true);



                fileprefix fileprefix = new fileprefix(new formDelegate(filePrefixSet), i);
                fileprefix.StartPosition = FormStartPosition.CenterScreen;
                fileprefix.ShowDialog();

            }

        }


        private void button35_ClickNEW(object sender, EventArgs e)
        {

            int alertButton = CameraRig.idxFromButton(camButtons.firstActiveButton());

            ArrayList i = new ArrayList();

            i.Add("Alert");
            i.Add(config.getProfile(bubble.profileInUse).toolTips);
            i.Add(config.getProfile(bubble.profileInUse).filenamePrefix);
            i.Add(config.getProfile(bubble.profileInUse).cycleStampChecked);
            i.Add(config.getProfile(bubble.profileInUse).startCycle);
            i.Add(config.getProfile(bubble.profileInUse).endCycle);
            i.Add(config.getProfile(bubble.profileInUse).currentCycle);
            i.Add(true);
            i.Add(true);
            i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[alertButton].cameraName, "fileDirAlertLoc"));
            i.Add(bubble.imageFolder);
            i.Add(CameraRig.rigInfoGet(bubble.profileInUse, CameraRig.rig[alertButton].cameraName, "fileDirAlertCust"));
            i.Add(true);

            fileprefix fileprefix = new fileprefix(new formDelegate(filePrefixSet), i);
            fileprefix.StartPosition = FormStartPosition.CenterScreen;
            fileprefix.ShowDialog();

        }

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
            config.getProfile(bubble.profileInUse).EmailIntelOn = EmailIntelOn.Checked; ;

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
