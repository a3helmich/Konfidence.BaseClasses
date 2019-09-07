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
        public void GetAttributeValue([NotNull] string nodeName, string attributeName, out string value)
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

        public void GetValue([NotNull] XmlNode node,[NotNull] string nodeName, [NotNull] out string value)
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

        public void GetValue([NotNull] XmlNode node,[NotNull] string nodeName, out bool value)
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

        [NotNull]
        public XmlNode SetValue([NotNull] XmlNode parentNode, [NotNull] string name, [NotNull] string value)
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
        [NotNull]
        public XmlNode SetValue([NotNull] string name, [NotNull] string value)
        {
            return SetValue(Root, name, value);
        }

        [NotNull]
        public XmlNode GetNode([NotNull] XmlNode parentNode, [NotNull] string name)
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
        [NotNull]
        public XmlNode GetNode([NotNull] string name)
        {
            return GetNode(Root, name);
        }

        [NotNull]
        protected XmlNode AddNode([NotNull] XmlDocument xmlDocument, [NotNull] string field, string value)
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
        protected void AddNode([NotNull] XmlDocument registrationXml, [NotNull] string field, [NotNull] XmlDocument xmlDocument)
        {
            var subDocumentNode = AddNode(registrationXml, field, string.Empty);

            subDocumentNode.InnerXml = xmlDocument.InnerXml;
        }

        [UsedImplicitly]
        protected void AddNode(XmlDocument registrationXml, [NotNull] XmlDocument xmlDocument)
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
