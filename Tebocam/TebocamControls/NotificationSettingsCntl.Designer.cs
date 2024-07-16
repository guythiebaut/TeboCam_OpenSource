namespace TeboCam
{
    partial class NotificationSettingsCntl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.captureMovementImages = new System.Windows.Forms.CheckBox();
            this.pingBox = new System.Windows.Forms.GroupBox();
            this.rdPingActiveCamera = new System.Windows.Forms.RadioButton();
            this.rdPingAllCameras = new System.Windows.Forms.RadioButton();
            this.ping = new System.Windows.Forms.CheckBox();
            this.pingMins = new System.Windows.Forms.TextBox();
            this.label40 = new System.Windows.Forms.Label();
            this.mosaicImagesPerRow = new System.Windows.Forms.TextBox();
            this.sendMosaic = new System.Windows.Forms.CheckBox();
            this.sndTest = new System.Windows.Forms.Button();
            this.SelectSoundBtn = new System.Windows.Forms.Button();
            this.plSnd = new System.Windows.Forms.CheckBox();
            this.label33 = new System.Windows.Forms.Label();
            this.sendEmail = new System.Windows.Forms.CheckBox();
            this.maxImagesToEmail = new System.Windows.Forms.TextBox();
            this.sendFullSize = new System.Windows.Forms.CheckBox();
            this.label19 = new System.Windows.Forms.Label();
            this.sendThumb = new System.Windows.Forms.CheckBox();
            this.label20 = new System.Windows.Forms.Label();
            this.loadToFtp = new System.Windows.Forms.CheckBox();
            this.label32 = new System.Windows.Forms.Label();
            this.groupBox14.SuspendLayout();
            this.pingBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.captureMovementImages);
            this.groupBox14.Controls.Add(this.pingBox);
            this.groupBox14.Controls.Add(this.mosaicImagesPerRow);
            this.groupBox14.Controls.Add(this.sendMosaic);
            this.groupBox14.Controls.Add(this.sndTest);
            this.groupBox14.Controls.Add(this.SelectSoundBtn);
            this.groupBox14.Controls.Add(this.plSnd);
            this.groupBox14.Controls.Add(this.label33);
            this.groupBox14.Controls.Add(this.sendEmail);
            this.groupBox14.Controls.Add(this.maxImagesToEmail);
            this.groupBox14.Controls.Add(this.sendFullSize);
            this.groupBox14.Controls.Add(this.label19);
            this.groupBox14.Controls.Add(this.sendThumb);
            this.groupBox14.Controls.Add(this.label20);
            this.groupBox14.Controls.Add(this.loadToFtp);
            this.groupBox14.Controls.Add(this.label32);
            this.groupBox14.Location = new System.Drawing.Point(0, 0);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(295, 375);
            this.groupBox14.TabIndex = 60;
            this.groupBox14.TabStop = false;
            // 
            // captureMovementImages
            // 
            this.captureMovementImages.AutoSize = true;
            this.captureMovementImages.BackColor = System.Drawing.SystemColors.Control;
            this.captureMovementImages.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.captureMovementImages.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.captureMovementImages.Location = new System.Drawing.Point(20, 58);
            this.captureMovementImages.Name = "captureMovementImages";
            this.captureMovementImages.Size = new System.Drawing.Size(201, 17);
            this.captureMovementImages.TabIndex = 66;
            this.captureMovementImages.Text = "Capture movement images";
            this.captureMovementImages.UseVisualStyleBackColor = false;
            this.captureMovementImages.CheckedChanged += new System.EventHandler(this.captureMovementImages_CheckedChanged);
            // 
            // pingBox
            // 
            this.pingBox.Controls.Add(this.rdPingActiveCamera);
            this.pingBox.Controls.Add(this.rdPingAllCameras);
            this.pingBox.Controls.Add(this.ping);
            this.pingBox.Controls.Add(this.pingMins);
            this.pingBox.Controls.Add(this.label40);
            this.pingBox.Location = new System.Drawing.Point(19, 281);
            this.pingBox.Name = "pingBox";
            this.pingBox.Size = new System.Drawing.Size(251, 86);
            this.pingBox.TabIndex = 65;
            this.pingBox.TabStop = false;
            this.pingBox.Text = "Ping";
            // 
            // rdPingActiveCamera
            // 
            this.rdPingActiveCamera.AutoSize = true;
            this.rdPingActiveCamera.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rdPingActiveCamera.Location = new System.Drawing.Point(121, 56);
            this.rdPingActiveCamera.Name = "rdPingActiveCamera";
            this.rdPingActiveCamera.Size = new System.Drawing.Size(94, 17);
            this.rdPingActiveCamera.TabIndex = 53;
            this.rdPingActiveCamera.Text = "Active Camera";
            this.rdPingActiveCamera.UseVisualStyleBackColor = true;
            // 
            // rdPingAllCameras
            // 
            this.rdPingAllCameras.AutoSize = true;
            this.rdPingAllCameras.Checked = true;
            this.rdPingAllCameras.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rdPingAllCameras.Location = new System.Drawing.Point(6, 56);
            this.rdPingAllCameras.Name = "rdPingAllCameras";
            this.rdPingAllCameras.Size = new System.Drawing.Size(80, 17);
            this.rdPingAllCameras.TabIndex = 52;
            this.rdPingAllCameras.TabStop = true;
            this.rdPingAllCameras.Text = "All Cameras";
            this.rdPingAllCameras.UseVisualStyleBackColor = true;
            this.rdPingAllCameras.CheckedChanged += new System.EventHandler(this.rdPingAllCameras_CheckedChanged);
            // 
            // ping
            // 
            this.ping.AutoSize = true;
            this.ping.BackColor = System.Drawing.SystemColors.Control;
            this.ping.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.ping.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ping.Location = new System.Drawing.Point(6, 24);
            this.ping.Name = "ping";
            this.ping.Size = new System.Drawing.Size(96, 17);
            this.ping.TabIndex = 49;
            this.ping.Text = "Ping Every";
            this.ping.UseVisualStyleBackColor = false;
            this.ping.CheckedChanged += new System.EventHandler(this.ping_CheckedChanged);
            // 
            // pingMins
            // 
            this.pingMins.BackColor = System.Drawing.Color.LemonChiffon;
            this.pingMins.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pingMins.ForeColor = System.Drawing.SystemColors.WindowText;
            this.pingMins.Location = new System.Drawing.Point(110, 22);
            this.pingMins.Name = "pingMins";
            this.pingMins.Size = new System.Drawing.Size(48, 21);
            this.pingMins.TabIndex = 50;
            this.pingMins.Leave += new System.EventHandler(this.pingMins_Leave);
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label40.ForeColor = System.Drawing.Color.Black;
            this.label40.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label40.Location = new System.Drawing.Point(164, 28);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(57, 13);
            this.label40.TabIndex = 51;
            this.label40.Text = "Minutes";
            // 
            // mosaicImagesPerRow
            // 
            this.mosaicImagesPerRow.BackColor = System.Drawing.Color.LemonChiffon;
            this.mosaicImagesPerRow.Enabled = false;
            this.mosaicImagesPerRow.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.mosaicImagesPerRow.ForeColor = System.Drawing.SystemColors.WindowText;
            this.mosaicImagesPerRow.Location = new System.Drawing.Point(162, 148);
            this.mosaicImagesPerRow.Name = "mosaicImagesPerRow";
            this.mosaicImagesPerRow.Size = new System.Drawing.Size(48, 21);
            this.mosaicImagesPerRow.TabIndex = 62;
            this.mosaicImagesPerRow.Text = "10";
            this.mosaicImagesPerRow.Leave += new System.EventHandler(this.mosaicImagesPerRow_Leave);
            // 
            // sendMosaic
            // 
            this.sendMosaic.AutoSize = true;
            this.sendMosaic.BackColor = System.Drawing.SystemColors.Control;
            this.sendMosaic.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.sendMosaic.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.sendMosaic.Location = new System.Drawing.Point(19, 149);
            this.sendMosaic.Name = "sendMosaic";
            this.sendMosaic.Size = new System.Drawing.Size(110, 17);
            this.sendMosaic.TabIndex = 61;
            this.sendMosaic.Text = "Email Mosaic";
            this.sendMosaic.UseVisualStyleBackColor = false;
            this.sendMosaic.CheckedChanged += new System.EventHandler(this.sendMosaic_CheckedChanged);
            // 
            // sndTest
            // 
            this.sndTest.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.sndTest.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.sndTest.Location = new System.Drawing.Point(242, 197);
            this.sndTest.Name = "sndTest";
            this.sndTest.Size = new System.Drawing.Size(42, 21);
            this.sndTest.TabIndex = 60;
            this.sndTest.Text = "Test";
            this.sndTest.UseVisualStyleBackColor = true;
            this.sndTest.Click += new System.EventHandler(this.sndTest_Click);
            // 
            // SelectSoundBtn
            // 
            this.SelectSoundBtn.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.SelectSoundBtn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SelectSoundBtn.Location = new System.Drawing.Point(123, 197);
            this.SelectSoundBtn.Name = "SelectSoundBtn";
            this.SelectSoundBtn.Size = new System.Drawing.Size(113, 21);
            this.SelectSoundBtn.TabIndex = 59;
            this.SelectSoundBtn.Text = "Select Sound...";
            this.SelectSoundBtn.UseVisualStyleBackColor = true;
            this.SelectSoundBtn.Click += new System.EventHandler(this.SelectSoundCntl_Click);
            // 
            // plSnd
            // 
            this.plSnd.AutoSize = true;
            this.plSnd.BackColor = System.Drawing.SystemColors.Control;
            this.plSnd.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.plSnd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.plSnd.Location = new System.Drawing.Point(20, 200);
            this.plSnd.Name = "plSnd";
            this.plSnd.Size = new System.Drawing.Size(98, 17);
            this.plSnd.TabIndex = 58;
            this.plSnd.Text = "Play Sound";
            this.plSnd.UseVisualStyleBackColor = false;
            this.plSnd.CheckedChanged += new System.EventHandler(this.plSnd_CheckedChanged);
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label33.ForeColor = System.Drawing.Color.Black;
            this.label33.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label33.Location = new System.Drawing.Point(53, 35);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(161, 13);
            this.label33.TabIndex = 48;
            this.label33.Text = "On Movement Detection";
            // 
            // sendEmail
            // 
            this.sendEmail.AutoSize = true;
            this.sendEmail.BackColor = System.Drawing.SystemColors.Control;
            this.sendEmail.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.sendEmail.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.sendEmail.Location = new System.Drawing.Point(20, 80);
            this.sendEmail.Name = "sendEmail";
            this.sendEmail.Size = new System.Drawing.Size(177, 17);
            this.sendEmail.TabIndex = 0;
            this.sendEmail.Text = "Send Notification Email";
            this.sendEmail.UseVisualStyleBackColor = false;
            this.sendEmail.CheckedChanged += new System.EventHandler(this.sendEmail_CheckedChanged);
            // 
            // maxImagesToEmail
            // 
            this.maxImagesToEmail.BackColor = System.Drawing.Color.LemonChiffon;
            this.maxImagesToEmail.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.maxImagesToEmail.ForeColor = System.Drawing.SystemColors.WindowText;
            this.maxImagesToEmail.Location = new System.Drawing.Point(22, 253);
            this.maxImagesToEmail.Name = "maxImagesToEmail";
            this.maxImagesToEmail.Size = new System.Drawing.Size(65, 21);
            this.maxImagesToEmail.TabIndex = 55;
            this.maxImagesToEmail.Text = "10";
            this.maxImagesToEmail.Leave += new System.EventHandler(this.maxImagesToEmail_Leave);
            // 
            // sendFullSize
            // 
            this.sendFullSize.AutoSize = true;
            this.sendFullSize.BackColor = System.Drawing.SystemColors.Control;
            this.sendFullSize.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.sendFullSize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.sendFullSize.Location = new System.Drawing.Point(20, 103);
            this.sendFullSize.Name = "sendFullSize";
            this.sendFullSize.Size = new System.Drawing.Size(182, 17);
            this.sendFullSize.TabIndex = 1;
            this.sendFullSize.Text = "Email Full Sized Images";
            this.sendFullSize.UseVisualStyleBackColor = false;
            this.sendFullSize.CheckedChanged += new System.EventHandler(this.sendFullSize_CheckedChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label19.ForeColor = System.Drawing.Color.Black;
            this.label19.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label19.Location = new System.Drawing.Point(19, 237);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(142, 13);
            this.label19.TabIndex = 57;
            this.label19.Text = "To send in one email";
            // 
            // sendThumb
            // 
            this.sendThumb.AutoSize = true;
            this.sendThumb.BackColor = System.Drawing.SystemColors.Control;
            this.sendThumb.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.sendThumb.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.sendThumb.Location = new System.Drawing.Point(20, 126);
            this.sendThumb.Name = "sendThumb";
            this.sendThumb.Size = new System.Drawing.Size(141, 17);
            this.sendThumb.TabIndex = 2;
            this.sendThumb.Text = "Email Thumbnails";
            this.sendThumb.UseVisualStyleBackColor = false;
            this.sendThumb.CheckedChanged += new System.EventHandler(this.sendThumb_CheckedChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label20.ForeColor = System.Drawing.Color.Black;
            this.label20.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label20.Location = new System.Drawing.Point(19, 224);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(191, 13);
            this.label20.TabIndex = 56;
            this.label20.Text = "Maximum number of images";
            // 
            // loadToFtp
            // 
            this.loadToFtp.AutoSize = true;
            this.loadToFtp.BackColor = System.Drawing.SystemColors.Control;
            this.loadToFtp.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.loadToFtp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.loadToFtp.Location = new System.Drawing.Point(20, 175);
            this.loadToFtp.Name = "loadToFtp";
            this.loadToFtp.Size = new System.Drawing.Size(186, 17);
            this.loadToFtp.TabIndex = 3;
            this.loadToFtp.Text = "Load Images To Website";
            this.loadToFtp.UseVisualStyleBackColor = false;
            this.loadToFtp.CheckedChanged += new System.EventHandler(this.loadToFtp_CheckedChanged);
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold);
            this.label32.ForeColor = System.Drawing.Color.Black;
            this.label32.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label32.Location = new System.Drawing.Point(16, 12);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(234, 23);
            this.label32.TabIndex = 47;
            this.label32.Text = "Notification Settings";
            // 
            // NotificationSettingsCntl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox14);
            this.Name = "NotificationSettingsCntl";
            this.Size = new System.Drawing.Size(295, 375);
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.pingBox.ResumeLayout(false);
            this.pingBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.CheckBox captureMovementImages;
        private System.Windows.Forms.GroupBox pingBox;
        private System.Windows.Forms.RadioButton rdPingActiveCamera;
        private System.Windows.Forms.RadioButton rdPingAllCameras;
        private System.Windows.Forms.CheckBox ping;
        private System.Windows.Forms.TextBox pingMins;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.TextBox mosaicImagesPerRow;
        private System.Windows.Forms.CheckBox sendMosaic;
        private System.Windows.Forms.Button sndTest;
        private System.Windows.Forms.Button SelectSoundBtn;
        private System.Windows.Forms.CheckBox plSnd;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.CheckBox sendEmail;
        private System.Windows.Forms.TextBox maxImagesToEmail;
        private System.Windows.Forms.CheckBox sendFullSize;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.CheckBox sendThumb;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.CheckBox loadToFtp;
        private System.Windows.Forms.Label label32;
    }
}
