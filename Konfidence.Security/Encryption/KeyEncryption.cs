using System;
using System.Diagnostics;
using System.Security.Cryptography;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.Security.Encryption
{
    public class KeyEncryption : IDisposable
    {
        private RSACryptoServiceProvider _rsaProvider;

        private bool _disposed;

        private RSACryptoServiceProvider _tempRsaProvider;
        private int _tempKeySize;

        // _MaxBytes schijnt uit te vinden te zijn, maar is een beetje vreemd, heb onderstussen het een en ander 
        // uitgezocht, maar de maximum datasize = keysize lijkt niet waar te zijn heb voorlopig gekozen voor halverwege 
        // de keysize, dit lijkt te voldoen, moet verder uitgezocht.
        //NB nov 2012: zal wel iets te maken hebben met de encoding van de string (onebyte/twobyte).
        private int _maxBytesServer; // default voor de serverside
        private int _maxBytesClient; // default voor de serverside

        public RSACryptoServiceProvider RsaProvider => _rsaProvider;

        [NotNull] public string PublicKey => _rsaProvider.ToXmlString(false);

        [NotNull] public string PrivateKey => _rsaProvider.ToXmlString(true);

        [UsedImplicitly]
        public int KeySize => TempKeyContainer.KeySize;

        public int PackageSize => _maxBytesServer / 2;

        private readonly ISecurityConfiguration _securityConfiguration;

        public KeyEncryption([NotNull] ISecurityConfiguration securityConfiguration)
        {
            if (securityConfiguration.Framework.IsAssigned())
            {
                var frameworkParts = securityConfiguration.Framework.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                var frameworkVersion = frameworkParts[1].Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries)[1].TrimStart('v');
                if (frameworkParts[0] == ".NETCoreApp" && double.TryParse(frameworkVersion, out var version) && version < 3)
                {
                    throw new Exception($"Minimaly dotnetcore 3.0 required, FrameWork: {securityConfiguration.Framework}");
                }
            }

            _disposed = false;
            _securityConfiguration = securityConfiguration;
        }

        public KeyEncryption(string containerName, [NotNull] ISecurityConfiguration securityConfiguration) : this(0, containerName, securityConfiguration)
        {
        }

        public KeyEncryption(int keySize, string containerName, [NotNull] ISecurityConfiguration securityConfiguration) : this(securityConfiguration)
        {
            _maxBytesClient = keySize / 8;
            _maxBytesServer = keySize / 8;

            var isTemporary = false;

            if (!containerName.IsAssigned())
            {
                isTemporary = true;

                containerName = "None";
            }

            Debug.WriteLine("Encryption: Utilhelper.ServerKeyEncryption(...) key - " + containerName);

            if (isTemporary)
            {
                _rsaProvider = TempKeyContainer;
            }
            else
            {
                GetKeyContainer(containerName);
            }

            Debug.WriteLine("Encryption: Utilhelper.ServerKeyEncryption(...) gotcontainer");
        }

        [NotNull]
        protected RSACryptoServiceProvider TempKeyContainer
        {
            get
            {
                var keySizeClient = _maxBytesClient * 8;
                var keySizeServer = GetMaxKeySize(); // dit valt te optimaliseren, door er voor te zorgen dat de oude niet direct wordt weggemikt

                if (keySizeServer < keySizeClient)
                {
                    keySizeClient = keySizeServer;
                }

                if (!_tempRsaProvider.IsAssigned() || _tempKeySize != keySizeClient)
                {
                    _tempRsaProvider = keySizeClient == 0 ? new RSACryptoServiceProvider() : new RSACryptoServiceProvider(keySizeClient);

                    _tempKeySize = _tempRsaProvider.KeySize;
                }

                return _tempRsaProvider;
            }
        }

        public int GetMaxKeySize()
        {
            // at first I wanted to remove redundant keys etc. but keeping them in 
            // store is faster for non-key generating actions. like here, but also for encoding and decoding
            // Rsa.PersistKeyInCsp = false;  // don't want to keep this in storage
            var keyContainer = new RSACryptoServiceProvider();

            var legalKeySize = keyContainer.LegalKeySizes[0];

            switch (_securityConfiguration.OSVersionPlatform)
            {
                case PlatformID.Win32Windows:
                    {
                        _maxBytesServer = legalKeySize.MinSize / 8;
                        break;
                    }
                default:
                    {
                        _maxBytesServer = keyContainer.KeySize / 8;
                        break;
                    }
            }

            if (_maxBytesClient == 0)
            {
                _maxBytesClient = _maxBytesServer;
            }

            return _maxBytesServer * 8;
        }

        [NotNull]
        private static CspParameters GetCspParameters(string containerName)
        {
            var cp = new CspParameters {KeyContainerName = containerName};

            cp.Flags |= CspProviderFlags.UseMachineKeyStore;

            return cp;
        }

        private void GetKeyContainer(string containerName)
        {
            // - if this is a clientsize Encryption determine maximum keysize
            // - get the serviceprovider
            // - if the serviceprovider allready exisists, with the wrong keysize, delete the serviceprovider
            // - get a new serviceprovider if deleted
            // - if the Encryption is temporary, make sure it is not persistend
            try
            {
                var cp = GetCspParameters(containerName);

                if (!_rsaProvider.IsAssigned())
                {
                    try
                    {
                        _rsaProvider = new RSACryptoServiceProvider(_maxBytesServer * 8, cp);
                    }
                    catch (CryptographicException e)
                    {
                        {
                            throw new Exception("create: " + e.Message, e);
                        }
                    }
                }

                if (_maxBytesServer > 0 && _rsaProvider.KeySize != _maxBytesServer * 8)
                {
                    try
                    {
                        try
                        {
                            Delete();
                        }
                        catch (CryptographicException e)
                        {
                            {
                                throw new Exception("delete: " + e.Message, e);
                            }
                        }


                        if (!_rsaProvider.IsAssigned())
                        {
                            _rsaProvider = new RSACryptoServiceProvider(_maxBytesServer * 8, cp);
                        }
                    }
                    catch (CryptographicException e)
                    {
                        {
                            throw new Exception("replace: " + e.Message, e);
                        }
                    }
                }
            }
            catch (CryptographicException e)
            {
                Debug.WriteLine("Encryption: Utilhelper.GetKeyContainer(...) unexpected exception - " + e.Message);

                throw new Exception("most outer: " + e.Message, e);
            }
        }

        public bool Delete()
        {
            try
            {
                // if a rsaprovider exists, make non persistent, clear it and nullify --> the key is deleted

                if (_rsaProvider.IsAssigned())
                {
                    // Delete the key entry in the container.
                    _rsaProvider.PersistKeyInCsp = false;

                    // Call Clear to release resources and delete the key from the container.
                    _rsaProvider.Clear();

                    _rsaProvider = null;

                    return true;
                }

                return false;
            }
            catch (CryptographicException e)
            {
                Debug.WriteLine("Encryption: UtilHelper.DeleteKeyFromContainer(...) unable to delete key - " + e.Message);

                return false;
            }
        }

        public void ReadKey([NotNull] string key)
        {
            _rsaProvider.FromXmlString(key);
            _maxBytesServer = _rsaProvider.KeySize / 8;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {

            if (!_disposed)
            {
                if (_rsaProvider.IsAssigned())
                {
                    _rsaProvider.Clear(); // resources vrijgeven.

                    _rsaProvider = null;
                }
            }
            _disposed = true;

        }

        #endregion
    }
}
