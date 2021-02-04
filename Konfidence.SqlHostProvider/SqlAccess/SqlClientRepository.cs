using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.DataBaseInterface;
using Konfidence.RepositoryInterface;
using Konfidence.RepositoryInterface.Objects;
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
        public ResponseParameters ExecuteSaveStoredProcedure([NotNull] RequestParameters executeParameters)
        {
            var database = GetDatabase();

            ResponseParameters resultParameters;

            using (var dbCommand = GetStoredProcCommand(executeParameters.StoredProcedure))
            {
                SetParameterData(executeParameters, database, dbCommand);

                ExecuteNonQuery(dbCommand);

                resultParameters = GetParameterData(executeParameters, database, dbCommand);
            }

            return resultParameters;
        }

        private static void SetParameterData([NotNull] RequestParameters executeParameters, [NotNull] Database database, DbCommand dbCommand)
        {
            // autoidfield parameter toevoegen
            database.AddParameter(dbCommand, executeParameters.AutoIdField, DbType.Int32, ParameterDirection.InputOutput,
                                                        executeParameters.AutoIdField, DataRowVersion.Proposed, executeParameters.Id);

            // alle velden die aan de kant van de database gewijzigd worden als parameter toevoegen
            foreach (var kvp in executeParameters.AutoUpdateFieldList)
            {
                var parameterObject = kvp.Value;

                database.AddParameter(dbCommand, parameterObject.ParameterName, parameterObject.DbType, ParameterDirection.InputOutput,
                                                            parameterObject.ParameterName, DataRowVersion.Proposed, parameterObject.Value);
            }

            // alle overige parameters toevoegen
            foreach (var parameterObject in executeParameters.ParameterObjectList)
            {
                database.AddInParameter(dbCommand, parameterObject.ParameterName, parameterObject.DbType, parameterObject.Value);
            }
        }

        [NotNull]
        private static ResponseParameters GetParameterData([NotNull] RequestParameters executeParameters, [NotNull] Database database, DbCommand dbCommand)
        {
            var responseParameters = new ResponseParameters();

            responseParameters.SetId((int)database.GetParameterValue(dbCommand, executeParameters.AutoIdField));
            responseParameters.SetAutoUpdateFieldList(executeParameters.AutoUpdateFieldList);

            foreach (var kvp in responseParameters.AutoUpdateFieldList)
            {
                kvp.Value.Value = database.GetParameterValue(dbCommand, kvp.Value.ParameterName);

                if (DBNull.Value.Equals(kvp.Value.Value))
                {
                    kvp.Value.Value = null;
                }
            }

            return responseParameters;
        }

        public void ExecuteGetStoredProcedure([NotNull] IBaseDataItem baseDataItem)
        {
            var database = GetDatabase();

            using (var dbCommand = GetStoredProcCommand(baseDataItem.LoadStoredProcedure))
            {
                SetParameterData(baseDataItem.GetParameterObjects(), database, dbCommand);

                using (var dataReader = database.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                    {
                        baseDataItem.GetKey(dataReader);
                        baseDataItem.GetData(dataReader);
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

        public void ExecuteGetListStoredProcedure<T>([NotNull] RetrieveListParameters<T> retrieveListParameters, IBaseDataItemList<T> baseDataItemList, IBaseClient baseClient) where T : IBaseDataItem
        {
            var database = GetDatabase();

            using (var dbCommand = GetStoredProcCommand(retrieveListParameters.StoredProcedure))
            {
                SetParameterData(retrieveListParameters, database, dbCommand);

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

        public void ExecuteGetRelatedListStoredProcedure<T>([NotNull] RetrieveListParameters<T> retrieveListParameters,
            IBaseDataItemList<T> parentDataItemList, IBaseDataItemList<T> relatedDataItemList, IBaseDataItemList<T> childDataItemList, IBaseClient baseClient) where T : IBaseDataItem
        {
            var database = GetDatabase();

            using (var dbCommand = GetStoredProcCommand(retrieveListParameters.StoredProcedure))
            {
                SetParameterData(retrieveListParameters, database, dbCommand);

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

        private static void SetParameterData<T>([NotNull] RetrieveListParameters<T> executeParameters, Database database, DbCommand dbCommand) where T : IBaseDataItem
        {
            foreach (var parameterObject in executeParameters.ParameterObjectList)
            {
                database.AddInParameter(dbCommand, parameterObject.ParameterName, parameterObject.DbType, parameterObject.Value);
            }

            executeParameters.ParameterObjectList.Clear();
        }

        public void ExecuteDeleteStoredProcedure(string deleteStoredProcedure, string autoIdField, int id)
        {
            var database = GetDatabase();

            using (var dbCommand = GetStoredProcCommand(deleteStoredProcedure))
            {
                database.AddInParameter(dbCommand, autoIdField, DbType.Int32, id);

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
