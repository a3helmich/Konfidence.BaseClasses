using System.Collections.Generic;

namespace Konfidence.BaseDataInterfaces
{
    public interface IBaseDataItemList<T> : IList<T> where T : IBaseDataItem
    {
        void SetParameters(string storedProcedure);

        //void AddItem(BaseClient client);
        T GetDataItem();

        List<IDbParameterObjectList> ConvertToListOfParameterObjectList();

        IDbParameterObjectList GetParameterObjectList();
    }
}
