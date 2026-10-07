using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PackageCleaner;

internal sealed class PackageFile
{
    private static readonly Regex PackageFileNamePattern =
        new(@"^(?<id>.+)\.(?<year>\d{4})\.\d+\.\d+\.(?:nupkg|snupkg)$",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    public string FileName { get; }

    public string PackageId { get; }

    public int Year { get; }

    private PackageFile(string fileName, string packageId, int year)
    {
        FileName = fileName;
        PackageId = packageId;
        Year = year;
    }

    public static bool TryParse(string fileName, [NotNullWhen(true)] out PackageFile? packageFile)
    {
        packageFile = null;

        Match match = PackageFileNamePattern.Match(fileName);

        if (!match.Success)
        {
            return false;
        }

        int year = int.Parse(match.Groups["year"].Value, CultureInfo.InvariantCulture);

        packageFile = new PackageFile(fileName, match.Groups["id"].Value, year);

        return true;
    }
}
