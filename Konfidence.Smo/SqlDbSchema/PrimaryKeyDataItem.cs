namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public class PrimaryKeyDataItem : SchemaBaseDataItem
    {
        // field definitions

        private string _constraintName = string.Empty;

        public int PrimaryKeyId => Id;

        public string ConstraintName
        {
            get => _constraintName;
            set => _constraintName = value;
        }

        public string ColumnName { get; set; } = string.Empty;

        public string DataType { get; set; } = string.Empty;


        public PrimaryKeyDataItem()
        {
        }

        public PrimaryKeyDataItem(string databaseName, string tableName)
            : this()
        {
            ConnectionName = databaseName;

            SetParameter(SqlConstant.TableName, tableName);

            GetItem(SpName.PrimarykeyGet);
        }

        // TODO: internal
        public override void InitializeDataItem()
        {
            AutoIdField = SqlConstant.PrimaryKeyId;
        }

        // TODO: internal
        public override void GetData()
        {
            GetField(SqlConstant.ConstraintNameField, out _constraintName);
        }
    }
}
