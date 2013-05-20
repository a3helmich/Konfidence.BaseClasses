using System;
using System.Data;
using Konfidence.BaseData.IRepositories;
using Konfidence.BaseData.ParameterObjects;
using Ninject;
using Ninject.Parameters;

namespace Konfidence.BaseData
{
    internal class SqlHost : BaseHost
	{
        private readonly NinjectDependencyResolver _Ninject = new NinjectDependencyResolver();

        private readonly IDatabaseRepository _Repository;

        protected IKernel Kernel
        {
            get { return _Ninject.Kernel; }
        }

        public SqlHost(string dataBaseName) : base(string.Empty, dataBaseName)
		{
            var dataBaseNameParam = new ConstructorArgument("databaseName", dataBaseName);

            _Repository = Kernel.Get<IDatabaseRepository>(dataBaseNameParam);
		}

	    private IDataReader DataReader
	    {
	        get
	        {
	            return _Repository.DataReader;
	        }
	    }

		#region GetField Methods
		internal override Int16 GetFieldInt16(string fieldName)
		{
			var fieldOrdinal = GetOrdinal(fieldName);

			if (DataReader.IsDBNull(fieldOrdinal))
			{
				return 0;
			}

			return DataReader.GetInt16(fieldOrdinal);
		}

        internal override Int32 GetFieldInt32(string fieldName)
        {
            var fieldOrdinal = GetOrdinal(fieldName);

            if (DataReader.IsDBNull(fieldOrdinal))
            {
                return 0;
            }

            return DataReader.GetInt32(fieldOrdinal);
        }

        internal override Guid GetFieldGuid(string fieldName)
        {
            var fieldOrdinal = GetOrdinal(fieldName);

            if (DataReader.IsDBNull(fieldOrdinal))
            {
                return Guid.Empty;
            }

            return DataReader.GetGuid(fieldOrdinal);
        }

		internal override string GetFieldString(string fieldName)
		{
			var fieldOrdinal = GetOrdinal(fieldName);

            if (DataReader.IsDBNull(fieldOrdinal))
			{
				return string.Empty;
			}

            return DataReader.GetString(fieldOrdinal);
		}

		internal override bool GetFieldBool(string fieldName)
		{
			var fieldOrdinal = GetOrdinal(fieldName);

            if (DataReader.IsDBNull(fieldOrdinal))
			{
				return false;
			}

            return DataReader.GetBoolean(fieldOrdinal);
		}

		internal override DateTime GetFieldDateTime(string fieldName)
		{
			var fieldOrdinal = GetOrdinal(fieldName);

            if (DataReader.IsDBNull(fieldOrdinal))
			{
				return DateTime.MinValue;
			}

            return DataReader.GetDateTime(fieldOrdinal);
		}

        internal override TimeSpan GetFieldTimeSpan(string fieldName)
        {
            var fieldOrdinal = GetOrdinal(fieldName);

            if (DataReader.IsDBNull(fieldOrdinal))
            {
                return TimeSpan.MinValue;
            }

            return (TimeSpan)DataReader.GetValue(fieldOrdinal);
        }

        internal override Decimal GetFieldDecimal(string fieldName)
        {
            var fieldOrdinal = GetOrdinal(fieldName);

            if (DataReader.IsDBNull(fieldOrdinal))
            {
                return 0;
            }

            return DataReader.GetDecimal(fieldOrdinal);
        }

        private int GetOrdinal(string fieldName)
        {
            if (!IsAssigned(DataReader))
            {
                const string message = @"_DataReader: in SQLHost.GetOrdinal(string fieldName);";

                throw new ArgumentNullException(message);
            }

            return DataReader.GetOrdinal(fieldName);
        }
        #endregion

		internal override void Save(BaseDataItem dataItem)
		{
			if (dataItem.AutoIdField.Equals(string.Empty))
			{
				throw (new Exception("AutoIdField not provided"));
			}

            if (dataItem.SaveStoredProcedure.Equals(string.Empty))
			{
				throw (new Exception("StoredProcedure not provided"));
			}

            var resultParameters = _Repository.ExecuteSaveStoredProcedure(new RequestParameters(dataItem, dataItem.SaveStoredProcedure));

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

	    internal override void GetItem(BaseDataItem dataItem, string getStoredProcedure)
	    {
	        if (getStoredProcedure.Equals(string.Empty))
			{
				throw (new Exception("GetStoredProcedure not provided"));
			}

            var retrieveParameters = new RetrieveParameters(dataItem, getStoredProcedure);

            _Repository.ExecuteGetStoredProcedure(retrieveParameters, () =>
	            {
	                dataItem.GetKey();
	                dataItem.GetData();

	                return true;
	            });
        }

        internal override void BuildItemList(IBaseDataItemList baseDataItemList, string getListStoredProcedure)
        {
            if (getListStoredProcedure.Equals(string.Empty))
            {
                throw (new Exception("GetListStoredProcedure not provided"));
            }

            baseDataItemList.SetParameters(getListStoredProcedure);

            var retrieveListParameters = new RetrieveListParameters(baseDataItemList, getListStoredProcedure);

            _Repository.ExecuteGetListStoredProcedure(retrieveListParameters, () => GetDataItem(baseDataItemList));
        }


	    internal override void BuildItemList(IBaseDataItemList parentDataItemList, IBaseDataItemList relatedDataItemList, IBaseDataItemList childDataItemList, string getRelatedStoredProcedure)
	    {
            if (getRelatedStoredProcedure.Equals(string.Empty))
            {
                throw (new Exception("GetListStoredProcedure not provided"));
            }

            parentDataItemList.SetParameters(getRelatedStoredProcedure);

            var retrieveListParameters = new RetrieveListParameters(parentDataItemList, getRelatedStoredProcedure);

            _Repository.ExecuteGetRelatedListStoredProcedure(retrieveListParameters,
                () => GetDataItem(parentDataItemList),
                () => GetDataItem(relatedDataItemList),
                () => GetDataItem(childDataItemList));
        }

        private bool GetDataItem(IBaseDataItemList baseDataItemList)
        {
            var dataItem = baseDataItemList.GetDataItem();

            dataItem.DataHost = this;

            dataItem.GetKey();
            dataItem.GetData();

            return true;
        }

        internal protected override void Delete(string deleteStoredProcedure, string autoIdField, int id)
        {
            if (deleteStoredProcedure.Equals(string.Empty))
            {
                throw (new Exception("DeleteStoredProcedure not provided"));
            }

            if (id > 0)
            {
                _Repository.ExecuteDeleteStoredProcedure(deleteStoredProcedure, autoIdField, id);
            }
        }

        internal override int ExecuteCommand(string storedProcedure, DbParameterObjectList parameterObjectList)
        {
            return _Repository.ExecuteNonQueryStoredProcedure(storedProcedure, parameterObjectList);
        }

	    internal override int ExecuteTextCommand(string textCommand)
		{
            return _Repository.ExecuteNonQuery(textCommand);
		}

		internal override bool TableExists(string tableName)
		{
            return _Repository.ObjectExists(tableName, "Tables");
		}

		internal override bool ViewExists(string viewName)
		{
            return _Repository.ObjectExists(viewName, "Views");
		}

        internal override bool StoredProcedureExists(string storedProcedureName)
        {
            return _Repository.ObjectExists(storedProcedureName, "Procedures");
        }

        internal override DataTable GetSchemaObject(string collection)
        {
            DataTable dataTable;

            var database = _Repository.GetDatabase();

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
	}
}