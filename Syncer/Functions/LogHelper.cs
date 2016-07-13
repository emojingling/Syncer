using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace chenz
{
    public static class LogHelper
    {
        private const string OtherMessages = "No Other Messages";

        /// <summary>将运行错误写入Log日志</summary>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        public static void WriteErrLog(string functionName, Exception e)
        {
            if (e == null) return;
            WriteErrLog(functionName, e.ToString());
        }
        /// <summary>将运行错误写入Log日志</summary>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        public static void WriteErrLog(string functionName, string errText)
        {
            FileStream fStream = null;
            string writeLine = string.Empty;
            try
            {
                string canWrite = SycerSQLiteHelper.GetSetting("WriteErrLog").ToUpper();
                if (canWrite.Equals("FALSE")) return;
                //得到当前时间及文件路径
                var dataTime = DateTime.Now;
                var now = string.Format("{0} {1}", dataTime.ToShortDateString(), dataTime.ToLongTimeString());
                var fileName = dataTime.ToString("yyyy-MM") + ".log";
                var dirPath = Application.StartupPath + "\\..\\Log";
                var filePath = string.Format("{0}\\..\\Log\\{1}",
                    Application.StartupPath, fileName); //每月一个记录
                //创建/打开.log文件
                if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
                fStream = !File.Exists(filePath) ? File.Create(filePath) : File.Open(filePath, FileMode.Append);
                //增加数据行
                writeLine = string.Format("{0}||{1}||{2}||{3}{4}", now, functionName, errText,
                    OtherMessages, Environment.NewLine); //将所有信息合为一行
            }
            catch { return; }
            try
            {
                var arrFunctionName = Encoding.UTF8.GetBytes(writeLine);
                fStream.Write(arrFunctionName, 0, arrFunctionName.Length);
            }
            catch { }
            finally
            {
                if (fStream != null) fStream.Close();
            }
        }

        public static void OpenLog()
        {
            try
            {
                //得到当前时间及文件路径
                var dataTime = DateTime.Now;
                var now = string.Format("{0} {1}", dataTime.ToShortDateString(), dataTime.ToLongTimeString());
                var fileName = dataTime.ToString("yyyy-MM") + ".log";
                var dirPath = Application.StartupPath + "\\..\\Log";
                var filePath = string.Format("{0}\\..\\Log\\{1}",
                    Application.StartupPath, fileName); //每月一个记录
                //打开文件
                if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
                if (File.Exists(filePath))
                {
                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    proc.StartInfo.FileName = filePath;
                    proc.StartInfo.UseShellExecute = true;
                    proc.Start();
                }
            }
            catch (Exception ex)
            {
                WriteErrLog("OpenLog", ex);
            }
        }
    }
}