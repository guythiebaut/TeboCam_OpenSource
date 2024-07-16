namespace TeboCam
{
    partial class LogFileManagementCntl
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
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.btnAdvancedFileManagement = new System.Windows.Forms.Button();
            this.groupBox19.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox19
            // 
            this.groupBox19.Controls.Add(this.btnAdvancedFileManagement);
            this.groupBox19.Location = new System.Drawing.Point(0, 0);
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.Size = new System.Drawing.Size(483, 89);
            this.groupBox19.TabIndex = 67;
            this.groupBox19.TabStop = false;
            this.groupBox19.Text = "Logs file management";
            // 
            // btnAdvancedFileManagement
            // 
            this.btnAdvancedFileManagement.Location = new System.Drawing.Point(16, 19);
            this.btnAdvancedFileManagement.Name = "btnAdvancedFileManagement";
            this.btnAdvancedFileManagement.Size = new System.Drawing.Size(442, 54);
            this.btnAdvancedFileManagement.TabIndex = 59;
            this.btnAdvancedFileManagement.Text = "Advanced file mangement options";
            this.btnAdvancedFileManagement.UseVisualStyleBackColor = true;
            this.btnAdvancedFileManagement.Click += new System.EventHandler(this.btnAdvancedFileManagement_Click);
            // 
            // LogFileManagementCntl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox19);
            this.Name = "LogFileManagementCntl";
            this.Size = new System.Drawing.Size(483, 89);
            this.groupBox19.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox19;
        private System.Windows.Forms.Button btnAdvancedFileManagement;
    }
}
