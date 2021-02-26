using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.DataBaseInterface;
using Konfidence.SqlHostProvider.SqlConnectionManagement;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Konfidence.SqlHostProvider.SqlAccess
{
    internal class SqlClientRepository : IDataRepository
    {
        [NotNull] private readonly IClientConfig _clientConfig;

        public SqlClientRepository([NotNull] IClientConfig clientConfig)
        {
            _clientConfig = clientConfig;
        }

        private Database GetDatabase()
        {
            Debug.WriteLine($"SqlClientRepository GetDatabase, default database: '{_clientConfig.DefaultDatabase}'");

            var connection = _clientConfig.GetConfigConnection();

            if (!connection.IsAssigned())
            {
                return new DatabaseProviderFactory().CreateDefault();
            }

            var config = ConnectionManagement.SetDatabaseSecurityInMemory(connection.UserName, connection.Password, connection.ConnectionName);

            return new DatabaseProviderFactory(config.GetSection).Create(connection.ConnectionName);
        }

        [NotNull]
        public DataTable GetSchemaObject(string collection)
        {
            var database = GetDatabase();

            using (var dbConnection = database.CreateConnection())
            {
                dbConnection.Open();

                using (var schemaTable = dbConnection.GetSchema(collection))
                {
                    var dataTable = schemaTable.Copy();

                    return dataTable;
                }
            }
        }

        public int ExecuteCommandStoredProcedure(string saveStoredProcedure, [NotNull] List<ISpParameterData> parameterObjectList)
        {
            var database = GetDatabase();

            using (var dbCommand = database.GetStoredProcCommand(saveStoredProcedure))
            {
                foreach (var parameterObject in parameterObjectList)
                {
                    database.AddInParameter(dbCommand, parameterObject.ParameterName, parameterObject.DbType, parameterObject.Value);
                }

                return database.ExecuteNonQuery(dbCommand);
            }
        }

        public void ExecuteSaveStoredProcedure([NotNull] IBaseDataItem dataItem)
        {
            var database = GetDatabase();

            using (var dbCommand = database.GetStoredProcCommand(dataItem.SaveStoredProcedure))
            {
                SetParameterData(dataItem, database, dbCommand);

                database.ExecuteNonQuery(dbCommand);

                GetParameterData(dataItem, database, dbCommand);
            }
        }

        public void ExecuteGetStoredProcedure([NotNull] IBaseDataItem dataItem)
        {
            var database = GetDatabase();

            using (var dbCommand = database.GetStoredProcCommand(dataItem.GetStoredProcedure))
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

        public void ExecuteGetByStoredProcedure([NotNull] IBaseDataItem dataItem, string storedProcedure)
        {
            var database = GetDatabase();

            using (var dbCommand = database.GetStoredProcCommand(storedProcedure))
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

        public void ExecuteGetListStoredProcedure<T>(IList<T> baseDataItemList, string storedProcedure, IBaseClient baseClient) where T : IBaseDataItem, new()
        {
            ExecuteGetListStoredProcedure(baseDataItemList, storedProcedure, new List<ISpParameterData>(), baseClient);
        }

        public void ExecuteGetListStoredProcedure<T>(IList<T> baseDataItemList, string storedProcedure, [NotNull] IList<ISpParameterData> spParameters, IBaseClient baseClient) where T : IBaseDataItem, new()
        {
            var database = GetDatabase();

            using (var dbCommand = database.GetStoredProcCommand(storedProcedure))
            {
                SetParameterData(spParameters, database, dbCommand);

                using (var dataReader = database.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        var dataItem = new T(); // dependency resolver

                        dataItem.InitializeDataItem();

                        dataItem.GetKey(dataReader);
                        dataItem.GetData(dataReader);

                        baseDataItemList.Add(dataItem);
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

            using (var dbCommand = database.GetStoredProcCommand(dataItem.DeleteStoredProcedure))
            {
                database.AddInParameter(dbCommand, dataItem.AutoIdField, DbType.Int32, dataItem.GetId());

                database.ExecuteNonQuery(dbCommand);
            }
        }

        public int ExecuteTextCommandQuery(string textCommand)
        {
            var database = GetDatabase();

            return database.ExecuteNonQuery(CommandType.Text, textCommand);
        }

        public bool ObjectExists(string objectName, string collection)
        {
            var database = GetDatabase();

            using (var dbConnection = database.CreateConnection())
            {
                dbConnection.Open();

                using (var schemaTable = dbConnection.GetSchema(collection))
                {
                    return schemaTable
                        .Rows
                        .Cast<DataRow>()
                        .Any(x => x[2].ToString().Equals(objectName, StringComparison.OrdinalIgnoreCase));
                }
            }
        }

        private static void SetParameterData([NotNull] IBaseDataItem dataItem, [NotNull] Database database, DbCommand dbCommand)
        {
            // autoidfield
            database.AddParameter(dbCommand, dataItem.AutoIdField, DbType.Int32, ParameterDirection.InputOutput,
                dataItem.AutoIdField, DataRowVersion.Proposed, dataItem.GetId());

            // fields changing at the database side
            foreach (var parameterObject in dataItem.AutoUpdateFieldDictionary.Values)
            {
                database.AddParameter(dbCommand, parameterObject.ParameterName, parameterObject.DbType, ParameterDirection.InputOutput,
                    parameterObject.ParameterName, DataRowVersion.Proposed, parameterObject.Value);
            }

            // all the other fields
            foreach (var parameterObject in dataItem.SetItemData())
            {
                database.AddInParameter(dbCommand, parameterObject.ParameterName, parameterObject.DbType, parameterObject.Value);
            }
        }

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

        private static void SetParameterData([NotNull] IList<ISpParameterData> parameterObjectList, Database database, DbCommand dbCommand)
        {
            foreach (var parameterObject in parameterObjectList)
            {
                database.AddInParameter(dbCommand, parameterObject.ParameterName, parameterObject.DbType, parameterObject.Value);
            }

            parameterObjectList.Clear();
        }
    }
}
