namespace Konfidence.BaseData.SqlDbSchema
{
    public class PrimaryKeyDataItem : SchemaBaseDataItem
    {
        // field definitions
        private const string CONSTRAINT_NAME = "Constraint_Name";
        private const string Primarykeyid = "PrimaryKeyId";

        private const string Tablename = "tableName";

        private string _constraintName = string.Empty;

        #region properties
        public int PrimaryKeyId => Id;

        public string ConstraintName
        {
            get { return _constraintName; }
            set { _constraintName = value; }
        }

        public string ColumnName { get; set; } = string.Empty;

        public string DataType { get; set; } = string.Empty;

        #endregion properties

        public PrimaryKeyDataItem()
        {
        }

        public PrimaryKeyDataItem(string tableName)
            : this()
        {
            SetParameter(Tablename, tableName);

            GetItem(SpNames.PrimarykeyGet);
        }

        protected internal override void InitializeDataItem()
        {
            AutoIdField = Primarykeyid;
        }

        protected internal override void GetData()
        {
            GetField(CONSTRAINT_NAME, out _constraintName);
        }
    }
}
