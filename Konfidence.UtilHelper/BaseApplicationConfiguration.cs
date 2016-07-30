using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Xml;
using Konfidence.Base;

namespace Konfidence.UtilHelper
{
    public class BaseApplicationConfiguration: BaseItem
    {
        private XmlDocument _configuration;
        private XmlNode _root;

        private readonly string _configFileName;

        protected string ConfigFileName => _configFileName;

        public BaseApplicationConfiguration(string configFileName)
        {
            _configFileName = configFileName;

            OpenConfiguration();
        }

        private void OpenConfiguration()
        {
            _root = null;
            _configuration = new XmlDocument();

            if (File.Exists(_configFileName))
            {
                _configuration.Load(_configFileName);

                _root = _configuration.DocumentElement;
            }

            if (!_root.IsAssigned())
            {
                _configuration.LoadXml("<configuration />");

                _root = _configuration.DocumentElement;
            }
        }

        public void Save()
        {
            _configuration.Save(_configFileName);
        }

        protected string GetNodeValue(string name)
        {
            string nodeValue = string.Empty;

            XmlNode xmlNode = _root.SelectSingleNode(name);

            if (xmlNode.IsAssigned())
            {
                nodeValue = xmlNode.InnerText;
            }

            return nodeValue;
        }

        protected bool GetBoolNodeValue(string name)
        {
            bool nodeValue = false;

            XmlNode xmlNode = _root.SelectSingleNode(name);

            if (xmlNode.IsAssigned())
            {
                nodeValue = bool.Parse(xmlNode.InnerText);
            }

            return nodeValue;
        }

        protected ArrayList GetArrayListNodeValue(string name)
        {
            var arrayArrayList = new ArrayList();
            var arrayByteListNodeValue = new ArrayList();

            string joinedArray = GetNodeValue(name);

            try
            {
                if (joinedArray.IsAssigned())
                {
                    var arrayListNodeValue = new ArrayList();

                    string[] splitArray = joinedArray.Split(' ');

                    arrayListNodeValue.AddRange(splitArray);

                    foreach (string byteString in arrayListNodeValue)
                    {
                        arrayByteListNodeValue.Add(Convert.ToByte(byteString));
                    }
                }
                var arrayByteListNode = arrayByteListNodeValue.ToArray(typeof(byte)) as byte[];

                if (arrayByteListNode.IsAssigned())
                {
                    arrayArrayList.Add(arrayByteListNode);
                }
            }
            catch (Exception)
            {
                // hier kom je o.a. terecht als de arraystring geen arraystring is.
                // op dit moment gebeurt dit als de password string verkeerd wordt aangepast.
                // ff niks
            }

            return arrayArrayList;
        }

        protected void SetNodeValue(string name, string value)
        {
            XmlNode valueNode = _root.SelectSingleNode(name);

            // Create the node if it doens't exist yet
            if (!valueNode.IsAssigned())
            {
                valueNode = _configuration.CreateNode(XmlNodeType.Element, name, null);

                _root.AppendChild(valueNode);
            }

            // remove the node if the assigned value is null or empty
            if (!value.IsAssigned())
            {
                _root.RemoveChild(valueNode);
            }
            else
            {
                valueNode.InnerText = value;
            }
        }

        protected void SetNodeValue(string name, bool value)
        {
            SetNodeValue(name, value.ToString());
        }


        /// <summary>
        /// passwords are not long enough to generate more than 1 byte block for enryption
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void SetNodeValue(string name, ArrayList value)
        {
            string joinedArray = string.Empty;

            if (value.IsAssigned())
            {
                var stringArrayList = new ArrayList();

                foreach (byte[] byteArray in value)
                {
                    foreach (byte byteChar in byteArray)
                    {
                        stringArrayList.Add(byteChar.ToString(CultureInfo.InvariantCulture));
                    }
                }

                var stringArray = stringArrayList.ToArray(typeof(string)) as string[];

                if (stringArray.IsAssigned())
                {
                    joinedArray = string.Join(" ", stringArray);
                }
            }

            SetNodeValue(name, joinedArray);
        }
    }
}
