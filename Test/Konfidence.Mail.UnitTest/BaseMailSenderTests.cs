using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.Mail.UnitTest;

[TestClass]
public class BaseMailSenderTests
{
    [TestMethod]
    public void SendEmail_WithNonExistentAttachmentFile_ReturnsFalseInsteadOfThrowing()
    {
        // Arrange
        BaseMailSender mailSender = new("from@example.com", "smtp.invalid.example", "user", "password");
        string nonExistentFile = Path.Combine(Path.GetTempPath(), $"DoesNotExist_{Guid.NewGuid():N}.txt");

        // Act
        bool result = false;
        Action action = () => result = mailSender.SendEmail("to@example.com", "subject", "body", false, nonExistentFile);

        // Assert
        // Before the fix, constructing the Attachment happened before the try block, so a bad
        // attachment path threw straight out of SendEmail instead of returning false like every
        // other failure mode.
        action.Should().NotThrow();
        result.Should().BeFalse();
    }

    [TestMethod]
    public void SendEmail_ThreeArgOverload_DelegatesAndSendsSuccessfully()
    {
        // Arrange
        using FakeSmtpServer server = FakeSmtpServer.Start();
        BaseMailSender mailSender = new("from@example.com", "127.0.0.1", "user", "password", server.Port);

        // Act
        bool result = mailSender.SendEmail("to@example.com", "subject", "body");

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void SendEmail_WithAttachment_SendsSuccessfully()
    {
        // Arrange
        using FakeSmtpServer server = FakeSmtpServer.Start();
        BaseMailSender mailSender = new("from@example.com", "127.0.0.1", "user", "password", server.Port);

        string attachmentFile = Path.Combine(Path.GetTempPath(), $"Attachment_{Guid.NewGuid():N}.txt");
        File.WriteAllText(attachmentFile, "attachment content");

        try
        {
            // Act
            bool result = mailSender.SendEmail("to@example.com", "subject", "body", false, attachmentFile);

            // Assert
            result.Should().BeTrue();
        }
        finally
        {
            File.Delete(attachmentFile);
        }
    }

    [TestMethod]
    public void SendEmail_WithNoListenerOnTargetPort_ReturnsFalse()
    {
        // Arrange
        // Unlike the attachment-file test above (which fails before ever reaching
        // smtpClient.Send()), this exercises the catch triggered by Send() itself.
        BaseMailSender mailSender = new("from@example.com", "127.0.0.1", "user", "password", GetFreeTcpPort());

        // Act
        bool result = mailSender.SendEmail("to@example.com", "subject", "body");

        // Assert
        result.Should().BeFalse();
    }

    private static int GetFreeTcpPort()
    {
        TcpListener tcpListener = new(IPAddress.Loopback, 0);

        tcpListener.Start();

        int port = ((IPEndPoint)tcpListener.LocalEndpoint).Port;

        tcpListener.Stop();

        return port;
    }

    private sealed class FakeSmtpServer : IDisposable
    {
        private readonly TcpListener _listener;
        private readonly Task _serverTask;

        private FakeSmtpServer(TcpListener listener, Task serverTask, int port)
        {
            _listener = listener;
            _serverTask = serverTask;
            Port = port;
        }

        public int Port { get; }

        public static FakeSmtpServer Start()
        {
            TcpListener listener = new(IPAddress.Loopback, 0);
            listener.Start();

            int port = ((IPEndPoint)listener.LocalEndpoint).Port;

            Task serverTask = Task.Run(async () =>
            {
                using TcpClient client = await listener.AcceptTcpClientAsync();
                using NetworkStream stream = client.GetStream();
                using StreamReader reader = new(stream, Encoding.ASCII);
                using StreamWriter writer = new(stream, Encoding.ASCII) { AutoFlush = true, NewLine = "\r\n" };

                await writer.WriteLineAsync("220 localhost SMTP ready");

                bool inData = false;

                while (true)
                {
                    string? line = await reader.ReadLineAsync();

                    if (line is null)
                    {
                        break;
                    }

                    if (inData)
                    {
                        if (line == ".")
                        {
                            inData = false;

                            await writer.WriteLineAsync("250 OK");
                        }

                        continue;
                    }

                    if (line.StartsWith("DATA", StringComparison.OrdinalIgnoreCase))
                    {
                        inData = true;

                        await writer.WriteLineAsync("354 Start mail input");
                    }
                    else if (line.StartsWith("QUIT", StringComparison.OrdinalIgnoreCase))
                    {
                        await writer.WriteLineAsync("221 Bye");

                        break;
                    }
                    else
                    {
                        await writer.WriteLineAsync("250 OK");
                    }
                }
            });

            return new FakeSmtpServer(listener, serverTask, port);
        }

        public void Dispose()
        {
            _serverTask.Wait(TimeSpan.FromSeconds(5));

            _listener.Stop();
        }
    }
}
