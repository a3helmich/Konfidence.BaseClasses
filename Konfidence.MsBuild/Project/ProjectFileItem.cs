using Konfidence.Base;

namespace Konfidence.MsBuild.Project
{
    // TODO : moet internal worde
    public class ProjectFileItem
    {
        public string FileName { get; }

        public string Action { get; }

        public ProjectFileItem(string fileName, string action)
        {
            FileName = fileName;
            Action = action;
        }
    }
}
