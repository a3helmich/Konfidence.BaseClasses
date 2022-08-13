using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.Security.Encryption
{
    public sealed class Decoder : IDisposable
    {
        private readonly KeyEncryption _decoder;
        private bool _disposed;

        public Decoder(string privateKey)
        {
            _disposed = false;

            _decoder = new KeyEncryption(string.Empty);

            _decoder.ReadKey(privateKey);
        }

        [UsedImplicitly]
        public string Decrypt(List<List<byte>> encryptedData)
        {
            if (!_decoder.RsaProvider.IsAssigned())
            {
                return string.Empty;
            }

            var asciiEncoding = new ASCIIEncoding();
            var encryptedDataList = new ArrayList();
            var rawData = new StringBuilder();

            foreach (var objectItem in encryptedData)
            {
                encryptedDataList.Add(objectItem);
            }

            foreach (List<byte> byteData in encryptedDataList)
            {
                var decryptedByteData = _decoder.RsaProvider.Decrypt(byteData.ToArray(), false);

                rawData = rawData.Append(asciiEncoding.GetString(decryptedByteData));
            }

            encryptedDataList.Clear();

            return rawData.ToString();
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {

            if (!_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (_decoder.IsAssigned())
                {
                    _decoder.Dispose();
                }
            }

            _disposed = true;
        }
    }
}
