using System.Drawing;
using System.Windows.Forms;

namespace TeboCam
{

    public delegate void ButtonDelegate(int id, Button cameraButton, Button activeButton, bool activate = true);

    public class GroupCameraButton
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

        public GroupCameraButton()
        {
            CameraButtonState = ButtonState.NotConnected;
            ActiveButtonState = ButtonState.NotConnected;
        }

        public void SetCameraButtonSize(int width, int height)
        {
            CameraButton.Size = new Size(width, height);
        }

        public void SetActiveButtonSize(int width, int height)
        {
            ActiveButton.Size = new Size(width, height);
        }

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
