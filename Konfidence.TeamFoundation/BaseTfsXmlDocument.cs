using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Konfidence.Base;

namespace Konfidence.TeamFoundation
{
    public class BaseTfsXmlDocument : XmlDocument
    {
        private bool _Changed = false;
        private string _FullFileName = string.Empty;

        private XmlElement _Root;
        private string _NameSpaceURI;
        private XmlNamespaceManager _XmlNamespaceManager;

        public bool Changed
        {
            get { return _Changed; }
            set { _Changed = value; }
        }

        public string FileName // TODO : moet protected worden
        {
            get { return _FullFileName; }
        }

        protected XmlElement Root
        {
            get { return _Root; }
        }

        protected string NameSpaceURI 
        {
            get { return _NameSpaceURI; }
        }

        private XmlNamespaceManager XmlNamespaceManager
        {
            get { return _XmlNamespaceManager; }
        }

        public override void Load(string fullFileName)
        {

            _FullFileName = fullFileName;

            base.Load(_FullFileName);

            _Root = DocumentElement;
            _NameSpaceURI = _Root.NamespaceURI;

            // create a shortcut namespace reference
            _XmlNamespaceManager = new XmlNamespaceManager(NameTable);
            _XmlNamespaceManager.AddNamespace("p", _NameSpaceURI);
        }

        protected static bool IsAssigned(object assignedObject)
        {
            return BaseItem.IsAssigned(assignedObject);
        }

        protected XmlNodeList GetItemGroupList(string itemGroupName)
        {
            XmlNode itemGroupList = GetItemGroup(itemGroupName);

            return itemGroupList.SelectNodes("p:" + itemGroupName, XmlNamespaceManager);
        }

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
    }
}
