# PackageCleaner

A dotnet tool that prunes a folder of NuGet packages down to the most recent year of each package. Written for my own `\\konfidence8\publish` share, which every build has been adding to since 2020 and nothing has ever been taking out of.

Part of the [Konfidence.BaseClasses](https://github.com/a3helmich/Konfidence.BaseClasses) collection of libraries.

## Installing

```
dotnet tool install -g PackageCleaner
```

## Using it

```
packagecleaner --PackageFolder=\\konfidence8\publish
packagecleaner --PackageFolder=\\konfidence8\publish --Apply=true
```

- `--PackageFolder` — folder holding the `.nupkg`/`.snupkg` files (required)
- `--KeepFile` — a JSON file naming older package years to spare, see below
- `--Apply=true` — actually remove the files. Without it the tool only reports what it would remove
- `--Verbose=true` — also list every file removed, and every package left untouched

Exit codes: `1` no `--PackageFolder`, `2` that folder does not exist, `3` the keep file is missing or
unreadable.

Run by hand. This is deliberately not part of any build pipeline: nothing schedules it and no
build step calls it. Removing packages from a shared folder is a decision taken per occasion,
after looking at what still points at the versions concerned.

## What it removes

The folder is read as a flat set of `<PackageId>.<year>.<major>.<minor>.nupkg`/`.snupkg` files — the
`year.major.minor` scheme the Konfidence packages use, where the year is the semver major. Anything
whose name does not fit that shape is ignored, not removed.

**Per package id, the most recent year that package has is kept in full, and every earlier year is
removed.** Each package is judged on its own years, never against the calendar or against the other
packages in the folder.

```
Konfidence.Mail.2021.1.49.nupkg         removed - 2026 is this package's latest year
Konfidence.Mail.2022.1.0.nupkg          removed
Konfidence.Mail.2024.1.5.nupkg          removed
Konfidence.Mail.2026.4.1.nupkg          kept    - every version of the latest year stays
Konfidence.Mail.2026.5.6.nupkg          kept
Konfidence.TestClasses.2021.1.49.nupkg  removed - this package's latest year is 2022
Konfidence.TestClasses.2022.2.0.nupkg   kept
WebLoggingClasses.2021.1.3.nupkg        kept    - only one year, nothing to remove
```

Two properties follow from judging each package on its own years, and both are deliberate:

- **No package is ever emptied.** Its latest year always survives, so a package that was retired
  years ago keeps the last versions it ever had. A rule measured against the calendar, or against the
  newest package in the folder, would wipe such a package entirely the first time anything else got a
  build.
- **Every version of the kept year stays.** Only whole years are removed, never individual versions
  inside the surviving year.

## Keeping older years that something still needs

Such a folder is usually also a NuGet restore source, and a pinned exact version that disappears from
it is a broken restore in whichever repository was pinned to it. This rule removes *every* year but
the last, so it reaches much further back than the packages still in active use.

`--KeepFile` names the exceptions, per package and per year:

```json
{
  "Keep": {
    "Konfidence.SqlHostProvider": [ 2021, 2022 ],
    "Konfidence.TestTools": [ 2024 ]
  }
}
```

Every version in a listed year survives. Package ids are matched ignoring case, and a package with no
entry is pruned normally. `PackageCleanerKeepList.json` next to the tool's sources is the list for
`\\konfidence8\publish`, derived from the exact versions the repositories there still pin:

```
packagecleaner --PackageFolder=\\konfidence8\publish --KeepFile=PackageCleanerKeepList.json
```

The tool reports what the list spared, and names any entry that matched no file on the share so it can
be pruned once the repository needing it has moved on.

**A keep file that is missing or unparseable stops the run with exit code `3`.** It exists to protect
versions something is still pinned to, so a mistyped path must not quietly become a full prune.

### Keeping the list honest

The entries are only as good as the inventory behind them. On this share they came from the exact
versions pinned across every consuming repository, which is also why they are mostly 2021/2022: those
are the repositories that cannot be moved forward, being netstandard/net48 libraries with net48
consumers — see `Shared/FloatingPackageVersions.md` in the Azure.Devops.Documentation repository.

Of the Konfidence packages only `Konfidence.BaseClasses` is also on nuget.org, so only its old
versions would still resolve after being removed here; every other package is share-only. Its entries
in the list are therefore belt-and-braces and could be dropped.

Targets **net9.0** and **net10.0**.

## Full documentation

The other libraries in the collection, and build/test instructions, are in the
[README on github.com](https://github.com/a3helmich/Konfidence.BaseClasses#readme).
