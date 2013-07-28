using System;
using System.Globalization;

namespace Konfidence.BaseData.SqlDbSchema
{
    public class ColumnDataItem : SchemaBaseDataItem, IColumnDataItem
    {
        private bool _IsComputed;
        private bool _IsDefaulted;
        private bool _IsPrimaryKey;
        private bool _IsAutoUpdated;
        private bool _IsLockInfo;

        private string _Name = string.Empty;
        private int _OrdinalPosition;
        private string _DataType = string.Empty;
        //private string _ColumnDefault = string.Empty;

        //private bool _IsNullable = false;
        private string _CharacterMaximumLength = string.Empty;
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

        public bool IsPrimaryKey
        {
            get { return _IsPrimaryKey; }
        }

        public bool IsAutoUpdated
        {
            get { return _IsAutoUpdated; }
        }

        public bool IsDefaulted
        {
            get { return _IsDefaulted; }
        }

        public bool IsComputed
        {
            get { return _IsComputed; }
            //set { _IsComputed = value; }
        }

        public bool IsLockInfo
        {
            get { return _IsLockInfo; }
        }

        public string Name
        {
            get { return _Name; }
        }

        protected int OrdinalPosition
        {
            get { return _OrdinalPosition; }
            set { _OrdinalPosition = value; }
        }

        public string SqlDataType
        {
            get { return _DataType; }
        }

        public string DataType
        {
            get { return GetDataType(_DataType); }
        }

        public string DbDataType
        {
            get { return GetDbDataType(); }
        }

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

        public string CharacterMaximumLength
        {
            get { return _CharacterMaximumLength; }
        }

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
            _IsPrimaryKey = false;
            _IsAutoUpdated = false;
            _IsDefaulted = false;
            _IsComputed = false;
            _IsLockInfo = false;
        }

        protected internal override void GetData()
        {
            string name;
            GetField("Name", out name);

            int defaultObjectId;
            GetField("Default_object_id", out defaultObjectId);

            bool isDefaulted = defaultObjectId > 0;

            bool isComputed;
            GetField("Is_Computed", out isComputed);

            int ordinalPosition;
            GetField("column_id", out ordinalPosition);

            string dataType;
            GetField("datatype", out dataType);

            Int16 characterMaximumLengthInt;
            GetField("max_length", out characterMaximumLengthInt);

            SetColumnData(name, isDefaulted, isComputed, ordinalPosition, dataType, characterMaximumLengthInt);
        }

        public void SetColumnData(string name, bool isDefaulted, bool isComputed, int ordinalPosition, string dataType, Int16 characterMaximumLengthInt)
        {
            _Name = name;
            _IsDefaulted = isDefaulted;
            _IsComputed = isComputed;
            _OrdinalPosition = ordinalPosition;
            _DataType = dataType;
            _CharacterMaximumLength = characterMaximumLengthInt.ToString(CultureInfo.InvariantCulture);
        }

        public ColumnDataItem(string name) : this()
        {
            _Name = name;
        }

        private string GetDataType(string dataType)
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

        public string DefaultPropertyValue
        {
            get
            {
                return GetDefaultPropertyValue(_DataType, string.Empty);
            }
        }

        internal void SetPrimaryKey(bool isPrimaryKey)
        {
            _IsPrimaryKey = isPrimaryKey;
        }

        internal void SetAutoUpdated(bool isAutoUpdated)
        {
            _IsAutoUpdated = isAutoUpdated;
        }

        internal void SetLockInfo(bool isLockInfo)
        {
            _IsLockInfo = isLockInfo;
        }
        
        public string NewGuidPropertyValue
        {
            get
            {
                return GetDefaultPropertyValue(_DataType, "newguid");
            }
        }

        internal bool IsGuidField
        {
            get
            {
                if (_DataType.Equals("uniqueidentifier", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }

                return false;
            }
        }

        private string GetDefaultPropertyValue(string dataType, string newValue)
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
