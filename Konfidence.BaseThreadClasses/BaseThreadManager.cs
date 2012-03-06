using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;

namespace Konfidence.BaseThreadClasses
{
    public abstract class BaseThreadManager<T, K, L> : BaseItem
        where T : BaseThreadRunner<K, L>, new() where K : BaseThreadAction<L>, new() where L : BaseThreadParameterObject
    {
        protected abstract void BeforeStart();
        protected abstract void AfterStop();

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

            BeforeStart();
        }

        private void DeleteThreadRunner()
        {
            AfterStop();

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
