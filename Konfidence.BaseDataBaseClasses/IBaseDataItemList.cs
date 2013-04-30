using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Konfidence.BaseData
{
    public interface IBaseDataItemList
    {
        void SetParameters(string storedProcedure, Database database, DbCommand dbCommand);
        void AddItem(BaseHost dataHost);
        BaseDataItem GetDataItem();
        List<List<DbParameterObject>> Convert2ListOfParameterObjectList();
    }
}
