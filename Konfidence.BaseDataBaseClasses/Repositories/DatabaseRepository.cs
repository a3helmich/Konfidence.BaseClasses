using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using Konfidence.Base;
using Konfidence.BaseData.IRepositories;
using Konfidence.BaseData.ParameterObjects;
using Konfidence.BaseData.SqlServerManagement;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Konfidence.BaseData.Repositories
{
    internal class DatabaseRepository : BaseItem, IDatabaseRepository
    {
        private readonly string _DataBasename;
        private IDataReader _DataReader;

        public IDataReader DataReader
        {
            get { return _DataReader; }
        }

        public DatabaseRepository(string databaseName)
        {
            _DataBasename = databaseName;
        }

        public Database GetDatabase()
        {
             Database databaseInstance;

            if (_DataBasename.Length > 0)
            {
                databaseInstance = DatabaseFactory.CreateDatabase(_DataBasename);
            }
            else
            {
                databaseInstance = DatabaseFactory.CreateDatabase();
            }

            if (Debugger.IsAttached)
            {
                if (databaseInstance.DbProviderFactory is SqlClientFactory)
                {
                    if (!SqlServerCheck.VerifyDatabaseServer(databaseInstance))
                    {
                    }
                }
            }

            return databaseInstance;
        }

        public DbCommand GetStoredProcCommand(string saveStoredProcedure)
        {
            return GetDatabase().GetStoredProcCommand(saveStoredProcedure);
        }

        public DbCommand GetStoredProcCommand(string saveStoredProcedure, List<object> parameters)
        {
            return GetDatabase().GetStoredProcCommand(saveStoredProcedure, parameters.ToArray());
        }

        public ResponseParameters ExecuteSaveStoredProcedure(RequestParameters executeParameters)
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

        private void SetParameterData(RequestParameters executeParameters, Database database, DbCommand dbCommand)
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

        private ResponseParameters GetParameterData(RequestParameters executeParameters, Database database, DbCommand dbCommand)
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

        public void ExecuteGetStoredProcedure(RetrieveParameters retrieveParameters, Func<bool> callback)
        {
            var database = GetDatabase();

            using (var dbCommand = GetStoredProcCommand(retrieveParameters.StoredProcedure))
            {
                SetParameterData(retrieveParameters, database, dbCommand);

                using (var dataReader = database.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                    {
                        _DataReader = dataReader;

                        if (IsAssigned(callback))
                        {
                            callback();
                        }

                        _DataReader = null;
                    }
                }
            }
        }

        private void SetParameterData(RetrieveParameters executeParameters, Database database, DbCommand dbCommand)
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


        public int ExecuteNonQuery(string storedProcedure, List<object> parameterList)
        {
            using (var dbCommand = GetStoredProcCommand(storedProcedure, parameterList))
            {
                return ExecuteNonQuery(dbCommand);
            }
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
