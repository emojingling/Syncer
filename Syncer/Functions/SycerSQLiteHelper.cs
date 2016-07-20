using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace chenz
{
    class SycerSQLiteHelper : MySQLiteHelper
    {
        /// <summary>连接主数据库</summary>
        /// <returns>返回数据库连接类</returns>
        /// <remarks>如果路径不存在，返回null</remarks>
        public static SQLiteConnection ConnectMainDb()
        {
            string path = System.Windows.Forms.Application.StartupPath + @"\..\DataBase\MainDB.db";
            if (!System.IO.File.Exists(path)) return null;
            var conn = new SQLiteConnection(GetConnectionString(path));
            return conn;
        }

        /// <summary>查找数据表中是否已经有该ID值</summary>
        /// <param name="dt">数据表</param>
        /// <param name="id">ID值</param>
        /// <returns>是否存在</returns>
        private static bool HaveIdInDt(DataTable dt, int id)
        {
            return dt.AsEnumerable().Any(dr => Convert.ToInt32(dr["id"]) == id);
        }

        /// <summary>得到表中的最大序号</summary>
        /// <param name="dt">数据表</param>
        /// <returns></returns>
        private static int GetLastIdInDt(DataTable dt)
        {
            return dt.AsEnumerable().Max(dr => Convert.ToInt32(dr["id"]));
        }

        /// <summary>从数据库得到文件集信息</summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="id">文件集序号</param>
        /// <param name="syncFile">[out]文件集信息</param>
        /// <returns>成功读取到文件,返回true;出现错误或未读取到文件,均返回false</returns>
        public static bool GetSyncFile(SQLiteConnection conn, int id, out SyncFile syncFile)
        {
            syncFile = null;
            if (conn == null) return false;

            try
            {
                //得到文件集在数据库中的ID
                var dtSyncFile = GetDataTable("SyncFile", conn);
                int countSyncFile = dtSyncFile.Rows.Count;
                if (countSyncFile == 0) return false;
                int index = -1, id_SyncFile = 0;
                for (int i = 0; i < countSyncFile; i++)
                {
                    DataRow dr = dtSyncFile.Rows[i];
                    if (Convert.ToInt32(dr["ID"]) == id)
                    {
                        index = i;
                        id_SyncFile = Convert.ToInt32(dr["ID"]);
                        break;
                    }
                }
                if (index == -1) return false;
                //得到文件集管理的文件信息列表
                var dtLinkedFile = GetDataTable("LinkedFile", conn);
                List<LinkedFile> list =
                    (from dr in dtLinkedFile.AsEnumerable()
                        where Convert.ToInt32(dr["ID_SyncFile"]) == id_SyncFile
                        select new LinkedFile(dr)).ToList();
                if (list.Count < 2) throw new Exception("文件集管理的文件数量小于2！");
                //构建文件集信息
                DataRow drSyncFile = dtSyncFile.Rows[index];
                syncFile = new SyncFile(Convert.ToInt32(drSyncFile["ID"]),
                    drSyncFile["FileSetName"].ToString(),
                    drSyncFile["LastFileHash"].ToString(),
                    drSyncFile["LastUpdatePath"].ToString(),
                    DateTime.FromBinary(Convert.ToInt64(drSyncFile["LastUpdateDate"])),
                    list,
                    Convert.ToInt32(drSyncFile["UpdateTimes"]));
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog("UpdateSyncFile", ex);
                return false;
            }
            finally
            {
                conn.Close();
            }
            return true;
        }

        /// <summary>添加一条SyncFile数据</summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="fileSetName">文件集名称</param>
        /// <param name="listFileInfo">链接文件信息列表</param>
        /// <returns>操作是否成功</returns>
        public static bool InsertSyncFile(SQLiteConnection conn, string fileSetName, List<FileInfoLite> listFileInfo)
        {
            SyncFile syncFile;
            if (!FileSyncHelper.CreateSyncFile(fileSetName, listFileInfo, out syncFile) || syncFile == null)
                return false;

            return InsertSyncFile(conn, syncFile);
        }

        /// <summary>添加一条SyncFile数据</summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="syncFile">文件集信息</param>
        /// <returns>操作是否成功</returns>
        public static bool InsertSyncFile(SQLiteConnection conn, SyncFile syncFile)
        {
            if (conn == null || syncFile == null) return false;
            int countLinkedFile = syncFile.ListLinkedFile.Count;
            System.Diagnostics.Debug.Assert(countLinkedFile >= 2);

            try
            {
                var dtSyncFile = GetDataTable("SyncFile", conn);
                if (HaveIdInDt(dtSyncFile, syncFile.ID))
                    return UpdateSyncFile(conn, syncFile);
                //添加SyncFile表数据
                List<string> sqls = new List<string>(countLinkedFile + 1);

                int id = 1;
                if (dtSyncFile.Rows.Count == 0) SetAutoIncr(conn, "SyncFile");
                else id = GetLastIdInDt(dtSyncFile) + 1;

                string sqlSyncFile = string.Format(
                    "INSERT INTO SyncFile (ID, FileSetName, LastFileHash, LastUpdatePath, LastUpdateDate, UpdateTimes) VALUES ({0}, '{1}', '{2}', '{3}', {4}, {5})",
                    id, syncFile.FileSetName, syncFile.LastFileHash, syncFile.LastUpdatePath, syncFile.LastUpdateDate.ToBinary(),
                    syncFile.UpdateTimes);
                sqls.Add(sqlSyncFile);
                //添加LinkedFile表数据
                for (int i = 0; i < countLinkedFile; i++)
                {
                    LinkedFile linkedFile = syncFile.ListLinkedFile[i];
                    string sql = string.Format(
                        "INSERT INTO LinkedFile (ID_SyncFile, FileName, IsLastNewest, FilePath, LastSyncDate) VALUES ({0}, '{1}', {2}, '{3}', {4})",
                        id, linkedFile.FileName, linkedFile.IsLastNewest, linkedFile.FilePath, linkedFile.LastSyncDate.ToBinary());
                    sqls.Add(sql);
                }
                //事务执行
                return ExecuteTransSQL(sqls, conn);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog("InsertSyncFile", ex);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>更新SyncFile</summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="fileSetName">文件集名称</param>
        /// <param name="listFileInfo">链接文件信息列表</param>
        /// <param name="id">文件集序号</param>
        /// <returns>操作是否成功</returns>
        public static bool UpdateSyncFile(SQLiteConnection conn, string fileSetName, List<FileInfoLite> listFileInfo, int id)
        {
            SyncFile syncFile;
            if (!FileSyncHelper.CreateSyncFile(fileSetName, listFileInfo, out syncFile, id) || syncFile == null)
                return false;

            return UpdateSyncFile(conn, syncFile);
        }

        /// <summary>更新SyncFile</summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="syncFile">文件集信息</param>
        /// <returns>操作是否成功</returns>
        public static bool UpdateSyncFile(SQLiteConnection conn, SyncFile syncFile)
        {
            if (conn == null || syncFile == null) return false;
            int countLinkedFile = syncFile.ListLinkedFile.Count;
            System.Diagnostics.Debug.Assert(countLinkedFile >= 2);

            try
            {
                //更新SyncFile表
                List<string> sqls = new List<string>(countLinkedFile + 1);
                string sqlSyncFile = string.Format(
                        "UPDATE SyncFile SET FileSetName = '{0}', LastFileHash = '{1}', LastUpdatePath = '{2}', LastUpdateDate = {3}, UpdateTimes = {4} WHERE ID = {5}",
                        syncFile.FileSetName, syncFile.LastFileHash, syncFile.LastUpdatePath, syncFile.LastUpdateDate.ToBinary(),
                        syncFile.UpdateTimes, syncFile.ID);
                sqls.Add(sqlSyncFile);
                //删除LinkedFile表中的过时信息
                var dtLinkedFile = GetDataTable("LinkedFile", conn);
                List<string> listFullName = syncFile.ListLinkedFile.Select(file => file.FullName).ToList();
                List<int> listDel = (from dr in dtLinkedFile.AsEnumerable()
                    let id_SyncFile = Convert.ToInt32(dr["ID_SyncFile"])
                    let fullPath = dr["FilePath"] + "\\" + dr["FileName"]
                    where id_SyncFile == syncFile.ID && !listFullName.Contains(fullPath)
                    select Convert.ToInt32(dr["ID"])).ToList();
                sqls.AddRange(listDel.Select(idDel => string.Format("DELETE FROM LinkedFile WHERE ID = {0}", idDel)));
                //更新LinkedFile表
                for (int i = 0; i < countLinkedFile; i++)
                {
                    LinkedFile linkedFile = syncFile.ListLinkedFile[i];
                    string sql = (from dr in dtLinkedFile.AsEnumerable()
                        let id = Convert.ToInt32(dr["ID"])
                        let id_SyncFile = Convert.ToInt32(dr["ID_SyncFile"])
                        let fullPath = dr["FilePath"] + "\\" + dr["FileName"]
                        where id_SyncFile == linkedFile.ID_SyncFile && fullPath.Equals(linkedFile.FullName)
                        select string.Format(
                            "UPDATE LinkedFile SET FileName = '{0}', IsLastNewest = {1}, FilePath = '{2}', LastSyncDate = {3} WHERE ID = {4}", 
                            linkedFile.FileName, linkedFile.IsLastNewest, linkedFile.FilePath,
                            linkedFile.LastSyncDate.ToBinary(), id)).FirstOrDefault() 
                            ?? string.Format(
                            "INSERT INTO LinkedFile (ID_SyncFile, FileName, IsLastNewest, FilePath, LastSyncDate) VALUES ({0}, '{1}', {2}, '{3}', {4})",
                            linkedFile.ID_SyncFile, linkedFile.FileName, linkedFile.IsLastNewest, linkedFile.FilePath,
                            linkedFile.LastSyncDate.ToBinary());
                    sqls.Add(sql);
                }
                //事务执行
                return ExecuteTransSQL(sqls, conn);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog("UpdateSyncFile", ex);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>删除SyncFile表中数据</summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="idSyncFile">文件集ID</param>
        /// <returns>操作是否成功</returns>
        public static bool DeleteSyncFile(SQLiteConnection conn, int idSyncFile)
        {
            if (conn == null || idSyncFile < 0) return false;

            try
            {
                //删除SyncFile表中数据
                List<string> sqls = new List<string>(2);
                string sqlSyncFile = string.Format("DELETE FROM SyncFile WHERE ID = {0}", idSyncFile);
                sqls.Add(sqlSyncFile);
                //删除LinkedFile表中数据
                string sqlLinkedFile = string.Format("DELETE FROM LinkedFile WHERE ID_Syncfile = {0}", idSyncFile);
                sqls.Add(sqlLinkedFile);
                //事务执行
                return ExecuteTransSQL(sqls, conn);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog("DeleteSyncFile", ex);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
