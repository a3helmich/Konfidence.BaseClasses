using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;

namespace Konfidence.BaseThreadClasses
{
    public abstract class BaseThreadAction<T> : BaseItem where T: BaseThreadParameterObject
    {
        internal protected abstract void Execute();

        private bool _IsTerminating = false;
        private T _ParameterObject = null;

        protected T ParameterObject
        {
            get { return _ParameterObject; }
        }

        internal bool IsTerminating
        {
            get { return _IsTerminating; }
            set { _IsTerminating = value; }
        }

        internal void SetParameterObject(BaseParameterObject parameterObject)
        {
            _ParameterObject = parameterObject as T;
        }

        protected void Finished()
        {
            IsTerminating = true;
        }
    }
}
