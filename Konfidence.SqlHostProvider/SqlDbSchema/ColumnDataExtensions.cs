using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public static class ColumnDataExtensions
    {
        extension(List<IColumnDataItem> columnDataItems)
        {
            public string GetJoinedFieldNames(List<string> fieldNameList)
            {
                IEnumerable<string> fieldNames = columnDataItems
                    .Where(columnDataItem => fieldNameList.Any(fieldName => fieldName.Equals(columnDataItem.Name, StringComparison.OrdinalIgnoreCase)))
                    .Select(columnDataItem => columnDataItem.Name);

                return string.Join("", fieldNames);
            }

            public string GetJoinedUnderscoreFieldNames(List<string> fieldNameList)
            {
                IEnumerable<string> fieldNames = columnDataItems
                    .Where(columnDataItem => fieldNameList.Any(fieldName => fieldName.Equals(columnDataItem.Name, StringComparison.OrdinalIgnoreCase)))
                    .Select(columnDataItem => columnDataItem.Name);

                return string.Join("_", fieldNames).ToUpperInvariant();
            }

            public string GetFieldNamesAsArguments(List<string> fieldNameList)
            {
                IEnumerable<string> fieldNames = columnDataItems
                    .Where(columnDataItem => fieldNameList.Any(fieldName => fieldName.Equals(columnDataItem.Name, StringComparison.OrdinalIgnoreCase)))
                    .Select(columnDataItem => columnDataItem.Name);

                return string.Join(", ", fieldNames);
            }

            public string GetFieldNamesAsParameters(List<string> fieldNameList)
            {
                IEnumerable<string> fieldNames = columnDataItems
                    .Where(columnDataItem => fieldNameList.Any(fieldName => fieldName.Equals(columnDataItem.Name, StringComparison.OrdinalIgnoreCase)))
                    .Select(columnDataItem => $"{columnDataItem.DataType} {columnDataItem.Name.ToLowerInvariant()}");

                return string.Join(", ", fieldNames);
            }

            [UsedImplicitly]
            public IColumnDataItem? Find(string columnName)
            {
                return columnDataItems.FirstOrDefault(columnDataItem =>
                    columnDataItem.Name.Equals(columnName, StringComparison.InvariantCultureIgnoreCase));
            }

            [UsedImplicitly]
            public bool HasDefaultValueFields()
            {
                return columnDataItems.Any(columnDataItem =>
                    columnDataItem.IsAutoUpdated || columnDataItem.IsComputed || columnDataItem.IsDefaulted);
            }
        }
    }
}
