using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public static class ColumnDataExtensions
    {
        [NotNull]
        public static string GetFieldNames([NotNull] this List<IColumnDataItem> columnDataItems, [NotNull] List<string> fieldNameList)
        {
            var fieldNames = columnDataItems
                .Where(columnDataItem => fieldNameList.Any(fieldName => fieldName.Equals(columnDataItem.Name, StringComparison.OrdinalIgnoreCase)))
                .Select(columnDataItem => columnDataItem.Name);

            return string.Join("", fieldNames);
        }

        [NotNull]
        public static string GetUnderscoreFieldNames([NotNull] this List<IColumnDataItem> columnDataItems, [NotNull] List<string> fieldNameList)
        {
            var fieldNames = columnDataItems
                .Where(columnDataItem => fieldNameList.Any(fieldName => fieldName.Equals(columnDataItem.Name, StringComparison.OrdinalIgnoreCase)))
                .Select(columnDataItem => columnDataItem.Name);

            return string.Join("_", fieldNames).ToUpperInvariant();
        }

        [NotNull]
        public static string GetCommaFieldNames([NotNull] this List<IColumnDataItem> columnDataItems, [NotNull] List<string> fieldNameList)
        {
            var fieldNames = columnDataItems
                .Where(columnDataItem => fieldNameList.Any(fieldName => fieldName.Equals(columnDataItem.Name, StringComparison.OrdinalIgnoreCase)))
                .Select(columnDataItem => columnDataItem.Name);

            return string.Join(", ", fieldNames);
        }

        [NotNull]
        public static string GetTypedCommaFieldNames([NotNull] this List<IColumnDataItem> columnDataItems, [NotNull] List<string> fieldNameList)
        {
            var fieldNames = columnDataItems
                .Where(columnDataItem => fieldNameList.Any(fieldName => fieldName.Equals(columnDataItem.Name, StringComparison.OrdinalIgnoreCase)))
                .Select(columnDataItem => $"{columnDataItem.DataType} {columnDataItem.Name.ToLowerInvariant()}");

            return string.Join(", ", fieldNames);
        }

        [CanBeNull]
        [UsedImplicitly]
        public static IColumnDataItem Find([NotNull] this List<IColumnDataItem> columnDataItems, string columnName)
        {
            return columnDataItems.FirstOrDefault(columnDataItem =>
                columnDataItem.Name.Equals(columnName, StringComparison.InvariantCultureIgnoreCase));
        }

        [UsedImplicitly]
        public static bool HasDefaultValueFields([NotNull] this List<IColumnDataItem> columnDataItems)
        {
            return columnDataItems.Any(columnDataItem => 
                columnDataItem.IsAutoUpdated || columnDataItem.IsComputed || columnDataItem.IsDefaulted);
        }
    }
}
