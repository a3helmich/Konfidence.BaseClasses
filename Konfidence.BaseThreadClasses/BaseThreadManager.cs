using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;

namespace Konfidence.BaseThreadClasses
{
    public class BaseThreadManager<T, K> : BaseItem where T: BaseThreadRunner<K>, new() where K: BaseThreadExecute, new()
    {
        private T _ThreadRunner = null;

        public T ThreadRunner
        {
            get { return _ThreadRunner; }
        }

        public bool IsActive
        {
            get { return IsAssigned(_ThreadRunner); }
        }

        public void StartThread()
        {
            if (!IsAssigned(_ThreadRunner))
            {
                _ThreadRunner = new T();

                _ThreadRunner.StartThreadRunner();
            }
        }

        public void StopThread()
        {
            if (IsAssigned(_ThreadRunner))
            {
                _ThreadRunner.StopThreadRunner();

                _ThreadRunner = null;
            }
        }
    }
}
