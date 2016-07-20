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
        private XmlDocument _Configuration;
        private XmlNode _Root;

        private readonly string _ConfigFileName;

        protected string ConfigFileName
        {
            get { return _ConfigFileName; }
        }

        public BaseApplicationConfiguration(string configFileName)
        {
            _ConfigFileName = configFileName;

            OpenConfiguration();
        }

        private void OpenConfiguration()
        {
            _Root = null;
            _Configuration = new XmlDocument();

            if (File.Exists(_ConfigFileName))
            {
                _Configuration.Load(_ConfigFileName);

                _Root = _Configuration.DocumentElement;
            }

            if (_Root == null)
            {
                _Configuration.LoadXml("<configuration />");

                _Root = _Configuration.DocumentElement;
            }
        }

        public void Save()
        {
            _Configuration.Save(_ConfigFileName);
        }

        protected string GetNodeValue(string name)
        {
            string nodeValue = string.Empty;

            XmlNode xmlNode = _Root.SelectSingleNode(name);

            if (xmlNode != null)
            {
                nodeValue = xmlNode.InnerText;
            }

            return nodeValue;
        }

        protected bool GetBoolNodeValue(string name)
        {
            bool nodeValue = false;

            XmlNode xmlNode = _Root.SelectSingleNode(name);

            if (xmlNode != null)
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
                if (!string.IsNullOrEmpty(joinedArray))
                {
                    var arrayListNodeValue = new ArrayList();

                    string[] splitArray = joinedArray.Split(' ');

                    arrayListNodeValue.AddRange(splitArray);

                    foreach (string byteString in arrayListNodeValue)
                    {
                        arrayByteListNodeValue.Add(Convert.ToByte(byteString));
                    }
                }

                arrayArrayList.Add(arrayByteListNodeValue.ToArray(typeof(byte)) as byte[]);
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
            XmlNode valueNode = _Root.SelectSingleNode(name);

            // Create the node if it doens't exist yet
            if (valueNode == null)
            {
                valueNode = _Configuration.CreateNode(XmlNodeType.Element, name, null);

                _Root.AppendChild(valueNode);
            }

            // remove the node if the assigned value is null or empty
            if (string.IsNullOrEmpty(value)) 
            {
                _Root.RemoveChild(valueNode);
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

                if (stringArray != null)
                {
                    joinedArray = string.Join(" ", stringArray);
                }
            }

            SetNodeValue(name, joinedArray);
        }
    }
}
