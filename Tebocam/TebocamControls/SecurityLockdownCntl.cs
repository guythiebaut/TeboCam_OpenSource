using System;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class SecurityLockdownCntl : UserControl
    {
        public delegate void LockTebocamDelegate();
        LockTebocamDelegate LockTebocam;
        public delegate void LockDownDelegate(bool hide);
        LockDownDelegate LockDown;

        public SecurityLockdownCntl(LockTebocamDelegate lockTebocam, LockDownDelegate lockDown)
        {
            InitializeComponent();
            LockTebocam = lockTebocam;
            LockDown = lockDown;
        }

        public TextBox GetTxtLockdownPassword() { return txtLockdownPassword; }
        public RadioButton GetRdLockdownOn() { return rdLockdownOn; }
        public Button GetBtnSecurityLockdownOn() { return btnSecurityLockdownOn; }

        private void btnSecurityLockdownOn_Click(object sender, EventArgs e)
        {
            LockTebocam();
        }

        private void rdLockdownOff_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().lockdownOn = !rdLockdownOff.Checked;
            LockDown(!rdLockdownOff.Checked);
            btnSecurityLockdownOn.Enabled = !rdLockdownOff.Checked;
        }

        private void txtLockdownPassword_Leave(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().lockdownPassword = txtLockdownPassword.Text;
        }
    }
}
