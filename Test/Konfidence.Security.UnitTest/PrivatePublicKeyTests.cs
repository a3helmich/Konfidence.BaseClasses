using System;
using System.Collections.Generic;
using FluentAssertions;
using Konfidence.Base;
using Konfidence.Security.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.Security.UnitTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class PrivatePublicKeyTest
    {
        [TestMethod]
        public void Constructor_CalledTwice_ReturnsSamePublicKeyFromStore()
        {
            // Each test uses its own container name because CAPI key containers are persisted
            // machine-wide by name - sharing a fixed name with another test method racing in
            // parallel let one test's DeleteEncryptionStore() delete the key mid-sequence here,
            // making ppk2 regenerate a different key instead of reusing ppk1's.
            string applicationName = $"TestRegistration_{Guid.NewGuid():N}";

            PrivatePublicKey ppk1 = new(applicationName);

            string publicKey1 = ppk1.PublicKey;

            PrivatePublicKey ppk2 = new(applicationName);

            string publicKey2 = ppk2.PublicKey;

            ppk1.DeleteEncryptionStore();
            ppk2.DeleteEncryptionStore();

            publicKey2.Should().Be(publicKey1, "Encryption not stored in Store");
        }

        [TestMethod]
        public void Encrypt_ThenDecrypt_RoundTripsOriginalString()
        {
            // this is only testing the encode decode functionality : NOT the encryption/decryption class!
            string resultString = string.Empty;
            string testString = string.Empty;

            testString += "-1teststring om te decoden encoden 1234567890";
            testString += "-2teststring om te decoden encoden 1234567890";
            testString += "-3teststring om te decoden encoden 1234567890";
            testString += "-4teststring om te decoden encoden 1234567890";

            string applicationName = $"TestRegistration_{Guid.NewGuid():N}";

            PrivatePublicKey ppk = new(applicationName);

            List<List<byte>>? arrayList;

            using (Encoder encoder = new(ppk.PublicKey))
            {
                arrayList = encoder.Encrypt(testString);
            }

            if (arrayList.IsAssigned())
            {
                using Decoder decoder = new(ppk.PrivateKey);

                resultString = decoder.Decrypt(arrayList);
            }

            ppk.DeleteEncryptionStore();

            arrayList.Should().NotBeNull();
            resultString.Should().NotBeNullOrWhiteSpace();
            testString.Should().Be(resultString, "encoding/decoding failed");
        }
    }
}
