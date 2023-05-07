namespace TeboCam
{
    partial class AdminCntl
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
            this.grpAlert = new System.Windows.Forms.GroupBox();
            this.btnAlert = new System.Windows.Forms.Button();
            this.numCamera = new System.Windows.Forms.NumericUpDown();
            this.chkAlertAll = new System.Windows.Forms.CheckBox();
            this.lblCamera = new System.Windows.Forms.Label();
            this.grpAlert.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCamera)).BeginInit();
            this.SuspendLayout();
            // 
            // grpAlert
            // 
            this.grpAlert.Controls.Add(this.lblCamera);
            this.grpAlert.Controls.Add(this.chkAlertAll);
            this.grpAlert.Controls.Add(this.numCamera);
            this.grpAlert.Controls.Add(this.btnAlert);
            this.grpAlert.Location = new System.Drawing.Point(37, 69);
            this.grpAlert.Name = "grpAlert";
            this.grpAlert.Size = new System.Drawing.Size(275, 89);
            this.grpAlert.TabIndex = 0;
            this.grpAlert.TabStop = false;
            this.grpAlert.Text = "Generate alert on camera(s)";
            // 
            // btnAlert
            // 
            this.btnAlert.Location = new System.Drawing.Point(166, 41);
            this.btnAlert.Name = "btnAlert";
            this.btnAlert.Size = new System.Drawing.Size(98, 23);
            this.btnAlert.TabIndex = 0;
            this.btnAlert.Text = "Generate alert";
            this.btnAlert.UseVisualStyleBackColor = true;
            this.btnAlert.Click += new System.EventHandler(this.btnAlert_Click);
            // 
            // numCamera
            // 
            this.numCamera.Location = new System.Drawing.Point(0, 42);
            this.numCamera.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numCamera.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCamera.Name = "numCamera";
            this.numCamera.Size = new System.Drawing.Size(60, 20);
            this.numCamera.TabIndex = 1;
            this.numCamera.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkAlertAll
            // 
            this.chkAlertAll.AutoSize = true;
            this.chkAlertAll.Location = new System.Drawing.Point(81, 45);
            this.chkAlertAll.Name = "chkAlertAll";
            this.chkAlertAll.Size = new System.Drawing.Size(79, 17);
            this.chkAlertAll.TabIndex = 2;
            this.chkAlertAll.Text = "all cameras";
            this.chkAlertAll.UseVisualStyleBackColor = true;
            // 
            // lblCamera
            // 
            this.lblCamera.AutoSize = true;
            this.lblCamera.Location = new System.Drawing.Point(6, 24);
            this.lblCamera.Name = "lblCamera";
            this.lblCamera.Size = new System.Drawing.Size(43, 13);
            this.lblCamera.TabIndex = 3;
            this.lblCamera.Text = "Camera";
            // 
            // AdminCntl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpAlert);
            this.Name = "AdminCntl";
            this.Size = new System.Drawing.Size(362, 226);
            this.grpAlert.ResumeLayout(false);
            this.grpAlert.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCamera)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpAlert;
        private System.Windows.Forms.Label lblCamera;
        private System.Windows.Forms.CheckBox chkAlertAll;
        private System.Windows.Forms.NumericUpDown numCamera;
        private System.Windows.Forms.Button btnAlert;
    }
}
