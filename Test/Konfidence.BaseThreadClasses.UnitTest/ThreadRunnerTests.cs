using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseThreadClasses.UnitTest;

[TestClass]
public class ThreadRunnerTests
{
    [TestMethod]
    public void StopThread_AfterActionThrows_CompletesWithoutHanging()
    {
        // Arrange
        ThreadManager<ThrowingThreadAction> threadManager = new();
        threadManager.StartThread(0);

        Thread.Sleep(200);

        // Act
        Task stopTask = Task.Run(() => threadManager.StopThread());
        bool completedInTime = stopTask.Wait(TimeSpan.FromSeconds(5));

        // Assert
        // Before the fix, ThreadAction.ExecuteAction() never reset IsAlive to false when Execute()
        // threw (the reset line ran after the lock block, which the exception skipped straight past),
        // so ThreadRunner.IsRunning never became false and StopThread()'s internal Join() blocked
        // forever.
        completedInTime.Should().BeTrue();
    }

    [TestMethod]
    public void StartThread_ActionThrows_LogsExceptionInsteadOfSwallowingSilently()
    {
        // Arrange
        TestTraceListener listener = new();
        Trace.Listeners.Add(listener);

        try
        {
            ThreadManager<ThrowingThreadAction> threadManager = new();

            // Act
            threadManager.StartThread(0);

            SpinWait.SpinUntil(() => listener.Messages.Any(message => message.Contains("Boom", StringComparison.Ordinal)), TimeSpan.FromSeconds(5));

            threadManager.StopThread();

            // Assert
            // Before the fix, the catch block around ThreadAction.ExecuteAction() in
            // InternalThreadLoop() silently swallowed every exception, leaving no trace at all.
            listener.Messages.Should().Contain(message => message.Contains("Boom", StringComparison.Ordinal));
        }
        finally
        {
            Trace.Listeners.Remove(listener);
        }
    }

    [TestMethod]
    [DataRow(SleepUnit.Daily)]
    [DataRow(SleepUnit.Hourly)]
    [DataRow(SleepUnit.Minutes)]
    [DataRow(SleepUnit.Seconds)]
    public void StartThread_WithZeroSleepTime_CompletesASleepCycleForEverySleepUnit(SleepUnit sleepUnit)
    {
        // Arrange
        // Only the Seconds arm of SleepThread's switch had ever executed. A sleepTime of 0 collapses
        // every unit to TimeSpan.Zero, so each arm runs without the test sleeping for days or hours.
        // The after-execute hook fires only once SleepThread has returned, which is exactly the
        // signal needed - no polling, and no dependence on how many iterations the loop manages.
        ThreadManager<NoOpThreadAction> threadManager = new();

        using ManualResetEventSlim sleepCompleted = new(false);

        threadManager.SetAfterExecuteAction(_ => sleepCompleted.Set());

        try
        {
            // Act
            threadManager.StartThread(0, sleepUnit);

            bool completedASleepCycle = sleepCompleted.Wait(TimeSpan.FromSeconds(30));

            // Assert
            completedASleepCycle.Should().BeTrue();
        }
        finally
        {
            threadManager.StopThread();
        }
    }

    [TestMethod]
    public void StartThread_WithUnmappedSleepUnit_FallsBackToAFourSecondSleep()
    {
        // Arrange
        // SleepUnit.Unknown matches no switch arm, so the loop silently keeps the "new(0, 0, 0, 4)"
        // seed value and sleeps four seconds between iterations - the requested sleepTime of 0 is
        // ignored entirely. An unmapped unit looks like "no delay" but is anything but.
        ThreadManager<NoOpThreadAction> threadManager = new();

        using ManualResetEventSlim sleepCompleted = new(false);

        threadManager.SetAfterExecuteAction(_ => sleepCompleted.Set());

        try
        {
            // Act
            threadManager.StartThread(0, SleepUnit.Unknown);

            bool completedWithinOneSecond = sleepCompleted.Wait(TimeSpan.FromSeconds(1));
            bool completedEventually = sleepCompleted.Wait(TimeSpan.FromSeconds(30));

            // Assert
            // A busy machine can only ever make the sleep finish later, never sooner, so the
            // "not yet after one second" half stays true under load.
            completedWithinOneSecond.Should().BeFalse();
            completedEventually.Should().BeTrue();
        }
        finally
        {
            threadManager.StopThread();
        }
    }

    [TestMethod]
    public void StopThread_ImmediatelyAfterStart_DoesNotThrowWhenWorkerClearsTheThreadField()
    {
        // Arrange
        // StopThreadRunner() and IsRunning both read the internalThread field twice - a null check
        // followed by a second, separate read. The worker thread nulls that same field in
        // CleanupThread() as it leaves the loop, so a stop that lands in between sees non-null on
        // the first read and null on the second, and dereferences null. A zero sleepTime makes the
        // worker exit fast enough to hit that window regularly.
        // The window between the two reads is only a couple of instructions wide, so the loop runs
        // on several threads at once - the resulting CPU contention is what makes the stopping
        // thread get preempted mid-check often enough to expose it.
        Action action = () => Parallel.For(0, 8, _ =>
        {
            for (int attempt = 0; attempt < 400; attempt++)
            {
                ThreadManager<NoOpThreadAction> threadManager = new();

                threadManager.StartThread(0);

                // StopThread() bails out early while IsRunning is still false, so the worker has to
                // be genuinely looping before the stop can reach the racy field reads at all.
                SpinWait.SpinUntil(() => threadManager.IsRunning, TimeSpan.FromSeconds(1));

                threadManager.StopThread();
            }
        });

        // Act & Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void StopThreadRunner_WhenNeverStarted_DoesNotThrow()
    {
        // Arrange
        // ThreadManager.StopThread() guards on IsRunning, so the runner's own "no thread yet"
        // branch is only reachable by driving the runner directly.
        ThreadManager<NoOpThreadAction> threadManager = new();
        ThreadRunner<NoOpThreadAction> threadRunner = new(threadManager);

        // Act
        Action action = () => threadRunner.StopThreadRunner();

        // Assert
        action.Should().NotThrow();
        threadRunner.IsRunning.Should().BeFalse();
    }

    private sealed class ThrowingThreadAction : ThreadAction
    {
        protected override void Execute()
        {
            throw new InvalidOperationException("Boom");
        }
    }

    private sealed class NoOpThreadAction : ThreadAction
    {
        protected override void Execute()
        {
        }
    }


    private sealed class TestTraceListener : TraceListener
    {
        private readonly ConcurrentQueue<string> _messages = new();

        public IReadOnlyList<string> Messages => [.. _messages];

        public override void Write(string? message)
        {
            if (message is not null)
            {
                _messages.Enqueue(message);
            }
        }

        public override void WriteLine(string? message)
        {
            if (message is not null)
            {
                _messages.Enqueue(message);
            }
        }
    }
}
