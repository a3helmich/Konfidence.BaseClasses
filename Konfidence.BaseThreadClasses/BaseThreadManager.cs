using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;

namespace Konfidence.BaseThreadClasses
{
    public abstract class BaseThreadManager<T, K> : BaseItem
        where T : BaseThreadRunner<K>, new() where K : BaseThreadAction, new() 
    {
        protected abstract void BeforeStart();
        protected abstract void AfterStop();

        private T _ThreadRunner = null;

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
