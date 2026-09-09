# Konfidence.Logging

Serilog-backed application logging abstraction. Provides a small, mockable `IApplicationLogger`
facade over Serilog, with sensible console/file/debug sink defaults and dependency-injection wiring.

Part of the [Konfidence.BaseClasses](https://github.com/a3helmich/Konfidence.BaseClasses) collection of libraries.

## What is in it

- **`IApplicationLogger` / `IApplicationLoggerFactory`** — the mockable logging facade consumers depend on; `Error`, `Verbose`, `Information` (each with `[CallerMemberName]` and optional `LogAction`/object payload) and `InformationRaw`
- **`ApplicationLoggerFactory`** — creates `IApplicationLogger` instances, and builds fully configured Serilog loggers (`GetFileOnlyLogger` for a lightweight file-only logger, plus internal helpers for the full console/file/debug application logger)
- **`LogAction`** — enum for common start/stop/exception logging phases, rendered into log messages
- **`LoggingDependencyInjectionExtensions.AddLoggingServices(...)`** — registers `IApplicationLoggerFactory`/`IApplicationLogger` in an `IServiceCollection` and configures the static `Serilog.Log.Logger`

Targets **net9.0** and **net10.0**.

## Full documentation

The other libraries in the collection, and build/test instructions, are in the
[README on github.com](https://github.com/a3helmich/Konfidence.BaseClasses#readme).
