﻿using System.Diagnostics.CodeAnalysis;
using Konfidence.BaseData;
using Konfidence.DataBaseInterface;
using Moq;

namespace Konfidence.BaseDatabaseClasses.Tests.Objects
{
    public class DataItem : BaseDataItem
    {
        protected override IBaseClient ClientBind()
        {
            var sqlClientMock = new Mock<IBaseClient>();

            return sqlClientMock.Object;
        }
    }
}