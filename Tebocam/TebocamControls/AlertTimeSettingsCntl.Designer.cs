namespace TeboCam
{
    partial class AlertTimeSettingsCntl
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
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.emailNotifInterval = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.imageFileInterval = new System.Windows.Forms.TextBox();
            this.groupBox13.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.label21);
            this.groupBox13.Controls.Add(this.label17);
            this.groupBox13.Controls.Add(this.label18);
            this.groupBox13.Controls.Add(this.emailNotifInterval);
            this.groupBox13.Controls.Add(this.label27);
            this.groupBox13.Controls.Add(this.label26);
            this.groupBox13.Controls.Add(this.imageFileInterval);
            this.groupBox13.Location = new System.Drawing.Point(0, 0);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(295, 187);
            this.groupBox13.TabIndex = 59;
            this.groupBox13.TabStop = false;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold);
            this.label21.ForeColor = System.Drawing.Color.Black;
            this.label21.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label21.Location = new System.Drawing.Point(16, 17);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(220, 23);
            this.label21.TabIndex = 38;
            this.label21.Text = "Alert Time Settings";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label17.ForeColor = System.Drawing.Color.Black;
            this.label17.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label17.Location = new System.Drawing.Point(30, 47);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(137, 13);
            this.label17.TabIndex = 33;
            this.label17.Text = "Frequency of email ";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label18.ForeColor = System.Drawing.Color.Black;
            this.label18.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label18.Location = new System.Drawing.Point(29, 64);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(161, 13);
            this.label18.TabIndex = 34;
            this.label18.Text = "notifications in seconds";
            // 
            // emailNotifInterval
            // 
            this.emailNotifInterval.BackColor = System.Drawing.Color.LemonChiffon;
            this.emailNotifInterval.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.emailNotifInterval.ForeColor = System.Drawing.SystemColors.WindowText;
            this.emailNotifInterval.Location = new System.Drawing.Point(32, 86);
            this.emailNotifInterval.Name = "emailNotifInterval";
            this.emailNotifInterval.Size = new System.Drawing.Size(65, 21);
            this.emailNotifInterval.TabIndex = 9;
            this.emailNotifInterval.Text = "2";
            this.emailNotifInterval.Leave += new System.EventHandler(this.emailNotifInterval_Leave);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label27.ForeColor = System.Drawing.Color.Black;
            this.label27.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label27.Location = new System.Drawing.Point(28, 114);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(137, 13);
            this.label27.TabIndex = 40;
            this.label27.Text = "Frequency of image";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label26.ForeColor = System.Drawing.Color.Black;
            this.label26.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label26.Location = new System.Drawing.Point(28, 131);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(221, 13);
            this.label26.TabIndex = 41;
            this.label26.Text = "file updates in seconds (e.g. 0.5)";
            // 
            // imageFileInterval
            // 
            this.imageFileInterval.BackColor = System.Drawing.Color.LemonChiffon;
            this.imageFileInterval.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.imageFileInterval.ForeColor = System.Drawing.SystemColors.WindowText;
            this.imageFileInterval.Location = new System.Drawing.Point(32, 153);
            this.imageFileInterval.Name = "imageFileInterval";
            this.imageFileInterval.Size = new System.Drawing.Size(65, 21);
            this.imageFileInterval.TabIndex = 10;
            this.imageFileInterval.Text = "2";
            this.imageFileInterval.Leave += new System.EventHandler(this.imageFileInterval_Leave);
            // 
            // AlertTimeSettingsCntl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox13);
            this.Name = "AlertTimeSettingsCntl";
            this.Size = new System.Drawing.Size(295, 187);
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox emailNotifInterval;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox imageFileInterval;
    }
}
