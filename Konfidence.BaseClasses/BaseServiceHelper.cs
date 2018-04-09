using System.Xml;

namespace Konfidence.Base
{
    public class BaseServiceHelper : BaseItem
    {
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

        protected void AddNode(XmlDocument registrationXml, string field, XmlDocument xmlDocument)
        {
            var subDocumentNode = AddNode(registrationXml, field, string.Empty);

            subDocumentNode.InnerXml = xmlDocument.InnerXml;
        }

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
