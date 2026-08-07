using System;
using System.IO;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.SqlHostProvider.LocalDb.UnitTest;

/// <summary>
/// Attaches the checked-in TestClassGenerator.mdf snapshot to the local SQL Server LocalDB
/// instance, under a database name unique to this process, so tests can exercise real database
/// code paths (DatabaseStructure, ColumnDataItem, SqlClientRepository) without needing a live,
/// network-reachable SQL Server - the way the existing IntegrationTest projects require. This
/// makes these tests runnable on GitHub Actions (which ships SQL Server LocalDB on windows-latest,
/// but has no route to the internal dev server).
///
/// The database name is per-process (not a fixed "TestClassGenerator") because
/// DatabaseStructure.BuildStructure() itself creates and deletes helper stored procedures as part
/// of normal schema introspection - two processes sharing one attached database race at the SQL
/// level regardless of how careful the attach/detach logic is. net9.0 and net10.0 test hosts can
/// run as genuinely concurrent processes under some test runners (this repo's
/// TestTfmsInParallel=false only affects the `dotnet test` CLI's own cross-targeting dispatch, not
/// e.g. ReSharper's/Visual Studio's independent test-runner process model), so per-process
/// isolation is the only fully race-free option.
///
/// If the local SQL Server engine can't open the checked-in snapshot at all (e.g. it was created
/// by a newer SQL Server release than the engine running these tests supports - database files
/// are one-way forward-compatible only), setup failure is recorded here instead of thrown, and
/// LocalDbTestBase.SkipIfSetupFailed() reports every test as Inconclusive with a clear reason
/// instead of failing the whole assembly.
/// </summary>
[TestClass]
public sealed class LocalDbTestDatabase
{
    private const string LocalDbInstance = @"(localdb)\MSSQLLocalDB";

    private static readonly string DatabaseName = $"TestClassGenerator_{Environment.ProcessId}_{Guid.NewGuid():N}";
    private static readonly string MasterConnectionString = $"Server={LocalDbInstance};Database=master;Integrated Security=true;TrustServerCertificate=true";

    private static string? _workingDirectory;

    internal static Exception? SetupFailure { get; private set; }

    [AssemblyInitialize]
    public static void AssemblyInitialize(TestContext _)
    {
        try
        {
            _workingDirectory = Path.Combine(Path.GetTempPath(), "Konfidence.SqlHostProvider.LocalDb.UnitTest", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_workingDirectory);

            string sourceMdf = Path.Combine(AppContext.BaseDirectory, "TestData", "TestClassGenerator.mdf");
            string sourceLdf = Path.Combine(AppContext.BaseDirectory, "TestData", "TestClassGenerator_log.ldf");

            string mdfPath = Path.Combine(_workingDirectory, "TestClassGenerator.mdf");
            string ldfPath = Path.Combine(_workingDirectory, "TestClassGenerator_log.ldf");

            File.Copy(sourceMdf, mdfPath);
            File.Copy(sourceLdf, ldfPath);

            using (SqlConnection master = new(MasterConnectionString))
            {
                master.Open();

                using SqlCommand attachCommand = new(
                    $"""
                     CREATE DATABASE [{DatabaseName}] ON PRIMARY (FILENAME = N'{mdfPath}')
                     LOG ON (FILENAME = N'{ldfPath}')
                     FOR ATTACH
                     """,
                    master);

                attachCommand.ExecuteNonQuery();
            }

            RewriteConnectionSettings();
        }
        catch (Exception exception)
        {
            SetupFailure = exception;
        }
    }

    [AssemblyCleanup]
    public static void AssemblyCleanup()
    {
        if (SetupFailure is null)
        {
            using SqlConnection master = new(MasterConnectionString);
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

/// <summary>
/// Base class for test classes that depend on LocalDbTestDatabase's attached snapshot. Reports
/// every test in a derived class as Inconclusive (not Failed) when the local SQL Server engine
/// couldn't open the checked-in snapshot, instead of letting the whole assembly hard-fail.
/// </summary>
public abstract class LocalDbTestBase
{
    [TestInitialize]
    public void SkipIfSetupFailed()
    {
        if (LocalDbTestDatabase.SetupFailure is { } failure)
        {
            Assert.Inconclusive($"Skipped: could not attach the LocalDB test database snapshot in this environment - {failure.Message}");
        }
    }
}
