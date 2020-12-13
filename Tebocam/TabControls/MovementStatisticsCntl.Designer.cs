namespace TeboCam
{
    partial class MovementStatisticsCntl
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
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.pnlStatsToFile = new System.Windows.Forms.Panel();
            this.txtStatsToFileMb = new System.Windows.Forms.TextBox();
            this.chkStatsToFileTimeStamp = new System.Windows.Forms.CheckBox();
            this.label37 = new System.Windows.Forms.Label();
            this.btnStatsToFileLocation = new System.Windows.Forms.Button();
            this.rdStatsToFileOff = new System.Windows.Forms.RadioButton();
            this.rdStatsToFileOn = new System.Windows.Forms.RadioButton();
            this.label31 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.groupBox11.SuspendLayout();
            this.pnlStatsToFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.pnlStatsToFile);
            this.groupBox11.Controls.Add(this.btnStatsToFileLocation);
            this.groupBox11.Controls.Add(this.rdStatsToFileOff);
            this.groupBox11.Controls.Add(this.rdStatsToFileOn);
            this.groupBox11.Controls.Add(this.label31);
            this.groupBox11.Controls.Add(this.label29);
            this.groupBox11.Location = new System.Drawing.Point(0, 0);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(418, 153);
            this.groupBox11.TabIndex = 66;
            this.groupBox11.TabStop = false;
            // 
            // pnlStatsToFile
            // 
            this.pnlStatsToFile.Controls.Add(this.txtStatsToFileMb);
            this.pnlStatsToFile.Controls.Add(this.chkStatsToFileTimeStamp);
            this.pnlStatsToFile.Controls.Add(this.label37);
            this.pnlStatsToFile.Location = new System.Drawing.Point(10, 75);
            this.pnlStatsToFile.Name = "pnlStatsToFile";
            this.pnlStatsToFile.Size = new System.Drawing.Size(396, 72);
            this.pnlStatsToFile.TabIndex = 66;
            // 
            // txtStatsToFileMb
            // 
            this.txtStatsToFileMb.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtStatsToFileMb.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtStatsToFileMb.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtStatsToFileMb.Location = new System.Drawing.Point(11, 27);
            this.txtStatsToFileMb.Name = "txtStatsToFileMb";
            this.txtStatsToFileMb.Size = new System.Drawing.Size(48, 21);
            this.txtStatsToFileMb.TabIndex = 73;
            this.txtStatsToFileMb.Leave += new System.EventHandler(this.txtStatsToFileMb_Leave);
            // 
            // chkStatsToFileTimeStamp
            // 
            this.chkStatsToFileTimeStamp.AutoSize = true;
            this.chkStatsToFileTimeStamp.BackColor = System.Drawing.SystemColors.Control;
            this.chkStatsToFileTimeStamp.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.chkStatsToFileTimeStamp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkStatsToFileTimeStamp.Location = new System.Drawing.Point(11, 4);
            this.chkStatsToFileTimeStamp.Name = "chkStatsToFileTimeStamp";
            this.chkStatsToFileTimeStamp.Size = new System.Drawing.Size(372, 17);
            this.chkStatsToFileTimeStamp.TabIndex = 72;
            this.chkStatsToFileTimeStamp.Text = "Create new time-stamped file when file size reaches:";
            this.chkStatsToFileTimeStamp.UseVisualStyleBackColor = false;
            this.chkStatsToFileTimeStamp.CheckedChanged += new System.EventHandler(this.chkStatsToFileTimeStamp_CheckedChanged);
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label37.ForeColor = System.Drawing.Color.Black;
            this.label37.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label37.Location = new System.Drawing.Point(65, 33);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(77, 13);
            this.label37.TabIndex = 74;
            this.label37.Text = "Megabytes";
            // 
            // btnStatsToFileLocation
            // 
            this.btnStatsToFileLocation.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnStatsToFileLocation.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnStatsToFileLocation.Location = new System.Drawing.Point(206, 49);
            this.btnStatsToFileLocation.Name = "btnStatsToFileLocation";
            this.btnStatsToFileLocation.Size = new System.Drawing.Size(150, 21);
            this.btnStatsToFileLocation.TabIndex = 71;
            this.btnStatsToFileLocation.Text = "Set file name...";
            this.btnStatsToFileLocation.UseVisualStyleBackColor = true;
            this.btnStatsToFileLocation.Click += new System.EventHandler(this.btnStatsToFileLocation_Click);
            // 
            // rdStatsToFileOff
            // 
            this.rdStatsToFileOff.AutoSize = true;
            this.rdStatsToFileOff.Checked = true;
            this.rdStatsToFileOff.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rdStatsToFileOff.Location = new System.Drawing.Point(156, 51);
            this.rdStatsToFileOff.Name = "rdStatsToFileOff";
            this.rdStatsToFileOff.Size = new System.Drawing.Size(39, 17);
            this.rdStatsToFileOff.TabIndex = 70;
            this.rdStatsToFileOff.TabStop = true;
            this.rdStatsToFileOff.Text = "Off";
            this.rdStatsToFileOff.UseVisualStyleBackColor = true;
            // 
            // rdStatsToFileOn
            // 
            this.rdStatsToFileOn.AutoSize = true;
            this.rdStatsToFileOn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rdStatsToFileOn.Location = new System.Drawing.Point(108, 51);
            this.rdStatsToFileOn.Name = "rdStatsToFileOn";
            this.rdStatsToFileOn.Size = new System.Drawing.Size(39, 17);
            this.rdStatsToFileOn.TabIndex = 69;
            this.rdStatsToFileOn.Text = "On";
            this.rdStatsToFileOn.UseVisualStyleBackColor = true;
            this.rdStatsToFileOn.CheckedChanged += new System.EventHandler(this.rdStatsToFileOn_CheckedChanged);
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label31.ForeColor = System.Drawing.Color.Black;
            this.label31.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label31.Location = new System.Drawing.Point(18, 53);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(84, 13);
            this.label31.TabIndex = 40;
            this.label31.Text = "Write to file";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold);
            this.label29.ForeColor = System.Drawing.Color.Black;
            this.label29.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label29.Location = new System.Drawing.Point(16, 17);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(227, 23);
            this.label29.TabIndex = 39;
            this.label29.Text = "Movement statistics";
            // 
            // MovementStatisticsCntl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox11);
            this.Name = "MovementStatisticsCntl";
            this.Size = new System.Drawing.Size(418, 153);
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.pnlStatsToFile.ResumeLayout(false);
            this.pnlStatsToFile.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.Panel pnlStatsToFile;
        private System.Windows.Forms.TextBox txtStatsToFileMb;
        private System.Windows.Forms.CheckBox chkStatsToFileTimeStamp;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Button btnStatsToFileLocation;
        private System.Windows.Forms.RadioButton rdStatsToFileOff;
        private System.Windows.Forms.RadioButton rdStatsToFileOn;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label29;
    }
}
