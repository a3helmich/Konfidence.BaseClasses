using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.DataBaseInterface;

namespace Konfidence.BaseData
{
    public static class BaseDataItemExtensions
    {
        [CanBeNull]
        public static T FindById<T>([NotNull] this IList<T> baseDataItems, int id) where T : class, IBaseDataItem
        {
            return baseDataItems.FirstOrDefault(x => x.GetId() == id);
        }

        //[CanBeNull]
        //internal static T FindByIsSelected<T>([NotNull] this IList<T> baseDataItems) where T : class, IBaseDataItem
        //{
        //    var firstSelected = baseDataItems.FirstOrDefault(x => x.IsSelected);

        //    if (firstSelected.IsAssigned())
        //    {
        //        return firstSelected;
        //    }

        //    // if none selected, make first selected
        //    if (baseDataItems.Any())
        //    {
        //        var first = baseDataItems.First();

        //        first.IsSelected = true;

        //        return first;
        //    }

        //    return null;
        //}

        //[CanBeNull]
        //internal static T FindByIsEditing<T>([NotNull] this IList<T> baseDataItems) where T : class, IBaseDataItem
        //{
        //    return baseDataItems.FirstOrDefault(x => x.IsEditing);
        //}

        //[CanBeNull]
        //public static T FindCurrent<T>([NotNull] this IList<T> baseDataItems) where T : class, IBaseDataItem
        //{
        //    var dataItem = baseDataItems.FindByIsEditing();

        //    if (dataItem.IsAssigned())
        //    {
        //        return dataItem;
        //    }

        //    return baseDataItems.FindByIsSelected();
        //}

        //public static bool HasCurrent<T>([NotNull] this IList<T> baseDataItems) where T : class, IBaseDataItem
        //{
        //        return baseDataItems.Any(x => x.IsEditing || x.IsSelected);
        //}

        //[UsedImplicitly]
        //public static void SetSelected<T>([NotNull] this IList<T> baseDataItems, int id, string isEditingText) where T : class, IBaseDataItem
        //{
        //    bool.TryParse(isEditingText, out var isEditing);

        //    baseDataItems.SetSelected(id, isEditing);
        //}

        //[UsedImplicitly]
        //public static void SetSelected<T>([NotNull] this IList<T> baseDataItems, int id) where T : class, IBaseDataItem
        //{
        //    var baseDataItem = baseDataItems.FindById(id);

        //    if (baseDataItem.IsAssigned())
        //    {
        //        baseDataItem.IsSelected = true;
        //    }
        //}

        //[UsedImplicitly]
        //public static void SetSelected<T>([NotNull] this IList<T> baseDataItems, IBaseDataItem dataItem) where T : class, IBaseDataItem
        //{
        //    if (dataItem.IsAssigned())
        //    {
        //        baseDataItems.SetSelected(dataItem.GetId());
        //    }
        //}

        //public static void SetSelected<T>([NotNull] this IList<T> baseDataItems, int id, bool isEditing) where T : class, IBaseDataItem
        //{
        //    if (baseDataItems.Any())
        //    {
        //        foreach (var baseDataItem in baseDataItems)
        //        {
        //            baseDataItem.IsSelected = false;
        //        }

        //        var selectedDataItem = baseDataItems.FindById(id);

        //        if (!selectedDataItem.IsAssigned())
        //        {
        //            return;
        //        }

        //        selectedDataItem.IsSelected = true;
        //        selectedDataItem.IsEditing = isEditing;
        //    }
        //}

        //public static void New<T>([NotNull] this IList<T> baseDataItems) where T : class, IBaseDataItem
        //{
        //    var dataItem = baseDataItems.FindCurrent();

        //    if (!dataItem.IsAssigned())
        //    {
        //        return;
        //    }

        //    //dataItem.IsSelected = false;
        //    //dataItem.IsEditing = true; 
        //}

        //[UsedImplicitly]
        //public static void Edit<T>(this T dataItem) where T : class, IBaseDataItem
        //{
        //    if (dataItem.IsAssigned())
        //    {
        //        dataItem.IsEditing = true; 
        //    }
        //}

        [UsedImplicitly]
        public static void Save<T>([NotNull] this IList<T> baseDataItems, T dataItem) where T : class, IBaseDataItem
        {
            if (!dataItem.IsAssigned())
            {
                return;
            }

            dataItem.Save();

            //baseDataItems.SetSelected(dataItem.GetId());

            //dataItem.IsEditing = false; 
        }

        [UsedImplicitly]
        public static void Cancel<T>(this T dataItem) where T : class, IBaseDataItem
        {
            if (!dataItem.IsAssigned())
            {
                return;
            }

            dataItem.LoadDataItem();

            //dataItem.IsEditing = false;
        }

        [UsedImplicitly]
        public static void Delete<T>([NotNull] this IList<T> baseDataItems, T dataItem) where T : class, IBaseDataItem
        {
            if (!dataItem.IsAssigned())
            {
                return;
            }

            dataItem.Delete();

            //var selectedIndex = baseDataItems.IndexOf(dataItem);

            baseDataItems.Remove(dataItem);

            //if (baseDataItems.Any())
            //{
            //    if (selectedIndex < baseDataItems.Count)
            //    {
            //        baseDataItems[selectedIndex].IsSelected = true;

            //        return;
            //    }

            //    baseDataItems[selectedIndex - 1].IsSelected = true;
            //}
        }
    }
}
