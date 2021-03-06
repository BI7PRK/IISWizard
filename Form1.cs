﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Security.AccessControl;

namespace IISWizard
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private class SiteParms
        {
            public string siteName = "";
            public string siteUrls = "";
            public string webFilePath = "";
            public string logFilePath = "";
            public string siteIP = "";
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Invoke(new ShowAppState(SetAppState), false);
            this.GetServIP();
            Thread thread = new Thread(SiteList);
            thread.Start();
        }

        private void GetServIP()
        {
            this.IPs.Items.Clear();
            IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            this.IPs.Items.Add("(全部未分配IP)");
            foreach (var item in addressList.Where(w=>w.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
            {
                this.IPs.Items.Add(item.ToString());
            }
            this.IPs.SelectedIndex = 0;
        }

        private void SiteList(object state)
        {
            List<string[]> list = new List<string[]>();
            Operator iisAdminer = new Operator();
            List<DirectoryEntry> dir = iisAdminer.GetWebSiteList;
            foreach (DirectoryEntry entry in dir)
            {
                string siteName = entry.Properties["ServerComment"].Value.ToString();
                string[] domain = entry.Properties["ServerBindings"][0].ToString().Split(new char[] { ':' });

                string str = Convert.ToInt32(entry.Name).ToString();
                DirectoryEntry ey = new DirectoryEntry("IIS://localhost/w3svc/" + str + "/root");

                object path = ey.Properties["Path"][0];
                path = (path == null) ? "" : path;
                string itemstate = iisAdminer.GetState(str);
                list.Add(new string[] { entry.Name, siteName, domain[2], path.ToString(), itemstate });
                ey.Close();
            }
            iisAdminer = null;
            siteView.Invoke(new AsynDataGrid(DataGridList), list);
            Invoke(new ShowAppState(SetAppState), true);
        }

        private delegate void AsynDataGrid(List<string[]> dt);
        private void DataGridList(List<string[]> dt)
        {
            siteView.Items.Clear();
            foreach (string[] item in dt)
            {
                ListViewItem LVI = new ListViewItem(item[0]);
                LVI.SubItems.Add(item[1]);
                LVI.SubItems.Add(item[2]);
                LVI.SubItems.Add(item[3]);
                LVI.SubItems.Add(item[4]);
                siteView.Items.Add(LVI);
            }
        }

        private delegate void ShowAppState(bool v);
        private void SetAppState(bool v)
        {
            progressBar1.Visible = (v != true);
            AddSite.Enabled = v;
        }


        private delegate void AlertDelegate(string[] s);
        private void CallMessageBox(string[] s)
        {
            MessageBox.Show(this, s[0], s[1], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void AddSite_Click(object sender, EventArgs e)
        {
            SiteParms parms = new SiteParms();
            parms.siteIP = IPs.SelectedItem.ToString();
            parms.siteName = webName.Text.Trim();
            parms.siteUrls = Address.Text.Trim();
            parms.webFilePath = sitePath.Text;
            parms.logFilePath = LogFilePath.Text;

            Thread thread = new Thread(new ParameterizedThreadStart(this.AddWebSite));
            thread.IsBackground = true;
            thread.Start(parms);
        }

        private void AddWebSite(object state)
        {
            SiteParms parms =(SiteParms)state;
            string ip = parms.siteIP;
            string doms = parms.siteUrls;
            string strName = parms.siteName;
            string sitepath = parms.webFilePath;
            if (sitepath == "" || doms == "" || sitepath == "")
            {
                string[] s = new string[] { "必要项没填写完整！\r\n1、网站名称。\r\n2、网站域名。\r\n3、本地路径。", "建立网站" };
                Invoke(new AlertDelegate(CallMessageBox), new object[] { s });
                return;
            }
            Invoke(new ShowAppState(SetAppState), false);
            Operator iisAdmin = new Operator();
            List<string> dom = new List<string>();
            string[] arr = doms.Split(new char[] { '\r' });
            foreach (string val in arr)
            {
                if (val != "")
                {
                    dom.Add(val.Replace("\n", ""));
                }
            }
            iisAdmin.WebName = strName;
            iisAdmin.WebPath = sitepath;
            iisAdmin.LogFile = parms.logFilePath;
            iisAdmin.Domain = dom;
            iisAdmin.HostIP = (ip.CompareTo("(全部未分配IP)") == 0) ? "" : ip;
            bool addWeb = iisAdmin.CreateNewWebSite();
            if (!addWeb)
            {
                string[] s = new string[] { "添加虚拟目录时发生错误，网站没有建立成功。", "建立网站" };
                Invoke(new AlertDelegate(CallMessageBox), new object[] { s });
                return;
            }
            string pwd = iisAdmin.WebPassword;
            string uid = iisAdmin.WebUserName;
            bool addUser = iisAdmin.NewSysUser(uid, pwd);
            if (!addUser)
            {
                //MessageBox.Show(this, "添加IIS用户时发生错误，没有顺利完成。", "添加用户", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //return;
            }

            FileSystemRights rights = FileSystemRights.Read
               | FileSystemRights.Write
               | FileSystemRights.ReadAndExecute
               | FileSystemRights.ListDirectory
               | FileSystemRights.Modify;

            bool addRights = NTFSControl.AddDirectorySecurity(sitepath, uid, rights);
            if (!addRights)
            {
                //MessageBox.Show(this, "添加文件夹权限时发生错误，没有顺利完成。", "附加权限", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //return;
            }

            //LogFilePath.Text = sitePath.Text = webName.Text = Address.Text = "";
            iisAdmin = null;
            this.RefreshGridView();
        }



        private void browser_Click(object sender, EventArgs e)
        {
            folderBrowser.SelectedPath = "";
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                sitePath.Text = folderBrowser.SelectedPath;
            }
        }

        private void browserLog_Click(object sender, EventArgs e)
        {
            folderBrowser.SelectedPath = "";
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                LogFilePath.Text = folderBrowser.SelectedPath;
            }
        }
        private void CheckWebState(object sender, CancelEventArgs e)
        {
            string siteID = this.GetSiteViewID;
            if (siteID == "")
            {
                e.Cancel = true;
            }
            Operator iisAdmin = new Operator();
            try
            {
                DirectoryEntry siteEntry = iisAdmin.GetDirectoryBySite(siteID);
                string state = siteEntry.Properties["ServerState"][0].ToString();
                StartSite.Enabled = (!state.Equals("2"));
                SuspendSite.Enabled = false;// (!state.Equals("6"));
                StopSite.Enabled = (!state.Equals("4"));
            }
            catch (Exception ex)
            {
                //
            }
            iisAdmin = null;
        }


        private void StartSite_Click(object sender, EventArgs e)
        {
            string siteNum = this.GetSiteViewID;
            Operator iisAdmin = new Operator();
            iisAdmin.StartWebSite(siteNum);
            iisAdmin = null;
            this.RefreshGridView();
        }

        private void SuspendSite_Click(object sender, EventArgs e)
        {
            string siteNum = this.GetSiteViewID;
            Operator iisAdmin = new Operator();
            iisAdmin.SuspendWebSite(siteNum);
            iisAdmin = null;
            this.RefreshGridView();
        }

        private void StopSite_Click(object sender, EventArgs e)
        {
            string siteNum = this.GetSiteViewID;
            Operator iisAdmin = new Operator();
            iisAdmin.StopWebSite(siteNum);
            iisAdmin = null;
            this.RefreshGridView();
        }

        private void RefreshGridView()
        {
            this.SiteList(null);
            Invoke(new ShowAppState(SetAppState), true);
        }

        private void DelSite_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "确定要删除吗？", "删除网站", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                Invoke(new ShowAppState(SetAppState), false);
                string siteNum = this.GetSiteViewID;
                Thread thread = new Thread(new ParameterizedThreadStart(this.DeleteSite));
                thread.IsBackground = true;
                thread.Start(siteNum);
            }
        }
        private void DeleteSite(object state)
        {
            string siteNum = state.ToString();
            Operator iisAdmin = new Operator();
            iisAdmin.DeleteWebSiteByName(siteNum);
            iisAdmin = null;
            this.RefreshGridView();
        }

        private void RePassword_Click(object sender, EventArgs e)
        {
            string siteNum = this.GetSiteViewID;
            Operator iisAdmin = new Operator();
            DirectoryEntry chdenEntry = iisAdmin.GetDirectoryBySite(siteNum, true);
            string UserName = chdenEntry.Properties["AnonymousUserName"][0].ToString();
            string Password = iisAdmin.PasswordString;

            chdenEntry.Properties["AnonymousUserPass"][0] = Password;
            chdenEntry.CommitChanges();

            bool outValue;
            if (!iisAdmin.CheckSysUser(UserName))
            {
                outValue = iisAdmin.NewSysUser(UserName, Password);
            }
            else
            {
                outValue = iisAdmin.ChangePawword(UserName, Password);
            }
            string msg = "密码重设成功。";
            if (!outValue)
            {
                msg = "密码重设失败。";
            }

            MessageBox.Show(this, msg, "重设密码", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void ReRights_Click(object sender, EventArgs e)
        {
            Invoke(new ShowAppState(SetAppState), false);
            string siteNum = this.GetSiteViewID;
            string WebPath = this.GetSiteFilePath;
            string[] parms = new string[] { siteNum, WebPath };
            Thread thread = new Thread(new ParameterizedThreadStart(this.ReSetRights));
            thread.IsBackground = true;
            thread.Start(parms);
        }

        private void ReSetRights(object state)
        {
            string[] parms = (string[])state;
            string siteNum = parms[0];
            string WebPath = parms[1];
            Operator iisAdmin = new Operator();
            DirectoryEntry chdenEntry = iisAdmin.GetDirectoryBySite(siteNum, true);
            string UserName = chdenEntry.Properties["AnonymousUserName"][0].ToString();

            FileSystemRights rights = FileSystemRights.Read
                | FileSystemRights.Write
                | FileSystemRights.ReadAndExecute
                | FileSystemRights.ListDirectory
                | FileSystemRights.Modify;



            bool addRights = NTFSControl.AddDirectorySecurity(WebPath, UserName, rights);
            string msg = "权限重设成功。";
            if (!addRights)
            {
                msg = "权限重设失败。";
            }
            Invoke(new ShowAppState(SetAppState), true);
            Invoke(new AlertDelegate(CallMessageBox), new object[] { new string[] { msg, "重设权限" } });
        }

        private void OpenExplorer_Click(object sender, EventArgs e)
        {
            string WebPath = this.GetSiteFilePath;
            Process.Start(WebPath);
        }

        private void OpenSite_Click(object sender, EventArgs e)
        {
            string WebUrl = this.GetSiteViewUrl;
            Process.Start("http://" + WebUrl);
        }

        private void doExit_Click(object sender, EventArgs e)
        {
            //this.Close();
            Application.Exit();
        }

        private string GetSiteViewID
        {
            get 
            {
                if (siteView.SelectedItems.Count > 0)
                {
                    return siteView.SelectedItems[0].SubItems[0].Text;
                }
                else
                {
                    return "";
                }
            }
        }

        private string GetSiteViewUrl
        {
            get
            {
                if (siteView.SelectedItems.Count > 0)
                {
                    return siteView.SelectedItems[0].SubItems[2].Text;
                }
                else
                {
                    return "";
                }
            }
        }

        private string GetSiteFilePath
        {
            get
            {
                if (siteView.SelectedItems.Count > 0)
                {
                    return siteView.SelectedItems[0].SubItems[3].Text;
                }
                else
                {
                    return "";
                }
            }
        }

    }
}
