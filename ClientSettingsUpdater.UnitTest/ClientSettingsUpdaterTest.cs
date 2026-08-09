using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Konfidence.Base;
using Konfidence.Mail;
using Konfidence.SqlHostProvider;
using Konfidence.SqlHostProvider.SqlAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ClientSettingsUpdater.UnitTest;

[TestClass]
public class ClientSettingsUpdaterTest
{
    [TestMethod]
    public void Constructor_WithCredentialsAndMissingConfigFolder_ExitsWith1()
    {
        // Arrange
        Mock<IErrorExiter> errorExiterMock = new();

        // Act
        ClientSettingsManager clientSettingsManager = new([$"-{Argument.Server}=Server"], errorExiterMock.Object);

        // Assert
        errorExiterMock.Verify(x => x.Exit(1), Times.Once);

        clientSettingsManager.MailServer.Should().BeNullOrWhiteSpace();
        clientSettingsManager.Server.Should().BeNullOrWhiteSpace();

        clientSettingsManager.UserName.Should().BeNullOrWhiteSpace();
        clientSettingsManager.Password.Should().BeNullOrWhiteSpace();

        clientSettingsManager.ConfigFolder.Should().BeNullOrWhiteSpace();
        clientSettingsManager.ConfigFileName.Should().BeNullOrWhiteSpace();
    }

    [TestMethod]
    public void Constructor_WithConfigFolderAndMissingUsername_ExitsWith2()
    {
        // Arrange
        Mock<IErrorExiter> errorExiterMock = new();

        // Act
        ClientSettingsManager clientSettingsManager = new([$"--{Argument.ConfigFileFolder}=."], errorExiterMock.Object);

        // Assert
        errorExiterMock.Verify(x => x.Exit(2), Times.Once);

        clientSettingsManager.MailServer.Should().BeNullOrWhiteSpace();
        clientSettingsManager.Server.Should().BeNullOrWhiteSpace();

        clientSettingsManager.UserName.Should().BeNullOrWhiteSpace();
        clientSettingsManager.Password.Should().BeNullOrWhiteSpace();

        clientSettingsManager.ConfigFolder.Should().Be(".");
        clientSettingsManager.ConfigFileName.Should().BeNullOrWhiteSpace();
    }

    [TestMethod]
    public void Constructor_WithConfigFolderAndMissingPassword_ExitsWith3()
    {
        // Arrange
        Mock<IErrorExiter> errorExiterMock = new();

        // Act
        ClientSettingsManager clientSettingsManager = new([$"--{Argument.ConfigFileFolder}=.", $"--{Argument.UserName}=Adrie"], errorExiterMock.Object);

        // Assert
        errorExiterMock.Verify(x => x.Exit(3), Times.Once);

        clientSettingsManager.MailServer.Should().BeNullOrWhiteSpace();
        clientSettingsManager.Server.Should().BeNullOrWhiteSpace();

        clientSettingsManager.UserName.Should().Be("Adrie");
        clientSettingsManager.Password.Should().BeNullOrWhiteSpace();

        clientSettingsManager.ConfigFolder.Should().Be(".");
        clientSettingsManager.ConfigFileName.Should().BeNullOrWhiteSpace();
    }

    [TestMethod]
    public void Constructor_WithNoParameters_ExitsWith4()
    {
        // Arrange
        Mock<IErrorExiter> errorExiterMock = new();

        // Act
        ClientSettingsManager clientSettingsManager = new([], errorExiterMock.Object);

        // Assert
        errorExiterMock.Verify(x => x.Exit(4), Times.Once);

        clientSettingsManager.MailServer.Should().BeNullOrWhiteSpace();
        clientSettingsManager.Server.Should().BeNullOrWhiteSpace();

        clientSettingsManager.UserName.Should().BeNullOrWhiteSpace();
        clientSettingsManager.Password.Should().BeNullOrWhiteSpace();

        clientSettingsManager.ConfigFolder.Should().BeNullOrWhiteSpace();
        clientSettingsManager.ConfigFileName.Should().BeNullOrWhiteSpace();
    }

