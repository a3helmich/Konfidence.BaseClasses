﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Konfidence.Base;

namespace Konfidence.TeamFoundation.ProjectBase
{
    public abstract class ProjectNode : BaseItem 
    {
        private string _ItemGroupName = string.Empty;
        private XmlNode _ItemGroupNode = null;
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

        // - search for an itemgroup node which contains 'itemGroupName' nodes
        private XmlNode GetItemGroupNode()
        {
            XmlNodeList itemGroupList = _TfsXmlDocument.Root.SelectNodes("p:ItemGroup", _TfsXmlDocument.XmlNamespaceManager);

            XmlNode foundItemGroup = null;

            string itemGroupName = _ItemGroupName.ToLower();

            List<XmlNode> 

            foreach (XmlNode itemGroup in itemGroupList)
            {
                if (itemGroup.HasChildNodes)
                {
                    string currentItemGroupName = itemGroup.FirstChild.Name.ToLower();

                    if (itemGroupName.Equals(currentItemGroupName))
                    {
                        if (IsAssigned(foundItemGroup))
                        {
                            processSecondaryItemGroup(foundItemGroup, itemGroup);
                        }
                        else
                        {
                            foundItemGroup = itemGroup;
                        }
                    }
                }
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

            itemGroup.ParentNode.RemoveChild(itemGroup);
        }

        internal protected XmlElement AppendChild()
        {
            XmlElement newElement = _TfsXmlDocument.CreateElement(_ItemGroupName, _TfsXmlDocument.NameSpaceURI);

            _ItemGroupNode.AppendChild(newElement);

            return newElement;
        }
    }
}
