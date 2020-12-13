using System;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class SecurityLockdownCntl : UserControl
    {
        public delegate void MinimiseTebocamDelegate(bool hide);
        MinimiseTebocamDelegate MinimiseTebocam;
        public delegate void ShowTebocamDelegate();
        ShowTebocamDelegate ShowTebocam;
        public delegate void LockDownDelegate(bool hide);
        LockDownDelegate LockDown;

        public SecurityLockdownCntl(MinimiseTebocamDelegate minimise, ShowTebocamDelegate show, LockDownDelegate lockDown)
        {
            InitializeComponent();
            MinimiseTebocam = minimise;
            ShowTebocam = show;
            LockDown = lockDown;
        }

        public TextBox GetTxtLockdownPassword() { return txtLockdownPassword; }
        public RadioButton GetRdLockdownOn() { return rdLockdownOn; }
        public Button GetBtnSecurityLockdownOn() { return btnSecurityLockdownOn; }

        private void btnSecurityLockdownOn_Click(object sender, EventArgs e)
        {
            MinimiseTebocam(false);
            this.Enabled = false;

            while (true)
            {

                if (Prompt.ShowDialog("Password", "Enter password to unlock") == ConfigurationHelper.GetCurrentProfile().lockdownPassword)
                {
                    this.Enabled = true;
                    ShowTebocam();
                    break;
                }
            }
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
