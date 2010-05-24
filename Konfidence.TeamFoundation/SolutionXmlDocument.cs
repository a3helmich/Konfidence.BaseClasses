using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Konfidence.TeamFoundation
{
    public class SolutionXmlDocument : BaseTfsXmlDocument
    {
        // TODO :  parse solution file into an xml document format
        private string _SolutionPath = string.Empty;

        // TODO : what to do when multiple solutions are in the same folder? 
        public SolutionXmlDocument(string solutionPath)
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
    }
}
