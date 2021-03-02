using System;
using System.Collections.Generic;
using System.Data;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.DataBaseInterface;

namespace Konfidence.SqlHostProvider.SqlAccess
{
    internal class SqlClient : IBaseClient
    {
        private readonly IDataRepository _repository;

        public SqlClient([NotNull] IDataRepository sqlClientRepository) 
        {
            _repository = sqlClientRepository;
        }

        public void Save([NotNull] IBaseDataItem dataItem)
        {
            if (dataItem.AutoIdField.Equals(string.Empty))
            {
                throw new Exception("AutoIdField not generated");
            }

            if (dataItem.SaveStoredProcedure.Equals(string.Empty))
            {
                throw new Exception("SaveStoredProcedure not generated");
            }

            _repository.ExecuteSaveStoredProcedure(dataItem);
        }

        public void GetItem([NotNull] IBaseDataItem dataItem)
        {
            if (!dataItem.GetStoredProcedure.IsAssigned())
            {
                throw new Exception("GetStoredProcedure not generated");
            }

            _repository.ExecuteGetStoredProcedure(dataItem);
        }

        public void GetItemBy([NotNull] IBaseDataItem dataItem, string storedProcedure)
        {
            if (!storedProcedure.IsAssigned())
            {
                throw new Exception("GetStoredProcedure not generetad");
            }

            _repository.ExecuteGetByStoredProcedure(dataItem, storedProcedure);
        }

        public void BuildItemList<T>([NotNull] IList<T> baseDataItemList, [NotNull] string getListStoredProcedure) where T : IBaseDataItem, new()
        {
            if (!getListStoredProcedure.IsAssigned())
            {
                throw new Exception("GetListStoredProcedure not generated");
            }

            _repository.ExecuteGetListStoredProcedure(baseDataItemList, getListStoredProcedure, this);
        }

        public void BuildItemList<T>([NotNull] IList<T> baseDataItemList, [NotNull] string getListStoredProcedure, IList<ISpParameterData> spParameters) where T : IBaseDataItem, new()
        {
            if (!getListStoredProcedure.IsAssigned())
            {
                throw new Exception("GetListStoredProcedure not generated");
            }

            _repository.ExecuteGetListStoredProcedure(baseDataItemList, getListStoredProcedure, spParameters, this);
        }

        public void Delete([NotNull] IBaseDataItem dataItem)
        {
            if (!dataItem.DeleteStoredProcedure.IsAssigned())
            {
                throw new Exception("DeleteStoredProcedure not generated");
            }

            _repository.ExecuteDeleteStoredProcedure(dataItem);
        }

        public int ExecuteCommand(string storedProcedure, List<ISpParameterData> parameterObjectList)
        {
            return _repository.ExecuteCommandStoredProcedure(storedProcedure, parameterObjectList);
        }

        public int ExecuteTextCommand(string textCommand)
        {
            return _repository.ExecuteTextCommandQuery(textCommand);
        }

        public bool TableExists(string tableName)
        {
            return _repository.ObjectExists(tableName, "Tables");
        }

        public bool ViewExists(string viewName)
        {
            return _repository.ObjectExists(viewName, "Views");
        }

        public bool StoredProcedureExists(string storedProcedureName)
        {
            return _repository.ObjectExists(storedProcedureName, "Procedures");
        }

        [NotNull]
        public DataTable GetSchemaObject(string collection)
        {
            return _repository.GetSchemaObject(collection);
        }
    }
}