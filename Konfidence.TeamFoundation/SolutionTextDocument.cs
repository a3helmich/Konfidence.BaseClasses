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

        public void AddProjectFile(string projectFile)
        {
            int lineIndex = 0;

            foreach (string line in _TextFileLines)
            {
                if (line.Equals("Global", StringComparison.InvariantCultureIgnoreCase))
                {
                    lineIndex++;
                    break;
                }
            }


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
