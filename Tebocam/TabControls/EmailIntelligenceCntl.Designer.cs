namespace TeboCam
{
    partial class EmailIntelligenceCntl
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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.emailIntelPanel = new System.Windows.Forms.Panel();
            this.EmailIntelMosaic = new System.Windows.Forms.RadioButton();
            this.label16 = new System.Windows.Forms.Label();
            this.emailIntelEmails = new System.Windows.Forms.TextBox();
            this.EmailIntelStop = new System.Windows.Forms.RadioButton();
            this.label48 = new System.Windows.Forms.Label();
            this.emailIntelMins = new System.Windows.Forms.TextBox();
            this.label56 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.EmailIntelOff = new System.Windows.Forms.RadioButton();
            this.EmailIntelOn = new System.Windows.Forms.RadioButton();
            this.groupBox4.SuspendLayout();
            this.emailIntelPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.emailIntelPanel);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.EmailIntelOff);
            this.groupBox4.Controls.Add(this.EmailIntelOn);
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(418, 194);
            this.groupBox4.TabIndex = 60;
            this.groupBox4.TabStop = false;
            // 
            // emailIntelPanel
            // 
            this.emailIntelPanel.Controls.Add(this.EmailIntelMosaic);
            this.emailIntelPanel.Controls.Add(this.label16);
            this.emailIntelPanel.Controls.Add(this.emailIntelEmails);
            this.emailIntelPanel.Controls.Add(this.EmailIntelStop);
            this.emailIntelPanel.Controls.Add(this.label48);
            this.emailIntelPanel.Controls.Add(this.emailIntelMins);
            this.emailIntelPanel.Controls.Add(this.label56);
            this.emailIntelPanel.Controls.Add(this.label49);
            this.emailIntelPanel.Enabled = false;
            this.emailIntelPanel.Location = new System.Drawing.Point(6, 64);
            this.emailIntelPanel.Name = "emailIntelPanel";
            this.emailIntelPanel.Size = new System.Drawing.Size(400, 117);
            this.emailIntelPanel.TabIndex = 65;
            // 
            // EmailIntelMosaic
            // 
            this.EmailIntelMosaic.AutoSize = true;
            this.EmailIntelMosaic.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.EmailIntelMosaic.Location = new System.Drawing.Point(15, 91);
            this.EmailIntelMosaic.Name = "EmailIntelMosaic";
            this.EmailIntelMosaic.Size = new System.Drawing.Size(254, 17);
            this.EmailIntelMosaic.TabIndex = 70;
            this.EmailIntelMosaic.Text = "Use/Increase Mosaic images to meet rule above";
            this.EmailIntelMosaic.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label16.ForeColor = System.Drawing.Color.Black;
            this.label16.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label16.Location = new System.Drawing.Point(15, 16);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(89, 13);
            this.label16.TabIndex = 33;
            this.label16.Text = "If more than";
            // 
            // emailIntelEmails
            // 
            this.emailIntelEmails.BackColor = System.Drawing.Color.LemonChiffon;
            this.emailIntelEmails.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.emailIntelEmails.ForeColor = System.Drawing.SystemColors.WindowText;
            this.emailIntelEmails.Location = new System.Drawing.Point(109, 12);
            this.emailIntelEmails.Name = "emailIntelEmails";
            this.emailIntelEmails.Size = new System.Drawing.Size(38, 21);
            this.emailIntelEmails.TabIndex = 9;
            this.emailIntelEmails.Text = "2";
            this.emailIntelEmails.Leave += new System.EventHandler(this.emailIntelEmails_Leave);
            // 
            // EmailIntelStop
            // 
            this.EmailIntelStop.AutoSize = true;
            this.EmailIntelStop.Checked = true;
            this.EmailIntelStop.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.EmailIntelStop.Location = new System.Drawing.Point(15, 68);
            this.EmailIntelStop.Name = "EmailIntelStop";
            this.EmailIntelStop.Size = new System.Drawing.Size(271, 17);
            this.EmailIntelStop.TabIndex = 69;
            this.EmailIntelStop.TabStop = true;
            this.EmailIntelStop.Text = "Stop sending emails until rule above is no longer met";
            this.EmailIntelStop.UseVisualStyleBackColor = true;
            this.EmailIntelStop.CheckedChanged += new System.EventHandler(this.EmailIntelStop_CheckedChanged);
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label48.ForeColor = System.Drawing.Color.Black;
            this.label48.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label48.Location = new System.Drawing.Point(153, 16);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(50, 13);
            this.label48.TabIndex = 42;
            this.label48.Text = "emails";
            // 
            // emailIntelMins
            // 
            this.emailIntelMins.BackColor = System.Drawing.Color.LemonChiffon;
            this.emailIntelMins.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.emailIntelMins.ForeColor = System.Drawing.SystemColors.WindowText;
            this.emailIntelMins.Location = new System.Drawing.Point(161, 35);
            this.emailIntelMins.Name = "emailIntelMins";
            this.emailIntelMins.Size = new System.Drawing.Size(38, 21);
            this.emailIntelMins.TabIndex = 43;
            this.emailIntelMins.Text = "2";
            this.emailIntelMins.Leave += new System.EventHandler(this.emailIntelMins_Leave);
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label56.ForeColor = System.Drawing.Color.Black;
            this.label56.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label56.Location = new System.Drawing.Point(15, 40);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(140, 13);
            this.label56.TabIndex = 45;
            this.label56.Text = "are sent in less than";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label49.ForeColor = System.Drawing.Color.Black;
            this.label49.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label49.Location = new System.Drawing.Point(205, 40);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(59, 13);
            this.label49.TabIndex = 44;
            this.label49.Text = "minutes";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold);
            this.label15.ForeColor = System.Drawing.Color.Black;
            this.label15.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label15.Location = new System.Drawing.Point(16, 14);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(390, 23);
            this.label15.TabIndex = 38;
            this.label15.Text = "Email intelligence/De-spamificator";
            // 
            // EmailIntelOff
            // 
            this.EmailIntelOff.AutoSize = true;
            this.EmailIntelOff.Checked = true;
            this.EmailIntelOff.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.EmailIntelOff.Location = new System.Drawing.Point(69, 40);
            this.EmailIntelOff.Name = "EmailIntelOff";
            this.EmailIntelOff.Size = new System.Drawing.Size(39, 17);
            this.EmailIntelOff.TabIndex = 68;
            this.EmailIntelOff.TabStop = true;
            this.EmailIntelOff.Text = "Off";
            this.EmailIntelOff.UseVisualStyleBackColor = true;
            // 
            // EmailIntelOn
            // 
            this.EmailIntelOn.AutoSize = true;
            this.EmailIntelOn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.EmailIntelOn.Location = new System.Drawing.Point(21, 40);
            this.EmailIntelOn.Name = "EmailIntelOn";
            this.EmailIntelOn.Size = new System.Drawing.Size(39, 17);
            this.EmailIntelOn.TabIndex = 67;
            this.EmailIntelOn.Text = "On";
            this.EmailIntelOn.UseVisualStyleBackColor = true;
            this.EmailIntelOn.CheckedChanged += new System.EventHandler(this.EmailIntelOn_CheckedChanged);
            // 
            // EmailIntelligenceCntl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox4);
            this.Name = "EmailIntelligenceCntl";
            this.Size = new System.Drawing.Size(418, 194);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.emailIntelPanel.ResumeLayout(false);
            this.emailIntelPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Panel emailIntelPanel;
        private System.Windows.Forms.RadioButton EmailIntelMosaic;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox emailIntelEmails;
        private System.Windows.Forms.RadioButton EmailIntelStop;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.TextBox emailIntelMins;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.RadioButton EmailIntelOff;
        private System.Windows.Forms.RadioButton EmailIntelOn;
    }
}
