# Konfidence.SqlDataAccess

A small, focused replacement for the SQL access parts of `EnterpriseLibrary.Data.NetCore` (the old Enterprise Library Data Access Application Block), built directly on `Microsoft.Data.SqlClient`.

It exists so `Konfidence.SqlHostProvider` no longer depends on that ~15-year-old, largely frozen package, which itself dragged in the deprecated `System.Data.SqlClient`.

Part of the [Konfidence.BaseClasses](https://github.com/a3helmich/Konfidence.BaseClasses) collection of libraries.

## What is in it

- **`SqlDatabase` + `SqlDatabaseFactory`** — a stateless, connection-string-driven executor: `CreateConnection`, `GetStoredProcCommand`, `AddInParameter`/`AddParameter`, `ExecuteNonQuery`, `ExecuteReader`, `GetParameterValue`. Mirrors the narrow slice of Enterprise Library's `Database` API that was actually used, including its connection lifecycle — `ExecuteReader` uses `CommandBehavior.CloseConnection`, so disposing the reader also closes the connection
- **`DatabaseSettings`** — a drop-in `ConfigurationSection` replacement for Enterprise Library's, read from `app.config`'s `<dataConfiguration defaultDatabase="..." />` section

Targets **net9.0** and **net10.0**. Has no dependency on any other Konfidence package — the dependency only flows one way.

## Full documentation

The other libraries in the collection, and build/test instructions, are in the
[README on github.com](https://github.com/a3helmich/Konfidence.BaseClasses#readme).
