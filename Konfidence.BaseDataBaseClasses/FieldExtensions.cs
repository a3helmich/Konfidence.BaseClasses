using System;
using JetBrains.Annotations;
using Konfidence.BaseData.Sp;

namespace Konfidence.BaseData
{
    public static class FieldExtensions
    {
        public static void SetField(this BaseDataItem baseDataItem, string fieldName, int value)
        {
            baseDataItem.SpParameterData.SetParameter(fieldName, value);
        }

        public static void SetField(this BaseDataItem baseDataItem, string fieldName, byte value)
        {
            baseDataItem.SpParameterData.SetParameter(fieldName, value);
        }

        [UsedImplicitly]
        public static void SetField(this BaseDataItem baseDataItem, string fieldName, short value)
        {
            baseDataItem.SpParameterData.SetParameter(fieldName, value);
        }

        public static void SetField(this BaseDataItem baseDataItem, string fieldName, long value)
        {
            baseDataItem.SpParameterData.SetParameter(fieldName, value);
        }

        public static void SetField(this BaseDataItem baseDataItem, string fieldName, Guid value)
        {
            baseDataItem.SpParameterData.SetParameter(fieldName, value);
        }

        public static void SetField(this BaseDataItem baseDataItem, string fieldName, string value)
        {
            baseDataItem.SpParameterData.SetParameter(fieldName, value);
        }

        public static void SetField(this BaseDataItem baseDataItem, string fieldName, bool value)
        {
            baseDataItem.SpParameterData.SetParameter(fieldName, value);
        }

        [UsedImplicitly]
        public static void SetField(this BaseDataItem baseDataItem, string fieldName, DateTime value)
        {
            baseDataItem.SpParameterData.SetParameter(fieldName, value);
        }

        [UsedImplicitly]
        public static void SetField(this BaseDataItem baseDataItem, string fieldName, TimeSpan value)
        {
            baseDataItem.SpParameterData.SetParameter(fieldName, value);
        }

        [UsedImplicitly]
        public static void SetField(this BaseDataItem baseDataItem, string fieldName, decimal value)
        {
            baseDataItem.SpParameterData.SetParameter(fieldName, value);
        }
    }
}
