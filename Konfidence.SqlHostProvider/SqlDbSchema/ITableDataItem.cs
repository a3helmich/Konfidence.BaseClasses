using System.Collections.Generic;
using JetBrains.Annotations;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    internal interface ITableDataItem
    {
        [UsedImplicitly]
        string Catalog { get; }
        [UsedImplicitly]
        List<IColumnDataItem> ColumnDataItems { get; }
        string Name { get; }
        [UsedImplicitly]
        string PrimaryKey { get; }
    }
}

