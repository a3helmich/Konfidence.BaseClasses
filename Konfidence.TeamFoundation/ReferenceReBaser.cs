using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.TeamFoundation.Project;
using System.Xml;
using System.IO;
using Konfidence.Base;


namespace Konfidence.TeamFoundation
{
    public class ReferenceReBaser : BaseItem
    {
        private TfsPermissions _TfsPermissions = new TfsPermissions("tfs.konfidence.nl");

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
                        projectXmlDocument.Save(fileName);
                    }
                }
            }
        }
    }
}
