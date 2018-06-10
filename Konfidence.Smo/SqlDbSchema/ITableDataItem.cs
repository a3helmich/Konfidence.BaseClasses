using JetBrains.Annotations;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public interface ITableDataItem
    {
        [UsedImplicitly]
        string Catalog { get; }
        [UsedImplicitly]
        IColumnDataItemList ColumnDataItemList { get; }
        string Name { get; }
        [UsedImplicitly]
        string PrimaryKey { get; }
    }
}

