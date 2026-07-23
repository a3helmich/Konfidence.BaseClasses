# Konfidence.BaseClasses 
This is a collection of dotnet library projects I use for the software I develop, containing shortcuts or base implementations that make developing and maintaining software a lot easier.
  
# Using the Libraries
All projects generate a .nupkg on build. These can be used to include in your software projects. Most libraries are build for **net9.0** and **net10.0**. Support for **netstandard2.0** is only available in versions upto 2022.1.x, after that only support for **net5.0** and **net6.0** is included.

# Build and Test

- Clone Konfidence.BaseClasses
- Open  Konfidence.BaseClasses.sln in visual studio
- Build the solution

All tests can either be run in visual studio, with dotnet test konfidence.baseclasses.sln or dotnet test [testprojectname].tests.csproj. 

Integration tests running against SQL server will fail until a test database setup is made available.

# Libraries

### Konfidence.BaseClasses
- Some extensions to make reading some patterns more fluent
	- when objects/string/guid/datatime/timespan assignments are actually assigned, .IsAssigned()
	- eof for stream reading, .IsEof()
	- string is a guid, .IsGuid()
	- string is numeric,  .IsNumeric()
	- earliest and latest time on a day, .StartOfDayTime(), .EndOfDayTime()
- CommandLineArgument parser: Meant for the configuration argument line parser used with MS dependency injection. Parses `-argument=value` / `-argument:value` style switches, not a full-fledged argument parser.
- Environment Variable getter: unified(user, machine, process). Should work on both Windows and Linux.
- FilePathExtensions: walks up parent directories to locate a file or directory (`TryFindFile`, `TryFindFileIncludingSubFolders`, `TryFindDirectory`), and `TryCreateAndValidateDirectory()` to ensure a folder exists.
- TaskExtensions: `DefaultIfCanceled(..)` to fall back to a default value/completed task instead of throwing when a `Task` was canceled.
- default configured Json serializer/deserializer, based on System.Text.Json.
	- `Serialize(..)`/`SerializeBytes(..)` with an optional compact (non-indented) mode, enums written as strings.
	- `Deserialize(..)` from `string` or `ReadOnlySpan<byte>`, with a case-sensitive option.
	- `Clone()`: a JSON-roundtrip deep clone, including properties normally hidden by `[JsonIgnore]`.
	- `DeserializeCsv<T>(..)`, CSV-to-object-list parsing based on CsvHelper.
- Some (unexpected) fast string extensions
	- TrimStart(..), TrimStartIgnoreCase(..), TrimEnd(..), TrimEndIgnoreCase(..)
	- TrimList()
	- ReplaceIgnoreCase(..)
	- InitLowerCase(), InitUpperCase()
	- Contains(..) with specified casing type
	- ToDecimal(), parse a string into a decimal
- Wpf/BaseViewModel: a small `INotifyPropertyChanged` base class for WPF view models, with `SetField(..)`/`SetFrozenField(..)` change-detecting setters and a `SuppressNotifications()` scope to batch/mute property-changed events.
  
### The Konfidence.BaseClasses package is available on [nuget.org](https://www.nuget.org/packages/Konfidence.BaseClasses). 

### Konfidence.BaseDataBaseClasses
- Some classes that make CRUD on SQL really easy, without the strong dependencies included in the EntityFramework, only usefull with my ClassGenerator. Also referenced by the Konfidence.SqlHostProvider package.
- `BaseDataItem`: abstract base for generated data items, tracking its own id/guid key, stored-procedure names (get/save/delete/get-by-guid) and the parameter list to send to the database, delegating actual reads/writes to an injected `IBaseClient`.
- `FieldExtensions`/`AutoUpdateFieldExtensions`: typed `SetField(..)` overloads (int, guid, string, decimal, DateTime, TimeSpan, ...) that register stored-procedure parameters on a `BaseDataItem`, plus tracking of server-generated/auto-update fields.
- `DataReaderExtensions`: typed `IDataReader.GetField(..)` helpers used while mapping a row back onto a data item.
- `Sp/SpParameter(Extensions)`: the stored-procedure parameter model (name, `DbType`, value) shared between data items and the SQL client.

