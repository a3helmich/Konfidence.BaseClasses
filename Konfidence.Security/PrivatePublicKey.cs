using Konfidence.Security.Encryption;

namespace Konfidence.Security
{
    public class PrivatePublicKey 
    {
        public string ApplicationName { get; }

        public string PublicKey { get; }

        public string PrivateKey { get; }

        public PrivatePublicKey(string applicationName)
        {
            ApplicationName = applicationName;

            using KeyEncryption encryption = new(ApplicationName);

            PublicKey = encryption.PublicKey;
            PrivateKey = encryption.PrivateKey;
        }

        public bool DeleteEncryptionStore()
        {
            try
            {
                using KeyEncryption clientKeyEncryption = new(ApplicationName);

                clientKeyEncryption.Delete();
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
