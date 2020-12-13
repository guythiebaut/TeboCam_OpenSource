namespace TeboCam
{
    partial class ImagesSavedFolderCntl
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
            this.groupBox21 = new System.Windows.Forms.GroupBox();
            this.radioButton11 = new System.Windows.Forms.RadioButton();
            this.button21 = new System.Windows.Forms.Button();
            this.radioButton10 = new System.Windows.Forms.RadioButton();
            this.groupBox21.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox21
            // 
            this.groupBox21.Controls.Add(this.radioButton11);
            this.groupBox21.Controls.Add(this.button21);
            this.groupBox21.Controls.Add(this.radioButton10);
            this.groupBox21.Location = new System.Drawing.Point(0, 0);
            this.groupBox21.Name = "groupBox21";
            this.groupBox21.Size = new System.Drawing.Size(423, 81);
            this.groupBox21.TabIndex = 65;
            this.groupBox21.TabStop = false;
            this.groupBox21.Text = "Images Saved Folder";
            // 
            // radioButton11
            // 
            this.radioButton11.AutoSize = true;
            this.radioButton11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radioButton11.Location = new System.Drawing.Point(15, 46);
            this.radioButton11.Name = "radioButton11";
            this.radioButton11.Size = new System.Drawing.Size(100, 17);
            this.radioButton11.TabIndex = 1;
            this.radioButton11.Text = "Custom location";
            this.radioButton11.UseVisualStyleBackColor = true;
            this.radioButton11.CheckedChanged += new System.EventHandler(this.radioButton11_CheckedChanged);
            // 
            // button21
            // 
            this.button21.Enabled = false;
            this.button21.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.button21.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button21.Location = new System.Drawing.Point(156, 46);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(102, 21);
            this.button21.TabIndex = 61;
            this.button21.Text = "Select folder...";
            this.button21.UseVisualStyleBackColor = true;
            this.button21.Click += new System.EventHandler(this.button21_Click);
            // 
            // radioButton10
            // 
            this.radioButton10.AutoSize = true;
            this.radioButton10.Checked = true;
            this.radioButton10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radioButton10.Location = new System.Drawing.Point(15, 23);
            this.radioButton10.Name = "radioButton10";
            this.radioButton10.Size = new System.Drawing.Size(99, 17);
            this.radioButton10.TabIndex = 0;
            this.radioButton10.TabStop = true;
            this.radioButton10.Text = "Default location";
            this.radioButton10.UseVisualStyleBackColor = true;
            // 
            // ImagesSavedFolderCntl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox21);
            this.Name = "ImagesSavedFolderCntl";
            this.Size = new System.Drawing.Size(423, 81);
            this.groupBox21.ResumeLayout(false);
            this.groupBox21.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox21;
        private System.Windows.Forms.RadioButton radioButton11;
        private System.Windows.Forms.Button button21;
        private System.Windows.Forms.RadioButton radioButton10;
    }
}
