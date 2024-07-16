//using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;


namespace TeboCam
{

    public delegate void configSettingsDelegate(List<int> list);


    public partial class webcamConfig : Form
    {
        private int detectionCountDown;
        public delegate void SaveChanges();
        private SaveChanges saveChanges;
        public CameraButtonsCntl ButtonCameraControl = new CameraButtonsCntl();
        List<GroupCameraButton> CameraButtons = new List<GroupCameraButton>();
        private formDelegate webcamConfigDelegate;
        private int currentlySelectedButton;

        public static BackgroundWorker dw = new BackgroundWorker();

        public static event drawEventHandler drawInitialRectangle;

        public static bool movementSetting;

        public static bool exposeArea = false;
        private static string selectedWebcam;
        private static int mainSelectedWebcam;
        //private static List<camButtons.ButtonColourEnum> buttons;
        private LevelControl LevelControlBox = new LevelControl();
        private ArrayList returnList = new ArrayList();
        private bool autoscroll;
        private static bool toolTip;
        Publisher publisher;
        Ping ping;
        bool baselineSetting;
        int detectionTrain;

        public webcamConfig(formDelegate sender, ArrayList from, List<GroupCameraButton> CameraButtonGroupInstance, SaveChanges save, Publisher pub, Ping pinger)
        {
            saveChanges = save;
            toolTip = (bool)from[0];
            mainSelectedWebcam = (int)from[1];
            autoscroll = (bool)from[2];
            // buttons = (List<camButtons.ButtonColourEnum>)from[3];
            webcamConfigDelegate = sender;
            publisher = pub;
            ping = pinger;
            from.Clear();
            InitializeComponent();
            cameraWindow.ping = ping;

            //this.Controls.Add(ButtonCameraControl);
            //ButtonCameraControl.Location = new Point(5, 480);
            //ButtonCameraControl.AddExistingButtons(CameraButtonGroupInstance);
            ConfigureCameraButtons(CameraButtonGroupInstance);
            //CameraButtons = CameraButtonGroupInstance;

            LevelControlBox.Left = 417;
            LevelControlBox.Top = 18;
            this.Controls.Add(LevelControlBox);

            camButtonSetColours();

            cameraSwitch(CameraRig.ConnectedCameras[CameraRig.CurrentlyDisplayingCamera].displayButton, true);

        }

        private void ConfigureCameraButtons(List<GroupCameraButton> CameraButtonGroupInstance)
        {
            foreach (GroupCameraButton group in CameraButtonGroupInstance)
            {
                ButtonCameraControl.AddButton(CameraButtons, ButtonCameraDelegation, null, false, new Size(18, 20), new Size(0, 0));
            }
            panel2.Controls.Add(ButtonCameraControl);
            ButtonCameraControl.Location = new Point(10, 2);
        }

        private void ButtonCameraDelegation(int id, Button cameraButton, Button activeButton, bool activate = false)
        {
            cameraSwitch(id, false);
        }

        private void webcamConfig_Load(object sender, EventArgs e)
        {
            toolTip1.Active = toolTip;
        }

