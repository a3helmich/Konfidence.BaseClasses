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
