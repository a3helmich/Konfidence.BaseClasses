using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Konfidence.TeamFoundation.Project
{
    public class DllReferenceNode : BaseTfsXmlNode
    {
        private XmlElement _HintPath = null;
        private XmlNamespaceManager _XmlNamespaceManager;

        public string HintPath
        {
            get 
            {
                if (!IsAssigned(_HintPath))
                {
                    _HintPath = TfsXmlNode.SelectSingleNode("p:HintPath", _XmlNamespaceManager) as XmlElement;
                }

                return _HintPath.InnerText; 
            }
        }

        private void SetHintPath(string hintPath)
        {
            if (!IsAssigned(_HintPath))
            {
                _HintPath = TfsXmlNode.SelectSingleNode("p:HintPath", _XmlNamespaceManager) as XmlElement;
            }

            _HintPath.InnerText = hintPath;
        }

        public DllReferenceNode(XmlNode xmlNode, XmlNamespaceManager xmlNamespaceManager) : base(xmlNode)
        {
            _XmlNamespaceManager = xmlNamespaceManager;
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
