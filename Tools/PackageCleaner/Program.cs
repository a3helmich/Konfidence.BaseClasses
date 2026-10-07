using System.Diagnostics.CodeAnalysis;

namespace PackageCleaner;

[ExcludeFromCodeCoverage]
static class Program
{
    static void Main(string[] args)
    {
        PackageCleanupManager packageCleanupManager = new(args, new PackageFileStore(), new ErrorExiter());

        packageCleanupManager.Execute();
    }
}