        private void camName_TextChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam).friendlyName = camName.Text;
            CameraRig.ConnectedCameraPopulateForCam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam);
        }


        private void bttnCamProp_Click(object sender, EventArgs e)
        {

            //hjfgjgf

            if (CameraRig.ConnectedCameras[CameraRig.ConfigCam].camera.isIPCamera)
            {

                try
                {

                    IPAddress parsedIPAddress;
                    Uri parsedUri;
                    string name = CameraRig.ConnectedCameras[CameraRig.ConfigCam].camera.name;

                    //check that the url resolves
                    if (Uri.TryCreate(name, UriKind.Absolute, out parsedUri) && IPAddress.TryParse(parsedUri.DnsSafeHost, out parsedIPAddress))
                    {
                        Internet.openInternetBrowserAt("http:\\" + parsedIPAddress.ToString());
                    }

                }
                catch (Exception ex)
                {
                    TebocamState.tebowebException.LogException(ex);
                    TebocamState.log.AddLine("Unable to connect webcam index site.");
                }
            }
            else
            {
                VideoCaptureDevice localSource = new VideoCaptureDevice(selectedWebcam);
                localSource.DisplayPropertyPage(IntPtr.Zero); // non-modal
            }



        }


        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

            if (drawModeOff.Checked)
            {
                ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam).areaDetection = radioButton4.Checked;
                CameraRig.ConnectedCameraPopulateForCam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam);

                CameraRig.getCam(selectedWebcam).MotionDetector.Reset();
                ConfigurationHelper.GetCurrentProfile().areaDetection = radioButton4.Checked;
                CameraRig.getCam(selectedWebcam).areaDetection = radioButton4.Checked;

                areaOffAtMotion.Enabled = radioButton4.Checked;
                groupBox4.Enabled = radioButton4.Checked;
                showSelection.Enabled = radioButton4.Checked;

                CameraRig.ConnectedCameras[CameraRig.ConfigCam].camera.MotionDetector.Reset();


            }


        }



        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam).areaDetectionWithin = radioButton1.Checked;
            CameraRig.ConnectedCameraPopulateForCam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam);
            ConfigurationHelper.GetCurrentProfile().areaDetectionWithin = radioButton1.Checked;
            CameraRig.AreaDetectionWithin = radioButton1.Checked;
            CameraRig.getCam(selectedWebcam).MotionDetector.Reset();
        }

        private void areaOffAtMotion_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam).areaOffAtMotion = areaOffAtMotion.Checked;
            CameraRig.ConnectedCameraPopulateForCam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam);
            CameraRig.ConnectedCameras[CameraRig.ConfigCam].camera.areaOffAtMotionTriggered = false;
            CameraRig.ConnectedCameras[CameraRig.ConfigCam].camera.areaOffAtMotionReset = false;

        }

        public bool DrawMode()
        {
            return drawModeOn.Checked;
        }

        private void drawModeOn_CheckedChanged(object sender, EventArgs e)
        {
            bool drawMode = drawModeOn.Checked;

            if (drawMode)
            {

                System.Diagnostics.Debug.WriteLine(CameraRig.cameraCount());

                if (ConfigurationHelper.GetCurrentProfile().imageToframe)
                {
                    cameraWindow.imageToFrame = false;
                    panel3.AutoScroll = true;
                }

                cameraWindow.imageToFrame = true;
                panel3.AutoScroll = false;

                if (!ConfigurationHelper.GetCurrentProfile().cameraShow)
                {
                    cameraWindow.showCam = true;
                }

                exposeArea = false;
                CameraRig.ExposeArea = false;
                radioButton8.Checked = false;

                groupBox11.Enabled = false;
                groupBox4.Enabled = false;
                showSelection.Enabled = false;

                CameraRig.getCam(selectedWebcam).MotionDetector.Reset();
                Thread.Sleep(250);
                cameraWindow.Camera.Lock();

                drawArgs a = new drawArgs();
                a.x = CameraRig.getCam(selectedWebcam).rectX;
                a.y = CameraRig.getCam(selectedWebcam).rectY;
                a.width = CameraRig.getCam(selectedWebcam).rectWidth;
                a.height = CameraRig.getCam(selectedWebcam).rectHeight;
                drawInitialRectangle(null, a);

                System.Diagnostics.Debug.WriteLine(CameraRig.cameraCount());

                CameraRig.getCam(selectedWebcam).MotionDetector.Reset();

                //System.Diagnostics.Debug.WriteLine(CameraRig.drawCam);

            }
            else
            {
                System.Diagnostics.Debug.WriteLine(CameraRig.cameraCount());

                if (ConfigurationHelper.GetCurrentProfile().imageToframe)
                {
                    cameraWindow.imageToFrame = true;
                    panel3.AutoScroll = false;
                }
                else
                {
                    cameraWindow.imageToFrame = false;
                    panel3.AutoScroll = true;
                }

                if (!ConfigurationHelper.GetCurrentProfile().cameraShow)
                {
                    cameraWindow.showCam = false;
                }

                groupBox11.Enabled = true;
                groupBox4.Enabled = radioButton4.Checked;
                showSelection.Enabled = radioButton4.Checked;
                areaOffAtMotion.Enabled = radioButton4.Checked;

                cameraWindow.Camera.Unlock();

            }

            if (ConfigurationHelper.GetCurrentProfile().areaDetection) { cameraWindow.drawRectOnOpen(); }
            cameraWindow.selectionOn = drawMode;
            cameraWindow.rectDrawn = true;
        }

        private void button15_Click(object sender, EventArgs e)
        {

            ConfigurationHelper.GetCurrentProfile().rectX = 20;
            ConfigurationHelper.GetCurrentProfile().rectY = 20;
            ConfigurationHelper.GetCurrentProfile().rectWidth = 80;
            ConfigurationHelper.GetCurrentProfile().rectHeight = 80;

            ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam).rectX = 20;
            ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam).rectY = 20;
            ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam).rectWidth = 80;
            ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam).rectHeight = 80;


            CameraRig.getCam(selectedWebcam).rectX = 20;
            CameraRig.getCam(selectedWebcam).rectY = 20;
            CameraRig.getCam(selectedWebcam).rectWidth = 80;
            CameraRig.getCam(selectedWebcam).rectHeight = 80;

            cameraWindow.drawRectOnOpen();

        }



        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            exposeArea = radioButton8.Checked;
            CameraRig.ExposeArea = radioButton8.Checked;
            radioButton7.Checked = !radioButton8.Checked;
        }

        private void calibrate_Click(object sender, EventArgs e)
        {
            var calibrate = new calibrate(postCalibrate, toolTip1.Active, CameraRig.getCam(selectedWebcam).camNo, publisher)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            calibrate.ShowDialog();
        }

        private void postCalibrate(ArrayList results)
        {

            trkMov.Value = (int)results[0];
            trkTimeSpike.Value = (int)results[1];
            trkToleranceSpike.Value = (int)results[2];
            rdSpikeOn.Checked = true;

        }


        private void button8_Click(object sender, EventArgs e)
        {

            if (!movementSetting)
            {
                ArrayList i = new ArrayList();

                i.Add(ConfigurationHelper.GetCurrentProfile().toolTips);
                train train = new train(new formDelegate(trainDetection), i);
                train.StartPosition = FormStartPosition.CenterScreen;
                train.ShowDialog();
            }

        }


        private void txtMov_TextChanged(object sender, EventArgs e)
        {
            if (!Valid.IsNumeric(txtMov.Text))
            {
                txtMov.Text = "1";
            }

            if (txtMov.Text.Trim() == "0") txtMov.Text = "1";
            //if (txtMov.Text.Trim() == "100") txtMov.Text = "99";

            trkMov.Value = Convert.ToInt32(txtMov.Text);

            ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam).movementVal = Convert.ToDouble(txtMov.Text) / 100;
            CameraRig.ConnectedCameraPopulateForCam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam);
            CameraRig.ConnectedCameras[CameraRig.ConfigCam].camera.movementVal = Convert.ToDouble(txtMov.Text) / 100;

        }

        private void txtMov_Leave(object sender, EventArgs e)
        {

            if (Convert.ToInt32(txtMov.Text) > 99 || Convert.ToInt32(txtMov.Text) < 1)
            {
                txtMov.Focus();
                MessageDialog.messageAlert("Value must be >= 1 and <= 99", "Error with sensitivity value");
                txtMov.Text = "1";
            }

        }




        private void trainDetection(ArrayList i)
        {

            CameraRig.TrainCam = CameraRig.getCam(selectedWebcam).camNo;

            detectionCountDown = (int)i[0];
            detectionTrain = (int)i[1];

            actCount.Visible = true;
            TebocamState.training.Clear();
            movementSetting = true;
            dw.DoWork -= new DoWorkEventHandler(trainMovement);
            dw.DoWork += new DoWorkEventHandler(trainMovement);
            dw.WorkerSupportsCancellation = true;
            dw.RunWorkerAsync();

        }
        //

        private void trainMovement(object sender, DoWorkEventArgs e)
        {

            int secondsToTrainStart;
            int secondsToLast = 0;

            int tmpInt;
            double tmpDbl, tmpVal;

            while (baselineSetting || movementSetting)
            {

                tmpInt = detectionCountDown;
                secondsToTrainStart = time.secondsSinceStart();

                CameraRig.getCam(CameraRig.TrainCam).calibrating = false;

                actCount.ForeColor = Color.Blue;
                txtMess.ForeColor = Color.Blue;
                txtMess.SynchronisedInvoke(() => txtMess.Text = "Counting Down");
                //SetInfo(txtMess, "Counting Down");

                while (tmpInt > 0)
                {

                    tmpInt = detectionCountDown + ((int)secondsToTrainStart - time.secondsSinceStart());
                    if (secondsToLast != tmpInt)
                    {
                        actCount.SynchronisedInvoke(() => actCount.Text = Convert.ToString(tmpInt));
                        //SetInfo(actCount, Convert.ToString(tmpInt));
                    }
                    secondsToLast = tmpInt;
                }

                tmpInt = detectionTrain;
                secondsToTrainStart = time.secondsSinceStart();
                CameraRig.getCam(CameraRig.TrainCam).detectionOn = true;
                CameraRig.getCam(CameraRig.TrainCam).calibrating = true;
                actCount.ForeColor = Color.Red;
                txtMess.ForeColor = Color.Red;

                txtMess.SynchronisedInvoke(() => txtMess.Text = "Recording Movement");
                //SetInfo(txtMess, "Recording Movement");

                while (tmpInt > 0)
                {
                    tmpInt = detectionTrain + (secondsToTrainStart - time.secondsSinceStart());
                    if (secondsToLast != tmpInt)
                    {
                        actCount.SynchronisedInvoke(() => actCount.Text = Convert.ToString(tmpInt));
                        //SetInfo(actCount, Convert.ToString(tmpInt));
                    }
                    secondsToLast = tmpInt;
                }

                //calculate average motion sensitivity setting
                //only calculate average based on non zero values
                CameraRig.getCam(CameraRig.TrainCam).detectionOn = false;
                CameraRig.getCam(CameraRig.TrainCam).calibrating = false;
                tmpDbl = 0;
                tmpInt = 0;
                foreach (double val in TebocamState.training)
                {
                    if (val > 0)
                    {
                        tmpDbl += val;
                        tmpInt++;
                    }
                }

                tmpVal = 0;
                if (tmpDbl > 0)
                {
                    tmpVal = tmpDbl / (double)tmpInt;
                }

                CameraRig.getCam(CameraRig.TrainCam).movementVal = tmpVal;
                //config.GetCurrentProfile().movementVal = tmpVal;
                txtMov.SynchronisedInvoke(() => txtMov.Text = Convert.ToString((int)Math.Floor(tmpVal * 100)));
                //SetInfo(txtMov, Convert.ToString((int)Math.Floor(tmpVal * 100)));

                actCount.ForeColor = Color.Blue;
                txtMess.ForeColor = Color.Blue;
                txtMov.SynchronisedInvoke(() => txtMov.Text = string.Empty);
                actCount.SynchronisedInvoke(() => actCount.Text = string.Empty);
                //SetInfo(txtMess, "");
                //SetInfo(actCount, "");
                FileManager.WriteFile("training");
                baselineSetting = false;
                movementSetting = false;

                dw.Dispose();
            }

        }

        private void webcamConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                //20110506
                drawModeOff.Checked = true;
                drawModeOn.Checked = false;
                radioButton7.Checked = true;
                radioButton8.Checked = false;
                //20110506

                returnList.Clear();

                for (int i = 0; i < CameraRig.cameraCount(); i++)
                {
                    CameraRig.getCam(i).motionLevelEvent -= new motionLevelEventHandler(drawLevel);
                    CameraRig.getCam(i).calibrating = false;
                }

                //levelBitmap.Dispose();

                //20130617 v - noopped as main camera window was showing a blank image
                //cameraWindow.Camera = null;
                //20130617 ^ - noopped as main camera window was showing a blank image

                CameraRig.ExposeArea = false;

                System.Diagnostics.Debug.WriteLine(CameraRig.cameraCount());

                returnList.Add(mainSelectedWebcam + 1);
                returnList.Add(autoscroll);

                webcamConfigDelegate(returnList);

            }
            catch (Exception ex)
            {
                TebocamState.tebowebException.LogException(ex);
            }

        }


        private void drawLevel(object sender, MotionLevelArgs a, CamIdArgs b)
        {

            if (b.camNo == CameraRig.ConfigCam)
            {

                double sensitivity = (double)Convert.ToInt32(txtMov.Text);
                LevelControlBox.levelDraw(Convert.ToInt32(a.lvl * 100), sensitivity);

            }

        }




        private void bttncam1_Click(object sender, EventArgs e)
        {
            cameraSwitch(1, false);
        }
        private void bttncam2_Click(object sender, EventArgs e)
        {
            cameraSwitch(2, false);
        }
        private void bttncam3_Click(object sender, EventArgs e)
        {
            cameraSwitch(3, false);
        }
        private void bttncam4_Click(object sender, EventArgs e)
        {
            cameraSwitch(4, false);
        }
        private void bttncam5_Click(object sender, EventArgs e)
        {
            cameraSwitch(5, false);
        }
        private void bttncam6_Click(object sender, EventArgs e)
        {
            cameraSwitch(6, false);
        }
        private void bttncam7_Click(object sender, EventArgs e)
        {
            cameraSwitch(7, false);
        }
        private void bttncam8_Click(object sender, EventArgs e)
        {
            cameraSwitch(8, false);
        }
        private void bttncam9_Click(object sender, EventArgs e)
        {
            cameraSwitch(9, false);
        }


        private bool camClick(int button)
        {
            bool canClick = CameraButtons.Any(x => x.id == button && x.CameraButtonState != GroupCameraButton.ButtonState.NotConnected);
            if (!canClick) return false;

            var connected = CameraButtons.Where(x => x.CameraButtonState != GroupCameraButton.ButtonState.NotConnected).ToList();
            connected.ForEach(x => x.CameraButtonIsConnectedAndInactive());
            var newActiveButton = CameraButtons.First(x => x.id == button);
            newActiveButton.CameraButtonIsActive();
            return true;

        }

        private void cameraSwitch(int button, bool load)
        {

            currentlySelectedButton = button;

            //int camId = CameraRig.idFromButton(button);
            int camId = CameraRig.idxFromButton(button);


            if (load || !load && camClick(button))
            {
                if (load || !load && CameraRig.cameraExists(camId))
                {

                    selectedWebcam = CameraRig.ConnectedCameras[camId].cameraName;
                    CameraRig.ConfigCam = camId;
                    CameraRig.getCam(camId).MotionDetector.Reset();

                    camName.Text = ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam).friendlyName;
                    drawModeOn.Checked = false;
                    drawModeOff.Checked = true;
                    txtMov.Text = ((double)(ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam).movementVal * 100)).ToString();
                    radioButton4.Checked = ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam).areaDetection;
                    radioButton3.Checked = !radioButton4.Checked;
                    areaOffAtMotion.Checked = ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam).areaOffAtMotion;
                    radioButton8.Checked = false;
                    radioButton7.Checked = true;
                    radioButton1.Checked = ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam).areaDetectionWithin;
                    radioButton2.Checked = !radioButton1.Checked;


                    trkTimeSpike.Value = ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam).timeSpike;
                    trkToleranceSpike.Value = ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam).toleranceSpike;
                    rdSpikeOn.Checked = ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam).lightSpike;
                    lblTimeSpike.Text = trkTimeSpike.Value.ToString();
                    lblToleranceSpike.Text = trkToleranceSpike.Value.ToString() + "%";

                    cameraWindow.Camera = null;
                    cameraWindow.Camera = CameraRig.getCam(camId); //CameraRig.rig[0].cam;

                    CameraRig.getCam(camId).motionLevelEvent -= new motionLevelEventHandler(drawLevel);
                    CameraRig.getCam(camId).motionLevelEvent += new motionLevelEventHandler(drawLevel);

                    camButtonSetColours();
                    //camControl(button);

                }

            }

        }


        public static List<Control> controls(Control form)
        {
            List<Control> controlList = new List<Control>();

            foreach (Control childControl in form.Controls)
            {
                {
                    // Recurse child controls.
                    controlList.AddRange(controls(childControl));
                    controlList.Add(childControl);
                }
            }
            return controlList;
        }


        private void camControl(int cam)
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
                    if (ctrl.Text == Convert.ToString(cam + 1))
                    {
                        ctrl.BackColor = Color.LawnGreen;
                    }
                    else
                    {
                        if (Convert.ToInt32(ctrl.Text) <= CameraRig.cameraCount())
                        {
                            ctrl.BackColor = Color.SkyBlue;
                        }
                    }
                }
            }

            availControls.Clear();

        }

        private void camButtonSetColours()
        {

            foreach (var buttonGroup in CameraButtons)
            {
                //display camera buttons
                if (buttonGroup.id == CameraRig.ConnectedCameras[CameraRig.ConfigCam].displayButton)
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
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "Change Button")
            {
                button3.Text = "Lock Buttons";
                button2.BackColor = Color.LawnGreen;
                button1.BackColor = Color.LawnGreen;
            }
            else
            {
                button3.Text = "Change Button";
                button2.BackColor = System.Drawing.SystemColors.Control;
                button1.BackColor = System.Drawing.SystemColors.Control;
            }

            button1.Enabled = !button1.Enabled;
            button2.Enabled = !button2.Enabled;
        }

        private void camUp(object sender, EventArgs e)
        {

            int wasButton = CameraRig.ConnectedCameras[CameraRig.ConfigCam].displayButton;
            int nowButton;

            if (wasButton == 9)
            {
                nowButton = 1;
            }
            else
            {
                nowButton = wasButton + 1;
            }

            CameraRig.changeDisplayButton(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam, CameraRig.ConfigCam, nowButton);
            cameraSwitch(nowButton, true);
        }

        private void camDown(object sender, EventArgs e)
        {

            int wasButton = CameraRig.ConnectedCameras[CameraRig.ConfigCam].displayButton;
            int nowButton;

            if (wasButton == 1)
            {
                nowButton = 9;
            }
            else
            {
                nowButton = wasButton - 1;
            }

            CameraRig.changeDisplayButton(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam, CameraRig.ConfigCam, nowButton);
            cameraSwitch(nowButton, true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (button5.Enabled &&
                CameraButtons.Where(x => x.id == currentlySelectedButton).First().CameraButtonState != GroupCameraButton.ButtonState.NotConnected
                )
            {
                var response = MessageBox.Show("Are you sure you want to remove this camera and all the associated information?" +
                    Environment.NewLine + Environment.NewLine +
                     "Once the camera is removed, if you require it again you will need to add the webcam again " +
                     "together with all the camera specific settings associated with it.", "Remove camera and associated information?",
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (response == DialogResult.Yes)
                {
                    CameraRig.cameraRemove(CameraRig.idxFromButton(currentlySelectedButton), true);
                    int firstAvailableButton = CameraRig.ConnectedCameras.Where(x => x.displayButton > 0).First().displayButton;
                    cameraSwitch(firstAvailableButton, false);
                    camButtonSetColours();
                    button5.Enabled = false;
                    button5.BackColor = System.Drawing.SystemColors.Control;
                    saveChanges();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (!button5.Enabled)
            {
                button5.Enabled = true;
                button5.BackColor = Color.LawnGreen;
            }
            else
            {
                button5.Enabled = false;
                button5.BackColor = System.Drawing.SystemColors.Control;
            }

        }


        private void rdSpikeOn_CheckedChanged(object sender, EventArgs e)
        {
            grpSpikeSettings.Enabled = rdSpikeOn.Checked;
            ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam).lightSpike = grpSpikeSettings.Enabled;
        }

        private void trkMov_ValueChanged(object sender, EventArgs e)
        {
            if (trkMov.Value == 100)
            {
                txtMov.Text = "99";
            }
            else
            {
                txtMov.Text = trkMov.Value.ToString();
            }

            ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam).movementVal = Convert.ToDouble(txtMov.Text) / 100;
            CameraRig.ConnectedCameraPopulateForCam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam);
        }

        private void trkTimeSpike_ValueChanged(object sender, EventArgs e)
        {
            lblTimeSpike.Text = trkTimeSpike.Value.ToString();
            //config.GetCurrentProfile().timeSpike = trkTimeSpike.Value;
            ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam).timeSpike = trkTimeSpike.Value;
        }

        private void trkToleranceSpike_ValueChanged(object sender, EventArgs e)
        {
            lblToleranceSpike.Text = trkToleranceSpike.Value.ToString() + "%";
            //config.GetCurrentProfile().toleranceSpike = trkToleranceSpike.Value;
            ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), selectedWebcam).toleranceSpike = trkToleranceSpike.Value;
        }


    }
}