using System.Xml;
using System.Collections.Generic;

namespace  Konfidence.TeamFoundation.Project
{
    public class ProjectReferenceGenerator: ProjectBaseGenerator
    {
        private bool _Changed = false;

        private List<string> _ChangeList = new List<string>();

        #region simple properties
        public bool Changed
        {
            get { return _Changed; }
        }

        public List<string> ChangeList
        {
            get { return _ChangeList; }
        }
        #endregion simple properties

        public ProjectReferenceGenerator(ProjectXmlDocument xmlDocument) : base(xmlDocument)
        {
            ReBase(@"\References\", @"c:\projects\References\");
        }

        public void ReBase(string fromBase, string toBase)
        {
            ExecuteReBase(fromBase, toBase);
        }

        // for each refence that has a relative path, replace that path with an absolute one.
        private void ExecuteReBase(string fromBase, string toBase)
        {
            foreach (XmlNode ddlReferenceNode in projectXmlDocument.DllReferenceItemGroupList)
            {
                if (ReBaseReference(ddlReferenceNode, fromBase, toBase))
                {
                    _Changed = true;
                }
            }
        }

        private bool ReBaseReference(XmlNode dllReferenceNode, string fromBase, string toBase)
        {
            bool changed = false;

            XmlNode hintPath = dllReferenceNode.SelectSingleNode("p:HintPath", XmlNamespaceManager);

            if (IsAssigned(hintPath))
            {
                string hintpathText = hintPath.InnerText;

                if (!hintpathText.StartsWith(toBase))
                {
                    if (hintpathText.Contains(fromBase))
                    {
                        int referenceIndex = hintpathText.IndexOf(fromBase);

                        hintpathText = hintpathText.Substring(referenceIndex);

                        _ChangeList.Add(projectXmlDocument.FileName + " - " + hintpathText);

                        hintPath.InnerText = hintpathText.Replace(fromBase, toBase);

                        changed = true;
                    }
                }
            }

            return changed;
        }
    }
}
