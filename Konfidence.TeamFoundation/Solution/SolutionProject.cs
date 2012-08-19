using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;

namespace Konfidence.TeamFoundation.Solution
{
    public class SolutionProject : BaseItem
    {
        private StringBuilder _InnerText = new StringBuilder();
        private List<string> _InnerList = null;

        private static char[] trimmer = new char[] {'"', ','};

        protected List<string> InnerList
        {
            get
            {
                if (!IsAssigned(_InnerList))
                {
                    _InnerList = new List<string>();

                    _InnerList.AddRange(_InnerText.ToString().Split(' '));
                }

                return _InnerList;
            }
        }

        private static string space = " ";

        private string _ProjectName = string.Empty;
        private string _ProjectFile = string.Empty;
        private string _ProjectGuid = string.Empty;

        public string ProjectFile
        {
            get
            {
                if (IsEmpty(_ProjectFile))
                {
                    _ProjectFile = InnerList[3].Trim(trimmer);
                }

                return _ProjectFile;
            }
        }

        public string ProjectName
        {
            get
            {
                if (IsEmpty(_ProjectName))
                {
                    _ProjectName = InnerList[2].Trim(trimmer);
                }

                return _ProjectName;
            }
        }

        public string ProjectGuid
        {
            get
            {
                if (IsEmpty(_ProjectGuid))
                {
                    _ProjectGuid = InnerList[4].Trim(trimmer);
                }

                return _ProjectGuid;
            }
        }

        internal void AddLine(string line)
        {
            if (_InnerText.ToString().Length == 0)
            {
                _InnerText.Append(line);
            }
            else
            {
                _InnerText.Append(space);
                _InnerText.Append(line);
            }
        }
    }
}
