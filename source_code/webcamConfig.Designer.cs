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
            this.cameraWindow = new TeboCam.CameraWindow();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.levelbox = new System.Windows.Forms.PictureBox();
            this.label38 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.trkMov = new System.Windows.Forms.TrackBar();
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
            resources.ApplyResources(this.bttnCamProp, "bttnCamProp");
            this.bttnCamProp.Name = "bttnCamProp";
            this.toolTip1.SetToolTip(this.bttnCamProp, resources.GetString("bttnCamProp.ToolTip"));
            this.bttnCamProp.UseVisualStyleBackColor = true;
            this.bttnCamProp.Click += new System.EventHandler(this.bttnCamProp_Click);
            // 
            // txtMov
            // 
            this.txtMov.BackColor = System.Drawing.Color.LemonChiffon;
            resources.ApplyResources(this.txtMov, "txtMov");
            this.txtMov.Name = "txtMov";
            this.toolTip1.SetToolTip(this.txtMov, resources.GetString("txtMov.ToolTip"));
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
            resources.ApplyResources(this.pnlSelection, "pnlSelection");
            this.pnlSelection.Name = "pnlSelection";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radioButton2);
            this.groupBox4.Controls.Add(this.radioButton1);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // radioButton2
            // 
            resources.ApplyResources(this.radioButton2, "radioButton2");
            this.radioButton2.Name = "radioButton2";
            this.toolTip1.SetToolTip(this.radioButton2, resources.GetString("radioButton2.ToolTip"));
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            resources.ApplyResources(this.radioButton1, "radioButton1");
            this.radioButton1.Checked = true;
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.TabStop = true;
            this.toolTip1.SetToolTip(this.radioButton1, resources.GetString("radioButton1.ToolTip"));
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.areaOffAtMotion);
            this.groupBox11.Controls.Add(this.radioButton3);
            this.groupBox11.Controls.Add(this.radioButton4);
            resources.ApplyResources(this.groupBox11, "groupBox11");
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.TabStop = false;
            // 
            // areaOffAtMotion
            // 
            resources.ApplyResources(this.areaOffAtMotion, "areaOffAtMotion");
            this.areaOffAtMotion.Name = "areaOffAtMotion";
            this.toolTip1.SetToolTip(this.areaOffAtMotion, resources.GetString("areaOffAtMotion.ToolTip"));
            this.areaOffAtMotion.UseVisualStyleBackColor = true;
            this.areaOffAtMotion.CheckedChanged += new System.EventHandler(this.areaOffAtMotion_CheckedChanged);
            // 
            // radioButton3
            // 
            resources.ApplyResources(this.radioButton3, "radioButton3");
            this.radioButton3.Name = "radioButton3";
            this.toolTip1.SetToolTip(this.radioButton3, resources.GetString("radioButton3.ToolTip"));
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            resources.ApplyResources(this.radioButton4, "radioButton4");
            this.radioButton4.Checked = true;
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.TabStop = true;
            this.toolTip1.SetToolTip(this.radioButton4, resources.GetString("radioButton4.ToolTip"));
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.button15);
            this.groupBox12.Controls.Add(this.drawModeOff);
            this.groupBox12.Controls.Add(this.drawModeOn);
            resources.ApplyResources(this.groupBox12, "groupBox12");
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.TabStop = false;
            // 
            // button15
            // 
            resources.ApplyResources(this.button15, "button15");
            this.button15.Name = "button15";
            this.toolTip1.SetToolTip(this.button15, resources.GetString("button15.ToolTip"));
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // drawModeOff
            // 
            resources.ApplyResources(this.drawModeOff, "drawModeOff");
            this.drawModeOff.Checked = true;
            this.drawModeOff.Name = "drawModeOff";
            this.drawModeOff.TabStop = true;
            this.drawModeOff.UseVisualStyleBackColor = true;
            // 
            // drawModeOn
            // 
            resources.ApplyResources(this.drawModeOn, "drawModeOn");
            this.drawModeOn.Name = "drawModeOn";
            this.toolTip1.SetToolTip(this.drawModeOn, resources.GetString("drawModeOn.ToolTip"));
            this.drawModeOn.UseVisualStyleBackColor = true;
            this.drawModeOn.CheckedChanged += new System.EventHandler(this.drawModeOn_CheckedChanged);
            // 
            // showSelection
            // 
            this.showSelection.Controls.Add(this.radioButton7);
            this.showSelection.Controls.Add(this.radioButton8);
            resources.ApplyResources(this.showSelection, "showSelection");
            this.showSelection.Name = "showSelection";
            this.showSelection.TabStop = false;
            // 
            // radioButton7
            // 
            resources.ApplyResources(this.radioButton7, "radioButton7");
            this.radioButton7.Checked = true;
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.TabStop = true;
            this.radioButton7.UseVisualStyleBackColor = true;
            // 
            // radioButton8
            // 
            resources.ApplyResources(this.radioButton8, "radioButton8");
            this.radioButton8.Name = "radioButton8";
            this.toolTip1.SetToolTip(this.radioButton8, resources.GetString("radioButton8.ToolTip"));
            this.radioButton8.UseVisualStyleBackColor = true;
            this.radioButton8.CheckedChanged += new System.EventHandler(this.radioButton8_CheckedChanged);
            // 
            // label33
            // 
            resources.ApplyResources(this.label33, "label33");
            this.label33.BackColor = System.Drawing.Color.Transparent;
            this.label33.ForeColor = System.Drawing.Color.Black;
            this.label33.Name = "label33";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtMess);
            this.groupBox1.Controls.Add(this.actCount);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // txtMess
            // 
            this.txtMess.BackColor = System.Drawing.SystemColors.Control;
            this.txtMess.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtMess, "txtMess");
            this.txtMess.ForeColor = System.Drawing.Color.RoyalBlue;
            this.txtMess.Name = "txtMess";
            // 
            // actCount
            // 
            this.actCount.BackColor = System.Drawing.SystemColors.Control;
            this.actCount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.actCount, "actCount");
            this.actCount.ForeColor = System.Drawing.Color.RoyalBlue;
            this.actCount.Name = "actCount";
            // 
            // calibrate
            // 
            resources.ApplyResources(this.calibrate, "calibrate");
            this.calibrate.Name = "calibrate";
            this.toolTip1.SetToolTip(this.calibrate, resources.GetString("calibrate.ToolTip"));
            this.calibrate.UseVisualStyleBackColor = true;
            this.calibrate.Click += new System.EventHandler(this.calibrate_Click);
            // 
            // button8
            // 
            resources.ApplyResources(this.button8, "button8");
            this.button8.Name = "button8";
            this.toolTip1.SetToolTip(this.button8, resources.GetString("button8.ToolTip"));
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // camName
            // 
            this.camName.BackColor = System.Drawing.Color.LemonChiffon;
            resources.ApplyResources(this.camName, "camName");
            this.camName.Name = "camName";
            this.toolTip1.SetToolTip(this.camName, resources.GetString("camName.ToolTip"));
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
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.button5, "button5");
            this.button5.Name = "button5";
            this.toolTip1.SetToolTip(this.button5, resources.GetString("button5.ToolTip"));
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            resources.ApplyResources(this.button4, "button4");
            this.button4.Name = "button4";
            this.toolTip1.SetToolTip(this.button4, resources.GetString("button4.ToolTip"));
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.toolTip1.SetToolTip(this.button3, resources.GetString("button3.ToolTip"));
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.toolTip1.SetToolTip(this.button2, resources.GetString("button2.ToolTip"));
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.camDown);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.toolTip1.SetToolTip(this.button1, resources.GetString("button1.ToolTip"));
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.camUp);
            // 
            // bttncam9
            // 
            this.bttncam9.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.bttncam9, "bttncam9");
            this.bttncam9.Name = "bttncam9";
            this.toolTip1.SetToolTip(this.bttncam9, resources.GetString("bttncam9.ToolTip"));
            this.bttncam9.UseVisualStyleBackColor = false;
            this.bttncam9.Click += new System.EventHandler(this.bttncam9_Click);
            // 
            // bttncam7
            // 
            this.bttncam7.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.bttncam7, "bttncam7");
            this.bttncam7.Name = "bttncam7";
            this.toolTip1.SetToolTip(this.bttncam7, resources.GetString("bttncam7.ToolTip"));
            this.bttncam7.UseVisualStyleBackColor = false;
            this.bttncam7.Click += new System.EventHandler(this.bttncam7_Click);
            // 
            // bttncam8
            // 
            this.bttncam8.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.bttncam8, "bttncam8");
            this.bttncam8.Name = "bttncam8";
            this.toolTip1.SetToolTip(this.bttncam8, resources.GetString("bttncam8.ToolTip"));
            this.bttncam8.UseVisualStyleBackColor = false;
            this.bttncam8.Click += new System.EventHandler(this.bttncam8_Click);
            // 
            // bttncam6
            // 
            this.bttncam6.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.bttncam6, "bttncam6");
            this.bttncam6.Name = "bttncam6";
            this.toolTip1.SetToolTip(this.bttncam6, resources.GetString("bttncam6.ToolTip"));
            this.bttncam6.UseVisualStyleBackColor = false;
            this.bttncam6.Click += new System.EventHandler(this.bttncam6_Click);
            // 
            // bttncam1
            // 
            this.bttncam1.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.bttncam1, "bttncam1");
            this.bttncam1.Name = "bttncam1";
            this.toolTip1.SetToolTip(this.bttncam1, resources.GetString("bttncam1.ToolTip"));
            this.bttncam1.UseVisualStyleBackColor = false;
            this.bttncam1.Click += new System.EventHandler(this.bttncam1_Click);
            // 
            // bttncam4
            // 
            this.bttncam4.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.bttncam4, "bttncam4");
            this.bttncam4.Name = "bttncam4";
            this.toolTip1.SetToolTip(this.bttncam4, resources.GetString("bttncam4.ToolTip"));
            this.bttncam4.UseVisualStyleBackColor = false;
            this.bttncam4.Click += new System.EventHandler(this.bttncam4_Click);
            // 
            // bttncam2
            // 
            this.bttncam2.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.bttncam2, "bttncam2");
            this.bttncam2.Name = "bttncam2";
            this.toolTip1.SetToolTip(this.bttncam2, resources.GetString("bttncam2.ToolTip"));
            this.bttncam2.UseVisualStyleBackColor = false;
            this.bttncam2.Click += new System.EventHandler(this.bttncam2_Click);
            // 
            // bttncam5
            // 
            this.bttncam5.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.bttncam5, "bttncam5");
            this.bttncam5.Name = "bttncam5";
            this.toolTip1.SetToolTip(this.bttncam5, resources.GetString("bttncam5.ToolTip"));
            this.bttncam5.UseVisualStyleBackColor = false;
            this.bttncam5.Click += new System.EventHandler(this.bttncam5_Click);
            // 
            // bttncam3
            // 
            this.bttncam3.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.bttncam3, "bttncam3");
            this.bttncam3.Name = "bttncam3";
            this.toolTip1.SetToolTip(this.bttncam3, resources.GetString("bttncam3.ToolTip"));
            this.bttncam3.UseVisualStyleBackColor = false;
            this.bttncam3.Click += new System.EventHandler(this.bttncam3_Click);
            // 
            // panel3
            // 
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Controls.Add(this.cameraWindow);
            this.panel3.Name = "panel3";
            // 
            // cameraWindow
            // 
            this.cameraWindow.BackColor = System.Drawing.SystemColors.Control;
            this.cameraWindow.Camera = null;
            resources.ApplyResources(this.cameraWindow, "cameraWindow");
            this.cameraWindow.Name = "cameraWindow";
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
            resources.ApplyResources(this.levelbox, "levelbox");
            this.levelbox.Name = "levelbox";
            this.levelbox.TabStop = false;
            this.toolTip1.SetToolTip(this.levelbox, resources.GetString("levelbox.ToolTip"));
            this.levelbox.Paint += new System.Windows.Forms.PaintEventHandler(this.levelbox_Paint);
            // 
            // label38
            // 
            resources.ApplyResources(this.label38, "label38");
            this.label38.Name = "label38";
            // 
            // label37
            // 
            resources.ApplyResources(this.label37, "label37");
            this.label37.Name = "label37";
            // 
            // label31
            // 
            resources.ApplyResources(this.label31, "label31");
            this.label31.Name = "label31";
            // 
            // label30
            // 
            resources.ApplyResources(this.label30, "label30");
            this.label30.Name = "label30";
            // 
            // label29
            // 
            resources.ApplyResources(this.label29, "label29");
            this.label29.Name = "label29";
            // 
            // trkMov
            // 
            resources.ApplyResources(this.trkMov, "trkMov");
            this.trkMov.Maximum = 100;
            this.trkMov.Minimum = 1;
            this.trkMov.Name = "trkMov";
            this.trkMov.TickFrequency = 10;
            this.trkMov.Value = 1;
            this.trkMov.Scroll += new System.EventHandler(this.trkMov_Scroll);
            // 
            // webcamConfig
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
            this.Name = "webcamConfig";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.webcamConfig_FormClosing);
            this.Load += new System.EventHandler(this.webcamConfig_Load);
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