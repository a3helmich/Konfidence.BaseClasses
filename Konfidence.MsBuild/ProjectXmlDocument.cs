using System.IO;
using System.Linq;
using System.Xml.Linq;
using Konfidence.Base;

namespace Konfidence.MsBuild;

internal class ProjectXmlDocument
{
    private const string PropertyGroupName = "PropertyGroup";
    private const string ProjectGuidName = "ProjectGuid";

    private readonly XDocument _projectDocument;

    public string FileName { get; }

    public string ProjectName => Path.GetFileNameWithoutExtension(FileName);

    public string ProjectGuid => ReadProjectGuid();

    private string ReadProjectGuid()
    {
        XElement? projectGuidElement = FindProjectGuidElement();

        return projectGuidElement.IsAssigned() ? projectGuidElement.Value : string.Empty;
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
}