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
            XmlNode referenceItemGroup = GetItemGroup("reference");

            if (IsAssigned(referenceItemGroup))
            {
                Execute(referenceItemGroup);
            }
        }

        // for each refence that has a relative path, replace that path with an absolute one.
        private void Execute(XmlNode referenceItemGroup)
        {
            XmlNodeList referenceNodeList = referenceItemGroup.SelectNodes("p:Reference", XmlNamespaceManager);

            foreach (XmlNode referenceNode in referenceNodeList)
            {
                XmlNode hintPath = referenceNode.SelectSingleNode("p:HintPath", XmlNamespaceManager);

                if (IsAssigned(hintPath))
                {
                    string hintpathText = hintPath.InnerText;

                    if (!hintpathText.StartsWith(@"c:\"))
                    {
                        if (hintpathText.Contains(@"\References\"))
                        {
                            int referenceIndex = hintpathText.IndexOf(@"\References\");

                            hintpathText = hintpathText.Substring(referenceIndex);

                            _ChangeList.Add(XmlDocument.FileName + " - " + hintpathText);

                            hintPath.InnerText = hintpathText.Replace(@"\References\", @"c:\projects\References\");

                            _Changed = true;
                        }
                    }
                }
            }
        }
    }
}
