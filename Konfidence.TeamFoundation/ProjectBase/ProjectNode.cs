using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Konfidence.Base;
using Konfidence.TeamFoundation.Project;

namespace Konfidence.TeamFoundation.ProjectBase
{
    public abstract class ProjectNode : BaseItem 
    {
        private string _GroupName = string.Empty;
        private XmlNode _ItemGroupNode = null;
        private XmlNode _PropertyGroupNode = null;
        private BaseTfsXmlDocument _TfsXmlDocument = null;

        internal XmlNamespaceManager XmlNamespaceManager
        {
            get
            {
                return _TfsXmlDocument.XmlNamespaceManager;
            }
        }

        // to avoid accidential initialation. (ie without parameters)
        protected ProjectNode()
        {
        }

        public ProjectNode(string itemGroupName, BaseTfsXmlDocument tfsXmlDocument)
        {
            _GroupName = itemGroupName;
            _TfsXmlDocument = tfsXmlDocument;

            _ItemGroupNode = GetItemGroupNode();
            _PropertyGroupNode = GetPropertyGroupNode();
        }

        public ProjectNode(string itemGroupName, BaseTfsXmlDocument tfsXmlDocument, XmlNode itemGroupNode, XmlNode propertyGroupNode)
        {
            _GroupName = itemGroupName;
            _TfsXmlDocument = tfsXmlDocument;

            _TfsXmlDocument.Root.AppendChild(itemGroupNode);

            _ItemGroupNode = itemGroupNode;
            _PropertyGroupNode = propertyGroupNode;
        }

        internal XmlNodeList GetNodeList()
        {
            if (IsAssigned(_ItemGroupNode))
            {
                return _ItemGroupNode.SelectNodes("p:" + _GroupName, _TfsXmlDocument.XmlNamespaceManager);
            }

            if (IsAssigned(_PropertyGroupNode))
            {
                return _PropertyGroupNode.ChildNodes;
            }

            return null;
        }

        // the content itemgroup also contains None elements -> iterate thru all childnodes, to determine
        // if this is a itemgroup with content nodes (the content itemgroup seems te be an exception)
        private bool AnyChildContainsGroupName(XmlNode group, string groupName)
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
            XmlNodeList PropertyGroupList = _TfsXmlDocument.Root.SelectNodes("p:PropertyGroup", _TfsXmlDocument.XmlNamespaceManager);

            string propertyGroupName = _GroupName.ToLower();

            foreach (XmlNode propertyGroup in PropertyGroupList)
            {
                if (FindChildNode(propertyGroupName, propertyGroup))
                {
                    return propertyGroup;
                }
            }

            return null;
        }

        private bool FindChildNode(string propertyGroupName, XmlNode propertyGroup)
        {
            if (propertyGroup.HasChildNodes)
            {
                string currentItemGroupName = propertyGroup.FirstChild.Name.ToLower();

                if (AnyChildContainsGroupName(propertyGroup, propertyGroupName))
                {
                    return true;
                }
            }

            return false;
        }

        // - search for an itemgroup node which contains 'itemGroupName' nodes
        private XmlNode GetItemGroupNode()
        {
            XmlNodeList itemGroupList = _TfsXmlDocument.Root.SelectNodes("p:ItemGroup", _TfsXmlDocument.XmlNamespaceManager);

            XmlNode foundItemGroup = null;

            string itemGroupName = _GroupName.ToLower();

            List<XmlNode> movedGroups = new List<XmlNode>();

            foreach (XmlNode itemGroup in itemGroupList)
            {
                if (itemGroup.HasChildNodes)
                {
                    string currentItemGroupName = itemGroup.FirstChild.Name.ToLower();

                    if (AnyChildContainsGroupName(itemGroup, itemGroupName))
                    {
                        if (IsAssigned(foundItemGroup))
                        {
                            processSecondaryItemGroup(foundItemGroup, itemGroup);

                            movedGroups.Add(itemGroup);
                        }
                        else
                        {
                            foundItemGroup = itemGroup;
                        }
                    }
                }
            }

            foreach (XmlNode itemGroup in movedGroups)
            {
                itemGroup.ParentNode.RemoveChild(itemGroup);
            }

            // TODO : only make a group when an item is added! (ie if the group doesn't yet exists)
            //if (!IsAssigned(foundItemGroup))
            //{
            //    foundItemGroup = CreateElement("ItemGroup", NameSpaceURI);

            //    Root.AppendChild(foundItemGroup);
            //}

            return foundItemGroup;
        }

        private void processSecondaryItemGroup(XmlNode foundItemGroup, XmlNode itemGroup)
        {
            List<XmlNode> collectList = new List<XmlNode>();

            foreach (XmlNode itemNode in itemGroup.ChildNodes)
            {
                collectList.Add(itemNode);
            }

            foreach(XmlNode itemNode in collectList)
            {
                foundItemGroup.AppendChild(itemNode);
            }
        }

        internal protected XmlElement AppendChild()
        {
            XmlElement newElement = _TfsXmlDocument.CreateElement(_GroupName, _TfsXmlDocument.NameSpaceURI);

            _ItemGroupNode.AppendChild(newElement);

            return newElement;
        }
    }
}
