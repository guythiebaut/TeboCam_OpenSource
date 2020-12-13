namespace TeboCam
{
    partial class AlertFilenameCntl
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
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.button35 = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.button35);
            this.groupBox6.Controls.Add(this.label12);
            this.groupBox6.Location = new System.Drawing.Point(0, 0);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(423, 110);
            this.groupBox6.TabIndex = 54;
            this.groupBox6.TabStop = false;
            // 
            // button35
            // 
            this.button35.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button35.Location = new System.Drawing.Point(31, 49);
            this.button35.Name = "button35";
            this.button35.Size = new System.Drawing.Size(256, 44);
            this.button35.TabIndex = 95;
            this.button35.Text = "Set Alert File Name";
            this.button35.UseVisualStyleBackColor = true;
            this.button35.Click += new System.EventHandler(this.button35_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold);
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(21, 17);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(266, 23);
            this.label12.TabIndex = 23;
            this.label12.Text = "Alert Filename Settings";
            // 
            // AlertFilenameCntl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox6);
            this.Name = "AlertFilenameCntl";
            this.Size = new System.Drawing.Size(423, 110);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button button35;
        private System.Windows.Forms.Label label12;
    }
}
