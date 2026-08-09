using System;
using System.Security.Cryptography;
using FluentAssertions;
using Konfidence.Security.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Konfidence.Security.UnitTest;

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
    public void PublicKey_AfterDelete_ReturnsEmptyString()
    {
        // Arrange
        // Delete() nulls RsaProvider, which is the only way to reach the "?? string.Empty" arm -
        // the key properties have to degrade to empty rather than throwing a NullReferenceException
        // on an instance whose key was already released.
        using TestContext context = CreateContext(PlatformID.Win32NT);
        _ = context.KeyEncryption.PublicKey;
        context.KeyEncryption.Delete();

        // Act
        string publicKey = context.KeyEncryption.PublicKey;

        // Assert
        publicKey.Should().BeEmpty();
    }

    [TestMethod]
    public void PrivateKey_AfterDelete_ReturnsEmptyString()
    {
        // Arrange
        using TestContext context = CreateContext(PlatformID.Win32NT);
        _ = context.KeyEncryption.PrivateKey;
        context.KeyEncryption.Delete();

        // Act
        string privateKey = context.KeyEncryption.PrivateKey;

        // Assert
        privateKey.Should().BeEmpty();
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
    public void ReadKey_WhenProviderAssigned_LoadsTheGivenKey()
    {
        // Arrange
        using TestContext source = CreateContext(PlatformID.Win32NT);
        string sourcePrivateKey = source.KeyEncryption.PrivateKey;

        using TestContext target = CreateContext(PlatformID.Win32NT);
        _ = target.KeyEncryption.PublicKey;

        // Act
        target.KeyEncryption.ReadKey(sourcePrivateKey);

        // Assert
        target.KeyEncryption.PrivateKey.Should().Be(sourcePrivateKey);
    }

    [TestMethod]
    public void Constructor_WithNamedContainer_CreatesAndPersistsAKey()
    {
        // Arrange
        string containerName = $"KonfidenceTestContainer_{Guid.NewGuid():N}";
        Mock<ISecurityConfiguration> configurationMock = new();
        configurationMock.Setup(x => x.OSVersionPlatform).Returns(PlatformID.Win32NT);

        using KeyEncryption keyEncryption = new(containerName, configurationMock.Object);

        try
        {
            // Act
            string publicKey = keyEncryption.PublicKey;

            // Assert
            publicKey.Should().NotBeNullOrEmpty();
            publicKey.Should().Contain("RSAKeyValue");
        }
        finally
        {
            keyEncryption.Delete();
        }
    }

    [TestMethod]
    public void Constructor_WithNamedContainerAndDifferentKeySizeThanExisting_RecreatesTheContainer()
    {
        // Arrange
        string containerName = $"KonfidenceTestContainer_{Guid.NewGuid():N}";

        Mock<ISecurityConfiguration> smallKeyConfigurationMock = new();
        smallKeyConfigurationMock.Setup(x => x.OSVersionPlatform).Returns(PlatformID.Win32Windows);

        Mock<ISecurityConfiguration> largeKeyConfigurationMock = new();
        largeKeyConfigurationMock.Setup(x => x.OSVersionPlatform).Returns(PlatformID.Win32NT);

        using KeyEncryption first = new(containerName, smallKeyConfigurationMock.Object);
        _ = first.PublicKey;

        try
        {
            // Act
            using KeyEncryption second = new(containerName, largeKeyConfigurationMock.Object);

            try
            {
                // Assert
                // Windows reuses an existing named key regardless of the requested size, so the
                // second construction's RsaProvider.KeySize (384, from the first instance) no
                // longer matches its own _maxBytesServer (computed from Win32NT), forcing the
                // delete-and-recreate branch in GetKeyContainer().
                second.RsaProvider.Should().NotBeNull();
                second.RsaProvider!.KeySize.Should().Be(1024);
            }
            finally
            {
                second.Delete();
            }
        }
        finally
        {
            first.Delete();
        }
    }

    [TestMethod]
    public void Constructor_WithContainerNameTooLongForTheKeyStore_ThrowsWrappingTheCryptographicException()
    {
        // Arrange
        // CAPI rejects an over-long key container name, which is the one way to make the
        // "new RSACryptoServiceProvider(size, cp)" call inside GetKeyContainer() fail - the
        // create-failure catch block was otherwise never reached by any test.
        string containerName = new('x', 5000);

        Mock<ISecurityConfiguration> configurationMock = new();
        configurationMock.Setup(x => x.OSVersionPlatform).Returns(PlatformID.Win32NT);

        // Act
        Action action = () =>
        {
            using KeyEncryption keyEncryption = new(containerName, configurationMock.Object);
        };

        // Assert
        // The original CryptographicException is rewrapped with a "create: " prefix and kept as
        // the inner exception, so the failing stage stays identifiable from the message alone.
        action.Should().Throw<Exception>()
            .Where(x => x.Message.StartsWith("create: "))
            .WithInnerException<CryptographicException>();
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
