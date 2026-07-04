using System;

namespace Konfidence.SqlDataAccess;

public static class SqlDatabaseFactory
{
    public static SqlDatabase Create(string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        return new SqlDatabase(connectionString);
    }
}
