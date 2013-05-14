using System.Collections.Generic;
using Konfidence.BaseData.ParameterObjects;

namespace Konfidence.BaseData
{
    public interface IBaseDataItemList 
    {
        void SetParameters(string storedProcedure);
        //void AddItem(BaseHost dataHost);
        BaseDataItem GetDataItem();
        List<DbParameterObjectList> Convert2ListOfParameterObjectList();
        DbParameterObjectList GetParameterObjectList();
    }
}
