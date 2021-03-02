﻿using System.Collections.Generic;
using System.Data;
using JetBrains.Annotations;
using Konfidence.BaseData;
using Konfidence.DataBaseInterface;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    internal class PrimaryKeyDataItem : BaseDataItem, IPrimaryKeyDataItem
    {
        public string ConstraintName { get; set; } = string.Empty;

        public string ConstraintType { get; set; } = string.Empty;

        public string TableName { get; set; } = string.Empty;

        [NotNull]
        internal static List<IPrimaryKeyDataItem> GetList([NotNull] IBaseClient client)
        {
            var primaryKeyDataItems = new List<PrimaryKeyDataItem>();

            var spParameterData = new List<ISpParameterData>();

            client.BuildItemList(primaryKeyDataItems, SpName.GetTablePrimaryKeyList, spParameterData);

            return new List<IPrimaryKeyDataItem>(primaryKeyDataItems);
        }

        // TODO: internal
        public override void GetData([NotNull] IDataReader dataReader)
        {
            dataReader.GetField(SqlConstant.ConstraintNameField,  out string constraintName);
            dataReader.GetField(SqlConstant.ConstraintTypeField, out string constraintType);
            dataReader.GetField(SqlConstant.ConstraintTableNameField, out string tableName);

            ConstraintName = constraintName;
            ConstraintType = constraintType;
            TableName = tableName;
        }
    }
}
