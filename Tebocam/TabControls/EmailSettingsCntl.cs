using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class EmailSettingsCntl : UserControl
    {
        private IMail mail;

        public EmailSettingsCntl(IMail email)
        {
            InitializeComponent();
            mail = email;
            SetFieldValues();
        }

        public void SetPingSubject(string val) { pingSubject.Text = val; }
        public void SetSentByName(string val) { sentByName.Text = val; }
        public void SetMailBody(string val) { mailBody.Text = val; }
        public void SetMailSubject(string val) { mailSubject.Text = val; }
        public void SetReplyTo(string val) { replyTo.Text = val; }
        public void SetSendTo(string val) { sendTo.Text = val; }
        public void SetSentBy(string val) { sentBy.Text = val; }

        private void SetFieldValues()
        {
            pingSubject.Text = ConfigurationHelper.GetCurrentProfile().pingSubject;
            sentByName.Text = ConfigurationHelper.GetCurrentProfile().sentByName;
            mailBody.Text = ConfigurationHelper.GetCurrentProfile().mailBody;
            mailSubject.Text = ConfigurationHelper.GetCurrentProfile().mailSubject;
            replyTo.Text = ConfigurationHelper.GetCurrentProfile().replyTo;
            sendTo.Text = ConfigurationHelper.GetCurrentProfile().sendTo;
            sentBy.Text = ConfigurationHelper.GetCurrentProfile().sentBy;
        }

        private void sendTo_Leave(object sender, EventArgs e)
        {
            string[] emails = sendTo.Text.Split(';');
            var nonValidEmailAddressPresent = emails.Any(x => !mail.validEmail(x));

            if (nonValidEmailAddressPresent)
            {
                MessageDialog.messageAlert("'Send Email To' contains valid email address", "Invalid Email");
                sendTo.BackColor = Color.Red;
            }
            else
            {
                sendTo.BackColor = Color.LemonChiffon;
            }
        }

        private void sendTo_TextChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().sendTo = sendTo.Text;
        }

        private void sentBy_Leave(object sender, EventArgs e)
        {
            if (!mail.validEmail(sentBy.Text))
            {
                MessageDialog.messageAlert("'Sent By' is not a valid email address", "Invalid Email");
                sentBy.BackColor = Color.Red;
            }
            else
            {
                sentBy.BackColor = Color.LemonChiffon;
            }
        }

        private void sentBy_TextChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().sentBy = sentBy.Text;
        }

        private void replyTo_Leave(object sender, EventArgs e)
        {
            if (!mail.validEmail(replyTo.Text))
            {
                MessageDialog.messageAlert("'Reply To' is not a valid email address", "Invalid Email");
                replyTo.BackColor = Color.Red;
            }
            else
            {
                replyTo.BackColor = Color.LemonChiffon;
            }
        }

        private void replyTo_TextChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().replyTo = replyTo.Text;
        }

        private void mailSubject_TextChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().mailSubject = mailSubject.Text;
        }

        private void mailBody_TextChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().mailBody = mailBody.Text;
        }

        private void pingSubject_TextChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().pingSubject = pingSubject.Text;
        }

        private void sentByName_TextChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().sentByName = sentByName.Text;
        }
    }
}
