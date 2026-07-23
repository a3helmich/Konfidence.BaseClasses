using System;
using System.Collections.Generic;
using System.Data;
using FluentAssertions;
using Konfidence.DatabaseInterface;
using Konfidence.SqlHostProvider.SqlAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Konfidence.SqlHostProvider.UnitTest.SqlAccess;

[TestClass]
public class SqlClientTests
{
    private static Mock<IBaseDataItem> CreateDataItemMock()
    {
        Mock<IBaseDataItem> dataItemMock = new();

        dataItemMock.SetupProperty(x => x.AutoIdField, string.Empty);
        dataItemMock.SetupProperty(x => x.SaveStoredProcedure, string.Empty);
        dataItemMock.SetupProperty(x => x.GetStoredProcedure, string.Empty);
        dataItemMock.SetupProperty(x => x.DeleteStoredProcedure, string.Empty);

        return dataItemMock;
    }

    private sealed class FakeBaseDataItem : IBaseDataItem
    {
        public string GuidIdField { get; set; } = string.Empty;
        public string AutoIdField { get; set; } = string.Empty;
        public Dictionary<string, ISpParameterData> AutoUpdateFieldDictionary { get; } = [];
        public string GetStoredProcedure { get; set; } = string.Empty;
        public string DeleteStoredProcedure { get; set; } = string.Empty;
        public string SaveStoredProcedure { get; set; } = string.Empty;
        public string GetByGuidStoredProcedure { get; set; } = string.Empty;
        public void InitializeDataItem() { }
        public List<ISpParameterData> SetItemData() => [];
        public void Save() { }
        public void Delete() { }
        public int GetId() => 0;
        public void SetId(int id) { }
        public void GetKey(IDataReader dataReader) { }
        public void GetData(IDataReader dataReader) { }
        public List<ISpParameterData> GetParameterObjects() => [];
        public bool IsNew => true;
    }

    [TestMethod]
    public void Save_WithEmptyAutoIdField_Throws()
    {
        // Arrange
        TestContext context = CreateContext();
        Mock<IBaseDataItem> dataItemMock = CreateDataItemMock();

        // Act
        Action action = () => context.Client.Save(dataItemMock.Object);

        // Assert
        action.Should().Throw<Exception>().WithMessage("AutoIdField not generated");
    }

    [TestMethod]
    public void Save_WithEmptySaveStoredProcedure_Throws()
    {
        // Arrange
        TestContext context = CreateContext();
        Mock<IBaseDataItem> dataItemMock = CreateDataItemMock();
        dataItemMock.Object.AutoIdField = "Id";

        // Act
        Action action = () => context.Client.Save(dataItemMock.Object);

        // Assert
        action.Should().Throw<Exception>().WithMessage("SaveStoredProcedure not generated/installed");
    }

    [TestMethod]
    public void Save_WithValidDataItem_CallsExecuteSaveStoredProcedure()
    {
        // Arrange
        TestContext context = CreateContext();
        Mock<IBaseDataItem> dataItemMock = CreateDataItemMock();
        dataItemMock.Object.AutoIdField = "Id";
        dataItemMock.Object.SaveStoredProcedure = "SaveItem";

        // Act
        context.Client.Save(dataItemMock.Object);

        // Assert
        context.RepositoryMock.Verify(x => x.ExecuteSaveStoredProcedure(dataItemMock.Object), Times.Once);
    }

    [TestMethod]
    public void GetItem_WithEmptyGetStoredProcedure_Throws()
    {
        // Arrange
        TestContext context = CreateContext();
        Mock<IBaseDataItem> dataItemMock = CreateDataItemMock();

        // Act
        Action action = () => context.Client.GetItem(dataItemMock.Object);

        // Assert
        action.Should().Throw<Exception>().WithMessage("GetStoredProcedure not generated/installed");
    }

    [TestMethod]
    public void GetItem_WithValidDataItem_CallsExecuteGetStoredProcedure()
    {
        // Arrange
        TestContext context = CreateContext();
        Mock<IBaseDataItem> dataItemMock = CreateDataItemMock();
        dataItemMock.Object.GetStoredProcedure = "GetItem";

        // Act
        context.Client.GetItem(dataItemMock.Object);

        // Assert
        context.RepositoryMock.Verify(x => x.ExecuteGetStoredProcedure(dataItemMock.Object), Times.Once);
    }

    [TestMethod]
    public void GetItemBy_WithEmptyStoredProcedure_Throws()
    {
        // Arrange
        TestContext context = CreateContext();
        Mock<IBaseDataItem> dataItemMock = CreateDataItemMock();

        // Act
        Action action = () => context.Client.GetItemBy(dataItemMock.Object, string.Empty);

        // Assert
        action.Should().Throw<Exception>().WithMessage("GetStoredProcedure not generated/installed");
    }

    [TestMethod]
    public void GetItemBy_WithValidStoredProcedure_CallsExecuteGetByStoredProcedure()
    {
        // Arrange
        TestContext context = CreateContext();
        Mock<IBaseDataItem> dataItemMock = CreateDataItemMock();

        // Act
        context.Client.GetItemBy(dataItemMock.Object, "GetItemByGuid");

        // Assert
        context.RepositoryMock.Verify(x => x.ExecuteGetByStoredProcedure(dataItemMock.Object, "GetItemByGuid"), Times.Once);
    }

    [TestMethod]
    public void BuildItemList_WithEmptyStoredProcedure_Throws()
    {
        // Arrange
        TestContext context = CreateContext();
        List<FakeBaseDataItem> dataItemList = [];

        // Act
        Action action = () => context.Client.BuildItemList(dataItemList, string.Empty);

        // Assert
        action.Should().Throw<Exception>().WithMessage("GetListStoredProcedure not generated/installed");
    }

