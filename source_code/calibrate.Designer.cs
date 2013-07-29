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
            this.label32 = new System.Windows.Forms.Label();
            this.cancel = new System.Windows.Forms.Button();
            this.startCountdown = new System.Windows.Forms.Button();
            this.trainVal = new System.Windows.Forms.TextBox();
            this.countVal = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnlControls = new System.Windows.Forms.Panel();
            this.grpSpikeSettings = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblSensitivity = new System.Windows.Forms.Label();
            this.lblToleranceSpike = new System.Windows.Forms.Label();
            this.trkMov = new System.Windows.Forms.TrackBar();
            this.lblTimeSpike = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.trkTimeSpike = new System.Windows.Forms.TrackBar();
            this.trkToleranceSpike = new System.Windows.Forms.TrackBar();
            this.grpSpikeSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkMov)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkTimeSpike)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkToleranceSpike)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Name = "label2";
            // 
            // label32
            // 
            resources.ApplyResources(this.label32, "label32");
            this.label32.ForeColor = System.Drawing.Color.Black;
            this.label32.Name = "label32";
            // 
            // cancel
            // 
            resources.ApplyResources(this.cancel, "cancel");
            this.cancel.Name = "cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // startCountdown
            // 
            resources.ApplyResources(this.startCountdown, "startCountdown");
            this.startCountdown.Name = "startCountdown";
            this.toolTip1.SetToolTip(this.startCountdown, resources.GetString("startCountdown.ToolTip"));
            this.startCountdown.UseVisualStyleBackColor = true;
            this.startCountdown.Click += new System.EventHandler(this.startCountdown_Click);
            // 
            // trainVal
            // 
            this.trainVal.BackColor = System.Drawing.Color.LemonChiffon;
            resources.ApplyResources(this.trainVal, "trainVal");
            this.trainVal.Name = "trainVal";
            this.toolTip1.SetToolTip(this.trainVal, resources.GetString("trainVal.ToolTip"));
            this.trainVal.Leave += new System.EventHandler(this.trainVal_Leave);
            // 
            // countVal
            // 
            this.countVal.BackColor = System.Drawing.Color.LemonChiffon;
            resources.ApplyResources(this.countVal, "countVal");
            this.countVal.Name = "countVal";
            this.toolTip1.SetToolTip(this.countVal, resources.GetString("countVal.ToolTip"));
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
            // pnlControls
            // 
            resources.ApplyResources(this.pnlControls, "pnlControls");
            this.pnlControls.Name = "pnlControls";
            // 
            // grpSpikeSettings
            // 
            this.grpSpikeSettings.Controls.Add(this.label6);
            this.grpSpikeSettings.Controls.Add(this.lblSensitivity);
            this.grpSpikeSettings.Controls.Add(this.lblToleranceSpike);
            this.grpSpikeSettings.Controls.Add(this.trkMov);
            this.grpSpikeSettings.Controls.Add(this.lblTimeSpike);
            this.grpSpikeSettings.Controls.Add(this.label1);
            this.grpSpikeSettings.Controls.Add(this.label4);
            this.grpSpikeSettings.Controls.Add(this.trkTimeSpike);
            this.grpSpikeSettings.Controls.Add(this.trkToleranceSpike);
            resources.ApplyResources(this.grpSpikeSettings, "grpSpikeSettings");
            this.grpSpikeSettings.Name = "grpSpikeSettings";
            this.grpSpikeSettings.TabStop = false;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Name = "label6";
            // 
            // lblSensitivity
            // 
            resources.ApplyResources(this.lblSensitivity, "lblSensitivity");
            this.lblSensitivity.ForeColor = System.Drawing.Color.Black;
            this.lblSensitivity.Name = "lblSensitivity";
            // 
            // lblToleranceSpike
            // 
            resources.ApplyResources(this.lblToleranceSpike, "lblToleranceSpike");
            this.lblToleranceSpike.ForeColor = System.Drawing.Color.Black;
            this.lblToleranceSpike.Name = "lblToleranceSpike";
            // 
            // trkMov
            // 
            resources.ApplyResources(this.trkMov, "trkMov");
            this.trkMov.Maximum = 100;
            this.trkMov.Minimum = 1;
            this.trkMov.Name = "trkMov";
            this.trkMov.TickFrequency = 10;
            this.trkMov.Value = 1;
            this.trkMov.Scroll += new System.EventHandler(this.trkMov_Scroll);
            // 
            // lblTimeSpike
            // 
            resources.ApplyResources(this.lblTimeSpike, "lblTimeSpike");
            this.lblTimeSpike.ForeColor = System.Drawing.Color.Black;
            this.lblTimeSpike.Name = "lblTimeSpike";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Name = "label1";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Name = "label4";
            // 
            // trkTimeSpike
            // 
            resources.ApplyResources(this.trkTimeSpike, "trkTimeSpike");
            this.trkTimeSpike.LargeChange = 1000;
            this.trkTimeSpike.Maximum = 10000;
            this.trkTimeSpike.Minimum = 100;
            this.trkTimeSpike.Name = "trkTimeSpike";
            this.trkTimeSpike.SmallChange = 500;
            this.trkTimeSpike.TickFrequency = 500;
            this.trkTimeSpike.Value = 500;
            this.trkTimeSpike.Scroll += new System.EventHandler(this.trkTimeSpike_Scroll);
            // 
            // trkToleranceSpike
            // 
            resources.ApplyResources(this.trkToleranceSpike, "trkToleranceSpike");
            this.trkToleranceSpike.LargeChange = 20;
            this.trkToleranceSpike.Maximum = 100;
            this.trkToleranceSpike.Name = "trkToleranceSpike";
            this.trkToleranceSpike.SmallChange = 2;
            this.trkToleranceSpike.TickFrequency = 10;
            this.trkToleranceSpike.Scroll += new System.EventHandler(this.trkToleranceSpike_Scroll);
            // 
            // calibrate
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpSpikeSettings);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label32);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.startCountdown);
            this.Controls.Add(this.trainVal);
            this.Controls.Add(this.countVal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "calibrate";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.calibrate_FormClosing);
            this.Load += new System.EventHandler(this.calibrate_Load);
            this.grpSpikeSettings.ResumeLayout(false);
            this.grpSpikeSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkMov)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkTimeSpike)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkToleranceSpike)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button startCountdown;
        private System.Windows.Forms.TextBox trainVal;
        private System.Windows.Forms.TextBox countVal;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel pnlControls;
        private System.Windows.Forms.GroupBox grpSpikeSettings;
        private System.Windows.Forms.Label lblToleranceSpike;
        private System.Windows.Forms.Label lblTimeSpike;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar trkTimeSpike;
        private System.Windows.Forms.TrackBar trkToleranceSpike;
        private System.Windows.Forms.TrackBar trkMov;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblSensitivity;
    }
}