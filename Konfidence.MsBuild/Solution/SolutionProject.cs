using Microsoft.VisualStudio.SolutionPersistence.Model;

namespace Konfidence.MsBuild.Solution;

public class SolutionProject
{
    public string ProjectFile { get; }

    public string ProjectName { get; }

    public string ProjectGuid { get; }

    internal SolutionProject(SolutionProjectModel project)
    {
        ProjectFile = project.FilePath;

        ProjectName = project.ActualDisplayName;

        ProjectGuid = project.Id.ToString("B").ToUpperInvariant();
    }
}