using System;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class UpdateOptionsCntl : UserControl
    {
        public delegate void UpdaterInstallDelegate(bool val);
        UpdaterInstallDelegate updaterInstall;
        public delegate void KeepWorkingDelegate(bool val);
        KeepWorkingDelegate keepWorking;
        public delegate void SetAPiInstanceToOffDelegate();
        SetAPiInstanceToOffDelegate setAPiInstanceToOff;

        public UpdateOptionsCntl(UpdaterInstallDelegate updaterInstall,
                                 KeepWorkingDelegate keepWorking,
                                 SetAPiInstanceToOffDelegate setAPiInstanceToOff)
        {
            InitializeComponent();
            this.updaterInstall = updaterInstall;
            this.keepWorking = keepWorking;
            this.setAPiInstanceToOff = setAPiInstanceToOff;
        }

        public Label GetLblCurVer() { return lblCurVer; }
        public Label GetLblVerAvail() { return lblVerAvail; }
        public CheckBox GetUpdateNotify() { return updateNotify; }
        public Button GetBttInstallUpdateAdmin() { return bttInstallUpdateAdmin; }
        public TextBox GetUpdateDebugLocationn() { return updateDebugLocation; }

        public void TriggerUpdate()
        {
            updaterInstall(true);
            keepWorking(false);
            setAPiInstanceToOff();
        }

        private void updateNotify_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().updatesNotify = updateNotify.Checked;
        }

        private void updateDebugLocation_TextChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().updateDebugLocation = updateDebugLocation.Text;
        }

        private void bttInstallUpdateAdmin_Click(object sender, EventArgs e)
        {
            TriggerUpdate();
        }
    }
}
