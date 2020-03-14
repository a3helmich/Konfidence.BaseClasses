using System.Collections.Generic;
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
    }
}
