using System;
using JetBrains.Annotations;

namespace Konfidence.BaseData
{
    public static class BaseDataItemSpParameterExtensions
    {

        [UsedImplicitly]
        public static void SetParameter([NotNull] this BaseDataItem baseDataItem, string fieldName, byte value)
        {
            baseDataItem.SetField(fieldName, value);
        }

        [UsedImplicitly]
        public static void SetParameter([NotNull] this BaseDataItem baseDataItem, string fieldName, int value)
        {
            baseDataItem.SetField(fieldName, value);
        }

        [UsedImplicitly]
        public static void SetParameter([NotNull] this BaseDataItem baseDataItem, string fieldName, short value)
        {
            baseDataItem.SetField(fieldName, value);
        }

        [UsedImplicitly]
        public static void SetParameter([NotNull] this BaseDataItem baseDataItem, string fieldName, long value)
        {
            baseDataItem.SetField(fieldName, value);
        }

        [UsedImplicitly]
        public static void SetParameter([NotNull] this BaseDataItem baseDataItem, string fieldName, decimal value)
        {
            baseDataItem.SetField(fieldName, value);
        }

        [UsedImplicitly]
        public static void SetParameter([NotNull] this BaseDataItem baseDataItem, string fieldName, Guid value)
        {
            baseDataItem.SetField(fieldName, value);
        }

        [UsedImplicitly]
        public static void SetParameter([NotNull] this BaseDataItem baseDataItem, string fieldName, string value)
        {
            baseDataItem.SetField(fieldName, value);
        }

        [UsedImplicitly]
        public static void SetParameter([NotNull] this BaseDataItem baseDataItem, string fieldName, bool value)
        {
            baseDataItem.SetField(fieldName, value);
        }

        [UsedImplicitly]
        public static void SetParameter([NotNull] this BaseDataItem baseDataItem, string fieldName, DateTime value)
        {
            baseDataItem.SetField(fieldName, value);
        }

        [UsedImplicitly]
        public static void SetParameter([NotNull] this BaseDataItem baseDataItem, string fieldName, TimeSpan value)
        {
            baseDataItem.SetField(fieldName, value);
        }
    }
}
