﻿using System.Data;
using JetBrains.Annotations;
using Konfidence.BaseData;
using Konfidence.DataBaseInterface;
using Konfidence.SqlHostProvider.SqlAccess;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    internal class PrimaryKeyDataItem : BaseDataItem
    {
        // field definitions

        private string _constraintName = string.Empty;

        public string ConstraintName
        {
            get => _constraintName;
            set => _constraintName = value;
        }

        public PrimaryKeyDataItem()
        {
            GetStoredProcedure = SpName.PrimarykeyGet;
        }

        public PrimaryKeyDataItem(string connectionName, string tableName)
            : this()
        {
            ConnectionName = connectionName;

            SetParameter(SqlConstant.TableName, tableName);

            GetItem();
        }

        [NotNull]
        protected override IBaseClient ClientBind()
        {
            return base.ClientBind<SqlClient>();
        }

        // TODO: internal
        public override void InitializeDataItem()
        {
            AutoIdField = SqlConstant.PrimaryKeyId;
        }

        // TODO: internal
        public override void GetData(IDataReader dataReader)
        {
            GetField(SqlConstant.ConstraintNameField, dataReader, out _constraintName);
        }
    }
}
