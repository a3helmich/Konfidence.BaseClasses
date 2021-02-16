using System;
using System.Collections.Generic;
using System.Text;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public interface IDatabaseStructure
    {
        string SelectedConnectionName { get; }

        List<ITableDataItem> Tables { get; }

        void BuildStructure();
    }
}
