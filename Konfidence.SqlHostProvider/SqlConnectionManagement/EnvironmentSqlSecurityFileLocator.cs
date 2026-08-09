using Konfidence.Base;

namespace Konfidence.SqlHostProvider.SqlConnectionManagement;

/// <summary>
/// The production locator: takes the security file path from the "ClientConfigLocation" environment
/// variable.
/// </summary>
internal sealed class EnvironmentSqlSecurityFileLocator : ISqlSecurityFileLocator
{
    internal const string EnvironmentVariableName = "ClientConfigLocation";

    public bool TryGetSecurityFilePath(out string filePath)
    {
        return EnvironmentVariableName.TryGetEnvironmentVariable(out filePath);
    }
}
