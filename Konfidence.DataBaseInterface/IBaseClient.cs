using System.Collections.Generic;
using System.Data;
using JetBrains.Annotations;

namespace Konfidence.DatabaseInterface
{
    public interface IBaseClient
    {
        void Save(IBaseDataItem dataItem);

        void GetItem(IBaseDataItem dataItem);

        void GetItemBy(IBaseDataItem dataItem, [NotNull] string storedProcedure);

        void Delete(IBaseDataItem dataItem);

        [UsedImplicitly]
        int ExecuteCommand(string storedProcedure, List<ISpParameterData> parameterObjectList);

        int ExecuteTextCommand(string textCommand);

        [UsedImplicitly]
        bool TableExists(string tableName);

        [UsedImplicitly]
        bool ViewExists(string viewName);

        bool StoredProcedureExists(string storedPprocedureName);

        DataTable GetSchemaObject(string objectType);

        void BuildItemList<T>(IList<T> baseDataItemList, string getListStoredProcedure) where T : IBaseDataItem, new();

        void BuildItemList<T>(IList<T> baseDataItemList, string getListStoredProcedure, IList<ISpParameterData> spParameters) where T : IBaseDataItem, new();
    }
}
