namespace TeboCam
{
    partial class FrameRateCntl
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
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.chkFrameRateTrack = new System.Windows.Forms.CheckBox();
            this.label64 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.numFrameRateCalcOver = new System.Windows.Forms.NumericUpDown();
            this.groupBox16.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFrameRateCalcOver)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox16
            // 
            this.groupBox16.Controls.Add(this.chkFrameRateTrack);
            this.groupBox16.Controls.Add(this.label64);
            this.groupBox16.Controls.Add(this.label35);
            this.groupBox16.Controls.Add(this.numFrameRateCalcOver);
            this.groupBox16.Location = new System.Drawing.Point(0, 0);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(489, 69);
            this.groupBox16.TabIndex = 74;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "Frame Rate";
            // 
            // chkFrameRateTrack
            // 
            this.chkFrameRateTrack.AutoSize = true;
            this.chkFrameRateTrack.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkFrameRateTrack.Location = new System.Drawing.Point(11, 20);
            this.chkFrameRateTrack.Name = "chkFrameRateTrack";
            this.chkFrameRateTrack.Size = new System.Drawing.Size(112, 17);
            this.chkFrameRateTrack.TabIndex = 75;
            this.chkFrameRateTrack.Text = "Track Frame Rate";
            this.chkFrameRateTrack.UseVisualStyleBackColor = true;
            this.chkFrameRateTrack.CheckedChanged += new System.EventHandler(this.chkFrameRateTrack_CheckedChanged);
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label64.ForeColor = System.Drawing.Color.Black;
            this.label64.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label64.Location = new System.Drawing.Point(256, 45);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(60, 13);
            this.label64.TabIndex = 63;
            this.label64.Text = "seconds";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label35.ForeColor = System.Drawing.Color.Black;
            this.label35.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label35.Location = new System.Drawing.Point(8, 45);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(175, 13);
            this.label35.TabIndex = 62;
            this.label35.Text = "Calculate frame rate over";
            // 
            // numFrameRateCalcOver
            // 
            this.numFrameRateCalcOver.BackColor = System.Drawing.Color.LemonChiffon;
            this.numFrameRateCalcOver.Location = new System.Drawing.Point(189, 43);
            this.numFrameRateCalcOver.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numFrameRateCalcOver.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numFrameRateCalcOver.Name = "numFrameRateCalcOver";
            this.numFrameRateCalcOver.Size = new System.Drawing.Size(61, 20);
            this.numFrameRateCalcOver.TabIndex = 61;
            this.numFrameRateCalcOver.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numFrameRateCalcOver.Leave += new System.EventHandler(this.numFrameRateCalcOver_Leave);
            // 
            // FrameRateCntl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox16);
            this.Name = "FrameRateCntl";
            this.Size = new System.Drawing.Size(489, 69);
            this.groupBox16.ResumeLayout(false);
            this.groupBox16.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFrameRateCalcOver)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox16;
        private System.Windows.Forms.CheckBox chkFrameRateTrack;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.NumericUpDown numFrameRateCalcOver;
    }
}
