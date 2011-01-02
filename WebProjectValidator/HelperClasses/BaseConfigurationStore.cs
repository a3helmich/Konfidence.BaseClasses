using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Konfidence.Base;

namespace WebProjectValidator.HelperClasses
{
    public class BaseConfigurationStore : BaseItem
    {
        private XmlDocument _ConfigDocument = new XmlDocument();
        private XmlElement _Root;
        private string _ConfigFileName = string.Empty;

        protected BaseConfigurationStore()
        {
            _ConfigFileName = Application.ProductName + ".xml";

            if (!File.Exists(_ConfigFileName))
            {
                _ConfigDocument.InnerXml = "<Config></Config>";
                _ConfigDocument.Save(_ConfigFileName);
            }
            else
            {
                _ConfigDocument.Load(_ConfigFileName);
            }

            _Root = _ConfigDocument.DocumentElement;
        }

        public void Save()
        {
            _ConfigDocument.Save(_ConfigFileName);
        }

        protected void SetProperty(string name, string value)
        {
            XmlNode valueNode = GetNode(name);

            valueNode.InnerText = value;
        }

        private XmlNode GetNode(string name)
        {
            XmlNode valueNode = _Root.SelectSingleNode(name);

            if (!IsAssigned(valueNode))
            {
                valueNode = _ConfigDocument.CreateElement(name);

                _Root.AppendChild(valueNode);
            }

            return valueNode;
        }

        protected string GetNodeValue(string name)
        {
            XmlNode selectedNode = GetNode(name);

            string nodeValue = selectedNode.InnerText;

            if (!IsEmpty(nodeValue))
            {
                return nodeValue;
            }

            return string.Empty;
        }

        protected void GetProperty(string name, out string value)
        {
            value = GetNodeValue(name);
        }

        protected void GetProperty(string name, out int value)
        {
            value = 0;

            int.TryParse(GetNodeValue(name), out value);
        }

        protected void GetProperty(string name, out decimal value)
        {
            value = 0;

            decimal.TryParse(GetNodeValue(name), out value);
        }

        protected void GetProperty(string name, out bool value)
        {
            value = false;

            bool.TryParse(GetNodeValue(name), out value);
        }

        protected void GetProperty(string name, out DateTime value)
        {
            value = DateTime.MinValue;

            DateTime.TryParse(GetNodeValue(name), out value);
        }
    }
}
