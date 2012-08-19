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
        private static string space = " ";

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
