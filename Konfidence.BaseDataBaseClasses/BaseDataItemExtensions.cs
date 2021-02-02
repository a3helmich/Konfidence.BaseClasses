using System;
using System.Linq;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.DataBaseInterface;

namespace Konfidence.BaseData
{
    public static class BaseDataItemExtensions
    {
        [CanBeNull]
        public static T FindById<T>([NotNull] this BaseDataItemList<T> baseDataItems, string textId) where T : class, IBaseDataItem
        {
            if (Guid.TryParse(textId, out var guidId))
            {
                return baseDataItems.FindById(guidId);
            }

            if (int.TryParse(textId, out var id))
            {
                return baseDataItems.FindById(id);
            }

            return null;
        }

        [CanBeNull]
        public static T FindById<T>([NotNull] this BaseDataItemList<T> baseDataItems, int id) where T : class, IBaseDataItem
        {
            return baseDataItems.FirstOrDefault(x => x.GetId() == id);
        }

        [CanBeNull]
        public static T FindById<T>([NotNull] this BaseDataItemList<T> baseDataItems, Guid guidId) where T : class, IBaseDataItem
        {
            return baseDataItems.FirstOrDefault(x => x.GuidIdValue == guidId);
        }

        [CanBeNull]
        internal static T FindByIsSelected<T>([NotNull] this BaseDataItemList<T> baseDataItems) where T : class, IBaseDataItem
        {
            var firstSelected = baseDataItems.FirstOrDefault(x => x.IsSelected);

            if (firstSelected.IsAssigned())
            {
                return firstSelected;
            }

            // if none selected, make first selected
            if (baseDataItems.Any())
            {
                var first = baseDataItems.First();

                first.IsSelected = true;

                return first;
            }

            return null;
        }

        [CanBeNull]
        internal static T FindByIsEditing<T>([NotNull] this BaseDataItemList<T> baseDataItems) where T : class, IBaseDataItem
        {
            return baseDataItems.FirstOrDefault(x => x.IsEditing);
        }

        [CanBeNull]
        public static T FindCurrent<T>([NotNull] this BaseDataItemList<T> baseDataItems) where T : class, IBaseDataItem
        {
            var dataItem = baseDataItems.FindByIsEditing();

            if (dataItem.IsAssigned())
            {
                return dataItem;
            }

            return baseDataItems.FindByIsSelected();
        }

        public static bool HasCurrent<T>([NotNull] this BaseDataItemList<T> baseDataItems) where T : class, IBaseDataItem
        {
                return baseDataItems.Any(x => x.IsEditing || x.IsSelected);
        }

        [UsedImplicitly]
        internal static void SetSelected<T>([NotNull] this BaseDataItemList<T> baseDataItems, string idText, string isEditingText) where T : class, IBaseDataItem
        {
            bool.TryParse(isEditingText, out var isEditing);

            if (Guid.TryParse(idText, out var guidId))
            {
                baseDataItems.SetSelected(guidId, isEditing);

                return;
            }

            if (int.TryParse(idText, out var id))
            {
                baseDataItems.SetSelected(id, isEditing);
            }
        }

        [UsedImplicitly]
        internal static void SetSelected<T>([NotNull] this BaseDataItemList<T> baseDataItems, string idText) where T : class, IBaseDataItem
        {
            if (Guid.TryParse(idText, out var guidId))
            {
                baseDataItems.SetSelected(guidId, false);

                return;
            }

            if (int.TryParse(idText, out var id))
            {
                baseDataItems.SetSelected(id);
            }
        }

        [UsedImplicitly]
        internal static void SetSelected<T>([NotNull] this BaseDataItemList<T> baseDataItems, BaseDataItem dataItem) where T : class, IBaseDataItem
        {
            if (dataItem.IsAssigned())
            {
                baseDataItems.SetSelected(dataItem.GetId());
            }
        }

        internal static void SetSelected<T>([NotNull] this BaseDataItemList<T> baseDataItems, int id, bool isEditing = false) where T : class, IBaseDataItem
        {
            baseDataItems.SetSelected(id, Guid.Empty, isEditing);
        }

        internal static void SetSelected<T>([NotNull] this BaseDataItemList<T> baseDataItems, Guid guidId, bool isEditing) where T : class, IBaseDataItem
        {
            baseDataItems.SetSelected(0, guidId, isEditing);
        }

        internal static void SetSelected<T>([NotNull] this BaseDataItemList<T> baseDataItems, int id, Guid guidId, bool isEditing) where T : class, IBaseDataItem
        {
            if (baseDataItems.Any())
            {
                foreach (var dataItem in baseDataItems)
                {
                    dataItem.IsSelected = false;
                }

                if (id < 1 && !guidId.IsAssigned())
                {
                    baseDataItems[0].IsSelected = true;
                }
                else
                {
                    var dataItem = id > 0 ? baseDataItems.FindById(id) : baseDataItems.FindById(guidId);

                    if (!dataItem.IsAssigned())
                    {
                        return;
                    }

                    dataItem.IsSelected = true;
                    dataItem.IsEditing = isEditing;
                }
            }
        }

        public static void New<T>([NotNull] this BaseDataItemList<T> baseDataItems) where T : class, IBaseDataItem
        {
            var dataItem = baseDataItems.FindCurrent();

            if (!dataItem.IsAssigned())
            {
                return;
            }

            dataItem.IsSelected = false;
            dataItem.IsEditing = true; // nieuw
        }

        [UsedImplicitly]
        public static void Edit<T>(this T dataItem) where T : class, IBaseDataItem
        {
            if (dataItem.IsAssigned())
            {
                dataItem.IsEditing = true; // nieuw
            }
        }

        [UsedImplicitly]
        public static void Save<T>([NotNull] this BaseDataItemList<T> baseDataItems, T dataItem) where T : class, IBaseDataItem
        {
            if (!dataItem.IsAssigned())
            {
                return;
            }

            dataItem.Save();

            baseDataItems.SetSelected(dataItem.GetId());

            dataItem.IsEditing = false; // nieuw
        }

        [UsedImplicitly]
        public static void Cancel<T>(this T dataItem) where T : class, IBaseDataItem
        {
            if (!dataItem.IsAssigned())
            {
                return;
            }

            dataItem.LoadDataItem();

            dataItem.IsEditing = false;
        }

        [UsedImplicitly]
        public static void Delete<T>([NotNull] this BaseDataItemList<T> baseDataItems, T dataItem) where T : class, IBaseDataItem
        {
            if (!dataItem.IsAssigned())
            {
                return;
            }

            dataItem.Delete();

            var selectedIndex = baseDataItems.IndexOf(dataItem);

            baseDataItems.Remove(dataItem);

            if (baseDataItems.Any())
            {
                if (selectedIndex < baseDataItems.Count)
                {
                    baseDataItems[selectedIndex].IsSelected = true;

                    return;
                }

                baseDataItems[selectedIndex - 1].IsSelected = true;
            }
        }
    }
}
