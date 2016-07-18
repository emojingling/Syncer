using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chenz
{
    [Serializable]
    public sealed class LinkedFile
    {
        private int _id;
        private int _id_SyncFile;
        private string _fileName;
        private int _isLastNewest;
        private string _filePath;
        private long _lastSyncDate;

        public int ID { get { return _id; } set { _id = value; } }
        public int ID_SyncFile { get { return _id_SyncFile; } set { _id_SyncFile = value; } }
        public string FileName { get { return _fileName; } set { _fileName = value; } }
        public int IsLastNewest { get { return _isLastNewest; } set { _isLastNewest = value; } }



        public string FilePath { get { return _filePath; } set { _filePath = value; } }
        public DateTime LastSyncDate { get { return DateTime.FromBinary(_lastSyncDate); } set { _lastSyncDate = value.ToBinary(); } }

        private LinkedFile() { }
        public LinkedFile()
    }
}
