using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;

namespace WebProjectValidator.HelperClasses
{
    public class ControlReference : BaseItem,  IComparable, IEquatable<ControlReference>
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

        public ControlReference(string reference, string fileFolder)
        {
            _Reference = reference;
            _FileName = GetFileName(reference, fileFolder);
        }

        private string GetFileName(string reference, string fileFolder)
        {
            string newControlFileName = reference.Replace("/", @"\");

            if (!newControlFileName.StartsWith(@"~"))
            {
                newControlFileName = fileFolder + @"\" + newControlFileName;
            }

            return newControlFileName;
        }

        public int CompareTo(object obj)
        {
            ControlReference otherControlReference = obj as ControlReference;

            if (IsAssigned(otherControlReference))
            {
                return FileName.CompareTo(otherControlReference.FileName);
            }

            throw new Exception("Object is not a ControlReference!");
        }

        public bool Equals(ControlReference other)
        {
            ControlReference otherControlReference = other as ControlReference;

            if (IsAssigned(otherControlReference))
            {
                return FileName.Equals(otherControlReference.FileName);
            }

            throw new Exception("Object is not a ControlReference!");
        }
    }
}
