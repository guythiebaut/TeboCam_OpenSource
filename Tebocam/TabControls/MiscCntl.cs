using System;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class MiscCntl : UserControl
    {
        public MiscCntl()
        {
            InitializeComponent();
        }

        public CheckBox GetChkHideWhenMinimised() { return chkHideWhenMinimised; }

        private void chkHideWhenMinimised_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().hideWhenMinimized = chkHideWhenMinimised.Checked;
        }

        private void infoMode_CheckedChanged(object sender, EventArgs e)
        {
            teboDebug.debugOn = infoMode.Checked;
        }
    }
}
