namespace TeboCam
{
    partial class LevelControl
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
            this.levelbox = new System.Windows.Forms.PictureBox();
            this.lbl0Perc = new System.Windows.Forms.Label();
            this.lbl100Perc = new System.Windows.Forms.Label();
            this.lbl75Perc = new System.Windows.Forms.Label();
            this.lbl25Perc = new System.Windows.Forms.Label();
            this.lbl50Perc = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.levelbox)).BeginInit();
            this.SuspendLayout();
            // 
            // levelbox
            // 
            this.levelbox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.levelbox.Location = new System.Drawing.Point(37, 13);
            this.levelbox.Name = "levelbox";
            this.levelbox.Size = new System.Drawing.Size(10, 243);
            this.levelbox.TabIndex = 43;
            this.levelbox.TabStop = false;
            this.levelbox.Paint += new System.Windows.Forms.PaintEventHandler(this.levelbox_Paint);
            // 
            // lbl0Perc
            // 
            this.lbl0Perc.AutoSize = true;
            this.lbl0Perc.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.lbl0Perc.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl0Perc.Location = new System.Drawing.Point(17, 249);
            this.lbl0Perc.Name = "lbl0Perc";
            this.lbl0Perc.Size = new System.Drawing.Size(14, 14);
            this.lbl0Perc.TabIndex = 52;
            this.lbl0Perc.Text = "0";
            // 
            // lbl100Perc
            // 
            this.lbl100Perc.AutoSize = true;
            this.lbl100Perc.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.lbl100Perc.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl100Perc.Location = new System.Drawing.Point(3, 6);
            this.lbl100Perc.Name = "lbl100Perc";
            this.lbl100Perc.Size = new System.Drawing.Size(28, 14);
            this.lbl100Perc.TabIndex = 51;
            this.lbl100Perc.Text = "100";
            // 
            // lbl75Perc
            // 
            this.lbl75Perc.AutoSize = true;
            this.lbl75Perc.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.lbl75Perc.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl75Perc.Location = new System.Drawing.Point(10, 66);
            this.lbl75Perc.Name = "lbl75Perc";
            this.lbl75Perc.Size = new System.Drawing.Size(21, 14);
            this.lbl75Perc.TabIndex = 50;
            this.lbl75Perc.Text = "75";
            // 
            // lbl25Perc
            // 
            this.lbl25Perc.AutoSize = true;
            this.lbl25Perc.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.lbl25Perc.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl25Perc.Location = new System.Drawing.Point(10, 186);
            this.lbl25Perc.Name = "lbl25Perc";
            this.lbl25Perc.Size = new System.Drawing.Size(21, 14);
            this.lbl25Perc.TabIndex = 49;
            this.lbl25Perc.Text = "25";
            // 
            // lbl50Perc
            // 
            this.lbl50Perc.AutoSize = true;
            this.lbl50Perc.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.lbl50Perc.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl50Perc.Location = new System.Drawing.Point(10, 127);
            this.lbl50Perc.Name = "lbl50Perc";
            this.lbl50Perc.Size = new System.Drawing.Size(21, 14);
            this.lbl50Perc.TabIndex = 48;
            this.lbl50Perc.Text = "50";
            // 
            // LevelControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.lbl0Perc);
            this.Controls.Add(this.lbl100Perc);
            this.Controls.Add(this.lbl75Perc);
            this.Controls.Add(this.lbl25Perc);
            this.Controls.Add(this.lbl50Perc);
            this.Controls.Add(this.levelbox);
            this.Name = "LevelControl";
            this.Size = new System.Drawing.Size(61, 284);
            ((System.ComponentModel.ISupportInitialize)(this.levelbox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox levelbox;
        private System.Windows.Forms.Label lbl0Perc;
        private System.Windows.Forms.Label lbl100Perc;
        private System.Windows.Forms.Label lbl75Perc;
        private System.Windows.Forms.Label lbl25Perc;
        private System.Windows.Forms.Label lbl50Perc;
    }
}
