using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace Konfidence.Base
{
    public class BaseXmlDocument : XmlDocument
    {
        private XmlElement _Root = null;

        private string _FullFileName = string.Empty;
        private string _PathName = string.Empty;
        private string _RootNameSpaceURI;
        private XmlNamespaceManager _XmlNamespaceManager;

        #region read only properties
        public XmlElement Root
        {
            get { return _Root; }
        }

        public string FileName
        {
            get { return _FullFileName; }
        }

        public string PathName
        {
            get { return _PathName; }
        }

        public string RootNameSpaceURI
        {
            get { return _RootNameSpaceURI; }
        }

        public XmlNamespaceManager XmlNamespaceManager
        {
            get { return _XmlNamespaceManager; }
        }
        #endregion read only properties

        public override void Load(string filename)
        {
            _FullFileName = filename;

            _PathName = Path.GetDirectoryName(filename) + @"\";

            base.Load(filename);

            PostLoad();
        }

        private void PostLoad()
        {
            _Root = DocumentElement;

            _RootNameSpaceURI = Root.NamespaceURI;

            // create a shortcut namespace reference
            _XmlNamespaceManager = new XmlNamespaceManager(NameTable);
            _XmlNamespaceManager.AddNamespace("p", _RootNameSpaceURI);
        }

        public override void Load(Stream inStream)
        {
            base.Load(inStream);

            PostLoad();
        }

        public override void Load(TextReader txtReader)
        {
            base.Load(txtReader);

            PostLoad();
        }

        public override void Load(XmlReader reader)
        {
            base.Load(reader);

            PostLoad();
        }

        public override void LoadXml(string xml)
        {
            base.LoadXml(xml);

            PostLoad();
        }

        public void GetAttributeValue(string nodeName, string attributeName, out string value)
        {
            XmlNode valueNode = Root.SelectSingleNode(nodeName);

            GetAttributeValue(valueNode, attributeName, out value);
        }

        public void GetAttributeValue(XmlNode valueNode, string attributeName, out string value)
        {
            value = string.Empty;

            if (IsAssigned(valueNode))
            {
                if (IsAssigned(valueNode.Attributes[attributeName]))
                {
                    value = valueNode.Attributes[attributeName].Value;
                }
            }
        }

        public void GetValue(string nodeName, out string value)
        {
            value = string.Empty;

            XmlNode valueNode = Root.SelectSingleNode(nodeName);

            if (IsAssigned(valueNode))
            {
                value = valueNode.InnerText;
            }
        }

        public void GetValue(string nodeName, out bool value)
        {
            value = false;

            XmlNode valueNode = Root.SelectSingleNode(nodeName);

            if (IsAssigned(valueNode))
            {
                bool.TryParse(valueNode.InnerText, out value);
            }
        }

        protected static bool IsAssigned(object assignedObject)
        {
            return BaseItem.IsAssigned(assignedObject);
        }

        protected static bool IsEmpty(string assignedObject)
        {
            return BaseItem.IsEmpty(assignedObject);
        }

        protected static bool IsNull(string assignedObject)
        {
            return BaseItem.IsNull(assignedObject);
        }
    }
}
