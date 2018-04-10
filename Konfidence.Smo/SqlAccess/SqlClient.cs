//using System;
//using System.Data;
//using Konfidence.Base;
//using Konfidence.BaseData;
//using Konfidence.BaseDataInterfaces;
//using Konfidence.RepositoryInterface;
//using Konfidence.RepositoryInterface.Objects;
//using Ninject;
//using Ninject.Parameters;

//namespace Konfidence.SqlHostProvider.SqlAccess
//{
//    internal class SqlClient : BaseClient
//    {
//        private readonly NinjectDependencyResolver _ninject = new NinjectDependencyResolver();

//        private readonly IDataRepository _repository;

//        protected IKernel Kernel => _ninject.Kernel;

//        public SqlClient(string databaseName) : base(string.Empty, databaseName)
//        {
//            var databaseNameParam = new ConstructorArgument("databaseName", databaseName);

//            _repository = Kernel.Get<IDataRepository>(databaseNameParam);
//        }

//        private IDataReader DataReader => _repository.DataReader;

//        #region GetField Methods
//        public override Int16 GetFieldInt16(string fieldName)
//        {
//            var fieldOrdinal = GetOrdinal(fieldName);

//            if (DataReader.IsDBNull(fieldOrdinal))
//            {
//                return 0;
//            }

//            return DataReader.GetInt16(fieldOrdinal);
//        }

//        public override Int32 GetFieldInt32(string fieldName)
//        {
//            var fieldOrdinal = GetOrdinal(fieldName);

//            if (DataReader.IsDBNull(fieldOrdinal))
//            {
//                return 0;
//            }

//            return DataReader.GetInt32(fieldOrdinal);
//        }

//        public override Guid GetFieldGuid(string fieldName)
//        {
//            var fieldOrdinal = GetOrdinal(fieldName);

//            if (DataReader.IsDBNull(fieldOrdinal))
//            {
//                return Guid.Empty;
//            }

//            return DataReader.GetGuid(fieldOrdinal);
//        }

//        public override string GetFieldString(string fieldName)
//        {
//            var fieldOrdinal = GetOrdinal(fieldName);

//            if (DataReader.IsDBNull(fieldOrdinal))
//            {
//                return string.Empty;
//            }

//            return DataReader.GetString(fieldOrdinal);
//        }

//        public override bool GetFieldBool(string fieldName)
//        {
//            var fieldOrdinal = GetOrdinal(fieldName);

//            if (DataReader.IsDBNull(fieldOrdinal))
//            {
//                return false;
//            }

//            return DataReader.GetBoolean(fieldOrdinal);
//        }

//        public override DateTime GetFieldDateTime(string fieldName)
//        {
//            var fieldOrdinal = GetOrdinal(fieldName);

//            if (DataReader.IsDBNull(fieldOrdinal))
//            {
//                return DateTime.MinValue;
//            }

//            return DataReader.GetDateTime(fieldOrdinal);
//        }

//        public override TimeSpan GetFieldTimeSpan(string fieldName)
//        {
//            var fieldOrdinal = GetOrdinal(fieldName);

//            if (DataReader.IsDBNull(fieldOrdinal))
//            {
//                return TimeSpan.MinValue;
//            }

//            return (TimeSpan)DataReader.GetValue(fieldOrdinal);
//        }

//        public override Decimal GetFieldDecimal(string fieldName)
//        {
//            var fieldOrdinal = GetOrdinal(fieldName);

//            if (DataReader.IsDBNull(fieldOrdinal))
//            {
//                return 0;
//            }

//            return DataReader.GetDecimal(fieldOrdinal);
//        }

//        private int GetOrdinal(string fieldName)
//        {
//            if (!DataReader.IsAssigned())
//            {
//                const string message = @"_DataReader: in SQLClient.GetOrdinal(string fieldName);";

//                throw new ArgumentNullException(message);
//            }

//            return DataReader.GetOrdinal(fieldName);
//        }
//        #endregion

//        public override void Save(IBaseDataItem dataItem)
//        {
//            if (dataItem.AutoIdField.Equals(string.Empty))
//            {
//                throw (new Exception("AutoIdField not provided"));
//            }

//            if (dataItem.SaveStoredProcedure.Equals(string.Empty))
//            {
//                throw (new Exception("StoredProcedure not provided"));
//            }

