using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Konfidence.BaseData.IRepositories
{
    internal interface IDatabaseRepository
    {
        Database GetDatabase();
        DbCommand GetStoredProcCommand(string saveStoredProcedure);
        DbCommand GetStoredProcCommand(string saveStoredProcedure, List<object> parameters);
        ResponseParameters ExecuteSaveStoredProcedure(RequestParameters executeParameters);
        int ExecuteNonQuery(string storedProcedure, List<object> parameterList);
        int ExecuteNonQuery(string textCommand);
        int ExecuteNonQuery(DbCommand commandType);
        bool ObjectExists(string objectName, string collection);
    }
}