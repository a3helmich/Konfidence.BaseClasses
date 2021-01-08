﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.Security.Encryption
{
    public class Decoder : IDisposable
    {
        private KeyEncryption _decoder;
        private bool _disposed;

        public Decoder([NotNull] string privateKey)
        {
            _disposed = false;

            _decoder = new KeyEncryption(string.Empty, new SecurityConfiguration());

            _decoder.ReadKey(privateKey);
        }

        [NotNull]
        public string Decrypt([NotNull] List<List<byte>> encryptedData)
        {
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
                if (_decoder.IsAssigned())
                {
                    _decoder.Dispose(); // resources vrijgeven.

                    _decoder = null;
                }
            }
            _disposed = true;

        }

        #endregion
    }
}
