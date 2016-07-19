using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace chenz
{
    [Serializable]
    public sealed class FileInfoLite
    {
        private string _fileName;
        private string _filePath;
        private DateTime _creationTime;
        private long _length;
        private DateTime _lastWriteTime;
        private bool _isReadOnly;

        public string FileName { get { return _fileName; } set { _fileName = value; } }
        public string FilePath { get { return _filePath; } set { _filePath = value; } }
        public DateTime CreationTime { get { return _creationTime; } set { _creationTime = value; } }

        public long Length { get { return _length; } set { _length = value; } }
        public DateTime LastWriteTime { get { return _lastWriteTime; } set { _lastWriteTime = value; } }
        public bool IsReadOnly { get { return _isReadOnly; } set { _isReadOnly = value; } }

        public string FullName { get { return FilePath + "\\" + FileName; } }

        private FileInfoLite() { }

        public FileInfoLite(FileInfo fileInfo)
        {
            FileName = fileInfo.Name;
            FilePath = fileInfo.DirectoryName;
            if (FilePath.EndsWith("\\")) FilePath.Substring(0, FilePath.Length - 1);
            CreationTime = fileInfo.CreationTime;
            Length = fileInfo.Length;
            LastWriteTime = fileInfo.LastWriteTime;
            IsReadOnly = fileInfo.IsReadOnly;
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}
