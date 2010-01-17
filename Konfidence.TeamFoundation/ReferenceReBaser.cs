using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Konfidence.Base;
using Konfidence.TeamFoundation.Project;


namespace Konfidence.TeamFoundation
{
    public class TfsServerException : Exception
    {
        public TfsServerException(string message) : base(message) { }
    }

    public class ReferenceReBaser : BaseItem
    {
        private TfsPermissions _TfsPermissions = new TfsPermissions("tfs.konfidence.nl");
        private List<string> _OverviewList = new List<string>();

        public List<string> OverviewList
        {
            get { return _OverviewList; }
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

        public void ReBaseProjects(string basePath)
        {
            List<string> projectFileList = new List<string>();

            projectFileList.AddRange(Directory.GetFiles(basePath, "*.csproj", SearchOption.AllDirectories));

            _OverviewList.Clear();

            foreach (string projectFile in projectFileList)
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
                        _OverviewList.AddRange(projectReference.ChangeList);

                        projectXmlDocument.Save(fileName);
                    }
                }
            }
        }
    }
}
