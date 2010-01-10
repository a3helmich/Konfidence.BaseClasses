using System;
using System.Collections.Generic;
using System.Text;

namespace Konfidence.Security.Encryption
{
    /// <summary>
    /// Summary description for ClientKeyEncryption.
    /// the Client needs to figure out what the maximumkey size is for the OS it is running on
    /// the ClientKeyEncryption does this, the ServerKeyEncryption doesn't
    /// </summary>
    public class ClientKeyEncryption : ServerKeyEncryption
    {
        public ClientKeyEncryption(string containerName) : base(0, containerName)
        {
        }

        protected override void InitializeEncryption()
        {
            SetIsClient(true);

            base.InitializeEncryption();
        }

        public bool Delete()
        {
            return DeleteKeyFromContainer();
        }
    }
}
