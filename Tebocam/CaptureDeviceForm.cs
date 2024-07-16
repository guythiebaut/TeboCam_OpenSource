using AForge.Video.DirectShow;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
//using dshow;
//using dshow.Core;

namespace TeboCam
{
    /// <summary>
    /// Summary description for CaptureDeviceForm.
    /// </summary>
    public class CaptureDeviceForm : System.Windows.Forms.Form
    {

        //AForge.Video.DirectShow.FilterInfoCollection filters;
        //FilterCollection filters;
        FilterInfoCollection filters;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox deviceCombo;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private TextBox txtPassword;
        private TextBox txtUsername;
        private Label label3;
        private Label label4;
        private RadioButton rdUsbCam;
        private RadioButton rdIpCam;
        private TextBox txtIpAddress;
        private Label label2;

        //private string device;
        private selected selectedValues = new selected();
        private ToolTip toolTip1;
        private IContainer components;
        string currentDvc;

        // Device
        //public string Device
        //{
        //    get { return device; }
        //}

        public selected Device
        {
            get { return selectedValues; }
        }

        // Constructor
        public CaptureDeviceForm(string dvc, bool tooltips)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            //
            currentDvc = dvc;
            toolTip1.Active = tooltips;
            PopulateUsbWebcamList(currentDvc);
        }

        private void PopulateUsbWebcamList(string dvc)
        {
            try
            {
                deviceCombo.Items.Clear();
                filters = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                //remove cameras that are already attached
                for (int i = filters.Count - 1; i >= 0; i--)
                {
                    if (CameraRig.ConnectedCameras.Where(x => x.cameraName == filters[i].MonikerString).Count() > 0)
                    {
                        filters.RemoveAt(i);
                    }
                }

                if (filters.Count == 0)
                    throw new ApplicationException();
                // add all devices to combo
                for (int i = 0; i < filters.Count; i++)
                {
                    deviceCombo.Items.Add(filters[i].MonikerString);
                }

                // number the webcams in case there are more than one of the same type present
                int tmpInt = 0;

                for (int f = 0; f < filters.Count; f++)
                {
                    int cnt = 1;
                    for (int i = tmpInt + 1; i < filters.Count; i++)
                    {
                        deviceCombo.Items[tmpInt] = cnt.ToString() + " - " + deviceCombo.Items[tmpInt].ToString();
                        cnt++;
                        deviceCombo.Items[i] = cnt.ToString() + " - " + deviceCombo.Items[i].ToString();
                        cnt++;
                    }
                    tmpInt++;
                }
                // set the selected index to the current webcam
                if (dvc != null)
                {
                    for (int i = 0; i < filters.Count; i++)
                    {
                        if (filters[i].MonikerString.ToString() == dvc)
                        { deviceCombo.SelectedIndex = i; }
                    }
                }
            }
            catch (ApplicationException)
            {
                deviceCombo.Items.Add("No local capture devices");
                deviceCombo.Enabled = false;
                okButton.Enabled = false;
            }

            if (deviceCombo.SelectedIndex == -1) deviceCombo.SelectedIndex = 0;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CaptureDeviceForm));
            this.label1 = new System.Windows.Forms.Label();
            this.deviceCombo = new System.Windows.Forms.ComboBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.rdUsbCam = new System.Windows.Forms.RadioButton();
            this.rdIpCam = new System.Windows.Forms.RadioButton();
            this.txtIpAddress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // deviceCombo
            // 
            this.deviceCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.deviceCombo, "deviceCombo");
            this.deviceCombo.Name = "deviceCombo";
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.Name = "cancelButton";
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // rdUsbCam
            // 
            resources.ApplyResources(this.rdUsbCam, "rdUsbCam");
            this.rdUsbCam.Checked = true;
            this.rdUsbCam.Name = "rdUsbCam";
            this.rdUsbCam.TabStop = true;
            this.rdUsbCam.UseVisualStyleBackColor = true;
            this.rdUsbCam.CheckedChanged += new System.EventHandler(this.rdUsbCam_CheckedChanged);
            // 
            // rdIpCam
            // 
            resources.ApplyResources(this.rdIpCam, "rdIpCam");
            this.rdIpCam.Name = "rdIpCam";
            this.rdIpCam.UseVisualStyleBackColor = true;
            // 
            // txtIpAddress
            // 
            resources.ApplyResources(this.txtIpAddress, "txtIpAddress");
            this.txtIpAddress.Name = "txtIpAddress";
            this.toolTip1.SetToolTip(this.txtIpAddress, resources.GetString("txtIpAddress.ToolTip"));
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtPassword
            // 
            resources.ApplyResources(this.txtPassword, "txtPassword");
            this.txtPassword.Name = "txtPassword";
            this.toolTip1.SetToolTip(this.txtPassword, resources.GetString("txtPassword.ToolTip"));
            // 
            // txtUsername
            // 
            resources.ApplyResources(this.txtUsername, "txtUsername");
            this.txtUsername.Name = "txtUsername";
            this.toolTip1.SetToolTip(this.txtUsername, resources.GetString("txtUsername.ToolTip"));
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 20000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.ToolTipTitle = "Tip";
            // 
            // CaptureDeviceForm
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtIpAddress);
            this.Controls.Add(this.rdIpCam);
            this.Controls.Add(this.rdUsbCam);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.deviceCombo);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CaptureDeviceForm";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        // On "Ok" button
        private void okButton_Click(object sender, System.EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(filters[deviceCombo.SelectedIndex].MonikerString.ToString());

            if (rdUsbCam.Checked)
            {
                selectedValues.name = filters[deviceCombo.SelectedIndex].MonikerString;
                selectedValues.address = filters[deviceCombo.SelectedIndex].MonikerString;
            }
            else
            {
                selectedValues.ipCam = true;
                selectedValues.name = txtIpAddress.Text;
                selectedValues.address = txtIpAddress.Text;
                selectedValues.user = txtUsername.Text;
                selectedValues.password = txtPassword.Text;
            }
            //device = filters[deviceCombo.SelectedIndex].MonikerString;
        }

        private void rdUsbCam_CheckedChanged(object sender, EventArgs e)
        {
            if (rdUsbCam.Checked)
            {
                PopulateUsbWebcamList(currentDvc);
                label1.Enabled = true;
                deviceCombo.Enabled = true;
                label2.Enabled = false;
                txtIpAddress.Enabled = false;
                txtUsername.Enabled = false;
                txtPassword.Enabled = false;
            }
            else
            {
                okButton.Enabled = true;
                label1.Enabled = false;
                deviceCombo.Enabled = false;
                label2.Enabled = true;
                txtIpAddress.Enabled = true;
                txtUsername.Enabled = true;
                txtPassword.Enabled = true;
            }
        }

        public class selected
        {

            public bool ipCam = false;
            public string name = "";
            public string address = "";
            public string user = "";
            public string password = "";

        }





    }
}
