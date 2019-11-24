using System;
using Konfidence.Security.Encryption;
using System.Diagnostics;
using System.Security.Cryptography;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.Security
{
    public class PrivatePublicKey 
    {
        public string ApplicationName { get; }

        public string PublicKey { get; }

        public string PrivateKey { get; }

        private readonly ISecurityConfiguration _securityConfiguration;

        public PrivatePublicKey(string applicationName, [NotNull] ISecurityConfiguration securityConfiguration)
        {
            _securityConfiguration = securityConfiguration;
            ApplicationName = applicationName;

            using (var encryption = new KeyEncryption(ApplicationName, securityConfiguration))
            {
                PublicKey = encryption.PublicKey;
                PrivateKey = encryption.PrivateKey;
            }
        }

        public bool DeleteEncryptionStore()
        {
            try
            {
                using (var clientKeyEncryption = new KeyEncryption(ApplicationName, _securityConfiguration))
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
    }
}
