using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using JetBrains.Annotations;

namespace Konfidence.Base;

public static class FilePathExtensions
{
    [UsedImplicitly]
    public static bool TryFindFile(
        this string fileName, 
        [NotNullWhen(true)] out string? fullFileName, 
        int maxDepth = 5)
    {
        fullFileName = null;

        string baseOffset = $"..{Path.DirectorySeparatorChar}";
        string offSetInc = baseOffset;

        int currentDepth = 0;

        while (currentDepth <= maxDepth)
        {
            string combinedFileName = Path.GetFullPath(Path.Combine(baseOffset, fileName));

            if (File.Exists(combinedFileName))
            {
                fullFileName = combinedFileName;

                return true;
            }

            baseOffset = Path.Combine(baseOffset, offSetInc);

            currentDepth = baseOffset.Split([Path.DirectorySeparatorChar], StringSplitOptions.RemoveEmptyEntries).Length;
        }

        return false;
    }

    [UsedImplicitly]
    public static bool TryFindFileIncludingSubFolders(
        this string fileName,
        out List<string> fullFileNames,
        int maxDepth = 5)
    {
        fullFileNames = [];

        string baseOffset = $"..{Path.DirectorySeparatorChar}";
        string offSetInc = baseOffset;

        int currentDepth = 0;

        while (currentDepth <= maxDepth)
        {
            string fullPath = Path.GetFullPath(baseOffset);

            var allFiles = Directory.EnumerateFiles(fullPath, fileName, SearchOption.AllDirectories).ToArray();

            fullFileNames.AddRange(allFiles.Where(x => x.EndsWith(fileName)));

            if (fullFileNames.Any())
            {
                return true;
            }

            baseOffset = Path.Combine(baseOffset, offSetInc);

            currentDepth = baseOffset.Split([Path.DirectorySeparatorChar], StringSplitOptions.RemoveEmptyEntries).Length;
        }

        return false;
    }

    [UsedImplicitly]
    public static bool TryFindDirectory(
        this string directoryName,
        [NotNullWhen(true)] out string? fullDirectoryName,
        int maxDepth = 5)
    {
        fullDirectoryName = null;

        string baseOffset = $"..{Path.DirectorySeparatorChar}";
        string offSetInc = baseOffset;

        int currentDepth = 0;

        while (currentDepth <= maxDepth)
        {
            string combinedDirectoryName = Path.GetFullPath(Path.Combine(baseOffset, directoryName));

            if (Directory.Exists(combinedDirectoryName))
            {
                fullDirectoryName = combinedDirectoryName;

                return true;
            }

            baseOffset = Path.Combine(baseOffset, offSetInc);

            currentDepth = baseOffset.Split([Path.DirectorySeparatorChar], StringSplitOptions.RemoveEmptyEntries).Length;
        }

        return false;
    }

    [UsedImplicitly]
    public static bool ValidateDirectory(this string path)
    {
        if (!Directory.Exists(path))
        {
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (DirectoryNotFoundException)
            {
            }
        }

        return Directory.Exists(path);
    }
}