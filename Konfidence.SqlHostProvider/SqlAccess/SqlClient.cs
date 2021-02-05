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

        private static int GetOrdinal([NotNull] string fieldName, [NotNull] IDataReader dataReader)
        {
            if (!dataReader.IsAssigned())
            {
                const string message = @"_DataReader: in SQLClient.GetOrdinal(string fieldName);";

                throw new ArgumentNullException(message);
            }

            return dataReader.GetOrdinal(fieldName);
        }

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

            _repository.ExecuteSaveStoredProcedure(dataItem);
        }

        public void GetItem([NotNull] IBaseDataItem dataItem)
        {
            if (!dataItem.GetStoredProcedure.IsAssigned())
            {
                throw (new Exception("GetStoredProcedure not provided"));
            }

            _repository.ExecuteGetStoredProcedure(dataItem);
        }

        public void GetItemBy([NotNull] IBaseDataItem dataItem, string storedProcedure)
        {
            if (!storedProcedure.IsAssigned())
            {
                throw (new Exception("GetStoredProcedure not provided"));
            }

            _repository.ExecuteGetByStoredProcedure(dataItem, storedProcedure);
        }

        public void BuildItemList<T>([NotNull] IBaseDataItemList<T> baseDataItemList, [NotNull] string getListStoredProcedure) where T : IBaseDataItem
        {
            if (!getListStoredProcedure.IsAssigned())
            {
                throw (new Exception("GetListStoredProcedure not provided"));
            }

            baseDataItemList.SetParameters(getListStoredProcedure);

            _repository.ExecuteGetListStoredProcedure(baseDataItemList, getListStoredProcedure, this);
        }

        public void BuildItemList<T>([NotNull] IBaseDataItemList<T> parentDataItemList, IBaseDataItemList<T> relatedDataItemList,
            IBaseDataItemList<T> childDataItemList, [NotNull] string getRelatedStoredProcedure) where T : IBaseDataItem
        {
            if (getRelatedStoredProcedure.Equals(string.Empty))
            {
                throw (new Exception("GetListStoredProcedure not provided"));
            }

            parentDataItemList.SetParameters(getRelatedStoredProcedure);

            _repository.ExecuteGetRelatedListStoredProcedure(getRelatedStoredProcedure, parentDataItemList, relatedDataItemList, childDataItemList, this);
        }

        public void Delete([NotNull] IBaseDataItem dataItem)
        {
            if (!dataItem.DeleteStoredProcedure.IsAssigned())
            {
                throw new Exception("DeleteStoredProcedure not provided");
            }

            _repository.ExecuteDeleteStoredProcedure(dataItem);
        }

        public int ExecuteCommand(string storedProcedure, List<IDbParameterData> parameterObjectList)
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

        [NotNull]
        public DataTable GetSchemaObject(string collection)
        {
            return _repository.GetSchemaObject(collection);
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