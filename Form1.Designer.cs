namespace IISWizard
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.siteView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.StartSite = new System.Windows.Forms.ToolStripMenuItem();
            this.SuspendSite = new System.Windows.Forms.ToolStripMenuItem();
            this.StopSite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.RePassword = new System.Windows.Forms.ToolStripMenuItem();
            this.ReRights = new System.Windows.Forms.ToolStripMenuItem();
            this.DelSite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.OpenExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenSite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.doExit = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.AddSite = new System.Windows.Forms.Button();
            this.IPs = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Address = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.browserLog = new System.Windows.Forms.Button();
            this.LogFilePath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.browser = new System.Windows.Forms.Button();
            this.sitePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.webName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.siteView);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(599, 273);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "网站列表";
            // 
            // siteView
            // 
            this.siteView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.siteView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.siteView.ContextMenuStrip = this.contextMenuStrip1;
            this.siteView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.siteView.FullRowSelect = true;
            this.siteView.GridLines = true;
            this.siteView.HoverSelection = true;
            this.siteView.Location = new System.Drawing.Point(3, 17);
            this.siteView.MultiSelect = false;
            this.siteView.Name = "siteView";
            this.siteView.Size = new System.Drawing.Size(593, 253);
            this.siteView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.siteView.TabIndex = 0;
            this.siteView.UseCompatibleStateImageBehavior = false;
            this.siteView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "网站ID";
            this.columnHeader1.Width = 49;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StartSite,
            this.SuspendSite,
            this.StopSite,
            this.toolStripSeparator1,
            this.RePassword,
            this.ReRights,
            this.DelSite,
            this.toolStripSeparator2,
            this.OpenExplorer,
            this.OpenSite,
            this.toolStripSeparator3,
            this.doExit});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 220);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.CheckWebState);
            // 
            // StartSite
            // 
            this.StartSite.Name = "StartSite";
            this.StartSite.Size = new System.Drawing.Size(148, 22);
            this.StartSite.Text = "启动(&S)";
            this.StartSite.Click += new System.EventHandler(this.StartSite_Click);
            // 
            // SuspendSite
            // 
            this.SuspendSite.Name = "SuspendSite";
            this.SuspendSite.Size = new System.Drawing.Size(148, 22);
            this.SuspendSite.Text = "暂停(&P)";
            this.SuspendSite.Click += new System.EventHandler(this.SuspendSite_Click);
            // 
            // StopSite
            // 
            this.StopSite.Name = "StopSite";
            this.StopSite.Size = new System.Drawing.Size(148, 22);
            this.StopSite.Text = "停止(&E)";
            this.StopSite.Click += new System.EventHandler(this.StopSite_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(145, 6);
            // 
            // RePassword
            // 
            this.RePassword.Name = "RePassword";
            this.RePassword.Size = new System.Drawing.Size(148, 22);
            this.RePassword.Text = "重置密码(&R)";
            this.RePassword.Click += new System.EventHandler(this.RePassword_Click);
            // 
            // ReRights
            // 
            this.ReRights.Name = "ReRights";
            this.ReRights.Size = new System.Drawing.Size(148, 22);
            this.ReRights.Text = "重设权限(&T)";
            this.ReRights.Click += new System.EventHandler(this.ReRights_Click);
            // 
            // DelSite
            // 
            this.DelSite.Name = "DelSite";
            this.DelSite.Size = new System.Drawing.Size(148, 22);
            this.DelSite.Text = "删除网站(&D)";
            this.DelSite.Click += new System.EventHandler(this.DelSite_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(145, 6);
            // 
            // OpenExplorer
            // 
            this.OpenExplorer.Name = "OpenExplorer";
            this.OpenExplorer.Size = new System.Drawing.Size(148, 22);
            this.OpenExplorer.Text = "资源管理器(&V)";
            this.OpenExplorer.Click += new System.EventHandler(this.OpenExplorer_Click);
            // 
            // OpenSite
            // 
            this.OpenSite.Name = "OpenSite";
            this.OpenSite.Size = new System.Drawing.Size(148, 22);
            this.OpenSite.Text = "访问网站(&G)";
            this.OpenSite.Click += new System.EventHandler(this.OpenSite_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(145, 6);
            // 
            // doExit
            // 
            this.doExit.Name = "doExit";
            this.doExit.Size = new System.Drawing.Size(148, 22);
            this.doExit.Text = "退出(&C)";
            this.doExit.Click += new System.EventHandler(this.doExit_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.progressBar1);
            this.groupBox2.Controls.Add(this.AddSite);
            this.groupBox2.Controls.Add(this.IPs);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.Address);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.browserLog);
            this.groupBox2.Controls.Add(this.LogFilePath);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.browser);
            this.groupBox2.Controls.Add(this.sitePath);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.webName);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 303);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(596, 184);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "建立新网站";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(250, 155);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(254, 18);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 13;
            // 
            // AddSite
            // 
            this.AddSite.Location = new System.Drawing.Point(510, 155);
            this.AddSite.Name = "AddSite";
            this.AddSite.Size = new System.Drawing.Size(65, 23);
            this.AddSite.TabIndex = 12;
            this.AddSite.Text = "添加";
            this.AddSite.UseVisualStyleBackColor = true;
            this.AddSite.Click += new System.EventHandler(this.AddSite_Click);
            // 
            // IPs
            // 
            this.IPs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.IPs.FormattingEnabled = true;
            this.IPs.Location = new System.Drawing.Point(301, 99);
            this.IPs.Name = "IPs";
            this.IPs.Size = new System.Drawing.Size(203, 20);
            this.IPs.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(248, 102);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "接入IP:";
            // 
            // Address
            // 
            this.Address.Location = new System.Drawing.Point(21, 102);
            this.Address.MaxLength = 500;
            this.Address.Multiline = true;
            this.Address.Name = "Address";
            this.Address.Size = new System.Drawing.Size(181, 72);
            this.Address.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(143, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "网站域名:(每行一个域名)";
            // 
            // browserLog
            // 
            this.browserLog.Location = new System.Drawing.Point(510, 58);
            this.browserLog.Name = "browserLog";
            this.browserLog.Size = new System.Drawing.Size(65, 23);
            this.browserLog.TabIndex = 7;
            this.browserLog.Text = "浏览...";
            this.browserLog.UseVisualStyleBackColor = true;
            this.browserLog.Click += new System.EventHandler(this.browserLog_Click);
            // 
            // LogFilePath
            // 
            this.LogFilePath.Location = new System.Drawing.Point(301, 60);
            this.LogFilePath.MaxLength = 255;
            this.LogFilePath.Name = "LogFilePath";
            this.LogFilePath.ReadOnly = true;
            this.LogFilePath.Size = new System.Drawing.Size(203, 21);
            this.LogFilePath.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(242, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "IIS日志:";
            // 
            // browser
            // 
            this.browser.Location = new System.Drawing.Point(510, 23);
            this.browser.Name = "browser";
            this.browser.Size = new System.Drawing.Size(65, 23);
            this.browser.TabIndex = 4;
            this.browser.Text = "浏览...";
            this.browser.UseVisualStyleBackColor = true;
            this.browser.Click += new System.EventHandler(this.browser_Click);
            // 
            // sitePath
            // 
            this.sitePath.Location = new System.Drawing.Point(301, 24);
            this.sitePath.MaxLength = 255;
            this.sitePath.Name = "sitePath";
            this.sitePath.ReadOnly = true;
            this.sitePath.Size = new System.Drawing.Size(203, 21);
            this.sitePath.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(236, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "本地路径:";
            // 
            // webName
            // 
            this.webName.Location = new System.Drawing.Point(21, 43);
            this.webName.MaxLength = 50;
            this.webName.Name = "webName";
            this.webName.Size = new System.Drawing.Size(181, 21);
            this.webName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "网站名称:(英文字母)";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "网站名称";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "首选域名";
            this.columnHeader3.Width = 191;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "所在目录";
            this.columnHeader4.Width = 214;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "运行状态";
            this.columnHeader5.Width = 69;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 501);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IIS网站管理精灵";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox webName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button browser;
        private System.Windows.Forms.TextBox sitePath;
        private System.Windows.Forms.Button browserLog;
        private System.Windows.Forms.TextBox LogFilePath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Address;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox IPs;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button AddSite;
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
        private System.Windows.Forms.ToolStripMenuItem StartSite;
        private System.Windows.Forms.ToolStripMenuItem SuspendSite;
        private System.Windows.Forms.ToolStripMenuItem StopSite;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem RePassword;
        private System.Windows.Forms.ToolStripMenuItem ReRights;
        private System.Windows.Forms.ToolStripMenuItem DelSite;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem OpenExplorer;
        private System.Windows.Forms.ToolStripMenuItem OpenSite;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem doExit;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ListView siteView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;


    }
}

