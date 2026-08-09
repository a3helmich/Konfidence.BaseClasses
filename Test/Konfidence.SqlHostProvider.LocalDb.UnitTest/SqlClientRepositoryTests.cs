using System;
using System.Data;
using FluentAssertions;
using Konfidence.Base;
using Konfidence.DatabaseInterface;
using Konfidence.SqlHostProvider.SqlAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Konfidence.SqlHostProvider.LocalDb.UnitTest;

[TestClass]
public class SqlClientRepositoryTests
{
    [TestMethod]
    public void ExecuteCommandStoredProcedure_Always_ReturnsRowsAffected()
    {
        // Arrange
        TestContext context = CreateContext();

        Mock<ISpParameterData> idParameterMock = new();
        idParameterMock.Setup(x => x.ParameterName).Returns("Id");
        idParameterMock.Setup(x => x.DbType).Returns(DbType.Int32);
        idParameterMock.Setup(x => x.Value).Returns(int.MaxValue);

        // Act
        int rowsAffected = context.Repository.ExecuteCommandStoredProcedure("gen_TestInt_DeleteRow", [idParameterMock.Object]);

        // Assert
        // No row exists with this Id, so nothing is actually deleted - this only confirms the
        // stored procedure executes end-to-end and reports a rows-affected count.
        rowsAffected.Should().Be(0);
    }

    private sealed class TestContext
    {
        public TestContext(SqlClientRepository repository)
        {
            Repository = repository;
        }

        public SqlClientRepository Repository { get; }
    }

    private static TestContext CreateContext()
    {
        IServiceProvider dependencyProvider = DependencyInjectionFactory.ConfigureDependencyInjection();

        IClientConfig? clientConfig = dependencyProvider.GetService<IClientConfig>();

        if (!clientConfig.IsAssigned())
        {
            throw new InvalidOperationException("ClientConfig not returned by dependency injection");
        }

        clientConfig.DefaultDatabase = "TestClassGenerator";

        return new TestContext(new SqlClientRepository(clientConfig));
    }
}
