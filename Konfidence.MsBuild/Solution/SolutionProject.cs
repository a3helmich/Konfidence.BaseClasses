using JetBrains.Annotations;
using Microsoft.VisualStudio.SolutionPersistence.Model;

namespace Konfidence.MsBuild.Solution
{
    public class SolutionProject
    {
        [NotNull]
        public string ProjectFile { get; }

        [NotNull]
        public string ProjectName { get; }

        [NotNull]
        public string ProjectGuid { get; }

        internal SolutionProject([NotNull] SolutionProjectModel project)
        {
            ProjectFile = project.FilePath;

            ProjectName = project.ActualDisplayName;

            ProjectGuid = project.Id.ToString("B").ToUpperInvariant();
        }
    }
}
