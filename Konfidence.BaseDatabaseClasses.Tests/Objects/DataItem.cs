﻿using Konfidence.BaseData;
using Konfidence.BaseDataInterfaces;
using Moq;

namespace Konfidence.BaseDatabaseClasses.Tests.Objects
{
    public class DataItem: BaseDataItem
    {
        public override IBaseClient ClientBind()
        {
            var sqlClientMock = new Mock<IBaseClient>();

            return sqlClientMock.Object;
        }
    }
}