    [TestMethod]
    public void Constructor_WithConfigFolderAndCredentials_SetsConfigFolder()
    {
        // Arrange
        Mock<IErrorExiter> errorExiterMock = new();

        // Act
        ClientSettingsManager clientSettingsManager = new([$"--{Argument.ConfigFileFolder}=.", $"--{Argument.UserName}=Adrie", $"--{Argument.Password}=fake_password"], errorExiterMock.Object);

        // Assert
        errorExiterMock.Verify(x => x.Exit(It.IsAny<int>()), Times.Never);

        clientSettingsManager.MailServer.Should().BeNullOrWhiteSpace();
        clientSettingsManager.Server.Should().BeNullOrWhiteSpace();

        clientSettingsManager.UserName.Should().Be("Adrie");
        clientSettingsManager.Password.Should().Be("fake_password");

        clientSettingsManager.ConfigFolder.Should().Be(".");
        clientSettingsManager.ConfigFileName.Should().Be("SqlClientSettings.json");
    }

    [TestMethod]
    public void Constructor_WithConfigFileNameAndRequiredFields_SetsConfigFileName()
    {
        // Arrange
        Mock<IErrorExiter> errorExiterMock = new();

        // Act
        ClientSettingsManager clientSettingsManager = new([$"--{Argument.ConfigFileFolder}=.", $"--{Argument.ConfigFileName}=test.json", $"--{Argument.UserName}=Adrie", $"--{Argument.Password}=fake_password"], errorExiterMock.Object);

        // Assert
        errorExiterMock.Verify(x => x.Exit(It.IsAny<int>()), Times.Never);

        clientSettingsManager.MailServer.Should().BeNullOrWhiteSpace();
        clientSettingsManager.Server.Should().BeNullOrWhiteSpace();

        clientSettingsManager.UserName.Should().Be("Adrie");
        clientSettingsManager.Password.Should().Be("fake_password");

        clientSettingsManager.ConfigFolder.Should().Be(".");
        clientSettingsManager.ConfigFileName.Should().Be("test.json");
    }

    [TestMethod]
    public void Constructor_WithServerAndRequiredFields_SetsServerConfig()
    {
        // Arrange
        Mock<IErrorExiter> errorExiterMock = new();

        // Act
        ClientSettingsManager clientSettingsManager = new([$"--{Argument.ConfigFileFolder}=.", $"--{Argument.Server}=server", $"--{Argument.UserName}=Adrie", $"--{Argument.Password}=fake_password"], errorExiterMock.Object);

        // Assert
        errorExiterMock.Verify(x => x.Exit(It.IsAny<int>()), Times.Never);

        clientSettingsManager.MailServer.Should().BeNullOrWhiteSpace();
        clientSettingsManager.Server.Should().Be("server");

        clientSettingsManager.UserName.Should().Be("Adrie");
        clientSettingsManager.Password.Should().Be("fake_password");

        clientSettingsManager.ConfigFolder.Should().Be(".");
        clientSettingsManager.ConfigFileName.Should().Be("SqlClientSettings.json");
    }

    [TestMethod]
    public void Constructor_WithMailServerAndRequiredFields_SetsMailConfigFile()
    {
        // Arrange
        Mock<IErrorExiter> errorExiterMock = new();

        // Act
        ClientSettingsManager clientSettingsManager = new([
            $"--{Argument.ConfigFileFolder}=.",
            $"--{Argument.MailServer}=mailserver",
            $"--{Argument.UserName}=Adrie",
            $"--{Argument.Password}=fake_password",
            $"--{Argument.Verbose}=verbose"
        ], errorExiterMock.Object);

        // Assert
        errorExiterMock.Verify(x => x.Exit(It.IsAny<int>()), Times.Never);

        clientSettingsManager.MailServer.Should().Be("mailserver");
        clientSettingsManager.Server.Should().BeNullOrWhiteSpace();

        clientSettingsManager.UserName.Should().Be("Adrie");
        clientSettingsManager.Password.Should().Be("fake_password");

        clientSettingsManager.ConfigFolder.Should().Be(".");
        clientSettingsManager.ConfigFileName.Should().Be("MailClientSettings.json");
    }