### Konfidence.BaseRest.Client
- Client for basic Restservice access. Using the RestSharp client. Used by my ClassGenerator.
- `BaseRestClient`: thin async wrapper (`GetAsync<T>`, `PostAsync<T>`) around RestSharp that JSON-serializes the request body, adds optional headers, and deserializes the response into `T`.
- `IRestClientConfig`/`RestClientConfig`: supplies the base URI the client is configured against.
  
### Konfidence.BaseThreadClasses
- basic manage-run-action pattern for simple threading. Consider using simpler Tasks before using this library.
- `ThreadManager<TAction>`: starts/stops a background `ThreadRunner`, exposing hooks (`SetInitializeAction`, `SetBeforeExecuteAction`, `SetAfterExecuteAction`) that run around each execution of a `ThreadAction`.
- `ThreadRunner<TAction>`/`ThreadAction`: the underlying loop, sleeping between runs for a configurable interval (`SleepUnit`: seconds/minutes/...).

### Konfidence.DataBaseInterface
- Interfacing between Konfidence.BaseDataBaseClasses && Konfidence.SqlHostProvider. And ofcourse dependency injection infra.  
- `IBaseClient`: the CRUD contract (get/save/delete/get-list, table/view/stored-procedure existence checks) a data item talks to.
- `IDataRepository`: the lower-level contract that actually executes stored procedures / text commands against ADO.NET (`IDataReader`/`DataTable`), implemented by `Konfidence.SqlHostProvider`.
- `IBaseDataItem`/`ISpParameterData`: the shape of a data item and of a single stored-procedure parameter, so the two libraries can reference each other without a hard dependency.

### Konfidence.Mail
- Base smtp client implementation: new BaseMailSender(..) -> SendEmail(..) 
- `BaseMailSender`: wraps `System.Net.Mail.SmtpClient` with basic-auth credentials, optional HTML body and a single file attachment; swallows send failures into a `bool` result rather than throwing.
- `MailAccounts`/`MailConstants`: supporting constants/config for known mail accounts.

### Konfidence.Security
- Creation and retrieval of public and private RSA keys.
- Save in (secured) local storage.
- Delete from (secured) local storage.
- Encoding with a shared public key.
- Decoding with your secret private key. 
- `PrivatePublicKey`: creates/loads an RSA key pair for a named application (via `KeyEncryption`) and can delete its store.
- `Encryption/Encoder` and `Encryption/Decoder`: split a string into key-size-limited blocks and RSA-encrypt/decrypt each block (`Encoder.Encrypt(..)` / `Decoder.Decrypt(..)`).
- `Encryption/KeyEncryption`: does the actual RSA key generation and reading, and the secured local storage read/write/delete.
- `ISecurityConfiguration`/`SecurityConfiguration`: configuration for where/how keys are stored.

### Konfidence.SqlDataAccess
A small, focused replacement for the SQL access parts of `EnterpriseLibrary.Data.NetCore` (the old Enterprise Library Data Access Application Block), built directly on `Microsoft.Data.SqlClient`. Exists so `Konfidence.SqlHostProvider` no longer depends on that ~15-year-old, largely frozen package (which itself dragged in the deprecated `System.Data.SqlClient`).
- `SqlDatabase` + `SqlDatabaseFactory`: a stateless, connection-string-driven executor (`CreateConnection`, `GetStoredProcCommand`, `AddInParameter`/`AddParameter`, `ExecuteNonQuery`, `ExecuteReader`, `GetParameterValue`) mirroring the narrow slice of EL's `Database` API that was actually used, including its connection-lifecycle behavior (`ExecuteReader` uses `CommandBehavior.CloseConnection` so callers can rely on disposing the reader to also close the connection).
- `DatabaseSettings`: a drop-in `ConfigurationSection` replacement for EL's `DatabaseSettings`, read from `app.config`'s `<dataConfiguration defaultDatabase="..." />` section.
- Has no dependency on `Konfidence.SqlHostProvider` or any other Konfidence package — the dependency only flows one way.

