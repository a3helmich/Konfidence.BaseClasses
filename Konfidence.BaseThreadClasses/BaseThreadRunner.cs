using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;

namespace Konfidence.BaseThreadClasses
{
    public abstract class BaseThreadRunner<T> : BaseItem where T : BaseThreadExecute, new()
    {
        protected abstract void ThreadLoop();

        private T _ThreadExecute = null;

        public T ThreadExecute
        {
            get { return _ThreadExecute; }
        }

        public void StartThreadRunner()
        {
        }

        public void StopThreadRunner()
        {
        }
    }
}
