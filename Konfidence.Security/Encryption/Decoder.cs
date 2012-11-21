using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Konfidence.Security.Encryption
{
    public class Decoder : IDisposable
    {
        private KeyEncryption _Decoder = null;
        private bool _Disposed = false;

        public Decoder(string privateKey)
        {
            _Decoder = new KeyEncryption(string.Empty);

            _Decoder.ReadKey(privateKey);
        }

        public string Decrypt(object[] encryptedData)
        {
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            ArrayList encryptedDataList = new ArrayList();
            StringBuilder rawData = new StringBuilder();

            foreach (object objectItem in encryptedData)
            {
                encryptedDataList.Add(objectItem);
            }

            foreach (byte[] byteData in encryptedDataList)
            {
                byte[] decryptedByteData;

                decryptedByteData = _Decoder.RsaProvider.Decrypt(byteData, false);

                rawData = rawData.Append(asciiEncoding.GetString(decryptedByteData));
            }

            encryptedDataList.Clear();

            return rawData.ToString();
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
                if (_Decoder != null)
                {
                    _Decoder.Dispose(); // resources vrijgeven.

                    _Decoder = null;
                }
            }
            _Disposed = true;

        }

        #endregion
    }
}
