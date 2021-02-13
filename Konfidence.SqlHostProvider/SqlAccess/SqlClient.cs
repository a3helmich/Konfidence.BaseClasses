using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.BaseData;
using Konfidence.DataBaseInterface;
using Ninject;
using Ninject.Parameters;

namespace Konfidence.SqlHostProvider.SqlAccess
{
    public class SqlClient : IBaseClient
    {
        private readonly NinjectDependencyResolver _ninject = new();

        private readonly IDataRepository _repository;

        public SqlClient(string connectionName) 
        {
            var connectionNameParam = new ConstructorArgument("connectionName", connectionName);

            if (!_ninject.Kernel.GetBindings(typeof(IDataRepository)).Any())
            {
                _ninject.Bind<IDataRepository>().To<SqlClientRepository>();
            }

            _repository = _ninject.Kernel.Get<IDataRepository>(connectionNameParam);
        }

        public void Save([NotNull] IBaseDataItem dataItem)
        {
            if (dataItem.AutoIdField.Equals(string.Empty))
            {
                throw new Exception("AutoIdField not provided");
            }

            if (dataItem.SaveStoredProcedure.Equals(string.Empty))
            {
                throw new Exception("StoredProcedure not provided");
            }

            _repository.ExecuteSaveStoredProcedure(dataItem);
        }

        public void GetItem([NotNull] IBaseDataItem dataItem)
        {
            if (!dataItem.GetStoredProcedure.IsAssigned())
            {
                throw new Exception("GetStoredProcedure not provided");
            }

            _repository.ExecuteGetStoredProcedure(dataItem);
        }

        public void GetItemBy([NotNull] IBaseDataItem dataItem, string storedProcedure)
        {
            if (!storedProcedure.IsAssigned())
            {
                throw new Exception("GetStoredProcedure not provided");
            }

            _repository.ExecuteGetByStoredProcedure(dataItem, storedProcedure);
        }

        public void BuildItemList<T>([NotNull] IList<T> baseDataItemList, [NotNull] string getListStoredProcedure) where T : IBaseDataItem, new()
        {
            if (!getListStoredProcedure.IsAssigned())
            {
                throw new Exception("GetListStoredProcedure not provided");
            }

            _repository.ExecuteGetListStoredProcedure(baseDataItemList, getListStoredProcedure, this);
        }

        public void BuildItemList<T>([NotNull] IList<T> baseDataItemList, [NotNull] string getListStoredProcedure, IList<ISpParameterData> spParameters) where T : IBaseDataItem, new()
        {
            if (!getListStoredProcedure.IsAssigned())
            {
                throw new Exception("GetListStoredProcedure not provided");
            }

            _repository.ExecuteGetListStoredProcedure(baseDataItemList, getListStoredProcedure, spParameters, this);
        }

        public void Delete([NotNull] IBaseDataItem dataItem)
        {
            if (!dataItem.DeleteStoredProcedure.IsAssigned())
            {
                throw new Exception("DeleteStoredProcedure not provided");
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