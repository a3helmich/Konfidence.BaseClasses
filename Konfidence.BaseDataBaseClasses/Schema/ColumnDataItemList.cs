using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Konfidence.BaseData.Schema
{
    public class ColumnDataItemList : BaseDataItemList<ColumnDataItem>
    {
        private DataTable _DataTable;
        private string _TableName;

        public ColumnDataItemList(string tableName)
        {
            _TableName = tableName;

            BuildItemList(_DataTable);
        }

        protected override void InitializeDataItemList()
        {
            ColumnDataItem columnDataItem = GetNewDataItem();

            _DataTable = columnDataItem.GetSchemaObject("Columns");
        }

        public ColumnDataItem Find(string columnName)
        {
            foreach (ColumnDataItem columnDataItem in this)
            {
                if (columnDataItem.Name.Equals(columnName))
                {
                    return columnDataItem;
                }
            }

            return null;
        }

        private void BuildItemList(DataTable dataTable)
        {
            foreach (DataRow dataRow in dataTable.Rows)
            {
                string tableName = dataRow["TABLE_NAME"] as string;
                ColumnDataItem columnDataItem = null;

                if (tableName.Equals(_TableName))
                {
                    string columnName = dataRow["COLUMN_NAME"] as string;

                    columnDataItem = new ColumnDataItem(columnName);

                    int ordinalPosition = (int)dataRow["ORDINAL_POSITION"];
                    string columnDefault = dataRow["COLUMN_DEFAULT"] as string;
                    string dataType = dataRow["DATA_TYPE"] as string;
                    string characterMaximumLength = string.Empty;
                    if (dataType.Equals("varchar") || dataType.Equals("char"))
                    {
                        int charLength = (int)dataRow["CHARACTER_MAXIMUM_LENGTH"];
                        characterMaximumLength = charLength.ToString();
                    }

                    columnDataItem.OrdinalPosition = ordinalPosition;
                    columnDataItem.ColumnDefault = columnDefault;
                    columnDataItem.DataType = dataType;
                    columnDataItem.CharacterMaximumLength = characterMaximumLength;

                    /*
                    bool isNullable = (bool)dataRow["IS_NULLABLE"];
                    int characterMaximumLength = (int)dataRow["CHARACTER_MAXIMUM_LENGTH"];
                    int characterOctetLength = (int)dataRow["CHARACTER_OCTET_LENGTH"];
                    int numericPrecision = (int)dataRow["NUMERIC_PRECISION"];
                    int numericPrecisionRadix = (int)dataRow["NUMERIC_PRECISION_RADIX"];
                    int numericScale = (int)dataRow["NUMERIC_SCALE"];
                    int dateTimePrecision = (int)dataRow["DATETIME_PRECISION"];
                    string characterSetCatalog = dataRow["CHARACTER_SET_CATALOG"] as string;
                    string characterSetSchema = dataRow["CHARACTER_SET_SCHEMA"] as string;
                    string characterSetName = dataRow["CHARACTER_SET_NAME"] as string;
                    string collationCatalog = dataRow["COLLATION_CATALOG"] as string;

                    columnDataItem.IsNullable = isNullable;
                    columnDataItem.CharacterOctetLength = characterOctetLength;
                    columnDataItem.NumericPrecision = numericPrecision;
                    columnDataItem.NumericPrecisionRadix = numericPrecisionRadix;
                    columnDataItem.DateTimePrecision = dateTimePrecision;
                    columnDataItem.CharacterSetCatalog = characterSetCatalog;
                    columnDataItem.CharacterSetSchema = characterSetSchema;
                    columnDataItem.CharacterSetName = characterSetName;
                    columnDataItem.CollationCatalog = collationCatalog;
                    */

                    this.Add(columnDataItem);
                }
            }
        }

        protected override ColumnDataItem GetNewDataItem()
        {
            return new ColumnDataItem();
        }

    }
}
