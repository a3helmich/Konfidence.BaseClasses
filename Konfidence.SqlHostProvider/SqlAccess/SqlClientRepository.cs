using System;
using System.Data;
using System.Data.Common;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.BaseData.Objects;
using Konfidence.DataBaseInterface;
using Konfidence.RepositoryInterface;
using Konfidence.RepositoryInterface.Objects;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Konfidence.SqlHostProvider.SqlAccess
{
    internal class SqlClientRepository : IDataRepository
    {
        private readonly string _connectionName;

        public IDataReader DataReader { get; private set; }

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

        public int ExecuteNonQueryStoredProcedure(string saveStoredProcedure, [NotNull] IDbParameterObjectList parameterObjectList)
        {
            var database = GetDatabase();

            using (var dbCommand = GetStoredProcCommand(saveStoredProcedure))
            {
                foreach (var parameterObject in parameterObjectList)
                {
                    database.AddInParameter(dbCommand, parameterObject.Field, parameterObject.DbType, parameterObject.Value);
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

                database.AddParameter(dbCommand, parameterObject.Field, parameterObject.DbType, ParameterDirection.InputOutput,
                                                            parameterObject.Field, DataRowVersion.Proposed, parameterObject.Value);
            }

            // alle overige parameters toevoegen
            foreach (var parameterObject in executeParameters.ParameterObjectList)
            {
                database.AddInParameter(dbCommand, parameterObject.Field, parameterObject.DbType, parameterObject.Value);
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
                kvp.Value.Value = database.GetParameterValue(dbCommand, kvp.Value.Field);

                if (DBNull.Value.Equals(kvp.Value.Value))
                {
                    kvp.Value.Value = null;
                }
            }

            return responseParameters;
        }

        public void ExecuteGetStoredProcedure([NotNull] RetrieveParameters retrieveParameters, Func<bool> callback)
        {
            var database = GetDatabase();

            using (var dbCommand = GetStoredProcCommand(retrieveParameters.StoredProcedure))
            {
                SetParameterData(retrieveParameters, database, dbCommand);

                using (var dataReader = database.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                    {
                        DataReader = dataReader;

                        if (callback.IsAssigned())
                        {
                            callback();
                        }

                        DataReader = null;
                    }
                }
            }
        }

        private static void SetParameterData([NotNull] RetrieveParameters executeParameters, Database database, DbCommand dbCommand)
        {
            foreach (var parameterObject in executeParameters.ParameterObjectList)
            {
                database.AddInParameter(dbCommand, parameterObject.Field, parameterObject.DbType, parameterObject.Value);
            }

            executeParameters.ParameterObjectList.Clear();
        }

        public void ExecuteGetListStoredProcedure<T>([NotNull] RetrieveListParameters<T> retrieveListParameters, Func<bool> callback) where T : IBaseDataItem
        {
            var database = GetDatabase();

            using (var dbCommand = GetStoredProcCommand(retrieveListParameters.StoredProcedure))
            {
                SetParameterData(retrieveListParameters, database, dbCommand);

                using (var dataReader = database.ExecuteReader(dbCommand))
                {
                    DataReader = dataReader;

                    while (dataReader.Read())
                    {
                        if (callback.IsAssigned())
                        {
                            callback();
                        }
                    }

                    DataReader = null;
                }
            }
        }

        public void ExecuteGetRelatedListStoredProcedure<T>([NotNull] RetrieveListParameters<T> retrieveListParameters, Func<bool> parentCallback, Func<bool> relatedCallback, Func<bool> childCallback) where T : IBaseDataItem
        {
            var database = GetDatabase();

            using (var dbCommand = GetStoredProcCommand(retrieveListParameters.StoredProcedure))
            {
                SetParameterData(retrieveListParameters, database, dbCommand);

                using (var dataReader = database.ExecuteReader(dbCommand))
                {
                    DataReader = dataReader;

                    while (dataReader.Read())
                    {
                        if (parentCallback.IsAssigned())
                        {
                            parentCallback();
                        }
                    }

                    dataReader.NextResult();

                    while (dataReader.Read())
                    {
                        if (relatedCallback.IsAssigned())
                        {
                            relatedCallback();
                        }
                    }

                    dataReader.NextResult();

                    while (dataReader.Read())
                    {
                        if (childCallback.IsAssigned())
                        {
                            childCallback();
                        }
                    }

                    DataReader = null;
                }
            }
        }

        private static void SetParameterData<T>([NotNull] RetrieveListParameters<T> executeParameters, Database database, DbCommand dbCommand) where T : IBaseDataItem
        {
            foreach (var parameterObject in executeParameters.ParameterObjectList)
            {
                database.AddInParameter(dbCommand, parameterObject.Field, parameterObject.DbType, parameterObject.Value);
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
        public int ExecuteNonQuery(string storedProcedure, [NotNull] DbParameterObjectList parameterList)
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
