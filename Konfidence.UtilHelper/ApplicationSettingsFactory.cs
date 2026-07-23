using JetBrains.Annotations;

namespace Konfidence.UtilHelper
{
    public static class ApplicationSettingsFactory
    {
        private static string _rootPath = string.Empty;

        [UsedImplicitly]
        public static IApplicationSettings ApplicationSettings(string application, string rootPath)
        {
            string normalizedRootPath = NormalizeRootPath(rootPath);

            _rootPath = normalizedRootPath;

            return CreateApplicationSettings(application, normalizedRootPath);
        }

        public static IApplicationSettings ApplicationSettings(string application)
        {
            return CreateApplicationSettings(application, _rootPath);
        }

        private static ApplicationSettings CreateApplicationSettings(string application, string rootPath)
        {
            return new ApplicationSettings(application)
            {
                RootPath = rootPath
            };
        }

        private static string NormalizeRootPath(string rootPath)
        {
            string normalizedRootPath = rootPath;

            if (!normalizedRootPath.EndsWith(@"\"))
            {
                normalizedRootPath += @"\";
            }

            if (!normalizedRootPath.EndsWith(@"settings\"))
            {
                normalizedRootPath += @"settings\";
            }

            return normalizedRootPath;
        }
    }
}