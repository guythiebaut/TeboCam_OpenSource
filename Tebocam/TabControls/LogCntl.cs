using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class LogCntl : UserControl
    {
        Log log;

        public LogCntl(Log log)
        {
            InitializeComponent();
            this.log = log;
        }

        public RichTextBox GetTxtLog() { return txtLog; }

        private void txtLog_DoubleClick(object sender, EventArgs e)
        {
            Form frm = new Form();
            frm.Width = 400;
            frm.Height = 400;
            frm.FormBorderStyle = FormBorderStyle.FixedSingle;
            var logTxt = new RichTextBox();
            logTxt.BackColor = Color.LemonChiffon;
            logTxt.ScrollBars = RichTextBoxScrollBars.Both;
            logTxt.Dock = DockStyle.Fill;
            frm.Controls.Add(logTxt);
            logTxt.Text = txtLog.Text;
            log.LogAdded += new EventHandler(delegate (Object o, EventArgs a) { txtLog.SynchronisedInvoke(() => logTxt.Text = string.Format("{0} [{1}]", TebocamState.log.Lines.Last().Message, TebocamState.log.Lines.Last().DT.ToString("yyyy/MM/dd-HH:mm:ss:fff", System.Globalization.CultureInfo.InvariantCulture)) + "\n" + logTxt.Text); });
            frm.Show();
        }
    }
}
