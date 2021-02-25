using System.Collections.Generic;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public interface IDatabaseStructure
    {
        List<ITableDataItem> Tables { get; }

        void BuildStructure();
    }
}
