using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace chenz
{
    static class CompareHelper
    {
        /// <summary>判断两个文件信息是否具有相同的大小和创建、修改时间。</summary>
        /// <param name="fi1">第一个文件信息</param>
        /// <param name="fi2">第二个文件信息</param>
        /// <returns></returns>
        public static bool IsFileSameSizeAndDate(FileInfo fi1, FileInfo fi2)
        {
            if (fi1 == null || fi2 == null) return false;
            if (!fi1.Exists || !fi2.Exists) return false;
            if (fi1.Length != fi2.Length) return false;
            if (fi1.CreationTime != fi2.CreationTime) return false;
            if (fi1.LastWriteTime != fi2.LastWriteTime) return false;
            return true;
        }

        /// <summary>判断两个文件是否具有相同的大小和创建、修改时间。</summary>
        /// <param name="filePath1">第一个文件路径</param>
        /// <param name="filePath2">第二个文件路径</param>
        /// <returns></returns>
        public static bool IsFileSameSizeAndDate(string filePath1, string filePath2)
        {
            if (string.IsNullOrWhiteSpace(filePath1) || string.IsNullOrWhiteSpace(filePath2))
                return false;
            FileInfo fi1 = new FileInfo(filePath1);
            FileInfo fi2 = new FileInfo(filePath2);
            return IsFileSameSizeAndDate(fi1, fi2);
        }

        /// <summary>判断两个文件是否相同。</summary>
        /// <param name="filePath1">第一个文件路径</param>
        /// <param name="filePath2">第二个文件路径</param>
        /// <returns></returns>
        public static bool IsFileSame(string filePath1, string filePath2)
        {
            if (IsFileSameSizeAndDate(filePath1, filePath2) == false) return false;

            //创建一个哈希算法对象 
            using (HashAlgorithm hash = HashAlgorithm.Create())
            {
                using (FileStream file1 = new FileStream(filePath1, FileMode.Open), 
                    file2 = new FileStream(filePath2, FileMode.Open))
                {
                    byte[] hashByte1 = hash.ComputeHash(file1);//哈希算法根据文本得到哈希码的字节数组 
                    byte[] hashByte2 = hash.ComputeHash(file2);
                    string str1 = BitConverter.ToString(hashByte1);//将字节数组装换为字符串 
                    string str2 = BitConverter.ToString(hashByte2);
                    return (str1 == str2);//比较哈希码 
                }
            }
        }
    }
}
