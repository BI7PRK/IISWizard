using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace IISWizard
{
    public abstract class NTFSControl
    {


        /// <summary>
        /// 获取 指定目录 除Administrators和SYSTEM之外的 权限列表
        /// </summary>
        /// <param name="DirName">指定目录</param>
        /// <returns></returns>
        public static List<string> GetDirectoryAccountSecurity(string DirName)
        {
            List<string> dAccount = new List<string>();
            DirectoryInfo dInfo = new DirectoryInfo(DirName);
            if (dInfo.Exists)
            {
                DirectorySecurity sec = Directory.GetAccessControl(DirName, AccessControlSections.All);
                foreach (FileSystemAccessRule rule in sec.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount)))
                {
                    if (rule.IdentityReference.Value != @"NT AUTHORITY\SYSTEM" && rule.IdentityReference.Value != @"BUILTIN\Administrators")
                        dAccount.Add(rule.IdentityReference.Value);
                }
            }
            return dAccount;
        }

        /// <summary>
        /// 添加 指定目录 指定用户 指定的 权限
        /// </summary>
        /// <param name="FileName">指定目录</param>
        /// <param name="Account">指定用户</param>
        /// <param name="UserRights"></param>
        /// <returns></returns>
        public static bool AddDirectorySecurity(string FileName, string Account, FileSystemRights rights)
        {
           
            bool ok;
            DirectoryInfo dInfo = new DirectoryInfo(FileName);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            InheritanceFlags iFlags = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;
            FileSystemAccessRule AccessRule2 = new FileSystemAccessRule(Account, rights, iFlags, PropagationFlags.None, AccessControlType.Allow);
            dSecurity.ModifyAccessRule(AccessControlModification.Add, AccessRule2, out ok);
            dInfo.SetAccessControl(dSecurity);
            return ok;
        }

        /// <summary>
        /// 移除 指定目录 指定用户的 权限
        /// </summary>
        /// <param name="DirName">指定目录</param>
        /// <param name="Account">指定用户</param>
        /// <returns></returns>
        public static bool RemoveDirectoryAccountSecurity(string DirName, string Account, FileSystemRights rights)
        {
            bool ok = false;
            DirectoryInfo dInfo = new DirectoryInfo(DirName);
            if (dInfo.Exists)
            {
                try
                {
                    NTAccount myAccount = new NTAccount(System.Environment.MachineName, Account);
                    DirectorySecurity dSecurity = dInfo.GetAccessControl();
                    FileSystemAccessRule AccessRule = new FileSystemAccessRule(Account, rights, AccessControlType.Allow);
                    dSecurity.RemoveAccessRuleAll(AccessRule);
                    //dSecurity.ModifyAccessRule(AccessControlModification.RemoveAll, AccessRule, out ok);
                    dInfo.SetAccessControl(dSecurity);
                }
                catch
                {
                    
                }
            }

            return ok;
        }

        /// <summary>
        ///  获取 指定文件 除Administrators和SYSTEM之外的 权限列表
        /// </summary>
        /// <param name="fileName">指定文件</param>
        /// <returns></returns>
        public static List<string> GetFileAccountSecurity(string fileName)
        {
            List<string> fAccount = new List<string>();
            FileInfo fInfo = new FileInfo(fileName);
            if (fInfo.Exists)
            {
                FileSecurity fec = File.GetAccessControl(fileName, AccessControlSections.All);
                foreach (FileSystemAccessRule rule in fec.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount)))
                {
                    if (rule.IdentityReference.Value != @"NT AUTHORITY\SYSTEM" && rule.IdentityReference.Value != @"BUILTIN\Administrators")
                        fAccount.Add(rule.IdentityReference.Value);
                }
            }
            return fAccount;
        }

        /// <summary>
        /// 移除 指定文件 指定用户的 权限
        /// </summary>
        /// <param name="fileName">指定文件</param>
        /// <param name="Account"> 指定用户</param>
        public static void RemoveFileAccountSecurity(string fileName, string Account)
        {

            FileInfo fInfo = new FileInfo(fileName);
            if (fInfo.Exists)
            {
                FileSecurity fSecurity = fInfo.GetAccessControl();
                FileSystemAccessRule AccessRule = new FileSystemAccessRule(Account, FileSystemRights.FullControl, AccessControlType.Allow);
                FileSystemAccessRule AccessRule2 = new FileSystemAccessRule(Account, FileSystemRights.FullControl, AccessControlType.Deny);
                fSecurity.RemoveAccessRuleAll(AccessRule);
                fSecurity.RemoveAccessRuleAll(AccessRule2);
                fInfo.SetAccessControl(fSecurity);
            }
        }
    }
}
