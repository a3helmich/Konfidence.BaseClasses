using System.Xml;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.MsBuild.ProjectBase;

namespace Konfidence.MsBuild.Project
{
    public class DllReferenceItemNode : ProjectItemNode
    {
        private XmlElement _hintPath;
        private readonly XmlNamespaceManager _xmlNamespaceManager;

        #region readOnlyProperties
        [NotNull]
        public string HintPath
        {
            get 
            {
                if (!_hintPath.IsAssigned())
                {
                    _hintPath = projectXmlNode.SelectSingleNode("p:HintPath", _xmlNamespaceManager) as XmlElement;
                }

                if (_hintPath.IsAssigned())
                {
                    return _hintPath.InnerText;
                }

                return string.Empty;
            }
        }
        #endregion readOnlyProperties

        public DllReferenceItemNode(XmlNode xmlNode, XmlNamespaceManager xmlNamespaceManager) : base(xmlNode)
        {
            _xmlNamespaceManager = xmlNamespaceManager;
        }
        
        private void SetHintPath(string hintPath)
        {
            if (!_hintPath.IsAssigned())
            {
                _hintPath = projectXmlNode.SelectSingleNode("p:HintPath", _xmlNamespaceManager) as XmlElement;
            }

            if (_hintPath.IsAssigned())
            {
                _hintPath.InnerText = hintPath;
            }
        }

        public bool ReBaseReference(string fromBase, string toBase)
        {
            var changed = false;
            var hintPath = HintPath;

            if (!string.IsNullOrEmpty(hintPath))
            {
                if (!hintPath.StartsWith(toBase))
                {
                    if (hintPath.Contains(fromBase))
                    {
                        var referenceIndex = hintPath.IndexOf(fromBase, System.StringComparison.InvariantCultureIgnoreCase);

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
