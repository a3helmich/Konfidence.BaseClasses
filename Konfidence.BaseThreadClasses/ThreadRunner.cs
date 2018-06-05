using System;
using System.Threading;
using Konfidence.Base;

namespace Konfidence.BaseThreadClasses
{
    internal class ThreadRunner<TAction> where TAction : ThreadAction, new()
    {
        private Thread internalThread;
        private readonly ThreadManager<TAction> _threadManager;

        private int _sleepTime = 3;
        private SleepUnit _sleepUnit = SleepUnit.Seconds;
        private bool _isAlive;

        internal bool IsRunning => internalThread.IsAlive && (ThreadAction.IsAlive || _isAlive);

        internal ThreadRunner(ThreadManager<TAction> threadManager)
        {
            _threadManager = threadManager;

            ThreadAction = new TAction();
        }

        internal TAction ThreadAction { get; }

        private void InternalThreadLoop()
        {
            _threadManager.InternalInitializeAction(ThreadAction);

            while (IsRunning)
            {
                _threadManager.InternalBeforeExecuteAction(ThreadAction);

                try
                {
                    ThreadAction.ExecuteAction();
                }
                catch
                {
                    // when the action throws an exception....
                }

                SleepThread(_sleepTime, _sleepUnit);

                _threadManager.InternalAfterExecuteAction(ThreadAction);
            }

            CleanupThread();
        }

        private void SleepThread(int sleepTime, SleepUnit sleepUnit)
        {
            if (internalThread.IsAlive && !ThreadAction.IsAlive)
            {
                var timeSpan = new TimeSpan(0, 0, 0, 4);

                switch (sleepUnit)
                {
                    case SleepUnit.Daily:
                        timeSpan = new TimeSpan(sleepTime, 0, 0, 0);
                        break;
                    case SleepUnit.Hourly:
                        timeSpan = new TimeSpan(0, sleepTime, 0, 0);
                        break;
                    case SleepUnit.Minutes:
                        timeSpan = new TimeSpan(0, 0, sleepTime, 0);
                        break;
                    case SleepUnit.Seconds:
                        timeSpan = new TimeSpan(0, 0, 0, sleepTime);
                        break;
                }

                Thread.Sleep(timeSpan); // relieve CPU, default 0 milliseconds
            }
        }

        internal void StartThreadRunner(int sleepTime, SleepUnit sleepUnit)
        {
            _sleepTime = sleepTime;
            _sleepUnit = sleepUnit;

            _isAlive = true;

            internalThread = new Thread(InternalThreadLoop);

            internalThread.Start();
        }

        internal void StopThreadRunner()
        {
            _isAlive = false;

            if (internalThread.IsAssigned())
            {
                if (internalThread.IsAlive)
                {
                    internalThread.Join();
                }
            }

            CleanupThread();
        }

        private void CleanupThread()
        {
            try
            {
                internalThread = null;

                GC.Collect(GC.MaxGeneration); 
            }
            catch 
            {
                // ReSharper disable once UnusedVariable
            }
        }
    }
}
