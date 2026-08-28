# Konfidence.BaseClasses

A collection of dotnet library projects I use for the software I develop, containing shortcuts or base implementations that make developing and maintaining software a lot easier.

The `Konfidence.BaseClasses` package is on [nuget.org](https://www.nuget.org/packages/Konfidence.BaseClasses); the rest are published to an internal share.

## Contents

| Library | What it is for |
| --- | --- |
| [Konfidence.BaseClasses](#konfidencebaseclasses-1) | Fluent extensions, JSON/CSV serialization, string helpers, WPF view model base |
| [Konfidence.BaseDataBaseClasses](#konfidencebasedatabaseclasses) | CRUD on SQL without EntityFramework, for ClassGenerator-generated data items |
| [Konfidence.BaseRest.Client](#konfidencebaserestclient) | Basic REST service access on top of RestSharp |
| [Konfidence.BaseThreadClasses](#konfidencebasethreadclasses) | A manage-run-action pattern for simple threading |
| [Konfidence.DataBaseInterface](#konfidencedatabaseinterface) | The contracts shared between the data and SQL libraries |
| [Konfidence.Mail](#konfidencemail) | Base SMTP client implementation |
| [Konfidence.Security](#konfidencesecurity) | RSA key creation/storage, encoding and decoding |
| [Konfidence.SqlDataAccess](#konfidencesqldataaccess) | Focused replacement for the Enterprise Library data block |
| [Konfidence.SqlHostProvider](#konfidencesqlhostprovider) | MS SQL access and database structure introspection |
| [Konfidence.TestTools](#konfidencetesttools) | Test configuration wiring for live SQL Server access |
| [Konfidence.Integration.TestClasses](#konfidenceintegrationtestclasses) | Shared generated test fixtures |
| [Konfidence.UtilHelper](#konfidenceutilhelper) | Obsolete classes. Technical debt :( |
| [Tools/ClientSettingsUpdater](#toolsclientsettingsupdater) | Keeps SQL secrets out of the repository, as a dotnet tool |
| [Test](#test) | The test projects |

## Using the libraries

All projects generate a `.nupkg` on build, ready to include in your own projects. Most libraries are built for **net9.0** and **net10.0**. Support for **netstandard2.0** is only available in versions up to 2022.1.x; after that only **net5.0** and **net6.0** are included.

### Versioning

Versions are `year.major.minor`, CalVer-style:

- **year** — the leading segment. NuGet reads this as the semantic major, so the version *appears* to major-bump each January whether or not anything actually broke.
- **major** — hand-maintained. Restarts at 1 each year, bumped mid-year only for a deliberate breaking change.
- **minor** — a build counter, incremented per pipeline run.

### Breaking changes

**2026.4** — `Wpf/BaseViewModel.SetFrozenField(..)` was removed. It raised `PropertyChanged` and then restored the field to its previous value: a "notify but do not actually change" setter. If you relied on it to force a re-read of a bound property, call `OnPropertyChanged(..)` directly instead.

## Build and test

1. Clone Konfidence.BaseClasses
2. Open `Konfidence.BaseClasses.sln` in Visual Studio
3. Build the solution

Tests run from Visual Studio, or with `dotnet test Konfidence.BaseClasses.sln`, or per project with `dotnet test [testprojectname].csproj`.

The test projects come in three kinds:

| Kind | Needs | Notes |
| --- | --- | --- |
| `*.UnitTest` | nothing | No database, no network. Runs anywhere. |
| `*.LocalDb.UnitTest` | SQL Server LocalDB | Attaches a checked-in `TestClassGenerator.mdf` snapshot under a database name unique to the running process. Covers the SQL code paths without a reachable server, which is what lets GitHub Actions run them. |
| `*.IntegrationTest` | a live SQL Server | Targets `konfidence2`/`konfidence3` and will fail until an equivalent test database setup is available. |

> **Running from a runner that parallelises target frameworks?** ReSharper does; `dotnet test` does not, because `Directory.Build.props` sets `TestTfmsInParallel=false`. The `*.IntegrationTest` projects share one live database, so anything assuming exclusive access to it fails intermittently. The fixtures here only ever touch rows they created themselves, and `DatabaseStructure` gives its helper stored procedures per-run names, for exactly this reason.

## Libraries

### Konfidence.BaseClasses

**Fluent extensions**, to make reading some patterns easier:

- `.IsAssigned()` — whether an object/string/guid/DateTime/TimeSpan is actually assigned
- `.IsEof()` — eof for stream reading
- `.IsGuid()`, `.IsNumeric()` — what a string holds
- `.StartOfDayTime()`, `.EndOfDayTime()` — earliest and latest time on a day

**Serialization**, a default-configured JSON serializer/deserializer based on System.Text.Json:

- `Serialize(..)` / `SerializeBytes(..)` — with an optional compact (non-indented) mode; enums written as strings
- `Deserialize(..)` — from `string` or `ReadOnlySpan<byte>`, with a case-sensitive option
- `Clone()` — a JSON-roundtrip deep clone. Note `[JsonIgnore]` properties are **not** preserved: they are forced into the written JSON but the read side still honours `[JsonIgnore]` and drops them, so a clone behaves like a plain serialize/deserialize round trip
- `DeserializeCsv<T>(..)` — CSV-to-object-list parsing, based on CsvHelper

**String extensions**, unexpectedly fast:

- `TrimStart(..)`, `TrimStartIgnoreCase(..)`, `TrimEnd(..)`, `TrimEndIgnoreCase(..)`, `TrimList()`
- `ReplaceIgnoreCase(..)`
- `InitLowerCase()`, `InitUpperCase()`
- `ToDecimal()` — parse a string into a decimal
- `Contains(..)` with a specified casing type — superseded by the framework's own `string.Contains(string, StringComparison)`, which has the same signature and beats an extension method at overload resolution. Only reachable as `StringExtensions.Contains(value, ..)`; new code should use the framework method

**Other:**

- **CommandLineArgument parser** — for the configuration argument line parser used with MS dependency injection. Parses `-argument=value` / `-argument:value` switches; not a full-fledged argument parser
- **Environment variable getter** — unified over user, machine and process scope. Works on Windows and Linux
- **FilePathExtensions** — walks up parent directories to find a file or directory (`TryFindFile`, `TryFindFileIncludingSubFolders`, `TryFindDirectory`), plus `TryCreateAndValidateDirectory()`
- **TaskExtensions** — `DefaultIfCanceled(..)`, to fall back to a default value or completed task instead of throwing when a `Task` was cancelled
- **Wpf/BaseViewModel** — a small `INotifyPropertyChanged` base class for WPF view models: a `SetField(..)` change-detecting setter and a `SuppressNotifications()` scope to batch/mute property-changed events. Suppression nests, so notifications resume only once the outermost scope is disposed. (`SetFrozenField(..)` was removed in 2026.4 — see [Breaking changes](#breaking-changes))

### Konfidence.BaseDataBaseClasses

Classes that make CRUD on SQL really easy, without the strong dependencies included in EntityFramework. Only useful with my ClassGenerator. Also referenced by the Konfidence.SqlHostProvider package.

- `BaseDataItem` — abstract base for generated data items. Tracks its own id/guid key, its stored-procedure names (get/save/delete/get-by-guid) and the parameter list to send to the database, delegating the actual reads and writes to an injected `IBaseClient`
- `FieldExtensions` / `AutoUpdateFieldExtensions` — typed `SetField(..)` overloads (int, guid, string, decimal, DateTime, TimeSpan, ...) that register stored-procedure parameters on a `BaseDataItem`, plus tracking of server-generated/auto-update fields
- `DataReaderExtensions` — typed `IDataReader.GetField(..)` helpers, used while mapping a row back onto a data item
- `Sp/SpParameter(Extensions)` — the stored-procedure parameter model (name, `DbType`, value) shared between data items and the SQL client

### Konfidence.BaseRest.Client

Client for basic REST service access, using the RestSharp client. Used by my ClassGenerator.

- `BaseRestClient` — thin async wrapper (`GetAsync<T>`, `PostAsync<T>`) around RestSharp that JSON-serializes the request body, adds optional headers and deserializes the response into `T`
- `IRestClientConfig` / `RestClientConfig` — supplies the base URI the client is configured against

### Konfidence.BaseThreadClasses

A basic manage-run-action pattern for simple threading. Consider using simpler Tasks before reaching for this library.

- `ThreadManager<TAction>` — starts and stops a background `ThreadRunner`, exposing hooks (`SetInitializeAction`, `SetBeforeExecuteAction`, `SetAfterExecuteAction`) that run around each execution of a `ThreadAction`
- `ThreadRunner<TAction>` / `ThreadAction` — the underlying loop, sleeping between runs for a configurable interval (`SleepUnit`: seconds/minutes/...)

### Konfidence.DataBaseInterface

Interfacing between Konfidence.BaseDataBaseClasses and Konfidence.SqlHostProvider. And of course dependency injection infra.

- `IBaseClient` — the CRUD contract a data item talks to (get/save/delete/get-list, plus table/view/stored-procedure existence checks)
- `IDataRepository` — the lower-level contract that actually executes stored procedures and text commands against ADO.NET (`IDataReader`/`DataTable`), implemented by `Konfidence.SqlHostProvider`
- `IBaseDataItem` / `ISpParameterData` — the shape of a data item and of a single stored-procedure parameter, so the two libraries can reference each other without a hard dependency

### Konfidence.Mail

Base SMTP client implementation: `new BaseMailSender(..)` → `SendEmail(..)`.

- `BaseMailSender` — wraps `System.Net.Mail.SmtpClient` with basic-auth credentials, an optional HTML body and a single file attachment. Send failures are swallowed into a `bool` result rather than thrown
- `MailAccounts` / `MailConstants` — supporting constants and config for known mail accounts

### Konfidence.Security

Creation and retrieval of public and private RSA keys, saved in and deleted from (secured) local storage. Encoding with a shared public key, decoding with your secret private key.

- `PrivatePublicKey` — creates or loads an RSA key pair for a named application (via `KeyEncryption`), and can delete its store
- `Encryption/Encoder` and `Encryption/Decoder` — split a string into key-size-limited blocks and RSA-encrypt/decrypt each block (`Encoder.Encrypt(..)`, `Decoder.Decrypt(..)`)
- `Encryption/KeyEncryption` — the actual RSA key generation and reading, and the secured local storage read/write/delete
- `ISecurityConfiguration` / `SecurityConfiguration` — configuration for where and how keys are stored

### Konfidence.SqlDataAccess

A small, focused replacement for the SQL access parts of `EnterpriseLibrary.Data.NetCore` (the old Enterprise Library Data Access Application Block), built directly on `Microsoft.Data.SqlClient`. It exists so `Konfidence.SqlHostProvider` no longer depends on that ~15-year-old, largely frozen package, which itself dragged in the deprecated `System.Data.SqlClient`.

- `SqlDatabase` + `SqlDatabaseFactory` — a stateless, connection-string-driven executor (`CreateConnection`, `GetStoredProcCommand`, `AddInParameter`/`AddParameter`, `ExecuteNonQuery`, `ExecuteReader`, `GetParameterValue`) mirroring the narrow slice of EL's `Database` API that was actually used. Including its connection lifecycle: `ExecuteReader` uses `CommandBehavior.CloseConnection`, so disposing the reader also closes the connection
- `DatabaseSettings` — a drop-in `ConfigurationSection` replacement for EL's `DatabaseSettings`, read from `app.config`'s `<dataConfiguration defaultDatabase="..." />` section
- Has no dependency on `Konfidence.SqlHostProvider` or any other Konfidence package — the dependency only flows one way

### Konfidence.SqlHostProvider

Provides MS SQL database access, now via `Konfidence.SqlDataAccess`/`Microsoft.Data.SqlClient` instead of the old enterprise libraries. Configured with `app.config` and `SqlClientSettings.json`, and able to manipulate app.config settings directly or in memory. Can return a DatabaseStructure describing a database: its tables, columns, types and some constraints. Used by my ClassGenerator and its generated artifacts.

- `DependencyInjectionFactory` — builds an `IServiceProvider` wired up with `SqlClient`, `SqlClientRepository`, `DatabaseStructure` and `IClientConfig`, reading configuration from `SqlClientSettings.json` plus command-line overrides (config folder / default database)
- `SqlAccess/SqlClient` + `SqlClientRepository` — implement `IBaseClient`/`IDataRepository` against MS SQL, executing the get/save/delete/list stored procedures and schema-existence checks generated for a data item
- `SqlAccess/ClientConfig` + `ClientConfigExtensions` / `ClientSettings` / `ConfigConnectionString` — connection configuration bound from `SqlClientSettings.json`, including reading and writing settings straight from and to `app.config`
- `SqlConnectionManagement/ConnectionManagement` — connection-string assembly and constants
- `SqlDbSchema/DatabaseStructure` (+ `TableDataItem`, `ColumnDataItem`, `PrimaryKeyDataItem`, `IndexDataItem`, `SpName`) — reads a database's structure (tables, columns, types, primary keys, indexes), the basis for the ClassGenerator's code generation. It does so by temporarily installing helper stored procedures and removing them again. Those helpers get a name unique to each run (`CG_..._{processId}_{guid}`) and are dropped in a `finally`, so two processes introspecting the same database concurrently cannot drop each other's procedures, and a failed run leaves nothing behind

### Konfidence.TestTools

Prepares the configuration of a unit test with live access to SQL Server. Since dotnet, the location of the TestHost and where the tests actually run differ, and app.config not being in the expected location is an issue.

### Konfidence.Integration.TestClasses

Shared test fixtures and generated SQL, used by the `*.IntegrationTest` projects that exercise `Konfidence.SqlHostProvider`/`Konfidence.BaseDataBaseClasses` against a real SQL Server instance.

### Konfidence.UtilHelper

Some obsolete classes. Technical debt :(

- `ApplicationSettings` / `IApplicationSettings` / `ApplicationSettingsFactory` — legacy `.settings` XML file get/set-string-value store
- `BaseApplicationConfiguration` — legacy XML app.config-style read/write of string/bool/byte-array node values
- `BaseXmlDocument` — thin XML document base helper

### Tools/ClientSettingsUpdater

For me: updates the `SqlClientSettings.json` in a build pipeline, keeping secrets out of the git repository. Packed as an installable dotnet tool.

- `ClientSettingsUpdater.UnitTest` — unit tests for the `ClientSettingsManager` update logic

### Test

Per-library unit test projects (`Konfidence.*.UnitTest`), plus `Test/TestByHandApp`, a small manual/exploratory console app for ad-hoc verification against a live SQL Server.

- `Konfidence.SqlDataAccess.UnitTest` — fast unit tests for `SqlDatabase`/`SqlDatabaseFactory`/`DatabaseSettings`
- `Konfidence.SqlHostProvider.UnitTest` — mocked, database-free tests for `SqlClient`, `ClientConfig(Extensions)`, `ColumnData(Extensions)`, `DependencyInjectionFactory` and the file-handling half of `ConnectionManagement`
- `Konfidence.SqlHostProvider.IntegrationTest` — the live-SQL-Server tests: `DataBaseStructureTests`, connection resolution, and the stored-procedure save/get/get-by/get-list/delete round trips
- `Konfidence.TestClasses.IntegrationTest` — runs ClassGenerator-generated `Dl.*DataItem` classes (from `Konfidence.Integration.TestClasses`) against a live SQL Server, using `Konfidence.TestTools` to wire up the test configuration and security settings
- `*.LocalDb.UnitTest` (`Konfidence.SqlHostProvider`, `Konfidence.BaseDatabaseClasses`, `Konfidence.TestClasses`) — the same kind of SQL coverage against a LocalDB-attached snapshot instead of a live server, so it runs on GitHub Actions. The Azure DevOps agent filters these out (`FullyQualifiedName!~LocalDb.UnitTest`) because its Windows-service session cannot run LocalDB, and relies on the `*.IntegrationTest` projects for that surface instead
