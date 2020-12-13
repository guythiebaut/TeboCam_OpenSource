using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class LogFileManagementCntl : UserControl
    {
        IFileInfo FileInfo;

        public LogFileManagementCntl(IFileInfo fileInfo)
        {
            InitializeComponent();
            FileInfo = fileInfo; 
        }

        private void btnAdvancedFileManagement_Click(object sender, EventArgs e)
        {
            Form frm = new Form();
            frm.Width = 414;
            frm.Height = 430;
            frm.FormBorderStyle = FormBorderStyle.FixedSingle;
            FileInfo.AddAggregates();
            var fileAdminCntl = new FileAdminCntl(FileInfo, frm);

            fileAdminCntl.GetLogsKeep().Text = ConfigurationHelper.GetCurrentProfile().logsKeep.ToString();
            fileAdminCntl.GetLogsKeepChk().Checked = ConfigurationHelper.GetCurrentProfile().logsKeepChk;
            frm.Controls.Add(fileAdminCntl);
            frm.ShowDialog();
        }
    }
}
