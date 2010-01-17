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
        private bool _IsAllreadyCheckedOut = false;

        public bool IsValid
        {
            get { return _IsValid; }
        }

        public TfsCheckOut(TfsPermissions tfsPermissions, string fileName)
        {
            _FileName = fileName;
            _TfsPermissions = tfsPermissions;

            _IsAllreadyCheckedOut = tfsPermissions.IsCheckedOut(fileName);

            if (!_IsAllreadyCheckedOut)
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    if (tfsPermissions.CheckOut(fileName))
                    {
                        _IsValid = true;
                    }
                }
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (!_IsAllreadyCheckedOut)
            {
                _TfsPermissions.CheckIn(_FileName);
            }
        }

        #endregion
    }

}
