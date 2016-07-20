using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace chenz
{
    class FileSyncHelper
    {
        /// <summary>创建SyncFile类</summary>
        /// <param name="fileSetName">文件集名称</param>
        /// <param name="listFileInfo">文件链接列表</param>
        /// <param name="syncFile">[out]SyncFile类</param>
        /// <param name="id">文件集序号（默认值为-1）</param>
        /// <returns>操作是否成功</returns>
        public static bool CreateSyncFile(string fileSetName, List<FileInfoLite> listFileInfo, out SyncFile syncFile, int id = -1)
        {
            syncFile = null;
            if (string.IsNullOrWhiteSpace(fileSetName) || listFileInfo == null || listFileInfo.Count < 2)
                return false;

            List<LinkedFile> list = new List<LinkedFile>(listFileInfo.Count);
            list.AddRange(
                listFileInfo.Select(
                    fileInfoLite =>
                        new LinkedFile(-1, id, fileInfoLite.FileName, false, fileInfoLite.FilePath,
                            fileInfoLite.LastWriteTime)));
            var newestDate = list.Max(f => f.LastSyncDate);
            string newestPath = null;
            string newestHash = null;
            foreach (LinkedFile file in list)
            {
                if (file.LastSyncDate == newestDate)
                {
                    file.IsLastNewest = 1;
                    newestPath = file.FullName;
                    newestHash = new FileInfo(newestPath).GetHashCode().ToString();
                    break;
                }
            }
            syncFile = new SyncFile(id, fileSetName, newestHash, newestPath, DateTime.Now, list, 0);
            return true;
        }

        /// <summary>刷新文件集信息</summary>
        /// <param name="syncFile">文件集</param>
        /// <param name="latestPath">最新文件路径</param>
        /// <param name="updateTime">最近更新时间</param>
        /// <param name="newUpdateTimes">新增更新次数（默认值为0）</param>
        /// <returns>操作是否成功</returns>
        public static bool FreshSyncFile(ref SyncFile syncFile, string latestPath, DateTime updateTime, int newUpdateTimes = 0)
        {
            if (syncFile == null || syncFile.ListLinkedFile == null || syncFile.ListLinkedFile.Count < 2)
                return false;
            if (!File.Exists(latestPath)) return false;

            FileInfo fi = new FileInfo(latestPath);
            syncFile.LastFileHash = fi.GetHashCode().ToString();
            syncFile.LastUpdatePath = latestPath;
            syncFile.LastUpdateDate = updateTime;
            syncFile.UpdateTimes += newUpdateTimes;

            int count = syncFile.ListLinkedFile.Count;
            for (int i = 0; i < count; i++)
            {
                LinkedFile file = syncFile.ListLinkedFile[i];
                file.IsLastNewest = file.FilePath.Equals(latestPath) ? 1 : 0;
                file.LastSyncDate = updateTime;
            }
            return true;
        }

        /// <summary>更新文件列表至最新文件</summary>
        /// <param name="filePaths">文件列表</param>
        /// <returns>操作是否成功</returns>
        public static bool UpdateFilesToNewest(IEnumerable<string> filePaths, out string latestPath, out DateTime updateTime)
        {
            latestPath = null;
            updateTime = DateTime.Now;
            if (filePaths == null || filePaths.Count() < 2) return false;

            try
            {
                List<FileInfo> list = GetFileInfoList(filePaths);
                var dt = list.Max(fi => fi.LastWriteTime);
                latestPath = list.First(fi => fi.LastWriteTime == dt).FullName;

                var ienu = filePaths.GetEnumerator();
                while (ienu.MoveNext())
                {
                    if (!CompareHelper.IsFileSame(latestPath, ienu.Current))
                    {
                        File.Copy(latestPath, ienu.Current, true);
                    }
                }
                updateTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog("UpdateFilesToNewest", ex);
                return false;
            }
            return true;
        }

        /// <summary>根据文件路径字符串列表得到其FileInfo列表</summary>
        /// <param name="filePaths">文件路径字符串列表</param>
        /// <returns>FileInfo列表</returns>
        private static List<FileInfo> GetFileInfoList(IEnumerable<string> filePaths)
        {
            List<FileInfo> list = new List<FileInfo>(filePaths.Count());
            var ienu = filePaths.GetEnumerator();
            while (ienu.MoveNext())
            {
                if (File.Exists(ienu.Current))
                {
                    list.Add(new FileInfo(ienu.Current));
                }
            }
            return list;
        }
    }
}
