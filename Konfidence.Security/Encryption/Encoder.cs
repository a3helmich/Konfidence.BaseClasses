using System;
using System.Collections;
using System.Text;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.Security.Encryption
{
    public class Encoder : IDisposable
    {
        private KeyEncryption _encoder;
        private bool _disposed;

        public Encoder(string publicKey)
        {
            _disposed = false;

            _encoder = new KeyEncryption(string.Empty, new Configuration());

            _encoder.ReadKey(publicKey);
        }

        public int KeySize => _encoder.RsaProvider.KeySize;

        [CanBeNull]
        public object[] Encrypt(string rawData)
        {
            ArrayList byteList = null;

            if (rawData.IsAssigned())
            {
                byteList = new ArrayList();

                var partialString = rawData;

                while (partialString.Length > _encoder.PackageSize)
                {
                    var partialStringBlock = partialString.Substring(0, _encoder.PackageSize);

                    byteList.Add(GetEnryptedDataBlock(partialStringBlock));

                    partialString = GetNextPartialString(partialString, _encoder.PackageSize);
                }

                byteList.Add(GetEnryptedDataBlock(partialString));
            }

            if (byteList.IsAssigned())
            {
                return byteList.ToArray();
            }

            return null;
        }

        private byte[] GetEnryptedDataBlock([NotNull] string partialString)
        {
            var asciiEncoding = new ASCIIEncoding();

            var byteData = asciiEncoding.GetBytes(partialString);

            var encryptedData = _encoder.RsaProvider.Encrypt(byteData, false);

            return encryptedData;
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
                    _encoder.Dispose(); // resources vrijgeven.

                    _encoder = null;
                }
            }
            _disposed = true;

        }

        #endregion
    }
}
