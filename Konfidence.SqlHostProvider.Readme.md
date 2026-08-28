# Konfidence.SqlHostProvider

MS SQL database access, via `Konfidence.SqlDataAccess`/`Microsoft.Data.SqlClient` instead of the old enterprise libraries. Configured with `app.config` and `SqlClientSettings.json`, and able to manipulate app.config settings directly or in memory. Used by my ClassGenerator and its generated artifacts.

Part of the [Konfidence.BaseClasses](https://github.com/a3helmich/Konfidence.BaseClasses) collection of libraries.

## What is in it

- **`AddSqlHostProviderServices(..)`** — a `IServiceCollection` extension that registers everything this library provides into an existing host's container
- **`DependencyInjectionFactory`** — builds a standalone `IServiceProvider` wired up with `SqlClient`, `SqlClientRepository`, `DatabaseStructure` and `IClientConfig`, reading configuration from `SqlClientSettings.json` plus command-line overrides
- **`SqlAccess/SqlClient` + `SqlClientRepository`** — implement `IBaseClient`/`IDataRepository` against MS SQL: the get/save/delete/list stored procedures and schema-existence checks generated for a data item
- **`SqlAccess/ClientConfig`** (+ `ClientSettings`, `ConfigConnectionString`) — connection configuration bound from `SqlClientSettings.json`, including reading and writing settings straight from and to `app.config`
- **`SqlConnectionManagement/IConnectionManagement` + `ConnectionManager`** — points the named connections declared in the host configuration at a database and server, and selects which one is active. Injectable; the older static `ConnectionManagement` is still there for existing callers
- **`SqlDbSchema/DatabaseStructure`** — returns a description of a database: its tables, columns, types, primary keys and indexes. The basis for the ClassGenerator's code generation

`DatabaseStructure` works by temporarily installing helper stored procedures in the target database and removing them again. Those helpers get a name unique to each run and are dropped in a `finally`, so concurrent introspection of the same database is safe and a failed run leaves nothing behind.

Targets **net9.0** and **net10.0**.

## Full documentation

The other libraries in the collection, and build/test instructions, are in the
[README on github.com](https://github.com/a3helmich/Konfidence.BaseClasses#readme).
