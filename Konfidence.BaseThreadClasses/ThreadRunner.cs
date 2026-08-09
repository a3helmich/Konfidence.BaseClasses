using System;
using System.Diagnostics;
using System.Threading;
using Konfidence.Base;

namespace Konfidence.BaseThreadClasses;

internal class ThreadRunner<TAction> where TAction : ThreadAction, new()
{
    private Thread? internalThread;
    private readonly ThreadManager<TAction> _threadManager;

    private int _sleepTime = 3;
    private SleepUnit _sleepUnit = SleepUnit.Seconds;
    private bool _isAlive;

    // The worker clears internalThread from CleanupThread() while other threads are reading it, so
    // every read below snapshots the field into a local first - checking the field twice lets it
    // turn null between the null check and the dereference.
    internal bool IsRunning
    {
        get
        {
            Thread? runningThread = internalThread;

            return runningThread.IsAssigned() && runningThread.IsAlive && (ThreadAction.IsAlive || _isAlive);
        }
    }

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
            catch (Exception exception)
            {
                Trace.WriteLine($"ThreadRunner action threw an exception: {exception}");
            }

            SleepThread(_sleepTime, _sleepUnit);

            _threadManager.InternalAfterExecuteAction(ThreadAction);
        }

        CleanupThread();
    }

    private void SleepThread(int sleepTime, SleepUnit sleepUnit)
    {
        Thread? runningThread = internalThread;

        if (runningThread.IsAssigned() && runningThread.IsAlive && !ThreadAction.IsAlive)
        {
            TimeSpan timeSpan = new(0, 0, 0, 4);

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

        Thread? runningThread = internalThread;

        if (runningThread.IsAssigned() && runningThread.IsAlive)
        {
            runningThread.Join();
        }

        CleanupThread();
    }

    private void CleanupThread()
    {
        internalThread = null;
    }
}
