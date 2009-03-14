using System.IO;
using System.Xml;
using Konfidence.DesignPatterns;

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

    sealed public class ApplicationSettingsFactory : SingletonFactory
    {
        static private string _RootPath = string.Empty;

        static public IApplicationSettings ApplicationSettings(string application, string rootPath)
        {
            _RootPath = rootPath;
            if (!_RootPath.EndsWith(@"\"))
                _RootPath += @"\";
            if (!_RootPath.EndsWith(@"settings\"))
                _RootPath += @"settings\";
            return ApplicationSettings(application);
        }

        static public IApplicationSettings ApplicationSettings(string application)
        {
            IApplicationSettings applicationSettings = Instance(typeof(ApplicationSettings)) as IApplicationSettings;

            (applicationSettings as ApplicationSettings)._Application = application;
            (applicationSettings as ApplicationSettings)._RootPath = _RootPath;

            return applicationSettings;
        }

        private ApplicationSettingsFactory()
        {
        }
    }

    internal class ApplicationSettings : IApplicationSettings
    {
        private XmlDocument _XmlDocument = new XmlDocument();
        private XmlNodeList _ElementList;
        private string _FileName = string.Empty;

        internal string _Application;
        internal string _RootPath = string.Empty;

        private XmlNodeList ElementList
        {
            get
            {
                if (_ElementList == null)
                {
                    _FileName = _RootPath + _Application + ".settings";
                    if (File.Exists(_FileName))
                        _XmlDocument.Load(_FileName);
                    else
                    {
                        _XmlDocument.LoadXml("<configuration />");
                    }

                    _ElementList = _XmlDocument.GetElementsByTagName("configuration");
                }

                return _ElementList;
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
            _XmlDocument.Save(_FileName);
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

            if (keyNode == null)
            {
                keyNode = _XmlDocument.CreateNode(XmlNodeType.Element, keyName, string.Empty);

                keyNode.InnerText = keyValue;

                XmlElement root = _XmlDocument.DocumentElement;
                root.AppendChild(keyNode);
            }
        }
    }
}
