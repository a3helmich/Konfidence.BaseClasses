using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Konfidence.DataBaseInterface
{
    internal interface IDataRepository
    {
        DataTable GetSchemaObject(string collection);

        //DbCommand GetStoredProcCommand(string saveStoredProcedure);

        int ExecuteCommandStoredProcedure(string saveStoredProcedure, List<ISpParameterData> parameterObjectList);

        void ExecuteSaveStoredProcedure(IBaseDataItem dataItem);

        void ExecuteGetStoredProcedure(IBaseDataItem dataItem);

        void ExecuteGetByStoredProcedure(IBaseDataItem dataItem, string storedProcedure);

        void ExecuteGetListStoredProcedure<T>(IList<T> baseDataItemList, string storedProcedure, IBaseClient baseClient) where T : IBaseDataItem, new();

        void ExecuteGetListStoredProcedure<T>(IList<T> baseDataItemList, string storedProcedure, IList<ISpParameterData> spParameters, IBaseClient baseClient) where T : IBaseDataItem, new();

        void ExecuteDeleteStoredProcedure(IBaseDataItem dataItem);

        int ExecuteTextCommandQuery(string textCommand);

        bool ObjectExists(string objectName, string collection);
    }
}