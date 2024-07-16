namespace TeboCam
{
    partial class OnlineCntl
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
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.btnHealthcheck = new System.Windows.Forms.Button();
            this.txtEndpointHealth = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPickupDirectory = new System.Windows.Forms.TextBox();
            this.label67 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.rdApiLocal = new System.Windows.Forms.RadioButton();
            this.rdApiRemote = new System.Windows.Forms.RadioButton();
            this.txtEndpointLocal = new System.Windows.Forms.TextBox();
            this.label66 = new System.Windows.Forms.Label();
            this.txtEndpoint = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.disCommOnlineSecs = new System.Windows.Forms.TextBox();
            this.disCommOnline = new System.Windows.Forms.CheckBox();
            this.label51 = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.label63 = new System.Windows.Forms.Label();
            this.SqlFtpUser = new System.Windows.Forms.TextBox();
            this.SqlFtpPwd = new System.Windows.Forms.TextBox();
            this.button20 = new System.Windows.Forms.Button();
            this.webUpd = new System.Windows.Forms.CheckBox();
            this.label57 = new System.Windows.Forms.Label();
            this.sqlUser = new System.Windows.Forms.TextBox();
            this.sqlInstance = new System.Windows.Forms.TextBox();
            this.label46 = new System.Windows.Forms.Label();
            this.sqlPwd = new System.Windows.Forms.TextBox();
            this.label54 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.sqlPoll = new System.Windows.Forms.TextBox();
            this.label50 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.sqlImageFilename = new System.Windows.Forms.TextBox();
            this.sqlImageRoot = new System.Windows.Forms.TextBox();
            this.groupBox20.SuspendLayout();
            this.panel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox20
            // 
            this.groupBox20.Controls.Add(this.btnHealthcheck);
            this.groupBox20.Controls.Add(this.txtEndpointHealth);
            this.groupBox20.Controls.Add(this.label1);
            this.groupBox20.Controls.Add(this.txtPickupDirectory);
            this.groupBox20.Controls.Add(this.label67);
            this.groupBox20.Controls.Add(this.panel8);
            this.groupBox20.Controls.Add(this.txtEndpointLocal);
            this.groupBox20.Controls.Add(this.label66);
            this.groupBox20.Controls.Add(this.txtEndpoint);
            this.groupBox20.Controls.Add(this.label30);
            this.groupBox20.Controls.Add(this.label39);
            this.groupBox20.Controls.Add(this.disCommOnlineSecs);
            this.groupBox20.Controls.Add(this.disCommOnline);
            this.groupBox20.Controls.Add(this.label51);
            this.groupBox20.Controls.Add(this.label58);
            this.groupBox20.Controls.Add(this.label63);
            this.groupBox20.Controls.Add(this.SqlFtpUser);
            this.groupBox20.Controls.Add(this.SqlFtpPwd);
            this.groupBox20.Controls.Add(this.button20);
            this.groupBox20.Controls.Add(this.webUpd);
            this.groupBox20.Controls.Add(this.label57);
            this.groupBox20.Controls.Add(this.sqlUser);
            this.groupBox20.Controls.Add(this.sqlInstance);
            this.groupBox20.Controls.Add(this.label46);
            this.groupBox20.Controls.Add(this.sqlPwd);
            this.groupBox20.Controls.Add(this.label54);
            this.groupBox20.Controls.Add(this.label52);
            this.groupBox20.Controls.Add(this.sqlPoll);
            this.groupBox20.Controls.Add(this.label50);
            this.groupBox20.Controls.Add(this.label53);
            this.groupBox20.Controls.Add(this.sqlImageFilename);
            this.groupBox20.Controls.Add(this.sqlImageRoot);
            this.groupBox20.Location = new System.Drawing.Point(0, 0);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Size = new System.Drawing.Size(447, 539);
            this.groupBox20.TabIndex = 79;
            this.groupBox20.TabStop = false;
            // 
            // btnHealthcheck
            // 
            this.btnHealthcheck.Location = new System.Drawing.Point(367, 194);
            this.btnHealthcheck.Name = "btnHealthcheck";
            this.btnHealthcheck.Size = new System.Drawing.Size(74, 21);
            this.btnHealthcheck.TabIndex = 96;
            this.btnHealthcheck.Text = "Check";
            this.btnHealthcheck.UseVisualStyleBackColor = true;
            this.btnHealthcheck.Click += new System.EventHandler(this.btnHealthcheck_Click);
            // 
            // txtEndpointHealth
            // 
            this.txtEndpointHealth.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtEndpointHealth.Location = new System.Drawing.Point(157, 195);
            this.txtEndpointHealth.Name = "txtEndpointHealth";
            this.txtEndpointHealth.Size = new System.Drawing.Size(204, 20);
            this.txtEndpointHealth.TabIndex = 94;
            this.txtEndpointHealth.Leave += new System.EventHandler(this.txtEndpointHealth_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(38, 195);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 95;
            this.label1.Text = "Health endpoint";
            // 
            // txtPickupDirectory
            // 
            this.txtPickupDirectory.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtPickupDirectory.Location = new System.Drawing.Point(157, 169);
            this.txtPickupDirectory.Name = "txtPickupDirectory";
            this.txtPickupDirectory.Size = new System.Drawing.Size(284, 20);
            this.txtPickupDirectory.TabIndex = 92;
            this.txtPickupDirectory.Leave += new System.EventHandler(this.txtPickupDirectory_Leave);
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label67.Location = new System.Drawing.Point(38, 169);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(83, 13);
            this.label67.TabIndex = 93;
            this.label67.Text = "Pickup directory";
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.rdApiLocal);
            this.panel8.Controls.Add(this.rdApiRemote);
            this.panel8.Location = new System.Drawing.Point(3, 219);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(30, 54);
            this.panel8.TabIndex = 79;
            // 
            // rdApiLocal
            // 
            this.rdApiLocal.AutoSize = true;
            this.rdApiLocal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rdApiLocal.Location = new System.Drawing.Point(10, 34);
            this.rdApiLocal.Name = "rdApiLocal";
            this.rdApiLocal.Size = new System.Drawing.Size(14, 13);
            this.rdApiLocal.TabIndex = 79;
            this.rdApiLocal.UseVisualStyleBackColor = true;
            // 
            // rdApiRemote
            // 
            this.rdApiRemote.AutoSize = true;
            this.rdApiRemote.Checked = true;
            this.rdApiRemote.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rdApiRemote.Location = new System.Drawing.Point(10, 8);
            this.rdApiRemote.Name = "rdApiRemote";
            this.rdApiRemote.Size = new System.Drawing.Size(14, 13);
            this.rdApiRemote.TabIndex = 80;
            this.rdApiRemote.TabStop = true;
            this.rdApiRemote.UseVisualStyleBackColor = true;
            this.rdApiRemote.CheckedChanged += new System.EventHandler(this.rdApiRemote_CheckedChanged);
            // 
            // txtEndpointLocal
            // 
            this.txtEndpointLocal.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtEndpointLocal.Location = new System.Drawing.Point(157, 252);
            this.txtEndpointLocal.Name = "txtEndpointLocal";
            this.txtEndpointLocal.Size = new System.Drawing.Size(284, 20);
            this.txtEndpointLocal.TabIndex = 90;
            this.txtEndpointLocal.Leave += new System.EventHandler(this.txtEndpointLocal_Leave);
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label66.Location = new System.Drawing.Point(39, 252);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(77, 13);
            this.label66.TabIndex = 91;
            this.label66.Text = "Local endpoint";
            // 
            // txtEndpoint
            // 
            this.txtEndpoint.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtEndpoint.Location = new System.Drawing.Point(157, 225);
            this.txtEndpoint.Name = "txtEndpoint";
            this.txtEndpoint.Size = new System.Drawing.Size(284, 20);
            this.txtEndpoint.TabIndex = 88;
            this.txtEndpoint.Leave += new System.EventHandler(this.txtEndpoint_Leave);
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label30.Location = new System.Drawing.Point(38, 225);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(88, 13);
            this.label30.TabIndex = 89;
            this.label30.Text = "Remote endpoint";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label39.Location = new System.Drawing.Point(113, 485);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(49, 13);
            this.label39.TabIndex = 87;
            this.label39.Text = "Seconds";
            // 
            // disCommOnlineSecs
            // 
            this.disCommOnlineSecs.BackColor = System.Drawing.Color.LemonChiffon;
            this.disCommOnlineSecs.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.disCommOnlineSecs.ForeColor = System.Drawing.SystemColors.WindowText;
            this.disCommOnlineSecs.Location = new System.Drawing.Point(42, 477);
            this.disCommOnlineSecs.Name = "disCommOnlineSecs";
            this.disCommOnlineSecs.Size = new System.Drawing.Size(65, 21);
            this.disCommOnlineSecs.TabIndex = 86;
            this.disCommOnlineSecs.Text = "10";
            this.disCommOnlineSecs.Leave += new System.EventHandler(this.disCommOnlineSecs_Leave);
            // 
            // disCommOnline
            // 
            this.disCommOnline.AutoSize = true;
            this.disCommOnline.BackColor = System.Drawing.SystemColors.Control;
            this.disCommOnline.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.disCommOnline.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.disCommOnline.Location = new System.Drawing.Point(42, 454);
            this.disCommOnline.Name = "disCommOnline";
            this.disCommOnline.Size = new System.Drawing.Size(236, 17);
            this.disCommOnline.TabIndex = 85;
            this.disCommOnline.Text = "Disregard Commands older than";
            this.disCommOnline.UseVisualStyleBackColor = false;
            this.disCommOnline.CheckedChanged += new System.EventHandler(this.disCommOnline_CheckedChanged);
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label51.ForeColor = System.Drawing.Color.Black;
            this.label51.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label51.Location = new System.Drawing.Point(46, 387);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(126, 13);
            this.label51.TabIndex = 84;
            this.label51.Text = "Ftp Root Directory";
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label58.ForeColor = System.Drawing.Color.Black;
            this.label58.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label58.Location = new System.Drawing.Point(46, 357);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(94, 13);
            this.label58.TabIndex = 83;
            this.label58.Text = "Ftp Password";
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label63.ForeColor = System.Drawing.Color.Black;
            this.label63.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label63.Location = new System.Drawing.Point(46, 325);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(62, 13);
            this.label63.TabIndex = 82;
            this.label63.Text = "Ftp User";
            // 
            // SqlFtpUser
            // 
            this.SqlFtpUser.BackColor = System.Drawing.Color.LemonChiffon;
            this.SqlFtpUser.Location = new System.Drawing.Point(175, 322);
            this.SqlFtpUser.Name = "SqlFtpUser";
            this.SqlFtpUser.Size = new System.Drawing.Size(266, 20);
            this.SqlFtpUser.TabIndex = 81;
            this.SqlFtpUser.Leave += new System.EventHandler(this.SqlFtpUser_Leave);
            // 
            // SqlFtpPwd
            // 
            this.SqlFtpPwd.BackColor = System.Drawing.Color.LemonChiffon;
            this.SqlFtpPwd.Location = new System.Drawing.Point(175, 349);
            this.SqlFtpPwd.Name = "SqlFtpPwd";
            this.SqlFtpPwd.PasswordChar = '*';
            this.SqlFtpPwd.Size = new System.Drawing.Size(266, 20);
            this.SqlFtpPwd.TabIndex = 79;
            this.SqlFtpPwd.Leave += new System.EventHandler(this.SqlFtpPwd_Leave);
            // 
            // button20
            // 
            this.button20.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button20.Location = new System.Drawing.Point(49, 281);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(352, 25);
            this.button20.TabIndex = 78;
            this.button20.Text = "Copy FTP Settings from Email and Ftp";
            this.button20.UseVisualStyleBackColor = true;
            this.button20.Click += new System.EventHandler(this.button20_Click);
            // 
            // webUpd
            // 
            this.webUpd.AutoSize = true;
            this.webUpd.BackColor = System.Drawing.SystemColors.Control;
            this.webUpd.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.webUpd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.webUpd.Location = new System.Drawing.Point(10, 27);
            this.webUpd.Name = "webUpd";
            this.webUpd.Size = new System.Drawing.Size(104, 17);
            this.webUpd.TabIndex = 69;
            this.webUpd.Text = "Web Update";
            this.webUpd.UseVisualStyleBackColor = false;
            this.webUpd.CheckedChanged += new System.EventHandler(this.webUpd_CheckedChanged);
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label57.Location = new System.Drawing.Point(39, 114);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(48, 13);
            this.label57.TabIndex = 77;
            this.label57.Text = "Instance";
            // 
            // sqlUser
            // 
            this.sqlUser.BackColor = System.Drawing.Color.LemonChiffon;
            this.sqlUser.Location = new System.Drawing.Point(157, 52);
            this.sqlUser.Name = "sqlUser";
            this.sqlUser.Size = new System.Drawing.Size(125, 20);
            this.sqlUser.TabIndex = 0;
            this.sqlUser.Leave += new System.EventHandler(this.sqlUser_Leave);
            // 
            // sqlInstance
            // 
            this.sqlInstance.BackColor = System.Drawing.Color.LemonChiffon;
            this.sqlInstance.Enabled = false;
            this.sqlInstance.Location = new System.Drawing.Point(157, 106);
            this.sqlInstance.Name = "sqlInstance";
            this.sqlInstance.Size = new System.Drawing.Size(125, 20);
            this.sqlInstance.TabIndex = 76;
            this.sqlInstance.Leave += new System.EventHandler(this.sqlInstance_Leave);
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label46.Location = new System.Drawing.Point(39, 60);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(29, 13);
            this.label46.TabIndex = 5;
            this.label46.Text = "User";
            // 
            // sqlPwd
            // 
            this.sqlPwd.BackColor = System.Drawing.Color.LemonChiffon;
            this.sqlPwd.Location = new System.Drawing.Point(157, 79);
            this.sqlPwd.Name = "sqlPwd";
            this.sqlPwd.PasswordChar = '*';
            this.sqlPwd.Size = new System.Drawing.Size(125, 20);
            this.sqlPwd.TabIndex = 1;
            this.sqlPwd.Leave += new System.EventHandler(this.sqlPwd_Leave);
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label54.Location = new System.Drawing.Point(288, 144);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(53, 13);
            this.label54.TabIndex = 74;
            this.label54.Text = "(seconds)";
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label52.Location = new System.Drawing.Point(39, 87);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(53, 13);
            this.label52.TabIndex = 11;
            this.label52.Text = "Password";
            // 
            // sqlPoll
            // 
            this.sqlPoll.BackColor = System.Drawing.Color.LemonChiffon;
            this.sqlPoll.Location = new System.Drawing.Point(157, 136);
            this.sqlPoll.Name = "sqlPoll";
            this.sqlPoll.Size = new System.Drawing.Size(125, 20);
            this.sqlPoll.TabIndex = 12;
            this.sqlPoll.Leave += new System.EventHandler(this.sqlPoll_Leave);
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label50.Location = new System.Drawing.Point(42, 414);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(81, 13);
            this.label50.TabIndex = 72;
            this.label50.Text = "Image Filename";
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label53.Location = new System.Drawing.Point(39, 144);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(87, 13);
            this.label53.TabIndex = 13;
            this.label53.Text = "Poll Online Every";
            // 
            // sqlImageFilename
            // 
            this.sqlImageFilename.BackColor = System.Drawing.Color.LemonChiffon;
            this.sqlImageFilename.Location = new System.Drawing.Point(175, 406);
            this.sqlImageFilename.Name = "sqlImageFilename";
            this.sqlImageFilename.Size = new System.Drawing.Size(266, 20);
            this.sqlImageFilename.TabIndex = 71;
            this.sqlImageFilename.Leave += new System.EventHandler(this.sqlImageFilename_Leave);
            // 
            // sqlImageRoot
            // 
            this.sqlImageRoot.BackColor = System.Drawing.Color.LemonChiffon;
            this.sqlImageRoot.Location = new System.Drawing.Point(175, 379);
            this.sqlImageRoot.Name = "sqlImageRoot";
            this.sqlImageRoot.Size = new System.Drawing.Size(266, 20);
            this.sqlImageRoot.TabIndex = 70;
            this.sqlImageRoot.Leave += new System.EventHandler(this.sqlImageRoot_Leave);
            // 
            // OnlineCntl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox20);
            this.Name = "OnlineCntl";
            this.Size = new System.Drawing.Size(450, 539);
            this.groupBox20.ResumeLayout(false);
            this.groupBox20.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox20;
        private System.Windows.Forms.TextBox txtPickupDirectory;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.RadioButton rdApiLocal;
        private System.Windows.Forms.RadioButton rdApiRemote;
        private System.Windows.Forms.TextBox txtEndpointLocal;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.TextBox txtEndpoint;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.TextBox disCommOnlineSecs;
        private System.Windows.Forms.CheckBox disCommOnline;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.TextBox SqlFtpUser;
        private System.Windows.Forms.TextBox SqlFtpPwd;
        private System.Windows.Forms.Button button20;
        private System.Windows.Forms.CheckBox webUpd;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.TextBox sqlUser;
        private System.Windows.Forms.TextBox sqlInstance;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.TextBox sqlPwd;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.Label label52;
        internal System.Windows.Forms.TextBox sqlPoll;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.TextBox sqlImageFilename;
        private System.Windows.Forms.TextBox sqlImageRoot;
        private System.Windows.Forms.TextBox txtEndpointHealth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnHealthcheck;
    }
}
