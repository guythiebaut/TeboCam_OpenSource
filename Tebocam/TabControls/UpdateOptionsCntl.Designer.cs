namespace TeboCam
{
    partial class UpdateOptionsCntl
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
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.updateDebugLocation = new System.Windows.Forms.TextBox();
            this.bttInstallUpdateAdmin = new ctlCuteButton.cuteButton();
            this.lblVerAvail = new System.Windows.Forms.Label();
            this.lblCurVer = new System.Windows.Forms.Label();
            this.updateNotify = new System.Windows.Forms.CheckBox();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label1);
            this.groupBox7.Controls.Add(this.updateDebugLocation);
            this.groupBox7.Controls.Add(this.bttInstallUpdateAdmin);
            this.groupBox7.Controls.Add(this.lblVerAvail);
            this.groupBox7.Controls.Add(this.lblCurVer);
            this.groupBox7.Controls.Add(this.updateNotify);
            this.groupBox7.Location = new System.Drawing.Point(0, 0);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(483, 137);
            this.groupBox7.TabIndex = 62;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Update Options";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(6, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 13);
            this.label1.TabIndex = 70;
            this.label1.Text = "Update debug location:";
            // 
            // updateDebugLocation
            // 
            this.updateDebugLocation.BackColor = System.Drawing.Color.LemonChiffon;
            this.updateDebugLocation.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.updateDebugLocation.ForeColor = System.Drawing.SystemColors.WindowText;
            this.updateDebugLocation.Location = new System.Drawing.Point(166, 86);
            this.updateDebugLocation.Name = "updateDebugLocation";
            this.updateDebugLocation.Size = new System.Drawing.Size(267, 21);
            this.updateDebugLocation.TabIndex = 69;
            this.updateDebugLocation.TextChanged += new System.EventHandler(this.updateDebugLocation_TextChanged);
            // 
            // bttInstallUpdateAdmin
            // 
            this.bttInstallUpdateAdmin.BackColor = System.Drawing.Color.White;
            this.bttInstallUpdateAdmin.cuteColor1 = System.Drawing.Color.Gold;
            this.bttInstallUpdateAdmin.cuteColor2 = System.Drawing.Color.Red;
            this.bttInstallUpdateAdmin.cuteTransparent1 = 100;
            this.bttInstallUpdateAdmin.cuteTransparent2 = 100;
            this.bttInstallUpdateAdmin.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.bttInstallUpdateAdmin.ForeColor = System.Drawing.Color.Black;
            this.bttInstallUpdateAdmin.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bttInstallUpdateAdmin.Location = new System.Drawing.Point(6, 113);
            this.bttInstallUpdateAdmin.Name = "bttInstallUpdateAdmin";
            this.bttInstallUpdateAdmin.Size = new System.Drawing.Size(249, 20);
            this.bttInstallUpdateAdmin.TabIndex = 67;
            this.bttInstallUpdateAdmin.Text = "Download And Install Update";
            this.bttInstallUpdateAdmin.UseVisualStyleBackColor = false;
            this.bttInstallUpdateAdmin.Click += new System.EventHandler(this.bttInstallUpdateAdmin_Click);
            // 
            // lblVerAvail
            // 
            this.lblVerAvail.AutoSize = true;
            this.lblVerAvail.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblVerAvail.ForeColor = System.Drawing.Color.Black;
            this.lblVerAvail.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblVerAvail.Location = new System.Drawing.Point(6, 71);
            this.lblVerAvail.Name = "lblVerAvail";
            this.lblVerAvail.Size = new System.Drawing.Size(129, 13);
            this.lblVerAvail.TabIndex = 13;
            this.lblVerAvail.Text = "Available Version: ";
            // 
            // lblCurVer
            // 
            this.lblCurVer.AutoSize = true;
            this.lblCurVer.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblCurVer.ForeColor = System.Drawing.Color.Black;
            this.lblCurVer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCurVer.Location = new System.Drawing.Point(6, 49);
            this.lblCurVer.Name = "lblCurVer";
            this.lblCurVer.Size = new System.Drawing.Size(99, 13);
            this.lblCurVer.TabIndex = 12;
            this.lblCurVer.Text = "This Version:  ";
            // 
            // updateNotify
            // 
            this.updateNotify.AutoSize = true;
            this.updateNotify.Checked = true;
            this.updateNotify.CheckState = System.Windows.Forms.CheckState.Checked;
            this.updateNotify.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.updateNotify.Location = new System.Drawing.Point(6, 20);
            this.updateNotify.Name = "updateNotify";
            this.updateNotify.Size = new System.Drawing.Size(186, 17);
            this.updateNotify.TabIndex = 0;
            this.updateNotify.Text = "Notify when updates are available";
            this.updateNotify.UseVisualStyleBackColor = true;
            this.updateNotify.CheckedChanged += new System.EventHandler(this.updateNotify_CheckedChanged);
            // 
            // UpdateOptionsCntl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox7);
            this.Name = "UpdateOptionsCntl";
            this.Size = new System.Drawing.Size(483, 137);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox7;
        private ctlCuteButton.cuteButton bttInstallUpdateAdmin;
        private System.Windows.Forms.Label lblVerAvail;
        private System.Windows.Forms.Label lblCurVer;
        private System.Windows.Forms.CheckBox updateNotify;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox updateDebugLocation;
    }
}
