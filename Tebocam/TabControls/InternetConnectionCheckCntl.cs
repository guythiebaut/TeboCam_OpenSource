using System;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class InternetConnectionCheckCntl : UserControl
    {
        public InternetConnectionCheckCntl()
        {
            InitializeComponent();
        }

        public TextBox GetTxtInternetConnection() { return txtInternetConnection; }

        private void txtInternetConnection_Leave(object sender, EventArgs e)
        {
            if (txtInternetConnection.Text.Trim() == "") txtInternetConnection.Text = "www.google.com";
            ConfigurationHelper.GetCurrentProfile().internetCheck = txtInternetConnection.Text;
        }
    }
}
