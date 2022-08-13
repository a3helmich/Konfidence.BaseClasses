using JetBrains.Annotations;

namespace Konfidence.UtilHelper
{
    public static class ApplicationSettingsFactory 
    {
        private static string _rootPath = string.Empty;

        [UsedImplicitly]
        public static IApplicationSettings ApplicationSettings(string application, string rootPath)
        {
            _rootPath = rootPath;

            if (!_rootPath.EndsWith(@"\"))
            {
                _rootPath += @"\";
            }

            if (!_rootPath.EndsWith(@"settings\"))
            {
                _rootPath += @"settings\";
            }

            return ApplicationSettings(application);
        }

        public static IApplicationSettings ApplicationSettings(string application)
        {
            var applicationSettings = new ApplicationSettings(application)
            {
                RootPath = _rootPath
            }; 


            return applicationSettings;
        }
    }
}