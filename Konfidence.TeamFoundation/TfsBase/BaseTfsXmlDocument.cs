using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using Konfidence.Base;

namespace Konfidence.TeamFoundation
{
    public class BaseTfsXmlDocument : BaseXmlDocument
    {
        private bool _Changed = false;

        private string _NameSpaceURI;
        private XmlNamespaceManager _XmlNamespaceManager;

        public bool Changed
        {
            get { return _Changed; }
            set { _Changed = value; }
        }

        internal protected string NameSpaceURI 
        {
            get { return _NameSpaceURI; }
        }

        internal protected XmlNamespaceManager XmlNamespaceManager
        {
            get { return _XmlNamespaceManager; }
        }

        public override void Load(string fullFileName)
        {
            base.Load(fullFileName);

            _NameSpaceURI = Root.NamespaceURI;

            // create a shortcut namespace reference
            _XmlNamespaceManager = new XmlNamespaceManager(NameTable);
            _XmlNamespaceManager.AddNamespace("p", _NameSpaceURI);
        }

        protected List<XmlNode> GetItemGroupList(string itemGroupName)
        {
            List<XmlNode> returnList = new List<XmlNode>();
            
            XmlNode itemGroupList = GetItemGroup(itemGroupName);

            if (IsAssigned(itemGroupList))
            {
                foreach (XmlNode selectedNode in itemGroupList.SelectNodes("p:" + itemGroupName, XmlNamespaceManager))
                {
                    returnList.Add(selectedNode);
                }
            }

            return returnList;
        }

        // - search for an itemgroup node which contains 'itemGroupName' nodes
        private XmlNode GetItemGroup(string itemGroupName)
        {
            XmlNodeList itemGroupList = Root.SelectNodes("p:ItemGroup", XmlNamespaceManager);

            XmlNode foundItemGroup = null;

            itemGroupName = itemGroupName.ToLower();

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

        protected void AddElement(XmlElement parentElement, string name, string value)
        {
            XmlElement childElement = CreateElement(name, NameSpaceURI);

            parentElement.AppendChild(childElement);

            childElement.InnerText = value;
        }
    }
}
