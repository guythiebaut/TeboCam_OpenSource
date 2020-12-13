namespace TeboCam
{
    partial class MiscCntl
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
            this.grpMisc = new System.Windows.Forms.GroupBox();
            this.chkHideWhenMinimised = new System.Windows.Forms.CheckBox();
            this.infoMode = new System.Windows.Forms.CheckBox();
            this.grpMisc.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpMisc
            // 
            this.grpMisc.Controls.Add(this.chkHideWhenMinimised);
            this.grpMisc.Controls.Add(this.infoMode);
            this.grpMisc.Location = new System.Drawing.Point(0, 0);
            this.grpMisc.Name = "grpMisc";
            this.grpMisc.Size = new System.Drawing.Size(353, 76);
            this.grpMisc.TabIndex = 75;
            this.grpMisc.TabStop = false;
            this.grpMisc.Text = "Misc";
            // 
            // chkHideWhenMinimised
            // 
            this.chkHideWhenMinimised.AutoSize = true;
            this.chkHideWhenMinimised.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkHideWhenMinimised.Location = new System.Drawing.Point(11, 20);
            this.chkHideWhenMinimised.Name = "chkHideWhenMinimised";
            this.chkHideWhenMinimised.Size = new System.Drawing.Size(125, 17);
            this.chkHideWhenMinimised.TabIndex = 72;
            this.chkHideWhenMinimised.Text = "Hide when minimised";
            this.chkHideWhenMinimised.UseVisualStyleBackColor = true;
            this.chkHideWhenMinimised.CheckedChanged += new System.EventHandler(this.chkHideWhenMinimised_CheckedChanged);
            // 
            // infoMode
            // 
            this.infoMode.AutoSize = true;
            this.infoMode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.infoMode.Location = new System.Drawing.Point(11, 43);
            this.infoMode.Name = "infoMode";
            this.infoMode.Size = new System.Drawing.Size(171, 17);
            this.infoMode.TabIndex = 71;
            this.infoMode.Text = "Capture live system information";
            this.infoMode.UseVisualStyleBackColor = true;
            this.infoMode.CheckedChanged += new System.EventHandler(this.infoMode_CheckedChanged);
            // 
            // MiscCntl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.grpMisc);
            this.Name = "MiscCntl";
            this.Size = new System.Drawing.Size(353, 76);
            this.grpMisc.ResumeLayout(false);
            this.grpMisc.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpMisc;
        private System.Windows.Forms.CheckBox chkHideWhenMinimised;
        private System.Windows.Forms.CheckBox infoMode;
    }
}
