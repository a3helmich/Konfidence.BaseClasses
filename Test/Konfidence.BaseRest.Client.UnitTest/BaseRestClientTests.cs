using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Konfidence.BaseRest.Client.UnitTest;

[TestClass]
public class BaseRestClientTests
{
    [TestMethod]
    public async Task GetAsync_AfterDispose_ThrowsObjectDisposedException()
    {
        // Arrange
        Mock<IRestClientConfig> clientConfigMock = new();
        clientConfigMock.Setup(x => x.BaseUri()).Returns(new Uri("http://127.0.0.1:1"));

        BaseRestClient client = new(clientConfigMock.Object);
        client.Dispose();

        // Act
        Func<Task> action = () => client.GetAsync<TestResponse>("test");

        // Assert
        // Before the fix, Dispose() never disposed the underlying RestClient, so calling GetAsync
        // after Dispose() would still attempt a real (failing) network call instead of throwing
        // ObjectDisposedException.
        await action.Should().ThrowAsync<ObjectDisposedException>();
    }

    [TestMethod]
    public async Task GetAsync_WithAlreadyCancelledToken_ThrowsInsteadOfReturningDefault()
    {
        // Arrange
        Mock<IRestClientConfig> clientConfigMock = new();
        clientConfigMock.Setup(x => x.BaseUri()).Returns(new Uri("http://127.0.0.1:1"));

        using BaseRestClient client = new(clientConfigMock.Object);
        using CancellationTokenSource cancellationTokenSource = new();
        cancellationTokenSource.Cancel();

        // Act
        Func<Task> action = () => client.GetAsync<TestResponse>("test", cancellationTokenSource.Token);

        // Assert
        // Before the fix, the CancellationToken was never passed through to RestClient.ExecuteAsync,
        // so a cancelled call would still try the real (failing) network request instead of being
        // cancelled up front.
        await action.Should().ThrowAsync<OperationCanceledException>();
    }

    private sealed class TestResponse
    {
        public string Value { get; set; } = string.Empty;
    }
}
