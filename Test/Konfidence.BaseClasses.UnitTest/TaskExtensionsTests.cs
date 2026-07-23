using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Konfidence.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseClasses.UnitTest;

[TestClass]
public class TaskExtensionsTests
{
    [TestMethod]
    public async Task DefaultIfCanceled_GenericTaskCompletesSuccessfully_ReturnsTaskResult()
    {
        // Arrange
        Task<int> task = Task.FromResult(42);

        // Act
        int result = await task.DefaultIfCanceled(-1);

        // Assert
        result.Should().Be(42);
    }

    [TestMethod]
    public async Task DefaultIfCanceled_GenericTaskIsCanceled_ReturnsDefaultValue()
    {
        // Arrange
        CancellationTokenSource cancellationTokenSource = new();
        cancellationTokenSource.Cancel();

        TaskCompletionSource<int> taskCompletionSource = new();
        taskCompletionSource.SetCanceled(cancellationTokenSource.Token);

        // Act
        int result = await taskCompletionSource.Task.DefaultIfCanceled(-1);

        // Assert
        result.Should().Be(-1);
    }

    [TestMethod]
    public async Task DefaultIfCanceled_NonGenericTask_CompletesWithoutThrowing()
    {
        // Arrange
        Task task = Task.CompletedTask;

        // Act
        Func<Task> action = () => task.DefaultIfCanceled();

        // Assert
        await action.Should().NotThrowAsync();
    }
}
