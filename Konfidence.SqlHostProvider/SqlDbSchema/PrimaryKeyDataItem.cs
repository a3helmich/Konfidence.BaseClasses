using System.Collections.Generic;
using System.Data;
using Konfidence.BaseData;
using Konfidence.DatabaseInterface;

namespace Konfidence.SqlHostProvider.SqlDbSchema;

internal class PrimaryKeyDataItem : BaseDataItem, IPrimaryKeyDataItem
{
    public string ConstraintName { get; set; } = string.Empty;

    public string ConstraintType { get; set; } = string.Empty;

    public string TableName { get; set; } = string.Empty;

    internal static List<IPrimaryKeyDataItem> GetList(IBaseClient client, string storedProcedure)
    {
        List<PrimaryKeyDataItem> primaryKeyDataItems = [];

        List<ISpParameterData> spParameterData = [];

        client.BuildItemList(primaryKeyDataItems, storedProcedure, spParameterData);

        return new List<IPrimaryKeyDataItem>(primaryKeyDataItems);
    }

    // TODO: internal
    public override void GetData(IDataReader dataReader)
    {
        dataReader.GetField(SqlConstant.ConstraintNameField, out string constraintName);
        dataReader.GetField(SqlConstant.ConstraintTypeField, out string constraintType);
        dataReader.GetField(SqlConstant.ConstraintTableNameField, out string tableName);

        ConstraintName = constraintName;
        ConstraintType = constraintType;
        TableName = tableName;
    }
}
