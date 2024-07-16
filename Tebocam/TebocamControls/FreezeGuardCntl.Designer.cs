namespace TeboCam
{
    partial class FreezeGuardCntl
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.freezeGuardWindow = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.pulseFreq = new System.Windows.Forms.TextBox();
            this.button15 = new System.Windows.Forms.Button();
            this.freezeGuardOff = new System.Windows.Forms.RadioButton();
            this.freezeGuardOn = new System.Windows.Forms.RadioButton();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.freezeGuardWindow);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.pulseFreq);
            this.groupBox2.Controls.Add(this.button15);
            this.groupBox2.Controls.Add(this.freezeGuardOff);
            this.groupBox2.Controls.Add(this.freezeGuardOn);
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(353, 155);
            this.groupBox2.TabIndex = 71;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "FreezeGuard  Protection";
            // 
            // freezeGuardWindow
            // 
            this.freezeGuardWindow.AutoSize = true;
            this.freezeGuardWindow.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.freezeGuardWindow.Location = new System.Drawing.Point(11, 115);
            this.freezeGuardWindow.Name = "freezeGuardWindow";
            this.freezeGuardWindow.Size = new System.Drawing.Size(159, 17);
            this.freezeGuardWindow.TabIndex = 72;
            this.freezeGuardWindow.Text = "Show FreezeGuard Window";
            this.freezeGuardWindow.UseVisualStyleBackColor = true;
            this.freezeGuardWindow.CheckedChanged += new System.EventHandler(this.freezeGuardWindow_CheckedChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.label14.ForeColor = System.Drawing.Color.Black;
            this.label14.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label14.Location = new System.Drawing.Point(12, 83);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(326, 13);
            this.label14.TabIndex = 74;
            this.label14.Text = "(TeboCam needs to be re-started for this to be applied)";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label13.Location = new System.Drawing.Point(10, 63);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(198, 13);
            this.label13.TabIndex = 73;
            this.label13.Text = "Pulse Frequency(per minute)";
            // 
            // pulseFreq
            // 
            this.pulseFreq.BackColor = System.Drawing.Color.LemonChiffon;
            this.pulseFreq.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pulseFreq.ForeColor = System.Drawing.SystemColors.WindowText;
            this.pulseFreq.Location = new System.Drawing.Point(208, 60);
            this.pulseFreq.Name = "pulseFreq";
            this.pulseFreq.Size = new System.Drawing.Size(48, 21);
            this.pulseFreq.TabIndex = 72;
            this.pulseFreq.Leave += new System.EventHandler(this.pulseFreq_Leave);
            // 
            // button15
            // 
            this.button15.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button15.Location = new System.Drawing.Point(179, 14);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(160, 37);
            this.button15.TabIndex = 71;
            this.button15.Text = "Test FreezeGuard";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // freezeGuardOff
            // 
            this.freezeGuardOff.AutoSize = true;
            this.freezeGuardOff.Checked = true;
            this.freezeGuardOff.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.freezeGuardOff.Location = new System.Drawing.Point(69, 28);
            this.freezeGuardOff.Name = "freezeGuardOff";
            this.freezeGuardOff.Size = new System.Drawing.Size(39, 17);
            this.freezeGuardOff.TabIndex = 1;
            this.freezeGuardOff.TabStop = true;
            this.freezeGuardOff.Text = "Off";
            this.freezeGuardOff.UseVisualStyleBackColor = true;
            // 
            // freezeGuardOn
            // 
            this.freezeGuardOn.AutoSize = true;
            this.freezeGuardOn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.freezeGuardOn.Location = new System.Drawing.Point(11, 28);
            this.freezeGuardOn.Name = "freezeGuardOn";
            this.freezeGuardOn.Size = new System.Drawing.Size(39, 17);
            this.freezeGuardOn.TabIndex = 0;
            this.freezeGuardOn.Text = "On";
            this.freezeGuardOn.UseVisualStyleBackColor = true;
            this.freezeGuardOn.CheckedChanged += new System.EventHandler(this.freezeGuardOn_CheckedChanged);
            // 
            // FreezeGuardCntl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox2);
            this.Name = "FreezeGuardCntl";
            this.Size = new System.Drawing.Size(353, 155);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox freezeGuardWindow;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox pulseFreq;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.RadioButton freezeGuardOff;
        private System.Windows.Forms.RadioButton freezeGuardOn;
    }
}
