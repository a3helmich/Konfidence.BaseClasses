﻿using System.Collections.Generic;
using JetBrains.Annotations;
using Konfidence.DataBaseInterface;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    //List<T>, IBaseDataItemList
    public interface IColumnDataItemList : IBaseDataItemList<ColumnDataItem>
    {
        [UsedImplicitly]
        bool HasDefaultValueFields { get; }

        [UsedImplicitly]
        string GetFieldNames(List<string> getListByFieldList);
        [UsedImplicitly]
        string GetLastField(List<string> findByFieldList);
        [UsedImplicitly]
        string GetUnderscoreFieldNames(List<string> getListByFieldList);
        [UsedImplicitly]
        string GetTypedCommaFieldNames(List<string> getListByFieldList);
        [UsedImplicitly]
        string GetFirstField(List<string> getListByFieldList);
        [UsedImplicitly]
        string GetCommaFieldNames(List<string> getListByFieldList);

        IColumnDataItem Find(string columnName);

        //        bool HasCurrent { get; }
        //        int Capacity { get; set; }
        //        IColumnDataItem FindById(string textId);
        //        IColumnDataItem FindById(int id);
        //        IColumnDataItem FindById(Guid guidId);
        //        IColumnDataItem FindCurrent();
        //        void SetSelected(string idText, string isEditingText);
        //        void SetSelected(string idText);
        //        void SetSelected(BaseDataItem dataItem);
        //        void New();
        //        void Edit(ColumnDataItem dataItem);
        //        void Save(ColumnDataItem dataItem);
        //        void Cancel(ColumnDataItem dataItem);
        //        void Delete(ColumnDataItem dataItem);
        //        void AddRange(IEnumerable<ColumnDataItem> collection);
        //        ReadOnlyCollection<ColumnDataItem> AsReadOnly();
        //        int BinarySearch(int index, int count, ColumnDataItem item, IComparer<ColumnDataItem> comparer);
        //        int BinarySearch(ColumnDataItem item);
        //        int BinarySearch(ColumnDataItem item, IComparer<ColumnDataItem> comparer);
        //        void CopyTo(ColumnDataItem[] array);
        //        void CopyTo(int index, ColumnDataItem[] array, int arrayIndex, int count);
        //        bool Exists(Predicate<ColumnDataItem> match);
        //        IColumnDataItem Find(Predicate<ColumnDataItem> match);
        //        List<IColumnDataItem> FindAll(Predicate<ColumnDataItem> match);
        //        int FindIndex(Predicate<ColumnDataItem> match);
        //        int FindIndex(int startIndex, Predicate<ColumnDataItem> match);
        //        int FindIndex(int startIndex, int count, Predicate<ColumnDataItem> match);
        //        IColumnDataItem FindLast(Predicate<ColumnDataItem> match);
        //        int FindLastIndex(Predicate<ColumnDataItem> match);
        //        int FindLastIndex(int startIndex, Predicate<ColumnDataItem> match);
        //        int FindLastIndex(int startIndex, int count, Predicate<ColumnDataItem> match);
        //        void ForEach(Action<ColumnDataItem> action);
        //        List<ColumnDataItem> GetRange(int index, int count);
        //        int IndexOf(ColumnDataItem item, int index);
        //        int IndexOf(ColumnDataItem item, int index, int count);
        //        void InsertRange(int index, IEnumerable<ColumnDataItem> collection);
        //        int LastIndexOf(ColumnDataItem item);
        //        int LastIndexOf(ColumnDataItem item, int index);
        //        int LastIndexOf(ColumnDataItem item, int index, int count);
        //        int RemoveAll(Predicate<ColumnDataItem> match);
        //        void RemoveRange(int index, int count);
        //        void Reverse();
        //        void Reverse(int index, int count);
        //        void Sort();
        //        void Sort(IComparer<ColumnDataItem> comparer);
        //        void Sort(int index, int count, IComparer<ColumnDataItem> comparer);
        //        void Sort(Comparison<ColumnDataItem> comparison);
        //        IColumnDataItem[] ToArray();
        //        void TrimExcess();
        //        bool TrueForAll(Predicate<ColumnDataItem> match);
    }
}