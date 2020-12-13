namespace TeboCam
{
    partial class SecurityLockdownCntl
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
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.btnSecurityLockdownOn = new System.Windows.Forms.Button();
            this.rdLockdownOff = new System.Windows.Forms.RadioButton();
            this.rdLockdownOn = new System.Windows.Forms.RadioButton();
            this.label38 = new System.Windows.Forms.Label();
            this.txtLockdownPassword = new System.Windows.Forms.TextBox();
            this.groupBox12.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.btnSecurityLockdownOn);
            this.groupBox12.Controls.Add(this.rdLockdownOff);
            this.groupBox12.Controls.Add(this.rdLockdownOn);
            this.groupBox12.Controls.Add(this.label38);
            this.groupBox12.Controls.Add(this.txtLockdownPassword);
            this.groupBox12.Location = new System.Drawing.Point(0, 0);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(353, 137);
            this.groupBox12.TabIndex = 73;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Security lockdown";
            // 
            // btnSecurityLockdownOn
            // 
            this.btnSecurityLockdownOn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSecurityLockdownOn.Location = new System.Drawing.Point(190, 20);
            this.btnSecurityLockdownOn.Name = "btnSecurityLockdownOn";
            this.btnSecurityLockdownOn.Size = new System.Drawing.Size(119, 32);
            this.btnSecurityLockdownOn.TabIndex = 35;
            this.btnSecurityLockdownOn.Text = "Lockdown now";
            this.btnSecurityLockdownOn.UseVisualStyleBackColor = true;
            this.btnSecurityLockdownOn.Click += new System.EventHandler(this.btnSecurityLockdownOn_Click);
            // 
            // rdLockdownOff
            // 
            this.rdLockdownOff.AutoSize = true;
            this.rdLockdownOff.Checked = true;
            this.rdLockdownOff.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rdLockdownOff.Location = new System.Drawing.Point(73, 31);
            this.rdLockdownOff.Name = "rdLockdownOff";
            this.rdLockdownOff.Size = new System.Drawing.Size(39, 17);
            this.rdLockdownOff.TabIndex = 16;
            this.rdLockdownOff.TabStop = true;
            this.rdLockdownOff.Text = "Off";
            this.rdLockdownOff.UseVisualStyleBackColor = true;
            this.rdLockdownOff.CheckedChanged += new System.EventHandler(this.rdLockdownOff_CheckedChanged);
            // 
            // rdLockdownOn
            // 
            this.rdLockdownOn.AutoSize = true;
            this.rdLockdownOn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rdLockdownOn.Location = new System.Drawing.Point(15, 31);
            this.rdLockdownOn.Name = "rdLockdownOn";
            this.rdLockdownOn.Size = new System.Drawing.Size(39, 17);
            this.rdLockdownOn.TabIndex = 15;
            this.rdLockdownOn.Text = "On";
            this.rdLockdownOn.UseVisualStyleBackColor = true;
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label38.ForeColor = System.Drawing.Color.Black;
            this.label38.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label38.Location = new System.Drawing.Point(12, 56);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(137, 13);
            this.label38.TabIndex = 14;
            this.label38.Text = "Lockdown Password";
            // 
            // txtLockdownPassword
            // 
            this.txtLockdownPassword.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtLockdownPassword.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtLockdownPassword.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtLockdownPassword.Location = new System.Drawing.Point(15, 77);
            this.txtLockdownPassword.Name = "txtLockdownPassword";
            this.txtLockdownPassword.PasswordChar = '*';
            this.txtLockdownPassword.Size = new System.Drawing.Size(294, 21);
            this.txtLockdownPassword.TabIndex = 13;
            this.txtLockdownPassword.Leave += new System.EventHandler(this.txtLockdownPassword_Leave);
            // 
            // SecurityLockdownCntl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox12);
            this.Name = "SecurityLockdownCntl";
            this.Size = new System.Drawing.Size(353, 137);
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.Button btnSecurityLockdownOn;
        private System.Windows.Forms.RadioButton rdLockdownOff;
        private System.Windows.Forms.RadioButton rdLockdownOn;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.TextBox txtLockdownPassword;
    }
}
