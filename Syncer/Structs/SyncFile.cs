using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chenz
{
    [Serializable]
    public sealed class SyncFile
    {
        private int _id;
        private string _fileSetName;
        private string _lastFileHash;
        private string _lastUpdatePath;
        private DateTime _lastUpdateDate;
        private int _updateTimes;
        private List<LinkedFile> _listLinkedFile;

        public int ID { get { return _id; } set { _id = value; } }
        public string FileSetName { get { return _fileSetName; } set { _fileSetName = value; } }
        public string LastFileHash { get { return _lastFileHash; } set { _lastFileHash = value; } }
        public string LastUpdatePath { get { return _lastUpdatePath; } set { _lastUpdatePath = value; } }
        public DateTime LastUpdateDate { get { return _lastUpdateDate; } set { _lastUpdateDate = value; } }
        public int UpdateTimes { get { return _updateTimes; } set { _updateTimes = value; } }
        public List<LinkedFile> ListLinkedFile { get { return _listLinkedFile; } set { _listLinkedFile = value; } }

        public int FileNum
        {
            get
            {
                if (ListLinkedFile == null) return 0;
                else return ListLinkedFile.Count;
            }
        }

        public List<string> ListFullName
        {
            get
            {
                if (ListLinkedFile == null) return null;
                List<string> list = new List<string>(ListLinkedFile.Count);
                foreach (var linkedFile in ListLinkedFile)
                {
                    list.Add(linkedFile.FullName);
                }
                return list;
            }
        }

        private SyncFile()
        {

        }

        public SyncFile(int id, string fileSetName, string lastFileHash, string lastUpdatePath, DateTime lastUpdateDate, 
            List<LinkedFile> listLinkedFile, int updateTimes = 0)
        {
            ID = id;
            FileSetName = fileSetName;
            LastFileHash = lastFileHash;
            LastUpdatePath = lastUpdatePath;
            LastUpdateDate = lastUpdateDate;
            if (updateTimes < 0) updateTimes = 0;
            UpdateTimes = updateTimes;
            ListLinkedFile = listLinkedFile;
        }

        public SyncFile(int id, string fileSetName, string lastFileHash, string lastUpdatePath, string lastUpdateDate,
            List<LinkedFile> listLinkedFile, int updateTimes = 0)
        {
            DateTime dtLastUpdateDate = Convert.ToDateTime(lastUpdateDate);
            ID = id;
            FileSetName = fileSetName;
            LastFileHash = lastFileHash;
            LastUpdatePath = lastUpdatePath;
            LastUpdateDate = dtLastUpdateDate;
            if (updateTimes < 0) updateTimes = 0;
            UpdateTimes = updateTimes;
            ListLinkedFile = listLinkedFile;
        }

        public override int GetHashCode()
        {
            return ID;
        }
    }
}
