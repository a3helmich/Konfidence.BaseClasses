using System.Collections.Generic;
using FluentAssertions;
using Konfidence.BaseData;
using Konfidence.DatabaseInterface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseDatabaseClasses.UnitTest;

[TestClass]
public class BaseDataItemTests
{
    private sealed class TestDataItem : BaseDataItem;

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
}
