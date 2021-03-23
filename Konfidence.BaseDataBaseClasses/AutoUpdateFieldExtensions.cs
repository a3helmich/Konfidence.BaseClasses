using System;
using JetBrains.Annotations;

namespace Konfidence.BaseData
{
    public static class AutoUpdateFieldExtensions
    {
        [UsedImplicitly]
        public static void GetAutoUpdateField([NotNull] this BaseDataItem baseDataItem, [NotNull] string fieldName, ref byte fieldValue)
        {
            fieldValue = (byte)(baseDataItem.GetAutoUpdateField(fieldName) ?? fieldValue);
        }

        [UsedImplicitly]
        public static void GetAutoUpdateField([NotNull] this BaseDataItem baseDataItem, [NotNull] string fieldName, ref short fieldValue)
        {
            fieldValue = (short)(baseDataItem.GetAutoUpdateField(fieldName) ?? fieldValue);
        }

        [UsedImplicitly]
        public static void GetAutoUpdateField([NotNull] this BaseDataItem baseDataItem, [NotNull] string fieldName, ref int fieldValue)
        {
            fieldValue = (int)(baseDataItem.GetAutoUpdateField(fieldName) ?? fieldValue);
        }

        [UsedImplicitly]
        public static void GetAutoUpdateField([NotNull] this BaseDataItem baseDataItem, [NotNull] string fieldName, ref long fieldValue)
        {
            fieldValue = (long)(baseDataItem.GetAutoUpdateField(fieldName) ?? fieldValue);
        }

        [UsedImplicitly]
        public static void GetAutoUpdateField([NotNull] this BaseDataItem baseDataItem, [NotNull] string fieldName, ref Guid fieldValue)
        {
            fieldValue = (Guid)(baseDataItem.GetAutoUpdateField(fieldName) ?? fieldValue);
        }

        [UsedImplicitly]
        public static void GetAutoUpdateField([NotNull] this BaseDataItem baseDataItem, [NotNull] string fieldName, [CanBeNull] ref string fieldValue)
        {
            fieldValue = (baseDataItem.GetAutoUpdateField(fieldName) ?? fieldValue) as string;
        }

        [UsedImplicitly]
        public static void GetAutoUpdateField([NotNull] this BaseDataItem baseDataItem, [NotNull] string fieldName, ref bool fieldValue)
        {
            fieldValue = (bool)(baseDataItem.GetAutoUpdateField(fieldName) ?? fieldValue);
        }

        [UsedImplicitly]
        public static void GetAutoUpdateField([NotNull] this BaseDataItem baseDataItem, [NotNull] string fieldName, ref DateTime fieldValue)
        {
            fieldValue = (DateTime)(baseDataItem.GetAutoUpdateField(fieldName) ?? fieldValue);
        }

        [UsedImplicitly]
        public static void GetAutoUpdateField([NotNull] this BaseDataItem baseDataItem, [NotNull] string fieldName, ref TimeSpan fieldValue)
        {
            fieldValue = (TimeSpan)(baseDataItem.GetAutoUpdateField(fieldName) ?? fieldValue);
        }

        [UsedImplicitly]
        public static void GetAutoUpdateField([NotNull] this BaseDataItem baseDataItem, [NotNull] string fieldName, ref decimal fieldValue)
        {
            fieldValue = (decimal)(baseDataItem.GetAutoUpdateField(fieldName) ?? fieldValue);
        }
    }
}
