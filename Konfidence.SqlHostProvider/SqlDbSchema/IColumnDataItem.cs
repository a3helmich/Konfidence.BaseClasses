using Konfidence.DataBaseInterface;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public interface IColumnDataItem : IBaseDataItem
    {
        string Name { get; }
        string TableName { get; }
        bool IsPrimaryKey { get; }
        bool IsDefaulted { get; }
        bool IsComputed { get; }
        bool IsAutoUpdated { get; }
        bool IsGuidField { get; }
        bool IsLockInfo { get; }
        string DefaultPropertyValue { get; }
        string CharacterMaximumLength { get; }
        string DataType { get; }
        string DbDataType { get; }
        string SqlDataType { get; }
        void SetPrimaryKey(bool isPrimaryKey);
        void SetAutoUpdated(bool isAutoUpdated);
        void SetLockInfo(bool isLockInfo);
    }
}
