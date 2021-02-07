using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using JetBrains.Annotations;
using Konfidence.BaseData;
using Konfidence.DataBaseInterface;
using Konfidence.SqlHostProvider.SqlAccess;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public class ColumnDataItem : BaseDataItem, IColumnDataItem
    {
        //private string _ColumnDefault = string.Empty;

        //private bool _IsNullable = false;
        //private int _CharacterOctetLength = 0;
        //private int _NumericPrecision = 0;
        //private int _NumericPrecisionRadix = 0;
        //private int _NumericScale = 0;
        //private int _DateTimePrecision = 0;
        //private string _CharacterSetCatalog = string.Empty;
        //private string _CharacterSetSchema = string.Empty;
        //private string _CharacterSetName = string.Empty;
        //private string _CollationCatalog = string.Empty;

        public bool IsPrimaryKey { get; private set; }

        public bool IsAutoUpdated { get; private set; }

        public bool IsDefaulted { get; private set; }

        public bool IsComputed { get; private set; }

        public bool IsLockInfo { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public string TableName { get; private set; } = string.Empty;

        protected int OrdinalPosition { get; private set; }

        public string SqlDataType { get; private set; } = string.Empty;

        [NotNull] public string DataType => GetDataType(SqlDataType);

        [NotNull] public string DbDataType => GetDbDataType();

        //public string ColumnDefault
        //{
        //    get { return _ColumnDefault; }
        //    set { _ColumnDefault = value; }
        //}

        //public bool IsNullable
        //{
        //    get { return _IsNullable; }
        //    set { _IsNullable = value; }
        //}

        public string CharacterMaximumLength { get; private set; } = string.Empty;

        //public int CharacterOctetLength
        //{
        //    get { return _CharacterOctetLength; }
        //    set { _CharacterOctetLength = value; }
        //}

        //public int NumericPrecision
        //{
        //    get { return _NumericPrecision; }
        //    set { _NumericPrecision = value; }
        //}

        //public int NumericPrecisionRadix
        //{
        //    get { return _NumericPrecisionRadix; }
        //    set { _NumericPrecisionRadix = value; }
        //}

        //public int NumericScale
        //{
        //    get { return _NumericScale; }
        //    set { _NumericScale = value; }
        //}

        //public int DateTimePrecision
        //{
        //    get { return _DateTimePrecision; }
        //    set { _DateTimePrecision = value; }
        //}

        //public string CharacterSetCatalog
        //{
        //    get { return _CharacterSetCatalog; }
        //    set { _CharacterSetCatalog = value; }
        //}

        //public string CharacterSetSchema
        //{
        //    get { return _CharacterSetSchema; }
        //    set { _CharacterSetSchema = value; }
        //}

        //public string CharacterSetName
        //{
        //    get { return _CharacterSetName; }
        //    set { _CharacterSetName = value; }
        //}

        //public string CollationCatalog
        //{
        //    get { return _CollationCatalog; }
        //    set { _CollationCatalog = value; }
        //}

        //============================================  >>>>>>

        //private void BuildItemList(DataTable dataTable)
        //{
        //    foreach (DataRow dataRow in dataTable.Rows)
        //    {
        //        string tableName = dataRow["TABLE_NAME"] as string;
        //        ColumnDataItem columnDataItem = null;

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

        //            columnDataItem.IsNullable = isNullable;
        //            columnDataItem.CharacterOctetLength = characterOctetLength;
        //            columnDataItem.NumericPrecision = numericPrecision;
        //            columnDataItem.NumericPrecisionRadix = numericPrecisionRadix;
        //            columnDataItem.DateTimePrecision = dateTimePrecision;
        //            columnDataItem.CharacterSetCatalog = characterSetCatalog;
        //            columnDataItem.CharacterSetSchema = characterSetSchema;
        //            columnDataItem.CharacterSetName = characterSetName;
        //            columnDataItem.CollationCatalog = collationCatalog;
        //            */

        //            this.Add(columnDataItem);
        //        }
        //    }
        //}
        //============================================   <<<<<<

        public ColumnDataItem()
        {
            IsPrimaryKey = false;
            IsAutoUpdated = false;
            IsDefaulted = false;
            IsComputed = false;
            IsLockInfo = false;
        }

        [NotNull]
        public static List<ColumnDataItem> GetList([NotNull] IBaseClient client)
        {
            var columnDataItems = new List<ColumnDataItem>();

            var spParameterData = new List<ISpParameterData>();

            client.BuildItemList(columnDataItems, SpName.GetColumnList, spParameterData);

            return columnDataItems;
        }

        [NotNull]
        protected override IBaseClient ClientBind()
        {
            return base.ClientBind<SqlClient>();
        }

        // TODO : internal
        public override void GetData(IDataReader dataReader)
        {
            GetField("Name", dataReader, out string name);

            GetField("tableName", dataReader, out string tableName);

            GetField("Default_object_id", dataReader, out int defaultObjectId);

            var isDefaulted = defaultObjectId > 0;

            GetField("Is_Computed", dataReader, out bool isComputed);

            GetField("column_id", dataReader, out int ordinalPosition);

            GetField("datatype", dataReader, out string dataType);

            GetField("max_length", dataReader, out short characterMaximumLengthInt);
            

            SetColumnData(name, tableName, isDefaulted, isComputed, ordinalPosition, dataType, characterMaximumLengthInt);
        }

        public void SetColumnData(string name, string tableName, bool isDefaulted, bool isComputed, int ordinalPosition, string dataType, short characterMaximumLengthInt)
        {
            Name = name;
            TableName = tableName;
            IsDefaulted = isDefaulted;
            IsComputed = isComputed;
            OrdinalPosition = ordinalPosition;
            SqlDataType = dataType;
            CharacterMaximumLength = characterMaximumLengthInt.ToString(CultureInfo.InvariantCulture);
        }

        [UsedImplicitly]
        public ColumnDataItem(string name) : this()
        {
            Name = name;
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
        private string GetDbDataType()
        {
            var dataType = DataType;

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

        [NotNull] public string DefaultPropertyValue => GetDefaultPropertyValue(SqlDataType, string.Empty);

        public void SetPrimaryKey(bool isPrimaryKey)
        {
            IsPrimaryKey = isPrimaryKey;
        }

        public void SetAutoUpdated(bool isAutoUpdated)
        {
            IsAutoUpdated = isAutoUpdated;
        }

        public void SetLockInfo(bool isLockInfo)
        {
            IsLockInfo = isLockInfo;
        }

        [UsedImplicitly] [NotNull] public string NewGuidPropertyValue => GetDefaultPropertyValue(SqlDataType, "newguid");

        public bool IsGuidField
        {
            get
            {
                if (SqlDataType.Equals("uniqueidentifier", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }

                return false;
            }
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
