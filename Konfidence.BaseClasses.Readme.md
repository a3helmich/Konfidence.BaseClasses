# Konfidence.BaseClasses

Shortcuts and base implementations I use across my own software: small, fluent helpers that make code read better, plus a few things I got tired of rewriting.

Part of the [Konfidence.BaseClasses](https://github.com/a3helmich/Konfidence.BaseClasses) collection of libraries.

## What is in it

- **Fluent checks** — `.IsAssigned()`, `.IsEof()`, `.IsGuid()`, `.IsNumeric()`, `.StartOfDayTime()`, `.EndOfDayTime()`
- **JSON and CSV** — a default-configured System.Text.Json serializer/deserializer, a JSON-roundtrip `Clone()`, and `DeserializeCsv<T>(..)` based on CsvHelper
- **String extensions** — trimming (with and without case sensitivity), `ReplaceIgnoreCase(..)`, `InitLowerCase()`/`InitUpperCase()`, `ToDecimal()`
- **File paths** — walk up parent directories to find a file or folder
- **Command line arguments** — a small parser for the configuration argument line used with MS dependency injection
- **Environment variables** — unified over user, machine and process scope; Windows and Linux
- **Tasks** — `DefaultIfCanceled(..)`, to fall back to a default instead of throwing on a cancelled `Task`
- **Wpf/BaseViewModel** — an `INotifyPropertyChanged` base class with a `SetField(..)` change-detecting setter and a nesting `SuppressNotifications()` scope

Targets **net9.0** and **net10.0**.

## Breaking changes

**2026.4** — `Wpf/BaseViewModel.SetFrozenField(..)` was removed. It raised `PropertyChanged` and then restored the field to its previous value. If you relied on it to force a re-read of a bound property, call `OnPropertyChanged(..)` directly instead.

## Full documentation

Per-method detail, the other libraries in the collection, and build/test instructions are in the
[README on github.com](https://github.com/a3helmich/Konfidence.BaseClasses#readme).
