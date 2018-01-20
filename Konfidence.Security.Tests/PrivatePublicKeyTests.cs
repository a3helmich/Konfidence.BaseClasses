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

        public PrivatePublicKeyTest()
        {
            //
            // TODO: Add constructor logic here
            //

            BaseItem.UnitTest = true;
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        //Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void MyTestInitialize()
        {
        }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void RetrieveCreatedKeyTest()
        {
            var configuration = new Configuration();

            var ppk1 = new PrivatePublicKey(APPLICATION_NAME, configuration);

            var publicKey1 = ppk1.PublicKey;

            var ppk2 = new PrivatePublicKey(APPLICATION_NAME, configuration);

            var publicKey2 = ppk2.PublicKey;

            Assert.AreEqual(publicKey1, publicKey2, "Encryption Store niet opgeslagen");
        }

        [TestMethod]
        public void EncodeDecodeTest()
        {
            // this is only testing the encode decode functionality : NOT the encryption/decryption class!
            string resultString;
            var testString = string.Empty;

            testString += "-1teststring om te decoden encoden 1234567890";
            testString += "-2teststring om te decoden encoden 1234567890";
            testString += "-3teststring om te decoden encoden 1234567890";
            testString += "-4teststring om te decoden encoden 1234567890";

            var configuration = new Configuration();

            var ppk = new PrivatePublicKey(APPLICATION_NAME, configuration);

            object[] arrayList;

            using (var encoder = new Encoder(ppk.PublicKey))
            {
                arrayList = encoder.Encrypt(testString);
            }

            using (var decoder = new Decoder(ppk.PrivateKey))
            {
                resultString = decoder.Decrypt(arrayList);
            }

            ppk.DeleteEncryptionStore();

            Assert.AreEqual(resultString, testString, false, "encoding/decoding failed");
        }
    }
}
