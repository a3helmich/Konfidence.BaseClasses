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
            PrivatePublicKey ppk1 = new(APPLICATION_NAME);

            string publicKey1 = ppk1.PublicKey;

            PrivatePublicKey ppk2 = new(APPLICATION_NAME);

            string publicKey2 = ppk2.PublicKey;

            ppk1.DeleteEncryptionStore();
            ppk2.DeleteEncryptionStore();

            publicKey2.Should().Be(publicKey1, "Encryption not stored in Store");
        }

        [TestMethod]
        public void EncodeDecodeTest()
        {
            // this is only testing the encode decode functionality : NOT the encryption/decryption class!
            string resultString = string.Empty;
            string testString = string.Empty;

            testString += "-1teststring om te decoden encoden 1234567890";
            testString += "-2teststring om te decoden encoden 1234567890";
            testString += "-3teststring om te decoden encoden 1234567890";
            testString += "-4teststring om te decoden encoden 1234567890";

            PrivatePublicKey ppk = new(APPLICATION_NAME);

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
