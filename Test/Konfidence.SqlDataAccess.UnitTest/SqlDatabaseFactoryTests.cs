using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.SqlDataAccess.UnitTest;

[TestClass]
public class SqlDatabaseFactoryTests
{
    [TestMethod]
    public void Create_With_null_connectionString_Should_throw()
    {
        // Arrange
        string? connectionString = null;

        // Act
        Action action = () => SqlDatabaseFactory.Create(connectionString!);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Create_With_empty_connectionString_Should_throw()
    {
        // Arrange

        // Act
        Action action = () => SqlDatabaseFactory.Create(string.Empty);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Create_With_whitespace_connectionString_Should_throw()
    {
        // Arrange

        // Act
        Action action = () => SqlDatabaseFactory.Create("   ");

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Create_With_valid_connectionString_Should_return_SqlDatabase()
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
