using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Konfidence.Base;
using Konfidence.MsBuild.Solution;
using Microsoft.VisualStudio.SolutionPersistence;
using Microsoft.VisualStudio.SolutionPersistence.Model;
using Microsoft.VisualStudio.SolutionPersistence.Serializer;

namespace Konfidence.MsBuild;

public class SolutionDocument
{
    private const string SolutionExtension = ".sln";

    private const string SolutionXmlExtension = ".slnx";

    private readonly string _solutionFile;

    private readonly ISolutionSerializer _serializer;

    private readonly SolutionModel _solutionModel;

    public int NumberOfSolutionProjects => HasSolutionItem ? Projects.Count + 1 : Projects.Count;

    public SolutionProjectList Projects
    {
        get
        {
            SolutionProjectList projects = [];

            foreach (SolutionProjectModel project in _solutionModel.SolutionProjects)
            {
                projects.Add(new SolutionProject(project));
            }

            return projects;
        }
    }

    public bool HasSolutionItem => _solutionModel.SolutionFolders.Count > 0;

    private SolutionDocument(string solutionFile)
    {
        _solutionFile = solutionFile;

        _serializer = SolutionSerializers.GetSerializerByMoniker(_solutionFile)
                      ?? throw new NotSupportedException($"no solution serializer for '{_solutionFile}'");

        _solutionModel = RunSynchronous(() => _serializer.OpenAsync(_solutionFile, CancellationToken.None));
    }

    public static SolutionDocument GetSolutionDocument(string solutionFile)
    {
        return new SolutionDocument(solutionFile);
    }

    /// <summary>
    /// Resolves the solution file from a directory and a name, trying .sln first and falling back
    /// to .slnx when no .sln file exists. The name may already carry either extension, in which case
    /// it is used as given.
    /// </summary>
    public static SolutionDocument GetSolutionDocument(string solutionDirectory, string solutionName)
    {
        return new SolutionDocument(ResolveSolutionFilePath(solutionDirectory, solutionName));
    }

    /// <summary>
    /// The resolution GetSolutionDocument(directory, name) applies, exposed on its own for a caller
    /// that needs the resolved path without opening the solution.
    /// </summary>
    public static string ResolveSolutionFilePath(string solutionDirectory, string solutionName)
    {
        if (HasSolutionExtension(solutionName))
        {
            return Path.Combine(solutionDirectory, solutionName);
        }

        string slnFilePath = Path.Combine(solutionDirectory, $"{solutionName}{SolutionExtension}");

        return File.Exists(slnFilePath) ? slnFilePath : Path.Combine(solutionDirectory, $"{solutionName}{SolutionXmlExtension}");
    }

    private static bool HasSolutionExtension(string solutionName)
    {
        return solutionName.EndsWith(SolutionExtension, StringComparison.OrdinalIgnoreCase)
               || solutionName.EndsWith(SolutionXmlExtension, StringComparison.OrdinalIgnoreCase);
    }

    public void AddProjectFile(string projectFilePath)
    {
        ProjectXmlDocument projectFile = ProjectXmlDocument.GetProjectXmlDocument(projectFilePath);

        if (!CanAddProjectFile(projectFile))
        {
            return;
        }

        AddProjectEntry(projectFile);
    }

    public void AddSolutionItem(string folderPath, string fileName)
    {
        SolutionFolderModel folder = _solutionModel.FindFolder(folderPath) ?? _solutionModel.AddFolder(folderPath);

        if (ContainsFile(folder, fileName))
        {
            return;
        }

        folder.AddFile(fileName);
    }

    private static bool ContainsFile(SolutionFolderModel folder, string fileName)
    {
        if (!folder.Files.IsAssigned())
        {
            return false;
        }

        foreach (string file in folder.Files)
        {
            if (file.Equals(fileName, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private bool CanAddProjectFile(ProjectXmlDocument projectFile)
    {
        foreach (SolutionProject project in Projects)
        {
            if (IsSameProject(projectFile, project))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsSameProject(ProjectXmlDocument projectFile, SolutionProject project)
    {
        if (projectFile.ProjectGuid.IsAssigned() && projectFile.ProjectGuid.Equals(project.ProjectGuid, StringComparison.InvariantCultureIgnoreCase))
        {
            return true;
        }

        if (projectFile.ProjectName.Equals(project.ProjectName, StringComparison.InvariantCultureIgnoreCase))
        {
            return true;
        }

        return projectFile.FileName.EndsWith(project.ProjectFile, StringComparison.InvariantCultureIgnoreCase);
    }

    private void AddProjectEntry(ProjectXmlDocument projectFile)
    {
        string relativeProjectFileName = projectFile.GetRelativeProjectFileName(SolutionPath);

        SolutionProjectModel project = _solutionModel.AddProject(relativeProjectFileName);

        SetProjectGuid(project, projectFile.ProjectGuid);
    }

    private static void SetProjectGuid(SolutionProjectModel project, string projectGuid)
    {
        if (!projectGuid.IsAssigned())
        {
            return;
        }

        project.Id = Guid.Parse(projectGuid);
    }

    private string SolutionPath
    {
        get
        {
            string solutionPath = Path.GetDirectoryName(_solutionFile) ?? string.Empty;

            if (solutionPath.IsAssigned() && !solutionPath.EndsWith(@"\"))
            {
                solutionPath += @"\";
            }

            return solutionPath;
        }
    }

    public void Save()
    {
        RunSynchronous(() => _serializer.SaveAsync(_solutionFile, _solutionModel, CancellationToken.None));
    }

    private static T RunSynchronous<T>(Func<Task<T>> operation)
    {
        return Task.Run(operation).GetAwaiter().GetResult();
    }

    private static void RunSynchronous(Func<Task> operation)
    {
        Task.Run(operation).GetAwaiter().GetResult();
    }
}
