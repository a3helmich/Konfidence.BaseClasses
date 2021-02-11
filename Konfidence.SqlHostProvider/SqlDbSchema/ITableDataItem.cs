using System.Collections.Generic;
using JetBrains.Annotations;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public interface ITableDataItem
    {
        [UsedImplicitly]
        string Catalog { get; }
        [UsedImplicitly] 
        string TableType { get; }
        [UsedImplicitly]
        List<IColumnDataItem> ColumnDataItems { get; }
        string Name { get; }
        [UsedImplicitly]
        string PrimaryKey { get; }
        string PrimaryKeyDataType { get; }
        bool HasGuidId { get; }
    }
}

