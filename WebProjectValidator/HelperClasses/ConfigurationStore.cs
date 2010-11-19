using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Konfidence.Base;

namespace WebProjectValidator.HelperClasses
{
    public class ConfigurationStore : BaseItem
    {
        private XmlDocument _ConfigDocument = new XmlDocument();
        private XmlElement _Root;
        private string _ConfigFileName = string.Empty;

        public ConfigurationStore()
        {
            _ConfigFileName = Application.ProductName + ".xml";

            if (!File.Exists(_ConfigFileName))
            {
                _ConfigDocument.InnerXml = "<Config><ProjectName></ProjectName><ProjectFolder></ProjectFolder><rbCSChecked></rbCSChecked></Config>";
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

        public void SetProperty(string name, string value)
        {
            XmlNode valueNode = _Root.SelectSingleNode(name);

            if (IsAssigned(valueNode))
            {
                valueNode.InnerText = value;
            }

        }

        private string GetNodeValue(string name)
        {
            string nodeValue = _Root.SelectSingleNode(name).InnerText;

            if (string.IsNullOrEmpty(nodeValue))
            {
                return string.Empty;
            }

            return nodeValue;
        }

        public void GetProperty(string name, out string value)
        {
            value = GetNodeValue(name);
        }

        public void GetProperty(string name, out int value)
        {
            value = 0;

            int.TryParse(GetNodeValue(name), out value);
        }

        public void GetProperty(string name, out decimal value)
        {
            value = 0;

            decimal.TryParse(GetNodeValue(name), out value);
        }

        public void GetProperty(string name, out bool value)
        {
            value = false;

            bool.TryParse(GetNodeValue(name), out value);
        }

        public void GetProperty(string name, out DateTime value)
        {
            value = DateTime.MinValue;

            DateTime.TryParse(GetNodeValue(name), out value);
        }
    }
}
