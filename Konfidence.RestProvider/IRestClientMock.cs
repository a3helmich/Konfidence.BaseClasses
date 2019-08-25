using System.Collections.Generic;
using Konfidence.BaseData.Objects;
using Konfidence.BaseDataInterfaces;

namespace Konfidence.RestProvider
{
    internal interface IRestClientMock
    {
        string Url { get; set; }

        int Save(IDbParameterObject[] dbParameterObject, int v);
        void Delete(int id);
        List<DbParameterObjectList> BuildItemList();
        int ExecuteTextCommand(string textCommand);
        bool TableExists(string tableName);
        bool ViewExists(string viewName);
    }
}