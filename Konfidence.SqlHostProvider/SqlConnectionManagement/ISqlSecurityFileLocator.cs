namespace Konfidence.SqlHostProvider.SqlConnectionManagement;

/// <summary>
/// Locates the file holding the SQL credentials that get copied onto the client configuration.
/// The default implementation reads the "ClientConfigLocation" environment variable, which
/// TryGetEnvironmentVariable resolves User scope first - meaning a process cannot override or clear
/// it for itself. Going through this interface keeps that ambient lookup out of the copy logic.
/// </summary>
internal interface ISqlSecurityFileLocator
{
    bool TryGetSecurityFilePath(out string filePath);
}
