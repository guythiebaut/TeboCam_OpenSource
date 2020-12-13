namespace TeboCam
{
    partial class InternetConnectionCheckCntl
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
            this.groupBox22 = new System.Windows.Forms.GroupBox();
            this.txtInternetConnection = new System.Windows.Forms.TextBox();
            this.groupBox22.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox22
            // 
            this.groupBox22.Controls.Add(this.txtInternetConnection);
            this.groupBox22.Location = new System.Drawing.Point(0, 0);
            this.groupBox22.Name = "groupBox22";
            this.groupBox22.Size = new System.Drawing.Size(483, 60);
            this.groupBox22.TabIndex = 70;
            this.groupBox22.TabStop = false;
            this.groupBox22.Text = "Internet Connection Check";
            // 
            // txtInternetConnection
            // 
            this.txtInternetConnection.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtInternetConnection.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtInternetConnection.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtInternetConnection.Location = new System.Drawing.Point(6, 20);
            this.txtInternetConnection.Name = "txtInternetConnection";
            this.txtInternetConnection.Size = new System.Drawing.Size(471, 21);
            this.txtInternetConnection.TabIndex = 67;
            this.txtInternetConnection.Leave += new System.EventHandler(this.txtInternetConnection_Leave);
            // 
            // InternetConnectionCheckCntl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox22);
            this.Name = "InternetConnectionCheckCntl";
            this.Size = new System.Drawing.Size(483, 60);
            this.groupBox22.ResumeLayout(false);
            this.groupBox22.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox22;
        private System.Windows.Forms.TextBox txtInternetConnection;
    }
}
