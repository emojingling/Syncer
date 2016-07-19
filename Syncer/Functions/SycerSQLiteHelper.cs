using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

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
                List<LinkedFile> list = new List<LinkedFile>();
                foreach (DataRow dr in dtLinkedFile.AsEnumerable())
                {
                    if (Convert.ToInt32(dr["ID_SyncFile"]) == id_SyncFile)
                    {
                        LinkedFile linkedFile = new LinkedFile(dr);
                        list.Add(linkedFile);
                    }
                }
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
                        syncFile.FileSetName, syncFile.LastFileHash, syncFile.LastUpdatePath, syncFile.LastUpdateDate,
                        syncFile.UpdateTimes, syncFile.ID);
                sqls.Add(sqlSyncFile);
                //删除LinkedFile表中的过时信息
                var dtLinkedFile = GetDataTable("LinkedFile", conn);
                sqls.AddRange(from dr in dtLinkedFile.AsEnumerable()
                    let id_SyncFile = Convert.ToInt32(dr["ID_SyncFile"])
                    where id_SyncFile == syncFile.ID
                    select Convert.ToInt32(dr["ID"])
                    into id
                    let haveId = syncFile.ListLinkedFile.Any(linkedFile => linkedFile.ID == id)
                    where !haveId
                    select string.Format("DELETE FROM LinkedFile WHERE ID = {0}", id));
                //更新LinkedFile表
                for (int i = 0; i < countLinkedFile; i++)
                {
                    LinkedFile linkedFile = syncFile.ListLinkedFile[i];
                    string sql = (from dr in dtLinkedFile.AsEnumerable()
                        let id = Convert.ToInt32(dr["ID"])
                        let id_SyncFile = Convert.ToInt32(dr["ID_SyncFile"])
                        where id == linkedFile.ID && id_SyncFile == linkedFile.ID_SyncFile
                        select string.Format(
                            "UPDATE LinkedFile SET FileName = '{0}', IsLastNewest = {1}, FilePath = '{2}', LastSyncDate = {3} WHERE ID = {4}", 
                            linkedFile.FileName, linkedFile.IsLastNewest, linkedFile.FilePath, 
                            linkedFile.LastSyncDate, id)).FirstOrDefault() 
                            ?? string.Format(
                            "INSERT INTO LinkedFile (ID_SyncFile, FileName, IsLastNewest, FilePath, LastSyncDate) VALUES ({0}, '{1}', {2}, '{3}', {4})",
                            linkedFile.ID_SyncFile, linkedFile.FileName, linkedFile.IsLastNewest, linkedFile.FilePath,
                            linkedFile.LastSyncDate);
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

        /// <summary>得到设置项的值</summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="settingKey">设置项名称</param>
        /// <returns>设置项的值</returns>
        public static string GetSetting(SQLiteConnection conn, string settingKey)
        {
            string sql = @"select SValue from SettingsTable where 'SKey'=" + settingKey;
            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            {
                try
                {
                    conn.Open();
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {  
                        return null;
                    }
                    else
                    {
                        return obj.ToString();
                    }
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    LogHelper.WriteErrLog("GetSetting", ex);
                }
                finally
                {
                    conn.Close();
                }
            }

            return null;
            }
    }
}