    [TestMethod]
    public void BuildItemList_WithValidStoredProcedure_CallsExecuteGetListStoredProcedure()
    {
        // Arrange
        TestContext context = CreateContext();
        List<FakeBaseDataItem> dataItemList = [];

        // Act
        context.Client.BuildItemList(dataItemList, "GetList");

        // Assert
        context.RepositoryMock.Verify(x => x.ExecuteGetListStoredProcedure(dataItemList, "GetList", context.Client), Times.Once);
    }

    [TestMethod]
    public void BuildItemListWithParameters_WithEmptyStoredProcedure_Throws()
    {
        // Arrange
        TestContext context = CreateContext();
        List<FakeBaseDataItem> dataItemList = [];
        List<ISpParameterData> spParameters = [];

        // Act
        Action action = () => context.Client.BuildItemList(dataItemList, string.Empty, spParameters);

        // Assert
        action.Should().Throw<Exception>().WithMessage("GetListStoredProcedure not generated/installed");
    }

    [TestMethod]
    public void BuildItemListWithParameters_WithValidStoredProcedure_CallsExecuteGetListStoredProcedure()
    {
        // Arrange
        TestContext context = CreateContext();
        List<FakeBaseDataItem> dataItemList = [];
        List<ISpParameterData> spParameters = [];

        // Act
        context.Client.BuildItemList(dataItemList, "GetList", spParameters);

        // Assert
        context.RepositoryMock.Verify(x => x.ExecuteGetListStoredProcedure(dataItemList, "GetList", spParameters, context.Client), Times.Once);
    }

    [TestMethod]
    public void Delete_WithEmptyDeleteStoredProcedure_Throws()
    {
        // Arrange
        TestContext context = CreateContext();
        Mock<IBaseDataItem> dataItemMock = CreateDataItemMock();

        // Act
        Action action = () => context.Client.Delete(dataItemMock.Object);

        // Assert
        action.Should().Throw<Exception>().WithMessage("DeleteStoredProcedure not generated/installed");
    }

    [TestMethod]
    public void Delete_WithValidDataItem_CallsExecuteDeleteStoredProcedure()
    {
        // Arrange
        TestContext context = CreateContext();
        Mock<IBaseDataItem> dataItemMock = CreateDataItemMock();
        dataItemMock.Object.DeleteStoredProcedure = "DeleteItem";

        // Act
        context.Client.Delete(dataItemMock.Object);

        // Assert
        context.RepositoryMock.Verify(x => x.ExecuteDeleteStoredProcedure(dataItemMock.Object), Times.Once);
    }

    [TestMethod]
    public void ExecuteCommand_Always_ReturnsRepositoryResult()
    {
        // Arrange
        TestContext context = CreateContext();
        List<ISpParameterData> parameters = [];
        context.RepositoryMock.Setup(x => x.ExecuteCommandStoredProcedure("DoSomething", parameters)).Returns(5);

        // Act
        int result = context.Client.ExecuteCommand("DoSomething", parameters);

        // Assert
        result.Should().Be(5);
    }

    [TestMethod]
    public void ExecuteTextCommand_Always_ReturnsRepositoryResult()
    {
        // Arrange
        TestContext context = CreateContext();
        context.RepositoryMock.Setup(x => x.ExecuteTextCommandQuery("SELECT 1")).Returns(1);

        // Act
        int result = context.Client.ExecuteTextCommand("SELECT 1");

        // Assert
        result.Should().Be(1);
    }

    [TestMethod]
    public void TableExists_Always_CallsObjectExistsWithTablesCollection()
    {
        // Arrange
        TestContext context = CreateContext();
        context.RepositoryMock.Setup(x => x.ObjectExists("MyTable", "Tables")).Returns(true);

        // Act
        bool result = context.Client.TableExists("MyTable");

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ViewExists_Always_CallsObjectExistsWithViewsCollection()
    {
        // Arrange
        TestContext context = CreateContext();
        context.RepositoryMock.Setup(x => x.ObjectExists("MyView", "Views")).Returns(true);

        // Act
        bool result = context.Client.ViewExists("MyView");

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void StoredProcedureExists_Always_CallsObjectExistsWithProceduresCollection()
    {
        // Arrange
        TestContext context = CreateContext();
        context.RepositoryMock.Setup(x => x.ObjectExists("MyProcedure", "Procedures")).Returns(true);

        // Act
        bool result = context.Client.StoredProcedureExists("MyProcedure");

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void GetSchemaObject_Always_ReturnsRepositoryResult()
    {
        // Arrange
        TestContext context = CreateContext();
        DataTable schemaTable = new();
        context.RepositoryMock.Setup(x => x.GetSchemaObject("Tables")).Returns(schemaTable);

        // Act
        DataTable result = context.Client.GetSchemaObject("Tables");

        // Assert
        result.Should().BeSameAs(schemaTable);
    }

    private sealed class TestContext
    {
        public TestContext(
            SqlClient Client,
            Mock<IDataRepository> RepositoryMock
        )
        {
            this.Client = Client;
            this.RepositoryMock = RepositoryMock;
        }

        public SqlClient Client { get; }

        public Mock<IDataRepository> RepositoryMock { get; }
    }

    private static TestContext CreateContext()
    {
        Mock<IDataRepository> repositoryMock = new();
        SqlClient client = new(repositoryMock.Object);

        return new TestContext(client, repositoryMock);
    }
}
