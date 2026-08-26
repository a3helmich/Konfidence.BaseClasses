using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Konfidence.Base;

namespace Konfidence.MsBuild
{
    public class ProjectXmlDocument
    {
        private const string PropertyGroupName = "PropertyGroup";
        private const string ProjectGuidName = "ProjectGuid";

        private readonly XDocument _projectDocument;

        public bool Changed { get; set; }

        public string FileName { get; }

        public string ProjectName => Path.GetFileNameWithoutExtension(FileName);

        public string ProjectGuid
        {
            get => ReadProjectGuid();
            set => WriteProjectGuid(value);
        }

        private string ReadProjectGuid()
        {
            XElement? projectGuidElement = FindProjectGuidElement();

            return projectGuidElement.IsAssigned() ? projectGuidElement.Value : string.Empty;
        }

        private void WriteProjectGuid(string projectGuid)
        {
            XElement? projectGuidElement = FindProjectGuidElement();

            if (projectGuidElement.IsAssigned() && projectGuid.IsGuid())
            {
                projectGuidElement.Value = projectGuid;
            }
        }

        private XElement? FindProjectGuidElement()
        {
            return _projectDocument.Root?
                .Elements().Where(element => element.Name.LocalName.Equals(PropertyGroupName))
                .Elements().FirstOrDefault(element => element.Name.LocalName.Equals(ProjectGuidName));
        }

        public string GetRelativeProjectFileName(string basePath)
        {
            if (!basePath.IsAssigned())
            {
                return Path.GetFileName(FileName);
            }

            return Path.GetRelativePath(basePath, FileName);
        }

        private ProjectXmlDocument(string projectFile)
        {
            FileName = projectFile;

            _projectDocument = XDocument.Load(projectFile, LoadOptions.PreserveWhitespace);
        }

        public static ProjectXmlDocument GetProjectXmlDocument(string projectFile)
        {
            return new ProjectXmlDocument(projectFile);
        }

        public void Save(string fileName)
        {
            using (XmlWriter projectWriter = XmlWriter.Create(fileName, ProjectWriterSettings()))
            {
                _projectDocument.Save(projectWriter);
            }
        }

        private XmlWriterSettings ProjectWriterSettings()
        {
            bool hasDeclaration = _projectDocument.Declaration.IsAssigned();

            return new XmlWriterSettings
            {
                OmitXmlDeclaration = !hasDeclaration,
                Encoding = new UTF8Encoding(hasDeclaration)
            };
        }
    }
}
