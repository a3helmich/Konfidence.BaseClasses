using System.IO;
using System.Xml;

namespace Konfidence.Base
{
    public class BaseXmlDocument : XmlDocument
    {
        private XmlElement _root;

        private string _fullFileName = string.Empty;
        private string _pathName = string.Empty;
        private string _rootNameSpaceUri;
        private XmlNamespaceManager _xmlNamespaceManager;

        public XmlElement Root => _root;

        public string FileName => _fullFileName;

        public string PathName => _pathName;

        public string RootNameSpaceUri => _rootNameSpaceUri;

        public XmlNamespaceManager XmlNamespaceManager => _xmlNamespaceManager;

        public override void Load(string filename)
        {
            _fullFileName = filename;

            _pathName = Path.GetDirectoryName(filename) + @"\";

            base.Load(filename);

            PostLoad();
        }

        private void PostLoad()
        {
            _root = DocumentElement;

            _rootNameSpaceUri = Root.NamespaceURI;

            // create a shortcut namespace reference
            _xmlNamespaceManager = new XmlNamespaceManager(NameTable);
            _xmlNamespaceManager.AddNamespace("p", _rootNameSpaceUri);
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

            if (valueNode.IsAssigned())
            {
                var attributes = valueNode.Attributes;

                if (attributes.IsAssigned())
                {
                    var attribute = attributes[attributeName];

                    if (attribute.IsAssigned())
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

            if (valueNode.IsAssigned())
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
            var valueNode = node.SelectSingleNode(nodeName);

            value = false;

            if (valueNode.IsAssigned())
            {
                if (bool.TryParse(valueNode.InnerText, out var outvalue))
                {
                    value = outvalue;
                }
            }
        }

        public XmlNode SetValue(XmlNode parentNode, string name, string value)
        {
            var childElement = parentNode.SelectSingleNode(name);

            if (!childElement.IsAssigned())
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

            if (!childElement.IsAssigned())
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
    }
}
