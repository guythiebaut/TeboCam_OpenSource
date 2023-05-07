using System;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class AdminCntl : UserControl
    {

        public AdminCntl(Form parentForm)
        {
            Initialise();
        }

        private void Initialise()
        {
            InitializeComponent();
        }

        private void btnAlert_Click(object sender, EventArgs e)
        {
            if (chkAlertAll.Checked)
            {
                foreach (var cam in CameraRig.ConnectedCameras)
                {
                    TebocamState.log.AddLine(string.Format("ForceAlarm  camNo:{0}", cam.camera.camNo));
                    cam.camera.ForceAlarm();
                }
            }
            else
            {
                TebocamState.log.AddLine(string.Format("ForceAlarm  camNo:{0}", GetCamera((int)numCamera.Value).camNo));
                Alarm(GetCamera((int)numCamera.Value));
            }
        }

        private void Alarm(Camera camera)
        {
            camera.ForceAlarm();
        }


        private Camera GetCamera(int num)
        {
            return CameraRig.getCam(CameraRig.idxFromButton(num));
        }

    }
}
