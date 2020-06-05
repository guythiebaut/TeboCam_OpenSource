using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace TeboCam
{
    public partial class fileprefix : Form
    {
        private const int cycleMin = 1;
        private const int cycleMax = 9999;
        private FilePrefixformDelegate prefixDelegate;
        FilePrefixSettingsDto settings;

        private string fromString;
        private bool toolTip;
        private string p_fname;
        private string p_cycleStampChecked;
        private string p_startCycle;
        private string p_endCycle;
        private string p_currentCycle;
        private bool p_includeStamp;
        private string p_fileLoc = "";
        private string p_fileLocDefault = "";
        private bool p_displayStamp;

        private string fileLoc;

        public fileprefix(FilePrefixSettingsDto filePrefixSettings)
        {
            InitializeComponent();
            settings = filePrefixSettings;
            prefixDelegate = settings.ResultDelegate;
            fromString = settings.FromString;
            toolTip = settings.ToolTip;
            p_fname = settings.FileName;
            p_cycleStampChecked = settings.CycleStampChecked.ToString();
            p_startCycle = settings.StartCycle.ToString();
            p_endCycle = settings.EndCycle.ToString();
            p_currentCycle = settings.CurrentCycle.ToString();
            p_includeStamp = settings.IncludeStamp;
            p_displayStamp = settings.DisplayStamp;
            groupBox21.Enabled = settings.ImagesSavedFolderEnabled;

            if (settings.ImagesSavedFolderEnabled)
            {
                p_fileLoc = settings.FileDir;
                p_fileLocDefault = settings.FileDirDefault;
            }


        }

        private void fileprefix_Load(object sender, EventArgs e)
        {

            lblTitle.Text = fromString + " Filename Prefix";
            filenamePrefix.Text = p_fname;
            cycleStamp.Checked = p_cycleStampChecked == "1";
            timeStamp.Checked = !cycleStamp.Checked;
            startCycle.Text = p_startCycle;
            endCycle.Text = p_endCycle;
            currentCycle.Text = p_currentCycle;
            checkBox1.Checked = p_includeStamp;
            checkBox1.Enabled = p_displayStamp;
            if (p_displayStamp) groupBox2.Enabled = checkBox1.Checked;
            fileLoc = p_fileLoc;

            if (p_fileLoc.TrimEnd('\\') == p_fileLocDefault.TrimEnd('\\'))
            {

                radioButton10.Checked = true;
                radioButton11.Checked = false;

            }
            else
            {

                radioButton10.Checked = false;
                radioButton11.Checked = true;

            }

            toolTip1.Active = toolTip;

        }


        private void filenamePrefix_Leave(object sender, EventArgs e)
        {


            filenamePrefix.Text = filenamePrefix.Text.Trim();

            if (!Valid.filenamePrefixValid(filenamePrefix.Text))
            {
                MessageDialog.messageAlert("Filename prefix can only contain a-z and 0-9.", "Invalid filename prefix");
                filenamePrefix.BackColor = Color.Red;
                if (filenamePrefix.Text == "") { filenamePrefix.Text = "TeboCam"; }; filenamePrefix.Focus();
                Invalidate();
            }

            else
            {
                filenamePrefix.BackColor = Color.LemonChiffon;
            }




        }

        private void fileprefix_FormClosing(object sender, FormClosingEventArgs e)
        {
            var dto = new FilePrefixSettingsResultDto()
            {
                FromString = fromString,
                FilenamePrefix = filenamePrefix.Text,
                CycleStamp = cycleStamp.Checked ? 1 : 2,
                StartCycle = startCycle.Text,
                EndCycle = endCycle.Text,
                CurrentCycle = currentCycle.Text,
                AppendToFilename = checkBox1.Checked,
                FileLoc = fileLoc,
                CustomLocation = radioButton11.Checked
            };

            prefixDelegate(dto); // This will call ReturnMethod in form1 and pass it val.
        }

        private void startCycle_Leave(object sender, EventArgs e)
        {
            startCycle.Text = Valid.verifyInt(startCycle.Text, cycleMin, cycleMax - 1, cycleMin.ToString());
        }

        private void endCycle_Leave(object sender, EventArgs e)
        {
            endCycle.Text = Valid.verifyInt(endCycle.Text, cycleMin + 1, cycleMax, cycleMax.ToString());
        }

        private void currentCycle_Leave(object sender, EventArgs e)
        {
            currentCycle.Text = Valid.verifyInt(currentCycle.Text, Convert.ToInt64(startCycle.Text), Convert.ToInt64(endCycle.Text), startCycle.Text);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox2.Enabled = checkBox1.Checked;
        }

        private void button21_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = fileLoc;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fileLoc = dialog.SelectedPath;
            }

            if (!fileLoc.EndsWith("\\")) fileLoc += "\\";

        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {

            button21.Enabled = radioButton11.Checked;
            if (radioButton10.Checked) fileLoc = p_fileLocDefault;


        }
    }

    public class FilePrefixSettingsDto
    {
        public FilePrefixformDelegate ResultDelegate;
        public string FromString;
        public bool ToolTip;
        public string FileName;
        public string FileDir;
        public string FileDirDefault;
        public int CycleStampChecked;
        public int StartCycle;
        public int EndCycle;
        public int CurrentCycle;
        public bool IncludeStamp;
        public bool DisplayStamp;
        public bool ImagesSavedFolderEnabled;
    }

    public class FilePrefixSettingsResultDto
    {
        public string FromString;
        public string FilenamePrefix;
        public int CycleStamp;
        public string StartCycle;
        public string EndCycle;
        public string CurrentCycle;
        public bool AppendToFilename;
        public string FileLoc;
        public bool CustomLocation;
    }
}