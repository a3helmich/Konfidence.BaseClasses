using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.MsBuild.Solution
{
    public class SolutionProject 
    {
        private readonly StringBuilder _innerText = new StringBuilder();
        private List<string> _innerList;

        private static readonly char[] Trimmer = new[] {'"', ','};

        [NotNull]
        protected List<string> InnerList
        {
            get
            {
                if (!_innerList.IsAssigned())
                {
                    _innerList = new List<string>();

                    _innerList.AddRange(_innerText.ToString().Split(' '));
                }

                return _innerList;
            }
        }

        private const string Space = " ";

        private string _projectName = string.Empty;
        private string _projectFile = string.Empty;
        private string _projectGuid = string.Empty;

        [NotNull]
        public string ProjectFile
        {
            get
            {
                if (!_projectFile.IsAssigned())
                {
                    _projectFile = InnerList[3].Trim(Trimmer);
                }

                return _projectFile;
            }
        }

        [NotNull]
        public string ProjectName
        {
            get
            {
                if (!_projectName.IsAssigned())
                {
                    _projectName = InnerList[2].Trim(Trimmer);
                }

                return _projectName;
            }
        }

        [NotNull]
        public string ProjectGuid
        {
            get
            {
                if (!_projectGuid.IsAssigned())
                {
                    _projectGuid = InnerList[4].Trim(Trimmer);
                }

                return _projectGuid;
            }
        }

        internal void AddLine(string line)
        {
            if (_innerText.ToString().Length == 0)
            {
                _innerText.Append(line);
            }
            else
            {
                _innerText.Append(Space);
                _innerText.Append(line);
            }
        }
    }
}
