using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.DataBaseInterface;
using Konfidence.RepositoryInterface;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Konfidence.SqlHostProvider.SqlAccess
{
    public class SqlClientRepository : IDataRepository
    {
        private readonly string _connectionName;

        public SqlClientRepository(string connectionName)
        {
            _connectionName = connectionName;
        }

        public Database GetDatabase()
        {
            var databaseProviderFactory = new DatabaseProviderFactory();

            var databaseInstance = _connectionName.IsAssigned() ? databaseProviderFactory.Create(_connectionName) : databaseProviderFactory.CreateDefault();

            return databaseInstance;
        }

        public DbCommand GetStoredProcCommand(string saveStoredProcedure)
        {
            return GetDatabase().GetStoredProcCommand(saveStoredProcedure);
        }

        public int ExecuteNonQueryStoredProcedure(string saveStoredProcedure, [NotNull] List<IDbParameterObject> parameterObjectList)
        {
            var database = GetDatabase();

            using (var dbCommand = GetStoredProcCommand(saveStoredProcedure))
            {
                foreach (var parameterObject in parameterObjectList)
                {
                    database.AddInParameter(dbCommand, parameterObject.ParameterName, parameterObject.DbType, parameterObject.Value);
                }

                return ExecuteNonQuery(dbCommand);
            }
        }

        [NotNull]
        public void ExecuteSaveStoredProcedure([NotNull] IBaseDataItem dataItem)
        {
            var database = GetDatabase();

            using (var dbCommand = GetStoredProcCommand(dataItem.SaveStoredProcedure))
            {
                SetParameterData(dataItem, database, dbCommand);

                ExecuteNonQuery(dbCommand);

                GetParameterData(dataItem, database, dbCommand);
            }
        }

        private static void SetParameterData([NotNull] IBaseDataItem dataItem, [NotNull] Database database, DbCommand dbCommand)
        {
            // autoidfield parameter toevoegen
            database.AddParameter(dbCommand, dataItem.AutoIdField, DbType.Int32, ParameterDirection.InputOutput,
                dataItem.AutoIdField, DataRowVersion.Proposed, dataItem.GetId());

            // alle velden die aan de kant van de database gewijzigd worden als parameter toevoegen
            foreach (var kvp in dataItem.AutoUpdateFieldDictionary)
            {
                var parameterObject = kvp.Value;

                database.AddParameter(dbCommand, parameterObject.ParameterName, parameterObject.DbType, ParameterDirection.InputOutput,
                                                            parameterObject.ParameterName, DataRowVersion.Proposed, parameterObject.Value);
            }

            // alle overige parameters toevoegen
            foreach (var parameterObject in dataItem.GetParameterObjects())
            {
                database.AddInParameter(dbCommand, parameterObject.ParameterName, parameterObject.DbType, parameterObject.Value);
            }
        }

        [NotNull]
        private static void GetParameterData([NotNull] IBaseDataItem dataItem, [NotNull] Database database, DbCommand dbCommand)
        {
            dataItem.SetId((int)database.GetParameterValue(dbCommand, dataItem.AutoIdField));

            foreach (var kvp in dataItem.AutoUpdateFieldDictionary)
            {
                var fieldValue = database.GetParameterValue(dbCommand, kvp.Value.ParameterName);

                if (DBNull.Value.Equals(fieldValue))
                {
                    kvp.Value.Value = null;

                    continue;
                }

                kvp.Value.Value = fieldValue;
            }
        }

        public void ExecuteGetStoredProcedure([NotNull] IBaseDataItem dataItem)
        {
            var database = GetDatabase();

            using (var dbCommand = GetStoredProcCommand(dataItem.GetStoredProcedure))
            {
                SetParameterData(dataItem.GetParameterObjects(), database, dbCommand);

                using (var dataReader = database.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                    {
                        dataItem.GetKey(dataReader);
                        dataItem.GetData(dataReader);
                    }
                }
            }
        }

        private static void SetParameterData([NotNull] List<IDbParameterObject> parameterObjectList, Database database, DbCommand dbCommand)
        {
            foreach (var parameterObject in parameterObjectList)
            {
                database.AddInParameter(dbCommand, parameterObject.ParameterName, parameterObject.DbType, parameterObject.Value);
            }
        }

        public void ExecuteGetListStoredProcedure<T>([NotNull] IBaseDataItemList<T> baseDataItemList, string storedProcedure, IBaseClient baseClient) where T : IBaseDataItem
        {
            var database = GetDatabase();

            using (var dbCommand = GetStoredProcCommand(storedProcedure))
            {
                SetParameterData(baseDataItemList.GetParameterObjectList(), database, dbCommand);

                using (var dataReader = database.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        var dataItem = baseDataItemList.GetDataItem();

                        dataItem.Client = baseClient;

                        dataItem.GetKey(dataReader);
                        dataItem.GetData(dataReader);
                    }
                }
            }
        }

        public void ExecuteGetRelatedListStoredProcedure<T>(string storedProcedure, [NotNull] IBaseDataItemList<T> parentDataItemList, IBaseDataItemList<T> relatedDataItemList, IBaseDataItemList<T> childDataItemList, IBaseClient baseClient) where T : IBaseDataItem
        {
            var database = GetDatabase();

            using (var dbCommand = GetStoredProcCommand(storedProcedure))
            {
                SetParameterData(parentDataItemList.GetParameterObjectList(), database, dbCommand);

                using (var dataReader = database.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        var dataItem = parentDataItemList.GetDataItem();

                        dataItem.Client = baseClient;

                        dataItem.GetKey(dataReader);
                        dataItem.GetData(dataReader);
                    }

                    dataReader.NextResult();

                    while (dataReader.Read())
                    {
                        var dataItem = relatedDataItemList.GetDataItem();

                        dataItem.Client = baseClient;

                        dataItem.GetKey(dataReader);
                        dataItem.GetData(dataReader);
                    }

                    dataReader.NextResult();

                    while (dataReader.Read())
                    {
                        var dataItem = childDataItemList.GetDataItem();

                        dataItem.Client = baseClient;

                        dataItem.GetKey(dataReader);
                        dataItem.GetData(dataReader);
                    }
                }
            }
        }

        public void ExecuteDeleteStoredProcedure([NotNull] IBaseDataItem dataItem)
        {
            var id = dataItem.GetId();

            if (id == 0)
            {
                return;
            }

            var database = GetDatabase();

            using (var dbCommand = GetStoredProcCommand(dataItem.DeleteStoredProcedure))
            {
                database.AddInParameter(dbCommand, dataItem.AutoIdField, DbType.Int32, dataItem.GetId());

                ExecuteNonQuery(dbCommand);
            }
        }

        [UsedImplicitly]
        public int ExecuteNonQuery(string storedProcedure, [NotNull] List<IDbParameterObject> parameterList)
        {
            return ExecuteNonQueryStoredProcedure(storedProcedure, parameterList);
        }

        public int ExecuteNonQuery(DbCommand dbCommand)
        {
            return GetDatabase().ExecuteNonQuery(dbCommand);
        }

        public int ExecuteNonQuery(string textCommand)
        {
            return GetDatabase().ExecuteNonQuery(CommandType.Text, textCommand);
        }

        public bool ObjectExists(string objectName, string collection)
        {
            // TODO: schema information
            //DbConnection dbConnection = database.CreateConnection();
            //DataTable schemaTable = dbConnection.GetSchema("Tables"); 
            // MetaDataCollections, DataSourceInformation, DataTypes, Restrictions, ReservedWords, 
            // Users, Databases, Tables, Columns, Views, ViewColumns, ProcedureParameters, 
            // Procedures, ForeignKeys, IndexColumns, Indexes, UserDefinedTypes

            using (var dbConnection = GetDatabase().CreateConnection())
            {
                dbConnection.Open();

                using (var schemaTable = dbConnection.GetSchema(collection))
                {
                    foreach (DataRow dataRow in schemaTable.Rows)
                    {
                        var foundName = dataRow[2].ToString();

                        if (objectName.Equals(foundName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
