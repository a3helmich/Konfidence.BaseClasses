namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    internal interface IPrimaryKeyDataItem
    {
        public string ConstraintName { get; set; }

        public string ConstraintType { get; set; }
        
        public string TableName { get; set; }
    }
}
