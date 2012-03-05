using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;

namespace Konfidence.BaseThreadClasses
{
    public class BaseThreadManager<T, K, L> : BaseItem
        where T : BaseThreadRunner<K, L>, new() where K : BaseThreadAction<L>, new() where L : BaseThreadParameterObject
    {
        private T _ThreadRunner = null;
        private L _ParameterObject = null;

        protected T ThreadRunner
        {
            get { return _ThreadRunner; }
        }

        public bool IsActive
        {
            get { return IsAssigned(_ThreadRunner); }
        }

        private BaseThreadManager()
        {
             // afsluiten voor de buitenwereld
        }

        public BaseThreadManager(L parameterObject)
        {
            _ParameterObject = parameterObject;
        }

        public void StartThread()
        {
            if (!IsAssigned(_ThreadRunner))
            {
                _ThreadRunner = new T();

                _ThreadRunner.SetParameterObject(_ParameterObject);

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
