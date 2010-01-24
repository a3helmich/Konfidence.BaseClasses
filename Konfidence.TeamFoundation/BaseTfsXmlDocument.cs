using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Konfidence.Base;

namespace Konfidence.TeamFoundation
{
    public class BaseTfsXmlDocument : XmlDocument
    {
        private bool _Changed = false;
        private string _FullFileName = string.Empty;

        private XmlElement _Root;
        private string _NameSpaceURI;

        public bool Changed
        {
            get { return _Changed; }
            set { _Changed = value; }
        }

        public string FileName // TODO : moet protected worden
        {
            get { return _FullFileName; }
        }

        protected XmlElement Root
        {
            get { return _Root; }
        }

        public string NameSpaceURI // TODO : moet protected
        {
            get { return _NameSpaceURI; }
        }

        public override void Load(string fullFileName)
        {

            _FullFileName = fullFileName;

            base.Load(_FullFileName);

            _Root = DocumentElement;
            _NameSpaceURI = _Root.NamespaceURI;
        }

        protected static bool IsAssigned(object assignedObject)
        {
            return BaseItem.IsAssigned(assignedObject);
        }
    }
}
