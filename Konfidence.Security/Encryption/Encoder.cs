﻿using System;
using System.Collections;
using System.Text;
using Konfidence.Base;

namespace Konfidence.Security.Encryption
{
    public class Encoder : BaseItem, IDisposable
    {
        private KeyEncryption _Encoder;
        private bool _Disposed;

        public Encoder(string publicKey)
        {
            _Disposed = false;

            _Encoder = new KeyEncryption(string.Empty);

            _Encoder.ReadKey(publicKey);
        }

        public int KeySize
        {
            get
            {
                return _Encoder.RsaProvider.KeySize;
            }
        }

        public object[] Encrypt(string rawData)
        {
            ArrayList byteList = null;

            if (!IsEmpty(rawData))
            {
                byteList = new ArrayList();

                string partialString = rawData;

                while (partialString.Length > _Encoder.PackageSize)
                {
                    string partialStringBlock = partialString.Substring(0, _Encoder.PackageSize);

                    byteList.Add(GetEnryptedDataBlock(partialStringBlock));

                    partialString = GetNextPartialString(partialString, _Encoder.PackageSize);
                }

                byteList.Add(GetEnryptedDataBlock(partialString));
            }

            if (byteList != null)
            {
                return byteList.ToArray();
            }

            return null;
        }

        private byte[] GetEnryptedDataBlock(string partialString)
        {
            var asciiEncoding = new ASCIIEncoding();

            byte[] byteData = asciiEncoding.GetBytes(partialString);

            byte[] encryptedData = _Encoder.RsaProvider.Encrypt(byteData, false);

            return encryptedData;
        }

        private string GetNextPartialString(string fullString, int packageSize)
        {
            return fullString.Substring(packageSize, fullString.Length - packageSize);
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {

            if (!_Disposed)
            {
                if (_Encoder != null)
                {
                    _Encoder.Dispose(); // resources vrijgeven.

                    _Encoder = null;
                }
            }
            _Disposed = true;

        }

        #endregion
    }
}
