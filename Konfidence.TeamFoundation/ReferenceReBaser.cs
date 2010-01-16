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
        private Permissions _tfsPermissions = new Permissions("tfs.konfidence.nl");

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
                    _tfsPermissions = new Permissions(tfsServerName.InnerText);
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

        private void RebaseProject(string fileName)
        {
            if (IsAssigned(_tfsPermissions))
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
}
