namespace TeboCam
{
    partial class ProfilesCntl
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
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.btnRunProfileCommand = new System.Windows.Forms.Button();
            this.label65 = new System.Windows.Forms.Label();
            this.ProfileCommandList = new System.Windows.Forms.ComboBox();
            this.label45 = new System.Windows.Forms.Label();
            this.profileList = new System.Windows.Forms.ComboBox();
            this.groupBox15.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.btnRunProfileCommand);
            this.groupBox15.Controls.Add(this.label65);
            this.groupBox15.Controls.Add(this.ProfileCommandList);
            this.groupBox15.Controls.Add(this.label45);
            this.groupBox15.Controls.Add(this.profileList);
            this.groupBox15.Location = new System.Drawing.Point(0, 0);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(353, 130);
            this.groupBox15.TabIndex = 64;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Profiles";
            // 
            // btnRunProfileCommand
            // 
            this.btnRunProfileCommand.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRunProfileCommand.Location = new System.Drawing.Point(6, 80);
            this.btnRunProfileCommand.Name = "btnRunProfileCommand";
            this.btnRunProfileCommand.Size = new System.Drawing.Size(114, 37);
            this.btnRunProfileCommand.TabIndex = 71;
            this.btnRunProfileCommand.Text = "Run profile command";
            this.btnRunProfileCommand.UseVisualStyleBackColor = true;
            this.btnRunProfileCommand.Click += new System.EventHandler(this.btnRunProfileCommand_Click);
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label65.ForeColor = System.Drawing.Color.Black;
            this.label65.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label65.Location = new System.Drawing.Point(6, 55);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(117, 13);
            this.label65.TabIndex = 70;
            this.label65.Text = "Profile command";
            // 
            // ProfileCommandList
            // 
            this.ProfileCommandList.BackColor = System.Drawing.Color.LemonChiffon;
            this.ProfileCommandList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProfileCommandList.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.ProfileCommandList.FormattingEnabled = true;
            this.ProfileCommandList.Items.AddRange(new object[] {
            "Select a command to run...",
            "Vault profiles",
            "New profile",
            "Copy profile",
            "Rename profile",
            "Delete profile",
            "Remove unused camera profiles"});
            this.ProfileCommandList.Location = new System.Drawing.Point(125, 55);
            this.ProfileCommandList.Name = "ProfileCommandList";
            this.ProfileCommandList.Size = new System.Drawing.Size(222, 21);
            this.ProfileCommandList.TabIndex = 69;
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label45.ForeColor = System.Drawing.Color.Black;
            this.label45.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label45.Location = new System.Drawing.Point(4, 28);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(103, 13);
            this.label45.TabIndex = 65;
            this.label45.Text = "Current profile";
            // 
            // profileList
            // 
            this.profileList.BackColor = System.Drawing.Color.LemonChiffon;
            this.profileList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.profileList.FormattingEnabled = true;
            this.profileList.Location = new System.Drawing.Point(125, 25);
            this.profileList.Name = "profileList";
            this.profileList.Size = new System.Drawing.Size(222, 21);
            this.profileList.TabIndex = 36;
            this.profileList.SelectedIndexChanged += new System.EventHandler(this.profileList_SelectedIndexChanged);
            // 
            // ProfilesCntl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox15);
            this.Name = "ProfilesCntl";
            this.Size = new System.Drawing.Size(353, 130);
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.Button btnRunProfileCommand;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.ComboBox ProfileCommandList;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.ComboBox profileList;
    }
}
