namespace Konfidence.BaseData.SqlDbSchema
{
    public class PrimaryKeyDataItem : SchemaBaseDataItem
    {
        // field definitions

        private string _constraintName = string.Empty;

        public int PrimaryKeyId => Id;

        public string ConstraintName
        {
            get { return _constraintName; }
            set { _constraintName = value; }
        }

        public string ColumnName { get; set; } = string.Empty;

        public string DataType { get; set; } = string.Empty;


        public PrimaryKeyDataItem()
        {
        }

        public PrimaryKeyDataItem(string databaseName, string tableName)
            : this()
        {
            DatabaseName = databaseName;

            SetParameter(SqlConstant.TableName, tableName);

            GetItem(SpName.PrimarykeyGet);
        }

        protected internal override void InitializeDataItem()
        {
            AutoIdField = SqlConstant.PrimaryKeyId;
        }

        protected internal override void GetData()
        {
            GetField(SqlConstant.ConstraintNameField, out _constraintName);
        }
    }
}
