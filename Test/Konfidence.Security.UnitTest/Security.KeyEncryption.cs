using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Konfidence.Security.Encryption;
using Moq;

namespace Konfidence.Security.UnitTest
{
    [TestClass]
    public class KeyEncryptionTests
    {
        [TestMethod]
        public void KeySize_WinNTWithEmptyContainerName_Returns1024()
        {
            // Arrange
            using TestContext context = CreateContext(PlatformID.Win32NT);

            // Act
            int maxKeySize = context.KeyEncryption.KeySize;

            // Assert
            maxKeySize.Should().Be(1024);
        }

        [TestMethod]
        public void KeySize_Win32WithEmptyContainerName_Returns384()
        {
            // Arrange
            using TestContext context = CreateContext(PlatformID.Win32Windows);

            // Act
            int maxKeySize = context.KeyEncryption.KeySize;

            // Assert
            maxKeySize.Should().Be(384);
        }

        [TestMethod]
        public void PackageSize_WinNTWithEmptyContainerName_Returns64()
        {
            // Arrange
            using TestContext context = CreateContext(PlatformID.Win32NT);

            // Act
            int packageSize = context.KeyEncryption.PackageSize;

            // Assert
            packageSize.Should().Be(64);
        }

        [TestMethod]
        public void PackageSize_Win32WithEmptyContainerName_Returns24()
        {
            // Arrange
            using TestContext context = CreateContext(PlatformID.Win32Windows);

            // Act
            int packageSize = context.KeyEncryption.PackageSize;

            // Assert
            packageSize.Should().Be(24);
        }

        [TestMethod]
        public void PublicKey_TemporaryContainer_ReturnsNonEmptyXmlString()
        {
            // Arrange
            using TestContext context = CreateContext(PlatformID.Win32NT);

            // Act
            string publicKey = context.KeyEncryption.PublicKey;

            // Assert
            publicKey.Should().NotBeNullOrEmpty();
            publicKey.Should().Contain("RSAKeyValue");
        }

        [TestMethod]
        public void PrivateKey_TemporaryContainer_ReturnsNonEmptyXmlString()
        {
            // Arrange
            using TestContext context = CreateContext(PlatformID.Win32NT);

            // Act
            string privateKey = context.KeyEncryption.PrivateKey;

            // Assert
            privateKey.Should().NotBeNullOrEmpty();
            privateKey.Should().Contain("RSAKeyValue");
        }

        [TestMethod]
        public void Delete_WhenKeyExists_ReturnsTrue()
        {
            // Arrange
            using TestContext context = CreateContext(PlatformID.Win32NT);
            _ = context.KeyEncryption.PublicKey;

            // Act
            bool result = context.KeyEncryption.Delete();

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void Delete_WhenNoKeyExists_ReturnsFalse()
        {
            // Arrange
            using TestContext context = CreateContext(PlatformID.Win32NT);
            _ = context.KeyEncryption.PublicKey;
            context.KeyEncryption.Delete();

            // Act
            bool result = context.KeyEncryption.Delete();

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void ReadKey_WhenNoProviderAssigned_DoesNotThrow()
        {
            // Arrange
            using TestContext context = CreateContext(PlatformID.Win32NT);
            _ = context.KeyEncryption.PublicKey;
            context.KeyEncryption.Delete();

            // Act
            Action action = () => context.KeyEncryption.ReadKey("<RSAKeyValue></RSAKeyValue>");

            // Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void Dispose_CalledTwice_DoesNotThrow()
        {
            // Arrange
            TestContext context = CreateContext(PlatformID.Win32NT);
            context.KeyEncryption.Dispose();

            // Act
            Action action = () => context.KeyEncryption.Dispose();

            // Assert
            action.Should().NotThrow();
        }

        private sealed class TestContext : IDisposable
        {
            public TestContext(
                KeyEncryption KeyEncryption,
                Mock<ISecurityConfiguration> ConfigurationMock
            )
            {
                this.KeyEncryption = KeyEncryption;
                this.ConfigurationMock = ConfigurationMock;
            }

            public KeyEncryption KeyEncryption { get; }

            public Mock<ISecurityConfiguration> ConfigurationMock { get; }

            public void Dispose()
            {
                KeyEncryption.Dispose();
            }
        }

        private static TestContext CreateContext(PlatformID platform)
        {
            Mock<ISecurityConfiguration> configurationMock = new();

            configurationMock.Setup(x => x.OSVersionPlatform).Returns(platform);

            KeyEncryption keyEncryption = new(string.Empty, configurationMock.Object);

            return new TestContext(keyEncryption, configurationMock);
        }
    }
}
