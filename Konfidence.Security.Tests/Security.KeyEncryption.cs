using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Konfidence.Security.Encryption;
using Moq;

namespace Konfidence.Security.Tests
{
    [TestClass]
    public class KeyEncryptionTests
    {
        [TestMethod]
        public void MaxKey_WithEmptyContainerName_ShouldGive1024AsMaxKeySize()
        {
            // arrange
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(x => x.OSVersionPlatform).Returns(PlatformID.Win32NT);

            using (var keyEncryption = new KeyEncryption(configurationMock.Object))
            {
                // act
                var maxKeySize = keyEncryption.GetMaxKeySize();

                // assert
                Assert.AreEqual(1024, maxKeySize);
            }
        }

        [TestMethod]
        public void MaxKeyWin32_WithEmptyContainerName_ShouldGive1024AsMaxKeySize()
        {
            // arrange
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(x => x.OSVersionPlatform).Returns(PlatformID.Win32Windows);

            using (var keyEncryption = new KeyEncryption(configurationMock.Object))
            {
                // act
                var maxKeySize = keyEncryption.GetMaxKeySize();

                // assert
                Assert.AreEqual(384, maxKeySize);
            }
        }
    }
}
