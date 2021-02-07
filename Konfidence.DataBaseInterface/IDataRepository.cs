using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Konfidence.DataBaseInterface
{
    public interface IDataRepository
    {
        DataTable GetSchemaObject(string collection);

        DbCommand GetStoredProcCommand(string saveStoredProcedure);

        int ExecuteNonQueryStoredProcedure(string saveStoredProcedure, List<ISpParameterData> parameterObjectList);

        void ExecuteSaveStoredProcedure(IBaseDataItem dataItem);

        void ExecuteGetStoredProcedure(IBaseDataItem dataItem);

        void ExecuteGetByStoredProcedure(IBaseDataItem dataItem, string storedProcedure);

        void ExecuteGetListStoredProcedure<T>(IBaseDataItemList<T> baseDataItemList, string storedProcedure, IBaseClient baseClient) where T : IBaseDataItem;

        void ExecuteGetRelatedListStoredProcedure<T>(string storedProcedure,
            IBaseDataItemList<T> parentDataItemList, IBaseDataItemList<T> relatedDataItemList,
            IBaseDataItemList<T> childDataItemList, IBaseClient baseClient) where T : IBaseDataItem;

        void ExecuteDeleteStoredProcedure(IBaseDataItem dataItem);

        int ExecuteNonQuery(string textCommand);

        bool ObjectExists(string objectName, string collection);
    }
}