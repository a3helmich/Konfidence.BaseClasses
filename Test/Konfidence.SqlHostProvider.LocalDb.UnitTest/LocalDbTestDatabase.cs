using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.SqlHostProvider.LocalDb.UnitTest;

/// <summary>
/// Attaches the checked-in TestClassGenerator.mdf snapshot to a SQL Server LocalDB instance, under
/// a database name unique to this process, so tests can exercise real database code paths
/// (DatabaseStructure, ColumnDataItem, SqlClientRepository) without needing a live,
/// network-reachable SQL Server - the way the existing IntegrationTest projects require. This
/// makes these tests runnable on GitHub Actions (which ships SQL Server LocalDB on windows-latest,
/// but has no route to the internal dev server) as well as this repo's self-hosted Azure DevOps
/// agent.
///
/// The database name is per-process (not a fixed "TestClassGenerator") because
/// DatabaseStructure.BuildStructure() itself creates and deletes helper stored procedures as part
/// of normal schema introspection - two processes sharing one attached database race at the SQL
/// level regardless of how careful the attach/detach logic is. net9.0 and net10.0 test hosts can
/// run as genuinely concurrent processes under some test runners, so per-process isolation is the
/// only fully race-free option.
///
/// Database files are one-way forward-compatible only (an older SQL Server engine can never open a
/// file created by a newer one), and the default "(localdb)\MSSQLLocalDB" alias resolves to
/// whichever version was current when that instance was first created on a given machine - not
/// necessarily one that can open the checked-in snapshot. Rather than pinning to one hardcoded
/// engine version, AssemblyInitialize enumerates every LocalDB version actually installed
/// (`sqllocaldb versions`) and tries each, oldest first, under a dedicated instance name, until one
/// successfully attaches the snapshot.
/// </summary>
[TestClass]
public sealed class LocalDbTestDatabase
{
    private static readonly string DatabaseName = $"TestClassGenerator_{Environment.ProcessId}_{Guid.NewGuid():N}";

    private static string? _workingDirectory;
    private static string? _instanceName;
    private static string _masterConnectionString = string.Empty;

    [AssemblyInitialize]
    public static void AssemblyInitialize(TestContext _)
    {
        _workingDirectory = Path.Combine(Path.GetTempPath(), "Konfidence.SqlHostProvider.LocalDb.UnitTest", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_workingDirectory);

        string sourceMdf = Path.Combine(AppContext.BaseDirectory, "TestData", "TestClassGenerator.mdf");
        string sourceLdf = Path.Combine(AppContext.BaseDirectory, "TestData", "TestClassGenerator_log.ldf");
        string mdfPath = Path.Combine(_workingDirectory, "TestClassGenerator.mdf");
        string ldfPath = Path.Combine(_workingDirectory, "TestClassGenerator_log.ldf");

        File.Copy(sourceMdf, mdfPath);
        File.Copy(sourceLdf, ldfPath);

        List<string> installedVersions = GetInstalledVersionsAscending();

        if (installedVersions.Count == 0)
        {
            throw new InvalidOperationException("No SQL Server LocalDB versions are installed (`sqllocaldb versions` returned nothing).");
        }

        List<Exception> attemptFailures = new();

        foreach (string version in installedVersions)
        {
            string instanceName = $"Konfidence_LocalDb_{version.Replace('.', '_')}";

            try
            {
                string pipeName = EnsureInstanceRunning(instanceName, version);
                string masterConnectionString = $"Server={pipeName};Database=master;Integrated Security=true;TrustServerCertificate=true;Pooling=false;Connect Timeout=3";

                using (SqlConnection master = OpenWithRetry(masterConnectionString))
                {
                    using SqlCommand attachCommand = new(
                        $"""
                         CREATE DATABASE [{DatabaseName}] ON PRIMARY (FILENAME = N'{mdfPath}')
                         LOG ON (FILENAME = N'{ldfPath}')
                         FOR ATTACH
                         """,
                        master);

                    attachCommand.ExecuteNonQuery();
                }

                _instanceName = instanceName;
                _masterConnectionString = masterConnectionString;

                RewriteConnectionSettings(pipeName);

                return;
            }
            catch (Exception exception)
            {
                attemptFailures.Add(new InvalidOperationException($"LocalDB version {version} (instance '{instanceName}'): {exception.Message}", exception));
            }
        }

        throw new AggregateException(
            $"Could not attach the LocalDB test database snapshot under any installed LocalDB version ({string.Join(", ", installedVersions)}).",
            attemptFailures);
    }

    [AssemblyCleanup]
    public static void AssemblyCleanup()
    {
        if (_instanceName is not null)
        {
            using SqlConnection master = new(_masterConnectionString);
            master.Open();

            using SqlCommand detachCommand = new(
                $"ALTER DATABASE [{DatabaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; EXEC sp_detach_db N'{DatabaseName}'",
                master);

            detachCommand.ExecuteNonQuery();
        }

        if (_workingDirectory is not null && Directory.Exists(_workingDirectory))
        {
            Directory.Delete(_workingDirectory, true);
        }
    }

