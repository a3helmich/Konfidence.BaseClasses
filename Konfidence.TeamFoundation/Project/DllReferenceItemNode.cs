﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Konfidence.TeamFoundation.ProjectBase;

namespace Konfidence.TeamFoundation.Project
{
    public class DllReferenceItemNode : ProjectItemNode
    {
        private XmlElement _HintPath = null;
        private XmlNamespaceManager _XmlNamespaceManager;

        #region readOnlyProperties
        public string HintPath
        {
            get 
            {
                if (!IsAssigned(_HintPath))
                {
                    _HintPath = TfsXmlNode.SelectSingleNode("p:HintPath", _XmlNamespaceManager) as XmlElement;
                }

                if (IsAssigned(_HintPath))
                {
                    return _HintPath.InnerText;
                }

                return string.Empty;
            }
        }
        #endregion readOnlyProperties

        public DllReferenceItemNode(XmlNode xmlNode, XmlNamespaceManager xmlNamespaceManager) : base(xmlNode)
        {
            _XmlNamespaceManager = xmlNamespaceManager;
        }
        
        private void SetHintPath(string hintPath)
        {
            if (!IsAssigned(_HintPath))
            {
                _HintPath = TfsXmlNode.SelectSingleNode("p:HintPath", _XmlNamespaceManager) as XmlElement;
            }

            _HintPath.InnerText = hintPath;
        }

        public bool ReBaseReference(string fromBase, string toBase)
        {
            bool changed = false;
            string hintPath = this.HintPath;

            if (!string.IsNullOrEmpty(hintPath))
            {
                if (!hintPath.StartsWith(toBase))
                {
                    if (hintPath.Contains(fromBase))
                    {
                        int referenceIndex = hintPath.IndexOf(fromBase);

                        hintPath = hintPath.Substring(referenceIndex);

                        SetHintPath(hintPath.Replace(fromBase, toBase));

                        changed = true;
                    }
                }
            }

            return changed;
        }
    }
}