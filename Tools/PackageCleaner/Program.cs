using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace PackageCleaner;

[ExcludeFromCodeCoverage]
static class Program
{
    private const string DefaultKeepFileName = "PackageCleanerKeepList.json";

    static void Main(string[] args)
    {
        string defaultKeepFile = Path.Combine(AppContext.BaseDirectory, DefaultKeepFileName);

        PackageCleanupManager packageCleanupManager =
            new(args, new PackageFileStore(), new ErrorExiter(), defaultKeepFile);

        packageCleanupManager.Execute();
    }
}
