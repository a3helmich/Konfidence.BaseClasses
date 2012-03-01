using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Konfidence.Base
{
    public class BaseServiceHelper : BaseItem
    {
        protected XmlNode AddNode(XmlDocument xmlDocument, string field, string value)
        {
            XmlNode root = xmlDocument.DocumentElement;

            XmlNode newNode = xmlDocument.CreateElement(field);

            if (!string.IsNullOrEmpty(value))
            {
                newNode.InnerText = value;
            }

            root.AppendChild(newNode);

            return newNode;
        }

        protected void AddNode(XmlDocument registrationXml, string field, XmlDocument xmlDocument)
        {
            XmlNode subDocumentNode = AddNode(registrationXml, field, string.Empty);

            subDocumentNode.InnerXml = xmlDocument.InnerXml;
        }

        protected void AddNode(XmlDocument registrationXml, XmlDocument xmlDocument)
        {
            XmlNode root = xmlDocument.DocumentElement;

            XmlNode subDocumentNode = AddNode(registrationXml, root.Name, string.Empty);

            subDocumentNode.InnerXml = root.InnerXml;
        }
    }
}
