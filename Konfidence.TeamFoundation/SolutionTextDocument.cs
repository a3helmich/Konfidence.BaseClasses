using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Konfidence.Base;
using Konfidence.TeamFoundation.Solution;

namespace Konfidence.TeamFoundation
{
    public class SolutionTextDocument : BaseItem
    {
        private string _SolutionPath = string.Empty;
        private string _SolutionFile = string.Empty;

        private List<string> _ConfigurationList = null;

        private List<string> _TextFileLines = new List<string>();

        public int SccNumberOfProjects
        {
            get
            {
                return ParseSccNumberOfProjects();
            }
        }

        public int NumberOfSolutionProjects
        {
            get
            {
                if (HasSolutionItem)
                {
                    return SolutionProjectList.Count() + 1;
                }

                return SolutionProjectList.Count();
            }
        }

        public SolutionProjectList SolutionProjectList
        {
            get
            {
                return ParseSolutionProjectList();
            }
        }

        protected List<string> ConfigurationList
        {
            get
            {
                if (!IsAssigned(_ConfigurationList))
                {
                    _ConfigurationList = ParseConfigurations();
                }

                return _ConfigurationList;
            }
        }

        public bool HasSolutionItem
        {
            get
            {
                return ParseSolutionItem();
            }
        }

        private bool ValidNumberOfProjects
        {
            get
            {
                if (SccNumberOfProjects != NumberOfSolutionProjects)
                {
                    return false;
                }

                return true;
            }
        }

        private int SolutionProjectCount()
        {
            return SolutionProjectList.Count;
        }

        private SolutionTextDocument()
        {
        }

        // TODO : what to do when multiple solutions are in the same folder? 
        internal SolutionTextDocument(string solutionPath)
        {
            _SolutionPath = solutionPath;
        }

        // TODO : get projectList from solution file
        public List<string> GetProjectFileList() 
        {
            List<string>  projectFileList = new List<string>();

            projectFileList.AddRange(Directory.GetFiles(_SolutionPath, "*.csproj", SearchOption.AllDirectories));

            return projectFileList;
        }

        public static SolutionTextDocument GetSolutionXmlDocument(string solutionFile)
        {
            SolutionTextDocument newSolutionTextDocument = new SolutionTextDocument();

            newSolutionTextDocument.Load(solutionFile);

            return newSolutionTextDocument;
        }

        private void Load(string solutionFile)
        {
            _SolutionFile = solutionFile;

            using (TextReader solutionTextFile = new StreamReader(_SolutionFile, Encoding.Default))
            {
                string line = solutionTextFile.ReadLine();

                while (line != null)
                {
                    _TextFileLines.Add(line);

                    line = solutionTextFile.ReadLine();
                }
            }
        }

        private int ParseSccNumberOfProjects()
        {
            int numberOfProjects = 0;

            foreach (string line in _TextFileLines)
            {
                if (line.Trim().StartsWith("SccNumberOfProjects"))
                {
                    string numberOfProjectString = line.Substring(line.IndexOf("=") + 1).Trim();

                    int.TryParse(numberOfProjectString, out numberOfProjects);

                    break;
                }
            }

            return numberOfProjects;
        }

        private bool ParseSolutionItem()
        {
            foreach (string line in _TextFileLines)
            {
                if (line.StartsWith(@"Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"")"))
                {
                    return true;
                }
            }

            return false;
        }

        private SolutionProjectList ParseSolutionProjectList()
        {
            SolutionProjectList solutionProjectList = new SolutionProjectList();

            SolutionProject project = null;

            foreach (string line in _TextFileLines)
            {
                if (line.Equals("EndProject") && IsAssigned(project))
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

                if (IsAssigned(project))
                {
                    project.AddLine(line);
                }
            }

            return solutionProjectList;
        }

        private List<string> ParseConfigurations()
        {
            List<string> configList = new List<string>();

            bool isConfigurationSection = false;

            foreach (string line in _TextFileLines)
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
            foreach (SolutionProject project in SolutionProjectList)
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

                // nb kan pas nadat het project is toegevoegd
                SetSccNumberOfProjects();

                AddTFSProjectEntries(projectFile);

                AddConfigurationPlatforms(projectFile);
            }
        }

        private void AddDataItemGeneratorConfigFile()
        {
            // Project("{2150E333-8FDC-42A3-9474-1A3956D46DE8}") = "DataItemGeneratorConfig", "DataItemGeneratorConfig"
            if (!ContainsDataItemGeneratorConfigFile())
            {
                List<string> resultFileLines = new List<string>();
                bool isAdded = false;

                foreach (string line in _TextFileLines)
                {
                    // voeg project toe
                    if (line.StartsWith("Project(", StringComparison.InvariantCultureIgnoreCase) && !isAdded)
                    {
                        InsertDataItemGeneratorConfigFileLines(resultFileLines);

                        isAdded = true;
                    }

                    resultFileLines.Add(line);
                }

                _TextFileLines.Clear();

                _TextFileLines.AddRange(resultFileLines);
            }
        }

        private void InsertDataItemGeneratorConfigFileLines(List<string> resultFileLines)
        {
            string projectGuid = Guid.NewGuid().ToString("B");
            
            string projectStart = "Project(\"{2150E333-8FDC-42A3-9474-1A3956D46DE8}\") = \"DataItemGeneratorConfig\", \"DataItemGeneratorConfig\", \"" + projectGuid + "\"";
            string projectSesionStart = "\tProjectSection(SolutionItems) = preProject";
            string fileEntry = "\t\tDataItemGenerator.config.xml = DataItemGenerator.config.xml";
            string projectSesionEnd = "\tEndProjectSection";
            string projectEnd = "EndProject";

            resultFileLines.Add(projectStart);
            resultFileLines.Add(projectSesionStart);
            resultFileLines.Add(fileEntry);
            resultFileLines.Add(projectSesionEnd);
            resultFileLines.Add(projectEnd);
        }

