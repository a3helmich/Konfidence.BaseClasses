using System;
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

    private sealed class ThrowingThreadAction : ThreadAction
    {
        protected override void Execute()
        {
            throw new InvalidOperationException("Boom");
        }
    }

    private sealed class TestTraceListener : TraceListener
    {
        private readonly List<string> _messages = [];

        public IReadOnlyList<string> Messages => _messages;

        public override void Write(string? message)
        {
            if (message is not null)
            {
                _messages.Add(message);
            }
        }

        public override void WriteLine(string? message)
        {
            if (message is not null)
            {
                _messages.Add(message);
            }
        }
    }
}
