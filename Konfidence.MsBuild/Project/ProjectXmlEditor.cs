using System.Collections.Generic;
using JetBrains.Annotations;

namespace  Konfidence.MsBuild.Project
{
    public class ProjectXmlEditor  
    {
        private bool _changed;

        private readonly List<string> _changeList = new List<string>();

        #region simple properties
        public bool Changed => _changed;

        public List<string> ChangeList => _changeList;

        #endregion simple properties

        public ProjectXmlEditor(ProjectXmlDocument projectXmlDocument)
        {
            _changed = false;

            const string fromBase = @"\References\";
            const string toBase = @"c:\projects\References\";

            DllReferenceRebase(projectXmlDocument, fromBase, toBase);
        }

        // TODO : naar ProjectXmlDocument verplaatsen?
        // for each dllRefence that has a relative path, replace that path with an absolute one.
        private void DllReferenceRebase([NotNull] ProjectXmlDocument projectXmlDocument, string fromBase, string toBase)
        {
            foreach (var dllReferenceNode in projectXmlDocument.DllReferenceItemNodeList)
            {
                if (dllReferenceNode.ReBaseReference(fromBase, toBase))
                {
                    var changeListText = projectXmlDocument.FileName + " - " + dllReferenceNode.HintPath;

                    _changeList.Add(changeListText);

                    _changed = true;
                }
            }
        }
    }
}
