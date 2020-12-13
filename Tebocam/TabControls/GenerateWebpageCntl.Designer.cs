namespace TeboCam
{
    partial class GenerateWebpageCntl
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
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.label34 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.lblImgPref = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.button9 = new System.Windows.Forms.Button();
            this.groupBox10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.label34);
            this.groupBox10.Controls.Add(this.label28);
            this.groupBox10.Controls.Add(this.numericUpDown4);
            this.groupBox10.Controls.Add(this.lblImgPref);
            this.groupBox10.Controls.Add(this.numericUpDown3);
            this.groupBox10.Controls.Add(this.button9);
            this.groupBox10.Location = new System.Drawing.Point(0, 0);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(483, 141);
            this.groupBox10.TabIndex = 63;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Generate WebPage";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label34.ForeColor = System.Drawing.Color.Black;
            this.label34.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label34.Location = new System.Drawing.Point(117, 55);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(23, 13);
            this.label34.TabIndex = 62;
            this.label34.Text = "To";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label28.ForeColor = System.Drawing.Color.Black;
            this.label28.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label28.Location = new System.Drawing.Point(3, 55);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(41, 13);
            this.label28.TabIndex = 61;
            this.label28.Text = "From";
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.BackColor = System.Drawing.Color.LemonChiffon;
            this.numericUpDown4.Location = new System.Drawing.Point(146, 53);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.numericUpDown4.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(61, 20);
            this.numericUpDown4.TabIndex = 60;
            this.numericUpDown4.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown4.ValueChanged += new System.EventHandler(this.numericUpDown4_ValueChanged);
            // 
            // lblImgPref
            // 
            this.lblImgPref.AutoSize = true;
            this.lblImgPref.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblImgPref.ForeColor = System.Drawing.Color.Black;
            this.lblImgPref.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblImgPref.Location = new System.Drawing.Point(3, 28);
            this.lblImgPref.Name = "lblImgPref";
            this.lblImgPref.Size = new System.Drawing.Size(100, 13);
            this.lblImgPref.TabIndex = 36;
            this.lblImgPref.Text = "Image Prefix: ";
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.BackColor = System.Drawing.Color.LemonChiffon;
            this.numericUpDown3.Location = new System.Drawing.Point(50, 53);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.numericUpDown3.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(61, 20);
            this.numericUpDown3.TabIndex = 59;
            this.numericUpDown3.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown3_ValueChanged);
            // 
            // button9
            // 
            this.button9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button9.Location = new System.Drawing.Point(6, 80);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(181, 37);
            this.button9.TabIndex = 34;
            this.button9.Text = "Generate Webpage";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // GenerateWebpageCntl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox10);
            this.Name = "GenerateWebpageCntl";
            this.Size = new System.Drawing.Size(483, 141);
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.Label lblImgPref;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.Button button9;
    }
}
