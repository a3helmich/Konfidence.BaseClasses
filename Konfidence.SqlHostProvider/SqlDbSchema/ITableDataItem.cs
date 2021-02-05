using JetBrains.Annotations;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public interface ITableDataItem
    {
        [UsedImplicitly]
        string Catalog { get; }
        [UsedImplicitly]
        ColumnDataItemList ColumnDataItemList { get; }
        string Name { get; }
        [UsedImplicitly]
        string PrimaryKey { get; }
    }
}

