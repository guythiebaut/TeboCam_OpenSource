namespace TeboCam
{
    partial class webcamConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(webcamConfig));
            this.bttnCamProp = new System.Windows.Forms.Button();
            this.txtMov = new System.Windows.Forms.TextBox();
            this.pnlSelection = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.areaOffAtMotion = new System.Windows.Forms.CheckBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.button15 = new System.Windows.Forms.Button();
            this.drawModeOff = new System.Windows.Forms.RadioButton();
            this.drawModeOn = new System.Windows.Forms.RadioButton();
            this.showSelection = new System.Windows.Forms.GroupBox();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.label33 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMess = new System.Windows.Forms.TextBox();
            this.actCount = new System.Windows.Forms.TextBox();
            this.calibrate = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.camName = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.bttncam9 = new System.Windows.Forms.Button();
            this.bttncam7 = new System.Windows.Forms.Button();
            this.bttncam8 = new System.Windows.Forms.Button();
            this.bttncam6 = new System.Windows.Forms.Button();
            this.bttncam1 = new System.Windows.Forms.Button();
            this.bttncam4 = new System.Windows.Forms.Button();
            this.bttncam2 = new System.Windows.Forms.Button();
            this.bttncam5 = new System.Windows.Forms.Button();
            this.bttncam3 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.levelbox = new System.Windows.Forms.PictureBox();
            this.label38 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.trkMov = new System.Windows.Forms.TrackBar();
            this.cameraWindow = new TeboCam.CameraWindow();
            this.pnlSelection.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.showSelection.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.levelbox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkMov)).BeginInit();
            this.SuspendLayout();
            // 
            // bttnCamProp
            // 
            this.bttnCamProp.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttnCamProp.Location = new System.Drawing.Point(195, 28);
            this.bttnCamProp.Name = "bttnCamProp";
            this.bttnCamProp.Size = new System.Drawing.Size(158, 72);
            this.bttnCamProp.TabIndex = 99;
            this.bttnCamProp.Text = "Camera Properties";
            this.toolTip1.SetToolTip(this.bttnCamProp, "Open the properties interface for the camera.");
            this.bttnCamProp.UseVisualStyleBackColor = true;
            this.bttnCamProp.Click += new System.EventHandler(this.bttnCamProp_Click);
            // 
            // txtMov
            // 
            this.txtMov.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtMov.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMov.Location = new System.Drawing.Point(390, 288);
            this.txtMov.MaxLength = 2;
            this.txtMov.Name = "txtMov";
            this.txtMov.Size = new System.Drawing.Size(49, 20);
            this.txtMov.TabIndex = 96;
            this.toolTip1.SetToolTip(this.txtMov, "Set motion sensitivity.\r\n\r\n0 = most sensitive\r\n99 = least sensitive\r\n\r\nYou can al" +
                    "so set the motion sensitivity with\r\nthe slide bar above.");
            this.txtMov.TextChanged += new System.EventHandler(this.txtMov_TextChanged);
            this.txtMov.Leave += new System.EventHandler(this.txtMov_Leave);
            // 
            // pnlSelection
            // 
            this.pnlSelection.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlSelection.Controls.Add(this.groupBox4);
            this.pnlSelection.Controls.Add(this.groupBox11);
            this.pnlSelection.Controls.Add(this.groupBox12);
            this.pnlSelection.Controls.Add(this.showSelection);
            this.pnlSelection.Location = new System.Drawing.Point(12, 132);
            this.pnlSelection.Name = "pnlSelection";
            this.pnlSelection.Size = new System.Drawing.Size(360, 261);
            this.pnlSelection.TabIndex = 100;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radioButton2);
            this.groupBox4.Controls.Add(this.radioButton1);
            this.groupBox4.Location = new System.Drawing.Point(13, 168);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(287, 65);
            this.groupBox4.TabIndex = 63;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Detect Motion";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(7, 43);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(86, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "Outside Area";
            this.toolTip1.SetToolTip(this.radioButton2, "Detect motion only outside the area drawn.");
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(7, 20);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(80, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Within Area";
            this.toolTip1.SetToolTip(this.radioButton1, "Detect motion only within the area drawn.");
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.areaOffAtMotion);
            this.groupBox11.Controls.Add(this.radioButton3);
            this.groupBox11.Controls.Add(this.radioButton4);
            this.groupBox11.Location = new System.Drawing.Point(13, 70);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(287, 46);
            this.groupBox11.TabIndex = 64;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Area Detect Mode";
            // 
            // areaOffAtMotion
            // 
            this.areaOffAtMotion.AutoSize = true;
            this.areaOffAtMotion.Location = new System.Drawing.Point(105, 21);
            this.areaOffAtMotion.Name = "areaOffAtMotion";
            this.areaOffAtMotion.Size = new System.Drawing.Size(122, 17);
            this.areaOffAtMotion.TabIndex = 2;
            this.areaOffAtMotion.Text = "Switch off on motion";
            this.toolTip1.SetToolTip(this.areaOffAtMotion, resources.GetString("areaOffAtMotion.ToolTip"));
            this.areaOffAtMotion.UseVisualStyleBackColor = true;
            this.areaOffAtMotion.CheckedChanged += new System.EventHandler(this.areaOffAtMotion_CheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(55, 20);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(39, 17);
            this.radioButton3.TabIndex = 1;
            this.radioButton3.Text = "Off";
            this.toolTip1.SetToolTip(this.radioButton3, "Use the whole of the image for movement detection.");
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Checked = true;
            this.radioButton4.Location = new System.Drawing.Point(7, 20);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(39, 17);
            this.radioButton4.TabIndex = 0;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "On";
            this.toolTip1.SetToolTip(this.radioButton4, "Use the area within rectangle already drawn to exclude or include\r\nmovement detec" +
                    "tion..");
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.button15);
            this.groupBox12.Controls.Add(this.drawModeOff);
            this.groupBox12.Controls.Add(this.drawModeOn);
            this.groupBox12.Location = new System.Drawing.Point(13, 19);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(287, 44);
            this.groupBox12.TabIndex = 65;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Area Draw Mode";
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(154, 14);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(96, 23);
            this.button15.TabIndex = 2;
            this.button15.Text = "Reset";
            this.toolTip1.SetToolTip(this.button15, "Reset the position of the rectangle.\r\n\r\nUseful if you resize the rectangle\r\nand c" +
                    "annot find it.");
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // drawModeOff
            // 
            this.drawModeOff.AutoSize = true;
            this.drawModeOff.Checked = true;
            this.drawModeOff.Location = new System.Drawing.Point(55, 20);
            this.drawModeOff.Name = "drawModeOff";
            this.drawModeOff.Size = new System.Drawing.Size(39, 17);
            this.drawModeOff.TabIndex = 1;
            this.drawModeOff.TabStop = true;
            this.drawModeOff.Text = "Off";
            this.drawModeOff.UseVisualStyleBackColor = true;
            // 
            // drawModeOn
            // 
            this.drawModeOn.AutoSize = true;
            this.drawModeOn.Location = new System.Drawing.Point(7, 20);
            this.drawModeOn.Name = "drawModeOn";
            this.drawModeOn.Size = new System.Drawing.Size(39, 17);
            this.drawModeOn.TabIndex = 0;
            this.drawModeOn.Text = "On";
            this.toolTip1.SetToolTip(this.drawModeOn, "Draw a rectangle around the are you wish to monitor or exclude.\r\n\r\nOnce the recta" +
                    "ngle is drawn you can resize it or drag it to another \r\narea of the screen.");
            this.drawModeOn.UseVisualStyleBackColor = true;
            this.drawModeOn.CheckedChanged += new System.EventHandler(this.drawModeOn_CheckedChanged);
            // 
            // showSelection
            // 
            this.showSelection.Controls.Add(this.radioButton7);
            this.showSelection.Controls.Add(this.radioButton8);
            this.showSelection.Location = new System.Drawing.Point(10, 122);
            this.showSelection.Name = "showSelection";
            this.showSelection.Size = new System.Drawing.Size(290, 40);
            this.showSelection.TabIndex = 66;
            this.showSelection.TabStop = false;
            this.showSelection.Text = "Show Area";
            // 
            // radioButton7
            // 
            this.radioButton7.AutoSize = true;
            this.radioButton7.Checked = true;
            this.radioButton7.Location = new System.Drawing.Point(55, 20);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new System.Drawing.Size(39, 17);
            this.radioButton7.TabIndex = 1;
            this.radioButton7.TabStop = true;
            this.radioButton7.Text = "Off";
            this.radioButton7.UseVisualStyleBackColor = true;
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Location = new System.Drawing.Point(7, 20);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(39, 17);
            this.radioButton8.TabIndex = 0;
            this.radioButton8.Text = "On";
            this.toolTip1.SetToolTip(this.radioButton8, "Show the area where motion detection is detected.\r\nThis is purely a visual check " +
                    "and does not affect motion deteciton.");
            this.radioButton8.UseVisualStyleBackColor = true;
            this.radioButton8.CheckedChanged += new System.EventHandler(this.radioButton8_CheckedChanged);
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.BackColor = System.Drawing.Color.Transparent;
            this.label33.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.ForeColor = System.Drawing.Color.Black;
            this.label33.Location = new System.Drawing.Point(6, 60);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(98, 13);
            this.label33.TabIndex = 102;
            this.label33.Text = "Camera Name";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtMess);
            this.groupBox1.Controls.Add(this.actCount);
            this.groupBox1.Location = new System.Drawing.Point(460, 317);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(338, 51);
            this.groupBox1.TabIndex = 109;
            this.groupBox1.TabStop = false;
            // 
            // txtMess
            // 
            this.txtMess.BackColor = System.Drawing.SystemColors.Control;
            this.txtMess.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMess.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMess.ForeColor = System.Drawing.Color.RoyalBlue;
            this.txtMess.Location = new System.Drawing.Point(16, 22);
            this.txtMess.Name = "txtMess";
            this.txtMess.Size = new System.Drawing.Size(198, 19);
            this.txtMess.TabIndex = 53;
            this.txtMess.Text = " ";
            this.txtMess.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // actCount
            // 
            this.actCount.BackColor = System.Drawing.SystemColors.Control;
            this.actCount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.actCount.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actCount.ForeColor = System.Drawing.Color.RoyalBlue;
            this.actCount.Location = new System.Drawing.Point(224, 22);
            this.actCount.Name = "actCount";
            this.actCount.Size = new System.Drawing.Size(97, 19);
            this.actCount.TabIndex = 39;
            this.actCount.Text = "00";
            this.actCount.Visible = false;
            // 
            // calibrate
            // 
            this.calibrate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.calibrate.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.calibrate.Location = new System.Drawing.Point(565, 288);
            this.calibrate.Margin = new System.Windows.Forms.Padding(0);
            this.calibrate.Name = "calibrate";
            this.calibrate.Size = new System.Drawing.Size(75, 23);
            this.calibrate.TabIndex = 106;
            this.calibrate.Text = "Calibrate";
            this.toolTip1.SetToolTip(this.calibrate, resources.GetString("calibrate.ToolTip"));
            this.calibrate.UseVisualStyleBackColor = true;
            this.calibrate.Click += new System.EventHandler(this.calibrate_Click);
            // 
            // button8
            // 
            this.button8.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.button8.Location = new System.Drawing.Point(476, 288);
            this.button8.Margin = new System.Windows.Forms.Padding(0);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 23);
            this.button8.TabIndex = 105;
            this.button8.Text = "Train";
            this.toolTip1.SetToolTip(this.button8, resources.GetString("button8.ToolTip"));
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // camName
            // 
            this.camName.BackColor = System.Drawing.Color.LemonChiffon;
            this.camName.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.camName.Location = new System.Drawing.Point(8, 80);
            this.camName.MaxLength = 20;
            this.camName.Name = "camName";
            this.camName.Size = new System.Drawing.Size(181, 20);
            this.camName.TabIndex = 107;
            this.toolTip1.SetToolTip(this.camName, "Friendly name for easy camera identification.");
            this.camName.TextChanged += new System.EventHandler(this.camName_TextChanged);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.button5);
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.bttncam9);
            this.panel2.Controls.Add(this.label33);
            this.panel2.Controls.Add(this.bttncam7);
            this.panel2.Controls.Add(this.camName);
            this.panel2.Controls.Add(this.bttncam8);
            this.panel2.Controls.Add(this.bttnCamProp);
            this.panel2.Controls.Add(this.bttncam6);
            this.panel2.Controls.Add(this.bttncam1);
            this.panel2.Controls.Add(this.bttncam4);
            this.panel2.Controls.Add(this.bttncam2);
            this.panel2.Controls.Add(this.bttncam5);
            this.panel2.Controls.Add(this.bttncam3);
            this.panel2.Location = new System.Drawing.Point(12, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(360, 114);
            this.panel2.TabIndex = 106;
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.SystemColors.Control;
            this.button5.Enabled = false;
            this.button5.Location = new System.Drawing.Point(295, 5);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(58, 20);
            this.button5.TabIndex = 129;
            this.button5.Text = "Confirm";
            this.toolTip1.SetToolTip(this.button5, "Confirm camera removal - camera will be re-attached at startup.");
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(195, 5);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(94, 20);
            this.button4.TabIndex = 3;
            this.button4.Text = "Remove Cam";
            this.toolTip1.SetToolTip(this.button4, "Remove the camera - camera will be re-attached at startup.");
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(30, 28);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(137, 26);
            this.button3.TabIndex = 3;
            this.button3.Text = "Change Button";
            this.toolTip1.SetToolTip(this.button3, "Change the button assigned to the camera");
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.Control;
            this.button2.Enabled = false;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.button2.Location = new System.Drawing.Point(9, 28);
            this.button2.Margin = new System.Windows.Forms.Padding(0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(20, 26);
            this.button2.TabIndex = 128;
            this.button2.Text = "<";
            this.toolTip1.SetToolTip(this.button2, "Select next button to the left");
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.camDown);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.Enabled = false;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.button1.Location = new System.Drawing.Point(169, 28);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(20, 26);
            this.button1.TabIndex = 127;
            this.button1.Text = ">";
            this.toolTip1.SetToolTip(this.button1, "Select next button to the right");
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.camUp);
            // 
            // bttncam9
            // 
            this.bttncam9.BackColor = System.Drawing.Color.Silver;
            this.bttncam9.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.bttncam9.Location = new System.Drawing.Point(169, 5);
            this.bttncam9.Margin = new System.Windows.Forms.Padding(0);
            this.bttncam9.Name = "bttncam9";
            this.bttncam9.Size = new System.Drawing.Size(20, 20);
            this.bttncam9.TabIndex = 124;
            this.bttncam9.Text = "9";
            this.toolTip1.SetToolTip(this.bttncam9, "Select Camera 9\r\n\r\nGreen - camera connected and selected\r\nBlue -  camera connecte" +
                    "d\r\nGrey - no camera present");
            this.bttncam9.UseVisualStyleBackColor = false;
            this.bttncam9.Click += new System.EventHandler(this.bttncam9_Click);
            // 
            // bttncam7
            // 
            this.bttncam7.BackColor = System.Drawing.Color.Silver;
            this.bttncam7.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.bttncam7.Location = new System.Drawing.Point(129, 5);
            this.bttncam7.Margin = new System.Windows.Forms.Padding(0);
            this.bttncam7.Name = "bttncam7";
            this.bttncam7.Size = new System.Drawing.Size(20, 20);
            this.bttncam7.TabIndex = 123;
            this.bttncam7.Text = "7";
            this.toolTip1.SetToolTip(this.bttncam7, "Select Camera 7\r\n\r\nGreen - camera connected and selected\r\nBlue -  camera connecte" +
                    "d\r\nGrey - no camera present");
            this.bttncam7.UseVisualStyleBackColor = false;
            this.bttncam7.Click += new System.EventHandler(this.bttncam7_Click);
            // 
            // bttncam8
            // 
            this.bttncam8.BackColor = System.Drawing.Color.Silver;
            this.bttncam8.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.bttncam8.Location = new System.Drawing.Point(149, 5);
            this.bttncam8.Margin = new System.Windows.Forms.Padding(0);
            this.bttncam8.Name = "bttncam8";
            this.bttncam8.Size = new System.Drawing.Size(20, 20);
            this.bttncam8.TabIndex = 122;
            this.bttncam8.Text = "8";
            this.toolTip1.SetToolTip(this.bttncam8, "Select Camera 8\r\n\r\nGreen - camera connected and selected\r\nBlue -  camera connecte" +
                    "d\r\nGrey - no camera present");
            this.bttncam8.UseVisualStyleBackColor = false;
            this.bttncam8.Click += new System.EventHandler(this.bttncam8_Click);
            // 
            // bttncam6
            // 
            this.bttncam6.BackColor = System.Drawing.Color.Silver;
            this.bttncam6.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.bttncam6.Location = new System.Drawing.Point(109, 5);
            this.bttncam6.Margin = new System.Windows.Forms.Padding(0);
            this.bttncam6.Name = "bttncam6";
            this.bttncam6.Size = new System.Drawing.Size(20, 20);
            this.bttncam6.TabIndex = 121;
            this.bttncam6.Text = "6";
            this.toolTip1.SetToolTip(this.bttncam6, "Select Camera 6\r\n\r\nGreen - camera connected and selected\r\nBlue -  camera connecte" +
                    "d\r\nGrey - no camera present");
            this.bttncam6.UseVisualStyleBackColor = false;
            this.bttncam6.Click += new System.EventHandler(this.bttncam6_Click);
            // 
            // bttncam1
            // 
            this.bttncam1.BackColor = System.Drawing.Color.Silver;
            this.bttncam1.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.bttncam1.Location = new System.Drawing.Point(9, 5);
            this.bttncam1.Margin = new System.Windows.Forms.Padding(0);
            this.bttncam1.Name = "bttncam1";
            this.bttncam1.Size = new System.Drawing.Size(20, 20);
            this.bttncam1.TabIndex = 1;
            this.bttncam1.Text = "1";
            this.toolTip1.SetToolTip(this.bttncam1, "Select Camera 1\r\n\r\nGreen - camera connected and selected\r\nBlue -  camera connecte" +
                    "d\r\nGrey - no camera present");
            this.bttncam1.UseVisualStyleBackColor = false;
            this.bttncam1.Click += new System.EventHandler(this.bttncam1_Click);
            // 
            // bttncam4
            // 
            this.bttncam4.BackColor = System.Drawing.Color.Silver;
            this.bttncam4.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.bttncam4.Location = new System.Drawing.Point(69, 5);
            this.bttncam4.Margin = new System.Windows.Forms.Padding(0);
            this.bttncam4.Name = "bttncam4";
            this.bttncam4.Size = new System.Drawing.Size(20, 20);
            this.bttncam4.TabIndex = 120;
            this.bttncam4.Text = "4";
            this.toolTip1.SetToolTip(this.bttncam4, "Select Camera 4\r\n\r\nGreen - camera connected and selected\r\nBlue -  camera connecte" +
                    "d\r\nGrey - no camera present");
            this.bttncam4.UseVisualStyleBackColor = false;
            this.bttncam4.Click += new System.EventHandler(this.bttncam4_Click);
            // 
            // bttncam2
            // 
            this.bttncam2.BackColor = System.Drawing.Color.Silver;
            this.bttncam2.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.bttncam2.Location = new System.Drawing.Point(29, 5);
            this.bttncam2.Margin = new System.Windows.Forms.Padding(0);
            this.bttncam2.Name = "bttncam2";
            this.bttncam2.Size = new System.Drawing.Size(20, 20);
            this.bttncam2.TabIndex = 117;
            this.bttncam2.Text = "2";
            this.toolTip1.SetToolTip(this.bttncam2, "Select Camera 2\r\n\r\nGreen - camera connected and selected\r\nBlue -  camera connecte" +
                    "d\r\nGrey - no camera present");
            this.bttncam2.UseVisualStyleBackColor = false;
            this.bttncam2.Click += new System.EventHandler(this.bttncam2_Click);
            // 
            // bttncam5
            // 
            this.bttncam5.BackColor = System.Drawing.Color.Silver;
            this.bttncam5.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.bttncam5.Location = new System.Drawing.Point(89, 5);
            this.bttncam5.Margin = new System.Windows.Forms.Padding(0);
            this.bttncam5.Name = "bttncam5";
            this.bttncam5.Size = new System.Drawing.Size(20, 20);
            this.bttncam5.TabIndex = 119;
            this.bttncam5.Text = "5";
            this.toolTip1.SetToolTip(this.bttncam5, "Select Camera 5\r\n\r\nGreen - camera connected and selected\r\nBlue -  camera connecte" +
                    "d\r\nGrey - no camera present");
            this.bttncam5.UseVisualStyleBackColor = false;
            this.bttncam5.Click += new System.EventHandler(this.bttncam5_Click);
            // 
            // bttncam3
            // 
            this.bttncam3.BackColor = System.Drawing.Color.Silver;
            this.bttncam3.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.bttncam3.Location = new System.Drawing.Point(49, 5);
            this.bttncam3.Margin = new System.Windows.Forms.Padding(0);
            this.bttncam3.Name = "bttncam3";
            this.bttncam3.Size = new System.Drawing.Size(20, 20);
            this.bttncam3.TabIndex = 118;
            this.bttncam3.Text = "3";
            this.toolTip1.SetToolTip(this.bttncam3, "Select Camera 3\r\n\r\nGreen - camera connected and selected\r\nBlue -  camera connecte" +
                    "d\r\nGrey - no camera present");
            this.bttncam3.UseVisualStyleBackColor = false;
            this.bttncam3.Click += new System.EventHandler(this.bttncam3_Click);
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.Controls.Add(this.cameraWindow);
            this.panel3.Location = new System.Drawing.Point(473, 18);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(338, 255);
            this.panel3.TabIndex = 107;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 20000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.ToolTipTitle = "Tip";
            // 
            // levelbox
            // 
            this.levelbox.Location = new System.Drawing.Point(447, 28);
            this.levelbox.Name = "levelbox";
            this.levelbox.Size = new System.Drawing.Size(10, 243);
            this.levelbox.TabIndex = 109;
            this.levelbox.TabStop = false;
            this.toolTip1.SetToolTip(this.levelbox, "Motion detection level.");
            this.levelbox.Paint += new System.Windows.Forms.PaintEventHandler(this.levelbox_Paint);
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label38.Location = new System.Drawing.Point(431, 261);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(14, 14);
            this.label38.TabIndex = 114;
            this.label38.Text = "0";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label37.Location = new System.Drawing.Point(417, 18);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(28, 14);
            this.label37.TabIndex = 113;
            this.label37.Text = "100";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.Location = new System.Drawing.Point(424, 78);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(21, 14);
            this.label31.TabIndex = 112;
            this.label31.Text = "75";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.Location = new System.Drawing.Point(424, 198);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(21, 14);
            this.label30.TabIndex = 111;
            this.label30.Text = "25";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.Location = new System.Drawing.Point(424, 139);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(21, 14);
            this.label29.TabIndex = 110;
            this.label29.Text = "50";
            // 
            // trkMov
            // 
            this.trkMov.Location = new System.Drawing.Point(390, 17);
            this.trkMov.Margin = new System.Windows.Forms.Padding(0);
            this.trkMov.Maximum = 100;
            this.trkMov.Minimum = 1;
            this.trkMov.Name = "trkMov";
            this.trkMov.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trkMov.Size = new System.Drawing.Size(42, 268);
            this.trkMov.TabIndex = 115;
            this.trkMov.TickFrequency = 10;
            this.trkMov.Value = 1;
            this.trkMov.Scroll += new System.EventHandler(this.trkMov_Scroll);
            // 
            // cameraWindow
            // 
            this.cameraWindow.BackColor = System.Drawing.SystemColors.Control;
            this.cameraWindow.Camera = null;
            this.cameraWindow.Location = new System.Drawing.Point(3, 8);
            this.cameraWindow.Name = "cameraWindow";
            this.cameraWindow.Size = new System.Drawing.Size(322, 242);
            this.cameraWindow.TabIndex = 2;
            // 
            // webcamConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(830, 409);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label38);
            this.Controls.Add(this.txtMov);
            this.Controls.Add(this.label37);
            this.Controls.Add(this.label31);
            this.Controls.Add(this.calibrate);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.levelbox);
            this.Controls.Add(this.trkMov);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pnlSelection);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(850, 450);
            this.Name = "webcamConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Webcam Configuration";
            this.Load += new System.EventHandler(this.webcamConfig_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.webcamConfig_FormClosing);
            this.pnlSelection.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.showSelection.ResumeLayout(false);
            this.showSelection.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.levelbox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkMov)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bttnCamProp;
        private System.Windows.Forms.TextBox txtMov;
        private System.Windows.Forms.Panel pnlSelection;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.GroupBox showSelection;
        private System.Windows.Forms.RadioButton radioButton7;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.CheckBox areaOffAtMotion;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.RadioButton drawModeOff;
        private System.Windows.Forms.RadioButton drawModeOn;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox camName;
        private System.Windows.Forms.Button calibrate;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private CameraWindow cameraWindow;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtMess;
        private System.Windows.Forms.TextBox actCount;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.PictureBox levelbox;
        private System.Windows.Forms.TrackBar trkMov;
        private System.Windows.Forms.Button bttncam9;
        private System.Windows.Forms.Button bttncam7;
        private System.Windows.Forms.Button bttncam8;
        private System.Windows.Forms.Button bttncam6;
        private System.Windows.Forms.Button bttncam1;
        private System.Windows.Forms.Button bttncam4;
        private System.Windows.Forms.Button bttncam2;
        private System.Windows.Forms.Button bttncam5;
        private System.Windows.Forms.Button bttncam3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
    }
}