using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Konfidence.Base;

namespace Konfidence.TeamFoundation
{
    public class SolutionTextDocument : BaseItem
    {
        private string _SolutionPath = string.Empty;
        private string _SolutionFile = string.Empty;

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
        }

        public void AddProjectFile(ProjectXmlDocument projectFile)
        {
            List<string> resultFileLines = new List<string>();

            foreach (string line in _TextFileLines)
            {
                if (line.Equals("Global", StringComparison.InvariantCultureIgnoreCase))
                {
                    InsertProjectLines(projectFile, resultFileLines);
                }

                resultFileLines.Add(line);
            }

            _TextFileLines.Clear();

            _TextFileLines.AddRange(resultFileLines);
        }

        private void InsertProjectLines(ProjectXmlDocument projectFile, List<string> resultFileLines)
        {
            // Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "TestClassGeneratorClasses", "TestClassGeneratorClassesDb\TestClassGeneratorClasses.csproj", "{CB59EBEC-CF6D-4613-93FE-3C387DD00513}"
// EndProject

            string solutionPath = Path.GetDirectoryName(this._SolutionFile);
            string projectPath = Path.GetDirectoryName(projectFile.FileName);

            if (!solutionPath.EndsWith(@"\"))
            {
                solutionPath += @"\";
            }

            string relativePath = projectPath.Replace(solutionPath, string.Empty);

            if (!relativePath.EndsWith(@"\"))
            {
                relativePath += @"\";
            }

            string projectStartLine = @"Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ";  // projecttype guid
            string projectName = "\"" + Path.GetFileNameWithoutExtension(projectFile.FileName) + "\", ";
            string projectFileName = "\"" + relativePath + Path.GetFileName(projectFile.FileName) + "\", ";
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
