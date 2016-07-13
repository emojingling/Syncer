using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace chenz
{
    class SycerSQLiteHelper : MySQLiteHelper
    {
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
