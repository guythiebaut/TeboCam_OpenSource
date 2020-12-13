namespace TeboCam
{
    partial class FtpSettingsCntl
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.label25 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.ftpUser = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.ftpPass = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label24 = new System.Windows.Forms.Label();
            this.ftpRoot = new System.Windows.Forms.TextBox();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label25);
            this.panel2.Controls.Add(this.label22);
            this.panel2.Controls.Add(this.ftpUser);
            this.panel2.Controls.Add(this.label23);
            this.panel2.Controls.Add(this.ftpPass);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.label24);
            this.panel2.Controls.Add(this.ftpRoot);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(338, 238);
            this.panel2.TabIndex = 62;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold);
            this.label25.ForeColor = System.Drawing.Color.Black;
            this.label25.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label25.Location = new System.Drawing.Point(12, 24);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(141, 23);
            this.label25.TabIndex = 31;
            this.label25.Text = "Ftp Settings";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label22.ForeColor = System.Drawing.Color.Black;
            this.label22.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label22.Location = new System.Drawing.Point(16, 63);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(62, 13);
            this.label22.TabIndex = 26;
            this.label22.Text = "Ftp User";
            // 
            // ftpUser
            // 
            this.ftpUser.BackColor = System.Drawing.Color.LemonChiffon;
            this.ftpUser.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.ftpUser.ForeColor = System.Drawing.SystemColors.WindowText;
            this.ftpUser.Location = new System.Drawing.Point(16, 88);
            this.ftpUser.Name = "ftpUser";
            this.ftpUser.Size = new System.Drawing.Size(294, 21);
            this.ftpUser.TabIndex = 5;
            this.ftpUser.TextChanged += new System.EventHandler(this.ftpUser_TextChanged);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label23.ForeColor = System.Drawing.Color.Black;
            this.label23.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label23.Location = new System.Drawing.Point(16, 118);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(94, 13);
            this.label23.TabIndex = 28;
            this.label23.Text = "Ftp Password";
            // 
            // ftpPass
            // 
            this.ftpPass.BackColor = System.Drawing.Color.LemonChiffon;
            this.ftpPass.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.ftpPass.ForeColor = System.Drawing.SystemColors.WindowText;
            this.ftpPass.Location = new System.Drawing.Point(16, 140);
            this.ftpPass.Name = "ftpPass";
            this.ftpPass.PasswordChar = '*';
            this.ftpPass.Size = new System.Drawing.Size(294, 21);
            this.ftpPass.TabIndex = 6;
            this.ftpPass.TextChanged += new System.EventHandler(this.ftpPass_TextChanged);
            // 
            // button2
            // 
            this.button2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button2.Location = new System.Drawing.Point(159, 24);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(57, 23);
            this.button2.TabIndex = 56;
            this.button2.Text = "Test";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label24.ForeColor = System.Drawing.Color.Black;
            this.label24.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label24.Location = new System.Drawing.Point(16, 170);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(126, 13);
            this.label24.TabIndex = 30;
            this.label24.Text = "Ftp Root Directory";
            // 
            // ftpRoot
            // 
            this.ftpRoot.BackColor = System.Drawing.Color.LemonChiffon;
            this.ftpRoot.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.ftpRoot.ForeColor = System.Drawing.SystemColors.WindowText;
            this.ftpRoot.Location = new System.Drawing.Point(16, 192);
            this.ftpRoot.Name = "ftpRoot";
            this.ftpRoot.Size = new System.Drawing.Size(294, 21);
            this.ftpRoot.TabIndex = 7;
            this.ftpRoot.TextChanged += new System.EventHandler(this.ftpRoot_TextChanged);
            // 
            // FtpSettingsCntl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panel2);
            this.Name = "FtpSettingsCntl";
            this.Size = new System.Drawing.Size(338, 238);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox ftpUser;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox ftpPass;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox ftpRoot;
    }
}
