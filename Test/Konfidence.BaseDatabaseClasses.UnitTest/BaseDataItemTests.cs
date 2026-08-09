using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FluentAssertions;
using Konfidence.BaseData;
using Konfidence.DatabaseInterface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Konfidence.BaseDatabaseClasses.UnitTest;

[TestClass]
public class BaseDataItemTests
{
    [TestMethod]
    public void GetParameterObjects_CalledTwiceAfterSettingAFieldOnce_SecondCallReturnsNoLeftoverParameters()
    {
        // Arrange
        TestDataItem dataItem = new();
        dataItem.SetField("Field", 7);

        // Act
        List<ISpParameterData> firstCall = dataItem.GetParameterObjects();
        List<ISpParameterData> secondCall = dataItem.GetParameterObjects();

        // Assert
        // Before the fix, GetParameterObjects() returned the live internal parameter list without
        // ever resetting it, so a data item reused for a second "get" call (without setting fields
        // again) would resend leftover parameters from the first call.
        firstCall.Should().HaveCount(1);
        secondCall.Should().BeEmpty();
    }

    [TestMethod]
    public void SetId_ThenGetId_ReturnsAssignedId()
    {
        // Arrange
        TestDataItem dataItem = new();

        // Act
        dataItem.SetId(7);

        // Assert
        dataItem.GetId().Should().Be(7);
    }

    [TestMethod]
    public void IsNew_WithoutId_ReturnsTrue()
    {
        // Arrange
        TestDataItem dataItem = new();

        // Act
        bool isNew = dataItem.IsNew;

        // Assert
        isNew.Should().BeTrue();
    }

    [TestMethod]
    public void IsNew_WithAssignedId_ReturnsFalse()
    {
        // Arrange
        TestDataItem dataItem = new();
        dataItem.SetId(7);

        // Act
        bool isNew = dataItem.IsNew;

        // Assert
        isNew.Should().BeFalse();
    }

