using System.Xml;
using System.Collections.Generic;

namespace  Konfidence.TeamFoundation.Project
{
    public class ProjectXmlEditor  
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

        public ProjectXmlEditor(ProjectXmlDocument projectXmlDocument) 
        {
            string fromBase = @"\References\";
            string toBase = @"c:\projects\References\";

            DllReferenceRebase(projectXmlDocument, fromBase, toBase);
        }

        // TODO : naar ProjectXmlDocument verplaatsen?
        // for each dllRefence that has a relative path, replace that path with an absolute one.
        private void DllReferenceRebase(ProjectXmlDocument projectXmlDocument, string fromBase, string toBase)
        {
            foreach (DllReferenceItemNode dllReferenceNode in projectXmlDocument.DllReferenceItemNodeList)
            {
                if (dllReferenceNode.ReBaseReference(fromBase, toBase))
                {
                    string changeListText = projectXmlDocument.FileName + " - " + dllReferenceNode.HintPath;

                    _ChangeList.Add(changeListText);

                    _Changed = true;
                }
            }
        }
    }
}
