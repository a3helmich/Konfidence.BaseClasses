using System.Xml;

namespace Konfidence.TeamFoundation.Project
{
    public class ProjectXmlDocument: XmlDocument
    {
        private bool _Changed = false;

        public bool Changed
        {
            get { return _Changed; }
            set { _Changed = value; }
        }
    }
}
