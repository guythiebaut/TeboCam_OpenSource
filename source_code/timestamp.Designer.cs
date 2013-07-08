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
            resources.ApplyResources(this.label32, "label32");
            this.label32.ForeColor = System.Drawing.Color.Black;
            this.label32.Name = "label32";
            // 
            // apply
            // 
            resources.ApplyResources(this.apply, "apply");
            this.apply.Name = "apply";
            this.toolTip1.SetToolTip(this.apply, resources.GetString("apply.ToolTip"));
            this.apply.UseVisualStyleBackColor = true;
            this.apply.Click += new System.EventHandler(this.apply_Click);
            // 
            // cancel
            // 
            resources.ApplyResources(this.cancel, "cancel");
            this.cancel.Name = "cancel";
            this.toolTip1.SetToolTip(this.cancel, resources.GetString("cancel.ToolTip"));
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
            resources.ApplyResources(this.addStamp, "addStamp");
            this.addStamp.BackColor = System.Drawing.SystemColors.Control;
            this.addStamp.Name = "addStamp";
            this.toolTip1.SetToolTip(this.addStamp, resources.GetString("addStamp.ToolTip"));
            this.addStamp.UseVisualStyleBackColor = false;
            this.addStamp.CheckedChanged += new System.EventHandler(this.addStamp_CheckedChanged);
            // 
            // br
            // 
            resources.ApplyResources(this.br, "br");
            this.br.Name = "br";
            this.toolTip1.SetToolTip(this.br, resources.GetString("br.ToolTip"));
            this.br.UseVisualStyleBackColor = true;
            // 
            // bl
            // 
            resources.ApplyResources(this.bl, "bl");
            this.bl.Name = "bl";
            this.toolTip1.SetToolTip(this.bl, resources.GetString("bl.ToolTip"));
            this.bl.UseVisualStyleBackColor = true;
            // 
            // tr
            // 
            resources.ApplyResources(this.tr, "tr");
            this.tr.Name = "tr";
            this.toolTip1.SetToolTip(this.tr, resources.GetString("tr.ToolTip"));
            this.tr.UseVisualStyleBackColor = true;
            // 
            // tl
            // 
            resources.ApplyResources(this.tl, "tl");
            this.tl.Checked = true;
            this.tl.Name = "tl";
            this.tl.TabStop = true;
            this.toolTip1.SetToolTip(this.tl, resources.GetString("tl.ToolTip"));
            this.tl.UseVisualStyleBackColor = true;
            // 
            // drawRect
            // 
            resources.ApplyResources(this.drawRect, "drawRect");
            this.drawRect.Name = "drawRect";
            this.toolTip1.SetToolTip(this.drawRect, resources.GetString("drawRect.ToolTip"));
            this.drawRect.UseVisualStyleBackColor = true;
            // 
            // statsChk
            // 
            resources.ApplyResources(this.statsChk, "statsChk");
            this.statsChk.Name = "statsChk";
            this.toolTip1.SetToolTip(this.statsChk, resources.GetString("statsChk.ToolTip"));
            this.statsChk.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.br);
            this.groupBox1.Controls.Add(this.bl);
            this.groupBox1.Controls.Add(this.tr);
            this.groupBox1.Controls.Add(this.tl);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.drawRect);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // statsBox
            // 
            this.statsBox.Controls.Add(this.statsChk);
            resources.ApplyResources(this.statsBox, "statsBox");
            this.statsBox.Name = "statsBox";
            this.statsBox.TabStop = false;
            // 
            // stampType
            // 
            this.stampType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.stampType.Items.AddRange(new object[] {
            resources.GetString("stampType.Items"),
            resources.GetString("stampType.Items1"),
            resources.GetString("stampType.Items2"),
            resources.GetString("stampType.Items3"),
            resources.GetString("stampType.Items4")});
            resources.ApplyResources(this.stampType, "stampType");
            this.stampType.Name = "stampType";
            // 
            // stampColour
            // 
            this.stampColour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.stampColour.Items.AddRange(new object[] {
            resources.GetString("stampColour.Items"),
            resources.GetString("stampColour.Items1"),
            resources.GetString("stampColour.Items2")});
            resources.ApplyResources(this.stampColour, "stampColour");
            this.stampColour.Name = "stampColour";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Name = "label3";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.Items.AddRange(new object[] {
            resources.GetString("comboBox1.Items"),
            resources.GetString("comboBox1.Items1"),
            resources.GetString("comboBox1.Items2"),
            resources.GetString("comboBox1.Items3")});
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // timestamp
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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