using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Konfidence.Base;
using Konfidence.Mail;
using Konfidence.SqlHostProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ClientSettingsUpdater.Tests
{
    [TestClass]
    public class ClientSettingsUpdaterTest
    {
        [TestMethod]
        public void WhenExecute_WithCredentialsAndMissingConfigFolder_ShouldExitWith_1()
        {
            // arrange
            Mock<IErrorExiter> errorExiterMock = new();

            // act
            ClientSettingsManager clientSettingsManager = new(new[] { $"-{Argument.Server}=Server" }, errorExiterMock.Object);

            // assert
            errorExiterMock.Verify(x => x.Exit(1), Times.Once);

            clientSettingsManager.MailServer.Should().BeNullOrWhiteSpace();
            clientSettingsManager.Server.Should().BeNullOrWhiteSpace();

            clientSettingsManager.UserName.Should().BeNullOrWhiteSpace();
            clientSettingsManager.Password.Should().BeNullOrWhiteSpace();

            clientSettingsManager.ConfigFolder.Should().BeNullOrWhiteSpace();
            clientSettingsManager.ConfigFileName.Should().BeNullOrWhiteSpace();
        }

        [TestMethod]
        public void WhenExecute_WithConfigFolderAndMissingUsername_ShouldExitWith_2()
        {
            // arrange
            Mock<IErrorExiter> errorExiterMock = new();

            // act
            ClientSettingsManager clientSettingsManager = new(new[] { $"--{Argument.ConfigFileFolder}=." }, errorExiterMock.Object);

            // assert
            errorExiterMock.Verify(x => x.Exit(2), Times.Once);

            clientSettingsManager.MailServer.Should().BeNullOrWhiteSpace();
            clientSettingsManager.Server.Should().BeNullOrWhiteSpace();
            
            clientSettingsManager.UserName.Should().BeNullOrWhiteSpace();
            clientSettingsManager.Password.Should().BeNullOrWhiteSpace();

            clientSettingsManager.ConfigFolder.Should().Be(@".");
            clientSettingsManager.ConfigFileName.Should().BeNullOrWhiteSpace();
        }

        [TestMethod]
        public void WhenExecute_WithConfigFolderAndMissingPassword_ShouldExitWith_3()
        {
            // arrange
            Mock<IErrorExiter> errorExiterMock = new();

            // act
            ClientSettingsManager clientSettingsManager = new(new[] { $"--{Argument.ConfigFileFolder}=.", $"--{Argument.UserName}=Adrie" }, errorExiterMock.Object);

            // assert
            errorExiterMock.Verify(x => x.Exit(3), Times.Once);

            clientSettingsManager.MailServer.Should().BeNullOrWhiteSpace();
            clientSettingsManager.Server.Should().BeNullOrWhiteSpace();
            
            clientSettingsManager.UserName.Should().Be(@"Adrie");
            clientSettingsManager.Password.Should().BeNullOrWhiteSpace();

            clientSettingsManager.ConfigFolder.Should().Be(@".");
            clientSettingsManager.ConfigFileName.Should().BeNullOrWhiteSpace();
        }

        [TestMethod]
        public void WhenExecute_WithNoParameters_ShouldExitWith_4()
        {
            // arrange
            Mock<IErrorExiter> errorExiterMock = new();

            // act
            ClientSettingsManager clientSettingsManager = new(Array.Empty<string>(), errorExiterMock.Object);

            // assert
            errorExiterMock.Verify(x => x.Exit(4), Times.Once);

            clientSettingsManager.MailServer.Should().BeNullOrWhiteSpace();
            clientSettingsManager.Server.Should().BeNullOrWhiteSpace();

            clientSettingsManager.UserName.Should().BeNullOrWhiteSpace();
            clientSettingsManager.Password.Should().BeNullOrWhiteSpace();

            clientSettingsManager.ConfigFolder.Should().BeNullOrWhiteSpace();
            clientSettingsManager.ConfigFileName.Should().BeNullOrWhiteSpace();
        }

        [TestMethod]
        public void WhenExecute_WithConfigFolderAndCredentials_ShouldSetConfigFolder()
        {
            // arrange
            Mock<IErrorExiter> errorExiterMock = new();

            // act
            ClientSettingsManager clientSettingsManager = new(new[] { $"--{Argument.ConfigFileFolder}=.", $"--{Argument.UserName}=Adrie", $"--{Argument.Password}=geheim" }, errorExiterMock.Object);

            // assert
            errorExiterMock.Verify(x => x.Exit(It.IsAny<int>()), Times.Never);

            clientSettingsManager.MailServer.Should().BeNullOrWhiteSpace();
            clientSettingsManager.Server.Should().BeNullOrWhiteSpace();

            clientSettingsManager.UserName.Should().Be(@"Adrie");
            clientSettingsManager.Password.Should().Be(@"geheim");

            clientSettingsManager.ConfigFolder.Should().Be(@".");
            clientSettingsManager.ConfigFileName.Should().Be(@"SqlClientSettings.json");
        }

        [TestMethod]
        public void WhenExecute_WithConfigFileNameAndRequiredFields_ShouldSetConfigFileName()
        {
            // arrange
            Mock<IErrorExiter> errorExiterMock = new();

            // act
            ClientSettingsManager clientSettingsManager = new(new[] { $"--{Argument.ConfigFileFolder}=.", $"--{Argument.ConfigFileName}=test.json", $"--{Argument.UserName}=Adrie", $"--{Argument.Password}=geheim" }, errorExiterMock.Object);

            // assert
            errorExiterMock.Verify(x => x.Exit(It.IsAny<int>()), Times.Never);

            clientSettingsManager.MailServer.Should().BeNullOrWhiteSpace();
            clientSettingsManager.Server.Should().BeNullOrWhiteSpace();
            
            clientSettingsManager.UserName.Should().Be(@"Adrie");
            clientSettingsManager.Password.Should().Be(@"geheim");

            clientSettingsManager.ConfigFolder.Should().Be(@".");
            clientSettingsManager.ConfigFileName.Should().Be(@"test.json");
        }

        [TestMethod]
        public void WhenExecute_WithServerAndRequiredFields_ShouldSqlSetServerConfig()
        {
            // arrange
            Mock<IErrorExiter> errorExiterMock = new();

            // act
            ClientSettingsManager clientSettingsManager = new(new[] { $"--{Argument.ConfigFileFolder}=.", $"--{Argument.Server}=server", $"--{Argument.UserName}=Adrie", $"--{Argument.Password}=geheim" }, errorExiterMock.Object);

            // assert
            errorExiterMock.Verify(x => x.Exit(It.IsAny<int>()), Times.Never);

            clientSettingsManager.MailServer.Should().BeNullOrWhiteSpace();
            clientSettingsManager.Server.Should().Be("server");

            clientSettingsManager.UserName.Should().Be(@"Adrie");
            clientSettingsManager.Password.Should().Be(@"geheim");

            clientSettingsManager.ConfigFolder.Should().Be(@".");
            clientSettingsManager.ConfigFileName.Should().Be(@"SqlClientSettings.json");
        }

        [TestMethod]
        public void WhenExecute_WithMailServerAndRequiredFields_ShouldSetMailConfigFile()
        {
            // arrange
            Mock<IErrorExiter> errorExiterMock = new();

            // act
            ClientSettingsManager clientSettingsManager = new(new[]
            {
                $"--{Argument.ConfigFileFolder}=.", 
                $"--{Argument.MailServer}=mailserver", 
                $"--{Argument.UserName}=Adrie", 
                $"--{Argument.Password}=geheim", 
                $"--{Argument.Verbose}=verbose"
            }, errorExiterMock.Object);

            // assert
            errorExiterMock.Verify(x => x.Exit(It.IsAny<int>()), Times.Never);

            clientSettingsManager.MailServer.Should().Be("mailserver");
            clientSettingsManager.Server.Should().BeNullOrWhiteSpace();

            clientSettingsManager.UserName.Should().Be(@"Adrie");
            clientSettingsManager.Password.Should().Be(@"geheim");

            clientSettingsManager.ConfigFolder.Should().Be(@".");
            clientSettingsManager.ConfigFileName.Should().Be(@"MailClientSettings.json");
        }

        [TestMethod]
        public void WhenReadMailConfig_ShouldGetMailAccounts()
        {
            // arrange
            Mock<IErrorExiter> errorExiterMock = new();
            ClientSettingsManager clientSettingsManager = new(new[] { $"--{Argument.ConfigFileFolder}=.", $"--{Argument.MailServer}=mail.konfidence.nl", $"--{Argument.UserName}=Adrie", $"--{Argument.Password}=geheim" }, errorExiterMock.Object);
            clientSettingsManager.Execute();

            clientSettingsManager = new ClientSettingsManager(new[] { $"--{Argument.ConfigFileFolder}=.", $"--{Argument.MailServer}=mail.konfidence.nl", $"--{Argument.UserName}=A3", $"--{Argument.Password}=geheim" }, errorExiterMock.Object);

            // act
            clientSettingsManager.Execute();
            MailAccounts? mailConfig = ReadMailConfig();

            // assert
            errorExiterMock.Verify(x => x.Exit(It.IsAny<int>()), Times.Never);

            mailConfig.Should().NotBeNull();

            Assert.IsNotNull(mailConfig);

            mailConfig.Accounts.Should().HaveCountGreaterOrEqualTo(2);
            MailAccount? account1 = mailConfig.Accounts.FirstOrDefault(x => x.UserName == "Adrie");
            MailAccount? account2 = mailConfig.Accounts.FirstOrDefault(x => x.UserName == "A3");

            account1.Should().NotBeNull();
            account1?.Server.Should().Be("mail.konfidence.nl");
            account1?.UserName.Should().Be("Adrie");
            account1?.Password.Should().Be("geheim");

            account2.Should().NotBeNull();
            account2?.Server.Should().Be("mail.konfidence.nl");
            account2?.UserName.Should().Be("A3");
            account2?.Password.Should().Be("geheim");
        }

        private static MailAccounts? ReadMailConfig()
        {
            return File.ReadAllText(MailConstants.DefaultMailServerConfigFileName).Deserialize(out MailAccounts? mailAccounts) 
                ? mailAccounts 
                : null;
        }
    }
}
