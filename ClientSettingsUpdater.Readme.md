# ClientSettingsUpdater

A dotnet tool that updates the `SqlClientSettings.json` of a build, keeping SQL secrets out of the git repository. Written for my own build pipeline: the credentials come from the pipeline's secret store and are written into the settings file just before the tests run.

Part of the [Konfidence.BaseClasses](https://github.com/a3helmich/Konfidence.BaseClasses) collection of libraries.

## Installing

```
dotnet tool install -g ClientSettingsUpdater
```

## Using it

```
clientsettingsupdater --ConfigFileFolder=. --UserName=<user> --Password=<password> --Server=<server>
```

- `--ConfigFileFolder` — folder holding the settings file (required)
- `--UserName`, `--Password` — the credentials to write (required)
- `--Server` — only update connections for this server; omit to update all of them
- `--ConfigFileName` — defaults to `SqlClientSettings.json`, or `MailClientSettings.json` when `--MailServer` is given
- `--MailServer` — update mail account credentials instead of SQL connections

Connections that already carry a user name are left untouched.

Targets **net9.0** and **net10.0**.

## Full documentation

The libraries in the collection, and build/test instructions, are in the
[README on github.com](https://github.com/a3helmich/Konfidence.BaseClasses#readme).
