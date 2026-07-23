using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Konfidence.Base;
using Konfidence.Security.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.Security.UnitTest;

[TestClass]
public class EncoderDecoderTests
{
    [TestMethod]
    public void Encrypt_UnassignedString_ReturnsNull()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        List<List<byte>>? result = context.Encoder.Encrypt(string.Empty);

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public void KeySize_Always_ReturnsUnderlyingRsaKeySize()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        int keySize = context.Encoder.KeySize;

        // Assert
        keySize.Should().BeGreaterThan(0);
    }

    [TestMethod]
    public void Decrypt_EmptyEncryptedData_ReturnsEmptyString()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        string result = context.Decoder.Decrypt([]);

        // Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Encrypt_ThenDecrypt_LongStringSpanningMultipleBlocks_RoundTripsSuccessfully()
    {
        // Arrange
        TestContext context = CreateContext();
        string testString = string.Concat(Enumerable.Repeat("teststring om te decoden encoden 1234567890-", 20));

        // Act
        List<List<byte>>? encrypted = context.Encoder.Encrypt(testString);
        string decrypted = encrypted.IsAssigned() ? context.Decoder.Decrypt(encrypted) : string.Empty;

        // Assert
        encrypted.Should().NotBeNull();
        encrypted!.Should().HaveCountGreaterThan(1);
        decrypted.Should().Be(testString);
    }

    [TestMethod]
    public void Dispose_ThenDecrypt_ThrowsBecauseUnderlyingRsaProviderWasActuallyCleared()
    {
        // Arrange
        TestContext context = CreateContext();
        string testString = "teststring om te decoden encoden 1234567890";
        List<List<byte>>? encrypted = context.Encoder.Encrypt(testString);

        context.Decoder.Dispose();

        // Act
        Action action = () => context.Decoder.Decrypt(encrypted!);

        // Assert
        action.Should().Throw<ObjectDisposedException>();
    }

    [TestMethod]
    public void Dispose_CalledTwice_DoesNotThrow()
    {
        // Arrange
        TestContext context = CreateContext();
        context.Decoder.Dispose();

        // Act
        Action action = () => context.Decoder.Dispose();

        // Assert
        action.Should().NotThrow();
    }

    private sealed class TestContext
    {
        public TestContext(
            Encoder Encoder,
            Decoder Decoder
        )
        {
            this.Encoder = Encoder;
            this.Decoder = Decoder;
        }

        public Encoder Encoder { get; }

        public Decoder Decoder { get; }
    }

    private static TestContext CreateContext()
    {
        using KeyEncryption keyEncryption = new(string.Empty);

        Encoder encoder = new(keyEncryption.PublicKey);
        Decoder decoder = new(keyEncryption.PrivateKey);

        return new TestContext(encoder, decoder);
    }
}
