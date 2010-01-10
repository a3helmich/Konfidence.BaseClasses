using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Konfidence.Base;

namespace Konfidence.Security.Encryption
{
    public class ServerKeyEncryption : BaseItem, IDisposable
    {
        private RSACryptoServiceProvider _RsaProvider;

        private bool _Disposed = false;
        private bool _IsClient = false;
        private bool _IsTemporary = false;

        private RSACryptoServiceProvider _TempRsaProvider = null;

        private string _ContainerName;

        // _MaxBytes schijnt uit te vinden te zijn, maar is een 
        // beetje vreemd heb onderstussen het een en ander 
        // uitgezocht, maar de maximum datasize = keysize lijkt
        // niet waar te zijn heb voorlopig gekozen voor halverwege 
        // keysize, dit lijkt te voldoen, moet verder uitgezocht
        private int _MaxBytes = 0; // default voor de serverside

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
                return _RsaProvider.KeySize;
            }
        }

        public int PackageSize
        {
            get
            {
                return _MaxBytes / 2;
            }
        }
        #endregion properties

        public ServerKeyEncryption(int keySize, string containerName)
        {
            _MaxBytes = keySize / 8;
            _ContainerName = containerName;

            if (string.IsNullOrEmpty(_ContainerName))
            {
                _IsTemporary = true;

                //_ContainerName = "temp:" + Guid.NewGuid().ToString(); // get a temporary containerName
                _ContainerName = "None";
            }

            Debug.WriteLine("Encryption: Utilhelper.ServerKeyEncryption(...) key - " + _ContainerName);

            InitializeEncryption();

            //if (_IsTemporary)
            {
                Debug.WriteLine("Encryption: Utilhelper.ServerKeyEncryption(...) initialized");
            }

            if (_IsTemporary)
            {
                _RsaProvider = GetTempKeyContainer();
            }
            else
            {
                GetKeyContainer(_ContainerName);
            }

            //if (_IsTemporary)
            {
                Debug.WriteLine("Encryption: Utilhelper.ServerKeyEncryption(...) gotcontainer");
            }
        }

        protected void SetIsClient(bool isClient)
        {
            _IsClient = isClient;
        }

        protected virtual void InitializeEncryption()
        {
        }

        private CspParameters GetCspParameters(string containerName)
        {
            CspParameters cp = new CspParameters();
            cp.KeyContainerName = containerName;
            cp.Flags |= CspProviderFlags.UseMachineKeyStore;

            return cp;
        }

        private void FindMaxKeySize()
        {
            GetTempKeyContainer();

            // at first i wanted to remove redundant keys etc. but keeping them in 
            // store is faster for non-key generating actions. like here, but also for encoding and decoding
            // Rsa.PersistKeyInCsp = false;  // don't want to keep this in storage

            KeySizes legalKeySize = _TempRsaProvider.LegalKeySizes[0];

            switch (Environment.OSVersion.Platform)
            {
                case System.PlatformID.Win32S:
                case System.PlatformID.Win32Windows:
                    {
                        _MaxBytes = legalKeySize.MinSize / 8;
                        break;
                    }
                default:
                    {
                        _MaxBytes = _TempRsaProvider.KeySize / 8;
                        break;
                    }
            }
        }

        private RSACryptoServiceProvider GetTempKeyContainer()
        {
            if (!IsAssigned(_TempRsaProvider))
            {
                _TempRsaProvider = new RSACryptoServiceProvider();
            }

            return _TempRsaProvider;
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

                if (_IsClient)
                {
                    FindMaxKeySize();
                }

                if (_RsaProvider == null)
                {
                    try
                    {
                        _RsaProvider = new RSACryptoServiceProvider(_MaxBytes * 8, cp);
                    }
                    catch (CryptographicException e)
                    {
                        //if (e.Message.ToLower.StartsWith("object already exixts"))
                        //{
                        //}
                        //else
                        {
                            throw e;
                        }
                    }
                }

                if (_MaxBytes > 0 && _RsaProvider.KeySize != _MaxBytes * 8)
                {
                    DeleteKeyFromContainer();

                    if (_RsaProvider == null)
                    {
                        _RsaProvider = new RSACryptoServiceProvider(_MaxBytes * 8, cp);
                    }

                }

                //if (_IsTemporary)
                //{
                //    _RsaProvider.PersistKeyInCsp = false;
                //}
            }
            catch (CryptographicException e)
            {
                Debug.WriteLine("Encryption: Utilhelper.GetKeyContainer(...) unexpected exception - " + e.Message);
            }
        }

        protected bool DeleteKeyFromContainer()
        {
            try
            {
                // if a rsaprovider exists, make non persistent, clear it and nullify --> the key is deleted

                if (_RsaProvider == null)
                    return false;

                // Delete the key entry in the container.
                _RsaProvider.PersistKeyInCsp = false;

                // Call Clear to release resources and delete the key from the container.
                _RsaProvider.Clear();

                _RsaProvider = null;

                return true;
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
            _MaxBytes = _RsaProvider.KeySize / 8;
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
