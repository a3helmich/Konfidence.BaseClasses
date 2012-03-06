using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;
using System.Threading;

namespace Konfidence.BaseThreadClasses
{
    public abstract class BaseThreadRunner<T> : BaseItem where T : BaseThreadAction, new() 
    {
        //protected abstract void ThreadLoop(T threadExecute);
        protected abstract void BeforeExecute();
        protected abstract void AfterExecute();
        protected abstract void InitializeThreadLoop();

        private T _ThreadAction = null;
        private Thread _InternalThread = null;


        internal bool IsRunning
        {
            get
            {
                if (IsAssigned(_ThreadAction))
                {
                    return true;
                }

                return false;
            }
        }

        protected T ThreadAction
        {
            get { return _ThreadAction; }
        }

        private void InternalThreadLoop()
        {
            InitializeThreadLoop();

            //ThreadLoop(ThreadAction);
            while (_InternalThread.IsAlive && !ThreadAction.IsTerminating)
            {
                BeforeExecute();

                ThreadAction.Execute();

                AfterExecute();
            }

            CleanupThread();
        }

        public void StartThreadRunner()
        {
            _ThreadAction = new T();

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

            if (_InternalThread.IsAlive)
            {
                _InternalThread.Join();
            }

            CleanupThread();
        }

        private void CleanupThread()
        {
            try
            {
                _InternalThread = null;

                _ThreadAction = null;

                GC.Collect(GC.MaxGeneration); //  GC.Collect(GC.GetGeneration(this));
            }
            catch (Exception ex)
            {
                string test = ex.Message;
            }
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
