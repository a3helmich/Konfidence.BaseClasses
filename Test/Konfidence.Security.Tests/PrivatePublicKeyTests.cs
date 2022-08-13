using System.Collections.Generic;
using FluentAssertions;
using Konfidence.Base;
using Konfidence.Security.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.Security.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class PrivatePublicKeyTest
    {
        private const string APPLICATION_NAME = "TestRegistration";

        [TestMethod]
        public void RetrieveCreatedKeyTest()
        {
            var ppk1 = new PrivatePublicKey(APPLICATION_NAME);

            var publicKey1 = ppk1.PublicKey;

            var ppk2 = new PrivatePublicKey(APPLICATION_NAME);

            var publicKey2 = ppk2.PublicKey;

            publicKey2.Should().Be(publicKey1, "Encryption Store niet opgeslagen");
        }

        [TestMethod]
        public void EncodeDecodeTest()
        {
            // this is only testing the encode decode functionality : NOT the encryption/decryption class!
            var resultString = string.Empty;
            var testString = string.Empty;

            testString += "-1teststring om te decoden encoden 1234567890";
            testString += "-2teststring om te decoden encoden 1234567890";
            testString += "-3teststring om te decoden encoden 1234567890";
            testString += "-4teststring om te decoden encoden 1234567890";

            var ppk = new PrivatePublicKey(APPLICATION_NAME);

            List<List<byte>>? arrayList;

            using (var encoder = new Encoder(ppk.PublicKey))
            {
                arrayList = encoder.Encrypt(testString);
            }

            if (arrayList.IsAssigned())
            {
                using var decoder = new Decoder(ppk.PrivateKey);

                resultString = decoder.Decrypt(arrayList);
            }

            ppk.DeleteEncryptionStore();

            arrayList.Should().NotBeNull();
            resultString.Should().NotBeNullOrWhiteSpace();
            testString.Should().Be(resultString, "encoding/decoding failed");
        }
    }
}
