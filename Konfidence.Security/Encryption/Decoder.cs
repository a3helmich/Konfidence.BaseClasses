using System;
using System.Collections;
using System.Text;
using Konfidence.Base;

namespace Konfidence.Security.Encryption
{
    public class Decoder : IDisposable
    {
        private KeyEncryption _Decoder;
        private bool _Disposed;

        public Decoder(string privateKey)
        {
            _Disposed = false;

            _Decoder = new KeyEncryption(string.Empty, new Configuration());

            _Decoder.ReadKey(privateKey);
        }

        public string Decrypt(object[] encryptedData)
        {
            var asciiEncoding = new ASCIIEncoding();
            var encryptedDataList = new ArrayList();
            var rawData = new StringBuilder();

            foreach (object objectItem in encryptedData)
            {
                encryptedDataList.Add(objectItem);
            }

            foreach (byte[] byteData in encryptedDataList)
            {
                byte[] decryptedByteData = _Decoder.RsaProvider.Decrypt(byteData, false);

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

            if (!_Disposed)
            {
                if (_Decoder.IsAssigned())
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
