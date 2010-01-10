using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

using Konfidence.Base;

namespace Konfidence.Security.Encryption
{
    internal class EncoderDecoder : ServerKeyEncryption
    {
        public EncoderDecoder() : base(0, string.Empty)
        {

        }

        internal void Clear()
        {
            Dispose();
        }
    }
}
