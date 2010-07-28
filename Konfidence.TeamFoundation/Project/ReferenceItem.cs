using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Konfidence.Base;

namespace Konfidence.TeamFoundation.Project
{
    public class ReferenceItem : BaseItem
    {
        private string _SpecificVersionElement = string.Empty;
        private string _HintPathElement = string.Empty;
        private string _IncludeAttribute = string.Empty;
        private string _Name = string.Empty;
        private bool _IsProjectReference = false;

        #region properties
        public string SpecificVersionElement
        {
            get { return _SpecificVersionElement; }
            set { _SpecificVersionElement = value; }
        }

        public string HintPathElement
        {
            get { return _HintPathElement; }
            set { _HintPathElement = value; }
        }

        public string IncludeAttribute
        {
            get { return _IncludeAttribute; }
            set { _IncludeAttribute = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public bool IsProjectReference
        {
            get { return _IsProjectReference; }
            set { _IsProjectReference = value; }
        }
        #endregion properties

    }
}