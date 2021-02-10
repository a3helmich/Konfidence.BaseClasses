using System.Collections.Generic;
using System.Data;
using JetBrains.Annotations;
using Konfidence.BaseData;
using Konfidence.DataBaseInterface;
using Konfidence.SqlHostProvider.SqlAccess;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    internal class PrimaryKeyDataItem : BaseDataItem, IPrimaryKeyDataItem
    {
        public string ConstraintName { get; set; } = string.Empty;

        public string ConstraintType { get; set; } = string.Empty;

        public string TableName { get; set; } = string.Empty;

        [NotNull]
        internal static List<PrimaryKeyDataItem> GetList([NotNull] IBaseClient client)
        {
            var primaryKeyDataItems = new List<PrimaryKeyDataItem>();

            var spParameterData = new List<ISpParameterData>();

            client.BuildItemList(primaryKeyDataItems, SpName.GetTablePrimaryKeyList, spParameterData);

            return primaryKeyDataItems;
        }

        [NotNull]
        protected override IBaseClient ClientBind()
        {
            return base.ClientBind<SqlClient>();
        }

        // TODO: internal
        public override void GetData([NotNull] IDataReader dataReader)
        {
            GetField(SqlConstant.ConstraintNameField, dataReader, out string constraintName);
            GetField(SqlConstant.ConstraintTypeField, dataReader, out string constraintType);
            GetField(SqlConstant.ConstraintTableNameField, dataReader, out string tableName);

            ConstraintName = constraintName;
            ConstraintType = constraintType;
            TableName = tableName;
        }
    }
}
