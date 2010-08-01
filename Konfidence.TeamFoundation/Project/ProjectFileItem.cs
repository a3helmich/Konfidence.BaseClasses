using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;

namespace Konfidence.TeamFoundation.Project
{
    // TODO : moet internal worde
    public class ProjectFileItem: BaseItem
    {
        private string _FileName = string.Empty;
        private string _Action = string.Empty;

        #region properties
        public string FileName
        {
            get { return _FileName; }
        }

        public string Action
        {
            get { return _Action; }
        }
        #endregion properties

        public ProjectFileItem(string fileName, string action)
        {
            _FileName = fileName;
            _Action = action;
        }
    }
}
