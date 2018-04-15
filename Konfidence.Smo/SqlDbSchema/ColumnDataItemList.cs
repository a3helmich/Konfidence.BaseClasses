using System;
using System.Collections.Generic;
using Konfidence.Base;
using Konfidence.BaseData;
using Konfidence.BaseDataInterfaces;
using Konfidence.SqlHostProvider.SqlAccess;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public class ColumnDataItemList : BaseDataItemList<ColumnDataItem>, IColumnDataItemList
    {
        private readonly string _tableName;

        private bool _hasDefaultValueFields;
        private bool _hasDefaultValueFieldsChecked;

        public bool HasDefaultValueFields
        {
            get
            {
                if (!_hasDefaultValueFieldsChecked)
                {
                    foreach (var columnItem in this)
                    {
                        if (columnItem.IsAutoUpdated || columnItem.IsComputed || columnItem.IsDefaulted)
                        {
                            _hasDefaultValueFields = true;

                            break;
                        }
                    }

                    _hasDefaultValueFieldsChecked = true;
                }

                return _hasDefaultValueFields;
            }
        }

        public ColumnDataItemList(string tableName, IEnumerable<ColumnDataItem> columnDataItems) : this(tableName)
        {
            AddRange(columnDataItems);
        }
        
        protected ColumnDataItemList(string tableName)
        {
            _tableName = tableName;

            _hasDefaultValueFields = false;
            _hasDefaultValueFieldsChecked = false;
        }

        protected override IBaseClient ClientBind()
        {
            return base.ClientBind<SqlClient>();
        }

        public static ColumnDataItemList GetList(string databaseName, string tableName)
        {
            var columnDataItemList = new ColumnDataItemList(tableName) {ConnectionName = databaseName};

            columnDataItemList.BuildItemList(SpName.ColumnsGetlist);

            return columnDataItemList;
        }

        public override void SetParameters(string storedProcedure)
        {

            if (storedProcedure.Equals(SpName.ColumnsGetlist))
            {
                SetParameter("TableName", _tableName);
            }
        }

        public IColumnDataItem Find(string columnName)
        {
            foreach (var columnDataItem in this)
            {
                if (columnDataItem.Name.Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return columnDataItem;
                }
            }

            return null;
        }

        // construct an identifier for the Columns by appending all Names 
        // ie: Column1 & Column2 -> Column1Column2 for GetByColumn1Column2
        public string GetFieldNames(List<string> getListByFieldList)
        {
            var fieldNames = string.Empty;

            foreach (var getListByField in getListByFieldList)
            {
                var columnDataItem = Find(getListByField);

                if (columnDataItem.IsAssigned())
                {
                    fieldNames += columnDataItem.Name;
                }
            }

            return fieldNames;
        }

        public string GetUnderscoreFieldNames(List<string> getListByFieldList)
        {
            var fieldNames = string.Empty;
            var delimiter = string.Empty;

            foreach (var getListByField in getListByFieldList)
            {
                var columnDataItem = Find(getListByField);

                if (columnDataItem.IsAssigned())
                {
                    fieldNames += delimiter + columnDataItem.Name;

                    delimiter = "_";
                }
            }

            return fieldNames.ToUpper();
        }

        public string GetTypedCommaFieldNames(List<string> getListByFieldList)
        {
            var commaFieldNames = string.Empty;
            var delimiter = string.Empty;

            foreach (var getListByField in getListByFieldList)
            {
                var columnDataItem = Find(getListByField);

                if (columnDataItem.IsAssigned())
                {
                    commaFieldNames += delimiter + columnDataItem.DataType + " " + columnDataItem.Name.ToLower();

                    delimiter = ", ";
                }
            }

            return commaFieldNames;
        }

        public string GetCommaFieldNames(List<string> getListByFieldList)
        {
            var commaFieldNames = string.Empty;
            var delimiter = string.Empty;

            foreach (var getListByField in getListByFieldList)
            {
                var columnDataItem = Find(getListByField);

                if (columnDataItem.IsAssigned())
                {
                    commaFieldNames += delimiter + columnDataItem.Name;

                    delimiter = ", ";
                }
            }

            return commaFieldNames;
        }

        public string GetFirstField(List<string> getListByFieldList)
        {
            if (getListByFieldList.Count > 0)
            {
                return getListByFieldList[0];
            }

            return string.Empty;
        }

        public string GetLastField(List<string> findByFieldList)
        {
            if (findByFieldList.Count > 0)
            {
                return findByFieldList[findByFieldList.Count - 1];
            }

            return string.Empty;
        }

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

        //            int ordinalPosition = (int)dataRow["ORDINAL_POSITION"];
        //            string columnDefault = dataRow["COLUMN_DEFAULT"] as string;
        //            string dataType = dataRow["DATA_TYPE"] as string;
        //            string characterMaximumLength = string.Empty;
        //            if (dataType.Equals("varchar") || dataType.Equals("char"))
        //            {
        //                int charLength = (int)dataRow["CHARACTER_MAXIMUM_LENGTH"];
        //                characterMaximumLength = charLength.ToString();
        //            }

        //            columnDataItem.OrdinalPosition = ordinalPosition;
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
    }
}
