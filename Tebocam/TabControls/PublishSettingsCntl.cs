using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections;

namespace TeboCam
{
    public partial class PublishSettingsCntl : UserControl
    {
        //List<CameraButtonGroup> PublishButtonGroupInstance;
        public delegate void FilePrefixSetDelegate(FilePrefixSettingsResultDto result);
        FilePrefixSetDelegate filePrefixSet;
        public delegate void ScheduleSetDelegate(ArrayList i);
        ScheduleSetDelegate scheduleSet;
        public delegate void SetPublishFirstDelegate(bool val);
        SetPublishFirstDelegate publishFirst;
        public delegate List<GroupCameraButton> GetPublishButtonGroupDelegate();
        GetPublishButtonGroupDelegate GetPublishButtonGroup;

        Publisher publisher;

        public PublishSettingsCntl(GetPublishButtonGroupDelegate PublishButtonGroupDel, FilePrefixSetDelegate filePrf, ScheduleSetDelegate schedule, Publisher publish, SetPublishFirstDelegate setPublishFirst)
        {
            InitializeComponent();
            GetPublishButtonGroup = PublishButtonGroupDel;
            filePrefixSet = filePrf;
            publisher = publish;
            scheduleSet = schedule;
            publishFirst = setPublishFirst;
        }

        public void SetPubTimerOn(bool val) { pubTimerOn.Checked = val; }
        public void SetLblstartpub(Color val) { lblstartpub.ForeColor = val; }
        public void SetLblstartpub(string val) { lblstartpub.Text = val; }
        public void SetLblendpub(Color val) { lblendpub.ForeColor = val; }
        public void SetLblendpub(string val) { lblendpub.Text = val; }
        public void SetPubImage(bool val) { pubImage.Checked = val; }
        public void SetPubFtpUser(string val) { pubFtpUser.Text = val; }
        public void SetPubFtpPass(string val) { pubFtpPass.Text = val; }
        public void SetPubFtpRoot(string val) { pubFtpRoot.Text = val; }
        public void SetPubTime(string val) { pubTime.Text = val; }
        public void SetPubHours(bool val) { pubHours.Checked = val; }
        public void SetPubMins(bool val) { pubMins.Checked = val; }
        public void SetPubSecs(bool val) { pubSecs.Checked = val; }
        public void SetPubToWeb(bool val) { pubToWeb.Checked = val; }
        public void SetPubToLocal(bool val) { pubToLocal.Checked = val; }
        public GroupBox GetGroupBox17() { return groupBox17; }
        public CheckBox GetPubImage() { return pubImage; }
        public CheckBox GetPubTimerOn() { return pubTimerOn; }

        private void pubImage_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().pubImage = pubImage.Checked;

            if (pubImage.Checked)
            {
                publishFirst(true);
                publisher.keepPublishing = true;
            }
            else
            {
                pubTimerOn.Checked = false;
                publisher.keepPublishing = false;
            }
        }

