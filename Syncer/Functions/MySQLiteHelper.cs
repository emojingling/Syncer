using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace chenz
{
    public class MySQLiteHelper
    {
        

        #region 创建数据库、表
        public static void CreateEmptyDB(string path)
        {
            try
            {
                if (File.Exists(path)) return;
                SQLiteConnection.CreateFile(path);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        public static void CreateEmptyDB(string path, string password)
        {
            try
            {
                if (File.Exists(path)) return;
                SQLiteConnection.CreateFile(path);

                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + path))
                {
                    conn.ChangePassword(password);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        public static void CreateDemoDB(string path)
        {
            SQLiteConnection conn = null;
            try
            {
                string dbPath = "Data Source = " + path;
                conn = new SQLiteConnection(dbPath); //创建数据库实例，指定文件位置
                conn.Open(); //打开数据库，若文件不存在会自动创建
                string tableName = string.Format("CarRec_{0}{1}", DateTime.Now.Year, DateTime.Now.Month);
                string sql = string.Format("CREATE TABLE IF NOT EXISTS {0}(id integer, name varchar(20), sex varchar(2))", tableName);
                //建表语句
                SQLiteCommand cmdCreateTable = new SQLiteCommand(sql, conn);
                cmdCreateTable.ExecuteNonQuery(); //如果表不存在，创建数据表
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        private static string GetSQLiteType(Type type)
        {
            if (type == typeof(int))
            {
                return "INT";
            }
            if (type == typeof(string))
            {
                return "NTEXT";
            }
            if (type == typeof(long))
            {
                return "INT64";
            }
            if (type == typeof(bool))
            {
                return "BOOLEAN";
            }
            if (type == typeof(double))
            {
                return "DOUBLE";
            }
            if (type == typeof(DateTime))
            {
                return "DATETIME";
            }
            if (type == typeof(short))
            {
                return "SMALLINT";
            }
            return "BLOB";
        }

        public static bool CreateTable(SQLiteConnection conn, string tableName, IEnumerable<string> colNames, IEnumerable<string> types)
        {
            if (conn == null || colNames == null || types == null) return false;
            if (colNames.Count() != types.Count()) return false;
            try
            {
                StringBuilder sb = new StringBuilder("CREATE TABLE IF NOT EXISTS ");
                sb.Append(tableName);
                sb.Append(" (");
                var enu1 = colNames.GetEnumerator();
                var enu2 = types.GetEnumerator();
                while (enu1.MoveNext() && enu2.MoveNext())
                {
                    sb.Append(enu1.Current);
                    sb.Append(" ");
                    sb.Append(enu2.Current);
                    sb.Append(",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(")");
                conn.Open();
                using (SQLiteCommand command = new SQLiteCommand(sb.ToString(), conn))
                {
                    command.ExecuteNonQuery();
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return false;
            }
            return true;
        }

        public static bool CreateTable(SQLiteConnection conn, string tableName, IEnumerable<string> colNames, IEnumerable<Type> types)
        {
            if (types == null || !types.Any()) return false;

            List<string> listType = new List<string>(types.Count());
            listType.AddRange(types.Select(GetSQLiteType));
            return CreateTable(conn, tableName, colNames, listType);
        }
        #endregion

        #region 得到连接字符串
        public static string GetConnectionString(string path)
        {
            return GetConnectionString(path, null);
        }

        public static string GetConnectionString(string path, string password)
        {
            if (string.IsNullOrEmpty(password))
                return "Data Source=" + path;
            return string.Format("Data Source={0};Password={1}", path, password);
        }
        #endregion

        #region 修改数据库密码
        /// <summary>修改密码</summary>
        /// <param name="path">数据库路径</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="oldPassword">没有密码时为空</param>
        public static bool ChangePassword(string path, string newPassword, string oldPassword = null)
        {
            try
            {
                var con = new SQLiteConnection(GetConnectionString(path, oldPassword));
                con.Open();
                con.ChangePassword(newPassword);
                con.Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return false;
            }
            return true;
        }
        #endregion

        #region 向数据库写入SQL语句
        public static bool ExecuteSQL(string sql, string path, string password)
        {
            try
            {
                var conn = new SQLiteConnection(GetConnectionString(path, password));
                conn.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                {
                    command.ExecuteNonQuery();
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return false;
            }
            return true;
        }

        public static bool ExecuteSQL(string sql, SQLiteConnection conn)
        {
            try
            {
                conn.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                {
                    command.ExecuteNonQuery();
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return false;
            }
            return true;
        }

        public static bool ExecuteTransSQL(IEnumerable<string> sqls, string path, string password)
        {
            SQLiteConnection conn;
            try
            {
                conn = new SQLiteConnection(GetConnectionString(path, password));
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return false;
            }
            return ExecuteTransSQL(sqls, conn);
        }

        public static bool ExecuteTransSQL(IEnumerable<string> sqls, SQLiteConnection conn)
        {
            if (sqls == null || conn == null) return false;
            SQLiteTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                IEnumerator<string> ienu = sqls.GetEnumerator();
                using (var cmd = conn.CreateCommand())
                {
                    while (ienu.MoveNext())
                    {
                        cmd.CommandText = ienu.Current;
                        cmd.ExecuteNonQuery();
                    }
                }
                trans.Commit();               
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                    trans.Dispose();
                    trans = null;
                }
                LogHelper.WriteErrLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return false;
            }
            finally
            {
                conn.Close();
            }
            return true;
        }
        #endregion

        #region 得到Reader或表

        public static SQLiteDataReader GetReader(SQLiteConnection conn, string sql)
        {
            if (conn == null || sql == null)
                throw new NullReferenceException("部分参数为null!");

            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            SQLiteDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        public static DataTable GetDataTable(String inputFile, string tableName)
        {
            DataTable dt = new DataTable();
            SQLiteConnection cnn = null;
            SQLiteDataReader reader = null;
            string sql = "SELECT * FROM " + tableName;
            try
            {
                var dbConnection = String.Format("Data Source={0}", inputFile);
                cnn = new SQLiteConnection(dbConnection);
                cnn.Open();
                SQLiteCommand mycommand = new SQLiteCommand(cnn);
                mycommand.CommandText = sql;
                reader = mycommand.ExecuteReader();
                dt.Load(reader);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (cnn != null)
                {
                    cnn.Close();
                }
            }
            return dt;
        }

        public static DataTable GetDataTable(String inputFile, string sql, IList<SQLiteParameter> cmdparams)
        {
            var dbConnection = String.Format("Data Source={0}", inputFile);
            DataTable dt = new DataTable();
            SQLiteConnection cnn = null;
            SQLiteDataReader reader = null;
            try
            {
                cnn = new SQLiteConnection(dbConnection);
                cnn.Open();
                SQLiteCommand mycommand = new SQLiteCommand(cnn);
                mycommand.CommandText = sql;
                mycommand.Parameters.AddRange(cmdparams.ToArray());
                mycommand.CommandTimeout = 180;
                reader = mycommand.ExecuteReader();
                dt.Load(reader);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (cnn != null)
                {
                    cnn.Close();
                }
            }
            return dt;
        }

        public static DataTable GetDataTable(string tableName, SQLiteConnection conn)
        {
            DataTable dt = new DataTable();
            SQLiteDataReader reader = null;
            string sql = "SELECT * FROM " + tableName;
            try
            {
                conn.Open();
                SQLiteCommand mycommand = new SQLiteCommand(conn);
                mycommand.CommandText = sql;
                reader = mycommand.ExecuteReader();
                dt.Load(reader);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return dt;
        }

        public static DataTable GetDataTable(string sql, IList<SQLiteParameter> cmdparams, SQLiteConnection conn)
        {
            DataTable dt = new DataTable();
            SQLiteDataReader reader = null;
            try
            {
                conn.Open();
                SQLiteCommand mycommand = new SQLiteCommand(conn);
                mycommand.CommandText = sql;
                mycommand.Parameters.AddRange(cmdparams.ToArray());
                mycommand.CommandTimeout = 180;
                reader = mycommand.ExecuteReader();
                dt.Load(reader);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return dt;
        }

        #endregion

        #region 数据库优化操作

        /// <summary>更改表的下一个自增默认值</summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="tableName">表名</param>
        /// <param name="value">自增值</param>
        /// <returns>操作是否成功</returns>
        public static bool SetAutoIncr(SQLiteConnection conn, string tableName, int value = 0)
        {
            if (conn == null || string.IsNullOrWhiteSpace(tableName)) return false;
            if (value < 0) value = 0;
            string sql = String.Format("UPDATE sqlite_sequence SET seq = {0} WHERE name='{1}'", value, tableName);
            return ExecuteSQL(sql, conn);
        }

        /// <summary>压缩数据库</summary>
        /// <param name="conn">数据库连接</param>
        /// <returns>操作是否成功</returns>
        public static bool Vacuum(SQLiteConnection conn)
        {
            if (conn == null) return false;

            string sql = "VACUUM";
            return ExecuteSQL(sql, conn);
        }

        #endregion
    }
}
