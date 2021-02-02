using System;
using System.Collections.Generic;
using System.Data;

namespace Konfidence.DataBaseInterface
{
    public interface IBaseClient
    {
        void Save(IBaseDataItem dataItem);

        void GetItem(IBaseDataItem dataItem, string getStoredProcedure);

        void Delete(string deleteStoredProcedure, string autoIdField, int id);

        int ExecuteCommand(string storedProcedure, List<IDbParameterObject> parameterObjectList);

        int ExecuteTextCommand(string textCommand);

        bool TableExists(string tableName);

        bool ViewExists(string viewName);

        bool StoredProcedureExists(string storedPprocedureName);

        byte GetFieldInt8(string fieldName, IDataReader dataReader);

        short GetFieldInt16(string fieldName, IDataReader dataReader);

        int GetFieldInt32(string fieldName, IDataReader dataReader);

        long GetFieldInt64(string fieldName, IDataReader dataReader);

        Guid GetFieldGuid(string fieldName, IDataReader dataReader);

        string GetFieldString(string fieldName, IDataReader dataReader);

        bool GetFieldBool(string fieldName, IDataReader dataReader);

        DateTime GetFieldDateTime(string fieldName, IDataReader dataReader);

        TimeSpan GetFieldTimeSpan(string fieldName, IDataReader dataReader);

        decimal GetFieldDecimal(string fieldName, IDataReader dataReader);

        DataTable GetSchemaObject(string objectType);

        DataTable GetIndexedColumns();

        DataTable GetTables();

        void BuildItemList<T>(IBaseDataItemList<T> baseDataItemList, string getListStoredProcedure) where T : IBaseDataItem;

        void BuildItemList<T>(IBaseDataItemList<T> parentDataItemList, IBaseDataItemList<T> relatedDataItemList, IBaseDataItemList<T> childDataItemList, string getRelatedStoredProcedure) where T : IBaseDataItem;
    }
}
