using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Konfidence.Base;

namespace Konfidence.Security.Encryption
{
    public class Encoder : BaseItem, IDisposable
    {
        private EncoderDecoder _Encoder = null;
        private bool _Disposed = false;

        public Encoder(string publicKey)
        {
            _Encoder = new EncoderDecoder();

            _Encoder.ReadKey(publicKey);
        }

        public int KeySize
        {
            get
            {
                return _Encoder.RsaProvider.KeySize;
            }
        }

        public ArrayList Encrypt(string rawData)
        {
            ArrayList byteList = null;

            if (IsAssigned(rawData))
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

            return byteList;
        }

        private byte[] GetEnryptedDataBlock(string partialString)
        {
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();

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

            if (!this._Disposed)
            {
                if (_Encoder != null)
                {
                    _Encoder.Clear(); // resources vrijgeven.

                    _Encoder = null;
                }
            }
            _Disposed = true;

        }

        #endregion
    }
}
