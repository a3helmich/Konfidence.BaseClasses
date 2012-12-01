using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Konfidence.Base;
using System.Security.AccessControl;

namespace Konfidence.Security.Encryption
{
    public class KeyEncryption : BaseItem, IDisposable
    {
        private RSACryptoServiceProvider _RsaProvider;

        private bool _Disposed = false;

        private RSACryptoServiceProvider _TempRsaProvider = null;
        private int _TempKeySize = 0;

        private string _ContainerName;

        // _MaxBytes schijnt uit te vinden te zijn, maar is een 
        // beetje vreemd heb onderstussen het een en ander 
        // uitgezocht, maar de maximum datasize = keysize lijkt
        // niet waar te zijn heb voorlopig gekozen voor halverwege 
        // keysize, dit lijkt te voldoen, moet verder uitgezocht
        //NB nov 2012: zal wel iets te maken hebben met de encoding van de string (onebyte/twobyte)
        private int _MaxBytesServer = 0; // default voor de serverside
        private int _MaxBytesClient = 0; // default voor de serverside

        #region properties
        public RSACryptoServiceProvider RsaProvider
        {
            get { return _RsaProvider; }
        }

        public string PublicKey
        {
            get
            {
                return _RsaProvider.ToXmlString(false);
            }
        }

        public string PrivateKey
        {
            get
            {
                return _RsaProvider.ToXmlString(true);
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
                return _MaxBytesServer / 2;
            }
        }
        #endregion properties

        protected RSACryptoServiceProvider TempKeyContainer
        {
            get
            {
                int keySizeClient = _MaxBytesClient * 8;
                int keySizeServer = GetMaxKeySize(); // dit valt te optimaliseren, door er voor te zorgen dat de oude niet direct wordt weggemikt

                if (keySizeServer < keySizeClient)
                {
                    keySizeClient = keySizeServer;
                }

                if (!IsAssigned(_TempRsaProvider) || _TempKeySize != keySizeClient)
                {
                    if (keySizeClient == 0)
                    {
                        _TempRsaProvider = new RSACryptoServiceProvider();
                    }
                    else
                    {
                        _TempRsaProvider = new RSACryptoServiceProvider(keySizeClient);
                    }

                    _TempKeySize = _TempRsaProvider.KeySize;
                }

                return _TempRsaProvider;
            }
        }

        private int GetMaxKeySize()
        {
            // at first i wanted to remove redundant keys etc. but keeping them in 
            // store is faster for non-key generating actions. like here, but also for encoding and decoding
            // Rsa.PersistKeyInCsp = false;  // don't want to keep this in storage
            RSACryptoServiceProvider keyContainer = new RSACryptoServiceProvider();

            KeySizes legalKeySize = keyContainer.LegalKeySizes[0];

            switch (Environment.OSVersion.Platform)
            {
                case System.PlatformID.Win32S:
                case System.PlatformID.Win32Windows:
                    {
                        _MaxBytesServer = legalKeySize.MinSize / 8;
                        break;
                    }
                default:
                    {
                        _MaxBytesServer = keyContainer.KeySize / 8;
                        break;
                    }
            }

            if (_MaxBytesClient == 0)
            {
                _MaxBytesClient = _MaxBytesServer;
            }

            return _MaxBytesServer * 8;
        }

        public KeyEncryption(string containerName) : this(0, containerName)
        {
        }

        public KeyEncryption(int keySize, string containerName)
        {
            _MaxBytesClient = keySize / 8;
            _MaxBytesServer = keySize / 8;
            _ContainerName = containerName;

            bool isTemporary = false;

            if (string.IsNullOrEmpty(_ContainerName))
            {
                isTemporary = true;

                _ContainerName = "None";
            }

            Debug.WriteLine("Encryption: Utilhelper.ServerKeyEncryption(...) key - " + _ContainerName);

            if (isTemporary)
            {
                _RsaProvider = TempKeyContainer;
            }
            else
            {
                GetKeyContainer(_ContainerName);
            }

            Debug.WriteLine("Encryption: Utilhelper.ServerKeyEncryption(...) gotcontainer");
        }

        public static int MaxKeySize()
        {
            using (KeyEncryption keyEncryption = new KeyEncryption(string.Empty))
            {
                return keyEncryption.KeySize;
            }
        }

        private CspParameters GetCspParameters(string containerName)
        {
            CspParameters cp = new CspParameters();

            string user = @"NT AUTHORITY\NETWORK SERVICE";

            CryptoKeyAccessRule rule = new CryptoKeyAccessRule(user, CryptoKeyRights.GenericRead | CryptoKeyRights.ReadData | CryptoKeyRights.ReadAttributes | CryptoKeyRights.Delete | CryptoKeyRights.FullControl | CryptoKeyRights.TakeOwnership | CryptoKeyRights.ChangePermissions, AccessControlType.Allow);

            CryptoKeySecurity cryptoKeySecurity = new CryptoKeySecurity();

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

                if (!IsAssigned(_RsaProvider))
                {
                    try
                    {
                        _RsaProvider = new RSACryptoServiceProvider(_MaxBytesServer * 8, cp);
                    }
                    catch (CryptographicException e)
                    {
                        {
                            throw new Exception("create: " + e.Message, e);
                        }
                    }
                }

                if (_MaxBytesServer > 0 && _RsaProvider.KeySize != _MaxBytesServer * 8)
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


                        if (!IsAssigned(_RsaProvider))
                        {
                            _RsaProvider = new RSACryptoServiceProvider(_MaxBytesServer * 8, cp);
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

                if (IsAssigned(_RsaProvider))
                {
                    // Delete the key entry in the container.
                    _RsaProvider.PersistKeyInCsp = false;

                    // Call Clear to release resources and delete the key from the container.
                    _RsaProvider.Clear();

                    _RsaProvider = null;

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
            _RsaProvider.FromXmlString(key);
            _MaxBytesServer = _RsaProvider.KeySize / 8;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {

            if (!this._Disposed)
            {
                if (_RsaProvider != null)
                {
                    _RsaProvider.Clear(); // resources vrijgeven.

                    _RsaProvider = null;
                }
            }
            _Disposed = true;

        }

        #endregion
    }
}
