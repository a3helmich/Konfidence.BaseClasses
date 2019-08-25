using System;
using System.Collections.Generic;
using Konfidence.BaseData.Objects;
using Konfidence.BaseDataInterfaces;

namespace Konfidence.RestProvider
{
    internal class RestClientMock : IRestClientMock
    {
        public string Url { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<DbParameterObjectList> BuildItemList()
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public int ExecuteTextCommand(string textCommand)
        {
            throw new NotImplementedException();
        }

        public int Save(IDbParameterObject[] dbParameterObject, int v)
        {
            throw new NotImplementedException();
        }

        public bool TableExists(string tableName)
        {
            throw new NotImplementedException();
        }

        public bool ViewExists(string viewName)
        {
            throw new NotImplementedException();
        }
    }
}