using System;
using System.Drawing;
using TeboCam.TebocamControls;

namespace TeboCam
{
    public partial class EmailHostSettingsCntl : TebocamCntl
    {
        IMail mail;
        public Size PanelSize;

        public EmailHostSettingsCntl(IMail imail)
        {
            InitializeComponent();
            mail = imail;
            SetFieldValues();
            SetExtraControls();
        }

        public void SetUser(string val) { emailUser.Text = val; }
        public void SetHost(string val) { smtpHost.Text = val; }
        public void SetPort(string val) { smtpPort.Text = val; }
        public void SetSsl(bool val) { SSL.Checked = val; }

        private void SetExtraControls()
        {
            //var emailPasswordCntl = new PasswordCntl("Email Password", data.emailPass, delegate (Object sender, PasswordChangedArgs args) { ConfigurationHelper.GetCurrentProfile().emailPass = args.password; }, 294);
            //AddControl(emailPasswordCntl, emailHostSettings.Controls, new Point(16,120));
            emailPasswordCntl.SetDesiredWidth(294);
            emailPasswordCntl.SetTitle("Email Password");
            emailPasswordCntl.SetPaswordFieldColour(Color.LemonChiffon);
            emailPasswordCntl.SetValue(ConfigurationHelper.GetCurrentProfile().emailPass);
            emailPasswordCntl.SetPasswordChanged(delegate (Object sender, PasswordChangedArgs args) { ConfigurationHelper.GetCurrentProfile().emailPass = args.password; });
        }

        public override void AfterControlAdded()
        {
            emailPasswordCntl.AfterControlAdded();
        }

        private void SetFieldValues()
        {
            emailUser.Text = ConfigurationHelper.GetCurrentProfile().emailUser;
            smtpHost.Text = ConfigurationHelper.GetCurrentProfile().smtpHost;
            smtpPort.Text = ConfigurationHelper.GetCurrentProfile().smtpPort.ToString();
            SSL.Checked = ConfigurationHelper.GetCurrentProfile().EnableSsl;
        }

        private void TestBtn_Click(object sender, EventArgs e)
        {
            mail.SetTestStatus(9);

            var eml = new EmailFields()
            {
                SentBy = ConfigurationHelper.GetCurrentProfile().sentBy,
                SentByName = ConfigurationHelper.GetCurrentProfile().sentByName,
                SendTo = ConfigurationHelper.GetCurrentProfile().sendTo,
                Subject = "TeboCam Test",
                BodyText = "This is a test email from TeboCam",
                ReplyTo = ConfigurationHelper.GetCurrentProfile().replyTo,
                Attachments = false,
                CurrentTime = time.secondsSinceStart(),
                User = ConfigurationHelper.GetCurrentProfile().emailUser,
                Password = ConfigurationHelper.GetCurrentProfile().emailPass,
                SmtpHost = ConfigurationHelper.GetCurrentProfile().smtpHost,
                SmtpPort = ConfigurationHelper.GetCurrentProfile().smtpPort,
                EnableSsl = ConfigurationHelper.GetCurrentProfile().EnableSsl
            };

            mail.sendEmail(eml);

            //huge code smell!!!
            while (mail.GetTestStatus() == 9) { }
            //huge code smell!!!

            if (mail.GetTestStatus() == 1)
            {
                MessageDialog.messageInform("It looks like the email test was successful", "Check your email");
            }
            else
            {
                MessageDialog.messageAlert("It looks like the email test was unsuccessful", "Check your email settings");
            }
            mail.SetTestStatus(0);
        }

        private void emailUser_TextChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().emailUser = emailUser.Text;
        }

        private void smtpHost_TextChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().smtpHost = smtpHost.Text;
        }

        private void smtpPort_Leave(object sender, EventArgs e)
        {
            smtpPort.Text = Valid.verifyInt(smtpPort.Text, 0, 99999, "25");
            ConfigurationHelper.GetCurrentProfile().smtpPort = Convert.ToInt32(smtpPort.Text);
        }

        private void SSL_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().EnableSsl = SSL.Checked;
        }

        private void emailUser_Leave(object sender, EventArgs e)
        {
            if (!mail.validEmail(emailUser.Text))
            {
                MessageDialog.messageAlert("'Email User' is not a valid email address", "Invalid Email");
                emailUser.BackColor = Color.Red;
            }
            else
            {
                emailUser.BackColor = Color.LemonChiffon;
            }
        }
    }
}
