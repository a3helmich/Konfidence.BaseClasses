using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Konfidence.BaseData
{
    internal interface IDatabaseRepository
    {
        Database GetDatabase();
        DbCommand GetStoredProcCommand(string saveStoredProcedure);
        DbCommand GetStoredProcCommand(string saveStoredProcedure, List<object> parameters);
        void SetParameterData(RequestParameters executeParameters, Database database, DbCommand dbCommand);
        int ExecuteNonQuery(DbCommand dbCommand);
        int ExecuteNonQuery(CommandType commandType, string textCommand);
        bool ObjectExists(string objectName, string collection);

        ResponseParameters GetParameterData(RequestParameters executeParameters, Database database, DbCommand dbCommand);
    }
}