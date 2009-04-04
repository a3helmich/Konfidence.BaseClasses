using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.TeamFoundation.Project;


namespace Konfidence.TeamFoundation
{
    class ReferenceReBaser
    {
        private Permissions _tfsPermissions = new Permissions("tfs.konfidence.nl");

        public void RebaseProject(string fileName)
        {
            if (_tfsPermissions.CheckOut(fileName))
            {
                ProjectXmlDocument projectXmlDocument = new ProjectXmlDocument();
                projectXmlDocument.Load(fileName);

                ProjectReferenceGenerator projectReference = new ProjectReferenceGenerator(projectXmlDocument);

                if (projectReference.Changed)
                {
                    projectXmlDocument.Save(fileName);

                    _tfsPermissions.CheckIn(fileName);
                }
                else
                {
                    _tfsPermissions.Undo(fileName);
                }
            }
        }

    }
}
