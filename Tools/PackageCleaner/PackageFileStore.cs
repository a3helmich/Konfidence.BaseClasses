using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace PackageCleaner;

[ExcludeFromCodeCoverage]
internal sealed class PackageFileStore : IPackageFileStore
{
    public bool FolderExists(string folder)
    {
        return Directory.Exists(folder);
    }

    public bool FileExists(string filePath)
    {
        return File.Exists(filePath);
    }

    public string ReadAllText(string filePath)
    {
        return File.ReadAllText(filePath);
    }

    public IReadOnlyList<string> GetPackageFileNames(string folder)
    {
        return Directory.GetFiles(folder, "*", SearchOption.TopDirectoryOnly)
            .Select(Path.GetFileName)
            .OfType<string>()
            .ToList();
    }

    public void Delete(string folder, string fileName)
    {
        File.Delete(Path.Combine(folder, fileName));
    }
}
