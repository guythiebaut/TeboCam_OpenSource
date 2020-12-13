using System;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class AlertTimeSettingsCntl : UserControl
    {
        public AlertTimeSettingsCntl()
        {
            InitializeComponent();
            SetFieldValues();
        }

        public TextBox GetEmailNotifInterval() { return emailNotifInterval; }
        public TextBox GetImageFileInterval() { return imageFileInterval; }


        private void SetFieldValues()
        {
            emailNotifInterval.Text = ConfigurationHelper.GetCurrentProfile().emailNotifyInterval.ToString();
            imageFileInterval.Text = ConfigurationHelper.GetCurrentProfile().imageSaveInterval.ToString();
        }

        private void emailNotifInterval_Leave(object sender, EventArgs e)
        {
            emailNotifInterval.Text = Valid.verifyInt(emailNotifInterval.Text, 1, 9999, "1");
            ConfigurationHelper.GetCurrentProfile().emailNotifyInterval = Convert.ToInt64(emailNotifInterval.Text);
        }

        private void imageFileInterval_Leave(object sender, EventArgs e)
        {
            imageFileInterval.Text = Valid.verifyDouble(imageFileInterval.Text, 0.1, 9999, "1");
            ConfigurationHelper.GetCurrentProfile().imageSaveInterval = Convert.ToDouble(imageFileInterval.Text);
        }
    }
}
