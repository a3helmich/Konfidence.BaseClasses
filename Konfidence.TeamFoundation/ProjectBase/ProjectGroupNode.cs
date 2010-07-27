using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Konfidence.Base;

namespace Konfidence.TeamFoundation.ProjectBase
{
    public class ProjectGroupNode : BaseItem 
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
        protected ProjectGroupNode()
        {
        }

        public ProjectGroupNode(string itemGroupName, BaseTfsXmlDocument tfsXmlDocument)
        {
            _ItemGroupName = itemGroupName;
            _TfsXmlDocument = tfsXmlDocument;

            _ItemGroupNode = GetItemGroupNode();
        }

        internal XmlNodeList GetItemNodeList()
        {
            return _ItemGroupNode.SelectNodes("p:" + _ItemGroupName, _TfsXmlDocument.XmlNamespaceManager);
        }

        // - search for an itemgroup node which contains 'itemGroupName' nodes
        private XmlNode GetItemGroupNode()
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

            // TODO : only make a group when an item is added! (ie if the group doesn't yet exists)
            //if (!IsAssigned(foundItemGroup))
            //{
            //    foundItemGroup = CreateElement("ItemGroup", NameSpaceURI);

            //    Root.AppendChild(foundItemGroup);
            //}

            return foundItemGroup;
        }

        internal protected XmlElement AppendChild()
        {
            XmlElement newElement = _TfsXmlDocument.CreateElement(_ItemGroupName, _TfsXmlDocument.NameSpaceURI);

            _ItemGroupNode.AppendChild(newElement);

            return newElement;
        }
    }
}
