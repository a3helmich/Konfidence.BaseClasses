using System.IO;
using System.Xml;
using JetBrains.Annotations;

namespace Konfidence.Base
{
    public class BaseXmlDocument : XmlDocument
    {
        public XmlElement Root { get; private set; }

        public string FileName { get; private set; } = string.Empty;

        public string PathName { get; private set; } = string.Empty;

        public string RootNameSpaceUri { get; private set; }

        public XmlNamespaceManager XmlNamespaceManager { get; private set; }

        public override void Load(string filename)
        {
            FileName = filename;

            PathName = Path.GetDirectoryName(filename) + @"\";

            base.Load(filename);

            PostLoad();
        }

        private void PostLoad()
        {
            Root = DocumentElement;

            if (Root.IsAssigned())
            {
                RootNameSpaceUri = Root.NamespaceURI;

                // create a shortcut namespace reference
                XmlNamespaceManager = new XmlNamespaceManager(NameTable);
                XmlNamespaceManager.AddNamespace("p", RootNameSpaceUri);
            }
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

        [UsedImplicitly]
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

        [UsedImplicitly]
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

        [UsedImplicitly]
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

        [UsedImplicitly]
        public XmlNode GetNode(string name)
        {
            return GetNode(Root, name);
        }

        protected XmlNode AddNode(XmlDocument xmlDocument, string field, string value)
        {
            XmlNode root = xmlDocument.DocumentElement;

            XmlNode newNode = xmlDocument.CreateElement(field);

            if (value.IsAssigned())
            {
                newNode.InnerText = value;
            }

            if (root.IsAssigned())
            {
                root.AppendChild(newNode);
            }

            return newNode;
        }

        [UsedImplicitly]
        protected void AddNode(XmlDocument registrationXml, string field, XmlDocument xmlDocument)
        {
            var subDocumentNode = AddNode(registrationXml, field, string.Empty);

            subDocumentNode.InnerXml = xmlDocument.InnerXml;
        }

        [UsedImplicitly]
        protected void AddNode(XmlDocument registrationXml, XmlDocument xmlDocument)
        {
            XmlNode root = xmlDocument.DocumentElement;

            if (root.IsAssigned())
            {
                var subDocumentNode = AddNode(registrationXml, root.Name, string.Empty);

                subDocumentNode.InnerXml = root.InnerXml;
            }

        }
    }
}
