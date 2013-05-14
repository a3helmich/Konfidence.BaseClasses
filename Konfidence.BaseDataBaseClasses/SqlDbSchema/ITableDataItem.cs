namespace Konfidence.BaseData.SqlDbSchema
{
    public interface ITableDataItem
    {
        string Catalog { get; }
        IColumnDataItemList ColumnDataItemList { get; }
        string Name { get; }
        string PrimaryKey { get; }
    }
}

