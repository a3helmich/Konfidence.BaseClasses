using System.Collections.Generic;
using Konfidence.Base;
using Konfidence.BaseDataInterfaces;

namespace Konfidence.RepositoryInterface.Objects
{
    public class ResponseParameters : BaseItem
    {
        public int Id { get; private set; }

        public Dictionary<string, IDbParameterObject> AutoUpdateFieldList { get; private set; }

        public void SetId(int id)
        {
            Id = id;
        }

        public void SetAutoUpdateFieldList(Dictionary<string, IDbParameterObject> autoUpdateFieldList)
        {
            AutoUpdateFieldList = autoUpdateFieldList;
        }
    }
}
