using System.IO;
using System.Xml;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.DesignPatterns.Singleton;

namespace Konfidence.UtilHelper
{
    /// <summary>
    /// Summary description for ApplicationSettings.
    /// </summary>
    /// 

    public interface IApplicationSettings : ISingleton
    {
        string GetStringValue(string keyName);
        void SetStringValue(string keyName, string keyValue);
        void Flush();
    }

    public sealed class ApplicationSettingsFactory : SingletonFactory
    {
        private static string _rootPath = string.Empty;

        [UsedImplicitly]
        public static IApplicationSettings ApplicationSettings(string application, string rootPath)
        {
            _rootPath = rootPath;
            if (!_rootPath.EndsWith(@"\"))
                _rootPath += @"\";
            if (!_rootPath.EndsWith(@"settings\"))
                _rootPath += @"settings\";
            return ApplicationSettings(application);
        }

        public static IApplicationSettings ApplicationSettings(string application)
        {
            var applicationSettings = GetInstance(typeof(ApplicationSettings)) as ApplicationSettings;

            if (applicationSettings.IsAssigned())
            {
                applicationSettings.Application = application;
                applicationSettings.RootPath = _rootPath;

                return applicationSettings;
            }

            return null;
        }

        private ApplicationSettingsFactory()
        {
        }
    }

    internal class ApplicationSettings : IApplicationSettings
    {
        private readonly XmlDocument _xmlDocument = new XmlDocument();
        private XmlNodeList _elementList;
        private string _fileName = string.Empty;

        internal string Application;
        internal string RootPath = string.Empty;

        private XmlNodeList ElementList
        {
            get
            {
                if (!_elementList.IsAssigned())
                {
                    _fileName = RootPath + Application + ".settings";
                    if (File.Exists(_fileName))
                        _xmlDocument.Load(_fileName);
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
            XmlNode keyNode = null;

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