        private void pubTime_Leave(object sender, EventArgs e)
        {
            pubTime.Text = Valid.verifyInt(pubTime.Text.ToString(), 1, 99999, "1");
            if (CameraRig.ConnectedCameras.Count > 0)
            {
                var publishSelectedButton = GetPublishButtonGroup().Find(x => x.CameraButtonState == GroupCameraButton.ButtonState.ConnectedAndActive);
                var publishingCamera = CameraRig.ConnectedCameras.Find(x => x.displayButton == publishSelectedButton.id);
                ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), publishingCamera.cameraName).pubTime = Convert.ToInt32(pubTime.Text);
                ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), publishingCamera.cameraName).publishFirst = true;
            }
        }

        private void pubHours_CheckedChanged(object sender, EventArgs e)
        {
            if (CameraRig.ConnectedCameras.Count > 0)
            {
                var publishSelectedButton = GetPublishButtonGroup().Find(x => x.CameraButtonState == GroupCameraButton.ButtonState.ConnectedAndActive);
                var publishingCamera = CameraRig.ConnectedCameras.Find(x => x.displayButton == publishSelectedButton.id);
                ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), publishingCamera.cameraName).pubHours = pubHours.Checked;
                ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), publishingCamera.cameraName).publishFirst = true;
            }
        }

        private void pubMins_CheckedChanged(object sender, EventArgs e)
        {
            if (CameraRig.ConnectedCameras.Count > 0)
            {
                var publishSelectedButton = GetPublishButtonGroup().Find(x => x.CameraButtonState == GroupCameraButton.ButtonState.ConnectedAndActive);
                var publishingCamera = CameraRig.ConnectedCameras.Find(x => x.displayButton == publishSelectedButton.id);
                ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), publishingCamera.cameraName).pubMins = pubMins.Checked;
                ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), publishingCamera.cameraName).publishFirst = true;
            }
        }

        private void pubSecs_CheckedChanged(object sender, EventArgs e)
        {
            if (CameraRig.ConnectedCameras.Count > 0)
            {
                var publishSelectedButton = GetPublishButtonGroup().Find(x => x.CameraButtonState == GroupCameraButton.ButtonState.ConnectedAndActive);
                var publishingCamera = CameraRig.ConnectedCameras.First(x => x.displayButton == publishSelectedButton.id);
                ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), publishingCamera.cameraName).pubSecs = pubSecs.Checked;
                ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), publishingCamera.cameraName).publishFirst = true;
            }

        }

        private void pubToWeb_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().publishWeb = pubToWeb.Checked;

            if (CameraRig.ConnectedCameras.Count > 0)
            {
                var publishSelectedButton = GetPublishButtonGroup().Find(x => x.CameraButtonState == GroupCameraButton.ButtonState.ConnectedAndActive);

                if (publishSelectedButton == null)
                {
                    pubToWeb.Checked = false;
                    return;
                }

                var publishingCamera = CameraRig.ConnectedCameras.First(x => x.displayButton == publishSelectedButton.id);
                ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), publishingCamera.cameraName).publishWeb = pubToWeb.Checked;
                ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), publishingCamera.cameraName).publishFirst = true;
            }
        }

        private void pubToLocal_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().publishLocal = pubToLocal.Checked;
            if (CameraRig.ConnectedCameras.Count > 0)
            {
                var publishSelectedButton = GetPublishButtonGroup().Find(x => x.CameraButtonState == GroupCameraButton.ButtonState.ConnectedAndActive);

                if (publishSelectedButton == null)
                {
                    pubToLocal.Checked = false;
                    return;
                }

                var publishingCamera = CameraRig.ConnectedCameras.Find(x => x.displayButton == publishSelectedButton.id);
                ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), publishingCamera.cameraName).publishLocal = pubToLocal.Checked;
                ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), publishingCamera.cameraName).publishFirst = true;
            }
        }


        private void bttnSetPrefixPublish_Click(object sender, EventArgs e)
        {
            if (CameraRig.ConnectedCameras.Count > 0)
            {
                if (!GetPublishButtonGroup().Any(x => x.CameraButtonState == GroupCameraButton.ButtonState.ConnectedAndActive)) return;
                int pubButton = CameraRig.idxFromButton(GetPublishButtonGroup().First(x => x.CameraButtonState == GroupCameraButton.ButtonState.ConnectedAndActive).id);
                var record = ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), CameraRig.ConnectedCameras[pubButton].cameraName);

                FilePrefixSettingsDto settings = new FilePrefixSettingsDto()
                {
                    ResultDelegate = new FilePrefixformDelegate(filePrefixSet),
                    FromString = "Publish Web",
                    ToolTip = ConfigurationHelper.GetCurrentProfile().toolTips,
                    FileName = record.filenamePrefixPubWeb,
                    CycleStampChecked = record.cycleStampCheckedPubWeb,
                    StartCycle = record.startCyclePubWeb,
                    EndCycle = record.endCyclePubWeb,
                    CurrentCycle = record.currentCyclePubWeb,
                    IncludeStamp = record.stampAppendPubWeb,
                    FileDir = string.Empty,
                    DisplayStamp = true,
                    FileDirDefault = string.Empty,
                    ImagesSavedFolderEnabled = false,
                };

                fileprefix fileprefix = new fileprefix(settings);
                fileprefix.StartPosition = FormStartPosition.CenterScreen;
                fileprefix.ShowDialog();
            }
        }

        private void bttnSetLocalFileName_Click(object sender, EventArgs e)
        {
            if (!GetPublishButtonGroup().Any(x => x.CameraButtonState == GroupCameraButton.ButtonState.ConnectedAndActive)) return;
            if (CameraRig.ConnectedCameras.Count > 0)
            {
                int pubButton = CameraRig.idxFromButton(GetPublishButtonGroup().First(x => x.CameraButtonState == GroupCameraButton.ButtonState.ConnectedAndActive).id);
                var record = ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), CameraRig.ConnectedCameras[pubButton].cameraName);

                FilePrefixSettingsDto settings = new FilePrefixSettingsDto()
                {
                    ResultDelegate = new FilePrefixformDelegate(filePrefixSet),
                    FromString = "Publish Local",
                    ToolTip = ConfigurationHelper.GetCurrentProfile().toolTips,
                    FileName = record.filenamePrefixPubLoc,
                    CycleStampChecked = record.cycleStampCheckedPubLoc,
                    StartCycle = record.startCyclePubLoc,
                    EndCycle = record.endCyclePubLoc,
                    CurrentCycle = record.currentCyclePubLoc,
                    IncludeStamp = record.stampAppendPubLoc,
                    DisplayStamp = true,
                    FileDir = record.fileDirPubLoc,
                    FileDirDefault = TebocamState.imageFolder,
                    ImagesSavedFolderEnabled = true,
                };

                fileprefix fileprefix = new fileprefix(settings);
                fileprefix.StartPosition = FormStartPosition.CenterScreen;
                fileprefix.ShowDialog();
            }
        }

        private void pubTimerOn_CheckedChanged(object sender, EventArgs e)
        {

             if (!GetPublishButtonGroup().Any(x => x.CameraButtonState == GroupCameraButton.ButtonState.ConnectedAndActive)) 
            {
                pubTimerOn.Checked = false;
                return;
            }

            ConfigurationHelper.GetCurrentProfile().timerOn = pubTimerOn.Checked;

            if (CameraRig.ConnectedCameras.Count > 0)
            {
                int pubButton = GetPublishButtonGroup().First(x => x.CameraButtonState == GroupCameraButton.ButtonState.ConnectedAndActive).id;
               var buttonId = CameraRig.idxFromButton(pubButton);
                ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), CameraRig.ConnectedCameras[buttonId].cameraName).timerOn = pubTimerOn.Checked;
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

        private void button37_Click(object sender, EventArgs e)
        {
            ArrayList i = new ArrayList();
            i.Add("Publish");
            i.Add(ConfigurationHelper.GetCurrentProfile().toolTips);
            i.Add(ConfigurationHelper.GetCurrentProfile().timerStartPub);
            i.Add(ConfigurationHelper.GetCurrentProfile().timerEndPub);
            schedule schedule = new schedule(new formDelegate(scheduleSet), i);
            schedule.StartPosition = FormStartPosition.CenterScreen;
            schedule.ShowDialog();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string tmpUser = "";
            string tmpPass = "";
            string tmpRoot = "";
            tmpUser = ConfigurationHelper.GetCurrentProfile().ftpUser;
            tmpPass = ConfigurationHelper.GetCurrentProfile().ftpPass;
            tmpRoot = ConfigurationHelper.GetCurrentProfile().ftpRoot;
            pubFtpUser.Text = tmpUser;
            pubFtpPass.Text = tmpPass;
            pubFtpRoot.Text = tmpRoot;
            ConfigurationHelper.GetCurrentProfile().pubFtpUser = tmpUser;
            ConfigurationHelper.GetCurrentProfile().pubFtpPass = tmpPass;
            ConfigurationHelper.GetCurrentProfile().pubFtpRoot = tmpRoot;
        }

        private void pubFtpUser_TextChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().pubFtpUser = pubFtpUser.Text;
        }

        private void pubFtpPass_TextChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().pubFtpPass = pubFtpPass.Text;
        }

        private void pubFtpRoot_TextChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().pubFtpRoot = pubFtpRoot.Text;
        }

        private void groupBox17_Enter(object sender, EventArgs e)
        {

        }
    }
}
