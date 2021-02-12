using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.DataBaseInterface;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Konfidence.SqlHostProvider.SqlAccess
{
    internal class SqlClientRepository : IDataRepository
    {
        private readonly string _connectionName;

        public SqlClientRepository(string connectionName)
        {
            _connectionName = connectionName;
        }

        private Database GetDatabase()
        {
            var databaseProviderFactory = new DatabaseProviderFactory();

            var databaseInstance = _connectionName.IsAssigned() ? databaseProviderFactory.Create(_connectionName) : databaseProviderFactory.CreateDefault();

            return databaseInstance;
        }

        [NotNull]
        public DataTable GetSchemaObject(string collection)
        {
            DataTable dataTable;

            var database = GetDatabase();

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

        public int ExecuteCommandStoredProcedure(string saveStoredProcedure, [NotNull] List<ISpParameterData> parameterObjectList)
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
            foreach (var parameterObject in dataItem.AutoUpdateFieldDictionary.Values)
            {
                database.AddParameter(dbCommand, parameterObject.ParameterName, parameterObject.DbType, ParameterDirection.InputOutput,
                                                            parameterObject.ParameterName, DataRowVersion.Proposed, parameterObject.Value);
            }

            // alle overige parameters toevoegen
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

        public void ExecuteGetByStoredProcedure([NotNull] IBaseDataItem dataItem, string storedProcedure)
        {
            var database = GetDatabase();

            using (var dbCommand = GetStoredProcCommand(storedProcedure))
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

            using (var dbCommand = GetStoredProcCommand(storedProcedure))
            {
                SetParameterData(spParameters, database, dbCommand);

                using (var dataReader = database.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        var dataItem = new T
                        {
                            Client = baseClient
                        };

                        dataItem.InitializeDataItem();

                        dataItem.GetKey(dataReader);
                        dataItem.GetData(dataReader);

                        baseDataItemList.Add(dataItem);
                    }
                }
            }
        }

        //public void ExecuteGetListStoredProcedure<T>([NotNull] IBaseDataItemList<T> baseDataItemList, string storedProcedure, IBaseClient baseClient) where T : IBaseDataItem
        //{
        //    var database = GetDatabase();

        //    using (var dbCommand = GetStoredProcCommand(storedProcedure))
        //    {
        //        SetParameterData(baseDataItemList.GetParameterObjectList(), database, dbCommand);

        //        using (var dataReader = database.ExecuteReader(dbCommand))
        //        {
        //            while (dataReader.Read())
        //            {
        //                var dataItem = baseDataItemList.GetDataItem();

        //                dataItem.Client = baseClient;

        //                dataItem.GetKey(dataReader);
        //                dataItem.GetData(dataReader);
        //            }
        //        }
        //    }
        //}

        //public void ExecuteGetRelatedListStoredProcedure<T>(string storedProcedure, [NotNull] IBaseDataItemList<T> parentDataItemList, IBaseDataItemList<T> relatedDataItemList, IBaseDataItemList<T> childDataItemList, IBaseClient baseClient) where T : IBaseDataItem
        //{
        //    var database = GetDatabase();

        //    using (var dbCommand = GetStoredProcCommand(storedProcedure))
        //    {
        //        SetParameterData(parentDataItemList.GetParameterObjectList(), database, dbCommand);

        //        using (var dataReader = database.ExecuteReader(dbCommand))
        //        {
        //            while (dataReader.Read())
        //            {
        //                var dataItem = parentDataItemList.GetDataItem();

        //                dataItem.Client = baseClient;

        //                dataItem.GetKey(dataReader);
        //                dataItem.GetData(dataReader);
        //            }

        //            dataReader.NextResult();

        //            while (dataReader.Read())
        //            {
        //                var dataItem = relatedDataItemList.GetDataItem();

        //                dataItem.Client = baseClient;

        //                dataItem.GetKey(dataReader);
        //                dataItem.GetData(dataReader);
        //            }

        //            dataReader.NextResult();

        //            while (dataReader.Read())
        //            {
        //                var dataItem = childDataItemList.GetDataItem();

        //                dataItem.Client = baseClient;

        //                dataItem.GetKey(dataReader);
        //                dataItem.GetData(dataReader);
        //            }
        //        }
        //    }
        //}

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
        public int ExecuteNonQuery(string storedProcedure, [NotNull] List<ISpParameterData> parameterList)
        {
            return ExecuteCommandStoredProcedure(storedProcedure, parameterList);
        }

        public int ExecuteTextCommandQuery(string textCommand)
        {
            return GetDatabase().ExecuteNonQuery(CommandType.Text, textCommand);
        }

        private int ExecuteNonQuery(DbCommand dbCommand)
        {
            return GetDatabase().ExecuteNonQuery(dbCommand);
        }

        private DbCommand GetStoredProcCommand(string saveStoredProcedure)
        {
            return GetDatabase().GetStoredProcCommand(saveStoredProcedure);
        }

        private static void SetParameterData([NotNull] IList<ISpParameterData> parameterObjectList, Database database, DbCommand dbCommand)
        {
            foreach (var parameterObject in parameterObjectList)
            {
                database.AddInParameter(dbCommand, parameterObject.ParameterName, parameterObject.DbType, parameterObject.Value);
            }

            parameterObjectList.Clear();
        }

        public bool ObjectExists(string objectName, string collection)
        {
            using (var dbConnection = GetDatabase().CreateConnection())
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
    }
}
