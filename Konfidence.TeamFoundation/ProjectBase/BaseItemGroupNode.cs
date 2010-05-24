using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Konfidence.Base;

namespace Konfidence.TeamFoundation.ProjectBase
{
    public class BaseItemGroupNode<BaseItemNode> : BaseItem
    {
        private string _ItemGroupName = string.Empty;
        private XmlNode _ItemGroupNode;
        private BaseTfsXmlDocument _TfsXmlDocument;

        private List<BaseItemNode> _ItemList;

        public List<BaseItemNode> ItemList
        {
            get
            {
                if (!IsAssigned(_ItemList))
                {
                    _ItemList = new List<BaseItemNode>();

                    foreach (BaseItemNode dllReference in GetItemGroupList())
                    {
                        _ItemList.Add(BaseItemNode.ge(dllReference, _TfsXmlDocument.XmlNamespaceManager));
                    }

                }
                return _ItemList;
            }
        } 

        protected BaseItemGroupNode()
        {
        }

        public BaseItemGroupNode(string itemGroupName, BaseTfsXmlDocument tfsXmlDocument)
        {
            _ItemGroupName = itemGroupName;
            _TfsXmlDocument = tfsXmlDocument;

            _ItemGroupNode = GetItemGroup();
        }

        private XmlNodeList GetItemGroupList()
        {
            return _ItemGroupNode.SelectNodes("p:" + _ItemGroupName, _TfsXmlDocument.XmlNamespaceManager);
        }

        // - search for an itemgroup node which contains 'itemGroupName' nodes
        private XmlNode GetItemGroup()
        {
            XmlNodeList itemGroupList = _TfsXmlDocument.Root.SelectNodes("p:ItemGroup", _TfsXmlDocument.XmlNamespaceManager);

            XmlNode foundItemGroup = null;

            string itemGroupName = _ItemGroupName.ToLower();

            foreach (XmlNode itemGroup in itemGroupList)
            {
                if (itemGroup.HasChildNodes)
                {
                    string currentItemGroupName = itemGroup.FirstChild.Name.ToLower();

                    if (itemGroupName.Equals(currentItemGroupName))
                    {
                        foundItemGroup = itemGroup;

                        break;
                    }
                }
            }

            // TODO : only make a group when an item is added!
            //if (!IsAssigned(foundItemGroup))
            //{
            //    foundItemGroup = CreateElement("ItemGroup", NameSpaceURI);

            //    Root.AppendChild(foundItemGroup);
            //}

            return foundItemGroup;
        }
    }
}
