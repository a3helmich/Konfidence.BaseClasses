using System;
using System.IO;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.SqlHostProvider.LocalDb.UnitTest;

/// <summary>
/// Attaches the checked-in TestClassGenerator.mdf snapshot to the default SQL Server LocalDB
/// instance, under a database name unique to this process, so tests can exercise real database
/// code paths (DatabaseStructure, ColumnDataItem, SqlClientRepository) without needing a live,
/// network-reachable SQL Server - the way the existing IntegrationTest projects require. This
/// makes these tests runnable on GitHub Actions (which ships SQL Server LocalDB on windows-latest,
/// but has no route to the internal dev server).
///
/// This project only runs on GitHub Actions - the self-hosted Azure DevOps agent runs its build
/// steps under a Windows service session, which SQL Server LocalDB does not support, so
/// azure-pipelines.yml filters this project out of its test run and relies on the existing
/// *.IntegrationTest projects for that coverage instead.
///
/// The database name is per-process (not a fixed "TestClassGenerator") because
/// DatabaseStructure.BuildStructure() itself creates and deletes helper stored procedures as part
/// of normal schema introspection - two processes sharing one attached database race at the SQL
/// level regardless of how careful the attach/detach logic is. net9.0 and net10.0 test hosts can
/// run as genuinely concurrent processes under some test runners, so per-process isolation is the
/// only fully race-free option.
/// </summary>
[TestClass]
public sealed class LocalDbTestDatabase
{
    private const string LocalDbInstance = @"(localdb)\MSSQLLocalDB";

    private static readonly string DatabaseName = $"TestClassGenerator_{Environment.ProcessId}_{Guid.NewGuid():N}";
    private static readonly string MasterConnectionString = $"Server={LocalDbInstance};Database=master;Integrated Security=true;TrustServerCertificate=true";

    private static string? _workingDirectory;

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

        using SqlConnection master = new(MasterConnectionString);
        master.Open();

        using SqlCommand attachCommand = new(
            $"""
             CREATE DATABASE [{DatabaseName}] ON PRIMARY (FILENAME = N'{mdfPath}')
             LOG ON (FILENAME = N'{ldfPath}')
             FOR ATTACH
             """,
            master);

        attachCommand.ExecuteNonQuery();

        RewriteConnectionSettings();
    }

    [AssemblyCleanup]
    public static void AssemblyCleanup()
    {
        using SqlConnection master = new(MasterConnectionString);
        master.Open();

        using SqlCommand detachCommand = new(
            $"ALTER DATABASE [{DatabaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; EXEC sp_detach_db N'{DatabaseName}'",
            master);

        detachCommand.ExecuteNonQuery();

        if (_workingDirectory is not null && Directory.Exists(_workingDirectory))
        {
            Directory.Delete(_workingDirectory, true);
        }
    }

    // DependencyInjectionFactory reads SqlClientSettings.json fresh on every
    // ConfigureDependencyInjection() call, so overwriting it here (before any test runs) is enough
    // to point every subsequent test at this process's own isolated database.
    private static void RewriteConnectionSettings()
    {
        string settingsPath = Path.Combine(AppContext.BaseDirectory, "SqlClientSettings.json");

        string json =
            $$"""
              {
                "DataConfiguration": {
                  "DefaultDatabase": "TestClassGenerator",
                  "UseEnvironmentSetting": false,
                  "Connections": [
                    {
                      "ConnectionName": "TestClassGenerator",
                      "Server": "(localdb)\\MSSQLLocalDB",
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
