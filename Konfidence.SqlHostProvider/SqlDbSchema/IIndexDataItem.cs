using System;
using System.Collections.Generic;
using System.Text;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    internal interface IIndexDataItem
    {
        string Catalog { get; }

        string TableName { get; }

        string IndexName { get; }

        string ContraintName { get; }

        bool IsPrimaryKey { get; }
    }
}
