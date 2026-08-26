using System.Collections.Generic;
using System.Xml;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.UtilHelper;

namespace Konfidence.MsBuild.DocumentBase
{
    public class BaseProjectXmlDocument : BaseXmlDocument
    {
        public bool Changed { get; set; }

        public override void Load(string fullFileName)
        {
            Changed = false;

            base.Load(fullFileName);
        }

        [NotNull]
        [UsedImplicitly]
        protected List<XmlNode> GetItemGroupList(string itemGroupName)
        {
            var returnList = new List<XmlNode>();
            
            var itemGroupList = GetItemGroup(itemGroupName);

            if (itemGroupList.IsAssigned())
            {
                var xmlNodeList = itemGroupList.SelectNodes("p:" + itemGroupName, XmlNamespaceManager);

                if (xmlNodeList.IsAssigned())
                {
                    foreach (XmlNode selectedNode in xmlNodeList)
                    {
                        returnList.Add(selectedNode);
                    }
                }
            }

            return returnList;
        }

        // - search for an itemgroup node which contains 'itemGroupName' nodes
        [CanBeNull]
        private XmlNode GetItemGroup(string itemGroupName)
        {
            var itemGroupList = Root.SelectNodes("p:ItemGroup", XmlNamespaceManager);

            XmlNode foundItemGroup = null;

            itemGroupName = itemGroupName.ToLower();

            if (itemGroupList != null)
            {
                foreach (XmlNode itemGroup in itemGroupList)
                {
                    if (itemGroup.HasChildNodes)
                    {
                        var currentItemGroupName = itemGroup.FirstChild.Name.ToLower();

                        if (itemGroupName.Equals(currentItemGroupName))
                        {
                            foundItemGroup = itemGroup;

                            break;
                        }
                    }
                }
            }

            return foundItemGroup;
        }
    }
}
