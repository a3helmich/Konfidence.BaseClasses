using System.Collections.Generic;
using Konfidence.BaseClassInterfaces;
using Konfidence.BaseDataInterfaces;

namespace Konfidence.Base
{
    //public class BaseDataItemList<T> : List<T>, IBaseDataItemList<T> where T : class, IBaseDataItem
    public class BaseItemList<T> : List<T>, IBaseItemList<T> where T : class, IBaseItem
    {
    }
}
