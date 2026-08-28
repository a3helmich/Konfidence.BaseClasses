# Konfidence.DatabaseInterface

The contracts sitting between `Konfidence.BaseDatabaseClasses` and `Konfidence.SqlHostProvider`, so the two can work together without a hard dependency on each other. And of course the dependency injection infra.

Part of the [Konfidence.BaseClasses](https://github.com/a3helmich/Konfidence.BaseClasses) collection of libraries.

## What is in it

- **`IBaseClient`** — the CRUD contract a data item talks to: get/save/delete/get-list, plus table/view/stored-procedure existence checks
- **`IDataRepository`** — the lower-level contract that actually executes stored procedures and text commands against ADO.NET (`IDataReader`/`DataTable`), implemented by `Konfidence.SqlHostProvider`
- **`IBaseDataItem` / `ISpParameterData`** — the shape of a data item and of a single stored-procedure parameter

Targets **net9.0** and **net10.0**. Interfaces only, no implementation.

## Full documentation

The other libraries in the collection, and build/test instructions, are in the
[README on github.com](https://github.com/a3helmich/Konfidence.BaseClasses#readme).
