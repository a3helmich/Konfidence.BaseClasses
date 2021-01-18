using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.Security.Encryption
{
    public class Encoder : IDisposable
    {
        private KeyEncryption _encoder;
        private bool _disposed;

        public Encoder([NotNull] string publicKey)
        {
            _disposed = false;

            _encoder = new KeyEncryption(string.Empty);

            _encoder.ReadKey(publicKey);
        }

        public int KeySize => _encoder.RsaProvider.KeySize;

        [CanBeNull]
        public List<List<byte>> Encrypt(string rawData)
        {
            List<List<byte>> byteList = null;

            if (rawData.IsAssigned())
            {
                byteList = new List<List<byte>>();

                var partialString = rawData;

                while (partialString.Length > _encoder.PackageSize)
                {
                    var partialStringBlock = partialString.Substring(0, _encoder.PackageSize);

                    byteList.Add(GetEncryptedDataBlock(partialStringBlock));

                    partialString = GetNextPartialString(partialString, _encoder.PackageSize);
                }

                byteList.Add(GetEncryptedDataBlock(partialString));
            }

            if (byteList.IsAssigned())
            {
                return byteList.ToList();
            }

            return null;
        }

        [NotNull]
        private List<byte> GetEncryptedDataBlock([NotNull] string partialString)
        {
            var asciiEncoding = new ASCIIEncoding();

            var byteData = asciiEncoding.GetBytes(partialString);

            var encryptedData = _encoder.RsaProvider.Encrypt(byteData, false);

            return encryptedData.ToList();
        }

        [NotNull]
        private static string GetNextPartialString([NotNull] string fullString, int packageSize)
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

            if (!_disposed)
            {
                if (_encoder.IsAssigned())
                {
                    _encoder.Dispose(); 

                    _encoder = null;
                }
            }
            _disposed = true;

        }

        #endregion
    }
}
