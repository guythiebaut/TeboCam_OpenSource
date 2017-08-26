using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Linq;


//using AForge.Video;
using AForge.Video.DirectShow;


namespace TeboCam
{

    public delegate void configSettingsDelegate(List<int> list);


    public partial class webcamConfig : Form
    {
        public CameraButtonsCntl ButtonCameraControl = new CameraButtonsCntl();
        List<CameraButtonGroup> CameraButtons = new List<CameraButtonGroup>();
        private formDelegate webcamConfigDelegate;
        private int currentlySelectedButton;

        public static BackgroundWorker dw = new BackgroundWorker();

        public static event drawEventHandler drawInitialRectangle;




        private static string selectedWebcam;
        private static int mainSelectedWebcam;
        //private static List<camButtons.ButtonColourEnum> buttons;
        private LevelControl LevelControlBox = new LevelControl();
        private ArrayList returnList = new ArrayList();
        private bool autoscroll;
        private static bool toolTip;

        public webcamConfig(formDelegate sender, ArrayList from, List<CameraButtonGroup> CameraButtonGroupInstance)
        {

            toolTip = (bool)from[0];
            mainSelectedWebcam = (int)from[1];
            autoscroll = (bool)from[2];
            // buttons = (List<camButtons.ButtonColourEnum>)from[3];
            webcamConfigDelegate = sender;
            from.Clear();
            InitializeComponent();

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

        private void ConfigureCameraButtons(List<CameraButtonGroup> CameraButtonGroupInstance)
        {
            foreach (CameraButtonGroup group in CameraButtonGroupInstance)
            {
                ButtonCameraControl.AddButton(CameraButtons, ButtonCameraDelegation, null, false,18);
            }
            panel2.Controls.Add(ButtonCameraControl);
            ButtonCameraControl.Location = new Point(10, 2);
        }

        private void ButtonCameraDelegation(int id, Button cameraButton, Button activeButton)
        {
            cameraSwitch(id, false);
        }

        private void webcamConfig_Load(object sender, EventArgs e)
        {
            toolTip1.Active = toolTip;
        }

        private void camName_TextChanged(object sender, EventArgs e)
        {

            CameraRig.updateInfo(bubble.profileInUse, selectedWebcam, CameraRig.infoEnum.friendlyName, camName.Text);
            CameraRig.rigInfoPopulateForCam(bubble.profileInUse, selectedWebcam);

        }


        private void bttnCamProp_Click(object sender, EventArgs e)
        {

            //hjfgjgf

            if (CameraRig.ConnectedCameras[CameraRig.ConfigCam].cam.isIPCamera)
            {

                try
                {

                    IPAddress parsedIPAddress;
                    Uri parsedUri;
                    string name = CameraRig.ConnectedCameras[CameraRig.ConfigCam].cam.name;

                    //check that the url resolves
                    if (Uri.TryCreate(name, UriKind.Absolute, out parsedUri) && IPAddress.TryParse(parsedUri.DnsSafeHost, out parsedIPAddress))
                    {

                        bubble.openInternetBrowserAt("http:\\" + parsedIPAddress.ToString());

                    }

                }
                catch (Exception)
                {

                    bubble.logAddLine("Unable to connect webcam index site.");

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

                CameraRig.updateInfo(bubble.profileInUse, selectedWebcam, CameraRig.infoEnum.areaDetection, radioButton4.Checked);
                CameraRig.rigInfoPopulateForCam(bubble.profileInUse, selectedWebcam);

                CameraRig.getCam(selectedWebcam).MotionDetector.Reset();
                config.getProfile(bubble.profileInUse).areaDetection = radioButton4.Checked;
                CameraRig.getCam(selectedWebcam).areaDetection = radioButton4.Checked;

                areaOffAtMotion.Enabled = radioButton4.Checked;
                groupBox4.Enabled = radioButton4.Checked;
                showSelection.Enabled = radioButton4.Checked;

                CameraRig.ConnectedCameras[CameraRig.ConfigCam].cam.MotionDetector.Reset();


            }


        }



        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            CameraRig.updateInfo(bubble.profileInUse, selectedWebcam, CameraRig.infoEnum.areaDetectionWithin, radioButton1.Checked);
            CameraRig.rigInfoPopulateForCam(bubble.profileInUse, selectedWebcam);
            config.getProfile(bubble.profileInUse).areaDetectionWithin = radioButton1.Checked;
            CameraRig.AreaDetectionWithin = radioButton1.Checked;
            CameraRig.getCam(selectedWebcam).MotionDetector.Reset();
        }

        private void areaOffAtMotion_CheckedChanged(object sender, EventArgs e)
        {

            CameraRig.updateInfo(bubble.profileInUse, selectedWebcam, CameraRig.infoEnum.areaOffAtMotion, areaOffAtMotion.Checked);
            CameraRig.rigInfoPopulateForCam(bubble.profileInUse, selectedWebcam);
            CameraRig.ConnectedCameras[CameraRig.ConfigCam].cam.areaOffAtMotionTriggered = false;
            CameraRig.ConnectedCameras[CameraRig.ConfigCam].cam.areaOffAtMotionReset = false;

        }

        private void drawModeOn_CheckedChanged(object sender, EventArgs e)
        {


            bool drawMode = drawModeOn.Checked;
            bubble.drawMode = drawModeOn.Checked;

            if (drawMode)
            {

                System.Diagnostics.Debug.WriteLine(CameraRig.cameraCount());

                if (config.getProfile(bubble.profileInUse).imageToframe)
                {
                    cameraWindow.imageToFrame = false;
                    panel3.AutoScroll = true;
                }

                if (!config.getProfile(bubble.profileInUse).cameraShow)
                {
                    cameraWindow.showCam = true;
                }


                bubble.exposeArea = false;
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

                if (config.getProfile(bubble.profileInUse).imageToframe)
                {
                    cameraWindow.imageToFrame = true;
                    panel3.AutoScroll = false;
                }
                else
                {
                    cameraWindow.imageToFrame = false;
                    panel3.AutoScroll = true;
                }

                if (!config.getProfile(bubble.profileInUse).cameraShow)
                {
                    cameraWindow.showCam = false;
                }

                groupBox11.Enabled = true;
                groupBox4.Enabled = radioButton4.Checked;
                showSelection.Enabled = radioButton4.Checked;
                areaOffAtMotion.Enabled = radioButton4.Checked;

                cameraWindow.Camera.Unlock();

            }

            if (config.getProfile(bubble.profileInUse).areaDetection) { cameraWindow.drawRectOnOpen(); }
            cameraWindow.selectionOn = drawMode;
            cameraWindow.rectDrawn = true;


        }

        private void button15_Click(object sender, EventArgs e)
        {

            config.getProfile(bubble.profileInUse).rectX = 20;
            config.getProfile(bubble.profileInUse).rectY = 20;
            config.getProfile(bubble.profileInUse).rectWidth = 80;
            config.getProfile(bubble.profileInUse).rectHeight = 80;

            CameraRig.updateInfo(bubble.profileInUse, selectedWebcam, CameraRig.infoEnum.rectX, 20);
            CameraRig.updateInfo(bubble.profileInUse, selectedWebcam, CameraRig.infoEnum.rectY, 20);
            CameraRig.updateInfo(bubble.profileInUse, selectedWebcam, CameraRig.infoEnum.rectWidth, 80);
            CameraRig.updateInfo(bubble.profileInUse, selectedWebcam, CameraRig.infoEnum.rectHeight, 80);

            CameraRig.getCam(selectedWebcam).rectX = 20;
            CameraRig.getCam(selectedWebcam).rectY = 20;
            CameraRig.getCam(selectedWebcam).rectWidth = 80;
            CameraRig.getCam(selectedWebcam).rectHeight = 80;

            cameraWindow.drawRectOnOpen();

        }



        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            bubble.exposeArea = radioButton8.Checked;
            CameraRig.ExposeArea = radioButton8.Checked;
            radioButton7.Checked = !radioButton8.Checked;
        }

