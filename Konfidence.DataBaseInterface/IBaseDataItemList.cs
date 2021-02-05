using System.Collections.Generic;

namespace Konfidence.DataBaseInterface
{
    public interface IBaseDataItemList<T> : IList<T> where T : IBaseDataItem
    {
        void SetParameters(string storedProcedure);

        T GetDataItem();

        List<IDbParameterData> GetParameterObjectList();
    }
}
