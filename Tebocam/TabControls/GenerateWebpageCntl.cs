using System;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class GenerateWebpageCntl : UserControl
    {
        public GenerateWebpageCntl()
        {
            InitializeComponent();
        }

        public Label GetLblImgPref() { return lblImgPref; }

        private void button9_Click(object sender, EventArgs e)
        {
            SaveFileDialog test = new SaveFileDialog();

            test.Title = "Save WebPage...";
            test.DefaultExt = "html";
            test.AddExtension = true;
            test.Filter = "html files (*.html)|*.html|All files (*.*)|*.*";
            test.FileName = "index";
            test.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (test.ShowDialog() == DialogResult.OK)
            {
                string tmpStr = test.FileName;
                webPage.writePage(ConfigurationHelper.GetCurrentProfile().filenamePrefix, TebocamState.ImgSuffix, Convert.ToInt32(numericUpDown3.Value), Convert.ToInt32(numericUpDown4.Value), tmpStr);
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown3.Value >= numericUpDown4.Value) { numericUpDown4.Value = numericUpDown3.Value + 1; }
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown4.Value <= numericUpDown3.Value) { numericUpDown4.Value = numericUpDown3.Value + 1; }
        }
    }
}
