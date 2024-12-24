using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
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
}