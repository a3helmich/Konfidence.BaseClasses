using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Xml;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.UtilHelper
{
    [UsedImplicitly]
    public class BaseApplicationConfiguration
    {
        private XmlDocument? _configuration;
        private XmlNode? _root;

        private readonly string ConfigFileName;

        public BaseApplicationConfiguration(string configFileName)
        {
            ConfigFileName = configFileName;

            OpenConfiguration();
        }

        private void OpenConfiguration()
        {
            _root = null;
            _configuration = new XmlDocument();

            if (File.Exists(ConfigFileName))
            {
                _configuration.Load(ConfigFileName);

                _root = _configuration.DocumentElement;
            }

            if (!_root.IsAssigned())
            {
                _configuration.LoadXml("<configuration />");

                _root = _configuration.DocumentElement;
            }
        }

        [UsedImplicitly]
        public void Save()
        {
            _configuration?.Save(ConfigFileName);
        }

        protected string GetNodeValue(string name)
        {
            string nodeValue = string.Empty;

            XmlNode? xmlNode = _root?.SelectSingleNode(name);

            if (xmlNode.IsAssigned())
            {
                nodeValue = xmlNode.InnerText;
            }

            return nodeValue;
        }

        [UsedImplicitly]
        protected bool GetBoolNodeValue(string name)
        {
            bool nodeValue = false;

            XmlNode? xmlNode = _root?.SelectSingleNode(name);

            if (xmlNode.IsAssigned())
            {
                nodeValue = bool.Parse(xmlNode.InnerText);
            }

            return nodeValue;
        }

        [UsedImplicitly]
        protected ArrayList GetArrayListNodeValue(string name)
        {
            ArrayList arrayArrayList = new();
            ArrayList arrayByteListNodeValue = new();

            string joinedArray = GetNodeValue(name);

            try
            {
                if (joinedArray.IsAssigned())
                {
                    ArrayList arrayListNodeValue = new();

                    string[] splitArray = joinedArray.Split(' ');

                    arrayListNodeValue.AddRange(splitArray);

                    foreach (string byteString in arrayListNodeValue)
                    {
                        arrayByteListNodeValue.Add(Convert.ToByte(byteString));
                    }
                }
                byte[]? arrayByteListNode = arrayByteListNodeValue.ToArray(typeof(byte)) as byte[];

                if (arrayByteListNode.IsAssigned())
                {
                    arrayArrayList.Add(arrayByteListNode);
                }
            }
            catch (Exception)
            {
                 // NOP
            }

            return arrayArrayList;
        }

        protected void SetNodeValue(string name, string value)
        {
            XmlNode? valueNode = _root?.SelectSingleNode(name);

            // Create the node if it doens't exist yet
            if (!valueNode.IsAssigned())
            {
                valueNode = _configuration?.CreateNode(XmlNodeType.Element, name, null);

                if (valueNode.IsAssigned())
                {
                    _root?.AppendChild(valueNode);
                }
            }

            if (valueNode.IsAssigned())
            {
                // remove the node if the assigned value is null or empty
                if (!value.IsAssigned())
                {
                    _root?.RemoveChild(valueNode);
                }
                else
                {
                    valueNode.InnerText = value;
                }
            }
        }

        [UsedImplicitly]
        protected void SetNodeValue(string name, bool value)
        {
            SetNodeValue(name, value.ToString());
        }


        /// <summary>
        /// passwords are not long enough to generate more than 1 byte block for enryption
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        [UsedImplicitly]
        protected void SetNodeValue(string name, ArrayList value)
        {
            string joinedArray = string.Empty;

            if (value.IsAssigned())
            {
                ArrayList stringArrayList = new();

                foreach (byte[] byteArray in value)
                {
                    foreach (byte byteChar in byteArray)
                    {
                        stringArrayList.Add(byteChar.ToString(CultureInfo.InvariantCulture));
                    }
                }

                string[]? stringArray = stringArrayList.ToArray(typeof(string)) as string[];

                if (stringArray.IsAssigned())
                {
                    joinedArray = string.Join(" ", stringArray);
                }
            }

            SetNodeValue(name, joinedArray);
        }
    }
}
