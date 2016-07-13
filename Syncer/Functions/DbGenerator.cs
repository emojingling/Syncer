using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace chenz.Functions
{
    class DbGenerator
    {


        private static void CreateSettingsTable(SQLiteConnection conn)
        {
            string[] colNamesSettings = { "ID", "SKey", "SValue" };
            string[] colTpyesSettings = { "integer PRIMARY KEY AUTOINCREMENT", "TEXT", "TEXT" };
            MySQLiteHelper.CreateTable(conn, "SettingsTable", colNamesSettings, colTpyesSettings);

            MySQLiteHelper.ExecuteSQL(string.Format(@"insert into SettingsTable values ('{0}', '{1}')", "WriteErrLog", "True"), conn);
            //Todo 加入选项
        }
    }
}