    [TestMethod]
    public void Constructor_WithVerboseArgument_WritesVerboseDebugOutput()
    {
        // Arrange
        Mock<IErrorExiter> errorExiterMock = new();
        TextWriter originalOut = Console.Out;
        StringWriter capturedOut = new();
        Console.SetOut(capturedOut);

        try
        {
            // Act
            _ = new ClientSettingsManager([$"--{Argument.ConfigFileFolder}=.", $"--{Argument.UserName}=Adrie", $"--{Argument.Password}=fake_password", $"--{Argument.Verbose}=true"], errorExiterMock.Object);
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        // Assert
        capturedOut.ToString().Should().Contain("configFolder:");
    }

    [TestMethod]
    public void Constructor_WithoutVerboseArgument_DoesNotWriteVerboseDebugOutput()
    {
        // Arrange
        Mock<IErrorExiter> errorExiterMock = new();
        TextWriter originalOut = Console.Out;
        StringWriter capturedOut = new();
        Console.SetOut(capturedOut);

        try
        {
            // Act
            _ = new ClientSettingsManager([$"--{Argument.ConfigFileFolder}=.", $"--{Argument.UserName}=Adrie", $"--{Argument.Password}=fake_password"], errorExiterMock.Object);
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        // Assert
        capturedOut.ToString().Should().NotContain("configFolder:");
    }

    [TestMethod]
    public void Constructor_WithWhitespaceOnlyVerboseValue_DoesNotWriteVerboseDebugOutput()
    {
        // Arrange
        // A whitespace-only value still lets TryParseArgument find the Verbose argument, so the
        // IsNullOrWhiteSpace check on the parsed value - not just the presence of the argument -
        // is what has to decide here.
        Mock<IErrorExiter> errorExiterMock = new();
        TextWriter originalOut = Console.Out;
        StringWriter capturedOut = new();
        Console.SetOut(capturedOut);

        try
        {
            // Act
            _ = new ClientSettingsManager([$"--{Argument.ConfigFileFolder}=.", $"--{Argument.UserName}=Adrie", $"--{Argument.Password}=fake_password", $"--{Argument.Verbose}=   "], errorExiterMock.Object);
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        // Assert
        capturedOut.ToString().Should().NotContain("configFolder:");
    }

    [TestMethod]
    public void Execute_WithSqlConnectionMissingCredentials_SetsUserNameAndPassword()
    {
        // Arrange
        string folder = Path.Combine(Path.GetTempPath(), $"ClientSettingsTest_{Guid.NewGuid():N}");
        Directory.CreateDirectory(folder);

        try
        {
            const string fileName = "config.json";
            string filePath = WriteClientSettingsFile(folder, fileName, new ConfigConnectionString { ConnectionName = "TestDb", Server = "konfidence2" });

            Mock<IErrorExiter> errorExiterMock = new();
            ClientSettingsManager clientSettingsManager = new(
                [$"--{Argument.ConfigFileFolder}={folder}", $"--{Argument.ConfigFileName}={fileName}", $"--{Argument.UserName}=Adrie", $"--{Argument.Password}=fake_password"],
                errorExiterMock.Object);

            // Act
            clientSettingsManager.Execute();

            // Assert
            errorExiterMock.Verify(x => x.Exit(It.IsAny<int>()), Times.Never);

            File.ReadAllText(filePath).Deserialize(out ClientSettings? updated);
            ConfigConnectionString connection = updated!.DataConfiguration!.Connections.Single();
            connection.UserName.Should().Be("Adrie");
            connection.Password.Should().Be("fake_password");
        }
        finally
        {
            Directory.Delete(folder, true);
        }
    }

    [TestMethod]
    public void Execute_WithSqlConnectionAlreadyHavingUserName_LeavesConnectionUnchanged()
    {
        // Arrange
        string folder = Path.Combine(Path.GetTempPath(), $"ClientSettingsTest_{Guid.NewGuid():N}");
        Directory.CreateDirectory(folder);

        try
        {
            const string fileName = "config.json";
            string filePath = WriteClientSettingsFile(folder, fileName, new ConfigConnectionString
            {
                ConnectionName = "TestDb",
                Server = "konfidence2",
                UserName = "ExistingUser",
                Password = "ExistingPassword"
            });

            Mock<IErrorExiter> errorExiterMock = new();
            ClientSettingsManager clientSettingsManager = new(
                [$"--{Argument.ConfigFileFolder}={folder}", $"--{Argument.ConfigFileName}={fileName}", $"--{Argument.UserName}=Adrie", $"--{Argument.Password}=fake_password"],
                errorExiterMock.Object);

            // Act
            clientSettingsManager.Execute();

            // Assert
            File.ReadAllText(filePath).Deserialize(out ClientSettings? updated);
            ConfigConnectionString connection = updated!.DataConfiguration!.Connections.Single();
            connection.UserName.Should().Be("ExistingUser");
            connection.Password.Should().Be("ExistingPassword");
        }
        finally
        {
            Directory.Delete(folder, true);
        }
    }

    [TestMethod]
    public void Execute_WithServerFilterNotMatchingConnection_LeavesConnectionUnchanged()
    {
        // Arrange
        string folder = Path.Combine(Path.GetTempPath(), $"ClientSettingsTest_{Guid.NewGuid():N}");
        Directory.CreateDirectory(folder);

        try
        {
            const string fileName = "config.json";
            string filePath = WriteClientSettingsFile(folder, fileName, new ConfigConnectionString { ConnectionName = "TestDb", Server = "konfidence2" });

            Mock<IErrorExiter> errorExiterMock = new();
            ClientSettingsManager clientSettingsManager = new(
                [$"--{Argument.ConfigFileFolder}={folder}", $"--{Argument.ConfigFileName}={fileName}", $"--{Argument.Server}=konfidence3", $"--{Argument.UserName}=Adrie", $"--{Argument.Password}=fake_password"],
                errorExiterMock.Object);

            // Act
            clientSettingsManager.Execute();

            // Assert
            File.ReadAllText(filePath).Deserialize(out ClientSettings? updated);
            ConfigConnectionString connection = updated!.DataConfiguration!.Connections.Single();
            connection.UserName.Should().BeEmpty();
            connection.Password.Should().BeEmpty();
        }
        finally
        {
            Directory.Delete(folder, true);
        }
    }

    [TestMethod]
    public void Execute_ReRunningForSameMailAccount_DoesNotDuplicateAndLeavesPasswordUnchanged()
    {
        // Arrange
        // The ForEach's "already has UserName -> skip" guard means the second run must leave the
        // existing account's password untouched (still "first_password") - and the subsequent
        // ".All(x => x.UserName != UserName)" check must then find the account already exists, so
        // no duplicate gets appended either. Neither branch is exercised by a single Execute() call.
        string uniqueUserName = $"TestUser_{Guid.NewGuid():N}";
        Mock<IErrorExiter> errorExiterMock = new();

        ClientSettingsManager first = new(
            [$"--{Argument.ConfigFileFolder}=.", $"--{Argument.MailServer}=mail.konfidence.nl", $"--{Argument.UserName}={uniqueUserName}", $"--{Argument.Password}=first_password"],
            errorExiterMock.Object);
        first.Execute();

        // Act
        ClientSettingsManager second = new(
            [$"--{Argument.ConfigFileFolder}=.", $"--{Argument.MailServer}=mail.konfidence.nl", $"--{Argument.UserName}={uniqueUserName}", $"--{Argument.Password}=second_password"],
            errorExiterMock.Object);
        second.Execute();

        // Assert
        MailAccounts? mailConfig = ReadMailConfig();
        mailConfig.Should().NotBeNull();

        List<MailAccount> matchingAccounts = mailConfig!.Accounts.Where(x => x.UserName == uniqueUserName).ToList();
        matchingAccounts.Should().ContainSingle();
        matchingAccounts[0].Password.Should().Be("first_password");
    }

    [TestMethod]
    public void Execute_WithMailServerFilterNotMatchingExistingAccount_LeavesAccountUnchanged()
    {
        // Arrange
        string folder = Path.Combine(Path.GetTempPath(), $"ClientSettingsTest_{Guid.NewGuid():N}");
        Directory.CreateDirectory(folder);

        try
        {
            const string fileName = "mail.json";
            string filePath = Path.Combine(folder, fileName);

            MailAccounts initialAccounts = new()
            {
                Accounts = [new MailAccount { Server = "other.mailserver.nl" }]
            };

            File.WriteAllText(filePath, initialAccounts.Serialize());

            Mock<IErrorExiter> errorExiterMock = new();
            ClientSettingsManager clientSettingsManager = new(
                [$"--{Argument.ConfigFileFolder}={folder}", $"--{Argument.ConfigFileName}={fileName}", $"--{Argument.MailServer}=mail.konfidence.nl", $"--{Argument.UserName}=Adrie", $"--{Argument.Password}=fake_password"],
                errorExiterMock.Object);

            // Act
            clientSettingsManager.Execute();

            // Assert
            File.ReadAllText(filePath).Deserialize(out MailAccounts? updated);

            // The mismatched-server account is left untouched...
            MailAccount originalAccount = updated!.Accounts.Single(x => x.Server == "other.mailserver.nl");
            originalAccount.UserName.Should().BeEmpty();

            // ...but since no account matches "mail.konfidence.nl" yet, a new one is appended for it.
            MailAccount newAccount = updated.Accounts.Single(x => x.Server == "mail.konfidence.nl");
            newAccount.UserName.Should().Be("Adrie");
        }
        finally
        {
            Directory.Delete(folder, true);
        }
    }

    [TestMethod]
    public void Execute_WithTwoMailAccounts_WritesBothAccountsToMailConfig()
    {
        // Arrange
        Mock<IErrorExiter> errorExiterMock = new();
        ClientSettingsManager clientSettingsManager = new([$"--{Argument.ConfigFileFolder}=.", $"--{Argument.MailServer}=mail.konfidence.nl", $"--{Argument.UserName}=Adrie", $"--{Argument.Password}=fake_password"], errorExiterMock.Object);
        clientSettingsManager.Execute();

        clientSettingsManager = new ClientSettingsManager([$"--{Argument.ConfigFileFolder}=.", $"--{Argument.MailServer}=mail.konfidence.nl", $"--{Argument.UserName}=A3", $"--{Argument.Password}=fake_password"], errorExiterMock.Object);

        // Act
        clientSettingsManager.Execute();
        MailAccounts? mailConfig = ReadMailConfig();

        // Assert
        errorExiterMock.Verify(x => x.Exit(It.IsAny<int>()), Times.Never);

        mailConfig.Should().NotBeNull();

        mailConfig!.Accounts.Should().HaveCountGreaterThanOrEqualTo(2);
        MailAccount? account1 = mailConfig.Accounts.FirstOrDefault(x => x.UserName == "Adrie");
        MailAccount? account2 = mailConfig.Accounts.FirstOrDefault(x => x.UserName == "A3");

        account1.Should().NotBeNull();
        account1?.Server.Should().Be("mail.konfidence.nl");
        account1?.UserName.Should().Be("Adrie");
        account1?.Password.Should().Be("fake_password");

        account2.Should().NotBeNull();
        account2?.Server.Should().Be("mail.konfidence.nl");
        account2?.UserName.Should().Be("A3");
        account2?.Password.Should().Be("fake_password");
    }

    [TestMethod]
    public void Execute_WithUnparsableSqlConnectionConfig_LeavesFileUnchanged()
    {
        // Arrange
        // UpdateFile()'s Deserialize() failure branch (return before touching any connection) is
        // otherwise unreachable - every other Execute() test relies on a config file that parses
        // successfully.
        string folder = Path.Combine(Path.GetTempPath(), $"ClientSettingsTest_{Guid.NewGuid():N}");
        Directory.CreateDirectory(folder);

        try
        {
            const string fileName = "config.json";
            string filePath = Path.Combine(folder, fileName);
            File.WriteAllText(filePath, "{ not valid json");

            Mock<IErrorExiter> errorExiterMock = new();
            ClientSettingsManager clientSettingsManager = new(
                [$"--{Argument.ConfigFileFolder}={folder}", $"--{Argument.ConfigFileName}={fileName}", $"--{Argument.UserName}=Adrie", $"--{Argument.Password}=fake_password"],
                errorExiterMock.Object);

            // Act
            Action action = () => clientSettingsManager.Execute();

            // Assert
            action.Should().NotThrow();
            errorExiterMock.Verify(x => x.Exit(It.IsAny<int>()), Times.Never);
            File.ReadAllText(filePath).Should().Be("{ not valid json");
        }
        finally
        {
            Directory.Delete(folder, true);
        }
    }

    [TestMethod]
    public void Execute_WithUnparsableMailServerConfig_LeavesFileUnchanged()
    {
        // Arrange
        // UpdateMailServerFile()'s Deserialize() failure branch (return before touching any
        // account) is otherwise unreachable - every other mail-server Execute() test relies on a
        // config file that parses successfully.
        string folder = Path.Combine(Path.GetTempPath(), $"ClientSettingsTest_{Guid.NewGuid():N}");
        Directory.CreateDirectory(folder);

        try
        {
            const string fileName = "mail.json";
            string filePath = Path.Combine(folder, fileName);
            File.WriteAllText(filePath, "{ not valid json");

            Mock<IErrorExiter> errorExiterMock = new();
            ClientSettingsManager clientSettingsManager = new(
                [$"--{Argument.ConfigFileFolder}={folder}", $"--{Argument.ConfigFileName}={fileName}", $"--{Argument.MailServer}=mail.konfidence.nl", $"--{Argument.UserName}=Adrie", $"--{Argument.Password}=fake_password"],
                errorExiterMock.Object);

            // Act
            Action action = () => clientSettingsManager.Execute();

            // Assert
            action.Should().NotThrow();
            errorExiterMock.Verify(x => x.Exit(It.IsAny<int>()), Times.Never);
            File.ReadAllText(filePath).Should().Be("{ not valid json");
        }
        finally
        {
            Directory.Delete(folder, true);
        }
    }

    [TestMethod]
    public void Execute_WithNullDataConfiguration_DoesNotThrow()
    {
        // Arrange
        // UpdateFile() reaches the connections through a null-conditional
        // (clientSettings.DataConfiguration?.Connections.ForEach(...)) - a config file that
        // deserializes successfully but carries no DataConfiguration section at all is the only
        // way to exercise that null-skip branch, distinct from the Deserialize()-failure path.
        string folder = Path.Combine(Path.GetTempPath(), $"ClientSettingsTest_{Guid.NewGuid():N}");
        Directory.CreateDirectory(folder);

        try
        {
            const string fileName = "config.json";
            string filePath = Path.Combine(folder, fileName);
            File.WriteAllText(filePath, new ClientSettings().Serialize());

            Mock<IErrorExiter> errorExiterMock = new();
            ClientSettingsManager clientSettingsManager = new(
                [$"--{Argument.ConfigFileFolder}={folder}", $"--{Argument.ConfigFileName}={fileName}", $"--{Argument.UserName}=Adrie", $"--{Argument.Password}=fake_password"],
                errorExiterMock.Object);

            // Act
            Action action = () => clientSettingsManager.Execute();

            // Assert
            action.Should().NotThrow();
            errorExiterMock.Verify(x => x.Exit(It.IsAny<int>()), Times.Never);
        }
        finally
        {
            Directory.Delete(folder, true);
        }
    }

    [TestMethod]
    public void Execute_WithNonExistentConfigFolder_ExitsWith6AndDoesNotThrow()
    {
        // Arrange
        // The mocked IErrorExiter doesn't actually terminate the process (unlike the real
        // Environment.Exit), so a missing `return;` after Exit(6) would let Execute() keep
        // running and hit a real DirectoryNotFoundException when it tries to enumerate the
        // non-existent folder.
        Mock<IErrorExiter> errorExiterMock = new();
        string nonExistentFolder = Path.Combine(Path.GetTempPath(), $"NonExistentFolder_{Guid.NewGuid():N}");
        ClientSettingsManager clientSettingsManager = new([$"--{Argument.ConfigFileFolder}={nonExistentFolder}", $"--{Argument.UserName}=Adrie", $"--{Argument.Password}=fake_password"], errorExiterMock.Object);

        // Act
        Action action = () => clientSettingsManager.Execute();

        // Assert
        action.Should().NotThrow();
        errorExiterMock.Verify(x => x.Exit(6), Times.Once);
    }

    [TestMethod]
    public void Execute_WithNoMatchingConfigFile_ExitsWith7()
    {
        // Arrange
        Mock<IErrorExiter> errorExiterMock = new();
        string emptyFolder = Path.Combine(Path.GetTempPath(), $"EmptyFolder_{Guid.NewGuid():N}");
        Directory.CreateDirectory(emptyFolder);

        try
        {
            ClientSettingsManager clientSettingsManager = new([$"--{Argument.ConfigFileFolder}={emptyFolder}", $"--{Argument.ConfigFileName}=DoesNotExist.json", $"--{Argument.UserName}=Adrie", $"--{Argument.Password}=fake_password"], errorExiterMock.Object);

            // Act
            clientSettingsManager.Execute();

            // Assert
            errorExiterMock.Verify(x => x.Exit(7), Times.Once);
        }
        finally
        {
            Directory.Delete(emptyFolder);
        }
    }

    private static MailAccounts? ReadMailConfig()
    {
        return File.ReadAllText(MailConstants.DefaultMailServerConfigFileName).Deserialize(out MailAccounts? mailAccounts)
            ? mailAccounts
            : null;
    }

    private static string WriteClientSettingsFile(string folder, string fileName, ConfigConnectionString connection)
    {
        ClientSettings clientSettings = new()
        {
            DataConfiguration = new DataConfiguration { Connections = [connection] }
        };

        string filePath = Path.Combine(folder, fileName);

        File.WriteAllText(filePath, clientSettings.Serialize());

        return filePath;
    }
}
