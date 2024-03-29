﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.BaseData;
using Konfidence.DatabaseInterface;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public class TableDataItem : BaseDataItem, ITableDataItem
    {
        public string Catalog { get; }

        public string Name { get; } 

        public string TableType { get; } 

        public List<IColumnDataItem> ColumnDataItems { get; }

        public string PrimaryKey { get; } 

        public string PrimaryKeyDataType { get; } 

        public bool HasGuidId { get; }

        public TableDataItem(string catalog, string name, List<IColumnDataItem> columnDataItems, string primaryKey, string primaryKeyDataType, string guidIdField, bool hasGuidId)
        {
            Catalog = catalog;
            Name = name;
            TableType = SqlConstant.TableType;

            PrimaryKey = primaryKey;
            PrimaryKeyDataType = primaryKeyDataType;

            HasGuidId = hasGuidId;

            GuidIdField = guidIdField;

            ColumnDataItems = columnDataItems;
        }

        internal static List<ITableDataItem> GetList(IBaseClient client, List<IColumnDataItem> allColumnDataItems)
        {
            var tableDataItems = new List<ITableDataItem>();

            var schemaTables = client
                .GetSchemaObject("Tables")
                .AsEnumerable()
                .Where(dataRow => dataRow["TABLE_TYPE"].Equals("BASE TABLE"))
                .ToList();

            tableDataItems.AddRange(MapSchemaTablesToTableDataItems(schemaTables, allColumnDataItems));

            return tableDataItems;
        }

        private static IEnumerable<TableDataItem> MapSchemaTablesToTableDataItems(IEnumerable<DataRow> schemaTables, List<IColumnDataItem> allColumnDataItems)
        {
            var tableDataItems = schemaTables
                .Select(tableDataRow => BuildTableDataItem(tableDataRow, allColumnDataItems))
                .ToList();

            return tableDataItems.Where(tableDataItem =>
                !tableDataItem.Name.Equals("dtproperties", StringComparison.OrdinalIgnoreCase) && !tableDataItem.Name.StartsWith("sys", StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        private static TableDataItem BuildTableDataItem(DataRow tableDataRow, List<IColumnDataItem> allColumnDataItems)
        {
            string catalog = tableDataRow["TABLE_CATALOG"].ToString() ?? string.Empty;

            var name = tableDataRow["TABLE_NAME"].ToString() ?? string.Empty;

            var columnDataItems = allColumnDataItems.Where(x => x.TableName == name).ToList();

            var indexedColumnDataItem = columnDataItems.FirstOrDefault(columnDataItem => columnDataItem.IsPrimaryKey);

            var primaryKey = indexedColumnDataItem?.Name ?? string.Empty;

            var primaryKeyDataType = indexedColumnDataItem?.DataType ?? string.Empty;

            var guidColumn = columnDataItems
                .Find(columnDataItem => columnDataItem.IsGuidField &&
                                        columnDataItem.Name.Equals($"{name}Id",
                                            StringComparison.InvariantCultureIgnoreCase));

            var hasGuidId = guidColumn.IsAssigned();

            var guidIdField = hasGuidId ? guidColumn?.Name ?? string.Empty : string.Empty;

            return new TableDataItem(catalog, name, columnDataItems, primaryKey, primaryKeyDataType, guidIdField, hasGuidId); 
        }

        // TODO : figure out which columns have an update trigger
        // find out which column is autoUpdated

        //SELECT      Tables.Name TableName,
        //Triggers.name TriggerName,
        //Triggers.crdate TriggerCreatedDate,
        //Comments.Text TriggerText
        //FROM      sysobjects Triggers
        //Inner Join sysobjects Tables On Triggers.parent_obj = Tables.id
        //Inner Join syscomments Comments On Triggers.id = Comments.id
        //WHERE      Triggers.xtype = 'TR'
        //And Tables.xtype = 'U'
        //ORDER BY Tables.Name, Triggers.name

        //SELECT
        //Object_Schema_name(object_id)+'.'+COALESCE(OBJECT_NAME(object_id), 'unknown')
        //+COALESCE('.'+ COL_NAME(object_id, column_id), '') AS [referencer],
        //Object_Schema_name(referenced_major_id)+'.'+OBJECT_NAME(referenced_major_id)
        //+COALESCE('.'+COL_NAME(referenced_major_id, referenced_minor_id), '') AS [Referenced]
        //FROM
        //sys.sql_dependencies
        //WHERE
        //class IN (0, 1) --AND referenced_major_id = OBJECT_ID('HumanResources.Employee')
        //ORDER BY
        //COALESCE(OBJECT_NAME(object_id), 'x'),
        //COALESCE(COL_NAME(object_id, column_id), 'a') 
    }
}
