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
            // arrange
            Mock<ISecurityConfiguration> configurationMock = new();

            configurationMock.Setup(x => x.OSVersionPlatform).Returns(PlatformID.Win32NT);

            using KeyEncryption keyEncryption = new(string.Empty, configurationMock.Object);

            // act
            int maxKeySize = keyEncryption.KeySize;

            // assert
            maxKeySize.Should().Be(1024);
        }

        [TestMethod]
        public void KeySize_Win32WithEmptyContainerName_Returns384()
        {
            // arrange
            Mock<ISecurityConfiguration> configurationMock = new();

            configurationMock.Setup(x => x.OSVersionPlatform).Returns(PlatformID.Win32Windows);

            using KeyEncryption keyEncryption = new(string.Empty, configurationMock.Object);

            // act
            int maxKeySize = keyEncryption.KeySize;

            // assert
            maxKeySize.Should().Be(384);
        }
    }
}
