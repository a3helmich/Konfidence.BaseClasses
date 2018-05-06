using System;
using System.Data;

namespace Konfidence.BaseDataInterfaces
{
    public interface IBaseClient
    {
        void Save(IBaseDataItem dataItem);

        void GetItem(IBaseDataItem dataItem, string getStoredProcedure);

        void Delete(string deleteStoredProcedure, string autoIdField, int id);

        int ExecuteCommand(string storedProcedure, IDbParameterObjectList parameterObjectList);

        int ExecuteTextCommand(string textCommand);

        bool TableExists(string tableName);

        bool ViewExists(string viewName);

        bool StoredProcedureExists(string storedPprocedureName);

        short GetFieldInt16(string fieldName);

        int GetFieldInt32(string fieldName);

        int GetFieldInt64(string fieldName);

        Guid GetFieldGuid(string fieldName);

        string GetFieldString(string fieldName);

        bool GetFieldBool(string fieldName);

        DateTime GetFieldDateTime(string fieldName);

        TimeSpan GetFieldTimeSpan(string fieldName);

        decimal GetFieldDecimal(string fieldName);

        DataTable GetSchemaObject(string objectType);

        void BuildItemList<T>(IBaseDataItemList<T> baseDataItemList, string getListStoredProcedure) where T : IBaseDataItem;

        void BuildItemList<T>(IBaseDataItemList<T> parentDataItemList, IBaseDataItemList<T> relatedDataItemList, IBaseDataItemList<T> childDataItemList, string getRelatedStoredProcedure) where T : IBaseDataItem;
    }
}