        private void calibrate_Click(object sender, EventArgs e)
        {

            ArrayList i = new ArrayList();

            //i.Add(config.getProfile(bubble.profileInUse).toolTips);
            i.Add(toolTip1.Active);
            i.Add(CameraRig.getCam(selectedWebcam).camNo);
            //System.Diagnostics.Debug.WriteLine("cam sent: " + Convert.ToString(i[1]));

            calibrate calibrate = new calibrate(postCalibrate, i);
            calibrate.StartPosition = FormStartPosition.CenterScreen;
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

            if (!bubble.movementSetting)
            {
                ArrayList i = new ArrayList();

                i.Add(config.getProfile(bubble.profileInUse).toolTips);
                train train = new train(new formDelegate(trainDetection), i);
                train.StartPosition = FormStartPosition.CenterScreen;
                train.ShowDialog();
            }

        }


        private void txtMov_TextChanged(object sender, EventArgs e)
        {
            if (!bubble.IsNumeric(txtMov.Text))
            {
                txtMov.Text = "1";
            }

            if (txtMov.Text.Trim() == "0") txtMov.Text = "1";
            //if (txtMov.Text.Trim() == "100") txtMov.Text = "99";

            trkMov.Value = Convert.ToInt32(txtMov.Text);

            CameraRig.updateInfo(bubble.profileInUse, selectedWebcam, CameraRig.infoEnum.movementVal, Convert.ToDouble(txtMov.Text) / 100);
            CameraRig.rigInfoPopulateForCam(bubble.profileInUse, selectedWebcam);
            CameraRig.ConnectedCameras[CameraRig.ConfigCam].cam.movementVal = Convert.ToDouble(txtMov.Text) / 100;

        }

