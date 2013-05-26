using System.Xml;
using System.IO;
using JetBrains.Annotations;

namespace Konfidence.Base
{
    public class BaseXmlDocument : XmlDocument
    {
        private XmlElement _Root;

        private string _FullFileName = string.Empty;
        private string _PathName = string.Empty;
        private string _RootNameSpaceUri;
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

        public string RootNameSpaceUri
        {
            get { return _RootNameSpaceUri; }
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

            _RootNameSpaceUri = Root.NamespaceURI;

            // create a shortcut namespace reference
            _XmlNamespaceManager = new XmlNamespaceManager(NameTable);
            _XmlNamespaceManager.AddNamespace("p", _RootNameSpaceUri);
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
            var valueNode = Root.SelectSingleNode(nodeName);

            GetAttributeValue(valueNode, attributeName, out value);
        }

        public void GetAttributeValue(XmlNode valueNode, string attributeName, out string value)
        {
            value = string.Empty;

            if (IsAssigned(valueNode))
            {
                var attributes = valueNode.Attributes;

                if (attributes != null)
                {
                    var attribute = attributes[attributeName];

                    if (IsAssigned(attribute))
                    {
                        value = attribute.Value;
                    }
                }
            }
        }

        public void GetValue(string nodeName, out string value)
        {
            GetValue(Root, nodeName, out value);
        }

        public void GetValue(XmlNode node,string nodeName, out string value)
        {
            value = string.Empty;

            var valueNode = node.SelectSingleNode(nodeName);

            if (IsAssigned(valueNode))
            {
                value = valueNode.InnerText;
            }
        }

        public void GetValue(string nodeName, out bool value)
        {
            GetValue(Root, nodeName, out value);
        }

        public void GetValue(XmlNode node,string nodeName, out bool value)
        {
            value = false;

            var valueNode = node.SelectSingleNode(nodeName);

            if (IsAssigned(valueNode))
            {
                bool.TryParse(valueNode.InnerText, out value);
            }
        }

        public XmlNode SetValue(XmlNode parentNode, string name, string value)
        {
            var childElement = parentNode.SelectSingleNode(name);

            if (!IsAssigned(childElement))
            {
                childElement = CreateElement(name, RootNameSpaceUri);

                parentNode.AppendChild(childElement);
            }

            childElement.InnerText = value;

            return childElement;
        }

        public XmlNode SetValue(string name, string value)
        {
            return SetValue(Root, name, value);
        }

        public XmlNode GetNode(XmlNode parentNode, string name)
        {
            //XmlNode childElement = parentNode.SelectSingleNode(name, _XmlNamespaceManager);
            var childElement = parentNode.SelectSingleNode(name);

            if (!IsAssigned(childElement))
            {
                childElement = CreateElement(name, RootNameSpaceUri);

                parentNode.AppendChild(childElement);
            }

            return childElement;
        }

        public XmlNode GetNode(string name)
        {
            return GetNode(Root, name);
        }

        [ContractAnnotation("assignedObject:null => false")]
        protected static bool IsAssigned(object assignedObject)
        {
            return BaseItem.IsAssigned(assignedObject);
        }

        [ContractAnnotation("assignedObject:null => false")]
        protected static bool IsEmpty(string assignedObject)
        {
            return BaseItem.IsEmpty(assignedObject);
        }

        [ContractAnnotation("assignedObject:null => false")]
        protected static bool IsNull(string assignedObject)
        {
            return BaseItem.IsNull(assignedObject);
        }
    }
}
