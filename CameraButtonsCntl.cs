using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class CameraButtonsCntl : UserControl
    {

        public CameraButtonsCntl()
        {
            InitializeComponent();
        }

        public void AddButton(List<CameraButtonGroup> ButtonGroup, ButtonDelegate butDel, ButtonDelegate actDel, bool displayActive, int? buttonWidth)
        {

            CameraButtonGroup grp = new CameraButtonGroup();
                        
            if (buttonWidth != null)
            {
                grp.CameraButton.Width = (int)buttonWidth;
                grp.ActiveButton.Width = (int)buttonWidth;
            }

            grp.ActiveButton.Visible = displayActive;
            int lastX = 0;

            if (ButtonGroup.Count == 0)
            {
                grp.id = 1;
            }
            else
            {
                var lastButtonGroup = ButtonGroup.Select(x => x).Last();
                grp.id = lastButtonGroup.id + 1;
                lastX = lastButtonGroup.CameraButton.Right;
            }

            ButtonGroup.Add(grp);
            this.Controls.Add(grp.CameraButton);
            this.Controls.Add(grp.ActiveButton);
            grp.CameraButton.Left = lastX + 1;
            grp.CameraButton.Top = 2;
            grp.ActiveButton.Left = grp.CameraButton.Left;
            grp.ActiveButton.Top = grp.CameraButton.Bottom + 2;
            grp.CameraButton.BackColor = Color.Silver;
            grp.ActiveButton.BackColor = Color.Silver;
            grp.camDelegate = butDel;
            grp.actDelegate = actDel;
            grp.CameraButton.Text = grp.id.ToString();
            EventHandler handlerCam = (sender, args) => { grp.camDelegate(grp.id, grp.CameraButton, grp.ActiveButton); };
            grp.CameraButton.Click += handlerCam;
            EventHandler handlerAct = (sender, args) => { grp.actDelegate(grp.id, grp.CameraButton, grp.ActiveButton); };
            grp.ActiveButton.Click += handlerAct;
        }
    }
}
