using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using JetBrains.Annotations;
using Konfidence.DataBaseInterface;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Konfidence.RepositoryInterface
{
    public interface IDataRepository
    {
        Database GetDatabase();

        [UsedImplicitly]
        DbCommand GetStoredProcCommand(string saveStoredProcedure);

        int ExecuteNonQueryStoredProcedure(string saveStoredProcedure, List<IDbParameterObject> parameterObjectList);

        void ExecuteSaveStoredProcedure(IBaseDataItem dataItem);

        void ExecuteGetStoredProcedure(IBaseDataItem dataItem);

        void ExecuteGetByStoredProcedure(IBaseDataItem dataItem, string storedProcedure);

        void ExecuteGetListStoredProcedure<T>(IBaseDataItemList<T> baseDataItemList, string storedProcedure, IBaseClient baseClient) where T : IBaseDataItem;

        void ExecuteGetRelatedListStoredProcedure<T>(string storedProcedure,
            IBaseDataItemList<T> parentDataItemList, IBaseDataItemList<T> relatedDataItemList,
            IBaseDataItemList<T> childDataItemList, IBaseClient baseClient) where T : IBaseDataItem;

        void ExecuteDeleteStoredProcedure(IBaseDataItem dataItem);

        int ExecuteNonQuery(string textCommand);

        [UsedImplicitly]
        int ExecuteNonQuery(DbCommand commandType);

        bool ObjectExists(string objectName, string collection);
    }
}