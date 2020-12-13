using System;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class AlertFilenameCntl : UserControl
    {
        public delegate void FilePrefixSetDelegate(FilePrefixSettingsResultDto result);
        FilePrefixSetDelegate filePrefixSet;

        public AlertFilenameCntl(FilePrefixSetDelegate filePrefix)
        {
            InitializeComponent();
            filePrefixSet = filePrefix;
        }

        private void button35_Click(object sender, EventArgs e)
        {
            var record = ConfigurationHelper.GetCurrentProfile();

            FilePrefixSettingsDto settings = new FilePrefixSettingsDto()
            {
                ResultDelegate = new FilePrefixformDelegate(filePrefixSet),
                FromString = "Alert",
                ToolTip = record.toolTips,
                FileName = record.filenamePrefix,
                CycleStampChecked = record.cycleStampChecked,
                StartCycle = (int)record.startCycle,
                EndCycle = (int)record.endCycle,
                CurrentCycle = (int)record.currentCycle,
                IncludeStamp = true,
                DisplayStamp = false,
                FileDir = string.Empty,
                FileDirDefault = string.Empty,
                ImagesSavedFolderEnabled = false
            };

            fileprefix fileprefix = new fileprefix(settings);
            fileprefix.StartPosition = FormStartPosition.CenterScreen;
            fileprefix.ShowDialog();
        }
    }
}
