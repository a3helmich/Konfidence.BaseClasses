using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
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
    public async Task GetAsync_WithSuccessfulResponse_DeserializesJsonContent()
    {
        // Arrange
        using TestHttpServer server = TestHttpServer.Start(context =>
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;

            return """{"value":"hello"}""";
        });

        Mock<IRestClientConfig> clientConfigMock = new();
        clientConfigMock.Setup(x => x.BaseUri()).Returns(server.BaseUri);

        using BaseRestClient client = new(clientConfigMock.Object);

        // Act
        TestResponse? result = await client.GetAsync<TestResponse>("test");

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().Be("hello");
    }

    [TestMethod]
    public async Task PostAsync_WithSuccessfulResponse_SendsBodyAndHeaderAndDeserializesResponse()
    {
        // Arrange
        string? receivedHeaderValue = null;
        string? receivedBody = null;

        using TestHttpServer server = TestHttpServer.Start(context =>
        {
            receivedHeaderValue = context.Request.Headers["X-Test-Header"];

            using System.IO.StreamReader reader = new(context.Request.InputStream, Encoding.UTF8);
            receivedBody = reader.ReadToEnd();

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;

            return """{"value":"posted"}""";
        });

        Mock<IRestClientConfig> clientConfigMock = new();
        clientConfigMock.Setup(x => x.BaseUri()).Returns(server.BaseUri);

        using BaseRestClient client = new(clientConfigMock.Object);
        Dictionary<string, string> headers = new() { ["X-Test-Header"] = "header-value" };

        // Act
        TestResponse? result = await client.PostAsync<TestResponse>("test", new TestRequest { Name = "input" }, headers);

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().Be("posted");
        receivedHeaderValue.Should().Be("header-value");
        receivedBody.Should().Contain("\"name\":\"input\"");
    }

    [TestMethod]
    public async Task GetAsync_WithNotFoundAndNoContent_ReturnsDefault()
    {
        // Arrange
        using TestHttpServer server = TestHttpServer.Start(context =>
        {
            context.Response.StatusCode = 404;

            return string.Empty;
        });

        Mock<IRestClientConfig> clientConfigMock = new();
        clientConfigMock.Setup(x => x.BaseUri()).Returns(server.BaseUri);

        using BaseRestClient client = new(clientConfigMock.Object);

        // Act
        TestResponse? result = await client.GetAsync<TestResponse>("test");

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetAsync_WithOkStatusAndEmptyBody_ThrowsJsonException()
    {
        // Arrange
        // The "no content" early return only fires when the status is *not* OK, so a 200 with an
        // empty body falls through to JsonSerializer.Deserialize and fails there instead of
        // returning default like the 404-with-no-content case does.
        using TestHttpServer server = TestHttpServer.Start(context =>
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;

            return string.Empty;
        });

        Mock<IRestClientConfig> clientConfigMock = new();
        clientConfigMock.Setup(x => x.BaseUri()).Returns(server.BaseUri);

        using BaseRestClient client = new(clientConfigMock.Object);

        // Act
        Func<Task> action = () => client.GetAsync<TestResponse>("test");

        // Assert
        await action.Should().ThrowAsync<JsonException>();
    }

    [TestMethod]
    public async Task GetAsync_WithNotFoundButJsonBody_StillDeserializesTheBody()
    {
        // Arrange
        // The early return needs *both* an unassigned body and a non-OK status, so an error status
        // that carries a payload is still deserialized into T rather than returning default.
        using TestHttpServer server = TestHttpServer.Start(context =>
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 404;

            return """{"value":"not found"}""";
        });

        Mock<IRestClientConfig> clientConfigMock = new();
        clientConfigMock.Setup(x => x.BaseUri()).Returns(server.BaseUri);

        using BaseRestClient client = new(clientConfigMock.Object);

        // Act
        TestResponse? result = await client.GetAsync<TestResponse>("test");

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().Be("not found");
    }

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

    private sealed class TestRequest
    {
        public string Name { get; set; } = string.Empty;
    }

    private sealed class TestHttpServer : IDisposable
    {
        private readonly HttpListener _listener;
        private readonly Task _serverTask;

        private TestHttpServer(HttpListener listener, Task serverTask, Uri baseUri)
        {
            _listener = listener;
            _serverTask = serverTask;
            BaseUri = baseUri;
        }

        public Uri BaseUri { get; }

        public static TestHttpServer Start(Func<HttpListenerContext, string> handleRequest)
        {
            int port = GetFreeTcpPort();
            Uri baseUri = new($"http://127.0.0.1:{port}/");

            HttpListener listener = new();
            listener.Prefixes.Add(baseUri.ToString());
            listener.Start();

            Task serverTask = Task.Run(async () =>
            {
                HttpListenerContext context = await listener.GetContextAsync();

                string responseBody = handleRequest(context);

                byte[] responseBytes = Encoding.UTF8.GetBytes(responseBody);

                await context.Response.OutputStream.WriteAsync(responseBytes);

                context.Response.Close();
            });

            return new TestHttpServer(listener, serverTask, baseUri);
        }

        public void Dispose()
        {
            _serverTask.Wait(TimeSpan.FromSeconds(5));

            _listener.Close();
        }

        private static int GetFreeTcpPort()
        {
            TcpListener tcpListener = new(IPAddress.Loopback, 0);

            tcpListener.Start();

            int port = ((IPEndPoint)tcpListener.LocalEndpoint).Port;

            tcpListener.Stop();

            return port;
        }
    }
}
