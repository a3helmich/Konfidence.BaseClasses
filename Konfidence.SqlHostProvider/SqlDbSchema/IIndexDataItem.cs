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
