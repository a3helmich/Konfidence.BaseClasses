using System.Xml;

namespace Konfidence.TeamFoundation.Project
{
    public class ProjectXmlDocument: XmlDocument
    {
        private bool _Changed = false;
        private string _FileName = string.Empty;

        public bool Changed
        {
            get { return _Changed; }
            set { _Changed = value; }
        }

        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        public override void Load(string filename)
        {
            _FileName = filename;

            base.Load(filename);
        }
    }
}
