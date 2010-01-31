﻿using System.Xml;
using System.Collections.Generic;

namespace Konfidence.TeamFoundation.Project
{
    public class ProjectXmlDocument : BaseTfsXmlDocument
    {
        private const string DLL_REFERENCE_ITEMGROUP_NAME = "Reference";

        private List<DllReferenceNode> _DllReferenceItemGroupList = null;

        // TODO : when a DllReferenceNode is added to the Xml it must also be added to the list

        public List<DllReferenceNode> DllReferenceItemGroupList
        {
            get
            {
                if (!IsAssigned(_DllReferenceItemGroupList))
                {
                    _DllReferenceItemGroupList = new List<DllReferenceNode>();

                    foreach (XmlNode dllReference in GetItemGroupList(DLL_REFERENCE_ITEMGROUP_NAME))
                    {
                        _DllReferenceItemGroupList.Add(new DllReferenceNode(dllReference, this.XmlNamespaceManager));
                    }

                }
                return _DllReferenceItemGroupList;
            }
        }
    }
}
