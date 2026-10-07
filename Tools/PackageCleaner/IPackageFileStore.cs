using System.Collections.Generic;

namespace PackageCleaner;

internal interface IPackageFileStore
{
    bool FolderExists(string folder);

    bool FileExists(string filePath);

    string ReadAllText(string filePath);

    IReadOnlyList<string> GetPackageFileNames(string folder);

    void Delete(string folder, string fileName);
}
