# Konfidence.Mail

A base SMTP client implementation: `new BaseMailSender(..)` → `SendEmail(..)`.

Part of the [Konfidence.BaseClasses](https://github.com/a3helmich/Konfidence.BaseClasses) collection of libraries.

## What is in it

- **`BaseMailSender`** — wraps `System.Net.Mail.SmtpClient` with basic-auth credentials, an optional HTML body and a single file attachment. Send failures are swallowed into a `bool` result rather than thrown
- **`MailAccounts` / `MailConstants`** — supporting constants and config for known mail accounts

Targets **net9.0** and **net10.0**.

## Full documentation

The other libraries in the collection, and build/test instructions, are in the
[README on github.com](https://github.com/a3helmich/Konfidence.BaseClasses#readme).
