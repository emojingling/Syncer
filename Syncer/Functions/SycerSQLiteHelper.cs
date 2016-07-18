using System;
using System.Collections.Generic;
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

        public static bool UpdateSyncFile(SQLiteConnection conn, SyncFile syncFile)
        {

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
