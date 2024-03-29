﻿using System;
using System.Diagnostics;
using System.Security.Cryptography;
using Konfidence.Base;

namespace Konfidence.Security.Encryption
{
    public sealed class KeyEncryption : IDisposable
    {
        private bool _disposed;

        private RSACryptoServiceProvider? _tempRsaProvider;

        private int _maxBytesServer; 

        public RSACryptoServiceProvider? RsaProvider { get; private set; }

        public string PublicKey => RsaProviderToXmlString(false);

        public string PrivateKey => RsaProviderToXmlString(true);

        public int KeySize => TempKeyContainer.KeySize;

        public int PackageSize => _maxBytesServer / 2;

        private readonly ISecurityConfiguration _securityConfiguration;

        private string RsaProviderToXmlString(bool includePrivateParameters)
        {
            try
            {
                return RsaProvider?.ToXmlString(includePrivateParameters)??string.Empty;
            }
            catch (PlatformNotSupportedException ex)
            {
                throw new Exception("Security is only supported by dotnetCore 3.0 or higher and the dotnet Framework x.x", ex);
            }
        }

        public KeyEncryption(string containerName) : this(containerName, null) { }

        internal KeyEncryption(string containerName, ISecurityConfiguration? securityConfiguration)
        {
            _securityConfiguration = securityConfiguration ?? new SecurityConfiguration();

            _maxBytesServer = GetMaxKeySize() / 8;

            var isTemporary = false;

            if (!containerName.IsAssigned())
            {
                isTemporary = true;

                containerName = "None";
            }

            Debug.WriteLine("Encryption: Utilhelper.ServerKeyEncryption(...) key - " + containerName);

            if (isTemporary)
            {
                RsaProvider = TempKeyContainer;
            }
            else
            {
                GetKeyContainer(containerName);
            }

            Debug.WriteLine("Encryption: Utilhelper.ServerKeyEncryption(...) gotcontainer");
        }

        private RSACryptoServiceProvider TempKeyContainer
        {
            get
            {
                var keySizeServer = GetMaxKeySize();

                if (!_tempRsaProvider.IsAssigned())
                {
                    _tempRsaProvider = keySizeServer == 0 ? new RSACryptoServiceProvider() : new RSACryptoServiceProvider(keySizeServer);
                }

                return _tempRsaProvider;
            }
        }

        private int GetMaxKeySize()
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

            return _maxBytesServer * 8;
        }

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
            // - if the serviceprovider allready exists, with the wrong keysize, delete the serviceprovider
            // - get a new serviceprovider if deleted
            // - if the Encryption is temporary, make sure it is not persistend
            try
            {
                var cp = GetCspParameters(containerName);

                if (!RsaProvider.IsAssigned())
                {
                    try
                    {
                        RsaProvider = new RSACryptoServiceProvider(_maxBytesServer * 8, cp);
                    }
                    catch (CryptographicException e)
                    {
                        if (RsaProvider.IsAssigned())
                        {
                            Delete();
                        }

                        throw new Exception("create: " + e.Message, e);
                    }
                }

                if (_maxBytesServer > 0 && RsaProvider.KeySize != _maxBytesServer * 8)
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


                        if (!RsaProvider.IsAssigned())
                        {
                            RsaProvider = new RSACryptoServiceProvider(_maxBytesServer * 8, cp);
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

                if (RsaProvider.IsAssigned())
                {
                    // Delete the key entry in the container.
                    RsaProvider.PersistKeyInCsp = false;

                    // Call Clear to release resources and delete the key from the container.
                    RsaProvider.Clear();

                    RsaProvider = null;

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

        public void ReadKey(string key)
        {
            if (!RsaProvider.IsAssigned())
            {
                return;
            }

            RsaProvider.FromXmlString(key);
            _maxBytesServer = RsaProvider.KeySize / 8;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {

            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (RsaProvider.IsAssigned())
                {
                    RsaProvider.Clear();
                }
            }

            _disposed = true;
        }
    }
}
