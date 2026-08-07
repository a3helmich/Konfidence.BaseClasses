using System;
using System.Linq;
using FluentAssertions;
using Konfidence.SqlHostProvider.SqlAccess;
using Konfidence.SqlHostProvider.SqlDbSchema;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.SqlHostProvider.LocalDb.UnitTest;

[TestClass]
public class DatabaseStructureTests : LocalDbTestBase
{
    [TestMethod]
    public void BuildStructure_TestClassGeneratorDatabase_GeneratesStructureForAllTables()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.Target.BuildStructure();

        // Assert
        context.Target.Tables.Should().HaveCount(8);
    }

    [TestMethod]
    public void BuildStructure_Always_SetsPrimaryKeyPerTable()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.Target.BuildStructure();

        // Assert
        context.Target.Tables.First(x => x.Name == "Test1").PrimaryKey.Should().Be("Id");
        context.Target.Tables.First(x => x.Name == "Test6").PrimaryKey.Should().Be("Test6Id");
        context.Target.Tables.First(x => x.Name == "Test7Exlude").PrimaryKey.Should().Be("Test7ExludeId");
    }

    [TestMethod]
    public void BuildStructure_TestIntTable_SetsHasGuidId()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.Target.BuildStructure();

        // Assert
        context.Target.Tables.First(x => x.Name == "TestInt").HasGuidId.Should().BeTrue();
    }

    [TestMethod]
    public void TableExists_ExistingTable_ReturnsTrue()
    {
        // Arrange
        TestContext context = CreateContext();
        context.Target.BuildStructure();

        // Act
        bool tableExists = context.Client.TableExists("Test1");

        // Assert
        tableExists.Should().BeTrue();
    }

    [TestMethod]
    public void TableExists_NonExistentTable_ReturnsFalse()
    {
        // Arrange
        TestContext context = CreateContext();
        context.Target.BuildStructure();

        // Act
        bool tableExists = context.Client.TableExists("DoesNotExist");

        // Assert
        tableExists.Should().BeFalse();
    }

    private sealed class TestContext
    {
        public TestContext(SqlClient client, DatabaseStructure target)
        {
            Client = client;
            Target = target;
        }

        public SqlClient Client { get; }

        public DatabaseStructure Target { get; }
    }

    private static TestContext CreateContext()
    {
        IServiceProvider dependencyProvider = DependencyInjectionFactory.ConfigureDependencyInjection();

        IClientConfig? clientConfig = dependencyProvider.GetService<IClientConfig>();

        if (clientConfig is null)
        {
            throw new InvalidOperationException("ClientConfig not returned by dependency injection");
        }

        clientConfig.DefaultDatabase = "TestClassGenerator";

        SqlClient client = new(new SqlClientRepository(clientConfig));
        DatabaseStructure target = new(client);

        return new TestContext(client, target);
    }
}
