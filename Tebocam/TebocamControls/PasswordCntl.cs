using System;
using System.Drawing;

namespace TeboCam.TebocamControls
{
    public partial class PasswordCntl : TebocamCntl
    {
        private bool TextVisible = false;
        private TeboCamDelegates.EventDelegate<PasswordChangedArgs> PasswordChanged;
        const int minimumWidth = 10;
        private int desiredWidth = 0;

        public PasswordCntl()
        {
            InitializeComponent();
        }

        public override void AfterControlAdded()
        {
            SetWidth(desiredWidth);
            HidePassword();
        }

        public PasswordCntl(string title, string value, TeboCamDelegates.EventDelegate<PasswordChangedArgs> passwordChanged, int width = 0, Color? passwordFieldColour = null, bool visible = false)
        {
            InitializeComponent();
            SetPasswordChanged(passwordChanged);
            SetValue(value);
            SetTitle(title);
            SetPaswordFieldColour(passwordFieldColour);
            desiredWidth = width;

            if (visible)
            {
                ShowPassword();
            }
            else
            {
                HidePassword();
            }
        }

        public void SetPaswordFieldColour(Color? colour)
        {
            PasswordField.BackColor = colour ?? Color.LemonChiffon;
        }

        public void SetTitle(string title)
        {
            lblTitle.Text = title;
        }

        public void SetValue(string value)
        {
            PasswordField.Text = value;
        }

        public void SetPasswordChanged(TeboCamDelegates.EventDelegate<PasswordChangedArgs> passwordChanged)
        {
            PasswordChanged = passwordChanged;
        }

        public void SetDesiredWidth(int width)
        {
            desiredWidth = width;
        }

        private void SetWidth(int newWidth)
        {
            var gapWidth = PasswordShowHide.Location.X - (PasswordField.Location.X + PasswordField.Width);
            var initialControlWidth = Width;
            var initialPasswordFieldWidth = PasswordField.Width;
            var growth = newWidth - (initialPasswordFieldWidth + gapWidth + PasswordShowHide.Width);
            Width = initialControlWidth + growth;

            if (newWidth < PasswordShowHide.Size.Width + gapWidth + minimumWidth)
            {
                return;
            }

            PasswordField.Width = newWidth - gapWidth - PasswordShowHide.Size.Width;
            var yPos = PasswordShowHide.Location.Y;
            var buttonLocation = new Point(PasswordField.Location.X + PasswordField.Width + gapWidth, yPos);
            PasswordShowHide.Location = buttonLocation;
            //var totalWIdth = PasswordField.Width + gapWidth + PasswordShowHide.Width;
            //PasswordField.Width = newWidth;
        }

        private void PasswordShowHide_Click(object sender, EventArgs e)
        {
            if (TextVisible)
            {
                HidePassword();
            }
            else
            {
                ShowPassword();
            }
        }

        public void HidePassword()
        {
            PasswordField.PasswordChar = '*';
            PasswordShowHide.Text = "Show";
            TextVisible = false;
        }

        public void ShowPassword()
        {
            PasswordField.PasswordChar = '\0';
            PasswordShowHide.Text = "Hide";
            TextVisible = true;
        }

        private void PasswordField_TextChanged(object sender, EventArgs e)
        {
            if (PasswordChanged != null)
            {
                PasswordChanged(this, new PasswordChangedArgs() { password = PasswordField.Text });
            }
        }
    }

    public class PasswordChangedArgs : EventArgs
    {
        private string _password;

        public string password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }
    }
}
