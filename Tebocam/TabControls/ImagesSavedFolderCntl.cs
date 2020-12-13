using System;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class ImagesSavedFolderCntl : UserControl
    {
        public ImagesSavedFolderCntl()
        {
            InitializeComponent();
        }

        public RadioButton GetRadioButton11() { return radioButton11; }

        private void button21_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = ConfigurationHelper.GetCurrentProfile().imageParentFolderCust;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string parent = dialog.SelectedPath;

                if ((parent.Length < 7) || LeftRightMid.Right(parent, 7) != @"\images")
                {
                    ConfigurationHelper.GetCurrentProfile().imageParentFolderCust = parent + @"\images\";
                    ConfigurationHelper.GetCurrentProfile().imageFolderCust = ConfigurationHelper.GetCurrentProfile().imageParentFolderCust + @"fullSize\";
                    ConfigurationHelper.GetCurrentProfile().thumbFolderCust = ConfigurationHelper.GetCurrentProfile().imageParentFolderCust + @"thumb\";
                    TebocamState.imageParentFolder = ConfigurationHelper.GetCurrentProfile().imageParentFolderCust;
                    TebocamState.imageFolder = ConfigurationHelper.GetCurrentProfile().imageFolderCust;
                    TebocamState.thumbFolder = ConfigurationHelper.GetCurrentProfile().thumbFolderCust;
                    FileManager.CreateDirIfNotExists(ConfigurationHelper.GetCurrentProfile().imageParentFolderCust);
                    FileManager.CreateDirIfNotExists(ConfigurationHelper.GetCurrentProfile().imageFolderCust);
                    FileManager.CreateDirIfNotExists(ConfigurationHelper.GetCurrentProfile().thumbFolderCust);
                }
            }
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().imageLocCust = radioButton11.Checked;
            button21.Enabled = radioButton11.Checked;

            if (!radioButton11.Checked)
            {
                ConfigurationHelper.GetCurrentProfile().imageParentFolderCust = TebocamState.imageParentFolder = Application.StartupPath + @"\images\";
                ConfigurationHelper.GetCurrentProfile().imageFolderCust = TebocamState.imageParentFolder + @"fullSize\";
                ConfigurationHelper.GetCurrentProfile().thumbFolderCust = TebocamState.imageParentFolder + @"thumb\";
                TebocamState.imageParentFolder = ConfigurationHelper.GetCurrentProfile().imageParentFolderCust;
                TebocamState.imageFolder = ConfigurationHelper.GetCurrentProfile().imageFolderCust;
                TebocamState.thumbFolder = ConfigurationHelper.GetCurrentProfile().thumbFolderCust;
            }
        }
    }
}
