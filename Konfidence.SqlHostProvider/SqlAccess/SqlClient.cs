using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.BaseData;
using Konfidence.DataBaseInterface;
using Konfidence.RepositoryInterface;
using Konfidence.RepositoryInterface.Objects;
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

        #region GetField Methods

        public void GetField([NotNull] string fieldName, out byte field, [NotNull] IDataReader dataReader)
        {
            var fieldOrdinal = GetOrdinal(fieldName, dataReader);

            field = dataReader.IsDBNull(fieldOrdinal) ? 0 : dataReader.GetByte(fieldOrdinal);
        }

        public byte GetFieldInt8([NotNull] string fieldName, [NotNull] IDataReader dataReader)
        {
            var fieldOrdinal = GetOrdinal(fieldName, dataReader);

            return dataReader.IsDBNull(fieldOrdinal) ? (byte) 0 : dataReader.GetByte(fieldOrdinal);
        }

        public short GetFieldInt16([NotNull] string fieldName, [NotNull] IDataReader dataReader)
        {
            var fieldOrdinal = GetOrdinal(fieldName, dataReader);

            return dataReader.IsDBNull(fieldOrdinal) ? (short)0 : dataReader.GetInt16(fieldOrdinal);
        }

        public int GetFieldInt32([NotNull] string fieldName, [NotNull] IDataReader dataReader)
        {
            var fieldOrdinal = GetOrdinal(fieldName, dataReader);

            return dataReader.IsDBNull(fieldOrdinal) ? 0 : dataReader.GetInt32(fieldOrdinal);
        }

        public long GetFieldInt64([NotNull] string fieldName, [NotNull] IDataReader dataReader)
        {
            var fieldOrdinal = GetOrdinal(fieldName, dataReader);

            return dataReader.IsDBNull(fieldOrdinal) ? 0 : dataReader.GetInt64(fieldOrdinal);
        }

        public Guid GetFieldGuid([NotNull] string fieldName, [NotNull] IDataReader dataReader)
        {
            var fieldOrdinal = GetOrdinal(fieldName, dataReader);

            return dataReader.IsDBNull(fieldOrdinal) ? Guid.Empty : dataReader.GetGuid(fieldOrdinal);
        }

        public string GetFieldString([NotNull] string fieldName, [NotNull] IDataReader dataReader)
        {
            var fieldOrdinal = GetOrdinal(fieldName, dataReader);

            return dataReader.IsDBNull(fieldOrdinal) ? string.Empty : dataReader.GetString(fieldOrdinal);
        }

        public bool GetFieldBool([NotNull] string fieldName, [NotNull] IDataReader dataReader)
        {
            var fieldOrdinal = GetOrdinal(fieldName, dataReader);

            return !dataReader.IsDBNull(fieldOrdinal) && dataReader.GetBoolean(fieldOrdinal);
        }

        public DateTime GetFieldDateTime([NotNull] string fieldName, [NotNull] IDataReader dataReader)
        {
            var fieldOrdinal = GetOrdinal(fieldName, dataReader);

            return dataReader.IsDBNull(fieldOrdinal) ? DateTime.MinValue : dataReader.GetDateTime(fieldOrdinal);
        }

        public TimeSpan GetFieldTimeSpan([NotNull] string fieldName, [NotNull] IDataReader dataReader)
        {
            var fieldOrdinal = GetOrdinal(fieldName, dataReader);

            return dataReader.IsDBNull(fieldOrdinal) ? TimeSpan.MinValue : (TimeSpan)dataReader.GetValue(fieldOrdinal);
        }

        public decimal GetFieldDecimal([NotNull] string fieldName, [NotNull] IDataReader dataReader)
        {
            var fieldOrdinal = GetOrdinal(fieldName, dataReader);

            return dataReader.IsDBNull(fieldOrdinal) ? 0 : dataReader.GetDecimal(fieldOrdinal);
        }

        private int GetOrdinal([NotNull] string fieldName, [NotNull] IDataReader dataReader)
        {
            if (!dataReader.IsAssigned())
            {
                const string message = @"_DataReader: in SQLClient.GetOrdinal(string fieldName);";

                throw new ArgumentNullException(message);
            }

            return dataReader.GetOrdinal(fieldName);
        }
        #endregion

        public void Save([NotNull] IBaseDataItem dataItem)
        {
            if (dataItem.AutoIdField.Equals(string.Empty))
            {
                throw (new Exception("AutoIdField not provided"));
            }

            if (dataItem.SaveStoredProcedure.Equals(string.Empty))
            {
                throw (new Exception("StoredProcedure not provided"));
            }

            var resultParameters = _repository.ExecuteSaveStoredProcedure(new RequestParameters(dataItem, dataItem.SaveStoredProcedure));

            dataItem.SetId(resultParameters.Id);

            foreach (var kvp in dataItem.AutoUpdateFieldDictionary)
            {
                kvp.Value.Value = resultParameters.AutoUpdateFieldList[kvp.Key].Value;

                if (DBNull.Value.Equals(kvp.Value.Value))
                {
                    kvp.Value.Value = null;
                }
            }
        }

        public void GetItem(IBaseDataItem dataItem, [NotNull] string getStoredProcedure)
        {
            if (!getStoredProcedure.IsAssigned())
            {
                throw (new Exception("GetStoredProcedure not provided"));
            }

            var retrieveParameters = new RetrieveParameters(dataItem, getStoredProcedure);

            _repository.ExecuteGetStoredProcedure(retrieveParameters, dataItem);
        }

        public void BuildItemList<T>([NotNull] IBaseDataItemList<T> baseDataItemList, [NotNull] string getListStoredProcedure) where T : IBaseDataItem
        {
            if (!getListStoredProcedure.IsAssigned())
            {
                throw (new Exception("GetListStoredProcedure not provided"));
            }

            baseDataItemList.SetParameters(getListStoredProcedure);

            var retrieveListParameters = new RetrieveListParameters<T>(baseDataItemList, getListStoredProcedure);

            _repository.ExecuteGetListStoredProcedure(retrieveListParameters, baseDataItemList, this);
        }

        public void BuildItemList<T>([NotNull] IBaseDataItemList<T> parentDataItemList, IBaseDataItemList<T> relatedDataItemList,
            IBaseDataItemList<T> childDataItemList, [NotNull] string getRelatedStoredProcedure) where T : IBaseDataItem
        {
            if (getRelatedStoredProcedure.Equals(string.Empty))
            {
                throw (new Exception("GetListStoredProcedure not provided"));
            }

            parentDataItemList.SetParameters(getRelatedStoredProcedure);

            var retrieveListParameters = new RetrieveListParameters<T>(parentDataItemList, getRelatedStoredProcedure);

            _repository.ExecuteGetRelatedListStoredProcedure(retrieveListParameters, parentDataItemList, relatedDataItemList, childDataItemList, this);
        }

        private bool GetDataItem<T>([NotNull] IBaseDataItemList<T> baseDataItemList, IDataReader dataReader) where T : IBaseDataItem
        {
            var dataItem = baseDataItemList.GetDataItem();

            dataItem.Client = this;

            dataItem.GetKey(dataReader);
            dataItem.GetData(dataReader);

            return true;
        }

        public void Delete([NotNull] string deleteStoredProcedure, string autoIdField, int id)
        {
            if (deleteStoredProcedure.Equals(string.Empty))
            {
                throw new Exception("DeleteStoredProcedure not provided");
            }

            if (id > 0)
            {
                _repository.ExecuteDeleteStoredProcedure(deleteStoredProcedure, autoIdField, id);
            }
        }

        public int ExecuteCommand(string storedProcedure, List<IDbParameterObject> parameterObjectList)
        {
            return _repository.ExecuteNonQueryStoredProcedure(storedProcedure, parameterObjectList);
        }

        public int ExecuteTextCommand(string textCommand)
        {
            return _repository.ExecuteNonQuery(textCommand);
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

        // TODO: internal
        [NotNull]
        public DataTable GetSchemaObject(string collection)
        {
            DataTable dataTable;

            var database = _repository.GetDatabase();

            using (var dbConnection = database.CreateConnection())
            {
                dbConnection.Open();

                using (var schemaTable = dbConnection.GetSchema(collection))
                {
                    dataTable = schemaTable.Copy();
                }
            }

            return dataTable;
        }

        [NotNull]
        public DataTable GetIndexedColumns()
        {
            return GetSchemaObject("IndexColumns");
        }

        [NotNull]
        public DataTable GetTables()
        {
            return GetSchemaObject("Tables");
        }
    }
}