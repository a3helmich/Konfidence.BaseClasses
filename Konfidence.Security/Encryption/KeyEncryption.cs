using System;
using System.Diagnostics;
using System.Security.AccessControl;
using System.Security.Cryptography;
using Konfidence.Base;

namespace Konfidence.Security.Encryption
{
    public class KeyEncryption : BaseItem, IDisposable
    {
        private RSACryptoServiceProvider _rsaProvider;

        private bool _disposed;

        private RSACryptoServiceProvider _tempRsaProvider;
        private int _tempKeySize;

        private readonly string _containerName;

        // _MaxBytes schijnt uit te vinden te zijn, maar is een 
        // beetje vreemd heb onderstussen het een en ander 
        // uitgezocht, maar de maximum datasize = keysize lijkt
        // niet waar te zijn heb voorlopig gekozen voor halverwege 
        // keysize, dit lijkt te voldoen, moet verder uitgezocht
        //NB nov 2012: zal wel iets te maken hebben met de encoding van de string (onebyte/twobyte)
        private int _maxBytesServer; // default voor de serverside
        private int _maxBytesClient; // default voor de serverside

        #region properties
        public RSACryptoServiceProvider RsaProvider
        {
            get { return _rsaProvider; }
        }

        public string PublicKey
        {
            get
            {
                return _rsaProvider.ToXmlString(false);
            }
        }

        public string PrivateKey
        {
            get
            {
                return _rsaProvider.ToXmlString(true);
            }
        }

        public int KeySize
        {
            get
            {
                return TempKeyContainer.KeySize;
            }
        }

        public int PackageSize
        {
            get
            {
                return _maxBytesServer / 2;
            }
        }
        #endregion properties

        public KeyEncryption()
        {
            _disposed = false;
        }

        protected RSACryptoServiceProvider TempKeyContainer
        {
            get
            {
                int keySizeClient = _maxBytesClient * 8;
                int keySizeServer = GetMaxKeySize(); // dit valt te optimaliseren, door er voor te zorgen dat de oude niet direct wordt weggemikt

                if (keySizeServer < keySizeClient)
                {
                    keySizeClient = keySizeServer;
                }

                if (!_tempRsaProvider.IsAssigned() || _tempKeySize != keySizeClient)
                {
                    if (keySizeClient == 0)
                    {
                        _tempRsaProvider = new RSACryptoServiceProvider();
                    }
                    else
                    {
                        _tempRsaProvider = new RSACryptoServiceProvider(keySizeClient);
                    }

                    _tempKeySize = _tempRsaProvider.KeySize;
                }

                return _tempRsaProvider;
            }
        }

        private int GetMaxKeySize()
        {
            // at first i wanted to remove redundant keys etc. but keeping them in 
            // store is faster for non-key generating actions. like here, but also for encoding and decoding
            // Rsa.PersistKeyInCsp = false;  // don't want to keep this in storage
            var keyContainer = new RSACryptoServiceProvider();

            KeySizes legalKeySize = keyContainer.LegalKeySizes[0];

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32S:
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

        public KeyEncryption(string containerName) : this(0, containerName)
        {
        }

        public KeyEncryption(int keySize, string containerName)
        {
            _maxBytesClient = keySize / 8;
            _maxBytesServer = keySize / 8;
            _containerName = containerName;

            bool isTemporary = false;

            if (_containerName.IsAssigned())
            {
                isTemporary = true;

                _containerName = "None";
            }

            Debug.WriteLine("Encryption: Utilhelper.ServerKeyEncryption(...) key - " + _containerName);

            if (isTemporary)
            {
                _rsaProvider = TempKeyContainer;
            }
            else
            {
                GetKeyContainer(_containerName);
            }

            Debug.WriteLine("Encryption: Utilhelper.ServerKeyEncryption(...) gotcontainer");
        }

        public static int MaxKeySize()
        {
            using (var keyEncryption = new KeyEncryption(string.Empty))
            {
                return keyEncryption.KeySize;
            }
        }

        private CspParameters GetCspParameters(string containerName)
        {
            var cp = new CspParameters();

            const string user = "Everyone"; //  @"NT AUTHORITY\NETWORK SERVICE"; //network service

            //Environment.

            var rule = new CryptoKeyAccessRule(user, CryptoKeyRights.Delete | CryptoKeyRights.FullControl | CryptoKeyRights.TakeOwnership | CryptoKeyRights.ChangePermissions, AccessControlType.Allow);

            var cryptoKeySecurity = new CryptoKeySecurity();

            cryptoKeySecurity.SetAccessRule(rule);

            //cryptoKeySecurity.SetOwner(rule);

            cp.KeyContainerName = containerName;
            cp.Flags |= CspProviderFlags.UseMachineKeyStore;
            cp.CryptoKeySecurity = cryptoKeySecurity;

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
                CspParameters cp = GetCspParameters(containerName);

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

        public void ReadKey(string key)
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
                if (_rsaProvider != null)
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
