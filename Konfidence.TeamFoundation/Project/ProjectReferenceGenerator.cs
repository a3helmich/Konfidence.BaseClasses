using System.Xml;
using System.Collections.Generic;

namespace  Konfidence.TeamFoundation.Project
{
    public class ProjectReferenceGenerator  
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

        public ProjectReferenceGenerator(ProjectXmlDocument projectXmlDocument) 
        {
            string fromBase = @"\References\";
            string toBase = @"c:\projects\References\";

            DllReferenceRebase(fromBase, toBase, projectXmlDocument);
        }

        // TODO : naar ProjectXmlDocument verplaatsen?
        // for each dllRefence that has a relative path, replace that path with an absolute one.
        private void DllReferenceRebase(string fromBase, string toBase, ProjectXmlDocument projectXmlDocument)
        {
            foreach (DllReferenceNode dllReferenceNode in projectXmlDocument.DllReferenceItemGroupList)
            {
                string changeListText = projectXmlDocument.FileName + " - " + dllReferenceNode.HintPath;

                if (dllReferenceNode.ReBaseReference(fromBase, toBase))
                {
                    _ChangeList.Add(changeListText);

                    _Changed = true;
                }
            }
        }
    }
}
