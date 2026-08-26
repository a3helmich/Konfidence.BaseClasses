using System.Collections.Generic;
using System.Xml;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.MsBuild.DocumentBase;

namespace Konfidence.MsBuild.ProjectBase
{
    public abstract class ProjectNode 
    {
        private readonly string _groupName = string.Empty;
        private readonly XmlNode _itemGroupNode;
        private readonly XmlNode _propertyGroupNode;
        private readonly BaseProjectXmlDocument _projectXmlDocument;

        internal XmlNamespaceManager XmlNamespaceManager => _projectXmlDocument.XmlNamespaceManager;

        protected ProjectNode()
        {
        }

        protected ProjectNode(string itemGroupName, BaseProjectXmlDocument projectXmlDocument)
        {
            _groupName = itemGroupName;
            _projectXmlDocument = projectXmlDocument;

            _itemGroupNode = GetItemGroupNode();
            _propertyGroupNode = GetPropertyGroupNode();
        }

        protected ProjectNode(string itemGroupName, BaseProjectXmlDocument projectXmlDocument, [NotNull] XmlNode itemGroupNode, XmlNode propertyGroupNode)
        {
            _groupName = itemGroupName;
            _projectXmlDocument = projectXmlDocument;

            _projectXmlDocument.Root.AppendChild(itemGroupNode);

            _itemGroupNode = itemGroupNode;
            _propertyGroupNode = propertyGroupNode;
        }

        [CanBeNull]
        internal XmlNodeList GetNodeList()
        {
            if (_itemGroupNode.IsAssigned())
            {
                return _itemGroupNode.SelectNodes("p:" + _groupName, _projectXmlDocument.XmlNamespaceManager);
            }

            if (_propertyGroupNode.IsAssigned())
            {
                return _propertyGroupNode.ChildNodes;
            }

            return null;
        }

        // the content itemgroup also contains None elements -> iterate thru all childnodes, to determine
        // if this is a itemgroup with content nodes (the content itemgroup seems te be an exception)
        private static bool AnyChildContainsGroupName([NotNull] XmlNode group, string groupName)
        {
            foreach (XmlNode itemNode in group.ChildNodes)
            {
                if (itemNode.Name.ToLower().Equals(groupName))
                {
                    return true;
                }
            }

            return false;
        }

        // - search for an itemgroup node which contains 'itemGroupName' nodes
        private XmlNode GetPropertyGroupNode()
        {
            var propertyGroupList = _projectXmlDocument.Root.SelectNodes("p:PropertyGroup", _projectXmlDocument.XmlNamespaceManager);

            var propertyGroupName = _groupName.ToLower();

            if (propertyGroupList.IsAssigned())
            {
                foreach (XmlNode propertyGroup in propertyGroupList)
                {
                    if (FindChildNode(propertyGroupName, propertyGroup))
                    {
                        return propertyGroup;
                    }
                }
            }

            return null;
        }

        private bool FindChildNode(string propertyGroupName, [NotNull] XmlNode propertyGroup)
        {
            if (propertyGroup.HasChildNodes)
            {
                //var currentItemGroupName = propertyGroup.FirstChild.Name.ToLower();

                if (AnyChildContainsGroupName(propertyGroup, propertyGroupName))
                {
                    return true;
                }
            }

            return false;
        }

        // - search for an itemgroup node which contains 'itemGroupName' nodes
        [CanBeNull]
        private XmlNode GetItemGroupNode()
        {
            var itemGroupList = _projectXmlDocument.Root.SelectNodes("p:ItemGroup", _projectXmlDocument.XmlNamespaceManager);

            XmlNode foundItemGroup = null;

            var itemGroupName = _groupName.ToLower();

            var movedGroups = new List<XmlNode>();

            if (itemGroupList.IsAssigned())
            {
                foreach (XmlNode itemGroup in itemGroupList)
                {
                    if (itemGroup.HasChildNodes)
                    {
                        //var currentItemGroupName = itemGroup.FirstChild.Name.ToLower();

                        if (AnyChildContainsGroupName(itemGroup, itemGroupName))
                        {
                            if (foundItemGroup.IsAssigned())
                            {
                                ProcessSecondaryItemGroup(foundItemGroup, itemGroup);

                                movedGroups.Add(itemGroup);
                            }
                            else
                            {
                                foundItemGroup = itemGroup;
                            }
                        }
                    }
                }
            }

            foreach (var itemGroup in movedGroups)
            {
                if (itemGroup.IsAssigned() && itemGroup.ParentNode.IsAssigned())
                {
                    itemGroup.ParentNode.RemoveChild(itemGroup);
                }
            }

            return foundItemGroup;
        }

        private void ProcessSecondaryItemGroup(XmlNode foundItemGroup, [NotNull] XmlNode itemGroup)
        {
            var collectList = new List<XmlNode>();

            foreach (XmlNode itemNode in itemGroup.ChildNodes)
            {
                collectList.Add(itemNode);
            }

            foreach(var itemNode in collectList)
            {
                foundItemGroup.AppendChild(itemNode);
            }
        }

        [NotNull]
        protected internal XmlElement AppendChild()
        {
            var newElement = _projectXmlDocument.CreateElement(_groupName, _projectXmlDocument.Root.NamespaceURI);

            _itemGroupNode.AppendChild(newElement);

            return newElement;
        }
    }
}
