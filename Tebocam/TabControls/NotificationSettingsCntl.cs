using System;
using System.Windows.Forms;
using System.IO;

namespace TeboCam
{
    public partial class NotificationSettingsCntl : UserControl
    {
        Ping pinger;
        Configuration configuration;
        AlertTimeSettingsCntl alertTimeSettings;
        public delegate void graphDateDelegate(string graphDate, string displayText);
        graphDateDelegate graphDate;
        public delegate void drawGraphDateDelegate();
        drawGraphDateDelegate drawGraph;

        public NotificationSettingsCntl(Ping ping, Configuration config, AlertTimeSettingsCntl alertSettings, graphDateDelegate graph, drawGraphDateDelegate drawGraphDate)
        {
            InitializeComponent();
            pinger = ping;
            configuration = config;
            alertTimeSettings = alertSettings;
            graphDate = graph;
            drawGraph = drawGraphDate;
        }

        public void SetCaptureMovementImages(bool val) { captureMovementImages.Checked = val; }
        public void SetSendEmail(bool val) { sendEmail.Checked = val; }
        public void SetSendFullSize(bool val) { sendFullSize.Checked = val; }
        public void SetSendThumb(bool val) { sendThumb.Checked = val; }
        public void SetSendMosaic(bool val) { sendMosaic.Checked = val; }
        public void SetMosaicImagesPerRow(int val) { mosaicImagesPerRow.Text = val.ToString(); }
        public void SetLoadToFtp(bool val) { loadToFtp.Checked = val; }
        public void SetPlSnd(bool val) { plSnd.Checked = val; }
        public void SetMaxImagesToEmail(long val) { maxImagesToEmail.Text = val.ToString(); }
        public void SetPing(bool val) { ping.Checked = val; }
        public void SetPingMins(int val) { pingMins.Text = val.ToString(); }
        public void SetRdPingAllCameras(bool val) { rdPingAllCameras.Checked = val; }


        private void sndTest_Click(object sender, EventArgs e)
        {
            string soundFile = ConfigurationHelper.GetCurrentProfile().soundAlert;

            if (soundFile != "")
            {
                Sound.RingMyBell(true);
            }

        }

        private void SelectSoundCntl_Click(object sender, EventArgs e)
        {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "wav files (*.wav)|*.wav|wav files (*.*)|*.*";

            string soundFile = ConfigurationHelper.GetCurrentProfile().soundAlert;

            if (soundFile != "")
            {
                dialog.InitialDirectory = Path.GetDirectoryName(soundFile);
                dialog.FileName = Path.GetFileName(soundFile);
            }

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ConfigurationHelper.GetCurrentProfile().soundAlert = dialog.FileName;
                configuration.WriteXmlFile(TebocamState.xmlFolder + FileManager.configFile + ".xml", configuration);
                TebocamState.log.AddLine("Config data saved.");
            }

            plSnd.Enabled = ConfigurationHelper.GetCurrentProfile().soundAlert != "";
        }