    private static List<string> GetInstalledVersionsAscending()
    {
        string output = RunSqlLocalDb("versions");

        List<Version> versions = Regex.Matches(output, @"\((\d+)\.(\d+)\.\d+\.\d+\)")
            .Select(match => new Version(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)))
            .Distinct()
            .OrderBy(version => version)
            .ToList();

        return versions.Select(version => $"{version.Major}.{version.Minor}").ToList();
    }

    // Returns the instance's literal named-pipe address (e.g. np:\\.\pipe\LOCALDB#28181DA3\tsql\query).
    // Connecting via that raw pipe, rather than the "(localdb)\<name>" alias, sidesteps a hand-off gap
    // where a custom named instance (Auto-create: No) that sqllocaldb.exe just created/started is
    // visible to the CLI immediately but not yet resolvable through ADO.NET's own LocalDB alias
    // lookup - observed reliably under the MSTest test host, not under a plain console host.
    private static string EnsureInstanceRunning(string instanceName, string version)
    {
        if (!TryGetPipeName(instanceName, out string? pipeName))
        {
            RunSqlLocalDb($"create \"{instanceName}\" {version} -s");
        }
        else
        {
            RunSqlLocalDb($"start \"{instanceName}\"");
        }

        for (int attempt = 0; attempt < 20; attempt++)
        {
            if (TryGetPipeName(instanceName, out pipeName) && pipeName is not null)
            {
                return pipeName;
            }

            Thread.Sleep(250);
        }

        throw new InvalidOperationException($"LocalDB instance '{instanceName}' did not report a pipe name after creation.");
    }

    // Even a pipe name reported by sqllocaldb info doesn't guarantee the very first connection
    // attempt succeeds (observed under the MSTest test host, not under a plain console host) -
    // retry the connection itself rather than trusting a single Open() call.
    private static SqlConnection OpenWithRetry(string connectionString)
    {
        Exception? lastFailure = null;

        for (int attempt = 0; attempt < 10; attempt++)
        {
            SqlConnection connection = new(connectionString);

            try
            {
                connection.Open();

                return connection;
            }
            catch (Exception exception)
            {
                lastFailure = exception;
                connection.Dispose();
                Thread.Sleep(500);
            }
        }

        throw new InvalidOperationException($"Could not open a connection using '{connectionString}' after repeated attempts.", lastFailure);
    }

    private static bool TryGetPipeName(string instanceName, out string? pipeName)
    {
        pipeName = null;

        string output;

        try
        {
            output = RunSqlLocalDb($"info \"{instanceName}\"");
        }
        catch (InvalidOperationException)
        {
            return false;
        }

        if (!output.Contains("State:", StringComparison.Ordinal) || !output.Contains("Running", StringComparison.Ordinal))
        {
            return false;
        }

        Match match = Regex.Match(output, @"Instance pipe name:\s*(\S+)");

        if (!match.Success)
        {
            return false;
        }

        pipeName = match.Groups[1].Value;

        return true;
    }

    private static string RunSqlLocalDb(string arguments)
    {
        using Process process = new();

        process.StartInfo.FileName = "sqllocaldb";
        process.StartInfo.Arguments = arguments;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;

        process.Start();

        string standardOutput = process.StandardOutput.ReadToEnd();
        string standardError = process.StandardError.ReadToEnd();

        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"sqllocaldb {arguments} failed (exit {process.ExitCode}): {standardOutput}{standardError}");
        }

        return standardOutput;
    }

    // DependencyInjectionFactory reads SqlClientSettings.json fresh on every
    // ConfigureDependencyInjection() call, so overwriting it here (before any test runs) is enough
    // to point every subsequent test at this process's own isolated database and instance. Uses the
    // literal pipe address (see EnsureInstanceRunning) rather than the "(localdb)\<name>" alias.
    private static void RewriteConnectionSettings(string pipeName)
    {
        string settingsPath = Path.Combine(AppContext.BaseDirectory, "SqlClientSettings.json");

        string escapedPipeName = pipeName.Replace(@"\", @"\\");

        string json =
            $$"""
              {
                "DataConfiguration": {
                  "DefaultDatabase": "TestClassGenerator",
                  "UseEnvironmentSetting": false,
                  "Connections": [
                    {
                      "ConnectionName": "TestClassGenerator",
                      "Server": "{{escapedPipeName}}",
                      "Database": "{{DatabaseName}}",
                      "UserName": "",
                      "Password": ""
                    }
                  ]
                }
              }
              """;

        File.WriteAllText(settingsPath, json);
    }
}
