using System;
using System.Collections.Generic;
using System.Text;
using Konfidence.BaseData.Schema.Helper;

namespace Konfidence.BaseData.Schema
{
    public class TableDataItem:  SchemaBaseDataItem
    {
        private string _Catalog = string.Empty;
        private string _Schema = string.Empty;
        private string _Name = string.Empty;
        private string _Type = "Table";

        private ColumnDataItemList _ColumnDataItemList = null;
        private IndexColumnsDataItemList _IndexColumnsDataItemList = null;
        private List<TableGetByFieldItem> _TableGetByItemList = new List<TableGetByFieldItem>();
        private List<TableGetByFieldItem> _TableFindByItemList = new List<TableGetByFieldItem>();
        private List<string> _GeneratedCsFiles = new List<string>();
        private List<string> _GeneratedSqlFiles = new List<string>();

        #region properties
        public List<TableGetByFieldItem> TableGetByItemList
        {
            get { return _TableGetByItemList; }
        }

        public List<TableGetByFieldItem> TableFindByItemList
        {
            get { return _TableFindByItemList; }
        }

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
            get { return _Type; }
        }

        public ColumnDataItemList ColumnDataItemList
        {
            get { return _ColumnDataItemList; }
            set { _ColumnDataItemList = value; }
        }

        public string PrimaryKey
        {
            get { return _IndexColumnsDataItemList.PrimaryKeyDataItem.ColumnName; }
        }

        public string PrimaryKeyDataType
        {
            get { return _IndexColumnsDataItemList.PrimaryKeyDataItem.DataType; }
        }

        public List<string> GeneratedCsFileList
        {
            get { return _GeneratedCsFiles; }
        }

        public List<string> GeneratedSqlFiles
        {
            get { return _GeneratedSqlFiles; }
        }
        #endregion properties

        public TableDataItem()
        {
        }

        public TableDataItem(string catalog, string schema, string name) : this()
        {
            _Catalog = catalog;
            _Schema = schema;
            _Name = name;

            _ColumnDataItemList = new ColumnDataItemList(name);
            _IndexColumnsDataItemList = new IndexColumnsDataItemList(name);

            // find out which column is the primaryKey
            foreach (ColumnDataItem columnDataItem in _ColumnDataItemList)
            {
                if (columnDataItem.Name.Equals(PrimaryKey))
                {
                    _IndexColumnsDataItemList.PrimaryKeyDataItem.DataType = columnDataItem.DataType;

                    columnDataItem.IsPrimaryKey = true;

                    break;
                }
            }

            // TODO : figure out which columns have an update trigger
            // find out which column is autoUpdated
            foreach (ColumnDataItem columnDataItem in _ColumnDataItemList)
            {
                switch (columnDataItem.Name.ToLower())
                {
                    case "changed":
                    case "changetime":
                        columnDataItem.IsAutoUpdated = true;
                        break;
                }
            }

            // TODO : figure out which columns have a default value
            // find out which column is defaulted
            foreach (ColumnDataItem columnDataItem in _ColumnDataItemList)
            {
                switch (columnDataItem.Name.ToLower())
                {
                    case "created":
                    case "changetime":
                    case "createtime":
                        columnDataItem.IsDefaulted = true;
                        break;
                }
            }
        }

        public void AddGetByItemList(List<TableGetByFieldItem> tableGetByItemList)
        {
            _TableGetByItemList = tableGetByItemList;
        }

        public void AddFindByItemList(List<TableGetByFieldItem> tableFindByItemList)
        {
            _TableFindByItemList = tableFindByItemList;
        }
    }
}
