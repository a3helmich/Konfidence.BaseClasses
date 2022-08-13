using System.Collections.Generic;
using System.Data;
using System.Linq;
using JetBrains.Annotations;
using Konfidence.BaseData;
using Konfidence.DatabaseInterface;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    internal class IndexDataItem : BaseDataItem, IIndexDataItem
    {
        public string Catalog { get; }

        public string TableName { get; }

        public string IndexName { get; }

        public string ContraintName { get; }

        public bool IsPrimaryKey { get; }

        private IndexDataItem(string catalog, string tableName, string indexName, string contraintName, bool isPrimaryKey)
        {
            Catalog = catalog;

            TableName = tableName;

            IndexName = indexName;

            ContraintName = contraintName;

            IsPrimaryKey = isPrimaryKey;
        }

        [UsedImplicitly]
        internal static List<IIndexDataItem> GetList(IBaseClient client, List<IPrimaryKeyDataItem> allPrimaryKeyDataItems)
        {
            var indexDataItems = new List<IIndexDataItem>();

            var schemaIndexes = client
                .GetSchemaObject("IndexColumns")
                .AsEnumerable()
                .ToList();

            indexDataItems.AddRange(MapSchemaIndexesToIndexDataItems(schemaIndexes, allPrimaryKeyDataItems));

            return indexDataItems;
        }

        private static List<IIndexDataItem> MapSchemaIndexesToIndexDataItems(List<DataRow> schemaIndexes, List<IPrimaryKeyDataItem> allPrimaryKeyDataItems)
        {
            return schemaIndexes.Select(indexDataRow => BuildIndexDataItem(indexDataRow, allPrimaryKeyDataItems)).ToList();
        }

        private static IIndexDataItem BuildIndexDataItem(DataRow indexDataRow, List<IPrimaryKeyDataItem> allPrimaryKeyDataItems)
        {
            string catalog = indexDataRow["TABLE_CATALOG"].ToString() ?? string.Empty;

            string tableName = indexDataRow["TABLE_NAME"].ToString() ?? string.Empty;

            string contraintName = indexDataRow["CONSTRAINT_NAME"].ToString() ?? string.Empty;

            string indexName = indexDataRow["COLUMN_NAME"].ToString() ?? string.Empty;

            bool isPrimaryKey = allPrimaryKeyDataItems.Any(primaryKeyDataItems => primaryKeyDataItems.ConstraintName == contraintName);

            return new IndexDataItem(catalog, tableName, indexName, contraintName, isPrimaryKey);
        }
    }
}
