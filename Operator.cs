using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace IISWizard
{
    

    public class Operator
    {
        /// <summary>
        /// 网站状态参数定义
        /// </summary>
        private enum SiteState
        {
            Runing = 2,
            Stoped = 4,
            Suspend = 6
        }

        #region 私有属性
        private string HOSTIP = "";
        private List<string> DOMAIN = new List<string>();
        private int PORT = 80;
        private string WEBNAME = "";
        private string WEBUSERNAME = "";
        private string WEBPASSWORD = "";
        private string WEBPATH = "";
        private string LOGPATH = "";
        private string DEFAULTDOC = "index.htm,index.html,index.asp,index.aspx,Default.htm,Default.html,Default.asp,Default.aspx";


        #endregion

        #region 公共属性

        public void WebSiteInfo(string HOSTIP, List<string> DOMAIN, int PORT, string WEBNAME, string WEBPATH, string DEFAULTDOC)
        {
            this.HOSTIP = HOSTIP;
            this.DOMAIN = DOMAIN;
            this.PORT = PORT;
            this.WEBNAME = WEBNAME;
            this.WEBPATH = WEBPATH;
            this.DEFAULTDOC = DEFAULTDOC;

        }

        /// <summary>
        /// 网站IP
        /// </summary>
        public string HostIP
        {
            get { return this.HOSTIP; }
            set { this.HOSTIP = value; }
        }
        /// <summary>
        /// 网站域名
        /// </summary>
        public List<string> Domain
        {
            get { return this.DOMAIN; }
            set { this.DOMAIN = value; }
        }
        /// <summary>
        /// 网站端口
        /// </summary>
        public int Port
        {
            get { return this.PORT; }
            set { this.PORT = value; }
        }
        /// <summary>
        /// 网站名称
        /// </summary>
        public string WebName
        {
            get { return this.WEBNAME; }
            set { this.WEBNAME = value; }
        }
        /// <summary>
        /// IIS登陆用户（只读）
        /// </summary>
        public string WebUserName
        {
            get { return this.WEBUSERNAME; }
        }
        /// <summary>
        /// IIS用户密码（只读）
        /// </summary>
        public string WebPassword
        {
            get { return this.WEBPASSWORD; }
        }
        /// <summary>
        /// 网站路径
        /// </summary>
        public string WebPath
        {
            get { return this.WEBPATH; }
            set { this.WEBPATH = value; }
        }
        /// <summary>
        /// 日志文件目录
        /// </summary>
        public string LogFile
        {
            get { return this.LOGPATH; }
            set { this.LOGPATH = value; }
        }
        /// <summary>
        /// 默认文档
        /// </summary>
        public string DefaultDoc
        {
            get { return this.DEFAULTDOC; }
            set { this.DEFAULTDOC = value; }
        } 
        #endregion

        #region 远程服务器设置

        private string HOSTNAME = "localhost";
        private string REMOTEUSERNAME = "";
        private string REMOTEPASSWORD = "";

        public void RemoteConfig(string hostName, string userName, string password)
        {
            this.HOSTNAME = hostName;
            this.REMOTEUSERNAME = userName;
            this.REMOTEPASSWORD = password;
        }

        /// <summary>
        /// 远程服务器
        /// </summary>
        public string HostName
        {
            get { return this.HOSTNAME; }
            set { this.HOSTNAME = value; }
        }
        /// <summary>
        /// 远程服务器用户
        /// </summary>
        public string RemoteUserName
        {
            get { return this.REMOTEUSERNAME; }
            set { this.REMOTEUSERNAME = value; }
        }
        /// <summary>
        /// 远程服务器密码
        /// </summary>
        public string RemotePassword
        {
            get { return this.REMOTEPASSWORD; }
            set { this.REMOTEPASSWORD = value; }
        }


        #endregion

        /// <summary>
        /// 根据是否有用户名来判断是否是远程服务器。
        /// 然后再构造出不同的DirectoryEntry出来
        /// </summary>
        /// <param name="entPath">IIS目录</param>
        /// <returns></returns>
        private DirectoryEntry GetDirectoryEntry(string entPath)
        {
            DirectoryEntry Entry;
            if (this.REMOTEUSERNAME == null)
            {
                Entry = new DirectoryEntry(entPath);
            }
            else
            {
                Entry = new DirectoryEntry(entPath, this.REMOTEUSERNAME, this.REMOTEPASSWORD, AuthenticationTypes.Secure);
            }
            return Entry;
        }

        /// <summary>
        /// 获取网站的 DirectoryEntry
        /// </summary>
        /// <param name="siteNum">网站编号</param>
        /// <returns></returns>
        public DirectoryEntry GetDirectoryBySite(string siteNum)
        {
            return GetDirectoryBySite(siteNum, false);
        }

        /// <summary>
        /// 获取虚拟目录的 DirectoryEntry
        /// </summary>
        /// <param name="siteNum">网站编号</param>
        /// <param name="isRoot">是否为子目录</param>
        /// <returns></returns>
        public DirectoryEntry GetDirectoryBySite(string siteNum, bool isRoot)
        {
            string siteEntPath;
            if (isRoot)
            {
                siteEntPath = string.Format("IIS://{0}/w3svc/{1}/root", this.HOSTNAME, siteNum);
            }
            else
            {
                siteEntPath = string.Format("IIS://{0}/w3svc/{1}", this.HOSTNAME, siteNum);
            }
            return this.GetDirectoryEntry(siteEntPath);
        }

        /// <summary>
        /// 生成一个随机字符串
        /// </summary>
        public string PasswordString
        {
            get
            {

                string Setchar = "0123456789";
                Setchar += "~!@#$%^&*()_+|";
                Setchar += ",./;'[]<>?:\"\\{}";
                Setchar += "abcdefghijklmnopqrstuvwxyz";
                Setchar += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                Random rd = new Random();

                string outValue = "";
                while (outValue.Length < 21)
                {
                    Thread.Sleep(1);
                    int star = rd.Next(90);
                    outValue += Setchar.Substring(star, 1);
                }
                return outValue;
            }
        }
        /// <summary>
        /// 获取所有网站列表
        /// </summary>
        public List<DirectoryEntry> GetWebSiteList
        {
            get
            {
                List<DirectoryEntry> list = new List<DirectoryEntry>();
                string EntryPath = String.Format("IIS://{0}/w3svc", this.HOSTNAME);
                DirectoryEntry dir = this.GetDirectoryEntry(EntryPath);
                try
                {
                    foreach (DirectoryEntry child in dir.Children)
                    {
                        if (child.SchemaClassName == "IIsWebServer")
                        {
                            list.Add(child);
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(null, ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
              
                return list;
            }
        }

        #region 网站用户操作

        /// <summary>
        /// 添加一个用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public bool NewSysUser(string userName, string password)
        {
            string EntryPath = String.Format("WinNT://{0}", this.HOSTNAME);
            DirectoryEntry obDirEntry = this.GetDirectoryEntry(EntryPath);
            DirectoryEntries users = obDirEntry.Children;
            try
            {
                DirectoryEntry user = users.Add(userName, "user");
                user.Invoke("Put", new string[] { "Description", "IIS网站独立用户" });
                user.Invoke("Put", "UserFlags", 66049); //密码永不过期
                //user.Invoke("Put", "PasswordExpired", -1); //密码永不过期
                user.CommitChanges();
                user.Invoke("SetPassword", password);

                DirectoryEntry grp = users.Find("Users", "group");
                if (grp.Name != "")
                {
                    //grp.Invoke("Add", user.Path);//将用户添加到某组
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 查找用户是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool CheckSysUser(string userName)
        {
            try
            {
                string EntryPath = String.Format("WinNT://{0}", this.HOSTNAME);
                DirectoryEntry obDirEntry = this.GetDirectoryEntry(EntryPath);
                DirectoryEntry obUser = obDirEntry.Children.Find(userName, "User");
                obDirEntry.Close();
                if (obUser.Name != "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Userpassword"></param>
        /// <returns></returns>
        public bool ChangePawword(string Username, string Userpassword)
        {
            try
            {
                string EntryPath = String.Format("WinNT://{0}", this.HOSTNAME);
                DirectoryEntry obDirEntry = this.GetDirectoryEntry(EntryPath);
                DirectoryEntry obUser = obDirEntry.Children.Find(Username, "User");
                obUser.Invoke("SetPassword", Userpassword);
                obUser.CommitChanges();
                obUser.Close();
                obDirEntry.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public bool DelSysUser(string Username)
        {
            try
            {
                string EntryPath = String.Format("WinNT://{0}", this.HOSTNAME);
                DirectoryEntry obDirEntry = this.GetDirectoryEntry(EntryPath);
                DirectoryEntry obUser = obDirEntry.Children.Find(Username, "User");//找得用户
                obDirEntry.Children.Remove(obUser);//删除用户
                obDirEntry.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        #endregion

        /// <summary>
        /// 创建一个新的网站
        /// </summary>
        public bool CreateNewWebSite() 
        {
            string str = "";
            List<string> bindings = new List<string>();
            foreach (string domain in this.DOMAIN)
            {
                string[] tmp = domain.Split(new char[] { ':' });
                switch (tmp.Length)
                {
                    case 1:
                        str = string.Format("{0}:{1}:{2}", new object[] { this.HOSTIP, this.PORT, domain });
                        break;
                    case 2:
                        str = string.Format("{0}:{1}:{2}", new object[] { this.HOSTIP, tmp[1], tmp[0] });
                        break;
                    default:
                        continue;
                }

                if (this.CheckEnavaible(str, this.WEBNAME))
                {
                    bindings.Add(str);
                }
            }
            if (bindings.Count == 0)
                return false;

            string EntryPath = String.Format("IIS://{0}/w3svc", this.HOSTNAME);
            DirectoryEntry RootEntry = this.GetDirectoryEntry(EntryPath);
            string NewSiteID = this.GetNewWebSiteID;
            try
            {
                DirectoryEntry newSiteEntry = RootEntry.Children.Add(NewSiteID, "IIsWebServer");
                newSiteEntry.CommitChanges();
                newSiteEntry.Properties["ServerComment"].Value = this.WEBNAME;
                foreach (string bind in bindings)
                {
                    newSiteEntry.Properties["ServerBindings"].Add(bind);
                }
                newSiteEntry.Properties["ServerAutoStart"].Value = true;
                string webpath = this.WEBPATH + "\\Web";

                if (!Directory.Exists(webpath))
                    Directory.CreateDirectory(webpath);

                string logFile = (string.IsNullOrEmpty(this.LOGPATH)) ? this.WEBPATH + "\\LogFile" : this.LOGPATH;
                if (!Directory.Exists(logFile))
                    Directory.CreateDirectory(logFile);

                newSiteEntry.Properties["LogFileDirectory"].Value = logFile;
                newSiteEntry.Properties["LogFileLocaltimeRollover"].Value = true;
                //newSiteEntry.Properties["LogFilePeriod"][0] = 4;
               // newSiteEntry.Properties["LogFileTruncateSize"][0] = 10;
                newSiteEntry.CommitChanges();

                DirectoryEntry vdEntry = newSiteEntry.Children.Add("root", "IIsWebVirtualDir");
                vdEntry.CommitChanges();

                this.SetScriptMaps(vdEntry);

                vdEntry.Properties["AuthAnonymous"][0] = true; // 允许匿名访问
                vdEntry.Properties["AnonymousUserName"][0] = this.WEBUSERNAME = (string.IsNullOrEmpty(this.WEBUSERNAME)) ? "IUSR_" + this.WEBNAME : this.WEBUSERNAME;
                vdEntry.Properties["AnonymousUserPass"][0] = this.WEBPASSWORD = (string.IsNullOrEmpty(this.WEBPASSWORD)) ? this.PasswordString : this.WEBPASSWORD;

                vdEntry.Properties["AccessExecute"][0] = false;
                vdEntry.Properties["AccessScript"][0] = true;
                vdEntry.Properties["EnableDefaultDoc"][0] = true;
                vdEntry.Properties["DefaultDoc"][0] = DEFAULTDOC;
                //vdEntry.Properties["ContentIndexed"][0] = true;
                vdEntry.Properties["AccessRead"][0] = true;
                vdEntry.Properties["AccessWrite"][0] = true;
                //vdEntry.Properties["AspEnableParentPaths"].Value = true;
                vdEntry.Properties["AccessSource"][0] = false;
                vdEntry.Properties["EnableDirBrowsing"][0] = false;
                //vdEntry.Properties["AuthChangeDisable"][0] = true;
                vdEntry.Properties["Path"].Value = webpath;
                vdEntry.Properties["AppFriendlyName"][0] = "默认应用程序";
                vdEntry.Invoke("AppCreate3", new object[] { 2, this.WEBNAME + "_AppPool", true });
                //vdEntry.Properties["AppIsolated"][0] = 1;
                vdEntry.CommitChanges();
                bindings.Clear();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private void SetScriptMaps(DirectoryEntry entry)
        {
            /*
            string[] script = { 
                                  @".asp,C:\WINDOWS\system32\inetsrv\asp.dll,5,GET,HEAD,POST,TRACE",
                                  @".asa,C:\WINDOWS\system32\inetsrv\asp.dll,5,GET,HEAD,POST,TRACE",
                                  @".asax,c:\windows\microsoft.net\framework\v2.0.50727\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
                                  @".ascx,c:\windows\microsoft.net\framework\v2.0.50727\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
                                  @".ashx,c:\windows\microsoft.net\framework\v2.0.50727\aspnet_isapi.dll,1,GET,HEAD,POST,DEBUG",
                                  @".asmx,c:\windows\microsoft.net\framework\v2.0.50727\aspnet_isapi.dll,1,GET,HEAD,POST,DEBUG",
                                  @".aspx,c:\windows\microsoft.net\framework\v2.0.50727\aspnet_isapi.dll,1,GET,HEAD,POST,DEBUG",
                                  @".axd,c:\windows\microsoft.net\framework\v2.0.50727\aspnet_isapi.dll,1,GET,HEAD,POST,DEBUG",
                                  @".config,c:\windows\microsoft.net\framework\v2.0.50727\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
                                  @".cs,c:\windows\microsoft.net\framework\v2.0.50727\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
                                  @".csproj,c:\windows\microsoft.net\framework\v2.0.50727\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
                                  @".vb,c:\windows\microsoft.net\framework\v2.0.50727\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
                                  @".vbproj,c:\windows\microsoft.net\framework\v2.0.50727\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
                                  @".master,c:\windows\microsoft.net\framework\v2.0.50727\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
                                  @".resources,c:\windows\microsoft.net\framework\v2.0.50727\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
                                  @".resx,c:\windows\microsoft.net\framework\v2.0.50727\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
                                  @".skin,c:\windows\microsoft.net\framework\v2.0.50727\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG"
                              };
            entry.Properties["ScriptMaps"].Clear();
            foreach (string value in script)
            {
                entry.Properties["ScriptMaps"].Add(value);
            }*/

        }

        /// <summary>
        ///  删除一个网站。
        /// </summary>
        /// <param name="siteNum">网站ID</param>
        public bool DeleteWebSiteByName(string siteNum)
        {
            try
            {
                DirectoryEntry siteEntry = this.GetDirectoryBySite(siteNum);
                string appName = siteEntry.Properties["ServerComment"].Value.ToString() + "_AppPool";
                this.DeleteAppPool(appName);

                DirectoryEntry childEntry = this.GetDirectoryBySite(siteNum, true);
                string userName = childEntry.Properties["AnonymousUserName"][0].ToString();
                this.DelSysUser(userName);
                var dir = new DirectoryInfo(childEntry.Properties["Path"].Value.ToString());

                FileSystemRights rights = FileSystemRights.Read
               | FileSystemRights.Write
               | FileSystemRights.ReadAndExecute
               | FileSystemRights.ListDirectory
               | FileSystemRights.Modify;

                NTFSControl.RemoveDirectoryAccountSecurity(dir.Parent.FullName, userName, rights);
                childEntry.Close();

                string rootPath = String.Format("IIS://{0}/w3svc", this.HOSTNAME);
                DirectoryEntry rootEntry = GetDirectoryEntry(rootPath);
                rootEntry.Children.Remove(siteEntry);
                rootEntry.CommitChanges();
                rootEntry.Close();
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "删除失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 删除应用程序池
        /// </summary>
        /// <param name="poolName"></param>
        /// <returns></returns>
        public bool DeleteAppPool(string poolName)
        {
            string poolPath = string.Format("IIS://{0}/w3svc/AppPools", this.HOSTNAME);
            DirectoryEntry PoolEntry = this.GetDirectoryEntry(poolPath);
            foreach (DirectoryEntry app in PoolEntry.Children)
            {
                if (app.Name == poolName)
                {
                    app.DeleteTree();
                    return true;
                }     

            }
            return false;
        }

        
        /// <summary>
        /// 获取一个网站的运行状态
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public string GetState(string str)
        {
            DirectoryEntry siteEntry = this.GetDirectoryBySite(str);
            SiteState state = (SiteState)siteEntry.Properties["ServerState"][0];
            string outValue = "未知";
            switch (state) 
            {
                case SiteState.Runing:
                    outValue = "运行";
                    break;
                case SiteState.Stoped:
                    outValue = "停止";
                    break;
                case SiteState.Suspend:
                    outValue = "暂停";
                    break;
                default:
                    break;
            }
            return outValue;
        }

        /// <summary>
        /// 启动网站
        /// </summary>
        /// <param name="siteNum">网站ID</param>
        public bool StartWebSite(string siteNum)
        {
            try
            {
                DirectoryEntry siteEntry = this.GetDirectoryBySite(siteNum);
                siteEntry.Invoke("Start", new object[] { });
                siteEntry.Close();
                return true;
            }
            catch 
            {
                return false;
            }

        }

        /// <summary>
        /// 暂停网站
        /// </summary>
        /// <param name="siteNum">网站ID</siteNum>
        public void SuspendWebSite(string siteNum)
        {
            DirectoryEntry siteEntry = this.GetDirectoryBySite(siteNum);
            //siteEntry.Invoke("Suspend", new object[] { });
            siteEntry.Close();
        }
        /// <summary>
        /// 停止网站
        /// </summary>
        /// <param name="siteNum"></param>
        public bool StopWebSite(string siteNum)
        {
            try{
                DirectoryEntry siteEntry = this.GetDirectoryBySite(siteNum);
                siteEntry.Invoke("Stop", new object[] { });
                siteEntry.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///  获取一个网站的编号。根据网站的ServerBindings或者ServerComment来确定网站编号
        /// </summary>
        /// <param name="siteName">网站名称</param>
        /// <returns>返回网站的编号</returns>
        /// <exception cref="NotFoundWebSiteException">表示没有找到网站</exception>
        public string GetWebSiteNum(string siteName)
        {
            Regex regex = new Regex(siteName, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            string tmpStr;
            DirectoryEntry ent = this.GetDirectoryBySite(siteName);
            foreach (DirectoryEntry child in ent.Children)
            {
                if (child.SchemaClassName == "IIsWebServer")
                {
                    if (child.Properties["ServerBindings"].Value != null)
                    {
                        tmpStr = child.Properties["ServerBindings"].Value.ToString();
                        if (regex.Match(tmpStr).Success)
                        {
                            return child.Name;
                        }
                    }
                    if (child.Properties["ServerComment"].Value != null)
                    {
                        tmpStr = child.Properties["ServerComment"].Value.ToString();
                        if (regex.Match(tmpStr).Success)
                        {
                            return child.Name;
                        }
                    }
                }
            }
            //throw new NotFiniteNumberException("没有找到我们想要的站点" + siteName);
            return "-1";
        }

        /// <summary>
        /// 确定一个新的网站与现有的网站没有相同的。
        /// </summary>
        /// <param name="bindStr">域名,端口</param>
        /// <param name="siteName">网站名称</param>
        /// <returns></returns>
        private bool CheckEnavaible(string bindStr, string siteName)
        {
            string entPath = String.Format("IIS://{0}/w3svc", this.HOSTNAME);
            DirectoryEntry directoryEntry = GetDirectoryEntry(entPath);
            foreach (DirectoryEntry entry in directoryEntry.Children)
            {
                if (entry.SchemaClassName == "IIsWebServer")
                {
                    if (entry.Properties["ServerComment"].Value.ToString() == siteName)
                    {
                        return false;
                    }
                    PropertyValueCollection Procoll = entry.Properties["ServerBindings"];
                    for (int x = 0; x < Procoll.Count; x++)
                    {
                        if (Procoll[x].ToString() == bindStr)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 获取最新一个网站ID编号
        /// </summary>
        private string GetNewWebSiteID
        {
            get
            {
                ArrayList list = new ArrayList();
                string siteEntPath = string.Format("IIS://{0}/w3svc", this.HOSTNAME);
                DirectoryEntry directoryEntry = this.GetDirectoryEntry(siteEntPath);
                foreach (DirectoryEntry entry in directoryEntry.Children)
                {
                    if (entry.SchemaClassName == "IIsWebServer")
                    {
                        string str = entry.Name;
                        list.Add(int.Parse(str));
                    }
                }
                list.Sort();
                int num = 1;
                foreach (int num2 in list)
                {
                    if (num == num2)
                    {
                        num++;
                    }
                }
                return num.ToString();
            }
        }
    }
}
