﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public static class SqlSchemaExtensions
    {
        // construct an identifier for the Columns by appending all Names 
        // ie: Column1 & Column2 -> Column1Column2 for GetByColumn1Column2
        [NotNull]
        public static string GetFieldNames(this List<IColumnDataItem> columnDataItems, [NotNull] List<string> fieldNameList)
        {
            var concatenatedFieldNames = string.Empty;

            foreach (var fieldName in fieldNameList)
            {
                var columnDataItem = columnDataItems.Find(fieldName);

                if (columnDataItem.IsAssigned())
                {
                    concatenatedFieldNames += columnDataItem.Name;
                }
            }

            return concatenatedFieldNames;
        }

        [NotNull]
        public static string GetUnderscoreFieldNames(this List<IColumnDataItem> columnDataItems, [NotNull] List<string> fieldNameList)
        {
            var fieldNames = string.Empty;
            var delimiter = string.Empty;

            foreach (var getListByField in fieldNameList)
            {
                var columnDataItem = columnDataItems.Find(getListByField);

                if (columnDataItem.IsAssigned())
                {
                    fieldNames += delimiter + columnDataItem.Name;

                    delimiter = "_";
                }
            }

            return fieldNames.ToUpper();
        }

        [NotNull]
        public static string GetCommaFieldNames(this List<IColumnDataItem> columnDataItems, [NotNull] List<string> fieldNameList)
        {
            var commaFieldNames = string.Empty;
            var delimiter = string.Empty;

            foreach (var getListByField in fieldNameList)
            {
                var columnDataItem = columnDataItems.Find(getListByField);

                if (columnDataItem.IsAssigned())
                {
                    commaFieldNames += delimiter + columnDataItem.Name;

                    delimiter = ", ";
                }
            }

            return commaFieldNames;
        }

        [NotNull]
        public static string GetTypedCommaFieldNames(this List<IColumnDataItem> columnDataItems, [NotNull] List<string> fieldNameList)
        {
            var commaFieldNames = string.Empty;
            var delimiter = string.Empty;

            foreach (var getListByField in fieldNameList)
            {
                var columnDataItem = columnDataItems.Find(getListByField);

                if (columnDataItem.IsAssigned())
                {
                    commaFieldNames += delimiter + columnDataItem.DataType + " " + columnDataItem.Name.ToLower();

                    delimiter = ", ";
                }
            }

            return commaFieldNames;
        }

        [CanBeNull]
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