    [TestMethod]
    public void GetKey_WithoutAutoIdField_LeavesIdUnchangedAndDoesNotReadFromDataReader()
    {
        // Arrange
        // AutoIdField defaults to an empty string, so GetKey has to bail out before touching the
        // reader - otherwise every item without an identity column would query ordinal "".
        TestDataItem dataItem = new();
        Mock<IDataReader> dataReaderMock = new();

        // Act
        dataItem.GetKey(dataReaderMock.Object);

        // Assert
        dataItem.GetId().Should().Be(0);
        dataReaderMock.Verify(x => x.GetOrdinal(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public void GetKey_WithAutoIdField_ReadsIdFromDataReader()
    {
        // Arrange
        TestDataItem dataItem = new() { AutoIdField = "Id" };
        Mock<IDataReader> dataReaderMock = new();
        dataReaderMock.Setup(x => x.GetOrdinal("Id")).Returns(0);
        dataReaderMock.Setup(x => x.IsDBNull(0)).Returns(false);
        dataReaderMock.Setup(x => x.GetInt32(0)).Returns(7);

        // Act
        dataItem.GetKey(dataReaderMock.Object);

        // Assert
        dataItem.GetId().Should().Be(7);
    }

    [TestMethod]
    public void AddAutoUpdateField_CalledTwiceForSameField_RegistersItOnlyOnce()
    {
        // Arrange
        // The ContainsKey guard means a second registration must be ignored rather than throwing
        // or replacing the existing entry (which would discard an already-fetched value).
        TestDataItem dataItem = new();
        dataItem.RegisterAutoUpdateField("Field", DbType.Int32);
        dataItem.AutoUpdateFieldDictionary["Field"].Value = 7;

        // Act
        dataItem.RegisterAutoUpdateField("Field", DbType.String);

        // Assert
        dataItem.AutoUpdateFieldDictionary.Should().ContainSingle();
        dataItem.AutoUpdateFieldDictionary["Field"].DbType.Should().Be(DbType.Int32);
        dataItem.AutoUpdateFieldDictionary["Field"].Value.Should().Be(7);
    }

    [TestMethod]
    public void GetAutoUpdateField_WithRegisteredFieldHoldingAValue_ReturnsThatValue()
    {
        // Arrange
        TestDataItem dataItem = new();
        dataItem.RegisterAutoUpdateField("Field", DbType.Int32);
        dataItem.AutoUpdateFieldDictionary["Field"].Value = 7;
        int fieldValue = 42;

        // Act
        dataItem.GetAutoUpdateField("Field", ref fieldValue);

        // Assert
        fieldValue.Should().Be(7);
    }

    [TestMethod]
    public void GetAutoUpdateField_WithUnregisteredField_FallsBackToExistingValue()
    {
        // Arrange
        // The dictionary lookup itself failing is a third distinct path, separate from a
        // registered-but-empty entry and from a null entry.
        TestDataItem dataItem = new();
        int fieldValue = 42;

        // Act
        dataItem.GetAutoUpdateField("Unregistered", ref fieldValue);

        // Assert
        fieldValue.Should().Be(42);
    }

    [TestMethod]
    public void GetAutoUpdateField_WithRegisteredFieldThatWasNeverFilled_FallsBackToExistingValue()
    {
        // Arrange
        // AddAutoUpdateField registers a real SpParameter whose Value starts out null, so the
        // lookup succeeds and the guard passes - it is the returned Value that is null, which is a
        // different path from both "field missing" and "entry itself is null".
        TestDataItem dataItem = new();
        dataItem.RegisterAutoUpdateField("Field", DbType.Int32);
        int fieldValue = 42;

        // Act
        dataItem.GetAutoUpdateField("Field", ref fieldValue);

        // Assert
        fieldValue.Should().Be(42);
    }

    [TestMethod]
    public void GetAutoUpdateField_WithNullParameterDataRegistered_FallsBackToExistingValue()
    {
        // Arrange
        // AutoUpdateFieldDictionary is public, so a null entry is reachable - the IsAssigned()
        // half of the lookup guard exists for exactly this case and is otherwise never taken.
        TestDataItem dataItem = new();
        dataItem.AutoUpdateFieldDictionary["Field"] = null!;
        int fieldValue = 42;

        // Act
        dataItem.GetAutoUpdateField("Field", ref fieldValue);

        // Assert
        fieldValue.Should().Be(42);
    }

    [TestMethod]
    public void Save_WithValidDataItem_CallsClientSaveAndRefreshesAutoUpdateData()
    {
        // Arrange
        // GetAutoUpdateData() must run after the client save so identity/computed columns written
        // by the stored procedure make it back onto the item.
        Mock<IBaseClient> clientMock = new();
        AutoUpdateTrackingDataItem dataItem = new();
        dataItem.AssignClient(clientMock.Object);

        // Act
        dataItem.Save();

        // Assert
        clientMock.Verify(x => x.Save(dataItem), Times.Once);
        dataItem.AutoUpdateDataCallCount.Should().Be(1);
    }

    [TestMethod]
    public void Save_WithInvalidDataItem_DoesNotCallClient()
    {
        // Arrange
        Mock<IBaseClient> clientMock = new();
        InvalidDataItem dataItem = new();
        dataItem.AssignClient(clientMock.Object);

        // Act
        dataItem.Save();

        // Assert
        clientMock.Verify(x => x.Save(It.IsAny<IBaseDataItem>()), Times.Never);
    }

    [TestMethod]
    public void Save_WithoutClient_DoesNotThrow()
    {
        // Arrange
        // Client is null until a repository assigns one, so every call site guards with "Client?." -
        // an unattached item has to stay silently inert rather than throwing.
        TestDataItem dataItem = new();

        // Act
        Action action = () => dataItem.Save();

        // Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void Delete_WithClient_CallsClientDeleteAndResetsId()
    {
        // Arrange
        Mock<IBaseClient> clientMock = new();
        TestDataItem dataItem = new();
        dataItem.AssignClient(clientMock.Object);
        dataItem.SetId(7);

        // Act
        dataItem.Delete();

        // Assert
        clientMock.Verify(x => x.Delete(dataItem), Times.Once);
        dataItem.GetId().Should().Be(0);
        dataItem.IsNew.Should().BeTrue();
    }

    [TestMethod]
    public void Delete_WithoutClient_StillResetsId()
    {
        // Arrange
        TestDataItem dataItem = new();
        dataItem.SetId(7);

        // Act
        dataItem.Delete();

        // Assert
        dataItem.GetId().Should().Be(0);
    }

    [TestMethod]
    public void GetItem_WithClient_CallsClientGetItem()
    {
        // Arrange
        Mock<IBaseClient> clientMock = new();
        TestDataItem dataItem = new();
        dataItem.AssignClient(clientMock.Object);

        // Act
        dataItem.InvokeGetItem();

        // Assert
        clientMock.Verify(x => x.GetItem(dataItem), Times.Once);
    }

    [TestMethod]
    public void GetItem_WithoutClient_DoesNotThrow()
    {
        // Arrange
        TestDataItem dataItem = new();

        // Act
        Action action = () => dataItem.InvokeGetItem();

        // Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void GetItemBy_WithClient_PassesStoredProcedureThrough()
    {
        // Arrange
        Mock<IBaseClient> clientMock = new();
        TestDataItem dataItem = new();
        dataItem.AssignClient(clientMock.Object);

        // Act
        dataItem.InvokeGetItemBy("GetByName");

        // Assert
        clientMock.Verify(x => x.GetItemBy(dataItem, "GetByName"), Times.Once);
    }

    [TestMethod]
    public void GetItemBy_WithoutClient_DoesNotThrow()
    {
        // Arrange
        TestDataItem dataItem = new();

        // Act
        Action action = () => dataItem.InvokeGetItemBy("GetByName");

        // Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void GetItemById_SetsAutoIdParameterBeforeCallingClient()
    {
        // Arrange
        Mock<IBaseClient> clientMock = new();
        List<ISpParameterData> capturedParameters = [];
        clientMock.Setup(x => x.GetItem(It.IsAny<IBaseDataItem>()))
            .Callback<IBaseDataItem>(item => capturedParameters = item.GetParameterObjects());

        TestDataItem dataItem = new() { AutoIdField = "Id" };
        dataItem.AssignClient(clientMock.Object);

        // Act
        dataItem.InvokeGetItem(7);

        // Assert
        ISpParameterData parameter = capturedParameters.Single();
        parameter.ParameterName.Should().Be("Id");
        parameter.Value.Should().Be(7);
    }

    [TestMethod]
    public void GetItemByGuid_SetsGuidParameterAndUsesGetByGuidStoredProcedure()
    {
        // Arrange
        Mock<IBaseClient> clientMock = new();
        List<ISpParameterData> capturedParameters = [];
        clientMock.Setup(x => x.GetItemBy(It.IsAny<IBaseDataItem>(), It.IsAny<string>()))
            .Callback<IBaseDataItem, string>((item, _) => capturedParameters = item.GetParameterObjects());

        Guid guidId = Guid.NewGuid();
        TestDataItem dataItem = new()
        {
            GuidIdField = "RowGuid",
            GetByGuidStoredProcedure = "GetByGuid"
        };
        dataItem.AssignClient(clientMock.Object);

        // Act
        dataItem.InvokeGetItem(guidId);

        // Assert
        clientMock.Verify(x => x.GetItemBy(dataItem, "GetByGuid"), Times.Once);

        ISpParameterData parameter = capturedParameters.Single();
        parameter.ParameterName.Should().Be("RowGuid");
        parameter.Value.Should().Be(guidId);
    }

    [TestMethod]
    public void SetItemData_CollectsParametersWrittenBySetData()
    {
        // Arrange
        // SetItemData is the "fill the parameter list, then hand it over" entry point, so the
        // SetData override has to run before the list is snapshotted and cleared.
        SetDataWritingDataItem dataItem = new();

        // Act
        List<ISpParameterData> parameters = dataItem.SetItemData();

        // Assert
        parameters.Should().ContainSingle();
        parameters.Single().ParameterName.Should().Be("Name");
        parameters.Single().Value.Should().Be("written by SetData");
        dataItem.GetParameterObjects().Should().BeEmpty();
    }

    [TestMethod]
    public void SetItemData_WithDefaultSetData_ReturnsNoParameters()
    {
        // Arrange
        TestDataItem dataItem = new();

        // Act
        List<ISpParameterData> parameters = dataItem.SetItemData();

        // Assert
        parameters.Should().BeEmpty();
    }

    [TestMethod]
    public void GetData_DefaultImplementation_DoesNotThrow()
    {
        // Arrange
        // GetData is a virtual no-op that generated data items override - the base version still
        // has to be safe to call for an item that does not.
        TestDataItem dataItem = new();
        Mock<IDataReader> dataReaderMock = new();

        // Act
        Action action = () => dataItem.GetData(dataReaderMock.Object);

        // Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void StoredProcedureProperties_RoundTripAssignedValues()
    {
        // Arrange
        TestDataItem dataItem = new();

        // Act
        dataItem.GetStoredProcedure = "Get";
        dataItem.SaveStoredProcedure = "Save";
        dataItem.DeleteStoredProcedure = "Delete";
        dataItem.GetByGuidStoredProcedure = "GetByGuid";

        // Assert
        dataItem.GetStoredProcedure.Should().Be("Get");
        dataItem.SaveStoredProcedure.Should().Be("Save");
        dataItem.DeleteStoredProcedure.Should().Be("Delete");
        dataItem.GetByGuidStoredProcedure.Should().Be("GetByGuid");
    }

    private class TestDataItem : BaseDataItem
    {
        public void AssignClient(IBaseClient client)
        {
            Client = client;
        }

        public void RegisterAutoUpdateField(string fieldName, DbType fieldType)
        {
            AddAutoUpdateField(fieldName, fieldType);
        }

        public void InvokeGetItem()
        {
            GetItem();
        }

        public void InvokeGetItem(int id)
        {
            GetItem(id);
        }

        public void InvokeGetItem(Guid guidId)
        {
            GetItem(guidId);
        }

        public void InvokeGetItemBy(string storedProcedure)
        {
            GetItemBy(storedProcedure);
        }
    }

    private sealed class AutoUpdateTrackingDataItem : TestDataItem
    {
        public int AutoUpdateDataCallCount { get; private set; }

        protected override void GetAutoUpdateData()
        {
            AutoUpdateDataCallCount++;
        }
    }

    private sealed class InvalidDataItem : TestDataItem
    {
        protected override bool IsValidDataItem()
        {
            return false;
        }
    }

    private sealed class SetDataWritingDataItem : TestDataItem
    {
        protected override void SetData()
        {
            this.SetField("Name", "written by SetData");
        }
    }
}
