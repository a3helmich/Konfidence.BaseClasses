using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public static class ColumnDataExtensions
    {
        public static string GetJoinedFieldNames(this List<IColumnDataItem> columnDataItems, List<string> fieldNameList)
        {
            var fieldNames = columnDataItems
                .Where(columnDataItem => fieldNameList.Any(fieldName => fieldName.Equals(columnDataItem.Name, StringComparison.OrdinalIgnoreCase)))
                .Select(columnDataItem => columnDataItem.Name);

            return string.Join("", fieldNames);
        }

        public static string GetJoinedUnderscoreFieldNames(this List<IColumnDataItem> columnDataItems, List<string> fieldNameList)
        {
            var fieldNames = columnDataItems
                .Where(columnDataItem => fieldNameList.Any(fieldName => fieldName.Equals(columnDataItem.Name, StringComparison.OrdinalIgnoreCase)))
                .Select(columnDataItem => columnDataItem.Name);

            return string.Join("_", fieldNames).ToUpperInvariant();
        }

        public static string GetFieldNamesAsArguments(this List<IColumnDataItem> columnDataItems, List<string> fieldNameList)
        {
            var fieldNames = columnDataItems
                .Where(columnDataItem => fieldNameList.Any(fieldName => fieldName.Equals(columnDataItem.Name, StringComparison.OrdinalIgnoreCase)))
                .Select(columnDataItem => columnDataItem.Name);

            return string.Join(", ", fieldNames);
        }

        public static string GetFieldNamesAsParameters(this List<IColumnDataItem> columnDataItems, List<string> fieldNameList)
        {
            var fieldNames = columnDataItems
                .Where(columnDataItem => fieldNameList.Any(fieldName => fieldName.Equals(columnDataItem.Name, StringComparison.OrdinalIgnoreCase)))
                .Select(columnDataItem => $"{columnDataItem.DataType} {columnDataItem.Name.ToLowerInvariant()}");

            return string.Join(", ", fieldNames);
        }

        [UsedImplicitly]
        public static IColumnDataItem? Find(this List<IColumnDataItem> columnDataItems, string columnName)
        {
            return columnDataItems.FirstOrDefault(columnDataItem =>
                columnDataItem.Name.Equals(columnName, StringComparison.InvariantCultureIgnoreCase));
        }

        [UsedImplicitly]
        public static bool HasDefaultValueFields(this List<IColumnDataItem> columnDataItems)
        {
            return columnDataItems.Any(columnDataItem => 
                columnDataItem.IsAutoUpdated || columnDataItem.IsComputed || columnDataItem.IsDefaulted);
        }
    }
}
