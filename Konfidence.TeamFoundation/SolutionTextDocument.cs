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

        private bool _HasNewProject = false;
        private int _NumberOfProjects = 0;

        protected bool HasNewProject
        {
            get { return _HasNewProject; }
        }

        private SolutionProjectList projectList = new SolutionProjectList();

        private List<string> _TextFileLines = new List<string>();

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

            ParseFile();
        }

        private void ParseFile()
        {
            SolutionProject project = null;

            foreach (string line in _TextFileLines)
            {
                if (line.Trim().StartsWith("SccNumberOfProjects"))
                {
                    string numberOfProjects = line.Substring(line.IndexOf("=") + 1).Trim();

                    int.TryParse(numberOfProjects, out _NumberOfProjects);
                }

                if (line.StartsWith("EndProject"))
                {
                    projectList.Add(project);

                    project = null;
                }

                if (line.StartsWith(@"Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"")"))
                {
                    project = new SolutionProject();
                }

                if (IsAssigned(project))
                {
                    project.AddLine(line);
                }
            }
        }

        private bool ValidNumberOfProjects
        {
            get
            {
                if (_NumberOfProjects != NewNumberOfProjects)
                {
                    return false;
                }

                return true;
            }
        }

        protected int NewNumberOfProjects
        {
            get 
            {
            if (HasNewProject)
            {
                return projectList.Count + 1;
            }

            return projectList.Count;
            }
        }

        private bool ValidProjectFile(ProjectXmlDocument projectFile)
        {


            return false;
        }

        public void AddProjectFile(ProjectXmlDocument projectFile)
        {
            if (ValidProjectFile(projectFile))
            {
                List<string> resultFileLines = new List<string>();

                string replaceLine;

                foreach (string line in _TextFileLines)
                {
                    replaceLine = string.Empty;

                    // voeg project toe
                    if (line.Equals("Global", StringComparison.InvariantCultureIgnoreCase))
                    {
                        InsertProjectLines(projectFile, resultFileLines);
                    }

                    // valideer het aantal projecten in de solution
                    if (line.Trim().StartsWith("SccNumberOfProjects", StringComparison.InvariantCultureIgnoreCase) && !ValidNumberOfProjects)
                    {
                        replaceLine = line.Substring(0, line.IndexOf("=")) + " " + NewNumberOfProjects.ToString();
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