        private bool ContainsDataItemGeneratorConfigFile()
        {
            foreach (string line in _TextFileLines)
            {
                if (line.StartsWith("Project(\"{2150E333-8FDC-42A3-9474-1A3956D46DE8}\") = \"DataItemGeneratorConfig\", \"DataItemGeneratorConfig\"", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private void AddProjectEntry(ProjectXmlDocument projectFile)
        {
            List<string> resultFileLines = new List<string>();

            foreach (string line in _TextFileLines)
            {
                // voeg project toe
                if (line.Equals("Global", StringComparison.InvariantCultureIgnoreCase))
                {
                    InsertProjectLines(projectFile, resultFileLines);
                }

                resultFileLines.Add(line);
            }

            _TextFileLines.Clear();

            _TextFileLines.AddRange(resultFileLines);
        }

        private void InsertConfigurationLines(List<string> resultFileLines, ProjectXmlDocument projectFile)
        {
            foreach (string configuration in ConfigurationList)
            {
                resultFileLines.Add("\t\t" + projectFile.ProjectGuid + "." + configuration.Replace("CPU =", "CPU.ActiveCfg ="));
                resultFileLines.Add("\t\t" + projectFile.ProjectGuid + "." + configuration.Replace("CPU =", "CPU.Build.0 ="));
            }
        }

        private void AddConfigurationPlatforms(ProjectXmlDocument projectFile)
        {
            List<string> resultFileLines = new List<string>();

            bool isConfigurationSection = false;

            foreach (string line in _TextFileLines)
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

            _TextFileLines.Clear();

            _TextFileLines.AddRange(resultFileLines);
        }

        private void AddTFSProjectEntries(ProjectXmlDocument projectFile)
        {
            List<string> resultFileLines = new List<string>();

            bool isTfsSection = false;

            foreach (string line in _TextFileLines)
            {
                if (line.Trim().StartsWith("GlobalSection(TeamFoundationVersionControl)"))
                {
                    isTfsSection = true;
                }
                else
                {
                    if (isTfsSection)
                    {
                        if (line.Trim().StartsWith("EndGlobalSection"))
                        {
                            string relativeProjectFileName = projectFile.GetRelativeProjectFileName(SolutionPath);

                            string count = SolutionProjectCount().ToString();

                            string SccProjectUniqueNameLine = ("\t\tSccProjectUniqueName" + count + " = " + relativeProjectFileName).Replace(@"\", @"\\");
                            //string SccProjectNameLine = "\t\tSccProjectName" + count + " = " + projectFile.ProjectName;
                            string SccProjectNameLine = "\t\tSccProjectName" + count + " = " + Path.GetDirectoryName(relativeProjectFileName);
                            string SccLocalPathLine = "\t\tSccLocalPath" + count + " = " + Path.GetDirectoryName(relativeProjectFileName);

                            resultFileLines.Add(SccProjectUniqueNameLine);
                            resultFileLines.Add(SccProjectNameLine);
                            resultFileLines.Add(SccLocalPathLine);

                            isTfsSection = false;
                        }
                    }
                }

                resultFileLines.Add(line);
            }

            _TextFileLines.Clear();

            _TextFileLines.AddRange(resultFileLines);
        }

        private void SetSccNumberOfProjects()
        {
            List<string> resultFileLines = new List<string>();

            string replaceLine;

            foreach (string line in _TextFileLines)
            {
                replaceLine = string.Empty;

                // valideer het aantal projecten in de solution
                if (line.Trim().StartsWith("SccNumberOfProjects", StringComparison.InvariantCultureIgnoreCase) && !ValidNumberOfProjects)
                {
                    replaceLine = line.Substring(0, line.IndexOf("=") + 1) + " " + NumberOfSolutionProjects.ToString();
                }

                if (IsEmpty(replaceLine))
                {
                    resultFileLines.Add(line);
                }
                else
                {
                    resultFileLines.Add(replaceLine);
                }
            }

            _TextFileLines.Clear();

            _TextFileLines.AddRange(resultFileLines);
        }

        protected string SolutionPath
        {
            get
            {
                string solutionPath = Path.GetDirectoryName(this._SolutionFile);

                if (!solutionPath.EndsWith(@"\"))
                {
                    solutionPath += @"\";
                }

                return solutionPath;
            }
        }

        private void InsertProjectLines(ProjectXmlDocument projectFile, List<string> resultFileLines)
        {
            string relativeProjectFileName = projectFile.GetRelativeProjectFileName(SolutionPath);

            string projectStartLine = @"Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ";  // projecttype guid
            string projectName = "\"" + projectFile.ProjectName + "\", ";
            string projectFileName = "\"" + relativeProjectFileName + "\", ";
            string projectGuid = "\"" + projectFile.ProjectGuid + "\"";

            string projectLine = projectStartLine + projectName + projectFileName + projectGuid;

            resultFileLines.Add(projectLine);
            resultFileLines.Add("EndProject");
        }

        public void Save()
        {
            using (TextWriter solutionTextFile = new StreamWriter(_SolutionFile, false, Encoding.UTF8))
            {
                foreach (string line in _TextFileLines)
                {
                    solutionTextFile.WriteLine(line);
                }
            }
        }
    }
}
