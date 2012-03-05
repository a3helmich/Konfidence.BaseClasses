using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;
using System.Threading;

namespace Konfidence.BaseThreadClasses
{
    public abstract class BaseThreadRunner<T> : BaseItem where T : BaseThreadExecute, new()
    {
        protected abstract void ThreadLoop(T threadExecute);

        private T _ThreadExecute = null;
        private Thread _InternalThread = null;

        public T ThreadExecute
        {
            get { return _ThreadExecute; }
        }

        private void InternalThreadLoop()
        {
            ThreadLoop(ThreadExecute);
        }

        public void StartThreadRunner()
        {
            _ThreadExecute = new T();

            _InternalThread = new Thread(new ThreadStart(InternalThreadLoop));

            _InternalThread.Start();
        }

        public void StopThreadRunner()
        {
            if (IsAssigned(_InternalThread))
            {
                if (IsAssigned(ThreadExecute))
                {
                    ThreadExecute.IsTerminating = true;
                }
            }

            _InternalThread.Join();

            _InternalThread = null;

            _ThreadExecute = null;
        }

        protected void SleepThread(int seconds)
        {
            if (_InternalThread.IsAlive)
            {
                int index = 0;

                // Wacht een aantal seconden
                while ((index < seconds) && !ThreadExecute.IsTerminating)
                {
                    index++;
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
