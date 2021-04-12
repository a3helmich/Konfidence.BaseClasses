using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Konfidence.Security.Encryption;
using Moq;

namespace Konfidence.Security.Tests
{
    [TestClass]
    public class KeyEncryptionTests
    {
        [TestMethod]
        public void MaxKey_WinNT_WithEmptyContainerName_ShouldGive_1024_AsMaxKeySize()
        {
            // arrange
            var configurationMock = new Mock<ISecurityConfiguration>();

            configurationMock.Setup(x => x.OSVersionPlatform).Returns(PlatformID.Win32NT);

            using var keyEncryption = new KeyEncryption(string.Empty, configurationMock.Object);

            // act
            var maxKeySize = keyEncryption.KeySize;

            // assert
            maxKeySize.Should().Be(1024);
        }

        [TestMethod]
        public void MaxKey_Win32_WithEmptyContainerName_ShouldGive_384_AsMaxKeySize()
        {
            // arrange
            var configurationMock = new Mock<ISecurityConfiguration>();

            configurationMock.Setup(x => x.OSVersionPlatform).Returns(PlatformID.Win32Windows);

            using var keyEncryption = new KeyEncryption(string.Empty, configurationMock.Object);

            // act
            var maxKeySize = keyEncryption.KeySize;

            // assert
            maxKeySize.Should().Be(384);
        }
    }
}
