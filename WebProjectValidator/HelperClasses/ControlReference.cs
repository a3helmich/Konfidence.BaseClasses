using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebProjectValidator.HelperClasses
{
    class ControlReference : IComparable
    {
        private string _FileName = string.Empty;
        private string _Reference = string.Empty;

        #region simple properties
        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        public string Reference
        {
            get { return _Reference; }
            set { _Reference = value; }
        }
        #endregion simple properties

        public ControlReference(string fileName, string reference)
        {
            _FileName = fileName;
            _Reference = reference;
        }

        public int CompareTo(object obj)
        {
            ControlReference other = obj as ControlReference;

            return other.FileName.CompareTo(FileName);
        }
    }
}
