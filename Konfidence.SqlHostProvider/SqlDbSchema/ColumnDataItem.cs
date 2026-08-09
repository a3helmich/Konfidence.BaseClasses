using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Konfidence.Base;
using Konfidence.BaseData;
using Konfidence.DatabaseInterface;

namespace Konfidence.SqlHostProvider.SqlDbSchema;

public class ColumnDataItem : BaseDataItem, IColumnDataItem
{
    public bool IsPrimaryKey { get; private set; }

    public bool IsAutoUpdated { get; private set; }

    public bool IsDefaulted { get; private set; }

    public bool IsComputed { get; private set; }

    public bool IsLockInfo { get; private set; }

    public bool IsGuidField { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string TableName { get; private set; } = string.Empty;

    public string SqlDataType { get; private set; } = string.Empty;

    public string DataType { get; private set; } = string.Empty;

    public string DbDataType { get; private set; } = string.Empty;

    public string DefaultPropertyValue { get; private set; } = string.Empty;

    public string NewGuidPropertyValue { get; private set; } = string.Empty;

    public string CharacterMaximumLength { get; private set; } = string.Empty;

    public ColumnDataItem()
    {
        IsPrimaryKey = false;
        IsAutoUpdated = false;
        IsDefaulted = false;
        IsComputed = false;
        IsLockInfo = false;
        IsGuidField = false;
    }

    internal static List<IColumnDataItem> GetList(IBaseClient client, List<IIndexDataItem> allIndexDataItems)
    {
        List<ColumnDataItem> columnDataItems = [];

        List<ISpParameterData> spParameterData = [];

        client.BuildItemList(columnDataItems, SpName.GetColumnList, spParameterData);

        foreach (ColumnDataItem columnDataItem in columnDataItems.Where(x => x.Name.Equals("syslock", StringComparison.OrdinalIgnoreCase)))
        {
            columnDataItem.IsLockInfo = true;
        }

        foreach (ColumnDataItem columnDataItem in columnDataItems.Where(x => x.Name.Equals("sysupdatetime", StringComparison.OrdinalIgnoreCase)))
        {
            columnDataItem.IsAutoUpdated = true;
        }

        foreach (ColumnDataItem columnDataItem in allIndexDataItems
                     .Where(indexDataItem => indexDataItem.IsPrimaryKey)
                     .SelectMany(indexDataItem => columnDataItems
                         .Where(columnDataItem => columnDataItem.Name == indexDataItem.IndexName && columnDataItem.TableName == indexDataItem.TableName)))
        {
            columnDataItem.IsPrimaryKey = true;
        }

        return new List<IColumnDataItem>(columnDataItems);
    }

    // TODO : internal
    public override void GetData(IDataReader dataReader)
    {
        dataReader.GetField("Name", out string name);
        dataReader.GetField("tableName", out string tableName);
        dataReader.GetField("Default_object_id", out int defaultObjectId);
        dataReader.GetField("Is_Computed", out bool isComputed);
        dataReader.GetField("datatype", out string dataType);
        dataReader.GetField("max_length", out short characterMaximumLengthInt);

        Name = name;
        TableName = tableName;
        IsDefaulted = defaultObjectId > 0;
        IsComputed = isComputed;
        SqlDataType = dataType;
        CharacterMaximumLength = characterMaximumLengthInt.ToString(CultureInfo.InvariantCulture);

        DataType = GetDataType(SqlDataType);

        DbDataType = GetDbDataType(DataType);

        DefaultPropertyValue = GetDefaultPropertyValue(SqlDataType, string.Empty);

        NewGuidPropertyValue = GetDefaultPropertyValue(SqlDataType, "newguid");

        IsGuidField = SqlDataType.Equals("uniqueidentifier", StringComparison.InvariantCultureIgnoreCase);
    }

    internal static string GetDataType(string dataType)
    {
        dataType = dataType.ToLower();

        switch (dataType)
        {
            case "char":
            case "nchar":
            case "varchar":
            case "nvarchar":
            case "text":
            case "ntext":
                dataType = "string";
                break;
            case "date":
            case "datetime":
                dataType = "DateTime";
                break;
            case "time":
                dataType = "TimeSpan";
                break;
            case "uniqueidentifier":
                dataType = "Guid";
                break;
            case "bit":
                dataType = "bool";
                break;
            case "xml":
                dataType = "XmlDocument";
                break;
            case "money":
                dataType = "decimal";
                break;
            case "smallint":
                dataType = "short";
                break;
            case "tinyint":
                dataType = "byte";
                break;
            case "bigint":
                dataType = "long";
                break;
        }

        return dataType;
    }

    internal string GetDbDataType(string dataType)
    {
        if (dataType.Equals("int", StringComparison.InvariantCultureIgnoreCase))
        {
            dataType += "32";
        }

        if (dataType.Equals("byte", StringComparison.InvariantCultureIgnoreCase))
        {
            dataType += "8";
        }

        if (dataType.Equals("short", StringComparison.InvariantCultureIgnoreCase))
        {
            dataType += "16";
        }

        if (dataType.Equals("long", StringComparison.InvariantCultureIgnoreCase))
        {
            dataType += "64";
        }

        if (DataType.Equals("XmlDocument", StringComparison.InvariantCultureIgnoreCase))
        {
            dataType = "string";
        }

        if (dataType.Equals("bool", StringComparison.InvariantCultureIgnoreCase))
        {
            dataType = "Boolean";
        }

        return dataType.InitUpperCase();
    }

    internal static string GetDefaultPropertyValue(string dataType, string newValue)
    {
        string defaultPropertyValuelinePart = string.Empty;

        switch (dataType)
        {
            case "int":
            case "tinyint":
            case "smallint":
            case "bigint":
                defaultPropertyValuelinePart = " = 0";
                break;
            case "bit":
                defaultPropertyValuelinePart = " = false";
                break;
            case "varchar":
            case "char":
            case "nvarchar":
            case "text":
            case "ntext":
            case "nchar":
                defaultPropertyValuelinePart = " = string.Empty";
                break;
            case "uniqueidentifier":
                defaultPropertyValuelinePart = newValue.Equals("newguid", StringComparison.InvariantCultureIgnoreCase) ? " = Guid.NewGuid()" : " = Guid.Empty";
                break;
            case "xml":
                defaultPropertyValuelinePart = " = new XmlDocument()";
                break;
            case "datetime":
                defaultPropertyValuelinePart = " = DateTime.MinValue";
                break;
        }

        return defaultPropertyValuelinePart;
    }
}
