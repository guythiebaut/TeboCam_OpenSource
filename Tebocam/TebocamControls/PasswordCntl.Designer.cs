namespace TeboCam.TebocamControls
{
    partial class PasswordCntl
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
            this.PasswordField = new System.Windows.Forms.TextBox();
            this.PasswordShowHide = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // PasswordField
            // 
            this.PasswordField.Location = new System.Drawing.Point(0, 16);
            this.PasswordField.Name = "PasswordField";
            this.PasswordField.Size = new System.Drawing.Size(158, 20);
            this.PasswordField.TabIndex = 0;
            this.PasswordField.TextChanged += new System.EventHandler(this.PasswordField_TextChanged);
            // 
            // PasswordShowHide
            // 
            this.PasswordShowHide.Location = new System.Drawing.Point(160, 15);
            this.PasswordShowHide.Name = "PasswordShowHide";
            this.PasswordShowHide.Size = new System.Drawing.Size(43, 23);
            this.PasswordShowHide.TabIndex = 1;
            this.PasswordShowHide.Text = "Show";
            this.PasswordShowHide.UseVisualStyleBackColor = true;
            this.PasswordShowHide.Click += new System.EventHandler(this.PasswordShowHide_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(109, 13);
            this.lblTitle.TabIndex = 13;
            this.lblTitle.Text = "Title";
            // 
            // PasswordCntl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.PasswordShowHide);
            this.Controls.Add(this.PasswordField);
            this.Name = "PasswordCntl";
            this.Size = new System.Drawing.Size(210, 40);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox PasswordField;
        private System.Windows.Forms.Button PasswordShowHide;
        private System.Windows.Forms.Label lblTitle;
    }
}
