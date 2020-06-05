using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TeboCam
{

    public delegate void ButtonDelegate(int id, Button cameraButton, Button activeButton, bool activate = true);

    public class CameraButtonGroup
    {
        public enum ButtonState
        {
            NotConnected,
            ConnectedAndInactive,
            ConnectedAndActive
        }

        public int id;
        public Button CameraButton = new Button();
        public ButtonState CameraButtonState { get; private set; }
        public ButtonDelegate camDelegate;
        public Button ActiveButton = new Button();
        public ButtonState ActiveButtonState { get; private set; }
        public ButtonDelegate actDelegate;

        public CameraButtonGroup()
        {
            CameraButton.Size = new Size(20, 20);
            ActiveButton.Size = new Size(20, 10);
            CameraButtonState = ButtonState.NotConnected;
            ActiveButtonState = ButtonState.NotConnected;
        }

        //need delegate for on click

        public void CameraButtonIsActive()
        {
            CameraButton.BackColor = Color.LawnGreen;
            CameraButtonState = ButtonState.ConnectedAndActive;
        }

        public void CameraButtonIsConnectedAndInactive()
        {
            CameraButton.BackColor = Color.SkyBlue;
            CameraButtonState = ButtonState.ConnectedAndInactive;
        }

        public void CameraButtonIsNotConnected()
        {
            CameraButton.BackColor = Color.Silver;
            CameraButtonState = ButtonState.NotConnected;
        }

        public void ActiveButtonIsActive()
        {
            ActiveButton.BackColor = Color.LawnGreen;
            ActiveButtonState = ButtonState.ConnectedAndActive;
        }

        public void ActiveButtonIsInactive()
        {
            ActiveButton.BackColor = Color.Silver;
            ActiveButtonState = ButtonState.NotConnected;
        }

    }
}
