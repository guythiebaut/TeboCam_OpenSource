using System;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class EmailIntelligenceCntl : UserControl
    {
        public EmailIntelligenceCntl()
        {
            InitializeComponent();
        }

        public void SetEmailIntelOn(bool val) { EmailIntelOn.Checked = val; }
        public void SetEmailIntelEmails(int val) { emailIntelEmails.Text = val.ToString(); }
        public void SetEmailIntelMins(int val) { emailIntelMins.Text = val.ToString(); }
        public void SetEmailIntelStop(bool val) { EmailIntelStop.Checked = val; }
        public void SetEmailIntelMosaic(bool val) { EmailIntelMosaic.Checked = val; }
        public void Set(bool val) { EmailIntelMosaic.Checked = val; }

        private void EmailIntelOn_CheckedChanged(object sender, EventArgs e)
        {
            emailIntelPanel.Enabled = EmailIntelOn.Checked;
            ConfigurationHelper.GetCurrentProfile().EmailIntelOn = EmailIntelOn.Checked;
        }

        private void emailIntelEmails_Leave(object sender, EventArgs e)
        {
            emailIntelEmails.Text = Valid.verifyInt(emailIntelEmails.Text, 1, 9999, "1");
            ConfigurationHelper.GetCurrentProfile().emailIntelEmails = Convert.ToInt32(emailIntelEmails.Text);
        }

        private void emailIntelMins_Leave(object sender, EventArgs e)
        {
            emailIntelMins.Text = Valid.verifyInt(emailIntelMins.Text, 1, 9999, "1");
            ConfigurationHelper.GetCurrentProfile().emailIntelMins = Convert.ToInt32(emailIntelMins.Text);
        }

        private void EmailIntelStop_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().EmailIntelStop = EmailIntelStop.Checked;
        }
    }
}
