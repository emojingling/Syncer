﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace chenz
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            ConfigHelper.AddRecord("WriteErrLog", "true");
            ConfigHelper.AddRecord("SyncMode", "SyncToNewest");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }
    }
}
