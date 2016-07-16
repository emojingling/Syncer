using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace chenz
{
    public class ConfigHelper
    {
        #region 内部变量

        public static string FileName = Path.GetFileName(System.Windows.Forms.Application.ExecutablePath);
        public static readonly string ExePath = Assembly.GetExecutingAssembly().Location;

        #endregion

        #region 通用函数

        /// <summary>
        ///     查询config文件是否包含记录
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="section">配置节名称</param>
        /// <returns>该key是否存在</returns>
        public static bool CheckRecordExist(string key, string section = "appSettings")
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ExePath);
                var appSettingsSection = (AppSettingsSection)config.GetSection(section);
                return (appSettingsSection.Settings[key] != null);
            }
            catch (Exception e)
            {
                LogHelper.WriteErrLog("AppConfigCon.CheckRecordExist", e);
                return false;
            }
        }

        /// <summary>
        ///     向config文件添加记录
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">键值</param>
        /// <param name="section">配置节名称</param>
        /// <returns>添加是否成功</returns>
        public static bool AddRecord(string key, string value, string section = "appSettings")
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ExePath);
                var appSettingsSection = (AppSettingsSection)config.GetSection(section);
                if (appSettingsSection.Settings[key] == null)
                {
                    appSettingsSection.Settings.Add(key, value);
                }
                else
                {
                    appSettingsSection.Settings[key].Value = value;
                }
                //config.AppSettings.Settings.Add(key, value);
                config.Save(ConfigurationSaveMode.Modified);
            }
            catch (Exception e)
            {
                var err = string.Format("App.Config can't add key: {0}!", key);
                LogHelper.WriteErrLog(err, e);
                return false;
            }

            return true;
        }

        /// <summary>检查输入项的初始化情况</summary>
        /// <param name="keys">输入项</param>
        /// <param name="values">输入项数据</param>
        /// <param name="section">配置节名称</param>
        /// <returns>操作是否成功</returns>
        /// <exception cref="Exception">keys and values have different count!</exception>
        public static bool IntiRecords(string[] keys, string[] values, string section = "appSettings")
        {
            try
            {
                int count = keys.Length;
                if (values.Length != count) throw new Exception("keys and values have different count!");
                var config = ConfigurationManager.OpenExeConfiguration(ExePath);
                var appSettingsSection = (AppSettingsSection)config.GetSection(section);
                for (int i = 0; i < count; i++)
                {
                    if (appSettingsSection.Settings[keys[i]] == null)
                    {
                        appSettingsSection.Settings.Add(keys[i], values[i]);
                    }
                }
                config.Save(ConfigurationSaveMode.Modified);
            }
            catch (Exception e)
            {
                LogHelper.WriteErrLog("AppConfigCon.IntiRecords", e);
                return false;
            }
            return true;
        }

        /// <summary>
        ///     从config文件读取记录
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="section">配置节名称</param>
        /// <returns>读取得到的键值</returns>
        public static string ReadRecord(string key, string section = "appSettings")
        {
            try
            {
                var value = string.Empty;
                var config = ConfigurationManager.OpenExeConfiguration(ExePath);
                var appSettingsSection = (AppSettingsSection)config.GetSection(section);
                if (appSettingsSection.Settings[key] != null)
                {
                    value = appSettingsSection.Settings[key].Value;
                }
                //string value = config.AppSettings.Settings[key].Value;
                return value;
            }
            catch (Exception e)
            {
                var err = string.Format("App.Config can't read key: {0}!", key);
                LogHelper.WriteErrLog(err, e);
                return "DataErr.";
            }
        }

        /// <summary>向config文件更新记录</summary>
        /// <param name="key">键</param>
        /// <param name="newValue">新键值</param>
        /// <param name="section">配置节名称</param>
        /// <returns>更新是否成功</returns>
        /// <exception cref="Exception">No match record exist!</exception>
        public static bool UpdateRecord(string key, string newValue, string section = "appSettings")
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ExePath);
                var appSettingsSection = (AppSettingsSection)config.GetSection(section);
                if (appSettingsSection.Settings[key] != null)
                {
                    appSettingsSection.Settings[key].Value = newValue;
                }
                else
                {
                    throw new Exception("No match record exist!");
                }
                config.Save(ConfigurationSaveMode.Modified);
            }
            catch (Exception e)
            {
                var err = string.Format("App.Config can't modify key: {0}!", key);
                LogHelper.WriteErrLog(err, e);
                return false;
            }
            return true;
        }

        /// <summary>向config文件批量更新记录</summary>
        /// <param name="keys">The keys.</param>
        /// <param name="newValues">The new values.</param>
        /// <param name="section">配置节名称</param>
        /// <returns>更新是否成功</returns>
        /// <exception cref="InvalidCastException">keys and values have diff count!</exception>
        /// <exception cref="NullReferenceException">No match record exist in keys[i]!</exception>
        /// <exception cref="Exception">No match record exist!</exception>
        public static bool UpdateRecord(string[] keys, string[] newValues, string section = "appSettings")
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ExePath);
                var appSettingsSection = (AppSettingsSection)config.GetSection(section);
                int count = keys.Length;
                if (count != newValues.Length) throw new InvalidCastException("keys and values have diff count!");
                for (int i = 0; i < count; i++)
                {
                    if (appSettingsSection.Settings[keys[i]] == null) throw new NullReferenceException("No match record exist in keys[i]!");
                    appSettingsSection.Settings[keys[i]].Value = newValues[i];
                }
                config.Save(ConfigurationSaveMode.Modified);
            }
            catch (Exception e)
            {
                LogHelper.WriteErrLog("AppConfigCon.UpdateRecord", e);
                return false;
            }
            return true;
        }

        /// <summary>
        ///     从config文件删除记录
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="section">配置节名称</param>
        /// <returns>删除是否成功</returns>
        public static bool DeleteRecord(string key, string section = "appSettings")
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ExePath);
                var appSettingsSection = (AppSettingsSection)config.GetSection(section);
                if (appSettingsSection.Settings[key] != null)
                {
                    appSettingsSection.Settings.Remove(key);
                }
                else
                {
                    throw new Exception("No match record exist!");
                }
                //config.AppSettings.Settings.Remove(key);
                config.Save(ConfigurationSaveMode.Modified);
            }
            catch (Exception e)
            {
                var err = string.Format("App.Config can't delete key: {0}!", key);
                LogHelper.WriteErrLog(err, e);
                return false;
            }

            return true;
        }

        #endregion
    }
}
