using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Konfidence.TeamFoundation
{
    internal class TfsCheckOut : IDisposable 
    {
        private string _FileName = string.Empty;
        private TfsPermissions _TfsPermissions = null;
        private bool _IsValid = false;

        public bool IsValid
        {
            get { return _IsValid; }
        }

        public TfsCheckOut(TfsPermissions tfsPermissions, string fileName)
        {
            _FileName = fileName;
            _TfsPermissions = tfsPermissions;

            if (!string.IsNullOrEmpty(fileName))
            {
                if (tfsPermissions.CheckOut(fileName))
                {
                    _IsValid = true;
                }
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
                _TfsPermissions.CheckIn(_FileName);
        }

        #endregion
    }

}
