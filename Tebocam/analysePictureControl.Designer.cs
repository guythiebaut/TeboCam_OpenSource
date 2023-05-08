namespace TeboCam
{
    partial class analysePictureControl
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
            this.imageBorder = new System.Windows.Forms.PictureBox();
            this.imageBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.imageBorder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // imageBorder
            // 
            this.imageBorder.BackColor = System.Drawing.SystemColors.Control;
            this.imageBorder.Location = new System.Drawing.Point(0, 0);
            this.imageBorder.Name = "imageBorder";
            this.imageBorder.Size = new System.Drawing.Size(120, 120);
            this.imageBorder.TabIndex = 0;
            this.imageBorder.TabStop = false;
            this.imageBorder.Paint += new System.Windows.Forms.PaintEventHandler(this.imageBorder_Paint);
            // 
            // imageBox
            // 
            this.imageBox.Location = new System.Drawing.Point(10, 10);
            this.imageBox.Name = "imageBox";
            this.imageBox.Size = new System.Drawing.Size(100, 100);
            this.imageBox.TabIndex = 1;
            this.imageBox.TabStop = false;
            // 
            // analysePictureControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.imageBox);
            this.Controls.Add(this.imageBorder);
            this.Name = "analysePictureControl";
            this.Size = new System.Drawing.Size(120, 120);
            ((System.ComponentModel.ISupportInitialize)(this.imageBorder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox imageBorder;
        private System.Windows.Forms.PictureBox imageBox;
    }
}
