using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.SqlDataAccess.UnitTest;

[TestClass]
public class DatabaseSettingsTests
{
    [TestMethod]
    public void DefaultDatabase_Should_round_trip_get_and_set()
    {
        // Arrange
        DatabaseSettings databaseSettings = new();

        // Act
        databaseSettings.DefaultDatabase = "TestClassGenerator";

        // Assert
        databaseSettings.DefaultDatabase.Should().Be("TestClassGenerator");
    }

    [TestMethod]
    public void DefaultDatabase_Should_default_to_empty()
    {
        // Arrange
        DatabaseSettings databaseSettings = new();

        // Act
        string? defaultDatabase = databaseSettings.DefaultDatabase;

        // Assert
        defaultDatabase.Should().BeEmpty();
    }
}
