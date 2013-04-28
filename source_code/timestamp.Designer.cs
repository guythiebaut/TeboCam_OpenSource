namespace TeboCam
{
    partial class timestamp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(timestamp));
            this.label32 = new System.Windows.Forms.Label();
            this.apply = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.addStamp = new System.Windows.Forms.CheckBox();
            this.br = new System.Windows.Forms.RadioButton();
            this.bl = new System.Windows.Forms.RadioButton();
            this.tr = new System.Windows.Forms.RadioButton();
            this.tl = new System.Windows.Forms.RadioButton();
            this.drawRect = new System.Windows.Forms.CheckBox();
            this.statsChk = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.statsBox = new System.Windows.Forms.GroupBox();
            this.stampType = new System.Windows.Forms.ComboBox();
            this.stampColour = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.statsBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.ForeColor = System.Drawing.Color.Black;
            this.label32.Location = new System.Drawing.Point(10, 9);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(139, 23);
            this.label32.TabIndex = 48;
            this.label32.Text = "Time Stamp";
            // 
            // apply
            // 
            this.apply.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.apply.Location = new System.Drawing.Point(171, 191);
            this.apply.Name = "apply";
            this.apply.Size = new System.Drawing.Size(132, 32);
            this.apply.TabIndex = 64;
            this.apply.Text = "Apply";
            this.toolTip1.SetToolTip(this.apply, "Apply the changes");
            this.apply.UseVisualStyleBackColor = true;
            this.apply.Click += new System.EventHandler(this.apply_Click);
            // 
            // cancel
            // 
            this.cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancel.Location = new System.Drawing.Point(316, 191);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(142, 32);
            this.cancel.TabIndex = 65;
            this.cancel.Text = "Cancel";
            this.toolTip1.SetToolTip(this.cancel, "Do not apply the changes");
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 20000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.ToolTipTitle = "Tip";
            // 
            // addStamp
            // 
            this.addStamp.AutoSize = true;
            this.addStamp.BackColor = System.Drawing.SystemColors.Control;
            this.addStamp.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.addStamp.Location = new System.Drawing.Point(12, 44);
            this.addStamp.Name = "addStamp";
            this.addStamp.Size = new System.Drawing.Size(226, 17);
            this.addStamp.TabIndex = 69;
            this.addStamp.Text = "Add date/time stamp to image";
            this.toolTip1.SetToolTip(this.addStamp, "Enable or disable webcam image publishing to webpage.");
            this.addStamp.UseVisualStyleBackColor = false;
            this.addStamp.CheckedChanged += new System.EventHandler(this.addStamp_CheckedChanged);
            // 
            // br
            // 
            this.br.AutoSize = true;
            this.br.Location = new System.Drawing.Point(89, 75);
            this.br.Name = "br";
            this.br.Size = new System.Drawing.Size(14, 13);
            this.br.TabIndex = 3;
            this.toolTip1.SetToolTip(this.br, "Text at lower right");
            this.br.UseVisualStyleBackColor = true;
            // 
            // bl
            // 
            this.bl.AutoSize = true;
            this.bl.Location = new System.Drawing.Point(21, 75);
            this.bl.Name = "bl";
            this.bl.Size = new System.Drawing.Size(14, 13);
            this.bl.TabIndex = 2;
            this.toolTip1.SetToolTip(this.bl, "Text at lower left");
            this.bl.UseVisualStyleBackColor = true;
            // 
            // tr
            // 
            this.tr.AutoSize = true;
            this.tr.Location = new System.Drawing.Point(89, 36);
            this.tr.Name = "tr";
            this.tr.Size = new System.Drawing.Size(14, 13);
            this.tr.TabIndex = 1;
            this.toolTip1.SetToolTip(this.tr, "Text at top right");
            this.tr.UseVisualStyleBackColor = true;
            // 
            // tl
            // 
            this.tl.AutoSize = true;
            this.tl.Checked = true;
            this.tl.Location = new System.Drawing.Point(21, 36);
            this.tl.Name = "tl";
            this.tl.Size = new System.Drawing.Size(14, 13);
            this.tl.TabIndex = 0;
            this.tl.TabStop = true;
            this.toolTip1.SetToolTip(this.tl, "Text at top left");
            this.tl.UseVisualStyleBackColor = true;
            // 
            // drawRect
            // 
            this.drawRect.AutoSize = true;
            this.drawRect.Location = new System.Drawing.Point(6, 19);
            this.drawRect.Name = "drawRect";
            this.drawRect.Size = new System.Drawing.Size(64, 17);
            this.drawRect.TabIndex = 72;
            this.drawRect.Text = "Opaque";
            this.toolTip1.SetToolTip(this.drawRect, "Show an opaque box behind the text or clock.");
            this.drawRect.UseVisualStyleBackColor = true;
            // 
            // statsChk
            // 
            this.statsChk.AutoSize = true;
            this.statsChk.Location = new System.Drawing.Point(6, 19);
            this.statsChk.Name = "statsChk";
            this.statsChk.Size = new System.Drawing.Size(106, 17);
            this.statsChk.TabIndex = 72;
            this.statsChk.Text = "Include Statistics";
            this.toolTip1.SetToolTip(this.statsChk, resources.GetString("statsChk.ToolTip"));
            this.statsChk.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.br);
            this.groupBox1.Controls.Add(this.bl);
            this.groupBox1.Controls.Add(this.tr);
            this.groupBox1.Controls.Add(this.tl);
            this.groupBox1.Location = new System.Drawing.Point(319, 77);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(141, 102);
            this.groupBox1.TabIndex = 68;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Time Stamp Position";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.drawRect);
            this.groupBox3.Location = new System.Drawing.Point(168, 78);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(145, 48);
            this.groupBox3.TabIndex = 71;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Time Stamp Background";
            // 
            // statsBox
            // 
            this.statsBox.Controls.Add(this.statsChk);
            this.statsBox.Location = new System.Drawing.Point(168, 132);
            this.statsBox.Name = "statsBox";
            this.statsBox.Size = new System.Drawing.Size(145, 48);
            this.statsBox.TabIndex = 73;
            this.statsBox.TabStop = false;
            this.statsBox.Text = "Publish timestamp Stats";
            // 
            // stampType
            // 
            this.stampType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.stampType.Items.AddRange(new object[] {
            "dd-mm-yy",
            "hh:mm:ss",
            "dd-mmm-yy hh:mm:ss",
            "Analogue Clock",
            "Analogue Clock + date"});
            this.stampType.Location = new System.Drawing.Point(9, 98);
            this.stampType.Name = "stampType";
            this.stampType.Size = new System.Drawing.Size(145, 21);
            this.stampType.TabIndex = 74;
            // 
            // stampColour
            // 
            this.stampColour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.stampColour.Items.AddRange(new object[] {
            "Red",
            "Black",
            "White"});
            this.stampColour.Location = new System.Drawing.Point(9, 157);
            this.stampColour.Name = "stampColour";
            this.stampColour.Size = new System.Drawing.Size(145, 21);
            this.stampColour.TabIndex = 75;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(6, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 76;
            this.label2.Text = "Time Stamp";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(6, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 77;
            this.label3.Text = "Time Stamp Colour";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.Items.AddRange(new object[] {
            "Alert",
            "Ping",
            "Publish",
            "Online"});
            this.comboBox1.Location = new System.Drawing.Point(155, 11);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(267, 28);
            this.comboBox1.TabIndex = 78;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // timestamp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 233);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.stampColour);
            this.Controls.Add(this.stampType);
            this.Controls.Add(this.statsBox);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.addStamp);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.apply);
            this.Controls.Add(this.label32);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "timestamp";
            this.Load += new System.EventHandler(this.timestamp_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.statsBox.ResumeLayout(false);
            this.statsBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Button apply;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton br;
        private System.Windows.Forms.RadioButton bl;
        private System.Windows.Forms.RadioButton tr;
        private System.Windows.Forms.RadioButton tl;
        private System.Windows.Forms.CheckBox addStamp;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox drawRect;
        private System.Windows.Forms.GroupBox statsBox;
        private System.Windows.Forms.CheckBox statsChk;
        private System.Windows.Forms.ComboBox stampType;
        private System.Windows.Forms.ComboBox stampColour;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}