using System.Xml;
using Konfidence.Base;

namespace Konfidence.TeamFoundation.Project
{
    public class ProjectBaseGenerator: BaseItem
    {
        private ProjectXmlDocument _XmlDocument;
        private XmlElement _Root;

        private string _NameSpaceURI;
        private XmlNamespaceManager _XmlNamespaceManager;

        #region properties
        public ProjectXmlDocument projectXmlDocument
        {
            get { return _XmlDocument; }
        }

        public XmlElement Root
        {
            get { return _Root; }
        }

        public XmlNamespaceManager XmlNamespaceManager
        {
            get { return _XmlNamespaceManager; }
        }
        #endregion properties

        public ProjectBaseGenerator(ProjectXmlDocument xmlDocument)
        {
            _XmlDocument = xmlDocument;

            _Root = _XmlDocument.DocumentElement;
            _NameSpaceURI = _Root.NamespaceURI;

            // create a shortcut namespace reference
            _XmlNamespaceManager = new XmlNamespaceManager(_XmlDocument.NameTable);
            _XmlNamespaceManager.AddNamespace("p", _NameSpaceURI);
        }

        // - search for an itemgroup node which contains Reference nodes
        // - if there is not an itemgroup, create one, to contain newly created references
        //protected XmlNode GetItemGroup(string itemGroupName)
        //{
        //    XmlNodeList itemGroupList = _Root.SelectNodes("p:ItemGroup", _XmlNamespaceManager);

        //    XmlNode foundItemGroup = null;

        //    itemGroupName = itemGroupName.ToLower();

        //    foreach (XmlNode itemGroup in itemGroupList)
        //    {
        //        string currentItemGroupName = itemGroup.FirstChild.Name.ToLower();

        //        if (itemGroupName.Equals(currentItemGroupName))
        //        {
        //            foundItemGroup = itemGroup;

        //            break;
        //        }
        //    }

        //    if (!IsAssigned(foundItemGroup))
        //    {
        //        _Root.AppendChild(_XmlDocument.CreateElement("ItemGroup", _NameSpaceURI));
        //    }

        //    return foundItemGroup;
        //}

        //protected XmlElement CreateElement(string elementName)
        //{
        //    return projectXmlDocument.CreateElement(elementName, _NameSpaceURI);
        //}
    }
}
