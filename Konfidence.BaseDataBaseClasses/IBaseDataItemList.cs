using System.Collections.Generic;
using System.Data.Common;
using Konfidence.BaseData.ParameterObjects;
using Microsoft.Practices.EnterpriseLibrary.Data;

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
