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
    public async Task DefaultIfCanceled_GenericTaskCanceledWithNoExplicitDefault_ReturnsDefaultOfT()
    {
        // Arrange
        // Confirms the "T defaultValue = default!" optional parameter actually works when the
        // caller omits it, not just when a value is passed explicitly.
        CancellationTokenSource cancellationTokenSource = new();
        cancellationTokenSource.Cancel();

        TaskCompletionSource<string?> taskCompletionSource = new();
        taskCompletionSource.SetCanceled(cancellationTokenSource.Token);

        // Act
        string? result = await taskCompletionSource.Task.DefaultIfCanceled();

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public async Task DefaultIfCanceled_GenericTaskFaulted_PropagatesException()
    {
        // Arrange
        TaskCompletionSource<int> taskCompletionSource = new();
        taskCompletionSource.SetException(new InvalidOperationException("boom"));

        // Act
        Func<Task> action = async () => await taskCompletionSource.Task.DefaultIfCanceled(-1);

        // Assert
        // t.IsCanceled is false for a faulted task, so the ternary falls through to t.Result,
        // which rethrows the original exception - only cancellation is swallowed here, not faults,
        // despite the method name suggesting broader failure tolerance.
        await action.Should().ThrowAsync<InvalidOperationException>();
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

    [TestMethod]
    public async Task DefaultIfCanceled_NonGenericTaskCanceled_CompletesWithoutThrowing()
    {
        // Arrange
        CancellationTokenSource cancellationTokenSource = new();
        cancellationTokenSource.Cancel();
        Task task = Task.FromCanceled(cancellationTokenSource.Token);

        // Act
        Func<Task> action = () => task.DefaultIfCanceled();

        // Assert
        await action.Should().NotThrowAsync();
    }

    [TestMethod]
    public async Task DefaultIfCanceled_NonGenericTaskFaulted_CompletesWithoutThrowing()
    {
        // Arrange
        // Unlike the generic overload (which rethrows on fault), this overload discards the
        // antecedent task entirely via the "_" parameter - it swallows both cancellation and
        // faults, not just cancellation.
        Task task = Task.FromException(new InvalidOperationException("boom"));

        // Act
        Func<Task> action = () => task.DefaultIfCanceled();

        // Assert
        await action.Should().NotThrowAsync();
    }
}
