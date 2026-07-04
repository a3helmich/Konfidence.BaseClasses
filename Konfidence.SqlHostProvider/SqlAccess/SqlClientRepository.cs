using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using Konfidence.Base;
using Konfidence.DatabaseInterface;
using Konfidence.SqlDataAccess;
using Microsoft.Data.SqlClient;

namespace Konfidence.SqlHostProvider.SqlAccess
{
    internal class SqlClientRepository : IDataRepository
    {
        private readonly IClientConfig _clientConfig;

        public SqlClientRepository(IClientConfig clientConfig)
        {
            _clientConfig = clientConfig;
        }

        internal SqlDatabase GetDatabase()
        {
            Debug.WriteLine($"SqlClientRepository GetDatabase, default database: '{_clientConfig.DefaultDatabase}'");

            ConfigConnectionString? connection = _clientConfig.GetConfigConnection();

            if (!connection.IsAssigned())
            {
                return GetDefaultDatabase();
            }

            return SqlDatabaseFactory.Create(BuildConnectionString(connection));
        }

        internal static string BuildConnectionString(ConfigConnectionString connection)
        {
            SqlConnectionStringBuilder builder = new()
            {
                DataSource = connection.Server,
                InitialCatalog = connection.Database
            };

            if (connection.UserName.IsAssigned() && connection.Password.IsAssigned())
            {
                builder.UserID = connection.UserName;
                builder.Password = connection.Password;
                builder.PersistSecurityInfo = true;
                builder.IntegratedSecurity = false;

                return builder.ConnectionString;
            }

            builder.IntegratedSecurity = true;

            return builder.ConnectionString;
        }

        private static SqlDatabase GetDefaultDatabase()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            DatabaseSettings? databaseSettings = config.Sections["dataConfiguration"] as DatabaseSettings;

            string? defaultDatabaseName = databaseSettings?.DefaultDatabase;

            ConnectionStringSettings? connectionStringSettings = defaultDatabaseName.IsAssigned()
                ? config.ConnectionStrings.ConnectionStrings[defaultDatabaseName]
                : null;

            if (connectionStringSettings is null)
            {
                throw new InvalidOperationException("No connection could be resolved: no matching connection was found in configuration, and no default database connection string is configured in app.config.");
            }

            return SqlDatabaseFactory.Create(connectionStringSettings.ConnectionString);
        }

        public DataTable GetSchemaObject(string collection)
        {
            SqlDatabase database = GetDatabase();

            using (DbConnection? dbConnection = database.CreateConnection())
            {
                dbConnection.Open();

                using (DataTable schemaTable = dbConnection.GetSchema(collection))
                {
                    DataTable dataTable = schemaTable.Copy();

                    return dataTable;
                }
            }
        }

        public int ExecuteCommandStoredProcedure(string saveStoredProcedure, List<ISpParameterData> parameterObjectList)
        {
            SqlDatabase database = GetDatabase();

            using (DbCommand? dbCommand = database.GetStoredProcCommand(saveStoredProcedure))
            {
                foreach (ISpParameterData parameterObject in parameterObjectList)
                {
                    database.AddInParameter(dbCommand, parameterObject.ParameterName, parameterObject.DbType, parameterObject.Value);
                }

                return database.ExecuteNonQuery(dbCommand);
            }
        }

        public void ExecuteSaveStoredProcedure(IBaseDataItem dataItem)
        {
            SqlDatabase database = GetDatabase();

            using (DbCommand? dbCommand = database.GetStoredProcCommand(dataItem.SaveStoredProcedure))
            {
                SetParameterData(dataItem, database, dbCommand);

                database.ExecuteNonQuery(dbCommand);

                GetParameterData(dataItem, database, dbCommand);
            }
        }

        public void ExecuteGetStoredProcedure(IBaseDataItem dataItem)
        {
            SqlDatabase database = GetDatabase();

            using (DbCommand? dbCommand = database.GetStoredProcCommand(dataItem.GetStoredProcedure))
            {
                SetParameterData(dataItem.GetParameterObjects(), database, dbCommand);

                using (IDataReader? dataReader = database.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                    {
                        dataItem.GetKey(dataReader);
                        dataItem.GetData(dataReader);
                    }
                }
            }
        }

        public void ExecuteGetByStoredProcedure(IBaseDataItem dataItem, string storedProcedure)
        {
            SqlDatabase database = GetDatabase();

            using (DbCommand? dbCommand = database.GetStoredProcCommand(storedProcedure))
            {
                SetParameterData(dataItem.GetParameterObjects(), database, dbCommand);

                using (IDataReader? dataReader = database.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                    {
                        dataItem.GetKey(dataReader);
                        dataItem.GetData(dataReader);
                    }
                }
            }
        }

