using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;

namespace Konfidence.Security.Encryption
{
    public sealed class Encoder : IDisposable
    {
        private readonly KeyEncryption _encoder;
        private bool _disposed;

        public Encoder(string publicKey)
        {
            _disposed = false;

            _encoder = new KeyEncryption(string.Empty);

            _encoder.ReadKey(publicKey);
        }

        public int KeySize => _encoder.RsaProvider?.KeySize??-1;

        public List<List<byte>>? Encrypt(string rawData)
        {
            List<List<byte>> byteList = new List<List<byte>>();

            if (rawData.IsAssigned())
            {
                var partialString = rawData;

                while (partialString.Length > _encoder.PackageSize)
                {
                    var partialStringBlock = partialString.Substring(0, _encoder.PackageSize);

                    byteList.Add(GetEncryptedDataBlock(partialStringBlock));

                    partialString = GetNextPartialString(partialString, _encoder.PackageSize);
                }

                byteList.Add(GetEncryptedDataBlock(partialString));
            }

            if (byteList.Any())
            {
                return byteList.ToList();
            }

            return null;
        }

        private List<byte> GetEncryptedDataBlock(string partialString)
        {
            if (!_encoder.RsaProvider.IsAssigned())
            {
                return new List<byte>();
            }
            var asciiEncoding = new ASCIIEncoding();

            var byteData = asciiEncoding.GetBytes(partialString);

            var encryptedData = _encoder.RsaProvider.Encrypt(byteData, false);

            return encryptedData.ToList();
        }

        private static string GetNextPartialString(string fullString, int packageSize)
        {
            return fullString.Substring(packageSize, fullString.Length - packageSize);
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
                if (_encoder.IsAssigned())
                {
                    _encoder.Dispose(); 
                }
            }

            _disposed = true;
        }
    }
}
