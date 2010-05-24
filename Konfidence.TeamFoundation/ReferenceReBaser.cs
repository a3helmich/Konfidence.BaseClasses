using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Konfidence.Base;
using Konfidence.TeamFoundation.Project;
using Konfidence.TeamFoundation.Exceptions;
using Konfidence.TeamFoundation;

namespace Konfidence.TeamFoundation
{
    public class ReferenceReBaser : BaseItem
    {
        private TfsPermissions _TfsPermissions = new TfsPermissions("tfs.konfidence.nl");
        private List<string> _RebasedProjectList = new List<string>();

        public List<string> RebasedProjectList
        {
            get { return _RebasedProjectList; }
        }

        public ReferenceReBaser()
        {
            // if the Team Foundation Server differs from the konfidence server. get this servername from 'ProjectReBaser.Config.xml'
            string configFileName = @"\Projects\References\Config\ProjectReBaser.Config.xml";

            if (File.Exists(configFileName))
            {
                XmlDocument configDocument = new XmlDocument();

                configDocument.Load(configFileName);

                XmlElement root = configDocument.DocumentElement;

                XmlNode tfsServerName = root.SelectSingleNode("tfsServerName");

                if (IsAssigned(tfsServerName))
                {
                    _TfsPermissions = new TfsPermissions(tfsServerName.InnerText);
                }
            }
        }

        public void ReBaseProjects(string solutionPath)
        {
            _RebasedProjectList.Clear();

            SolutionXmlDocument solutionXmlDocument = new SolutionXmlDocument(solutionPath);

            foreach (string projectFile in solutionXmlDocument.GetProjectFileList())
            {
                RebaseProject(projectFile);
            }
        }

        private TfsCheckOut GetTfsCheckOut(string fileName)
        {
            TfsCheckOut tfsCheckOut = null;

            if (IsAssigned(_TfsPermissions))
            {
                tfsCheckOut = new TfsCheckOut(_TfsPermissions, fileName);
            }
            else
            {
                throw new TfsServerException("server permission voor '" + _TfsPermissions.TfsServer + "' kan niet worden aangemaakt."); 
            }

            return tfsCheckOut;
        }

        private void RebaseProject(string fileName)
        {
            using (TfsCheckOut tfsCheckOut = GetTfsCheckOut(fileName))
            {
                if (tfsCheckOut.IsValid)
                {
                    ProjectXmlDocument projectXmlDocument = new ProjectXmlDocument();
                    projectXmlDocument.Load(fileName);

                    ProjectReferenceGenerator projectReference = new ProjectReferenceGenerator(projectXmlDocument);

                    if (projectReference.Changed)
                    {
                        _RebasedProjectList.AddRange(projectReference.ChangeList);

                        projectXmlDocument.Save(fileName);
                    }
                }
                else
                {
                    _RebasedProjectList.Add(fileName + " - Failed (allready checked out?)");
                }
            }
        }
    }
}
