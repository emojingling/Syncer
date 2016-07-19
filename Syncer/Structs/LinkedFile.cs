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
        private DateTime _lastSyncDate;

        public int ID { get { return _id; } set { _id = value; } }
        public int ID_SyncFile { get { return _id_SyncFile; } set { _id_SyncFile = value; } }
        public string FileName { get { return _fileName; } set { _fileName = value; } }
        public int IsLastNewest { get { return _isLastNewest; } set { _isLastNewest = value; } }



        public string FilePath { get { return _filePath; } set { _filePath = value; } }
        public DateTime LastSyncDate { get { return _lastSyncDate; } set { _lastSyncDate = value; } }

        public string FullName { get { return FilePath + "\\" + FileName; } }

        private LinkedFile() { }
        public LinkedFile(int id, int id_SyncFile, string fileName, bool isLastNewest, string filePath, DateTime lastSyncDate)
        {
            ID = id;
            ID_SyncFile = id_SyncFile;
            FileName = fileName;
            IsLastNewest = isLastNewest ? 1 : 0;
            FilePath = filePath;
            LastSyncDate = lastSyncDate;
        }

        public LinkedFile(System.Data.DataRow dr)
        {
            ID = Convert.ToInt32(dr["ID"]);
            ID_SyncFile = Convert.ToInt32(dr["ID_SyncFile"]);
            FileName = dr["FileName"].ToString();
            IsLastNewest = Convert.ToInt32(dr["IsLastNewest"]);
            FilePath = dr["FilePath"].ToString();
            LastSyncDate = DateTime.FromBinary(Convert.ToInt64(dr["LastSyncDate"]));
        }
    }
}
