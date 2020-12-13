using System;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class FtpSettingsCntl : UserControl
    {
        private ILog log;

        public FtpSettingsCntl(ILog logger)
        {
            InitializeComponent();
            log = logger;
            SetFieldValues();
        }

        public void SetUser(string val) { ftpUser.Text = val; }
        public void SetPassword(string val) { ftpPass.Text = val; }
        public void SetRoot(string val) { ftpRoot.Text = val; }

        private void SetFieldValues()
        {
            ftpUser.Text = ConfigurationHelper.GetCurrentProfile().ftpUser;
            ftpPass.Text = ConfigurationHelper.GetCurrentProfile().ftpPass;
            ftpRoot.Text = ConfigurationHelper.GetCurrentProfile().ftpRoot;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            ftp.testFtp = true;
            ftp.testFtpError = false;
            FileManager.WriteFile("test");
            log.AddLine("ftp test: uploading file");
            ftp.Upload(TebocamState.xmlFolder + FileManager.testFile + ".xml", ConfigurationHelper.GetCurrentProfile().ftpRoot, ConfigurationHelper.GetCurrentProfile().ftpUser, ConfigurationHelper.GetCurrentProfile().ftpPass, 0);

            if (!ftp.testFtpError)
            {
                TebocamState.log.AddLine("ftp test: deleting file");
                ftp.DeleteFTP(FileManager.testFile + ".xml", ConfigurationHelper.GetCurrentProfile().ftpRoot, ConfigurationHelper.GetCurrentProfile().ftpUser, ConfigurationHelper.GetCurrentProfile().ftpPass, false);
                if (ftp.testFtpError)
                {
                    log.AddLine("Error with test ftp: deleting file");
                    MessageDialog.messageInform("Error with test ftp: deleting file", "Error");
                }
            }
            else
            {
                log.AddLine("Error with test ftp: uploading file");
                MessageDialog.messageInform("Error with test ftp: uploading file", "Error");
            }

            if (!ftp.testFtpError)
            {
                MessageDialog.messageInform("Ftp test was successful!", "Success");
            }

            ftp.testFtp = false;
            ftp.testFtpError = false;
        }

        private void ftpUser_TextChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().ftpUser = ftpUser.Text;
        }

        private void ftpPass_TextChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().ftpPass = ftpPass.Text;
        }

        private void ftpRoot_TextChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().ftpRoot = ftpRoot.Text;
        }
    }
}
