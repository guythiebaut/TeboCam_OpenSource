namespace TeboCam
{
    partial class FileAdminCntl
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
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.lblFileType = new System.Windows.Forms.Label();
            this.FileTypeList = new System.Windows.Forms.ComboBox();
            this.btnZipAndVaultSelectedFiles = new System.Windows.Forms.Button();
            this.btnDeleteFiles = new System.Windows.Forms.Button();
            this.lblFileSize = new System.Windows.Forms.Label();
            this.lblFileCount = new System.Windows.Forms.Label();
            this.logsKeep = new System.Windows.Forms.TextBox();
            this.logsKeepChk = new System.Windows.Forms.CheckBox();
            this.groupBox12.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.logsKeep);
            this.groupBox12.Controls.Add(this.logsKeepChk);
            this.groupBox12.Controls.Add(this.lblFileType);
            this.groupBox12.Controls.Add(this.FileTypeList);
            this.groupBox12.Controls.Add(this.btnZipAndVaultSelectedFiles);
            this.groupBox12.Controls.Add(this.btnDeleteFiles);
            this.groupBox12.Controls.Add(this.lblFileSize);
            this.groupBox12.Controls.Add(this.lblFileCount);
            this.groupBox12.Location = new System.Drawing.Point(16, 15);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(349, 337);
            this.groupBox12.TabIndex = 73;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "File adminmistration";
            // 
            // lblFileType
            // 
            this.lblFileType.AutoSize = true;
            this.lblFileType.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblFileType.ForeColor = System.Drawing.Color.Black;
            this.lblFileType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblFileType.Location = new System.Drawing.Point(16, 44);
            this.lblFileType.Name = "lblFileType";
            this.lblFileType.Size = new System.Drawing.Size(64, 13);
            this.lblFileType.TabIndex = 71;
            this.lblFileType.Text = "File type";
            // 
            // FileTypeList
            // 
            this.FileTypeList.BackColor = System.Drawing.Color.LemonChiffon;
            this.FileTypeList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FileTypeList.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.FileTypeList.FormattingEnabled = true;
            this.FileTypeList.Location = new System.Drawing.Point(96, 41);
            this.FileTypeList.Name = "FileTypeList";
            this.FileTypeList.Size = new System.Drawing.Size(222, 21);
            this.FileTypeList.TabIndex = 70;
            this.FileTypeList.SelectedIndexChanged += new System.EventHandler(this.FileTypeList_SelectedIndexChanged);
            // 
            // btnZipAndVaultSelectedFiles
            // 
            this.btnZipAndVaultSelectedFiles.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnZipAndVaultSelectedFiles.Location = new System.Drawing.Point(19, 186);
            this.btnZipAndVaultSelectedFiles.Name = "btnZipAndVaultSelectedFiles";
            this.btnZipAndVaultSelectedFiles.Size = new System.Drawing.Size(157, 32);
            this.btnZipAndVaultSelectedFiles.TabIndex = 39;
            this.btnZipAndVaultSelectedFiles.Text = "Zip and vault selected files";
            this.btnZipAndVaultSelectedFiles.UseVisualStyleBackColor = true;
            this.btnZipAndVaultSelectedFiles.Click += new System.EventHandler(this.btnZipAndVaultSelectedFiles_Click);
            // 
            // btnDeleteFiles
            // 
            this.btnDeleteFiles.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDeleteFiles.Location = new System.Drawing.Point(19, 137);
            this.btnDeleteFiles.Name = "btnDeleteFiles";
            this.btnDeleteFiles.Size = new System.Drawing.Size(157, 32);
            this.btnDeleteFiles.TabIndex = 38;
            this.btnDeleteFiles.Text = "Delete selected files";
            this.btnDeleteFiles.UseVisualStyleBackColor = true;
            this.btnDeleteFiles.Click += new System.EventHandler(this.btnDeleteFiles_Click);
            // 
            // lblFileSize
            // 
            this.lblFileSize.AutoSize = true;
            this.lblFileSize.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblFileSize.ForeColor = System.Drawing.Color.Black;
            this.lblFileSize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblFileSize.Location = new System.Drawing.Point(16, 103);
            this.lblFileSize.Name = "lblFileSize";
            this.lblFileSize.Size = new System.Drawing.Size(61, 13);
            this.lblFileSize.TabIndex = 37;
            this.lblFileSize.Text = "File size";
            // 
            // lblFileCount
            // 
            this.lblFileCount.AutoSize = true;
            this.lblFileCount.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblFileCount.ForeColor = System.Drawing.Color.Black;
            this.lblFileCount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblFileCount.Location = new System.Drawing.Point(16, 80);
            this.lblFileCount.Name = "lblFileCount";
            this.lblFileCount.Size = new System.Drawing.Size(71, 13);
            this.lblFileCount.TabIndex = 36;
            this.lblFileCount.Text = "File count";
            // 
            // logsKeep
            // 
            this.logsKeep.BackColor = System.Drawing.Color.LemonChiffon;
            this.logsKeep.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.logsKeep.ForeColor = System.Drawing.SystemColors.WindowText;
            this.logsKeep.Location = new System.Drawing.Point(19, 292);
            this.logsKeep.Name = "logsKeep";
            this.logsKeep.Size = new System.Drawing.Size(65, 21);
            this.logsKeep.TabIndex = 73;
            this.logsKeep.Text = "30";
            // 
            // logsKeepChk
            // 
            this.logsKeepChk.AutoSize = true;
            this.logsKeepChk.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.logsKeepChk.Location = new System.Drawing.Point(19, 259);
            this.logsKeepChk.Name = "logsKeepChk";
            this.logsKeepChk.Size = new System.Drawing.Size(242, 17);
            this.logsKeepChk.TabIndex = 72;
            this.logsKeepChk.Text = "Only keep logs for last N days specified below";
            this.logsKeepChk.UseVisualStyleBackColor = true;
            this.logsKeepChk.CheckedChanged += new System.EventHandler(this.logsKeepChk_CheckedChanged);
            // 
            // FileAdminCntl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox12);
            this.Name = "FileAdminCntl";
            this.Size = new System.Drawing.Size(399, 387);
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.Button btnZipAndVaultSelectedFiles;
        private System.Windows.Forms.Button btnDeleteFiles;
        private System.Windows.Forms.Label lblFileSize;
        private System.Windows.Forms.Label lblFileCount;
        private System.Windows.Forms.ComboBox FileTypeList;
        private System.Windows.Forms.Label lblFileType;
        private System.Windows.Forms.TextBox logsKeep;
        private System.Windows.Forms.CheckBox logsKeepChk;
    }
}