//            var resultParameters = _repository.ExecuteSaveStoredProcedure(new RequestParameters(dataItem, dataItem.SaveStoredProcedure));

//            dataItem.SetId(resultParameters.Id);

//            foreach (var kvp in dataItem.AutoUpdateFieldDictionary)
//            {
//                kvp.Value.Value = resultParameters.AutoUpdateFieldList[kvp.Key].Value;

//                if (DBNull.Value.Equals(kvp.Value.Value))
//                {
//                    kvp.Value.Value = null;
//                }
//            }
//        }

//        public override void GetItem(IBaseDataItem dataItem, string getStoredProcedure)
//        {
//            if (getStoredProcedure.Equals(string.Empty))
//            {
//                throw (new Exception("GetStoredProcedure not provided"));
//            }

//            var retrieveParameters = new RetrieveParameters(dataItem, getStoredProcedure);

//            _repository.ExecuteGetStoredProcedure(retrieveParameters, () =>
//                {
//                    dataItem.GetKey();
//                    dataItem.GetData();

//                    return true;
//                });
//        }

//        internal override void BuildItemList<T>(IBaseDataItemList<T> baseDataItemList, string getListStoredProcedure)
//        {
//            if (getListStoredProcedure.Equals(string.Empty))
//            {
//                throw (new Exception("GetListStoredProcedure not provided"));
//            }

//            baseDataItemList.SetParameters(getListStoredProcedure);

//            var retrieveListParameters = new RetrieveListParameters<T>(baseDataItemList, getListStoredProcedure);

//            _repository.ExecuteGetListStoredProcedure(retrieveListParameters, () => GetDataItem(baseDataItemList));
//        }


//        internal override void BuildItemList<T>(IBaseDataItemList<T> parentDataItemList, IBaseDataItemList<T> relatedDataItemList,
//            IBaseDataItemList<T> childDataItemList, string getRelatedStoredProcedure)
//        {
//            if (getRelatedStoredProcedure.Equals(string.Empty))
//            {
//                throw (new Exception("GetListStoredProcedure not provided"));
//            }

//            parentDataItemList.SetParameters(getRelatedStoredProcedure);

//            var retrieveListParameters = new RetrieveListParameters<T>(parentDataItemList, getRelatedStoredProcedure);

//            _repository.ExecuteGetRelatedListStoredProcedure(retrieveListParameters,
//                () => GetDataItem(parentDataItemList),
//                () => GetDataItem(relatedDataItemList),
//                () => GetDataItem(childDataItemList));
//        }

//        private bool GetDataItem<T>(IBaseDataItemList<T> baseDataItemList) where T : IBaseDataItem
//        {
//            var dataItem = baseDataItemList.GetDataItem();

//            dataItem.Client = this;

//            dataItem.GetKey();
//            dataItem.GetData();

//            return true;
//        }

//        public override void Delete(string deleteStoredProcedure, string autoIdField, int id)
//        {
//            if (deleteStoredProcedure.Equals(string.Empty))
//            {
//                throw (new Exception("DeleteStoredProcedure not provided"));
//            }

//            if (id > 0)
//            {
//                _repository.ExecuteDeleteStoredProcedure(deleteStoredProcedure, autoIdField, id);
//            }
//        }

//        public override int ExecuteCommand(string storedProcedure, IDbParameterObjectList parameterObjectList)
//        {
//            return _repository.ExecuteNonQueryStoredProcedure(storedProcedure, parameterObjectList);
//        }

//        public override int ExecuteTextCommand(string textCommand)
//        {
//            return _repository.ExecuteNonQuery(textCommand);
//        }

//        public override bool TableExists(string tableName)
//        {
//            return _repository.ObjectExists(tableName, "Tables");
//        }

//        public override bool ViewExists(string viewName)
//        {
//            return _repository.ObjectExists(viewName, "Views");
//        }

//        public override bool StoredProcedureExists(string storedProcedureName)
//        {
//            return _repository.ObjectExists(storedProcedureName, "Procedures");
//        }

//        // TODO: internal
//        public override DataTable GetSchemaObject(string collection)
//        {
//            DataTable dataTable;

//            var database = _repository.GetDatabase();

//            using (var dbConnection = database.CreateConnection())
//            {
//                dbConnection.Open();

//                using (var schemaTable = dbConnection.GetSchema(collection))
//                {
//                    dataTable = schemaTable.Copy();
//                }
//            }

//            return dataTable;
//        }
//    }
//}