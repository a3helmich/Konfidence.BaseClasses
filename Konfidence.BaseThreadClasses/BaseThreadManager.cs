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

        public bool IsRunning
        {
            get
            {
                if (IsAssigned(_ThreadRunner))
                {
                    return _ThreadRunner.IsRunning;
                }

                return false;
            }
        }

        private BaseThreadManager()
        {
             // afsluiten voor de buitenwereld
        }

        public BaseThreadManager(L parameterObject)
        {
            _ParameterObject = parameterObject;
        }

        private void GetThreadRunner()
        {
            _ThreadRunner = new T();
        }

        private void DeleteThreadRunner()
        {
            _ThreadRunner = null;
        }

        public void StartThread()
        {
            if (!IsRunning)
            {
                GetThreadRunner();

                ThreadRunner.SetParameterObject(_ParameterObject);

                ThreadRunner.StartThreadRunner();
            }
        }

        public void StopThread()
        {
            if (IsRunning)
            {
                ThreadRunner.StopThreadRunner();

                DeleteThreadRunner();
            }
        }
    }
}
