using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Konfidence.BaseData
{
	internal class SqlHost : BaseHost
	{
        private IDataReader _DataReader;
	    private readonly IDatabaseRepository _DatabaseRepository;

		public SqlHost(string dataBaseName): base(string.Empty, dataBaseName)
		{
            // TODO : figure out if the Host is properly configured
            //        and if all resources are avalable

            _DatabaseRepository = new DatabaseRepository(dataBaseName);
		}

		#region GetField Methods
		internal override Int16 GetFieldInt16(string fieldName)
		{
			var fieldOrdinal = GetOrdinal(fieldName);

			if (_DataReader.IsDBNull(fieldOrdinal))
			{
				return 0;
			}

			return _DataReader.GetInt16(fieldOrdinal);
		}

        internal override Int32 GetFieldInt32(string fieldName)
        {
            var fieldOrdinal = GetOrdinal(fieldName);

            if (_DataReader.IsDBNull(fieldOrdinal))
            {
                return 0;
            }

            return _DataReader.GetInt32(fieldOrdinal);
        }

        internal override Guid GetFieldGuid(string fieldName)
        {
            var fieldOrdinal = GetOrdinal(fieldName);

            if (_DataReader.IsDBNull(fieldOrdinal))
            {
                return Guid.Empty;
            }

            return _DataReader.GetGuid(fieldOrdinal);
        }

		internal override string GetFieldString(string fieldName)
		{
			var fieldOrdinal = GetOrdinal(fieldName);

			if (_DataReader.IsDBNull(fieldOrdinal))
			{
				return string.Empty;
			}

			return _DataReader.GetString(fieldOrdinal);
		}

		internal override bool GetFieldBool(string fieldName)
		{
			var fieldOrdinal = GetOrdinal(fieldName);

			if (_DataReader.IsDBNull(fieldOrdinal))
			{
				return false;
			}

			return _DataReader.GetBoolean(fieldOrdinal);
		}

		internal override DateTime GetFieldDateTime(string fieldName)
		{
			var fieldOrdinal = GetOrdinal(fieldName);

			if (_DataReader.IsDBNull(fieldOrdinal))
			{
				return DateTime.MinValue;
			}

			return _DataReader.GetDateTime(fieldOrdinal);
		}

        internal override TimeSpan GetFieldTimeSpan(string fieldName)
        {
            var fieldOrdinal = GetOrdinal(fieldName);

            if (_DataReader.IsDBNull(fieldOrdinal))
            {
                return TimeSpan.MinValue;
            }

            return (TimeSpan)_DataReader.GetValue(fieldOrdinal);
        }

        internal override Decimal GetFieldDecimal(string fieldName)
        {
            var fieldOrdinal = GetOrdinal(fieldName);

            if (_DataReader.IsDBNull(fieldOrdinal))
            {
                return 0;
            }

            return _DataReader.GetDecimal(fieldOrdinal);
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
				throw (new Exception("SaveStoredProcedure not provided"));
			}

		    var executeParameters = new RequestParameters(dataItem);
		    ResponseParameters resultParameters;

			var database = _DatabaseRepository.GetDatabase();

            using (var dbCommand = _DatabaseRepository.GetStoredProcCommand(dataItem.SaveStoredProcedure))
            {
                _DatabaseRepository.SetParameterData(executeParameters, database, dbCommand);

                _DatabaseRepository.ExecuteNonQuery(dbCommand);

                resultParameters = _DatabaseRepository.GetParameterData(executeParameters, database, dbCommand);
            }

		    dataItem.SetKey(resultParameters.Id);

            foreach (var kvp in dataItem.AutoUpdateFieldList)
            {
                kvp.Value.Value = resultParameters.AutoUpdateFieldList[kvp.Key];

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

            var database = _DatabaseRepository.GetDatabase();

            using (var dbCommand = _DatabaseRepository.GetStoredProcCommand(getStoredProcedure))
			{
                dataItem.SetParameters(getStoredProcedure, database, dbCommand);

				using (var dataReader = database.ExecuteReader(dbCommand))
				{
					if (dataReader.Read())
					{
						_DataReader = dataReader;

                        dataItem.GetKey();

						dataItem.GetData();

						_DataReader = null;
					}
				}
			}
		}

		internal override void Delete(string deleteStoredProcedure, string autoIdField, int id)
		{
			if (deleteStoredProcedure.Equals(string.Empty))
			{
				throw (new Exception("DeleteStoredProcedure not provided"));
			}

			if (id > 0)
			{
                var database = _DatabaseRepository.GetDatabase();

                using (var dbCommand = _DatabaseRepository.GetStoredProcCommand(deleteStoredProcedure))
				{
					database.AddInParameter(dbCommand, autoIdField, DbType.Int32, id);

                    _DatabaseRepository.ExecuteNonQuery(dbCommand);
				}
			}
		}

        internal override void BuildItemList(IBaseDataItemList parentDataItemList, IBaseDataItemList relatedDataItemList, IBaseDataItemList childDataItemList, string getRelatedStoredProcedure)
	    {
            if (getRelatedStoredProcedure.Equals(string.Empty))
            {
                throw (new Exception("GetListStoredProcedure not provided"));
            }

            var database = _DatabaseRepository.GetDatabase();

            using (var dbCommand = _DatabaseRepository.GetStoredProcCommand(getRelatedStoredProcedure))
            {
                parentDataItemList.SetParameters(getRelatedStoredProcedure, database, dbCommand);

                using (var dataReader = database.ExecuteReader(dbCommand))
                {
                    _DataReader = dataReader;

                    while (dataReader.Read())
                    {
                        parentDataItemList.AddItem(this);
                    }

                    dataReader.NextResult();

                    while (dataReader.Read())
                    {
                        relatedDataItemList.AddItem(this);
                    }

                    dataReader.NextResult();

                    while (dataReader.Read())
                    {
                        childDataItemList.AddItem(this);
                    }

                    _DataReader = null;
                }
            }
        }

	    internal override void BuildItemList(IBaseDataItemList baseDataItemList, string getListStoredProcedure)
		{
			if (getListStoredProcedure.Equals(string.Empty))
			{
				throw (new Exception("GetListStoredProcedure not provided"));
			}

            var database = _DatabaseRepository.GetDatabase();

            using (var dbCommand = _DatabaseRepository.GetStoredProcCommand(getListStoredProcedure))
			{
				baseDataItemList.SetParameters(getListStoredProcedure, database, dbCommand);

				using (var dataReader = database.ExecuteReader(dbCommand))
				{
				    _DataReader = dataReader;

                    while (dataReader.Read())
					{
						baseDataItemList.AddItem(this);
					}

				    _DataReader = null;
				}
			}
		}   

		// TODO : Paramaters via de parameter class doorgeven
		internal override int ExecuteCommand(string storedProcedure, params object[] parameters)
		{
		    var parameterList = new List<object> {parameters};

		    using (var dbCommand = _DatabaseRepository.GetStoredProcCommand(storedProcedure, parameterList))
			{
                return _DatabaseRepository.ExecuteNonQuery(dbCommand);
			}
		}

		internal override int ExecuteTextCommand(string textCommand)
		{
            return _DatabaseRepository.ExecuteNonQuery(CommandType.Text, textCommand);
		}

		internal override bool TableExists(string tableName)
		{
            return _DatabaseRepository.ObjectExists(tableName, "Tables");
		}

		internal override bool ViewExists(string viewName)
		{
            return _DatabaseRepository.ObjectExists(viewName, "Views");
		}

        internal override bool StoredProcedureExists(string storedProcedureName)
        {
            return _DatabaseRepository.ObjectExists(storedProcedureName, "Procedures");
        }

        internal override DataTable GetSchemaObject(string collection)
        {
            var database = _DatabaseRepository.GetDatabase();
            DataTable dataTable;

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

		private int GetOrdinal(string fieldName)
		{
			if (!IsAssigned(_DataReader))
			{
			    const string message = @"_DataReader: in SQLHost.GetOrdinal(string fieldName);";

			    throw new ArgumentNullException(message);
			}

		    return _DataReader.GetOrdinal(fieldName);
		}
	}
}