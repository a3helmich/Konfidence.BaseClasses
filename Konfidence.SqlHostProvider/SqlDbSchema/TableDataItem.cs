using System;
using JetBrains.Annotations;
using Konfidence.BaseData;
using Konfidence.DataBaseInterface;
using Konfidence.SqlHostProvider.SqlAccess;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public class TableDataItem : BaseDataItem, ITableDataItem
    {
        private readonly ColumnDataItemList _columnDataItemList;
        private readonly IndexColumnsDataItemProperties _indexColumnsDataItemProperties;

        public string Catalog { get; } = string.Empty;

        //public string Schema { get; } = string.Empty;

        public string Name { get; } = string.Empty;

        [UsedImplicitly] [NotNull] public string TableType => SqlConstant.TableType;

        public IColumnDataItemList ColumnDataItemList => _columnDataItemList;

        public string PrimaryKey => _indexColumnsDataItemProperties.PrimaryKeyDataItem.ColumnName;

        [UsedImplicitly]
        public string PrimaryKeyDataType => _indexColumnsDataItemProperties.PrimaryKeyDataItem.DataType;

        public bool HasGuidId { get; }

        //public TableDataItem()
        //{
        //}
        
        protected override IBaseClient ClientBind()
        {
            return base.ClientBind<SqlClient>();
        }

        public TableDataItem(string connectionName, string catalog, string name)
        {
            ConnectionName = connectionName;
            Catalog = catalog;
            Name = name;

            _columnDataItemList = SqlDbSchema.ColumnDataItemList.GetList(name, ConnectionName);
            _indexColumnsDataItemProperties = new IndexColumnsDataItemProperties(ConnectionName, name);

            // find out which column is the primaryKey
            foreach (var columnDataItem in _columnDataItemList)
            {
                if (columnDataItem.Name.Equals(PrimaryKey, StringComparison.InvariantCultureIgnoreCase))
                {
                    _indexColumnsDataItemProperties.PrimaryKeyDataItem.DataType = columnDataItem.DataType;

                    columnDataItem.SetPrimaryKey(true);

                    break;
                }
            }

            HasGuidId = false;

            // find out if te guidId exists for this table
            foreach (var columnDataItem in _columnDataItemList)
            {
                if (columnDataItem.IsGuidField)
                {
                    if (columnDataItem.Name.Equals(Name + "Id", StringComparison.InvariantCultureIgnoreCase))
                    {
                        HasGuidId = true;

                        break;
                    }
                }
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

            foreach (var columnDataItem in _columnDataItemList)
            {
                switch (columnDataItem.Name.ToLower())
                {
                    case "sysinserttime":
                        break;
                    case "sysupdatetime":
                        columnDataItem.SetAutoUpdated(true);
                        break;
                }
            }

            // DataItem specific lockinfo
            foreach (var columnDataItem in _columnDataItemList)
            {
                switch (columnDataItem.Name.ToLower())
                {
                    case "syslock":
                        columnDataItem.SetLockInfo(true);
                        break;
                }
            }
        }

        public TableDataItem()
        {
            throw new NotImplementedException();
        }

        //internal System.Data.DataTable GetTables()
        //{
        //    return GetSchemaObject("Tables");
        //}
    }
}
