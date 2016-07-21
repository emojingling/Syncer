using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace chenz
{
    static class DriveHelper
    {
        /// <summary>得到文件目录对应列表</summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool GetNamePathList(out List<NamePath> list)
        {
            List<DriveInfo> listVolume = GetNTFSDrives();

            list = new List<NamePath>();
            try
            {
                foreach (DriveInfo di in listVolume)
                {
                    var usnJournal = new NtfsUsnJournal(di);
                    List<WINAPI.UsnEntry> listFile; //本卷的全部Usn信息表
                    var rtnCode = usnJournal.GetNtfsVolumeFiles(out listFile);
                    var dicPath = usnJournal.GetFilePaths(listFile);

                    list.AddRange(from entry in listFile
                        where dicPath.ContainsKey(entry.FileReferenceNumber)
                        select new NamePath(entry.Name, dicPath[entry.FileReferenceNumber]));
                }
            }
            catch (Exception ex)
            {
                string strErr = ex.Message.Contains("Access is denied")
                    ? @"请使用管理员权限重新打开本程序！"
                    : @"未能成功打开磁盘！";
                LogHelper.WriteErrLog(strErr, ex);
                return false;
            }
            return true;
        }

        /// <summary>得到全部的NTFS卷列表</summary>
        /// <returns></returns>
        private static List<DriveInfo> GetNTFSDrives()
        {
            DriveInfo[] volumes = DriveInfo.GetDrives();
            string strNTFS = @"NTFS";
            List<DriveInfo> list = volumes.Where(di => di.IsReady && di.DriveFormat.Equals(strNTFS)).ToList();
            list.TrimExcess();
            return list;
        }
    }
}
