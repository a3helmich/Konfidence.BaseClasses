using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using JetBrains.Annotations;
using Konfidence.DataBaseInterface;
using Konfidence.RepositoryInterface.Objects;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Konfidence.RepositoryInterface
{
    public interface IDataRepository
    {
        Database GetDatabase();

        [UsedImplicitly]
        DbCommand GetStoredProcCommand(string saveStoredProcedure);

        int ExecuteNonQueryStoredProcedure(string saveStoredProcedure, List<IDbParameterObject> parameterObjectList);

        ResponseParameters ExecuteSaveStoredProcedure(RequestParameters executeParameters);

        void ExecuteGetStoredProcedure(RetrieveParameters retrieveParameters, IBaseDataItem baseDataItem);

        void ExecuteGetListStoredProcedure<T>(RetrieveListParameters<T> retrieveListParameters, IBaseDataItemList<T> baseDataItemList, IBaseClient baseClient) where T : IBaseDataItem;

        void ExecuteGetRelatedListStoredProcedure<T>(RetrieveListParameters<T> retrieveListParameters,
            IBaseDataItemList<T> parentDataItemList, IBaseDataItemList<T> relatedDataItemList,
            IBaseDataItemList<T> childDataItemList, IBaseClient baseClient) where T : IBaseDataItem;

        void ExecuteDeleteStoredProcedure(string deleteStoredProcedure, string autoIdField, int id);

        int ExecuteNonQuery(string textCommand);

        [UsedImplicitly]
        int ExecuteNonQuery(DbCommand commandType);

        bool ObjectExists(string objectName, string collection);
    }
}