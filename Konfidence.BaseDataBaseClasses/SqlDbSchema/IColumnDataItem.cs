namespace Konfidence.BaseData.SqlDbSchema
{
    public interface IColumnDataItem
    {
        string Name { get; set; }
        string DataType { get; }
    }
}
