using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Konfidence.Base;

namespace PackageCleaner;

internal sealed class KeepList
{
    private readonly Dictionary<string, HashSet<int>> _yearsByPackageId;

    private KeepList(Dictionary<string, HashSet<int>> yearsByPackageId)
    {
        _yearsByPackageId = yearsByPackageId;
    }

    public static KeepList Empty => new(new Dictionary<string, HashSet<int>>(StringComparer.OrdinalIgnoreCase));

    public static bool TryParse(string json, [NotNullWhen(true)] out KeepList? keepList)
    {
        keepList = null;

        if (!json.Deserialize(out KeepListDto? keepListDto))
        {
            return false;
        }

        Dictionary<string, HashSet<int>> yearsByPackageId = new(StringComparer.OrdinalIgnoreCase);

        foreach (KeyValuePair<string, List<int>> entry in keepListDto.Keep)
        {
            yearsByPackageId[entry.Key] = [..entry.Value];
        }

        keepList = new KeepList(yearsByPackageId);

        return true;
    }

    public bool Keeps(PackageFile packageFile)
    {
        return _yearsByPackageId.TryGetValue(packageFile.PackageId, out HashSet<int>? years) && years.Contains(packageFile.Year);
    }

    public List<string> GetEntriesMatchingNothing(List<PackageFile> packageFiles)
    {
        HashSet<string> present = packageFiles
            .Select(x => FormatEntry(x.PackageId, x.Year))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        return _yearsByPackageId
            .SelectMany(entry => entry.Value.Select(year => FormatEntry(entry.Key, year)))
            .Where(entry => !present.Contains(entry))
            .OrderBy(entry => entry, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static string FormatEntry(string packageId, int year)
    {
        return $"{packageId} {year}";
    }
}
