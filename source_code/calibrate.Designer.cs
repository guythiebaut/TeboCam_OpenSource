namespace TeboCam
{
    partial class calibrate
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(calibrate));
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.cancel = new System.Windows.Forms.Button();
            this.startCountdown = new System.Windows.Forms.Button();
            this.trainVal = new System.Windows.Forms.TextBox();
            this.countVal = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(67, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(182, 18);
            this.label3.TabIndex = 80;
            this.label3.Text = "Image save interval";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(67, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 18);
            this.label2.TabIndex = 79;
            this.label2.Text = "Seconds to record";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(234, 23);
            this.label1.TabIndex = 78;
            this.label1.Text = "Movement Detection";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.ForeColor = System.Drawing.Color.Black;
            this.label32.Location = new System.Drawing.Point(73, 12);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(108, 23);
            this.label32.TabIndex = 77;
            this.label32.Text = "Calibrate";
            // 
            // cancel
            // 
            this.cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancel.Location = new System.Drawing.Point(147, 151);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(116, 25);
            this.cancel.TabIndex = 76;
            this.cancel.Text = "Exit";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // startCountdown
            // 
            this.startCountdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startCountdown.Location = new System.Drawing.Point(12, 151);
            this.startCountdown.Name = "startCountdown";
            this.startCountdown.Size = new System.Drawing.Size(116, 25);
            this.startCountdown.TabIndex = 75;
            this.startCountdown.Text = "Start Calibration";
            this.toolTip1.SetToolTip(this.startCountdown, resources.GetString("startCountdown.ToolTip"));
            this.startCountdown.UseVisualStyleBackColor = true;
            this.startCountdown.Click += new System.EventHandler(this.startCountdown_Click);
            // 
            // trainVal
            // 
            this.trainVal.BackColor = System.Drawing.Color.LemonChiffon;
            this.trainVal.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trainVal.Location = new System.Drawing.Point(16, 110);
            this.trainVal.MaxLength = 4;
            this.trainVal.Name = "trainVal";
            this.trainVal.Size = new System.Drawing.Size(45, 20);
            this.trainVal.TabIndex = 74;
            this.trainVal.Text = "0.5";
            this.trainVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.trainVal, "The frequency at which to save images.\r\n\r\nThe recomended value is 0.5\r\n(0.5 will " +
                    "save two images every second)");
            this.trainVal.Leave += new System.EventHandler(this.trainVal_Leave);
            // 
            // countVal
            // 
            this.countVal.BackColor = System.Drawing.Color.LemonChiffon;
            this.countVal.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.countVal.Location = new System.Drawing.Point(16, 75);
            this.countVal.MaxLength = 4;
            this.countVal.Name = "countVal";
            this.countVal.Size = new System.Drawing.Size(45, 20);
            this.countVal.TabIndex = 73;
            this.countVal.Text = "30";
            this.countVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.countVal, "The number of seconds to run the calibration test for.\r\n\r\nThe recommended value i" +
                    "s 30.\r\n");
            this.countVal.Leave += new System.EventHandler(this.countVal_Leave);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 20000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.ToolTipTitle = "Tip";
            // 
            // calibrate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 194);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label32);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.startCountdown);
            this.Controls.Add(this.trainVal);
            this.Controls.Add(this.countVal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "calibrate";
            this.Load += new System.EventHandler(this.calibrate_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.calibrate_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button startCountdown;
        private System.Windows.Forms.TextBox trainVal;
        private System.Windows.Forms.TextBox countVal;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}