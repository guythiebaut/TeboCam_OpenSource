using System;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class FrameRateCntl : UserControl
    {
        public FrameRateCntl()
        {
            InitializeComponent();
        }

        public NumericUpDown GetNumFrameRateCalcOver() { return numFrameRateCalcOver; }
        public CheckBox GetChkFrameRateTrack() { return chkFrameRateTrack; }

        private void chkFrameRateTrack_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().framerateTrack = chkFrameRateTrack.Checked;
            CameraRig.ConnectedCameras.ForEach(x => x.camera.frameRateTrack = chkFrameRateTrack.Checked);
        }

        private void numFrameRateCalcOver_Leave(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().framesSecsToCalcOver = (int)numFrameRateCalcOver.Value;
        }

    }
}
