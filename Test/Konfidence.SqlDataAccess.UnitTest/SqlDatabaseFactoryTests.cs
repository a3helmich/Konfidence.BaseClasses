using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.SqlDataAccess.UnitTest;

[TestClass]
public class SqlDatabaseFactoryTests
{
    [TestMethod]
    public void Create_WithNullConnectionString_Throws()
    {
        // Arrange
        string? connectionString = null;

        // Act
        Action action = () => SqlDatabaseFactory.Create(connectionString!);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Create_WithEmptyConnectionString_Throws()
    {
        // Arrange

        // Act
        Action action = () => SqlDatabaseFactory.Create(string.Empty);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Create_WithWhitespaceConnectionString_Throws()
    {
        // Arrange

        // Act
        Action action = () => SqlDatabaseFactory.Create("   ");

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Create_WithValidConnectionString_ReturnsSqlDatabase()
    {
        // Arrange
        const string connectionString = "Data Source=konfidence2;Initial Catalog=TestClassGenerator;Integrated Security=True";

        // Act
        SqlDatabase database = SqlDatabaseFactory.Create(connectionString);

        // Assert
        database.Should().NotBeNull();
        database.CreateConnection().ConnectionString.Should().Be(connectionString);
    }
}
