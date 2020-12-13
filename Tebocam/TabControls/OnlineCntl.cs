using System;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class OnlineCntl : UserControl
    {
        public OnlineCntl()
        {
            InitializeComponent();
        }

        private void webUpd_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().webUpd = webUpd.Checked;
            ApiProcess.webUpdLastChecked = 0;
            ApiProcess.webFirstTimeThru = true;
        }

        public TextBox GetTxtPickupDirectory() { return txtPickupDirectory; }
        public RadioButton GetRdApiRemote() { return rdApiRemote; }
        public RadioButton GetRdApiLocal() { return rdApiLocal; }
        public TextBox GetSqlInstance() { return sqlInstance; }
        public TextBox GetSqlImageRoot() { return sqlImageRoot; }
        public TextBox GetSqlImageFilename() { return sqlImageFilename; }
        public TextBox GetSqlFtpUser() { return SqlFtpUser; }
        public TextBox GetSqlFtpPwd() { return SqlFtpPwd; }
        public CheckBox GetDisCommOnline() { return disCommOnline; }
        public TextBox GetDisCommOnlineSecs() { return disCommOnlineSecs; }
        public CheckBox GetWebUpd() { return webUpd; }
        public TextBox GetSqlUser() { return sqlUser; }
        public TextBox GetSqlPwd() { return sqlPwd; }
        public TextBox GetSqlPoll() { return sqlPoll; }
        public TextBox GetTxtEndpoint() { return txtEndpoint; }
        public TextBox GetTxtEndpointLocal() { return txtEndpointLocal; }

        private void sqlUser_Leave(object sender, EventArgs e)
        {
            if (sqlUser.Text != ConfigurationHelper.GetCurrentProfile().webUser)
            {
                TebocamState.log.AddLine("API credentials user changed.");
                ConfigurationHelper.GetCurrentProfile().webUser = sqlUser.Text;
                ApiProcess.ApiAuthenticationAttemptCount = 0;
                ApiProcess.apiCredentialsValidated = false;
                ApiProcess.webUpdLastChecked = 0;
                ApiProcess.webFirstTimeThru = true;
            }
        }

        private void sqlPwd_Leave(object sender, EventArgs e)
        {
            if (sqlPwd.Text != ConfigurationHelper.GetCurrentProfile().webPass)
            {
                TebocamState.log.AddLine("API credentials password changed.");
                ConfigurationHelper.GetCurrentProfile().webPass = sqlPwd.Text;
                ApiProcess.ApiAuthenticationAttemptCount = 0;
                ApiProcess.apiCredentialsValidated = false;
                ApiProcess.webUpdLastChecked = 0;
                ApiProcess.webFirstTimeThru = true;

            }
        }

        private void sqlInstance_Leave(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().webInstance = sqlInstance.Text;
        }

        private void sqlPoll_Leave(object sender, EventArgs e)
        {
            int tmpInt = ConfigurationHelper.GetCurrentProfile().webPoll;
            sqlPoll.Text = Valid.verifyInt(sqlPoll.Text, 30, 9999, "30");

            if (Convert.ToInt32(sqlPoll.Text) != tmpInt)
            {
                ConfigurationHelper.GetCurrentProfile().webPoll = Convert.ToInt32(sqlPoll.Text);
                ApiProcess.webUpdLastChecked = 0;
                ApiProcess.webFirstTimeThru = true;
            }
        }

        private void txtPickupDirectory_Leave(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().PickupDirectory = txtPickupDirectory.Text.Trim();
        }

        private void txtEndpoint_Leave(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().AuthenticateEndpoint = txtEndpoint.Text.Trim();
        }

        private void txtEndpointLocal_Leave(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().LocalAuthenticateEndpoint = txtEndpointLocal.Text.Trim();
        }

        private void rdApiRemote_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().UseRemoteEndpoint = rdApiRemote.Checked;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            string tmpUser = "";
            string tmpPass = "";
            string tmpRoot = "";
            tmpUser = ConfigurationHelper.GetCurrentProfile().ftpUser;
            tmpPass = ConfigurationHelper.GetCurrentProfile().ftpPass;
            tmpRoot = ConfigurationHelper.GetCurrentProfile().ftpRoot;
            SqlFtpUser.Text = tmpUser;
            SqlFtpPwd.Text = tmpPass;
            sqlImageRoot.Text = tmpRoot;
            ConfigurationHelper.GetCurrentProfile().webFtpUser = tmpUser;
            ConfigurationHelper.GetCurrentProfile().webFtpPass = tmpPass;
            ConfigurationHelper.GetCurrentProfile().webImageRoot = tmpRoot;
        }

        private void SqlFtpUser_Leave(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().webFtpUser = SqlFtpUser.Text;
            ApiProcess.webFirstTimeThru = true;
        }

        private void SqlFtpPwd_Leave(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().webFtpPass = SqlFtpPwd.Text;
            ApiProcess.webFirstTimeThru = true;
        }

        private void sqlImageRoot_Leave(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().webImageRoot = sqlImageRoot.Text;
            ApiProcess.webFirstTimeThru = true;
        }

        private void sqlImageFilename_Leave(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().webImageFileName = sqlImageFilename.Text;
            ApiProcess.webFirstTimeThru = true;
        }

        private void disCommOnline_CheckedChanged(object sender, EventArgs e)
        {
            disCommOnlineSecs.Enabled = disCommOnline.Checked;
            ConfigurationHelper.GetCurrentProfile().disCommOnline = disCommOnline.Checked;
        }

        private void disCommOnlineSecs_Leave(object sender, EventArgs e)
        {
            disCommOnlineSecs.Text = Valid.verifyInt(disCommOnlineSecs.Text, 1, 86400, "1");
            ConfigurationHelper.GetCurrentProfile().disCommOnlineSecs = Convert.ToInt32(disCommOnlineSecs.Text);
        }
    }
}
