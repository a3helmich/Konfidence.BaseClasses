﻿using Konfidence.DatabaseInterface;

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
        string NewGuidPropertyValue { get; }
        
        string CharacterMaximumLength { get; }
        string DataType { get; }
        string DbDataType { get; }
        string SqlDataType { get; }
    }
}
