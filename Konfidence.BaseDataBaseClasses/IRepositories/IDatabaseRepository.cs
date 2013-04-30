using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Konfidence.BaseData.ParameterObjects;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Konfidence.BaseData.IRepositories
{
    internal interface IDatabaseRepository
    {
        Database GetDatabase();
        IDataReader DataReader { get; }
        DbCommand GetStoredProcCommand(string saveStoredProcedure);
        DbCommand GetStoredProcCommand(string saveStoredProcedure, List<object> parameters);
        ResponseParameters ExecuteSaveStoredProcedure(RequestParameters executeParameters);
        void ExecuteGetStoredProcedure(RetrieveParameters retrieveParameters, Func<bool> callback);
        int ExecuteNonQuery(string storedProcedure, List<object> parameterList);
        int ExecuteNonQuery(string textCommand);
        int ExecuteNonQuery(DbCommand commandType);
        bool ObjectExists(string objectName, string collection);
    }
}