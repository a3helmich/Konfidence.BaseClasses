using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Konfidence.BaseData
{
	internal class SQLHost : BaseHost
	{
		private IDataReader _DataReader = null;

		public SQLHost(string dataBaseName): base(string.Empty, dataBaseName)
		{
		}

		#region GetField Methods
		internal override Int32 GetFieldInt32(string fieldName)
		{
			int fieldOrdinal = GetOrdinal(fieldName);

			if (_DataReader.IsDBNull(fieldOrdinal))
			{
				return 0;
			}

			return _DataReader.GetInt32(fieldOrdinal);
		}

		internal override string GetFieldString(string fieldName)
		{
			int fieldOrdinal = GetOrdinal(fieldName);

			if (_DataReader.IsDBNull(fieldOrdinal))
			{
				return string.Empty;
			}

			return _DataReader.GetString(fieldOrdinal);
		}

		internal override bool GetFieldBool(string fieldName)
		{
			int fieldOrdinal = GetOrdinal(fieldName);

			if (_DataReader.IsDBNull(fieldOrdinal))
			{
				return false;
			}

			return _DataReader.GetBoolean(fieldOrdinal);
		}

		internal override DateTime GetFieldDateTime(string fieldName)
		{
			int fieldOrdinal = GetOrdinal(fieldName);

			if (_DataReader.IsDBNull(fieldOrdinal))
			{
				return DateTime.MinValue;
			}

			return _DataReader.GetDateTime(fieldOrdinal);
		}

        internal override Decimal GetFieldDecimal(string fieldName)
        {
            int fieldOrdinal = GetOrdinal(fieldName);

            if (_DataReader.IsDBNull(fieldOrdinal))
            {
                return 0;
            }

            return _DataReader.GetDecimal(fieldOrdinal);
        }
        #endregion

		internal override void Save(BaseDataItem dataItem, string saveStoredProcedure, string autoIdField, int id)
		{
			if (autoIdField.Equals(string.Empty))
			{
				throw (new Exception("AutoIdField not provided"));
			}

			if (saveStoredProcedure.Equals(string.Empty))
			{
				throw (new Exception("SaveStoredProcedure not provided"));
			}

			Database database = GetDatabase();

			DbCommand dbCommand = database.GetStoredProcCommand(saveStoredProcedure);

			SetItemData(dataItem, database, dbCommand);

			database.ExecuteNonQuery(dbCommand);

			ItemId = (int)database.GetParameterValue(dbCommand, autoIdField);
		}

		internal override void GetItem(BaseDataItem dataItem, string getStoredProcedure, string autoIdField, int id)
		{
			if (getStoredProcedure.Equals(string.Empty))
			{
				throw (new Exception("GetStoredProcedure not provided"));
			}

			Database database = GetDatabase();

			using (DbCommand dbCommand = database.GetStoredProcCommand(getStoredProcedure))
			{
				dataItem.SetParameters(getStoredProcedure, database, dbCommand, id);

				using (IDataReader dataReader = database.ExecuteReader(dbCommand))
				{
					if (dataReader.Read())
					{
						_DataReader = dataReader;

						if (autoIdField.Equals(string.Empty)) // TODO: AutoIdField verplicht maken? ==> ja!
						{
							// throw(new Exception("AutoIdField is not assigned"));
						}
						else
						{
							ItemId = GetFieldInt32(autoIdField);
						}

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
				Database database = GetDatabase();

				using (DbCommand dbCommand = database.GetStoredProcCommand(deleteStoredProcedure))
				{
					database.AddInParameter(dbCommand, autoIdField, DbType.Int32, id);

					database.ExecuteNonQuery(dbCommand);
				}
			}
		}

		internal override void BuildItemList(IBaseDataItemList BaseDataItemList, string getListStoredProcedure)
		{
			if (getListStoredProcedure.Equals(string.Empty))
			{
				throw (new Exception("GetListStoredProcedure not provided"));
			}

			Database database = GetDatabase();

			using (DbCommand dbCommand = database.GetStoredProcCommand(getListStoredProcedure))
			{
				BaseDataItemList.SetParameters(getListStoredProcedure, database, dbCommand);

				using (IDataReader dataReader = database.ExecuteReader(dbCommand))
				{

					_DataReader = dataReader;

					while (dataReader.Read())
					{
						BaseDataItemList.AddItem(this);
					}

					_DataReader = null;
				}
			}
		}   

		// TODO : Paramaters via de parameter class doorgeven
		internal override int ExecuteCommand(string storedProcedure, params object[] parameters)
		{
			Database database = GetDatabase();

			int effectedRows;

			using (DbCommand dbCommand = database.GetStoredProcCommand(storedProcedure, parameters))
			{
				effectedRows = database.ExecuteNonQuery(dbCommand);
			}

			return effectedRows;
		}

		internal override int ExecuteTextCommand(string textCommand)
		{
			Database database = GetDatabase();

			return database.ExecuteNonQuery(CommandType.Text, textCommand);
		}

		internal override bool TableExists(string tableName)
		{
			return ObjectExists(tableName, "Tables");
		}

		internal override bool ViewExists(string viewName)
		{
			return ObjectExists(viewName, "Views");
		}

        internal override bool StoredProcedureExists(string storedProcedureName)
        {
            return ObjectExists(storedProcedureName, "Procedures");
        }

        internal override DataTable GetSchemaObject(string collection)
        {
            Database database = GetDatabase();
            DataTable dataTable;

            using (DbConnection dbConnection = database.CreateConnection())
            {
                dbConnection.Open();

                using (DataTable schemaTable = dbConnection.GetSchema(collection))
                {
                        dataTable = schemaTable.Copy();
                }
            }

            return dataTable;
        }

		private bool ObjectExists(string objectName, string collection)
		{
			// TODO: schema information
			//DbConnection dbConnection = database.CreateConnection();
			//DataTable schemaTable = dbConnection.GetSchema("Tables"); 
			// MetaDataCollections, DataSourceInformation, DataTypes, Restrictions, ReservedWords, 
			// Users, Databases, Tables, Columns, Views, ViewColumns, ProcedureParameters, 
			// Procedures, ForeignKeys, IndexColumns, Indexes, UserDefinedTypes
			Database database = GetDatabase();

			using (DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();

				using (DataTable schemaTable = dbConnection.GetSchema(collection))
				{
					foreach (DataRow dataRow in schemaTable.Rows)
					{
						string foundName = dataRow[2].ToString();

						if (objectName.Equals(foundName, StringComparison.InvariantCultureIgnoreCase))
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		private Database GetDatabase()
		{
			if (DataBaseName.Length > 0)
			{
				return DatabaseFactory.CreateDatabase(DataBaseName);
			}
			else
			{
				return DatabaseFactory.CreateDatabase();
			}
		}

		private int GetOrdinal(string fieldName)
		{
			if (!IsAssigned(_DataReader))
			{
				throw new ArgumentNullException("dataRecord");
			}

			return _DataReader.GetOrdinal(fieldName);
		}

		private static void SetItemData(BaseDataItem dataItem, Database database, DbCommand dbCommand)
		{
			database.AddParameter(dbCommand, dataItem.AutoIdField, DbType.Int32, ParameterDirection.InputOutput,
														dataItem.AutoIdField, DataRowVersion.Proposed, dataItem.Id);

			List<BaseDataItem.ParameterObject> ParameterObjectList = dataItem.SetItemData();

			foreach (BaseDataItem.ParameterObject parameterObject in ParameterObjectList)
			{
				database.AddInParameter(dbCommand, parameterObject.Field, parameterObject.DbType, parameterObject.Value);
			}
		}
	}
}