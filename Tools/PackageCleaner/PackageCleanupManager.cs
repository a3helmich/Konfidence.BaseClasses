using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Konfidence.Base;

namespace PackageCleaner;

internal class PackageCleanupManager
{
    private const string Enabled = "true";

    private readonly IPackageFileStore _packageFileStore;
    private readonly IErrorExiter _errorExiter;
    private readonly bool _verbose;

    public readonly string PackageFolder = string.Empty;
    public readonly string KeepFile = string.Empty;
    public readonly bool Apply;

    private readonly bool _keepFileWasSpecified;
    private bool _keepListIsInUse;

    public PackageCleanupManager(string[] args, IPackageFileStore packageFileStore, IErrorExiter errorExiter,
        string defaultKeepFile)
    {
        _packageFileStore = packageFileStore;
        _errorExiter = errorExiter;

        if (args.TryParseArgument(Argument.Verbose, out string verbose))
        {
            _verbose = verbose.IsAssigned();
        }

        if (args.TryParseArgument(Argument.Apply, out string apply))
        {
            Apply = apply.Equals(Enabled, StringComparison.OrdinalIgnoreCase);
        }

        _keepFileWasSpecified = args.TryParseArgument(Argument.KeepFile, out KeepFile);

        if (!_keepFileWasSpecified)
        {
            KeepFile = defaultKeepFile;
        }

        if (!args.TryParseArgument(Argument.PackageFolder, out PackageFolder))
        {
            Console.WriteLine($"required argument missing: --{Argument.PackageFolder}");

            _errorExiter.Exit(1);
        }
    }

    public void Execute()
    {
        if (!PackageFolder.IsAssigned())
        {
            return;
        }

        if (!_packageFileStore.FolderExists(PackageFolder))
        {
            Console.WriteLine($"package folder not found: '{PackageFolder}'");

            _errorExiter.Exit(2);

            return;
        }

        if (!TryGetKeepList(out KeepList? keepList))
        {
            _errorExiter.Exit(3);

            return;
        }

        List<PackageFile> packageFiles = GetPackageFiles();

        Console.WriteLine($"folder: '{PackageFolder}'");
        Console.WriteLine($"packages: {packageFiles.Count}");

        ReportUntouchedPackageIds(packageFiles);

        List<PackageFile> filesFromOlderYears = GetFilesOlderThanEachPackagesLatestYear(packageFiles);

        List<PackageFile> keptByKeepList = filesFromOlderYears.Where(keepList.Keeps).ToList();

        List<PackageFile> removablePackageFiles = filesFromOlderYears
            .Where(packageFile => !keepList.Keeps(packageFile))
            .ToList();

        ReportKeepList(keepList, packageFiles, keptByKeepList);

        if (!removablePackageFiles.Any())
        {
            Console.WriteLine("nothing to remove: every package has versions from one year only, or the rest is kept");

            return;
        }

        ReportRemovablePackageFiles(packageFiles, removablePackageFiles);

        if (!Apply)
        {
            Console.WriteLine($"preview only, nothing removed. pass --{Argument.Apply}={Enabled} to remove these {removablePackageFiles.Count} files");

            return;
        }

        Remove(removablePackageFiles);
    }

    private bool TryGetKeepList([NotNullWhen(true)] out KeepList? keepList)
    {
        keepList = KeepList.Empty;

        if (!KeepFile.IsAssigned())
        {
            return true;
        }

        if (!_packageFileStore.FileExists(KeepFile))
        {
            return AcceptMissingKeepFile();
        }

        if (!KeepList.TryParse(_packageFileStore.ReadAllText(KeepFile), out keepList))
        {
            Console.WriteLine($"keep file could not be read: '{KeepFile}'");

            return false;
        }

        _keepListIsInUse = true;

        return true;
    }

    // A keep file named on the command line must stop the run when it is not there: it exists to
    // protect older years that something is still pinned to, and carrying on without it would delete
    // exactly those. The copy shipped alongside the tool is only the default, so its absence means no
    // exclusions rather than a mistake.
    private bool AcceptMissingKeepFile()
    {
        if (_keepFileWasSpecified)
        {
            Console.WriteLine($"keep file not found: '{KeepFile}'");

            return false;
        }

        Console.WriteLine($"no keep file alongside the tool, nothing is excluded: '{KeepFile}'");

        return true;
    }

