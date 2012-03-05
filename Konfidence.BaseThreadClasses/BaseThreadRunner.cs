using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;
using System.Threading;

namespace Konfidence.BaseThreadClasses
{
    public abstract class BaseThreadRunner<T, L> : BaseItem where T : BaseThreadAction<L>, new() where L: BaseThreadParameterObject
    {
        //protected abstract void ThreadLoop(T threadExecute);
        protected abstract void BeforeExecute();
        protected abstract void AfterExecute();

        private T _ThreadAction = null;
        private Thread _InternalThread = null;
        private BaseParameterObject _ParameterObject;

        protected T ThreadAction
        {
            get { return _ThreadAction; }
        }

        private void InternalThreadLoop()
        {
            //ThreadLoop(ThreadAction);
            while (_InternalThread.IsAlive && !ThreadAction.IsTerminating)
            {
                BeforeExecute();

                ThreadAction.Execute();

                AfterExecute();
            }
        }

        internal void SetParameterObject(BaseParameterObject parameterObject)
        {
            _ParameterObject = parameterObject;
        }

        public void StartThreadRunner()
        {
            _ThreadAction = new T();

            _ThreadAction.SetParameterObject(_ParameterObject);

            _InternalThread = new Thread(new ThreadStart(InternalThreadLoop));

            _InternalThread.Start();
        }

        public void StopThreadRunner()
        {
            if (IsAssigned(_InternalThread))
            {
                if (IsAssigned(ThreadAction))
                {
                    ThreadAction.IsTerminating = true;
                }
            }

            _InternalThread.Join();

            _InternalThread = null;

            _ThreadAction = null;
        }

        protected void SleepThread(int seconds)
        {
            if (_InternalThread.IsAlive)
            {
                int index = 0;

                // Wacht een aantal seconden
                while ((index < seconds) && !ThreadAction.IsTerminating)
                {
                    index++;
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
