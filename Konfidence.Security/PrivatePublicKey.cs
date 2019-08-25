using Konfidence.Security.Encryption;
using System.Diagnostics;
using System.Security.Cryptography;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.Security
{
    public class PrivatePublicKey 
    {
        public static int MaxKeySize = -1;

        public string ApplicationName { get; }

        public string PublicKey { get; }

        public string PrivateKey { get; }

        private readonly IConfiguration _configuration;

        public string ServerPublicKey { get; set; } = string.Empty;

        public PrivatePublicKey(string applicationName, IConfiguration configuration) : this(0, applicationName, configuration)
        {
        }

        public PrivatePublicKey(int maxKeySize, string applicationName, IConfiguration configuration)
        {
            _configuration = configuration;
            ApplicationName = applicationName;

            using (var test = new KeyEncryption(maxKeySize, ApplicationName, configuration))
            {
                PublicKey = test.PublicKey;
                PrivateKey = test.PrivateKey;

                if (test.IsAssigned())
                {
                    using (var clientKeyEncryption = new KeyEncryption(maxKeySize, ApplicationName, configuration))
                    {
                        PublicKey = clientKeyEncryption.PublicKey;
                        PrivateKey = clientKeyEncryption.PrivateKey;
                        MaxKeySize = clientKeyEncryption.GetMaxKeySize();
                    }
                }
            }
        }

        public bool DeleteEncryptionStore()
        {
            try
            {
                using (var clientKeyEncryption = new KeyEncryption(ApplicationName, _configuration))
                {
                    clientKeyEncryption.Delete();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        [UsedImplicitly]
        [NotNull]
        public string Decode(object[] encodedObjectArray)
        {
            var decodedString = string.Empty;

            using (var decoder = new Decoder(PrivateKey))
            {
                try
                {
                    decodedString = decoder.Decrypt(encodedObjectArray);

                    Debug.WriteLine("Decryption: finished");
                }
                catch (CryptographicException cex)
                {
                    Debug.WriteLine("Decryption: failed - " + cex.Message);
                }
            }

            return decodedString;
        }

        [CanBeNull]
        public object[] Encode(string toEncrypt)
        {
            object[] encryptedRegistrationData;

            using (var encoder = new Encoder(ServerPublicKey))
            {
                encryptedRegistrationData = encoder.Encrypt(toEncrypt);
            }

            return encryptedRegistrationData;
        }

        [UsedImplicitly]
        public static int GetKeySize(string clientPublicKey)
        {
            using (var encoder = new Encoder(clientPublicKey))
            {
                return encoder.KeySize;
            }
        }
    }
}
