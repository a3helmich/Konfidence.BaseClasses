using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using Konfidence.Base;
using Konfidence.BaseData.SqlServerManagement;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Konfidence.BaseData
{
    internal class DatabaseRepository : BaseItem, IDatabaseRepository
    {
        private readonly string _DataBasename;

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

        public void SetParameterData(RequestParameters executeParameters, Database database, DbCommand dbCommand)
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

        public ResponseParameters GetParameterData(RequestParameters executeParameters, Database database, DbCommand dbCommand)
        {
            var responseParameters = new ResponseParameters();

            responseParameters.SetId((int)database.GetParameterValue(dbCommand, executeParameters.AutoIdField));
            responseParameters.SetAutoUpdateFieldList(executeParameters.AutoUpdateFieldList);

            //foreach (var kvp in responseParameters.AutoUpdateFieldList)
            //{
            //    kvp.Value.Value = database.GetParameterValue(dbCommand, kvp.Value.Field);

            //    if (DBNull.Value.Equals(kvp.Value.Value))
            //    {
            //        kvp.Value.Value = null;
            //    }
            //}

            return responseParameters;
        }

        public int ExecuteNonQuery(DbCommand dbCommand)
        {
            return GetDatabase().ExecuteNonQuery(dbCommand);
        }

        public int ExecuteNonQuery(CommandType commandType, string textCommand)
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
