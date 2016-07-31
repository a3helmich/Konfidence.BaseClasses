﻿namespace Konfidence.BaseData.SqlDbSchema
{
    public interface IColumnDataItem 
    {
        string Name { get; }
        bool IsPrimaryKey { get; }
        bool IsDefaulted { get; }
        bool IsComputed { get; }
        bool IsAutoUpdated { get; }
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