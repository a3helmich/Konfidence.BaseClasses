using System;
using System.Collections.Generic;
using System.Text;

namespace Konfidence.BaseData.Schema
{
    public class ColumnDataItem: SchemaBaseDataItem
    {
        private bool _IsPrimaryKey = false;
        private bool _IsAutoUpdated = false;
        private bool _IsDefaulted = false;
        private bool _IsInternal = false;

        private string _Name = string.Empty;
        private int _OrdinalPosition = 0;
        private string _DataType = string.Empty;
        private string _ColumnDefault = string.Empty;

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
        public bool IsDefaulted
        {
            get { return _IsDefaulted; }
            set { _IsDefaulted = value; }
        }

        public bool IsAutoUpdated
        {
            get { return _IsAutoUpdated; }
            set { _IsAutoUpdated = value; }
        }

        public bool IsInternal
        {
            get { return _IsInternal; }
            set { _IsInternal = value; }
        }

        public bool IsPrimaryKey
        {
            get { return _IsPrimaryKey; }
            set { _IsPrimaryKey = value; }
        }

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

        public string DataType
        {
            get { return _DataType; }
            set { _DataType = value; }
        }

        public string ColumnDefault
        {
            get { return _ColumnDefault; }
            set { _ColumnDefault = value; }
        }

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
        }

        public ColumnDataItem(string name): this()
        {
            _Name = name;
        }
    }
}
