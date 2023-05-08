using System;
using System.Windows.Forms;

namespace TeboCam
{
    public static class Internet
    {
        public static void openInternetBrowserAt(string url)
        {
            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                TebocamState.tebowebException.LogException(ex);

                if (ex.ErrorCode == -2147467259)
                {
                    MessageBox.Show("Your internet browser does not appear to be opening with TeboCam." + Environment.NewLine + "Please open your browser with this URL: " + url);
                }
            }
            catch (System.Exception other)
            {
                TebocamState.tebowebException.LogException(other);
                MessageBox.Show(other.Message);
            }
        }

        public static bool internetConnected(string site)
        //needs to be non blank otherwise a false positive is returned
        {
            if (site.Trim() == "") site = "s";

            try
            {
                System.Net.Sockets.TcpClient clnt = new System.Net.Sockets.TcpClient(site, 80);
                clnt.Close();
                return true;
            }
            catch (Exception e)
            {
                TebocamState.tebowebException.LogException(e);
                return false;
            }
        }
    }
}
