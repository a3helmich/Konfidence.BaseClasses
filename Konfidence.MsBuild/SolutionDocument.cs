using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.MsBuild.Solution;

namespace Konfidence.MsBuild
{
    public class SolutionDocument 
    {
        private string _solutionFile = string.Empty;

        private List<string> _configurationList;

        private readonly List<string> _textFileLines = new List<string>();

        public int NumberOfSolutionProjects
        {
            get
            {
                if (HasSolutionItem)
                {
                    return Projects.Count + 1;
                }

                return Projects.Count;
            }
        }

        public SolutionProjectList Projects => ParseSolutionProjectList();

        protected List<string> ConfigurationList
        {
            get
            {
                if (!_configurationList.IsAssigned())
                {
                    _configurationList = ParseConfigurations();
                }

                return _configurationList;
            }
        }

        public bool HasSolutionItem => ParseSolutionItem();

        private int ProjectCount()
        {
            return Projects.Count;
        }

        private SolutionDocument()
        {
        }

        [NotNull]
        public static SolutionDocument GetSolutionDocument(string solutionFile)
        {
            var newSolutionTextDocument = new SolutionDocument();

            newSolutionTextDocument.Load(solutionFile);

            return newSolutionTextDocument;
        }

        private void Load(string solutionFile)
        {
            _solutionFile = solutionFile;

            using (TextReader solutionTextFile = new StreamReader(_solutionFile, Encoding.Default))
            {
                var line = solutionTextFile.ReadLine();

                while (line != null)
                {
                    _textFileLines.Add(line);

                    line = solutionTextFile.ReadLine();
                }
            }
        }

        private bool ParseSolutionItem()
        {
            foreach (var line in _textFileLines)
            {
                if (line.StartsWith(@"Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"")"))
                {
                    return true;
                }
            }

            return false;
        }

        [NotNull]
        private SolutionProjectList ParseSolutionProjectList()
        {
            var solutionProjectList = new SolutionProjectList();

            SolutionProject project = null;

            foreach (var line in _textFileLines)
            {
                if (line.Equals("EndProject") && project.IsAssigned())
                {
                    solutionProjectList.Add(project);

                    project = null;
                }

                if (line.StartsWith("Project"))
                {
                    if (!line.StartsWith(@"Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"")"))
                    {
                        if (line.StartsWith(@"Project("""))
                        {
                            project = new SolutionProject();
                        }
                    }
                }

                if (project.IsAssigned())
                {
                    project.AddLine(line);
                }
            }

            return solutionProjectList;
        }

        [NotNull]
        private List<string> ParseConfigurations()
        {
            var configList = new List<string>();

            var isConfigurationSection = false;

            foreach (var line in _textFileLines)
            {
                if (line.Trim().StartsWith("GlobalSection(SolutionConfigurationPlatforms)"))
                {
                    isConfigurationSection = true;
                }
                else
                {
                    if (isConfigurationSection)
                    {
                        if (line.Trim().StartsWith("EndGlobalSection"))
                        {

                            isConfigurationSection = false;
                        }
                        else
                        {
                            configList.Add(line.Trim());
                        }
                    }
                }
            }

            return configList;
        }

