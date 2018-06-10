using System.Collections.Generic;
using Konfidence.BaseDataInterfaces;

namespace Konfidence.RepositoryInterface.Objects
{
    public class ResponseParameters
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
