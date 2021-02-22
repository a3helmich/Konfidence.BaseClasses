using System.Collections.Generic;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public interface IDatabaseStructure
    {
        string SelectedConnectionName { get; }

        List<ITableDataItem> Tables { get; }

        void BuildStructure();
    }
}
