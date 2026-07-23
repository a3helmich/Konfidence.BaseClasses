using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.SqlDataAccess.UnitTest;

[TestClass]
public class DatabaseSettingsTests
{
    [TestMethod]
    public void DefaultDatabase_GetAndSet_RoundTrips()
    {
        // Arrange
        DatabaseSettings databaseSettings = new();

        // Act
        databaseSettings.DefaultDatabase = "TestClassGenerator";

        // Assert
        databaseSettings.DefaultDatabase.Should().Be("TestClassGenerator");
    }

    [TestMethod]
    public void DefaultDatabase_WhenUnset_DefaultsToEmpty()
    {
        // Arrange
        DatabaseSettings databaseSettings = new();

        // Act
        string? defaultDatabase = databaseSettings.DefaultDatabase;

        // Assert
        defaultDatabase.Should().BeEmpty();
    }
}
