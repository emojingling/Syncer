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
        /// <summary>比较字节数组</summary>
        /// <param name="b1">字节数组1</param>
        /// <param name="b2">字节数组2</param>
        /// <returns>如果两个数组相同，返回0；如果数组1小于数组2，返回小于0的值；如果数组1大于数组2，返回大于0的值。</returns>
        public static int BytesCompare(byte[] b1, byte[] b2)
        {
            int result = 0;
            if (b1.Length != b2.Length)
                result = b1.Length - b2.Length;
            else
            {
                for (int i = 0; i < b1.Length; i++)
                {
                    if (b1[i] != b2[i])
                    {
                        result = (int)(b1[i] - b2[i]);
                        break;
                    }
                }
            }
            return result;
        }

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
                return CompareFileByHash(hash, filePath1, filePath2);
            }
            
        }

        /// <summary>根据哈希码判断两个文件是否相同</summary>
        /// <param name="hash">哈希算法对象</param>
        /// <param name="filePath1">第一个文件路径</param>
        /// <param name="filePath2">第二个文件路径</param>
        /// <returns></returns>
        private static bool CompareFileByHash(HashAlgorithm hash, string filePath1, string filePath2)
        {
            using (FileStream file1 = new FileStream(filePath1, FileMode.Open),
                    file2 = new FileStream(filePath2, FileMode.Open))
            {
                byte[] hashByte1 = hash.ComputeHash(file1); //哈希算法根据文本得到哈希码的字节数组
                byte[] hashByte2 = hash.ComputeHash(file2);
                return BytesCompare(hashByte1, hashByte2) == 0; //比较哈希码
            }
        }

        /// <summary>判断两组文件是否（分别）相同。</summary>
        /// <param name="filePaths1">第一组文件路径</param>
        /// <param name="filePaths2">第二组文件路径</param>
        /// <exception cref="ArgumentNullException ">所有参数必须非空</exception>
        /// <exception cref="ArgumentException">两个参数长度必须相同</exception>
        /// <returns></returns>
        public static bool[] IsFilesSame(IEnumerable<string> filePaths1, IEnumerable<string> filePaths2)
        {
            if (filePaths1 == null || filePaths2 == null)
                throw new ArgumentNullException("待比较的数组为null！");
            int count = filePaths1.Count();
            if (filePaths2.Count() != count)
                throw new ArgumentException("待比较的两个数组长度不同！");
            if (count == 0) return new bool[0];

            bool[] results = new bool[count];

            using (HashAlgorithm hash = HashAlgorithm.Create())
            {
                IEnumerator<string> ienu1 = filePaths1.GetEnumerator();
                IEnumerator<string> ienu2 = filePaths2.GetEnumerator();
                int index = 0;
                while (ienu1.MoveNext() && ienu2.MoveNext())
                {
                    string filePath1 = ienu1.Current;
                    string filePath2 = ienu2.Current;
                    if (IsFileSameSizeAndDate(filePath1, filePath2) == false)
                    {
                        results[index] = false;
                    }
                    else
                    {
                        results[index] = CompareFileByHash(hash, filePath1, filePath2);
                    }
                    index++;
                }
            }

            return results;
        }
    }
}
