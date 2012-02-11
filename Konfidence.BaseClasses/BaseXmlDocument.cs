using System;
using System.Collections.Generic;
using System.Linq;
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

        #region read only properties
        public XmlElement Root
        {
            get { return _Root; }
        }

        public string FileName
        {
            get { return _FullFileName; }
        }

        protected string PathName
        {
            get { return _PathName; }
        }
        #endregion read only properties

        public override void Load(string filename)
        {
            _FullFileName = filename;

            _PathName = Path.GetDirectoryName(filename) + @"\";

            base.Load(filename);

            _Root = DocumentElement;
        }

        public override void Load(Stream inStream)
        {
            base.Load(inStream);

            _Root = DocumentElement;
        }

        public override void Load(TextReader txtReader)
        {
            base.Load(txtReader);

            _Root = DocumentElement;
        }

        public override void Load(XmlReader reader)
        {
            base.Load(reader);

            _Root = DocumentElement;
        }

        public override void LoadXml(string xml)
        {
            base.LoadXml(xml);

            _Root = DocumentElement;
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
                value = valueNode.Value;
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
