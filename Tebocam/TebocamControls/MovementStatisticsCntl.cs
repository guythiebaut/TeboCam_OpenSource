using System;
using System.IO;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class MovementStatisticsCntl : UserControl
    {
        public MovementStatisticsCntl()
        {
            InitializeComponent();
        }

        public void SetRdStatsToFileOn(bool val) { rdStatsToFileOn.Checked = val; }
        public void SetPnlStatsToFile(bool val) { pnlStatsToFile.Enabled = val; }
        public void SetChkStatsToFileTimeStamp(bool val) { chkStatsToFileTimeStamp.Checked = val; }
        public void SetTxtStatsToFileMb(double val) { txtStatsToFileMb.Text = val.ToString(); }

        private void rdStatsToFileOn_CheckedChanged(object sender, EventArgs e)
        {
            pnlStatsToFile.Enabled = rdStatsToFileOn.Checked;
            ConfigurationHelper.GetCurrentProfile().StatsToFileOn = rdStatsToFileOn.Checked;

            if (ConfigurationHelper.GetCurrentProfile().StatsToFileLocation == string.Empty)
            {
                ConfigurationHelper.GetCurrentProfile().StatsToFileLocation = Application.StartupPath + "\\" + "MovementStats.txt";
            }
        }

        private void btnStatsToFileLocation_Click(object sender, EventArgs e)
        {
            SaveFileDialog statsDialog = new SaveFileDialog();

            if (ConfigurationHelper.GetCurrentProfile().StatsToFileLocation != string.Empty)
            {
                statsDialog.InitialDirectory = Path.GetDirectoryName(ConfigurationHelper.GetCurrentProfile().StatsToFileLocation);
                statsDialog.FileName = Path.GetFileName(ConfigurationHelper.GetCurrentProfile().StatsToFileLocation);
            }
            else
            {
                statsDialog.InitialDirectory = Application.StartupPath;
                statsDialog.FileName = "MovementStats";
            }

            statsDialog.Title = "Save Statistics";
            statsDialog.DefaultExt = "txt";
            statsDialog.AddExtension = true;
            statsDialog.Filter = "text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (statsDialog.ShowDialog() == DialogResult.OK)
            {
                ConfigurationHelper.GetCurrentProfile().StatsToFileLocation = statsDialog.FileName;
                statistics.fileName = string.Empty;
            }
        }

        private void txtStatsToFileMb_Leave(object sender, EventArgs e)
        {
            txtStatsToFileMb.Text = Valid.verifyDouble(txtStatsToFileMb.Text, .01, double.MaxValue, "0.01");
            ConfigurationHelper.GetCurrentProfile().StatsToFileMb = Convert.ToDouble(txtStatsToFileMb.Text);
        }

        private void chkStatsToFileTimeStamp_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().StatsToFileTimeStamp = chkStatsToFileTimeStamp.Checked;
        }
    }
}
