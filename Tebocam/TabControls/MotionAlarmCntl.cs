using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TeboCam
{
    public partial class MotionAlarmCntl : UserControl
    {
        public delegate bool LoadingDelegate();
        LoadingDelegate Loading;
        public delegate bool CountingdownDelegate();
        CountingdownDelegate countingdown;
        public delegate void ScheduleSetDelegate(ArrayList i);
        ScheduleSetDelegate scheduleSet;
        public delegate void CountingdownstopDelegate(bool a);
        CountingdownstopDelegate countingdownstop;
        public delegate void ActiveCountdownDelegate(object sender, DoWorkEventArgs e);
        ActiveCountdownDelegate activeCountdown;
        public delegate void ActCountVisibleDelegate(bool a);
        ActCountVisibleDelegate actCountVisible;
        public delegate void selcambleDelegate(int a, bool b);
        selcambleDelegate selcam;
        BackgroundWorker bw;
        List<GroupCameraButton> NotConnectedCameras;

        public MotionAlarmCntl(selcambleDelegate selcam,
                               ActCountVisibleDelegate actCountVisible,
                               ActiveCountdownDelegate activeCountdown,
                               CountingdownstopDelegate countingdownstop,
                               ScheduleSetDelegate scheduleSet,
                               LoadingDelegate Loading,
                               CountingdownDelegate countingdown,
                               BackgroundWorker bw,
                               List<GroupCameraButton> NotConnectedCameras)
        {
            InitializeComponent();
            this.selcam = selcam;
            this.actCountVisible = actCountVisible;
            this.Loading = Loading;
            this.countingdown = countingdown;
            this.bw = bw;
            this.scheduleSet = scheduleSet;
            this.countingdownstop = countingdownstop;
            this.activeCountdown = activeCountdown;
            this.NotConnectedCameras = NotConnectedCameras;
            this.selcam = selcam;
        }

        public TextBox GetActCountdown() { return actCountdown; }
        public Label GetLblstartmov() { return lblstartmov; }
        public Label GetLblendmov() { return lblendmov; }
        public Label GetLblTime() { return lblTime; }
        public CheckBox GetBttnMotionSchedule() { return bttnMotionSchedule; }
        public CheckBox GetBttnActivateAtEveryStartup() { return bttnActivateAtEveryStartup; }
        public CheckBox GetBttnMotionScheduleOnAtStart() { return bttnMotionScheduleOnAtStart; }
        public RadioButton GetBttnMotionInactive() { return bttnMotionInactive; }
        public RadioButton GetBttnMotionAtStartup() { return bttnMotionAtStartup; }
        public RadioButton GetBttnNow() { return bttnNow; }
        public RadioButton GetBttnTime() { return bttnTime; }
        public RadioButton GetBttnSeconds() { return bttnSeconds; }
        public RadioButton GetBttnMotionActive() { return bttnMotionActive; }
        public NumericUpDown GetNumericUpDown1() { return numericUpDown1; }
        public NumericUpDown GetNumericUpDown2() { return numericUpDown2; }

        private void button38_Click(object sender, EventArgs e)
        {
            ArrayList i = new ArrayList();
            i.Add("Alert");
            i.Add(ConfigurationHelper.GetCurrentProfile().toolTips);
            i.Add(ConfigurationHelper.GetCurrentProfile().timerStartMov);
            i.Add(ConfigurationHelper.GetCurrentProfile().timerEndMov);
            schedule schedule = new schedule(new formDelegate(scheduleSet), i);
            schedule.StartPosition = FormStartPosition.CenterScreen;
            schedule.ShowDialog();
        }

        private void bttnMotionActive_CheckedChanged(object sender, EventArgs e)
        {
            Movement.areaOffAtMotionInit();

            if (bttnMotionActive.Checked)
            {
                if (!countingdown())
                {

                    //about to go to active motion detection
                    //however no camera is selected as active
                    //so activate all cameras.
                    //as we could choose an incorrect camera.

                    if (!licence.aCameraIsSelected())
                    {
                        var availableCameraButtons = NotConnectedCameras.Where(x => x.CameraButtonState != GroupCameraButton.ButtonState.ConnectedAndActive).ToList();
                        availableCameraButtons.ForEach(x => selcam(x.id, true));
                    }

                    actCountVisible(true);
                    countingdownstop(false);
                    bool a = bw.IsBusy;
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

                countingdownstop(true);
                TebocamState.Alert.on = false;
                TebocamState.log.AddLine("Motion detection inactivated");
            }
        }

        private void bttnActivateAtEveryStartup_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().activateAtEveryStartup = bttnActivateAtEveryStartup.Checked;
        }

        private void bttnNow_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().countdownNow = bttnNow.Checked;
        }

        private void bttnTime_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().countdownTime = bttnTime.Checked;
        }

        private void actCountdown_TextChanged(object sender, EventArgs e)
        {
            if (Valid.IsNumeric(actCountdown.Text))
            {
                ConfigurationHelper.GetCurrentProfile().activatecountdown = Convert.ToInt32(actCountdown.Text);
            }
            else
            {
                actCountdown.Text = "0";
                ConfigurationHelper.GetCurrentProfile().activatecountdown = 0;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value == 24) { numericUpDown1.Value = 0; }
            if (numericUpDown1.Value == -1) { numericUpDown1.Value = 23; }
            if (!Loading()) { ConfigurationHelper.GetCurrentProfile().activatecountdownTime = numericUpDown1.Value.ToString().PadLeft(2, '0') + numericUpDown2.Value.ToString().PadLeft(2, '0'); }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown2.Value == 60) { numericUpDown2.Value = 0; }
            if (numericUpDown2.Value == -1) { numericUpDown2.Value = 59; }
            if (!Loading()) { ConfigurationHelper.GetCurrentProfile().activatecountdownTime = numericUpDown1.Value.ToString().PadLeft(2, '0') + numericUpDown2.Value.ToString().PadLeft(2, '0'); }
        }

        private void bttnMotionSchedule_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().timerOnMov = bttnMotionSchedule.Checked;

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
            ConfigurationHelper.GetCurrentProfile().scheduleOnAtStart = bttnMotionScheduleOnAtStart.Checked;
        }
    }
}