        private bool CanAddProjectFile(ProjectXmlDocument projectFile)
        {
            foreach (var project in Projects)
            {
                if (projectFile.ProjectGuid.Equals(project.ProjectGuid, StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }

                if (projectFile.ProjectName.Equals(project.ProjectName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }

                if (projectFile.FileName.EndsWith(project.ProjectFile, StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        public void AddProjectFile(ProjectXmlDocument projectFile)
        {
            if (CanAddProjectFile(projectFile))
            {
                AddDataItemGeneratorConfigFile();

                AddProjectEntry(projectFile);

                AddConfigurationPlatforms(projectFile);
            }
        }

        private void AddDataItemGeneratorConfigFile()
        {
            // Project("{2150E333-8FDC-42A3-9474-1A3956D46DE8}") = "ClassGeneratorConfig", "ClassGeneratorConfig"
            if (!ContainsDataItemGeneratorConfigFile())
            {
                var containsFolder = ContainsDataItemGeneratorConfigFolder();

                var resultFileLines = new List<string>();
                var isAdded = false;
                var folderFound = false;
                var folderProjectFound = false;

                foreach (var line in _textFileLines)
                {
                    if (!isAdded)
                    {
                        if (containsFolder)
                        {
                            if (folderFound)
                            {
                                if (folderProjectFound)
                                {
                                    // voeg projectRegel toe
                                    AddDataItemGeneratorConfigFileLines(resultFileLines);

                                    isAdded = true;
                                }
                                else
                                {
                                    if (line.Trim().StartsWith("ProjectSection(SolutionItems) = preProject"))
                                    {
                                        folderProjectFound = true;
                                    }
                                }
                            }
                            else
                            {
                                if (line.StartsWith("Project(\"{2150E333-8FDC-42A3-9474-1A3956D46DE8}\") = \"ClassGeneratorConfig\", \"ClassGeneratorConfig\"", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    folderFound = true;
                                }
                            }
                        }
                        else
                        {
                            if (line.StartsWith("Project(", StringComparison.InvariantCultureIgnoreCase))
                            {
                                InsertDataItemGeneratorConfigFileLines(resultFileLines);

                                isAdded = true;
                            }
                        }
                    }

                    resultFileLines.Add(line);
                }

                _textFileLines.Clear();

                _textFileLines.AddRange(resultFileLines);
            }
        }

        private static void AddDataItemGeneratorConfigFileLines([NotNull] List<string> resultFileLines)
        {
            const string fileEntry = "\t\tClassModelGenerator.config.json = ClassModelGenerator.config.json";

            resultFileLines.Add(fileEntry);
        }

        private static void InsertDataItemGeneratorConfigFileLines([NotNull] List<string> resultFileLines)
        {
            var projectGuid = "{36786862-305D-446D-8B01-BDAC2476260C}"; //Guid.NewGuid().ToString("B").ToUpper();

            var projectStart = "Project(\"{2150E333-8FDC-42A3-9474-1A3956D46DE8}\") = \"ClassGeneratorConfig\", \"ClassGeneratorConfig\", \"" + projectGuid + "\"";
            const string projectSesionStart = "\tProjectSection(SolutionItems) = preProject";
            const string fileEntry = "\t\tClassModelGenerator.config.json = ClassModelGenerator.config.json";
            const string projectSesionEnd = "\tEndProjectSection";
            const string projectEnd = "EndProject";

            resultFileLines.Add(projectStart);
            resultFileLines.Add(projectSesionStart);
            resultFileLines.Add(fileEntry);
            resultFileLines.Add(projectSesionEnd);
            resultFileLines.Add(projectEnd);
        }

        private bool ContainsDataItemGeneratorConfigFolder()
        {
            foreach (var line in _textFileLines)
            {
                if (line.StartsWith("Project(\"{2150E333-8FDC-42A3-9474-1A3956D46DE8}\") = \"ClassGeneratorConfig\", \"ClassGeneratorConfig\"", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private bool ContainsDataItemGeneratorConfigFile()
        {
            var foundEntry = false;

            foreach (var line in _textFileLines)
            {
                if (foundEntry)
                {
                    if (line.Trim().Equals("ClassModelGenerator.config.json = ClassModelGenerator.config.json", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }

                    if (line.Trim().StartsWith("EndProject"))
                    {
                        return false;
                    }
                }

                if (line.StartsWith("Project(\"{2150E333-8FDC-42A3-9474-1A3956D46DE8}\") = \"ClassGeneratorConfig\", \"ClassGeneratorConfig\"", StringComparison.InvariantCultureIgnoreCase))
                {
                    foundEntry = true;
                }
            }

            return false;
        }

        private void AddProjectEntry(ProjectXmlDocument projectFile)
        {
            var resultFileLines = new List<string>();

            foreach (var line in _textFileLines)
            {
                // voeg project toe
                if (line.Equals("Global", StringComparison.InvariantCultureIgnoreCase))
                {
                    InsertProjectLines(projectFile, resultFileLines);
                }

                resultFileLines.Add(line);
            }

            _textFileLines.Clear();

            _textFileLines.AddRange(resultFileLines);
        }

        private void InsertConfigurationLines(List<string> resultFileLines, ProjectXmlDocument projectFile)
        {
            foreach (var configuration in ConfigurationList)
            {
                resultFileLines.Add("\t\t" + projectFile.ProjectGuid + "." + configuration.Replace("CPU =", "CPU.ActiveCfg ="));
                resultFileLines.Add("\t\t" + projectFile.ProjectGuid + "." + configuration.Replace("CPU =", "CPU.Build.0 ="));
            }
        }

        private void AddConfigurationPlatforms(ProjectXmlDocument projectFile)
        {
            var resultFileLines = new List<string>();

            var isConfigurationSection = false;

            foreach (var line in _textFileLines)
            {
                if (line.Trim().StartsWith("GlobalSection(ProjectConfigurationPlatforms)"))
                {
                    isConfigurationSection = true;
                }
                else
                {
                    if (isConfigurationSection)
                    {
                        if (line.Trim().StartsWith("EndGlobalSection"))
                        {
                            InsertConfigurationLines(resultFileLines, projectFile);

                            isConfigurationSection = false;
                        }
                    }
                }

                resultFileLines.Add(line);
            }

            _textFileLines.Clear();

            _textFileLines.AddRange(resultFileLines);
        }

        [CanBeNull]
        protected string SolutionPath
        {
            get
            {
                var solutionPath = Path.GetDirectoryName(_solutionFile);

                if (solutionPath.IsAssigned() && !solutionPath.EndsWith(@"\"))
                {
                    solutionPath += @"\";
                }

                return solutionPath;
            }
        }

        private void InsertProjectLines([NotNull] ProjectXmlDocument projectFile, [NotNull] List<string> resultFileLines)
        {
            var relativeProjectFileName = projectFile.GetRelativeProjectFileName(SolutionPath);

            const string projectStartLine = @"Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = "; // projecttype guid
            var projectName = "\"" + projectFile.ProjectName + "\", ";
            var projectFileName = "\"" + relativeProjectFileName + "\", ";
            var projectGuid = "\"" + projectFile.ProjectGuid + "\"";

            var projectLine = projectStartLine + projectName + projectFileName + projectGuid;

            resultFileLines.Add(projectLine);
            resultFileLines.Add("EndProject");
        }

        public void Save()
        {
            using (TextWriter solutionTextFile = new StreamWriter(_solutionFile, false, Encoding.UTF8))
            {
                foreach (var line in _textFileLines)
                {
                    solutionTextFile.WriteLine(line);
                }
            }
        }
    }
}
