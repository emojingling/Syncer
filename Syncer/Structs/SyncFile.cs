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
        private long _lastUpdateDate;
        private int _updateTimes;

        public int ID { get { return _id; } set { _id = value; } }
        public string FileSetName { get { return _fileSetName; } set { _fileSetName = value; } }
        public string LastFileHash { get { return _lastFileHash; } set { _lastFileHash = value; } }
        public string LastUpdatePath { get { return _lastUpdatePath; } set { _lastUpdatePath = value; } }
        public DateTime LastUpdateDate { get { return DateTime.FromBinary(_lastUpdateDate); } set { _lastUpdateDate = value.ToBinary(); } }
        public int UpdateTimes { get { return _updateTimes; } set { _updateTimes = value; } }

        private SyncFile()
        {

        }

        public SyncFile(int id, string fileSetName, string lastFileHash, string lastUpdatePath, DateTime lastUpdateDate, int updateTimes = 0)
        {
            ID = id;
            FileSetName = fileSetName;
            LastFileHash = lastFileHash;
            LastUpdatePath = lastUpdatePath;
            LastUpdateDate = lastUpdateDate;
            if (updateTimes < 0) updateTimes = 0;
            UpdateTimes = updateTimes;
        }

        public SyncFile(int id, string fileSetName, string lastFileHash, string lastUpdatePath, string lastUpdateDate, int updateTimes = 0)
        {
            DateTime dtLastUpdateDate = Convert.ToDateTime(lastUpdateDate);
            ID = id;
            FileSetName = fileSetName;
            LastFileHash = lastFileHash;
            LastUpdatePath = lastUpdatePath;
            LastUpdateDate = dtLastUpdateDate;
            if (updateTimes < 0) updateTimes = 0;
            UpdateTimes = updateTimes;
        }

        public override int GetHashCode()
        {
            return ID;
        }
    }
}
