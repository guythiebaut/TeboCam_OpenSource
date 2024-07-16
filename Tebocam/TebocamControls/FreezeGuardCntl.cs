using System;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class FreezeGuardCntl : UserControl
    {
        public delegate void pulseStopEventDelegate();
        pulseStopEventDelegate pulseStopEvent;

        public FreezeGuardCntl(pulseStopEventDelegate pulseStop)
        {
            InitializeComponent();
            pulseStopEvent = pulseStop;
        }

        public void SetFreezeGuardOn(bool val) { freezeGuardOn.Checked = val; }
        public void SetFreezeGuardWindow(bool val) { freezeGuardWindow.Checked = val; }
        public void SetPulseFreq(string val) { pulseFreq.Text = val; }

        private void button15_Click(object sender, EventArgs e)
        {
            if (button15.Text == "Test FreezeGuard")
            {

                pulseStopEvent();
                button15.Text = "Pulse Stopped";

            }
            else
            {

                pulseStopEvent();
                button15.Text = "Test FreezeGuard";

            }
        }

        private void freezeGuardOn_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().freezeGuard = freezeGuardOn.Checked;
        }

        private void pulseFreq_Leave(object sender, EventArgs e)
        {
            decimal result = 1m;

            if (!Valid.IsDecimal(pulseFreq.Text))
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

            ConfigurationHelper.GetCurrentProfile().pulseFreq = result;

        }

        private void freezeGuardWindow_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().freezeGuardWindowShow = freezeGuardWindow.Checked;
        }
    }
}
