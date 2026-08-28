# Konfidence.BaseDatabaseClasses

Classes that make CRUD on SQL really easy, without the strong dependencies that come with EntityFramework. Only really useful together with my ClassGenerator, which generates the data items that build on it.

Part of the [Konfidence.BaseClasses](https://github.com/a3helmich/Konfidence.BaseClasses) collection of libraries.

## What is in it

- **`BaseDataItem`** — abstract base for generated data items. Tracks its own id/guid key, its stored-procedure names (get/save/delete/get-by-guid) and the parameter list to send to the database, delegating the actual reads and writes to an injected `IBaseClient`
- **`FieldExtensions` / `AutoUpdateFieldExtensions`** — typed `SetField(..)` overloads (int, guid, string, decimal, DateTime, TimeSpan, ...) that register stored-procedure parameters on a `BaseDataItem`, plus tracking of server-generated/auto-update fields
- **`DataReaderExtensions`** — typed `IDataReader.GetField(..)` helpers, used while mapping a row back onto a data item
- **`Sp/SpParameter(Extensions)`** — the stored-procedure parameter model (name, `DbType`, value) shared between data items and the SQL client

Targets **net9.0** and **net10.0**. Used together with `Konfidence.SqlHostProvider`, which does the actual database access.

## Full documentation

The other libraries in the collection, and build/test instructions, are in the
[README on github.com](https://github.com/a3helmich/Konfidence.BaseClasses#readme).
