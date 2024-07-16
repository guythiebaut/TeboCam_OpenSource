using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class CameraButtonsCntl : UserControl
    {

        public CameraButtonsCntl()
        {
            InitializeComponent();
        }

        public void AddButton(List<GroupCameraButton> ButtonGroup, ButtonDelegate butDel, ButtonDelegate actDel, bool displayActive, Size cameraButtonSize, Size activeButtonSize)
        {

            GroupCameraButton grpButton = new GroupCameraButton();
            grpButton.SetCameraButtonSize(cameraButtonSize.Width, cameraButtonSize.Height);
            grpButton.SetActiveButtonSize(activeButtonSize.Width, activeButtonSize.Height);
            grpButton.ActiveButton.Visible = displayActive;
            int lastX = 0;

            if (ButtonGroup.Count == 0)
            {
                grpButton.id = 1;
            }
            else
            {
                var lastButtonGroup = ButtonGroup.Select(x => x).Last();
                grpButton.id = lastButtonGroup.id + 1;
                lastX = lastButtonGroup.CameraButton.Right;
            }

            ButtonGroup.Add(grpButton);
            this.Controls.Add(grpButton.CameraButton);
            this.Controls.Add(grpButton.ActiveButton);
            grpButton.CameraButton.Text = grpButton.id.ToString();
            grpButton.CameraButton.Left = lastX + 1;
            grpButton.CameraButton.Top = 2;
            grpButton.ActiveButton.Left = grpButton.CameraButton.Left;
            grpButton.ActiveButton.Top = grpButton.CameraButton.Bottom + 2;
            grpButton.CameraButton.BackColor = Color.Silver;
            grpButton.ActiveButton.BackColor = Color.Silver;
            grpButton.camDelegate = butDel;
            grpButton.actDelegate = actDel;
            EventHandler handlerCam = (sender, args) => { grpButton.camDelegate(grpButton.id, grpButton.CameraButton, grpButton.ActiveButton); };
            grpButton.CameraButton.Click += handlerCam;
            EventHandler handlerAct = (sender, args) => { grpButton.actDelegate(grpButton.id, grpButton.CameraButton, grpButton.ActiveButton); };
            grpButton.ActiveButton.Click += handlerAct;
        }

        public void MoveButton(int buttonId)
        {

        }

        public void ButtonVisibility(bool show)
        {

        }
    }
}
