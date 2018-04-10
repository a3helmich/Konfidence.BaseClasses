using System;
using System.Globalization;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public class ColumnDataItem : SchemaBaseDataItem, IColumnDataItem
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

        #region properties

        public bool IsPrimaryKey { get; private set; }

        public bool IsAutoUpdated { get; private set; }

        public bool IsDefaulted { get; private set; }

        public bool IsComputed { get; private set; }

        public bool IsLockInfo { get; private set; }

        public string Name { get; private set; } = string.Empty;

        protected int OrdinalPosition { get; private set; }

        public string SqlDataType { get; private set; } = string.Empty;

        public string DataType => GetDataType(SqlDataType);

        public string DbDataType => GetDbDataType();

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
        #endregion properties

        public ColumnDataItem()
        {
            IsPrimaryKey = false;
            IsAutoUpdated = false;
            IsDefaulted = false;
            IsComputed = false;
            IsLockInfo = false;
        }

        // TODO : internal
        public override void GetData()
        {
            GetField("Name", out string name);

            GetField("Default_object_id", out int defaultObjectId);

            var isDefaulted = defaultObjectId > 0;

            GetField("Is_Computed", out bool isComputed);

            GetField("column_id", out int ordinalPosition);

            GetField("datatype", out string dataType);

            GetField("max_length", out short characterMaximumLengthInt);

            SetColumnData(name, isDefaulted, isComputed, ordinalPosition, dataType, characterMaximumLengthInt);
        }

        public void SetColumnData(string name, bool isDefaulted, bool isComputed, int ordinalPosition, string dataType, Int16 characterMaximumLengthInt)
        {
            Name = name;
            IsDefaulted = isDefaulted;
            IsComputed = isComputed;
            OrdinalPosition = ordinalPosition;
            SqlDataType = dataType;
            CharacterMaximumLength = characterMaximumLengthInt.ToString(CultureInfo.InvariantCulture);
        }

        public ColumnDataItem(string name) : this()
        {
            Name = name;
        }

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
                    dataType = "Decimal";
                    break;
            }

            return dataType;
        }

        private string GetDbDataType()
        {
            var dataType = DataType;

            if (dataType.Equals("int", StringComparison.InvariantCultureIgnoreCase))
            {
                dataType += "32";
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

        public string DefaultPropertyValue => GetDefaultPropertyValue(SqlDataType, string.Empty);

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
        
        public string NewGuidPropertyValue => GetDefaultPropertyValue(SqlDataType, "newguid");

        internal bool IsGuidField
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

        private static string GetDefaultPropertyValue(string dataType, string newValue)
        {
            var defaultPropertyValuelinePart = string.Empty;

            switch (dataType)
            {
                case "int":
                    defaultPropertyValuelinePart = " = 0";
                    break;
                case "bit":
                    defaultPropertyValuelinePart = " = false";
                    break;
                case "varchar":
                case "char":
                case "nvarchar":
                case "text":
                case "nchar":
                    defaultPropertyValuelinePart = " = string.Empty";
                    break;
                case "uniqueidentifier":
                    if (newValue.Equals("newguid", StringComparison.InvariantCultureIgnoreCase))
                    {
                        defaultPropertyValuelinePart = " = Guid.NewGuid()";
                    }
                    else
                    {
                        defaultPropertyValuelinePart = " = Guid.Empty";
                    }
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