### Konfidence.SqlHostProvider
Provides MS Sql database access, now via `Konfidence.SqlDataAccess`/`Microsoft.Data.SqlClient` instead of the old enterprise libraries. Configured with app.config and SqlClientSettings.json. Allows manipulation of app.config settings directly or in memory. Can return a DatabaseStructure, which describes a database. Its tables, columns, types and some constraints. Used by my ClassGenerator and its generated artifacts.
- `DependencyInjectionFactory`: builds an `IServiceProvider` wired up with `SqlClient`, `SqlClientRepository`, `DatabaseStructure` and `IClientConfig`, reading configuration from `SqlClientSettings.json` plus command-line overrides (config folder / default database).
- `SqlAccess/SqlClient` + `SqlClientRepository`: implement `IBaseClient`/`IDataRepository` against MS SQL, executing the get/save/delete/list stored procedures and schema-existence checks generated for a data item.
- `SqlAccess/ClientConfig` + `ClientConfigExtensions`/`ClientSettings`/`ConfigConnectionString`: connection configuration bound from `SqlClientSettings.json`, including reading/writing settings straight from/to `app.config`.
- `SqlConnectionManagement/ConnectionManagement`: connection-string assembly and constants.
- `SqlDbSchema/DatabaseStructure` (+ `TableDataItem`, `ColumnDataItem`, `PrimaryKeyDataItem`, `IndexDataItem`, `SpName`): reads a database's structure (tables, columns, types, primary keys, indexes) by temporarily installing and then removing helper stored procedures — the basis for the ClassGenerator's code generation.

### Konfidence.TestTools
Prepares the configuration of a unittest with live access to SqlServer. Since dotnet, the location of the TestHost and where the tests are running are different. The app.config not being in the expected location is an issue.

### Konfidence.Integration.TestClasses
Shared test fixtures/generated SQL used by the `*.IntegrationTest` projects that exercise `Konfidence.SqlHostProvider`/`Konfidence.BaseDataBaseClasses` against a real SQL Server instance.

### Konfidence.UtilHelper
Some obsolete classes. Technical debt :(
- `ApplicationSettings`/`IApplicationSettings`/`ApplicationSettingsFactory`: legacy `.settings` XML file get/set-string-value store.
- `BaseApplicationConfiguration`: legacy XML `app.config`-style read/write of string/bool/byte-array node values.
- `BaseXmlDocument`: thin XML document base helper.
- `dllLoading`: P/Invoke wrapper (`LoadLibraryEx`/`GetProcAddress`/`FreeLibrary`) for loading native Win32 DLLs and calling exported functions via delegates.

### Tools/ClientSettingsUpdater
For me: updates the SqlClientSettings.json in a buildpipeline, keeping secrets out of the git repository. Packed as installable dotnet tool.
- `ClientSettingsUpdater.UnitTest`: unit tests for the `ClientSettingsManager` update logic.

### Test
Per-library unit test projects (`Konfidence.*.UnitTest`) plus `Test/TestByHandApp`, a small manual/exploratory console app for ad-hoc verification against a live SQL Server.
- `Konfidence.TestClasses.IntegrationTest`: integration tests that run ClassGenerator-generated `Dl.*DataItem` classes (from `Konfidence.Integration.TestClasses`) against a live SQL Server, using `Konfidence.TestTools` to wire up the test configuration/security settings.
- `Konfidence.SqlDataAccess.UnitTest`: fast unit tests for `SqlDatabase`/`SqlDatabaseFactory`/`DatabaseSettings` — no live SQL Server needed.
- `Konfidence.SqlHostProvider.UnitTest` / `Konfidence.SqlHostProvider.IntegrationTest`: the former holds fast mocked tests, the latter holds only the live-SQL-Server `DataBaseStructureTests.cs` (`TestCategory("DatabaseStructure")`).
