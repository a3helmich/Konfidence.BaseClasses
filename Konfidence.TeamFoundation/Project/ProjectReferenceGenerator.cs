using System.Xml;
using System.Collections.Generic;

namespace  Konfidence.TeamFoundation.Project
{
    public class ProjectReferenceGenerator: ProjectBaseGenerator
    {
        private bool _Changed = false;

        public bool Changed
        {
            get { return _Changed; }
        }

        private XmlNode _ReferenceItemGroup = null;

        public ProjectReferenceGenerator(ProjectXmlDocument xmlDocument) : base(xmlDocument)
        {
            _ReferenceItemGroup = GetItemGroup("reference");

            Execute();
        }

        private void Execute()
        {
            XmlNodeList referenceNodeList = _ReferenceItemGroup.SelectNodes("p:Reference", XmlNamespaceManager);

            foreach (XmlNode referenceNode in referenceNodeList)
            {
                XmlNode hintPath = referenceNode.SelectSingleNode("p:HintPath", XmlNamespaceManager);

                string hintpathText = hintPath.InnerText;

                if (!hintpathText.StartsWith(@"c:\"))
                {
                    if (hintpathText.Contains(@"\References\"))
                    {
                        int referenceIndex = hintpathText.IndexOf(@"\References\");
                        hintpathText = hintpathText.Substring(referenceIndex);
                        hintpathText = hintpathText.Replace(@"\References\", @"c:\projects\References\");

                        hintPath.InnerText = hintpathText;

                        _Changed = true;
                    }
                }
            }
        }
    }
}
