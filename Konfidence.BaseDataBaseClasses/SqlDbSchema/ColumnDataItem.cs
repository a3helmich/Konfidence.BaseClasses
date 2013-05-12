using System;
using System.Globalization;

namespace Konfidence.BaseData.SqlDbSchema
{
    public class ColumnDataItem : SchemaBaseDataItem, IColumnDataItem
    {
        private bool _IsComputed;

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

        public bool IsPrimaryKey { get; set; }

        public bool IsAutoUpdated { get; set; }

        public bool IsDefaulted { get; set; }

        public bool IsComputed
        {
            get { return _IsComputed; }
            set { _IsComputed = value; }
        }

        public bool IsLockInfo { get; set; }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public int OrdinalPosition
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
            set { _CharacterMaximumLength = value; }
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
            IsPrimaryKey = false;
            IsAutoUpdated = false;
            IsDefaulted = false;
            _IsComputed = false;
            IsLockInfo = false;
        }

        protected internal override void GetData()
        {
            GetField("Name", out _Name);

            int defaultObjectId;

            GetField("Default_object_id", out defaultObjectId);

            if (defaultObjectId > 0)
            {
                IsDefaulted = true;
            }

            GetField("Is_Computed", out _IsComputed);

            GetField("column_id", out _OrdinalPosition);
            //_ColumnDefault = columnDefault;

            GetField("datatype", out _DataType);

            Int16 characterMaximumLengthInt;

            GetField("max_length", out characterMaximumLengthInt);

            _CharacterMaximumLength = characterMaximumLengthInt.ToString(CultureInfo.InvariantCulture);
        }

        public ColumnDataItem(string name)
            : this()
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

        public string NewGuidPropertyValue
        {
            get
            {
                return GetDefaultPropertyValue(_DataType, "newguid");
            }
        }

        public bool IsGuidField
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
