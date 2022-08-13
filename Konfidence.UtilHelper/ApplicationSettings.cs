using System.IO;
using System.Xml;
using Konfidence.Base;

namespace Konfidence.UtilHelper
{
    internal class ApplicationSettings : IApplicationSettings
    {
        private readonly XmlDocument _xmlDocument = new();
        private XmlNodeList? _elementList;
        private string _fileName = string.Empty;

        private readonly string Application;
        internal string RootPath = string.Empty;

        public ApplicationSettings(string application)
        {
            Application = application;
        }

        private XmlNodeList ElementList
        {
            get
            {
                if (!_elementList.IsAssigned())
                {
                    _fileName = RootPath + Application + ".settings";
                    if (File.Exists(_fileName))
                    {
                        _xmlDocument.Load(_fileName);
                    }
                    else
                    {
                        _xmlDocument.LoadXml("<configuration />");
                    }

                    _elementList = _xmlDocument.GetElementsByTagName("configuration");
                }

                return _elementList;
            }
        }

        public string GetStringValue(string keyName)
        {
            foreach (XmlNode xmlNode in ElementList)
            {
                if (xmlNode.Name == "setting")
                {
                    foreach (XmlNode xmlAttributeNode in xmlNode.ChildNodes)
                    {
                        if (xmlAttributeNode.Name == keyName)
                        {
                            return xmlAttributeNode.InnerText;
                        }
                    }
                }
            }

            return string.Empty;
        }

        public void Flush()
        {
            _xmlDocument.Save(_fileName);
        }

        public void SetStringValue(string keyName, string keyValue)
        {
            XmlNode? keyNode = null;

            foreach (XmlNode xmlNode in ElementList)
            {
                if (xmlNode.Name == "setting")
                {
                    foreach (XmlNode xmlAttributeNode in xmlNode.ChildNodes)
                    {
                        if (xmlAttributeNode.Name == keyName)
                        {
                            xmlAttributeNode.InnerText = keyValue;
                            keyNode = xmlAttributeNode;
                            break;
                        }
                    }
                }
            }

            if (!keyNode.IsAssigned())
            {
                keyNode = _xmlDocument.CreateNode(XmlNodeType.Element, keyName, string.Empty);

                keyNode.InnerText = keyValue;

                var root = _xmlDocument.DocumentElement;

                if (root.IsAssigned())
                {
                    root.AppendChild(keyNode);
                }
            }
        }
    }
}
