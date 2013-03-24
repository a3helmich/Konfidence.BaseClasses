using System;
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

        private T _ThreadAction;
        private Thread _InternalThread;

        internal int SleepTime;
        internal int WaitTime;

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

            while (_InternalThread.IsAlive && !ThreadAction.IsTerminating)
            {
                BeforeExecute();

                ThreadAction.Execute();

                SleepThread(WaitTime); // default 100 miliseconds

                AfterExecute();
            }

            CleanupThread();
        }

        protected void SleepThread(int milliSeconds)
        {
            if (SleepTime > WaitTime)
            {
                SleepTime = WaitTime;
            }

            if (_InternalThread.IsAlive)
            {
                int index = 0;

                // Wacht een aantal seconden
                while ((index < milliSeconds) && !ThreadAction.IsTerminating)
                {
                    index++;

                    Thread.Sleep(SleepTime); // relieve CPU, default 0 milliseconds
                }
            }
        }

        public void StartThreadRunner()
        {
            _ThreadAction = new T();

            _InternalThread = new Thread(InternalThreadLoop);

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
    }
}
