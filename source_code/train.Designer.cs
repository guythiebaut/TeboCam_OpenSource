namespace TeboCam
{
    partial class train
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(train));
            this.countVal = new System.Windows.Forms.TextBox();
            this.trainVal = new System.Windows.Forms.TextBox();
            this.startCountdown = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.label32 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // countVal
            // 
            this.countVal.BackColor = System.Drawing.Color.LemonChiffon;
            this.countVal.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.countVal.Location = new System.Drawing.Point(3, 72);
            this.countVal.MaxLength = 4;
            this.countVal.Name = "countVal";
            this.countVal.Size = new System.Drawing.Size(45, 20);
            this.countVal.TabIndex = 52;
            this.countVal.Text = "15";
            this.countVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.countVal, "Seconds to be counted down to the beginning of the recording process. ");
            this.countVal.TextChanged += new System.EventHandler(this.countVal_TextChanged);
            // 
            // trainVal
            // 
            this.trainVal.BackColor = System.Drawing.Color.LemonChiffon;
            this.trainVal.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trainVal.Location = new System.Drawing.Point(3, 107);
            this.trainVal.MaxLength = 4;
            this.trainVal.Name = "trainVal";
            this.trainVal.Size = new System.Drawing.Size(45, 20);
            this.trainVal.TabIndex = 53;
            this.trainVal.Text = "20";
            this.trainVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.trainVal, "Seconds to record movement for.");
            this.trainVal.TextChanged += new System.EventHandler(this.trainVal_TextChanged);
            // 
            // startCountdown
            // 
            this.startCountdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startCountdown.Location = new System.Drawing.Point(16, 149);
            this.startCountdown.Name = "startCountdown";
            this.startCountdown.Size = new System.Drawing.Size(116, 25);
            this.startCountdown.TabIndex = 64;
            this.startCountdown.Text = "Start Countdown";
            this.startCountdown.UseVisualStyleBackColor = true;
            this.startCountdown.Click += new System.EventHandler(this.startCountdown_Click);
            // 
            // cancel
            // 
            this.cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancel.Location = new System.Drawing.Point(151, 149);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(116, 25);
            this.cancel.TabIndex = 65;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.ForeColor = System.Drawing.Color.Black;
            this.label32.Location = new System.Drawing.Point(96, 9);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(66, 23);
            this.label32.TabIndex = 66;
            this.label32.Text = "Train";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(12, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(234, 23);
            this.label1.TabIndex = 67;
            this.label1.Text = "Movement Detection";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(54, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(207, 18);
            this.label2.TabIndex = 71;
            this.label2.Text = "Seconds to countdown";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(54, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 18);
            this.label3.TabIndex = 72;
            this.label3.Text = "Seconds to train";
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 20000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.ToolTipTitle = "Tip";
            // 
            // train
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 192);
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
            this.Name = "train";
            this.Load += new System.EventHandler(this.train_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox countVal;
        private System.Windows.Forms.TextBox trainVal;
        private System.Windows.Forms.Button startCountdown;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}