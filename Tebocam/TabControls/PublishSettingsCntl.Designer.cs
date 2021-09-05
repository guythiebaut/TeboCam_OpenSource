namespace TeboCam
{
    partial class PublishSettingsCntl
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
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.lblendpub = new System.Windows.Forms.Label();
            this.lblstartpub = new System.Windows.Forms.Label();
            this.bttnSetPublishSchedule = new System.Windows.Forms.Button();
            this.bttnSetLocalFileName = new System.Windows.Forms.Button();
            this.bttnSetPrefixPublish = new System.Windows.Forms.Button();
            this.pubToWeb = new System.Windows.Forms.CheckBox();
            this.pubToLocal = new System.Windows.Forms.CheckBox();
            this.pubTimerOn = new System.Windows.Forms.CheckBox();
            this.bttnCopyFtpSettings = new System.Windows.Forms.Button();
            this.label55 = new System.Windows.Forms.Label();
            this.pubTime = new System.Windows.Forms.TextBox();
            this.pubSecs = new System.Windows.Forms.RadioButton();
            this.pubMins = new System.Windows.Forms.RadioButton();
            this.pubHours = new System.Windows.Forms.RadioButton();
            this.pubFtpRoot = new System.Windows.Forms.TextBox();
            this.label59 = new System.Windows.Forms.Label();
            this.pubFtpPass = new System.Windows.Forms.TextBox();
            this.label60 = new System.Windows.Forms.Label();
            this.pubFtpUser = new System.Windows.Forms.TextBox();
            this.label61 = new System.Windows.Forms.Label();
            this.pubImage = new System.Windows.Forms.CheckBox();
            this.groupBox17.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.lblendpub);
            this.groupBox17.Controls.Add(this.lblstartpub);
            this.groupBox17.Controls.Add(this.bttnSetPublishSchedule);
            this.groupBox17.Controls.Add(this.bttnSetLocalFileName);
            this.groupBox17.Controls.Add(this.bttnSetPrefixPublish);
            this.groupBox17.Controls.Add(this.pubToWeb);
            this.groupBox17.Controls.Add(this.pubToLocal);
            this.groupBox17.Controls.Add(this.pubTimerOn);
            this.groupBox17.Controls.Add(this.bttnCopyFtpSettings);
            this.groupBox17.Controls.Add(this.label55);
            this.groupBox17.Controls.Add(this.pubTime);
            this.groupBox17.Controls.Add(this.pubSecs);
            this.groupBox17.Controls.Add(this.pubMins);
            this.groupBox17.Controls.Add(this.pubHours);
            this.groupBox17.Controls.Add(this.pubFtpRoot);
            this.groupBox17.Controls.Add(this.label59);
            this.groupBox17.Controls.Add(this.pubFtpPass);
            this.groupBox17.Controls.Add(this.label60);
            this.groupBox17.Controls.Add(this.pubFtpUser);
            this.groupBox17.Controls.Add(this.label61);
            this.groupBox17.Controls.Add(this.pubImage);
            this.groupBox17.Location = new System.Drawing.Point(0, 0);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(375, 439);
            this.groupBox17.TabIndex = 62;
            this.groupBox17.TabStop = false;
            this.groupBox17.Enter += new System.EventHandler(this.groupBox17_Enter);
            // 
            // lblendpub
            // 
            this.lblendpub.AutoSize = true;
            this.lblendpub.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblendpub.ForeColor = System.Drawing.Color.Black;
            this.lblendpub.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblendpub.Location = new System.Drawing.Point(171, 178);
            this.lblendpub.Name = "lblendpub";
            this.lblendpub.Size = new System.Drawing.Size(106, 13);
            this.lblendpub.TabIndex = 98;
            this.lblendpub.Text = "Scheduled end:";
            // 
            // lblstartpub
            // 
            this.lblstartpub.AutoSize = true;
            this.lblstartpub.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblstartpub.ForeColor = System.Drawing.Color.Black;
            this.lblstartpub.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblstartpub.Location = new System.Drawing.Point(8, 178);
            this.lblstartpub.Name = "lblstartpub";
            this.lblstartpub.Size = new System.Drawing.Size(117, 13);
            this.lblstartpub.TabIndex = 97;
            this.lblstartpub.Text = "Scheduled start: ";
            // 
            // bttnSetPublishSchedule
            // 
            this.bttnSetPublishSchedule.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bttnSetPublishSchedule.Location = new System.Drawing.Point(5, 201);
            this.bttnSetPublishSchedule.Name = "bttnSetPublishSchedule";
            this.bttnSetPublishSchedule.Size = new System.Drawing.Size(345, 26);
            this.bttnSetPublishSchedule.TabIndex = 96;
            this.bttnSetPublishSchedule.Text = "Set Publish Schedule";
            this.bttnSetPublishSchedule.UseVisualStyleBackColor = true;
            this.bttnSetPublishSchedule.Click += new System.EventHandler(this.button37_Click);
            // 
            // bttnSetLocalFileName
            // 
            this.bttnSetLocalFileName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bttnSetLocalFileName.Location = new System.Drawing.Point(132, 124);
            this.bttnSetLocalFileName.Name = "bttnSetLocalFileName";
            this.bttnSetLocalFileName.Size = new System.Drawing.Size(152, 23);
            this.bttnSetLocalFileName.TabIndex = 95;
            this.bttnSetLocalFileName.Text = "Set Local File Name";
            this.bttnSetLocalFileName.UseVisualStyleBackColor = true;
            this.bttnSetLocalFileName.Click += new System.EventHandler(this.bttnSetLocalFileName_Click);
            // 
            // bttnSetPrefixPublish
            // 
            this.bttnSetPrefixPublish.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bttnSetPrefixPublish.Location = new System.Drawing.Point(132, 97);
            this.bttnSetPrefixPublish.Name = "bttnSetPrefixPublish";
            this.bttnSetPrefixPublish.Size = new System.Drawing.Size(152, 23);
            this.bttnSetPrefixPublish.TabIndex = 94;
            this.bttnSetPrefixPublish.Text = "Set Web File Name";
            this.bttnSetPrefixPublish.UseVisualStyleBackColor = true;
            this.bttnSetPrefixPublish.Click += new System.EventHandler(this.bttnSetPrefixPublish_Click);
            // 
            // pubToWeb
            // 
            this.pubToWeb.AutoSize = true;
            this.pubToWeb.BackColor = System.Drawing.SystemColors.Control;
            this.pubToWeb.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pubToWeb.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pubToWeb.Location = new System.Drawing.Point(6, 98);
            this.pubToWeb.Name = "pubToWeb";
            this.pubToWeb.Size = new System.Drawing.Size(120, 17);
            this.pubToWeb.TabIndex = 70;
            this.pubToWeb.Text = "Publish to web";
            this.pubToWeb.UseVisualStyleBackColor = false;
            this.pubToWeb.CheckedChanged += new System.EventHandler(this.pubToWeb_CheckedChanged);
            // 
            // pubToLocal
            // 
            this.pubToLocal.AutoSize = true;
            this.pubToLocal.BackColor = System.Drawing.SystemColors.Control;
            this.pubToLocal.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pubToLocal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pubToLocal.Location = new System.Drawing.Point(6, 126);
            this.pubToLocal.Name = "pubToLocal";
            this.pubToLocal.Size = new System.Drawing.Size(120, 17);
            this.pubToLocal.TabIndex = 69;
            this.pubToLocal.Text = "Publish locally";
            this.pubToLocal.UseVisualStyleBackColor = false;
            this.pubToLocal.CheckedChanged += new System.EventHandler(this.pubToLocal_CheckedChanged);
            // 
            // pubTimerOn
            // 
            this.pubTimerOn.AutoSize = true;
            this.pubTimerOn.BackColor = System.Drawing.SystemColors.Control;
            this.pubTimerOn.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pubTimerOn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pubTimerOn.Location = new System.Drawing.Point(6, 153);
            this.pubTimerOn.Name = "pubTimerOn";
            this.pubTimerOn.Size = new System.Drawing.Size(301, 17);
            this.pubTimerOn.TabIndex = 67;
            this.pubTimerOn.Text = "Switch on/off publishing at time set below";
            this.pubTimerOn.UseVisualStyleBackColor = false;
            this.pubTimerOn.CheckedChanged += new System.EventHandler(this.pubTimerOn_CheckedChanged);
            // 
            // bttnCopyFtpSettings
            // 
            this.bttnCopyFtpSettings.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bttnCopyFtpSettings.Location = new System.Drawing.Point(6, 236);
            this.bttnCopyFtpSettings.Name = "bttnCopyFtpSettings";
            this.bttnCopyFtpSettings.Size = new System.Drawing.Size(344, 25);
            this.bttnCopyFtpSettings.TabIndex = 62;
            this.bttnCopyFtpSettings.Text = "Copy FTP Settings from Email and Ftp";
            this.bttnCopyFtpSettings.UseVisualStyleBackColor = true;
            this.bttnCopyFtpSettings.Click += new System.EventHandler(this.button14_Click);
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label55.ForeColor = System.Drawing.Color.Black;
            this.label55.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label55.Location = new System.Drawing.Point(11, 75);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(45, 13);
            this.label55.TabIndex = 61;
            this.label55.Text = "Every";
            // 
            // pubTime
            // 
            this.pubTime.BackColor = System.Drawing.Color.LemonChiffon;
            this.pubTime.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pubTime.ForeColor = System.Drawing.SystemColors.WindowText;
            this.pubTime.Location = new System.Drawing.Point(66, 72);
            this.pubTime.Name = "pubTime";
            this.pubTime.Size = new System.Drawing.Size(65, 21);
            this.pubTime.TabIndex = 57;
            this.pubTime.Text = "2";
            this.pubTime.Leave += new System.EventHandler(this.pubTime_Leave);
            // 
            // pubSecs
            // 
            this.pubSecs.AutoSize = true;
            this.pubSecs.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pubSecs.Location = new System.Drawing.Point(287, 75);
            this.pubSecs.Name = "pubSecs";
            this.pubSecs.Size = new System.Drawing.Size(49, 17);
            this.pubSecs.TabIndex = 56;
            this.pubSecs.Text = "Secs";
            this.pubSecs.UseVisualStyleBackColor = true;
            this.pubSecs.CheckedChanged += new System.EventHandler(this.pubSecs_CheckedChanged);
            // 
            // pubMins
            // 
            this.pubMins.AutoSize = true;
            this.pubMins.Checked = true;
            this.pubMins.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pubMins.Location = new System.Drawing.Point(215, 75);
            this.pubMins.Name = "pubMins";
            this.pubMins.Size = new System.Drawing.Size(47, 17);
            this.pubMins.TabIndex = 55;
            this.pubMins.TabStop = true;
            this.pubMins.Text = "Mins";
            this.pubMins.UseVisualStyleBackColor = true;
            this.pubMins.CheckedChanged += new System.EventHandler(this.pubMins_CheckedChanged);
            // 
            // pubHours
            // 
            this.pubHours.AutoSize = true;
            this.pubHours.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pubHours.Location = new System.Drawing.Point(144, 75);
            this.pubHours.Name = "pubHours";
            this.pubHours.Size = new System.Drawing.Size(53, 17);
            this.pubHours.TabIndex = 54;
            this.pubHours.Text = "Hours";
            this.pubHours.UseVisualStyleBackColor = true;
            this.pubHours.CheckedChanged += new System.EventHandler(this.pubHours_CheckedChanged);
            // 
            // pubFtpRoot
            // 
            this.pubFtpRoot.BackColor = System.Drawing.Color.LemonChiffon;
            this.pubFtpRoot.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pubFtpRoot.ForeColor = System.Drawing.SystemColors.WindowText;
            this.pubFtpRoot.Location = new System.Drawing.Point(6, 377);
            this.pubFtpRoot.Name = "pubFtpRoot";
            this.pubFtpRoot.Size = new System.Drawing.Size(344, 21);
            this.pubFtpRoot.TabIndex = 33;
            this.pubFtpRoot.TextChanged += new System.EventHandler(this.pubFtpRoot_TextChanged);
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label59.ForeColor = System.Drawing.Color.Black;
            this.label59.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label59.Location = new System.Drawing.Point(2, 361);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(126, 13);
            this.label59.TabIndex = 36;
            this.label59.Text = "Ftp Root Directory";
            // 
            // pubFtpPass
            // 
            this.pubFtpPass.BackColor = System.Drawing.Color.LemonChiffon;
            this.pubFtpPass.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pubFtpPass.ForeColor = System.Drawing.SystemColors.WindowText;
            this.pubFtpPass.Location = new System.Drawing.Point(6, 330);
            this.pubFtpPass.Name = "pubFtpPass";
            this.pubFtpPass.PasswordChar = '*';
            this.pubFtpPass.Size = new System.Drawing.Size(344, 21);
            this.pubFtpPass.TabIndex = 32;
            this.pubFtpPass.TextChanged += new System.EventHandler(this.pubFtpPass_TextChanged);
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label60.ForeColor = System.Drawing.Color.Black;
            this.label60.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label60.Location = new System.Drawing.Point(2, 314);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(94, 13);
            this.label60.TabIndex = 35;
            this.label60.Text = "Ftp Password";
            // 
            // pubFtpUser
            // 
            this.pubFtpUser.BackColor = System.Drawing.Color.LemonChiffon;
            this.pubFtpUser.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pubFtpUser.ForeColor = System.Drawing.SystemColors.WindowText;
            this.pubFtpUser.Location = new System.Drawing.Point(6, 290);
            this.pubFtpUser.Name = "pubFtpUser";
            this.pubFtpUser.Size = new System.Drawing.Size(344, 21);
            this.pubFtpUser.TabIndex = 31;
            this.pubFtpUser.TextChanged += new System.EventHandler(this.pubFtpUser_TextChanged);
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label61.ForeColor = System.Drawing.Color.Black;
            this.label61.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label61.Location = new System.Drawing.Point(2, 264);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(62, 13);
            this.label61.TabIndex = 34;
            this.label61.Text = "Ftp User";
            // 
            // pubImage
            // 
            this.pubImage.AutoSize = true;
            this.pubImage.BackColor = System.Drawing.SystemColors.Control;
            this.pubImage.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pubImage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pubImage.Location = new System.Drawing.Point(5, 49);
            this.pubImage.Name = "pubImage";
            this.pubImage.Size = new System.Drawing.Size(178, 17);
            this.pubImage.TabIndex = 1;
            this.pubImage.Text = "Publish Webcam Image";
            this.pubImage.UseVisualStyleBackColor = false;
            this.pubImage.CheckedChanged += new System.EventHandler(this.pubImage_CheckedChanged);
            // 
            // PublishSettingsCntl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox17);
            this.Name = "PublishSettingsCntl";
            this.Size = new System.Drawing.Size(375, 439);
            this.groupBox17.ResumeLayout(false);
            this.groupBox17.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox17;
        private System.Windows.Forms.Label lblendpub;
        private System.Windows.Forms.Label lblstartpub;
        private System.Windows.Forms.Button bttnSetPublishSchedule;
        private System.Windows.Forms.Button bttnSetLocalFileName;
        private System.Windows.Forms.Button bttnSetPrefixPublish;
        private System.Windows.Forms.CheckBox pubToWeb;
        private System.Windows.Forms.CheckBox pubToLocal;
        private System.Windows.Forms.CheckBox pubTimerOn;
        private System.Windows.Forms.Button bttnCopyFtpSettings;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.TextBox pubTime;
        private System.Windows.Forms.RadioButton pubSecs;
        private System.Windows.Forms.RadioButton pubMins;
        private System.Windows.Forms.RadioButton pubHours;
        private System.Windows.Forms.TextBox pubFtpRoot;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.TextBox pubFtpPass;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.TextBox pubFtpUser;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.CheckBox pubImage;
    }
}
