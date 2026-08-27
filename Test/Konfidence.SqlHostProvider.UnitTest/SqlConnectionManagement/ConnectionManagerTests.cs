using Konfidence.SqlHostProvider.SqlConnectionManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Konfidence.SqlHostProvider.UnitTest.SqlConnectionManagement;

[TestClass]
public class ConnectionManagerTests
{
    [TestMethod]
    public void SetActiveConnection_Always_SelectsThatConnectionAsTheDefault()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.ConnectionManager.SetActiveConnection("SchemaDatabaseDevelopment");

        // Assert
        context.WriterMock.Verify(x => x.SetDefaultDatabase("SchemaDatabaseDevelopment"), Times.Once);
    }

    [TestMethod]
    public void SetApplicationDatabase_Always_WritesTheConnectionNameFirst()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.ConnectionManager.SetApplicationDatabase("Newsletter", "devserver", "SchemaDatabaseDevelopment");

        // Assert
        context.WriterMock.Verify(x => x.SetConnectionString("SchemaDatabaseDevelopment", "Newsletter", "devserver"), Times.Once);
    }

    [TestMethod]
    public void SetApplicationDatabase_ForTwoConnectionNames_PointsEachAtItsOwnServer()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.ConnectionManager.SetApplicationDatabase("Newsletter", "devserver", "SchemaDatabaseDevelopment");
        context.ConnectionManager.SetApplicationDatabase("Newsletter", "prodserver", "SchemaDatabaseDeployment");

        // Assert
        context.WriterMock.Verify(x => x.SetConnectionString("SchemaDatabaseDevelopment", "Newsletter", "devserver"), Times.Once);
        context.WriterMock.Verify(x => x.SetConnectionString("SchemaDatabaseDeployment", "Newsletter", "prodserver"), Times.Once);
    }

    [TestMethod]
    public void SetApplicationDatabase_ForTheSameConnectionTwice_RepointsIt()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.ConnectionManager.SetApplicationDatabase("Newsletter", "devserver", "SchemaDatabaseDevelopment");
        context.ConnectionManager.SetApplicationDatabase("MyBooks", "otherserver", "SchemaDatabaseDevelopment");

        // Assert
        context.WriterMock.Verify(x => x.SetConnectionString("SchemaDatabaseDevelopment", "MyBooks", "otherserver"), Times.Once);
    }

    [TestMethod]
    public void SetActiveConnection_SwitchingBetweenTargets_SelectsEachInTurn()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.ConnectionManager.SetActiveConnection("SchemaDatabaseDevelopment");
        context.ConnectionManager.SetActiveConnection("SchemaDatabaseDeployment");

        // Assert
        context.WriterMock.Verify(x => x.SetDefaultDatabase("SchemaDatabaseDevelopment"), Times.Once);
        context.WriterMock.Verify(x => x.SetDefaultDatabase("SchemaDatabaseDeployment"), Times.Once);
    }

    private static TestContext CreateContext()
    {
        Mock<IApplicationConfigurationWriter> writerMock = new();

        return new TestContext(new ConnectionManager(writerMock.Object), writerMock);
    }

    private sealed class TestContext
    {
        public ConnectionManager ConnectionManager { get; }

        public Mock<IApplicationConfigurationWriter> WriterMock { get; }

        public TestContext(ConnectionManager connectionManager, Mock<IApplicationConfigurationWriter> writerMock)
        {
            ConnectionManager = connectionManager;
            WriterMock = writerMock;
        }
    }
}
