namespace TeboCam
{
    partial class EmailHostSettingsCntl
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
            this.EmailHostPanel = new System.Windows.Forms.Panel();
            this.emailPasswordCntl = new TeboCam.TebocamControls.PasswordCntl();
            this.TestBtn = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.SSL = new System.Windows.Forms.CheckBox();
            this.emailUser = new System.Windows.Forms.TextBox();
            this.smtpHost = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.smtpPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.EmailHostPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // EmailHostPanel
            // 
            this.EmailHostPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EmailHostPanel.Controls.Add(this.emailPasswordCntl);
            this.EmailHostPanel.Controls.Add(this.TestBtn);
            this.EmailHostPanel.Controls.Add(this.label10);
            this.EmailHostPanel.Controls.Add(this.SSL);
            this.EmailHostPanel.Controls.Add(this.emailUser);
            this.EmailHostPanel.Controls.Add(this.smtpHost);
            this.EmailHostPanel.Controls.Add(this.label1);
            this.EmailHostPanel.Controls.Add(this.label3);
            this.EmailHostPanel.Controls.Add(this.smtpPort);
            this.EmailHostPanel.Controls.Add(this.label4);
            this.EmailHostPanel.Location = new System.Drawing.Point(0, 0);
            this.EmailHostPanel.Name = "EmailHostPanel";
            this.EmailHostPanel.Size = new System.Drawing.Size(338, 286);
            this.EmailHostPanel.TabIndex = 63;
            // 
            // emailPasswordCntl
            // 
            this.emailPasswordCntl.BackColor = System.Drawing.Color.Transparent;
            this.emailPasswordCntl.Location = new System.Drawing.Point(16, 115);
            this.emailPasswordCntl.Name = "emailPasswordCntl";
            this.emailPasswordCntl.Size = new System.Drawing.Size(198, 40);
            this.emailPasswordCntl.TabIndex = 56;
            // 
            // TestBtn
            // 
            this.TestBtn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.TestBtn.Location = new System.Drawing.Point(241, 13);
            this.TestBtn.Name = "TestBtn";
            this.TestBtn.Size = new System.Drawing.Size(57, 23);
            this.TestBtn.TabIndex = 55;
            this.TestBtn.Text = "Test";
            this.TestBtn.UseVisualStyleBackColor = true;
            this.TestBtn.Click += new System.EventHandler(this.TestBtn_Click);
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold);
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(12, 13);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(223, 23);
            this.label10.TabIndex = 22;
            this.label10.Text = "Email Host Settings";
            // 
            // SSL
            // 
            this.SSL.BackColor = System.Drawing.SystemColors.Control;
            this.SSL.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.SSL.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SSL.Location = new System.Drawing.Point(112, 249);
            this.SSL.Name = "SSL";
            this.SSL.Size = new System.Drawing.Size(49, 17);
            this.SSL.TabIndex = 4;
            this.SSL.Text = "SSL";
            this.SSL.UseVisualStyleBackColor = false;
            this.SSL.CheckedChanged += new System.EventHandler(this.SSL_CheckedChanged);
            // 
            // emailUser
            // 
            this.emailUser.BackColor = System.Drawing.Color.LemonChiffon;
            this.emailUser.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.emailUser.ForeColor = System.Drawing.SystemColors.WindowText;
            this.emailUser.Location = new System.Drawing.Point(16, 82);
            this.emailUser.MaximumSize = new System.Drawing.Size(294, 21);
            this.emailUser.MinimumSize = new System.Drawing.Size(294, 21);
            this.emailUser.Name = "emailUser";
            this.emailUser.Size = new System.Drawing.Size(294, 21);
            this.emailUser.TabIndex = 0;
            this.emailUser.TextChanged += new System.EventHandler(this.emailUser_TextChanged);
            this.emailUser.Leave += new System.EventHandler(this.emailUser_Leave);
            // 
            // smtpHost
            // 
            this.smtpHost.BackColor = System.Drawing.Color.LemonChiffon;
            this.smtpHost.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.smtpHost.ForeColor = System.Drawing.SystemColors.WindowText;
            this.smtpHost.Location = new System.Drawing.Point(16, 192);
            this.smtpHost.MaximumSize = new System.Drawing.Size(294, 21);
            this.smtpHost.MinimumSize = new System.Drawing.Size(294, 21);
            this.smtpHost.Name = "smtpHost";
            this.smtpHost.Size = new System.Drawing.Size(294, 21);
            this.smtpHost.TabIndex = 2;
            this.smtpHost.TextChanged += new System.EventHandler(this.smtpHost_TextChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(13, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Email User";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(13, 168);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "SMTP Host";
            // 
            // smtpPort
            // 
            this.smtpPort.BackColor = System.Drawing.Color.LemonChiffon;
            this.smtpPort.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.smtpPort.ForeColor = System.Drawing.SystemColors.WindowText;
            this.smtpPort.Location = new System.Drawing.Point(16, 249);
            this.smtpPort.Name = "smtpPort";
            this.smtpPort.Size = new System.Drawing.Size(80, 21);
            this.smtpPort.TabIndex = 3;
            this.smtpPort.Leave += new System.EventHandler(this.smtpPort_Leave);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(13, 224);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "SMTP Port";
            // 
            // EmailHostSettingsCntl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.EmailHostPanel);
            this.Name = "EmailHostSettingsCntl";
            this.Size = new System.Drawing.Size(338, 286);
            this.EmailHostPanel.ResumeLayout(false);
            this.EmailHostPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel EmailHostPanel;
        private System.Windows.Forms.Button TestBtn;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox SSL;
        private System.Windows.Forms.TextBox emailUser;
        private System.Windows.Forms.TextBox smtpHost;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox smtpPort;
        private System.Windows.Forms.Label label4;
        private TebocamControls.PasswordCntl emailPasswordCntl;
    }
}
