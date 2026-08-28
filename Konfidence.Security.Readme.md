# Konfidence.Security

Creation and retrieval of public and private RSA keys, saved in and deleted from (secured) local storage. Encode with a shared public key, decode with your secret private key.

Part of the [Konfidence.BaseClasses](https://github.com/a3helmich/Konfidence.BaseClasses) collection of libraries.

## What is in it

- **`PrivatePublicKey`** — creates or loads an RSA key pair for a named application, and can delete its store
- **`Encryption/Encoder` and `Encryption/Decoder`** — split a string into key-size-limited blocks and RSA-encrypt/decrypt each block: `Encoder.Encrypt(..)`, `Decoder.Decrypt(..)`
- **`Encryption/KeyEncryption`** — the actual RSA key generation and reading, and the secured local storage read/write/delete
- **`ISecurityConfiguration` / `SecurityConfiguration`** — configuration for where and how keys are stored

Targets **net9.0** and **net10.0**. Key containers are stored through the Windows CryptoAPI machine key store.

## Full documentation

The other libraries in the collection, and build/test instructions, are in the
[README on github.com](https://github.com/a3helmich/Konfidence.BaseClasses#readme).
