using System;
using System.Data;
using System.Data.Common;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.SqlDataAccess.Tests;

[TestClass]
public class SqlDatabaseTests
{
    private const string CONNECTION_STRING = "Data Source=konfidence2;Initial Catalog=TestClassGenerator;Integrated Security=True";

    private sealed record TestContext(SqlDatabase Database);

    private static TestContext CreateContext()
    {
        SqlDatabase database = SqlDatabaseFactory.Create(CONNECTION_STRING);

        return new TestContext(database);
    }

    [TestMethod]
    public void GetStoredProcCommand_Should_set_CommandType_to_StoredProcedure()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        DbCommand command = context.Database.GetStoredProcCommand("GetItem");

        // Assert
        command.CommandType.Should().Be(CommandType.StoredProcedure);
    }

    [TestMethod]
    public void GetStoredProcCommand_Should_set_CommandText_to_given_name()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        DbCommand command = context.Database.GetStoredProcCommand("GetItem");

        // Assert
        command.CommandText.Should().Be("GetItem");
    }

    [TestMethod]
    public void AddInParameter_Should_add_parameter_with_name_type_and_value()
    {
        // Arrange
        TestContext context = CreateContext();
        DbCommand command = context.Database.GetStoredProcCommand("GetItem");

        // Act
        context.Database.AddInParameter(command, "Id", DbType.Int32, 42);

        // Assert
        SqlParameter parameter = (SqlParameter)command.Parameters["@Id"];

        parameter.DbType.Should().Be(DbType.Int32);
        parameter.Direction.Should().Be(ParameterDirection.Input);
        parameter.Value.Should().Be(42);
    }

    [TestMethod]
    public void AddInParameter_With_empty_string_value_Should_set_a_valid_Size()
    {
        // Arrange
        TestContext context = CreateContext();
        DbCommand command = context.Database.GetStoredProcCommand("SaveItem");

        // Act
        context.Database.AddInParameter(command, "Name", DbType.String, string.Empty);

        // Assert
        SqlParameter parameter = (SqlParameter)command.Parameters["@Name"];

        parameter.Size.Should().BeGreaterThan(0);
    }

    [TestMethod]
    public void AddInParameter_With_byte_array_value_Should_set_a_valid_Size()
    {
        // Arrange
        TestContext context = CreateContext();
        DbCommand command = context.Database.GetStoredProcCommand("SaveItem");

        // Act
        context.Database.AddInParameter(command, "Data", DbType.Binary, new byte[] { 1, 2, 3 });

        // Assert
        SqlParameter parameter = (SqlParameter)command.Parameters["@Data"];

        parameter.Size.Should().Be(3);
    }

    [TestMethod]
    public void AddInParameter_With_char_value_Should_set_a_valid_Size()
    {
        // Arrange
        TestContext context = CreateContext();
        DbCommand command = context.Database.GetStoredProcCommand("SaveItem");

        // Act
        context.Database.AddInParameter(command, "Flag", DbType.StringFixedLength, "Y");

        // Assert
        SqlParameter parameter = (SqlParameter)command.Parameters["@Flag"];

        parameter.Size.Should().Be(1);
    }

    [TestMethod]
    public void AddInParameter_With_char_and_null_value_Should_set_a_valid_Size()
    {
        // Arrange
        TestContext context = CreateContext();
        DbCommand command = context.Database.GetStoredProcCommand("SaveItem");

        // Act
        context.Database.AddInParameter(command, "Flag", DbType.StringFixedLength, null);

        // Assert
        SqlParameter parameter = (SqlParameter)command.Parameters["@Flag"];

        parameter.Size.Should().Be(1);
    }

    [TestMethod]
    public void Simulated_TestIntDataItem_save_parameter_sequence_Should_have_valid_Size_for_all_string_parameters()
    {
        // Arrange
        TestContext context = CreateContext();
        DbCommand command = context.Database.GetStoredProcCommand("gen_TestInt_SaveRow");

        // Act
        context.Database.AddParameter(command, "Id", DbType.Int32, ParameterDirection.InputOutput, "Id", DataRowVersion.Proposed, 0);
        context.Database.AddParameter(command, "TestIntId", DbType.Guid, ParameterDirection.InputOutput, "TestIntId", DataRowVersion.Proposed, null);
        context.Database.AddParameter(command, "SysInsertTime", DbType.DateTime, ParameterDirection.InputOutput, "SysInsertTime", DataRowVersion.Proposed, null);
        context.Database.AddParameter(command, "SysUpdateTime", DbType.DateTime, ParameterDirection.InputOutput, "SysUpdateTime", DataRowVersion.Proposed, null);
        context.Database.AddInParameter(command, "testTinyInt", DbType.Byte, (byte)0);
        context.Database.AddInParameter(command, "testBigInt", DbType.Int64, (long)0);
        context.Database.AddInParameter(command, "testInt", DbType.Int32, 0);
        context.Database.AddInParameter(command, "testNtext", DbType.String, string.Empty);
        context.Database.AddInParameter(command, "SysLock", DbType.String, string.Empty);

        // Assert
        for (int index = 0; index < command.Parameters.Count; index++)
        {
            SqlParameter parameter = (SqlParameter)command.Parameters[index];

            bool needsSize = parameter.SqlDbType is SqlDbType.NVarChar or SqlDbType.VarChar or SqlDbType.VarBinary or SqlDbType.NChar or SqlDbType.Char;

            if (needsSize)
            {
                parameter.Size.Should().NotBe(0, $"parameter[{index}] '{parameter.ParameterName}' ({parameter.SqlDbType}) must have a valid Size");
            }
        }
    }

    [TestMethod]
    public void AddInParameter_With_null_value_Should_add_DBNull_value()
    {
        // Arrange
        TestContext context = CreateContext();
        DbCommand command = context.Database.GetStoredProcCommand("GetItem");

        // Act
        context.Database.AddInParameter(command, "Description", DbType.String, null);

        // Assert
        command.Parameters["@Description"].Value.Should().Be(DBNull.Value);
    }

    [TestMethod]
    public void AddParameter_Should_set_direction_sourceColumn_and_sourceVersion()
    {
        // Arrange
        TestContext context = CreateContext();
        DbCommand command = context.Database.GetStoredProcCommand("SaveItem");

        // Act
        context.Database.AddParameter(command, "Id", DbType.Int32, ParameterDirection.InputOutput, "Id", DataRowVersion.Proposed, 7);

        // Assert
        DbParameter parameter = command.Parameters["@Id"];

        parameter.Direction.Should().Be(ParameterDirection.InputOutput);
        parameter.SourceColumn.Should().Be("Id");
        parameter.SourceVersion.Should().Be(DataRowVersion.Proposed);
        parameter.Value.Should().Be(7);
    }

    [TestMethod]
    public void GetParameterValue_Should_return_previously_set_value()
    {
        // Arrange
        TestContext context = CreateContext();
        DbCommand command = context.Database.GetStoredProcCommand("SaveItem");

        context.Database.AddParameter(command, "Id", DbType.Int32, ParameterDirection.InputOutput, "Id", DataRowVersion.Proposed, null);
        command.Parameters["@Id"].Value = 99;

        // Act
        object value = context.Database.GetParameterValue(command, "Id");

        // Assert
        value.Should().Be(99);
    }

    [TestMethod]
    public void CreateConnection_Should_return_connection_with_given_connectionString()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        DbConnection connection = context.Database.CreateConnection();

        // Assert
        connection.ConnectionString.Should().Be(CONNECTION_STRING);
    }

    [TestMethod]
    public void BuildParameterName_Without_leading_at_Should_add_leading_at()
    {
        // Arrange

        // Act
        string parameterName = SqlDatabase.BuildParameterName("Id");

        // Assert
        parameterName.Should().Be("@Id");
    }

    [TestMethod]
    public void BuildParameterName_With_leading_at_Should_leave_unchanged()
    {
        // Arrange

        // Act
        string parameterName = SqlDatabase.BuildParameterName("@Id");

        // Assert
        parameterName.Should().Be("@Id");
    }
}
