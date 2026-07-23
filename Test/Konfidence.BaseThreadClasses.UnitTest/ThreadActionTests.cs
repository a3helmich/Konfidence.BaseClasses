using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseThreadClasses.UnitTest;

[TestClass]
public class ThreadActionTests
{
    [TestMethod]
    public void ExecuteAction_TwoDifferentInstances_RunConcurrentlyWithoutSharedLock()
    {
        // Arrange
        BlockingThreadAction actionOne = new();
        BlockingThreadAction actionTwo = new();

        // Act
        Task taskOne = Task.Run(() => actionOne.ExecuteAction());
        bool oneEntered = actionOne.WaitUntilEntered(TimeSpan.FromSeconds(2));

        Task taskTwo = Task.Run(() => actionTwo.ExecuteAction());
        bool twoEnteredWhileOneStillRunning = actionTwo.WaitUntilEntered(TimeSpan.FromSeconds(2));

        actionOne.Release();
        actionTwo.Release();

        Task.WaitAll(taskOne, taskTwo);

        // Assert
        // Before the fix, ThreadAction.ExecuteAction() used a single lock shared by every
        // ThreadAction subclass, so actionTwo would block until actionOne released it and
        // twoEnteredWhileOneStillRunning would be false (the Wait would time out).
        oneEntered.Should().BeTrue();
        twoEnteredWhileOneStillRunning.Should().BeTrue();
    }

    private sealed class BlockingThreadAction : ThreadAction
    {
        private readonly ManualResetEventSlim _entered = new(false);
        private readonly ManualResetEventSlim _release = new(false);

        protected override void Execute()
        {
            _entered.Set();
            _release.Wait();
        }

        public bool WaitUntilEntered(TimeSpan timeout)
        {
            return _entered.Wait(timeout);
        }

        public void Release()
        {
            _release.Set();
        }
    }
}