        private void txtMov_Leave(object sender, EventArgs e)
        {

            if (Convert.ToInt32(txtMov.Text) > 99 || Convert.ToInt32(txtMov.Text) < 1)
            {
                txtMov.Focus();
                bubble.messageAlert("Value must be >= 1 and <= 99", "Error with sensitivity value");
                txtMov.Text = "1";
            }

        }




        private void trainDetection(ArrayList i)
        {

            CameraRig.TrainCam = CameraRig.getCam(selectedWebcam).camNo;

            bubble.detectionCountDown = (int)i[0];
            bubble.detectionTrain = (int)i[1];

            actCount.Visible = true;
            bubble.training.Clear();
            bubble.movementSetting = true;
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

            while (bubble.baselineSetting || bubble.movementSetting)
            {

                tmpInt = bubble.detectionCountDown;
                secondsToTrainStart = time.secondsSinceStart();

                CameraRig.getCam(CameraRig.TrainCam).calibrating = false;

                actCount.ForeColor = Color.Blue;
                txtMess.ForeColor = Color.Blue;
                txtMess.SynchronisedInvoke(() => txtMess.Text = "Counting Down");
                //SetInfo(txtMess, "Counting Down");

                while (tmpInt > 0)
                {

                    tmpInt = bubble.detectionCountDown + ((int)secondsToTrainStart - time.secondsSinceStart());
                    if (secondsToLast != tmpInt)
                    {
                        actCount.SynchronisedInvoke(() => actCount.Text = Convert.ToString(tmpInt));
                        //SetInfo(actCount, Convert.ToString(tmpInt));
                    }
                    secondsToLast = tmpInt;
                }

                tmpInt = bubble.detectionTrain;
                secondsToTrainStart = time.secondsSinceStart();
                CameraRig.getCam(CameraRig.TrainCam).detectionOn = true;
                CameraRig.getCam(CameraRig.TrainCam).calibrating = true;
                actCount.ForeColor = Color.Red;
                txtMess.ForeColor = Color.Red;

                txtMess.SynchronisedInvoke(() => txtMess.Text = "Recording Movement");
                //SetInfo(txtMess, "Recording Movement");

                while (tmpInt > 0)
                {
                    tmpInt = bubble.detectionTrain + (secondsToTrainStart - time.secondsSinceStart());
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
                foreach (double val in bubble.training)
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

                config.getProfile(bubble.profileInUse).movementVal = tmpVal;
                txtMov.SynchronisedInvoke(() => txtMov.Text = Convert.ToString((int)Math.Floor(tmpVal * 100)));
                //SetInfo(txtMov, Convert.ToString((int)Math.Floor(tmpVal * 100)));

                actCount.ForeColor = Color.Blue;
                txtMess.ForeColor = Color.Blue;
                txtMov.SynchronisedInvoke(() => txtMov.Text = string.Empty);
                actCount.SynchronisedInvoke(() => actCount.Text = string.Empty);
                //SetInfo(txtMess, "");
                //SetInfo(actCount, "");
                FileManager.WriteFile("training");
                bubble.baselineSetting = false;
                bubble.movementSetting = false;

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
            catch { }

        }


        private void drawLevel(object sender, MotionLevelArgs a, CamIdArgs b)
        {

            if (b.cam == CameraRig.ConfigCam)
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
            bool canClick = CameraButtons.Where(x => x.id == button && x.CameraButtonState != CameraButtonGroup.ButtonState.NotConnected).Count() > 0;
            if (!canClick) return false;

            var connected = CameraButtons.Where(x => x.CameraButtonState != CameraButtonGroup.ButtonState.NotConnected).ToList();
            connected.ForEach(x => x.CameraButtonState = CameraButtonGroup.ButtonState.ConnectedAndInactive);
            var newActiveButton = CameraButtons.Where(x => x.id == button).First();
            newActiveButton.CameraButtonState = CameraButtonGroup.ButtonState.ConnectedAndActive;
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

                    camName.Text = (string)CameraRig.rigInfoGet(bubble.profileInUse, selectedWebcam, CameraRig.infoEnum.friendlyName);
                    drawModeOn.Checked = false;
                    drawModeOff.Checked = true;
                    txtMov.Text = Convert.ToString((double)(CameraRig.rigInfoGet(bubble.profileInUse, selectedWebcam, CameraRig.infoEnum.movementVal)) * 100);
                    radioButton4.Checked = (bool)(CameraRig.rigInfoGet(bubble.profileInUse, selectedWebcam, CameraRig.infoEnum.areaDetection));
                    radioButton3.Checked = !radioButton4.Checked;
                    areaOffAtMotion.Checked = (bool)(CameraRig.rigInfoGet(bubble.profileInUse, selectedWebcam, CameraRig.infoEnum.areaOffAtMotion));
                    radioButton8.Checked = false;
                    radioButton7.Checked = true;
                    radioButton1.Checked = (bool)(CameraRig.rigInfoGet(bubble.profileInUse, selectedWebcam, CameraRig.infoEnum.areaDetectionWithin));
                    radioButton2.Checked = !radioButton1.Checked;


                    trkTimeSpike.Value = (int)(CameraRig.rigInfoGet(bubble.profileInUse, selectedWebcam, CameraRig.infoEnum.timeSpike));
                    trkToleranceSpike.Value = (int)(CameraRig.rigInfoGet(bubble.profileInUse, selectedWebcam, CameraRig.infoEnum.toleranceSpike));
                    rdSpikeOn.Checked = (bool)(CameraRig.rigInfoGet(bubble.profileInUse, selectedWebcam, CameraRig.infoEnum.lightSpike));
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
                    if (CameraRig.CameraConnectedToButton(buttonGroup.id))
                    {
                        buttonGroup.CameraButtonIsInactive();
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

            CameraRig.changeDisplayButton(bubble.profileInUse, selectedWebcam, CameraRig.ConfigCam, nowButton);
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

            CameraRig.changeDisplayButton(bubble.profileInUse, selectedWebcam, CameraRig.ConfigCam, nowButton);
            cameraSwitch(nowButton, true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (CameraButtons.Where(x => x.id == currentlySelectedButton).First().CameraButtonState != CameraButtonGroup.ButtonState.NotConnected &&
                button5.Enabled)
            {
                CameraRig.cameraRemove(CameraRig.idxFromButton(currentlySelectedButton));
                int firstAvailableButton = CameraRig.ConnectedCameras.Where(x => x.displayButton > 0).First().displayButton;
                cameraSwitch(firstAvailableButton, false);
                camButtonSetColours();
                button5.Enabled = false;
                button5.BackColor = System.Drawing.SystemColors.Control;
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
            CameraRig.updateInfo(bubble.profileInUse, selectedWebcam, CameraRig.infoEnum.lightSpike, grpSpikeSettings.Enabled);

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

            CameraRig.updateInfo(bubble.profileInUse, selectedWebcam, CameraRig.infoEnum.movementVal, Convert.ToDouble(txtMov.Text) / 100);
            CameraRig.rigInfoPopulateForCam(bubble.profileInUse, selectedWebcam);
        }

        private void trkTimeSpike_ValueChanged(object sender, EventArgs e)
        {
            lblTimeSpike.Text = trkTimeSpike.Value.ToString();
            config.getProfile(bubble.profileInUse).timeSpike = trkTimeSpike.Value;
            CameraRig.updateInfo(bubble.profileInUse, selectedWebcam, CameraRig.infoEnum.timeSpike, trkTimeSpike.Value);
        }

        private void trkToleranceSpike_ValueChanged(object sender, EventArgs e)
        {
            lblToleranceSpike.Text = trkToleranceSpike.Value.ToString() + "%";
            config.getProfile(bubble.profileInUse).toleranceSpike = trkToleranceSpike.Value;
            CameraRig.updateInfo(bubble.profileInUse, selectedWebcam, CameraRig.infoEnum.toleranceSpike, trkToleranceSpike.Value);
        }

    }
}