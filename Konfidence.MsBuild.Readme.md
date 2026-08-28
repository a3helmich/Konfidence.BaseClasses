# Konfidence.MsBuild

Programmatic reading and editing of Visual Studio solution files and the projects they contain — adding a generated project to a solution without opening the IDE.

Part of the [Konfidence.BaseClasses](https://github.com/a3helmich/Konfidence.BaseClasses) collection of libraries.

## What is in it

- **`SolutionDocument`** — loads a solution file (`GetSolutionDocument(..)`), reports the projects it contains, and edits it: `AddProjectFile(..)`, `AddSolutionItem(..)` and `Save()`
- **`SolutionProject` / `SolutionProjectList`** — a project inside a solution: its file, name and guid
- Project files are read for the details a solution entry needs — project name, project guid and the path relative to the solution

Targets **net9.0** and **net10.0**.

## Full documentation

The other libraries in the collection, and build/test instructions, are in the
[README on github.com](https://github.com/a3helmich/Konfidence.BaseClasses#readme).
