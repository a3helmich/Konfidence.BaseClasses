using System;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace Konfidence.SqlDataAccess;

public sealed class SqlDatabase
{
    private readonly string _connectionString;

    internal SqlDatabase(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public DbCommand GetStoredProcCommand(string storedProcedureName)
    {
        return new SqlCommand(storedProcedureName)
        {
            CommandType = CommandType.StoredProcedure
        };
    }

    public void AddInParameter(DbCommand command, string name, DbType dbType, object? value)
    {
        SqlParameter parameter = new(BuildParameterName(name), value ?? DBNull.Value)
        {
            DbType = dbType,
            Direction = ParameterDirection.Input
        };

        ApplySizeIfNeeded(parameter, dbType, value);

        command.Parameters.Add(parameter);
    }

    public void AddParameter(DbCommand command, string name, DbType dbType, ParameterDirection direction,
        string sourceColumn, DataRowVersion sourceVersion, object? value)
    {
        SqlParameter parameter = new(BuildParameterName(name), value ?? DBNull.Value)
        {
            DbType = dbType,
            Direction = direction,
            SourceColumn = sourceColumn,
            SourceVersion = sourceVersion
        };

        ApplySizeIfNeeded(parameter, dbType, value);

        command.Parameters.Add(parameter);
    }

    /// <summary>
    /// Explicitly setting DbType on a SqlParameter disables ADO.NET's automatic Size inference from
    /// the assigned value, which then throws "the Size property has an invalid size of 0" for
    /// variable/fixed-length string and binary types unless Size is set explicitly here.
    /// </summary>
    private static void ApplySizeIfNeeded(SqlParameter parameter, DbType dbType, object? value)
    {
        switch (dbType)
        {
            case DbType.String:
            case DbType.AnsiString:
            case DbType.Binary:
                parameter.Size = value switch
                {
                    string stringValue => Math.Max(stringValue.Length, 1),
                    byte[] byteArrayValue => Math.Max(byteArrayValue.Length, 1),
                    _ => -1
                };
                break;
            case DbType.StringFixedLength:
            case DbType.AnsiStringFixedLength:
                parameter.Size = value is string fixedStringValue ? Math.Max(fixedStringValue.Length, 1) : 1;
                break;
        }
    }

    public int ExecuteNonQuery(DbCommand command)
    {
        using SqlConnection connection = new(_connectionString);

        command.Connection = connection;

        connection.Open();

        return command.ExecuteNonQuery();
    }

    public int ExecuteNonQuery(CommandType commandType, string commandText)
    {
        using SqlConnection connection = new(_connectionString);
        using SqlCommand command = new(commandText, connection)
        {
            CommandType = commandType
        };

        connection.Open();

        return command.ExecuteNonQuery();
    }

    public IDataReader ExecuteReader(DbCommand command)
    {
        SqlConnection connection = new(_connectionString);

        command.Connection = connection;

        connection.Open();

        return command.ExecuteReader(CommandBehavior.CloseConnection);
    }

    public object GetParameterValue(DbCommand command, string name)
    {
        return command.Parameters[BuildParameterName(name)].Value!;
    }

    internal static string BuildParameterName(string name)
    {
        return name.StartsWith('@') ? name : $"@{name}";
    }
}