        public void ExecuteGetListStoredProcedure<T>(IList<T> baseDataItemList, string storedProcedure, IBaseClient baseClient) where T : IBaseDataItem, new()
        {
            ExecuteGetListStoredProcedure(baseDataItemList, storedProcedure, new List<ISpParameterData>(), baseClient);
        }

        public void ExecuteGetListStoredProcedure<T>(IList<T> baseDataItemList, string storedProcedure, IList<ISpParameterData> spParameters, IBaseClient baseClient) where T : IBaseDataItem, new()
        {
            SqlDatabase database = GetDatabase();

            using (DbCommand? dbCommand = database.GetStoredProcCommand(storedProcedure))
            {
                SetParameterData(spParameters, database, dbCommand);

                using (IDataReader? dataReader = database.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        T dataItem = new(); // dependency resolver

                        dataItem.InitializeDataItem();

                        dataItem.GetKey(dataReader);
                        dataItem.GetData(dataReader);

                        baseDataItemList.Add(dataItem);
                    }
                }
            }
        }

        public void ExecuteDeleteStoredProcedure(IBaseDataItem dataItem)
        {
            int id = dataItem.GetId();

            if (id == 0)
            {
                return;
            }

            SqlDatabase database = GetDatabase();

            using (DbCommand? dbCommand = database.GetStoredProcCommand(dataItem.DeleteStoredProcedure))
            {
                database.AddInParameter(dbCommand, dataItem.AutoIdField, DbType.Int32, dataItem.GetId());

                database.ExecuteNonQuery(dbCommand);
            }
        }

        public int ExecuteTextCommandQuery(string textCommand)
        {
            SqlDatabase database = GetDatabase();

            return database.ExecuteNonQuery(CommandType.Text, textCommand);
        }

        public bool ObjectExists(string objectName, string collection)
        {
            SqlDatabase database = GetDatabase();

            using (DbConnection? dbConnection = database.CreateConnection())
            {
                dbConnection.Open();

                using (DataTable schemaTable = dbConnection.GetSchema(collection))
                {
                    IEnumerable<DataRow> rows = schemaTable
                        .Rows
                        .OfType<DataRow>();
                    return rows
                        .Any(x => (x[2].ToString() ?? string.Empty).Equals(objectName, StringComparison.OrdinalIgnoreCase));
                }
            }
        }

        private static void SetParameterData(IBaseDataItem dataItem, SqlDatabase database, DbCommand dbCommand)
        {
            // autoidfield
            database.AddParameter(dbCommand, dataItem.AutoIdField, DbType.Int32, ParameterDirection.InputOutput,
                dataItem.AutoIdField, DataRowVersion.Proposed, dataItem.GetId());

            // fields changing at the database side
            foreach (ISpParameterData parameterObject in dataItem.AutoUpdateFieldDictionary.Values)
            {
                database.AddParameter(dbCommand, parameterObject.ParameterName, parameterObject.DbType, ParameterDirection.InputOutput,
                    parameterObject.ParameterName, DataRowVersion.Proposed, parameterObject.Value);
            }

            // all the other fields
            foreach (ISpParameterData parameterObject in dataItem.SetItemData())
            {
                database.AddInParameter(dbCommand, parameterObject.ParameterName, parameterObject.DbType, parameterObject.Value);
            }
        }

        private static void GetParameterData(IBaseDataItem dataItem, SqlDatabase database, DbCommand dbCommand)
        {
            dataItem.SetId((int)database.GetParameterValue(dbCommand, dataItem.AutoIdField));

            foreach (KeyValuePair<string, ISpParameterData> kvp in dataItem.AutoUpdateFieldDictionary)
            {
                object? fieldValue = database.GetParameterValue(dbCommand, kvp.Value.ParameterName);

                if (DBNull.Value.Equals(fieldValue))
                {
                    kvp.Value.Value = null;

                    continue;
                }

                kvp.Value.Value = fieldValue;
            }
        }

        private static void SetParameterData(IList<ISpParameterData> parameterObjectList, SqlDatabase database, DbCommand dbCommand)
        {
            foreach (ISpParameterData parameterObject in parameterObjectList)
            {
                database.AddInParameter(dbCommand, parameterObject.ParameterName, parameterObject.DbType, parameterObject.Value);
            }

            parameterObjectList.Clear();
        }
    }
}
