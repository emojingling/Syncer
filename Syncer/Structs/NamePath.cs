using System;

namespace chenz
{
    [Serializable]
    public sealed class NamePath
    {
        private string _name;
        private string _path;

        private NamePath() { }

        public NamePath(string name, string path)
        {
            _name = name;
            _path = path;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }
    }
}
