using System.Collections.Generic;
using System.Data;
using System.Linq;
using JetBrains.Annotations;
using Konfidence.BaseData;
using Konfidence.DataBaseInterface;
using Konfidence.SqlHostProvider.SqlAccess;

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

        [NotNull]
        internal static List<IIndexDataItem> GetList([NotNull] IBaseClient client, List<IPrimaryKeyDataItem> allPrimaryKeyDataItems)
        {
            var indexDataItems = new List<IIndexDataItem>();

            var schemaIndexes = client
                .GetSchemaObject("IndexColumns")
                .AsEnumerable()
                .ToList();

            indexDataItems.AddRange(MapSchemaIndexesToIndexDataItems(schemaIndexes, allPrimaryKeyDataItems));

            return indexDataItems;
        }

        [NotNull]
        private static List<IIndexDataItem> MapSchemaIndexesToIndexDataItems([NotNull] List<DataRow> schemaIndexes, List<IPrimaryKeyDataItem> allPrimaryKeyDataItems)
        {
            return schemaIndexes.Select(indexDataRow => BuildIndexDataItem(indexDataRow, allPrimaryKeyDataItems)).ToList();
        }

        [NotNull]
        private static IIndexDataItem BuildIndexDataItem([NotNull] DataRow indexDataRow, [NotNull] List<IPrimaryKeyDataItem> allPrimaryKeyDataItems)
        {
            var catalog = indexDataRow["TABLE_CATALOG"].ToString();

            var tableName = indexDataRow["TABLE_NAME"].ToString();

            var contraintName = indexDataRow["CONSTRAINT_NAME"].ToString();

            var indexName = indexDataRow["COLUMN_NAME"].ToString();

            var isPrimaryKey = allPrimaryKeyDataItems.Any(primaryKeyDataItems => primaryKeyDataItems.ConstraintName == contraintName);

            return new IndexDataItem(catalog, tableName, indexName, contraintName, isPrimaryKey);
        }
    }
}
