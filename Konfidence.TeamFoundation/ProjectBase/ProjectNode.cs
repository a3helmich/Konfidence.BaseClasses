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
        private string _ItemGroupName = string.Empty;
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
            _ItemGroupName = itemGroupName;
            _TfsXmlDocument = tfsXmlDocument;

            _ItemGroupNode = GetItemGroupNode();
            _PropertyGroupNode = GetPropertyGroupNode();
        }

        public ProjectNode(string itemGroupName, BaseTfsXmlDocument tfsXmlDocument, XmlNode itemGroupNode)
        {
            _ItemGroupName = itemGroupName;
            _TfsXmlDocument = tfsXmlDocument;

            _TfsXmlDocument.Root.AppendChild(itemGroupNode);

            _ItemGroupNode = itemGroupNode;
        }

        internal XmlNodeList GetItemNodeList()
        {
            if (IsAssigned(_ItemGroupNode))
            {
                return _ItemGroupNode.SelectNodes("p:" + _ItemGroupName, _TfsXmlDocument.XmlNamespaceManager);
            }

            return null;
        }

        // the content itemgroup also contains None elements -> iterate thru all childnodes, to determine
        // if this is a itemgroup with content nodes (the content itemgroup seems te be an exception)
        private bool AnyChildContainsItemGroupName(XmlNode itemGroup, string itemGroupName)
        {
            foreach (XmlNode itemNode in itemGroup.ChildNodes)
            {
                if (itemNode.Name.ToLower().Equals(itemGroupName))
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

            XmlNode foundPropertyGroup = null;

            string propertyGroupName = _ItemGroupName.ToLower();

            foreach (XmlNode propertyGroup in PropertyGroupList)
            {
                if (propertyGroup.HasChildNodes)
                {
                    string currentItemGroupName = propertyGroup.FirstChild.Name.ToLower();

                    if (AnyChildContainsItemGroupName(propertyGroup, propertyGroupName))
                    {
                        foundPropertyGroup = propertyGroup;
                    }
                }
            }

            return foundPropertyGroup;
        }

        // - search for an itemgroup node which contains 'itemGroupName' nodes
        private XmlNode GetItemGroupNode()
        {
            XmlNodeList itemGroupList = _TfsXmlDocument.Root.SelectNodes("p:ItemGroup", _TfsXmlDocument.XmlNamespaceManager);

            XmlNode foundItemGroup = null;

            string itemGroupName = _ItemGroupName.ToLower();

            List<XmlNode> movedGroups = new List<XmlNode>();

            foreach (XmlNode itemGroup in itemGroupList)
            {
                if (itemGroup.HasChildNodes)
                {
                    string currentItemGroupName = itemGroup.FirstChild.Name.ToLower();

                    if (AnyChildContainsItemGroupName(itemGroup, itemGroupName))
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
            XmlElement newElement = _TfsXmlDocument.CreateElement(_ItemGroupName, _TfsXmlDocument.NameSpaceURI);

            _ItemGroupNode.AppendChild(newElement);

            return newElement;
        }
    }
}