        private void captureMovementImages_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().captureMovementImages = captureMovementImages.Checked;

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
                drawGraph();
            }
        }

        private void sendEmail_CheckedChanged(object sender, EventArgs e)
        {
            alertTimeSettings.GetEmailNotifInterval().Enabled = sendEmail.Checked;
            ConfigurationHelper.GetCurrentProfile().sendNotifyEmail = sendEmail.Checked;

            if (!sendEmail.Checked)
            {
                sendThumb.Checked = false;
                ConfigurationHelper.GetCurrentProfile().sendThumbnailImages = false;
                sendFullSize.Checked = false;
                ConfigurationHelper.GetCurrentProfile().sendFullSizeImages = false;
                sendMosaic.Checked = false;
                ConfigurationHelper.GetCurrentProfile().sendMosaicImages = false;
            }

        }

        private void sendFullSize_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().sendFullSizeImages = sendFullSize.Checked;
            if (sendFullSize.Checked)
            {
                sendThumb.Checked = false;
                ConfigurationHelper.GetCurrentProfile().sendThumbnailImages = false;
                sendMosaic.Checked = false;
                ConfigurationHelper.GetCurrentProfile().sendMosaicImages = false;
            }
            alertTimeSettings.GetImageFileInterval().Enabled= sendFullSize.Checked || sendThumb.Checked || sendMosaic.Checked || loadToFtp.Checked;
            sendEmail.Checked = ConfigurationHelper.GetCurrentProfile().sendThumbnailImages || ConfigurationHelper.GetCurrentProfile().sendFullSizeImages || ConfigurationHelper.GetCurrentProfile().sendMosaicImages;
            ConfigurationHelper.GetCurrentProfile().sendNotifyEmail = sendEmail.Checked;

        }

        private void sendThumb_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().sendThumbnailImages = sendThumb.Checked;
            if (sendThumb.Checked)
            {
                sendFullSize.Checked = false;
                ConfigurationHelper.GetCurrentProfile().sendFullSizeImages = false;
                sendMosaic.Checked = false;
                ConfigurationHelper.GetCurrentProfile().sendMosaicImages = false;
            }
            alertTimeSettings.GetEmailNotifInterval().Enabled = sendFullSize.Checked || sendThumb.Checked || sendMosaic.Checked || loadToFtp.Checked;
            sendEmail.Checked = ConfigurationHelper.GetCurrentProfile().sendThumbnailImages || ConfigurationHelper.GetCurrentProfile().sendFullSizeImages || ConfigurationHelper.GetCurrentProfile().sendMosaicImages;
            ConfigurationHelper.GetCurrentProfile().sendNotifyEmail = sendEmail.Checked;

        }

        private void sendMosaic_CheckedChanged(object sender, EventArgs e)
        {

            ConfigurationHelper.GetCurrentProfile().sendMosaicImages = sendMosaic.Checked;
            if (sendMosaic.Checked)
            {
                sendFullSize.Checked = false;
                ConfigurationHelper.GetCurrentProfile().sendFullSizeImages = false;
                sendThumb.Checked = false;
                ConfigurationHelper.GetCurrentProfile().sendThumbnailImages = false;
            }
            alertTimeSettings.GetImageFileInterval().Enabled = sendFullSize.Checked || sendThumb.Checked || sendMosaic.Checked || loadToFtp.Checked;
            sendEmail.Checked = ConfigurationHelper.GetCurrentProfile().sendThumbnailImages || ConfigurationHelper.GetCurrentProfile().sendFullSizeImages || ConfigurationHelper.GetCurrentProfile().sendMosaicImages;
            ConfigurationHelper.GetCurrentProfile().sendNotifyEmail = sendEmail.Checked;
            mosaicImagesPerRow.Enabled = sendMosaic.Checked;

        }

        private void mosaicImagesPerRow_Leave(object sender, EventArgs e)
        {
            mosaicImagesPerRow.Text = Valid.verifyInt(mosaicImagesPerRow.Text, 1, 100, "1");
            ConfigurationHelper.GetCurrentProfile().mosaicImagesPerRow = Convert.ToInt32(mosaicImagesPerRow.Text);
        }

        private void loadToFtp_CheckedChanged(object sender, EventArgs e)
        {
            alertTimeSettings.GetImageFileInterval().Enabled = sendFullSize.Checked || sendThumb.Checked || loadToFtp.Checked;
            ConfigurationHelper.GetCurrentProfile().loadImagesToFtp = loadToFtp.Checked;
        }

        private void plSnd_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().soundAlertOn = plSnd.Checked;
            //FileManager.WriteFile("config");
            //config.WebcamSettingsConfigDataPopulate();
            configuration.WriteXmlFile(TebocamState.xmlFolder + FileManager.configFile + ".xml", configuration);
            TebocamState.log.AddLine("Config data saved.");
        }

        private void maxImagesToEmail_Leave(object sender, EventArgs e)
        {
            maxImagesToEmail.Text = Valid.verifyInt(maxImagesToEmail.Text, 1, 9999, "1");
            ConfigurationHelper.GetCurrentProfile().maxImagesToEmail = Convert.ToInt64(maxImagesToEmail.Text);
        }

        private void ping_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().ping = ping.Checked;
            if (pinger != null)
            {
                pinger.pingedBefore = false;
            }
        }

        private void pingMins_Leave(object sender, EventArgs e)
        {
            pingMins.Text = Valid.verifyInt(pingMins.Text, 1, 9999, "1");
            ConfigurationHelper.GetCurrentProfile().pingInterval = Convert.ToInt32(pingMins.Text);
        }

        private void rdPingAllCameras_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().pingAll = rdPingAllCameras.Checked;
        }

    }
}
