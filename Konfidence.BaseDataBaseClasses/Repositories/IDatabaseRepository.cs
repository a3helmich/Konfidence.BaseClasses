using System;
using System.Data;
using System.Data.Common;
using Konfidence.BaseData.ParameterObjects;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Konfidence.BaseData.Repositories
{
    public interface IDatabaseRepository
    {
        Database GetDatabase();

        IDataReader DataReader { get; }

        DbCommand GetStoredProcCommand(string saveStoredProcedure);

        int ExecuteNonQueryStoredProcedure(string saveStoredProcedure, DbParameterObjectList parameterObjectList);

        ResponseParameters ExecuteSaveStoredProcedure(RequestParameters executeParameters);

        void ExecuteGetStoredProcedure(RetrieveParameters retrieveParameters, Func<bool> callback);

        void ExecuteGetListStoredProcedure(RetrieveListParameters retrieveListParameters, Func<bool> callback);

        void ExecuteGetRelatedListStoredProcedure(RetrieveListParameters retrieveListParameters,
                                                  Func<bool> parentCallback, Func<bool> relatedCallback,
                                                  Func<bool> childCallback);

        void ExecuteDeleteStoredProcedure(string deleteStoredProcedure, string autoIdField, int id);

        int ExecuteNonQuery(string textCommand);

        int ExecuteNonQuery(DbCommand commandType);

        bool ObjectExists(string objectName, string collection);
    }
}