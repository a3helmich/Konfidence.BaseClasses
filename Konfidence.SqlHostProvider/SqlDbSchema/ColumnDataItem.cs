using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using Konfidence.BaseData;
using Konfidence.DataBaseInterface;
using Konfidence.SqlHostProvider.SqlAccess;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
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

        //private int _ordinalPosition;

        public string SqlDataType { get; private set; } = string.Empty;

        public string DataType { get; private set; } = string.Empty;

        public string DbDataType { get; private set; } = string.Empty;

        public string DefaultPropertyValue { get; private set; } = string.Empty;

        public string NewGuidPropertyValue { get; private set; } = string.Empty;

        public string CharacterMaximumLength { get; private set; } = string.Empty;

        //        if (tableName.Equals(_TableName))
        //        {
        //            string columnName = dataRow["COLUMN_NAME"] as string;

        //            columnDataItem = new ColumnDataItem(columnName);

        //            string columnDefault = dataRow["COLUMN_DEFAULT"] as string;
        //            string dataType = dataRow["DATA_TYPE"] as string;
        //            string characterMaximumLength = string.Empty;
        //            if (dataType.Equals("varchar") || dataType.Equals("char"))
        //            {
        //                int charLength = (int)dataRow["CHARACTER_MAXIMUM_LENGTH"];
        //                characterMaximumLength = charLength.ToString();
        //            }

        //            columnDataItem.ColumnDefault = columnDefault;
        //            columnDataItem.DataType = dataType;
        //            columnDataItem.CharacterMaximumLength = characterMaximumLength;

        //            /*
        //            bool isNullable = (bool)dataRow["IS_NULLABLE"];
        //            int characterMaximumLength = (int)dataRow["CHARACTER_MAXIMUM_LENGTH"];
        //            int characterOctetLength = (int)dataRow["CHARACTER_OCTET_LENGTH"];
        //            int numericPrecision = (int)dataRow["NUMERIC_PRECISION"];
        //            int numericPrecisionRadix = (int)dataRow["NUMERIC_PRECISION_RADIX"];
        //            int numericScale = (int)dataRow["NUMERIC_SCALE"];
        //            int dateTimePrecision = (int)dataRow["DATETIME_PRECISION"];
        //            string characterSetCatalog = dataRow["CHARACTER_SET_CATALOG"] as string;
        //            string characterSetSchema = dataRow["CHARACTER_SET_SCHEMA"] as string;
        //            string characterSetName = dataRow["CHARACTER_SET_NAME"] as string;
        //            string collationCatalog = dataRow["COLLATION_CATALOG"] as string;

        //            this.Add(columnDataItem);
        //        }
        //============================================   <<<<<<

        public ColumnDataItem()
        {
            IsPrimaryKey = false;
            IsAutoUpdated = false;
            IsDefaulted = false;
            IsComputed = false;
            IsLockInfo = false;
            IsGuidField = false;
        }

        [NotNull]
        internal static List<IColumnDataItem> GetList([NotNull] IBaseClient client, [NotNull] List<IIndexDataItem> allIndexDataItems)
        {
            var columnDataItems = new List<ColumnDataItem>();

            var spParameterData = new List<ISpParameterData>();

            client.BuildItemList(columnDataItems, SpName.GetColumnList, spParameterData);

            columnDataItems
                .Where(x => x.Name.Equals("syslock", StringComparison.OrdinalIgnoreCase))
                .ForEach(x => x.IsLockInfo = true);

            columnDataItems
                .Where(x => x.Name.Equals("sysupdatetime", StringComparison.OrdinalIgnoreCase))
                .ForEach(x => x.IsAutoUpdated = true);

            allIndexDataItems
                .Where(indexDataItem => indexDataItem.IsPrimaryKey)
                .SelectMany(indexDataItem => columnDataItems
                    .Where(columnDataItem => columnDataItem.Name == indexDataItem.IndexName && columnDataItem.TableName == indexDataItem.TableName))
                .ForEach(x => x.IsPrimaryKey = true);

            return new List<IColumnDataItem>(columnDataItems);
        }

        // TODO : internal
        public override void GetData([NotNull] IDataReader dataReader)
        {
            GetField("Name", dataReader, out string name);
            GetField("tableName", dataReader, out string tableName);
            GetField("Default_object_id", dataReader, out int defaultObjectId);
            GetField("Is_Computed", dataReader, out bool isComputed);
            //GetField("column_id", dataReader, out int ordinalPosition);
            GetField("datatype", dataReader, out string dataType);
            GetField("max_length", dataReader, out short characterMaximumLengthInt);

            Name = name;
            TableName = tableName;
            IsDefaulted = defaultObjectId > 0;
            IsComputed = isComputed;
            //_ordinalPosition = ordinalPosition;
            SqlDataType = dataType;
            CharacterMaximumLength = characterMaximumLengthInt.ToString(CultureInfo.InvariantCulture);

            DataType = GetDataType(SqlDataType);

            DbDataType = GetDbDataType(DataType);

            DefaultPropertyValue = GetDefaultPropertyValue(SqlDataType, string.Empty);

            NewGuidPropertyValue = GetDefaultPropertyValue(SqlDataType, "newguid");

            IsGuidField = SqlDataType.Equals("uniqueidentifier", StringComparison.InvariantCultureIgnoreCase);
        }

        [NotNull]
        private static string GetDataType(string dataType)
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

        [NotNull]
        private string GetDbDataType(string dataType)
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

            dataType = dataType.Substring(0, 1).ToUpper() + dataType.Substring(1, dataType.Length - 1);

            return dataType;
        }

        [NotNull]
        private static string GetDefaultPropertyValue(string dataType, string newValue)
        {
            var defaultPropertyValuelinePart = string.Empty;

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
}
