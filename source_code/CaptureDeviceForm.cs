using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using AForge.Video.DirectShow;
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
        public CaptureDeviceForm(string dvc,bool tooltips)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            try
            {

                toolTip1.Active = tooltips;

                filters = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (filters.Count == 0)
                    throw new ApplicationException();

                // add all devices to combo
                for (int i = 0; i < filters.Count; i++)
                {
                    deviceCombo.Items.Add(filters[i].MonikerString);
                }
                //                foreach (VideoCaptureDevice filter in filters)
                //{
                //    deviceCombo.Items.Add(filter.MonikerString);
                //                }

                // number the webcams in case there are more than one of the same type present
                int tmpInt = 0;

                for (int f = 0; f < filters.Count; f++)
                {
                 
                  
                //foreach (Filter filter in filters)
                //{

                    int cnt = 1;
                    for (int i = tmpInt + 1; i < filters.Count; i++)
                    {
                        deviceCombo.Items[tmpInt] = cnt.ToString() + " - " + deviceCombo.Items[tmpInt].ToString();
                        cnt++;
                        deviceCombo.Items[i] = cnt.ToString() + " - " + deviceCombo.Items[i].ToString();
                        cnt++;
                    }

                    tmpInt++;
                                   
                //}

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
                //okButton.Enabled = false;
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
            this.label1.Location = new System.Drawing.Point(7, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select a usb webcam";
            // 
            // deviceCombo
            // 
            this.deviceCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.deviceCombo.Location = new System.Drawing.Point(7, 59);
            this.deviceCombo.Name = "deviceCombo";
            this.deviceCombo.Size = new System.Drawing.Size(325, 21);
            this.deviceCombo.TabIndex = 6;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Location = new System.Drawing.Point(178, 203);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.okButton.Location = new System.Drawing.Point(88, 203);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 8;
            this.okButton.Text = "Ok";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // rdUsbCam
            // 
            this.rdUsbCam.AutoSize = true;
            this.rdUsbCam.Checked = true;
            this.rdUsbCam.Location = new System.Drawing.Point(12, 12);
            this.rdUsbCam.Name = "rdUsbCam";
            this.rdUsbCam.Size = new System.Drawing.Size(86, 17);
            this.rdUsbCam.TabIndex = 10;
            this.rdUsbCam.TabStop = true;
            this.rdUsbCam.Text = "USB Camera";
            this.rdUsbCam.UseVisualStyleBackColor = true;
            this.rdUsbCam.CheckedChanged += new System.EventHandler(this.rdUsbCam_CheckedChanged);
            // 
            // rdIpCam
            // 
            this.rdIpCam.AutoSize = true;
            this.rdIpCam.Location = new System.Drawing.Point(103, 12);
            this.rdIpCam.Name = "rdIpCam";
            this.rdIpCam.Size = new System.Drawing.Size(74, 17);
            this.rdIpCam.TabIndex = 11;
            this.rdIpCam.Text = "IP Camera";
            this.rdIpCam.UseVisualStyleBackColor = true;
            // 
            // txtIpAddress
            // 
            this.txtIpAddress.Enabled = false;
            this.txtIpAddress.Location = new System.Drawing.Point(7, 108);
            this.txtIpAddress.Name = "txtIpAddress";
            this.txtIpAddress.Size = new System.Drawing.Size(325, 20);
            this.txtIpAddress.TabIndex = 14;
            this.toolTip1.SetToolTip(this.txtIpAddress, resources.GetString("txtIpAddress.ToolTip"));
            // 
            // label2
            // 
            this.label2.Enabled = false;
            this.label2.Location = new System.Drawing.Point(7, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(287, 14);
            this.label2.TabIndex = 13;
            this.label2.Text = "IP webcam address(mjpeg stream)";
            // 
            // txtPassword
            // 
            this.txtPassword.Enabled = false;
            this.txtPassword.Location = new System.Drawing.Point(152, 151);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(122, 20);
            this.txtPassword.TabIndex = 16;
            this.toolTip1.SetToolTip(this.txtPassword, "The password for your webcam\r\n\r\nThis will be set or you will have \r\nset this from" +
        " the IP webcam web page.");
            // 
            // txtUsername
            // 
            this.txtUsername.Enabled = false;
            this.txtUsername.Location = new System.Drawing.Point(7, 151);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(122, 20);
            this.txtUsername.TabIndex = 15;
            this.toolTip1.SetToolTip(this.txtUsername, "The login for your webcam.\r\n\r\nThis will be set or you will have \r\nset this from t" +
        "he IP webcam web page.");
            // 
            // label3
            // 
            this.label3.Enabled = false;
            this.label3.Location = new System.Drawing.Point(149, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 14);
            this.label3.TabIndex = 16;
            this.label3.Text = "Password";
            // 
            // label4
            // 
            this.label4.Enabled = false;
            this.label4.Location = new System.Drawing.Point(7, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 14);
            this.label4.TabIndex = 17;
            this.label4.Text = "Login";
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
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(344, 238);
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CaptureDeviceForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Open Webcam";
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

                label1.Enabled = true;
                deviceCombo.Enabled = true;
                label2.Enabled = false;
                txtIpAddress.Enabled = false;
                txtUsername.Enabled = false;
                txtPassword.Enabled = false;


            }
            else
            {

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
