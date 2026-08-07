using System;
using System.Threading;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseThreadClasses.UnitTest;

[TestClass]
public class ThreadManagerTests
{
    [TestMethod]
    public void IsRunning_BeforeStart_ReturnsFalse()
    {
        // Arrange
        ThreadManager<NoOpThreadAction> manager = new();

        // Act
        bool result = manager.IsRunning;

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void SetInitializeAction_Always_ReturnsSameInstanceForChaining()
    {
        // Arrange
        ThreadManager<NoOpThreadAction> manager = new();

        // Act
        ThreadManager<NoOpThreadAction> result = manager.SetInitializeAction(_ => { });

        // Assert
        result.Should().BeSameAs(manager);
    }

    [TestMethod]
    public void SetBeforeExecuteAction_Always_ReturnsSameInstanceForChaining()
    {
        // Arrange
        ThreadManager<NoOpThreadAction> manager = new();

        // Act
        ThreadManager<NoOpThreadAction> result = manager.SetBeforeExecuteAction(_ => { });

        // Assert
        result.Should().BeSameAs(manager);
    }

    [TestMethod]
    public void SetAfterExecuteAction_Always_ReturnsSameInstanceForChaining()
    {
        // Arrange
        ThreadManager<NoOpThreadAction> manager = new();

        // Act
        ThreadManager<NoOpThreadAction> result = manager.SetAfterExecuteAction(_ => { });

        // Assert
        result.Should().BeSameAs(manager);
    }

    [TestMethod]
    public void InternalInitializeAction_WithNoActionSet_DoesNotThrow()
    {
        // Arrange
        ThreadManager<NoOpThreadAction> manager = new();
        NoOpThreadAction action = new();

        // Act
        Action act = () => manager.InternalInitializeAction(action);

        // Assert
        act.Should().NotThrow();
    }

    [TestMethod]
    public void InternalInitializeAction_WithActionSet_InvokesItWithTheGivenThreadAction()
    {
        // Arrange
        ThreadManager<NoOpThreadAction> manager = new();
        NoOpThreadAction action = new();
        NoOpThreadAction? received = null;
        manager.SetInitializeAction(threadAction => received = threadAction);

        // Act
        manager.InternalInitializeAction(action);

        // Assert
        received.Should().BeSameAs(action);
    }

    [TestMethod]
    public void InternalBeforeExecuteAction_WithNoActionSet_DoesNotThrow()
    {
        // Arrange
        ThreadManager<NoOpThreadAction> manager = new();
        NoOpThreadAction action = new();

        // Act
        Action act = () => manager.InternalBeforeExecuteAction(action);

        // Assert
        act.Should().NotThrow();
    }

    [TestMethod]
    public void InternalBeforeExecuteAction_WithActionSet_InvokesItWithTheGivenThreadAction()
    {
        // Arrange
        ThreadManager<NoOpThreadAction> manager = new();
        NoOpThreadAction action = new();
        NoOpThreadAction? received = null;
        manager.SetBeforeExecuteAction(threadAction => received = threadAction);

        // Act
        manager.InternalBeforeExecuteAction(action);

        // Assert
        received.Should().BeSameAs(action);
    }

    [TestMethod]
    public void InternalAfterExecuteAction_WithNoActionSet_DoesNotThrow()
    {
        // Arrange
        ThreadManager<NoOpThreadAction> manager = new();
        NoOpThreadAction action = new();

        // Act
        Action act = () => manager.InternalAfterExecuteAction(action);

        // Assert
        act.Should().NotThrow();
    }

    [TestMethod]
    public void InternalAfterExecuteAction_WithActionSet_InvokesItWithTheGivenThreadAction()
    {
        // Arrange
        ThreadManager<NoOpThreadAction> manager = new();
        NoOpThreadAction action = new();
        NoOpThreadAction? received = null;
        manager.SetAfterExecuteAction(threadAction => received = threadAction);

        // Act
        manager.InternalAfterExecuteAction(action);

        // Assert
        received.Should().BeSameAs(action);
    }

    [TestMethod]
    public void StartThread_ThenStopThread_TransitionsIsRunningCorrectly()
    {
        // Arrange
        ThreadManager<NoOpThreadAction> manager = new();

        // Act
        manager.StartThread(0);
        bool runningAfterStart = SpinWait.SpinUntil(() => manager.IsRunning, TimeSpan.FromSeconds(2));

        manager.StopThread();
        bool runningAfterStop = manager.IsRunning;

        // Assert
        runningAfterStart.Should().BeTrue();
        runningAfterStop.Should().BeFalse();
    }

    [TestMethod]
    public void StartThread_CalledTwiceWhileRunning_DoesNotThrow()
    {
        // Arrange
        ThreadManager<NoOpThreadAction> manager = new();
        manager.StartThread(0);
        SpinWait.SpinUntil(() => manager.IsRunning, TimeSpan.FromSeconds(2));

        try
        {
            // Act
            Action act = () => manager.StartThread(0);

            // Assert
            // Before relying on the IsRunning guard, calling Thread.Start() on an
            // already-started Thread instance throws ThreadStateException.
            act.Should().NotThrow();
        }
        finally
        {
            manager.StopThread();
        }
    }

    [TestMethod]
    public void StopThread_WhenNotRunning_DoesNotThrow()
    {
        // Arrange
        ThreadManager<NoOpThreadAction> manager = new();

        // Act
        Action act = () => manager.StopThread();

        // Assert
        act.Should().NotThrow();
    }

    private sealed class NoOpThreadAction : ThreadAction
    {
        protected override void Execute()
        {
        }
    }
}