    private List<PackageFile> GetPackageFiles()
    {
        return _packageFileStore.GetPackageFileNames(PackageFolder)
            .Select(fileName => PackageFile.TryParse(fileName, out PackageFile? packageFile) ? packageFile : null)
            .OfType<PackageFile>()
            .ToList();
    }

    private static List<PackageFile> GetFilesOlderThanEachPackagesLatestYear(List<PackageFile> packageFiles)
    {
        return GroupByPackageId(packageFiles)
            .SelectMany(GetFilesBeforeTheLatestYear)
            .OrderBy(x => x.FileName, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static IEnumerable<PackageFile> GetFilesBeforeTheLatestYear(IGrouping<string, PackageFile> packageFilesForOnePackageId)
    {
        int latestYear = packageFilesForOnePackageId.Max(x => x.Year);

        return packageFilesForOnePackageId.Where(x => x.Year < latestYear);
    }

    private static IEnumerable<IGrouping<string, PackageFile>> GroupByPackageId(List<PackageFile> packageFiles)
    {
        return packageFiles.GroupBy(x => x.PackageId, StringComparer.OrdinalIgnoreCase);
    }

    private void ReportKeepList(KeepList keepList, List<PackageFile> packageFiles, List<PackageFile> keptByKeepList)
    {
        if (!_keepListIsInUse)
        {
            return;
        }

        Console.WriteLine($"keep file: '{KeepFile}', keeping {keptByKeepList.Count} older files");

        GroupByPackageId(keptByKeepList)
            .OrderBy(group => group.Key, StringComparer.OrdinalIgnoreCase)
            .ToList()
            .ForEach(group =>
            {
                string keptYears = string.Join(", ", group.Select(x => x.Year).Distinct().OrderBy(x => x));

                Console.WriteLine($"keeping: {group.Key} {keptYears} ({group.Count()} files)");
            });

        keepList.GetEntriesMatchingNothing(packageFiles)
            .ForEach(entry => Console.WriteLine($"keep file entry matches nothing, prune it: {entry}"));
    }

    private void ReportUntouchedPackageIds(List<PackageFile> packageFiles)
    {
        if (!_verbose)
        {
            return;
        }

        GroupByPackageId(packageFiles)
            .Where(group => group.Select(x => x.Year).Distinct().Count() == 1)
            .Select(group => group.Key)
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
            .ToList()
            .ForEach(packageId => Console.WriteLine($"untouched: {packageId} has versions from one year only"));
    }

    private static void ReportRemovablePackageFiles(List<PackageFile> packageFiles, List<PackageFile> removablePackageFiles)
    {
        Dictionary<string, int> latestYears = GroupByPackageId(packageFiles)
            .ToDictionary(group => group.Key, GetLatestYear, StringComparer.OrdinalIgnoreCase);

        GroupByPackageId(removablePackageFiles)
            .OrderBy(group => group.Key, StringComparer.OrdinalIgnoreCase)
            .ToList()
            .ForEach(group =>
            {
                string removedYears = string.Join(", ", group.Select(x => x.Year).Distinct().OrderBy(x => x));

                Console.WriteLine($"{group.Key}: {group.Count()} files from {removedYears}, keeping {latestYears[group.Key]}");
            });
    }

    private static int GetLatestYear(IGrouping<string, PackageFile> packageFilesForOnePackageId)
    {
        return packageFilesForOnePackageId.Max(x => x.Year);
    }

    private void Remove(List<PackageFile> removablePackageFiles)
    {
        removablePackageFiles.ForEach(packageFile =>
        {
            if (_verbose)
            {
                Console.WriteLine($"removing: {packageFile.FileName}");
            }

            _packageFileStore.Delete(PackageFolder, packageFile.FileName);
        });

        Console.WriteLine($"removed {removablePackageFiles.Count} files");
    }
}
