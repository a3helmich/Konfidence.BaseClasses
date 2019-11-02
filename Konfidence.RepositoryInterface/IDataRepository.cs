using System;
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

        IDataReader DataReader { get; }

        [UsedImplicitly]
        DbCommand GetStoredProcCommand(string saveStoredProcedure);

        int ExecuteNonQueryStoredProcedure(string saveStoredProcedure, IDbParameterObjectList parameterObjectList);

        ResponseParameters ExecuteSaveStoredProcedure(RequestParameters executeParameters);

        void ExecuteGetStoredProcedure(RetrieveParameters retrieveParameters, Func<bool> callback);

        void ExecuteGetListStoredProcedure<T>(RetrieveListParameters<T> retrieveListParameters, Func<bool> callback) where T : IBaseDataItem;

        void ExecuteGetRelatedListStoredProcedure<T>(RetrieveListParameters<T> retrieveListParameters,
                                                  Func<bool> parentCallback, Func<bool> relatedCallback,
                                                  Func<bool> childCallback) where T : IBaseDataItem;

        void ExecuteDeleteStoredProcedure(string deleteStoredProcedure, string autoIdField, int id);

        int ExecuteNonQuery(string textCommand);

        [UsedImplicitly]
        int ExecuteNonQuery(DbCommand commandType);

        bool ObjectExists(string objectName, string collection);
    }
}