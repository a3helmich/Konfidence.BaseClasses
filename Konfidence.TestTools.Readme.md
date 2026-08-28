# Konfidence.TestTools

Prepares the configuration of a unit test that needs live access to SQL Server.

Since dotnet, the location of the TestHost and the location where the tests actually run are different, and app.config not being where it is expected is an issue. This puts the settings where the test process will find them.

Part of the [Konfidence.BaseClasses](https://github.com/a3helmich/Konfidence.BaseClasses) collection of libraries.

## What is in it

- **`SqlTestToolExtensions`** — copies the test project's `dataConfiguration` section and connection strings into the active (TestHost) configuration, and copies the SQL credentials for a named connection into it
- **`FileCompareTool`** — file comparison helper for tests that verify generated output

Targets **net9.0** and **net10.0**. Intended for test projects only.

## Full documentation

The other libraries in the collection, and build/test instructions, are in the
[README on github.com](https://github.com/a3helmich/Konfidence.BaseClasses#readme).
