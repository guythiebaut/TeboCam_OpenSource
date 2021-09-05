using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class FileAdminCntl : UserControl
    {
        IFileInfo FileInfo;
        private Dictionary<string, string> FileType;
        private Form containingForm;

        public TextBox GetLogsKeep() { return logsKeep; }
        public CheckBox GetLogsKeepChk() { return logsKeepChk; }

        private class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        public FileAdminCntl(IFileInfo fileInfo, Form form)
        {
            InitializeComponent();
            FileInfo = fileInfo;
            containingForm = form;
            PrepareData();
            PopulateFileTypesDropdown();

            containingForm.FormClosing += new FormClosingEventHandler(formClosing);
        }

        void formClosing(object sender, FormClosingEventArgs e)
        {
            logsKeep.Text = Valid.verifyInt(logsKeep.Text, 1, 99999, "30");
            ConfigurationHelper.GetCurrentProfile().logsKeep = Convert.ToInt32(logsKeep.Text);
        }

        private void PrepareData()
        {
            FileType = FileInfo.GetFileTypes();
        }

        private void PopulateFileTypesDropdown() 
        {
            //https://stackoverflow.com/a/3063421
            foreach (var keyValuePair in FileType)
            {
                ComboboxItem item = new ComboboxItem();
                item.Value = keyValuePair.Key;
                item.Text = keyValuePair.Value;
                FileTypeList.Items.Add(item);
            }

            FileTypeList.SelectedIndex = 0;
        }

        private void FileTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var id = SelectedId();
            lblFileCount.Text = $"{FileInfo.GetTypeForId(id)} count: {FileInfo.GetCountForId(id)}";
            lblFileSize.Text = $"{FileInfo.GetTypeForId(id)} size(kB): {FileInfo.GetSizeForId(id)}";
        }

        private string SelectedId()
        {
            return (FileTypeList.SelectedItem as ComboboxItem).Value.ToString();
        }

        private void btnZipAndVaultSelectedFiles_Click(object sender, EventArgs e)
        {
            FileInfo.ArchiveFiles(SelectedId());
        }

        private void btnDeleteFiles_Click(object sender, EventArgs e)
        {
            FileInfo.DeleteFiles(SelectedId());
        }


        private void logsKeepChk_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationHelper.GetCurrentProfile().logsKeepChk = logsKeepChk.Checked;
        }
    }
}
