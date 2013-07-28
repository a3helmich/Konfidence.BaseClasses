using System;

namespace Konfidence.BaseData.SqlDbSchema
{
    public class TableDataItem : SchemaBaseDataItem, ITableDataItem
    {
        private readonly string _Catalog = string.Empty;
        private readonly string _Schema = string.Empty;
        private readonly string _Name = string.Empty;
        private const string TYPE = "Table";

        private readonly ColumnDataItemList _ColumnDataItemList;
        private readonly IndexColumnsDataItemList _IndexColumnsDataItemList;
        private readonly bool _HasGuidId;

        #region properties

        public string Catalog
        {
            get { return _Catalog; }
        }

        public string Schema
        {
            get { return _Schema; }
        }

        public string Name
        {
            get { return _Name; }
        }

        public string Type
        {
            get { return TYPE; }
        }

        public IColumnDataItemList ColumnDataItemList
        {
            get { return _ColumnDataItemList; }
        }

        public string PrimaryKey
        {
            get { return _IndexColumnsDataItemList.PrimaryKeyDataItem.ColumnName; }
        }

        public string PrimaryKeyDataType
        {
            get { return _IndexColumnsDataItemList.PrimaryKeyDataItem.DataType; }
        }

        public bool HasGuidId
        {
            get { return _HasGuidId; }
        }
        #endregion properties

        //public TableDataItem()
        //{
        //}

        public TableDataItem(string catalog, string schema, string name)
        {
            _Catalog = catalog;
            _Schema = schema;
            _Name = name;

            _ColumnDataItemList = SqlDbSchema.ColumnDataItemList.GetList(name);
            _IndexColumnsDataItemList = new IndexColumnsDataItemList(name);

            // find out which column is the primaryKey
            foreach (var columnDataItem in _ColumnDataItemList)
            {
                if (columnDataItem.Name.Equals(PrimaryKey, StringComparison.InvariantCultureIgnoreCase))
                {
                    _IndexColumnsDataItemList.PrimaryKeyDataItem.DataType = columnDataItem.DataType;

                    columnDataItem.SetPrimaryKey(true);

                    break;
                }
            }

            _HasGuidId = false;
            // find out if te guidId exists for this tbale
            foreach (var columnDataItem in _ColumnDataItemList)
            {
                if (columnDataItem.IsGuidField)
                {
                    if (columnDataItem.Name.Equals(Name + "Id", StringComparison.InvariantCultureIgnoreCase))
                    {
                        _HasGuidId = true;

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

            foreach (var columnDataItem in _ColumnDataItemList)
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
            foreach (var columnDataItem in _ColumnDataItemList)
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
