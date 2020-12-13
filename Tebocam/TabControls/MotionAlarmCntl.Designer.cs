namespace TeboCam
{
    partial class MotionAlarmCntl
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
            this.MotionAlarm = new System.Windows.Forms.GroupBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.bttnMotionScheduleOnAtStart = new System.Windows.Forms.CheckBox();
            this.bttnMotionSchedule = new System.Windows.Forms.CheckBox();
            this.lblstartmov = new System.Windows.Forms.Label();
            this.lblendmov = new System.Windows.Forms.Label();
            this.button38 = new System.Windows.Forms.Button();
            this.bttnActivateAtEveryStartup = new System.Windows.Forms.CheckBox();
            this.bttnMotionAtStartup = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.bttnNow = new System.Windows.Forms.RadioButton();
            this.lblTime = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.bttnTime = new System.Windows.Forms.RadioButton();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.bttnSeconds = new System.Windows.Forms.RadioButton();
            this.actCountdown = new System.Windows.Forms.TextBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.bttnMotionInactive = new System.Windows.Forms.RadioButton();
            this.bttnMotionActive = new System.Windows.Forms.RadioButton();
            this.MotionAlarm.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // MotionAlarm
            // 
            this.MotionAlarm.Controls.Add(this.groupBox9);
            this.MotionAlarm.Controls.Add(this.bttnActivateAtEveryStartup);
            this.MotionAlarm.Controls.Add(this.bttnMotionAtStartup);
            this.MotionAlarm.Controls.Add(this.groupBox5);
            this.MotionAlarm.Controls.Add(this.bttnMotionInactive);
            this.MotionAlarm.Controls.Add(this.bttnMotionActive);
            this.MotionAlarm.Location = new System.Drawing.Point(0, 0);
            this.MotionAlarm.Name = "MotionAlarm";
            this.MotionAlarm.Size = new System.Drawing.Size(499, 165);
            this.MotionAlarm.TabIndex = 33;
            this.MotionAlarm.TabStop = false;
            this.MotionAlarm.Text = "Motion Alarm";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.bttnMotionScheduleOnAtStart);
            this.groupBox9.Controls.Add(this.bttnMotionSchedule);
            this.groupBox9.Controls.Add(this.lblstartmov);
            this.groupBox9.Controls.Add(this.lblendmov);
            this.groupBox9.Controls.Add(this.button38);
            this.groupBox9.Location = new System.Drawing.Point(280, 41);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(200, 121);
            this.groupBox9.TabIndex = 104;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Schedule";
            // 
            // bttnMotionScheduleOnAtStart
            // 
            this.bttnMotionScheduleOnAtStart.AutoSize = true;
            this.bttnMotionScheduleOnAtStart.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bttnMotionScheduleOnAtStart.Location = new System.Drawing.Point(97, 21);
            this.bttnMotionScheduleOnAtStart.Name = "bttnMotionScheduleOnAtStart";
            this.bttnMotionScheduleOnAtStart.Size = new System.Drawing.Size(75, 17);
            this.bttnMotionScheduleOnAtStart.TabIndex = 103;
            this.bttnMotionScheduleOnAtStart.Text = "On at start";
            this.bttnMotionScheduleOnAtStart.UseVisualStyleBackColor = true;
            this.bttnMotionScheduleOnAtStart.CheckedChanged += new System.EventHandler(this.bttnMotionScheduleOnAtStart_CheckedChanged);
            // 
            // bttnMotionSchedule
            // 
            this.bttnMotionSchedule.AutoSize = true;
            this.bttnMotionSchedule.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bttnMotionSchedule.Location = new System.Drawing.Point(6, 21);
            this.bttnMotionSchedule.Name = "bttnMotionSchedule";
            this.bttnMotionSchedule.Size = new System.Drawing.Size(40, 17);
            this.bttnMotionSchedule.TabIndex = 97;
            this.bttnMotionSchedule.Text = "On";
            this.bttnMotionSchedule.UseVisualStyleBackColor = true;
            this.bttnMotionSchedule.CheckedChanged += new System.EventHandler(this.bttnMotionSchedule_CheckedChanged);
            // 
            // lblstartmov
            // 
            this.lblstartmov.AutoSize = true;
            this.lblstartmov.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblstartmov.ForeColor = System.Drawing.Color.Black;
            this.lblstartmov.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblstartmov.Location = new System.Drawing.Point(3, 75);
            this.lblstartmov.Name = "lblstartmov";
            this.lblstartmov.Size = new System.Drawing.Size(47, 13);
            this.lblstartmov.TabIndex = 99;
            this.lblstartmov.Text = "Start: ";
            // 
            // lblendmov
            // 
            this.lblendmov.AutoSize = true;
            this.lblendmov.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblendmov.ForeColor = System.Drawing.Color.Black;
            this.lblendmov.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblendmov.Location = new System.Drawing.Point(3, 96);
            this.lblendmov.Name = "lblendmov";
            this.lblendmov.Size = new System.Drawing.Size(35, 13);
            this.lblendmov.TabIndex = 100;
            this.lblendmov.Text = "End:";
            // 
            // button38
            // 
            this.button38.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button38.Location = new System.Drawing.Point(4, 43);
            this.button38.Name = "button38";
            this.button38.Size = new System.Drawing.Size(188, 29);
            this.button38.TabIndex = 101;
            this.button38.Text = "Set Schedule";
            this.button38.UseVisualStyleBackColor = true;
            this.button38.Click += new System.EventHandler(this.button38_Click);
            // 
            // bttnActivateAtEveryStartup
            // 
            this.bttnActivateAtEveryStartup.AutoSize = true;
            this.bttnActivateAtEveryStartup.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bttnActivateAtEveryStartup.Location = new System.Drawing.Point(273, 21);
            this.bttnActivateAtEveryStartup.Name = "bttnActivateAtEveryStartup";
            this.bttnActivateAtEveryStartup.Size = new System.Drawing.Size(141, 17);
            this.bttnActivateAtEveryStartup.TabIndex = 102;
            this.bttnActivateAtEveryStartup.Text = "Activate at every startup";
            this.bttnActivateAtEveryStartup.UseVisualStyleBackColor = true;
            this.bttnActivateAtEveryStartup.CheckedChanged += new System.EventHandler(this.bttnActivateAtEveryStartup_CheckedChanged);
            // 
            // bttnMotionAtStartup
            // 
            this.bttnMotionAtStartup.AutoSize = true;
            this.bttnMotionAtStartup.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bttnMotionAtStartup.Location = new System.Drawing.Point(172, 20);
            this.bttnMotionAtStartup.Name = "bttnMotionAtStartup";
            this.bttnMotionAtStartup.Size = new System.Drawing.Size(72, 17);
            this.bttnMotionAtStartup.TabIndex = 59;
            this.bttnMotionAtStartup.Text = "At Startup";
            this.bttnMotionAtStartup.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.bttnNow);
            this.groupBox5.Controls.Add(this.lblTime);
            this.groupBox5.Controls.Add(this.label43);
            this.groupBox5.Controls.Add(this.bttnTime);
            this.groupBox5.Controls.Add(this.numericUpDown2);
            this.groupBox5.Controls.Add(this.bttnSeconds);
            this.groupBox5.Controls.Add(this.actCountdown);
            this.groupBox5.Controls.Add(this.numericUpDown1);
            this.groupBox5.Location = new System.Drawing.Point(7, 54);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(265, 100);
            this.groupBox5.TabIndex = 58;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Activate in/at";
            // 
            // bttnNow
            // 
            this.bttnNow.AutoSize = true;
            this.bttnNow.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bttnNow.Location = new System.Drawing.Point(6, 21);
            this.bttnNow.Name = "bttnNow";
            this.bttnNow.Size = new System.Drawing.Size(47, 17);
            this.bttnNow.TabIndex = 63;
            this.bttnNow.Text = "Now";
            this.bttnNow.UseVisualStyleBackColor = true;
            this.bttnNow.CheckedChanged += new System.EventHandler(this.bttnNow_CheckedChanged);
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblTime.ForeColor = System.Drawing.Color.Black;
            this.lblTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblTime.Location = new System.Drawing.Point(214, 70);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(43, 13);
            this.lblTime.TabIndex = 58;
            this.lblTime.Text = "00:00";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label43.ForeColor = System.Drawing.Color.Black;
            this.label43.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label43.Location = new System.Drawing.Point(148, 70);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(11, 13);
            this.label43.TabIndex = 62;
            this.label43.Text = ":";
            // 
            // bttnTime
            // 
            this.bttnTime.AutoSize = true;
            this.bttnTime.Checked = true;
            this.bttnTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bttnTime.Location = new System.Drawing.Point(6, 66);
            this.bttnTime.Name = "bttnTime";
            this.bttnTime.Size = new System.Drawing.Size(48, 17);
            this.bttnTime.TabIndex = 1;
            this.bttnTime.TabStop = true;
            this.bttnTime.Text = "Time";
            this.bttnTime.UseVisualStyleBackColor = true;
            this.bttnTime.CheckedChanged += new System.EventHandler(this.bttnTime_CheckedChanged);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.BackColor = System.Drawing.Color.LemonChiffon;
            this.numericUpDown2.Location = new System.Drawing.Point(165, 68);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(43, 20);
            this.numericUpDown2.TabIndex = 59;
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // bttnSeconds
            // 
            this.bttnSeconds.AutoSize = true;
            this.bttnSeconds.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bttnSeconds.Location = new System.Drawing.Point(6, 42);
            this.bttnSeconds.Name = "bttnSeconds";
            this.bttnSeconds.Size = new System.Drawing.Size(67, 17);
            this.bttnSeconds.TabIndex = 0;
            this.bttnSeconds.Text = "Seconds";
            this.bttnSeconds.UseVisualStyleBackColor = true;
            // 
            // actCountdown
            // 
            this.actCountdown.BackColor = System.Drawing.Color.LemonChiffon;
            this.actCountdown.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.actCountdown.ForeColor = System.Drawing.SystemColors.WindowText;
            this.actCountdown.Location = new System.Drawing.Point(94, 41);
            this.actCountdown.Name = "actCountdown";
            this.actCountdown.Size = new System.Drawing.Size(48, 21);
            this.actCountdown.TabIndex = 40;
            this.actCountdown.TextChanged += new System.EventHandler(this.actCountdown_TextChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.BackColor = System.Drawing.Color.LemonChiffon;
            this.numericUpDown1.Location = new System.Drawing.Point(94, 68);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(48, 20);
            this.numericUpDown1.TabIndex = 58;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // bttnMotionInactive
            // 
            this.bttnMotionInactive.AutoSize = true;
            this.bttnMotionInactive.Checked = true;
            this.bttnMotionInactive.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bttnMotionInactive.Location = new System.Drawing.Point(89, 20);
            this.bttnMotionInactive.Name = "bttnMotionInactive";
            this.bttnMotionInactive.Size = new System.Drawing.Size(63, 17);
            this.bttnMotionInactive.TabIndex = 1;
            this.bttnMotionInactive.TabStop = true;
            this.bttnMotionInactive.Text = "Inactive";
            this.bttnMotionInactive.UseVisualStyleBackColor = true;
            // 
            // bttnMotionActive
            // 
            this.bttnMotionActive.AutoSize = true;
            this.bttnMotionActive.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bttnMotionActive.Location = new System.Drawing.Point(17, 20);
            this.bttnMotionActive.Name = "bttnMotionActive";
            this.bttnMotionActive.Size = new System.Drawing.Size(55, 17);
            this.bttnMotionActive.TabIndex = 0;
            this.bttnMotionActive.Text = "Active";
            this.bttnMotionActive.UseVisualStyleBackColor = true;
            this.bttnMotionActive.CheckedChanged += new System.EventHandler(this.bttnMotionActive_CheckedChanged);
            // 
            // MotionAlarmCntl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.MotionAlarm);
            this.Name = "MotionAlarmCntl";
            this.Size = new System.Drawing.Size(499, 165);
            this.MotionAlarm.ResumeLayout(false);
            this.MotionAlarm.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox MotionAlarm;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.CheckBox bttnMotionScheduleOnAtStart;
        private System.Windows.Forms.CheckBox bttnMotionSchedule;
        private System.Windows.Forms.Label lblstartmov;
        private System.Windows.Forms.Label lblendmov;
        private System.Windows.Forms.Button button38;
        private System.Windows.Forms.CheckBox bttnActivateAtEveryStartup;
        private System.Windows.Forms.RadioButton bttnMotionAtStartup;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton bttnNow;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.RadioButton bttnTime;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.RadioButton bttnSeconds;
        private System.Windows.Forms.TextBox actCountdown;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.RadioButton bttnMotionInactive;
        private System.Windows.Forms.RadioButton bttnMotionActive;
    }
}
