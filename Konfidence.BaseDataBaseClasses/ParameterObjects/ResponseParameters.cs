using System.Collections.Generic;
using Konfidence.Base;

namespace Konfidence.BaseData.ParameterObjects
{
    public class ResponseParameters : BaseItem
    {
        public int Id { get; private set; }

        public Dictionary<string, DbParameterObject> AutoUpdateFieldList { get; private set; }

        public void SetId(int id)
        {
            Id = id;
        }

        public void SetAutoUpdateFieldList(Dictionary<string, DbParameterObject> autoUpdateFieldList)
        {
            AutoUpdateFieldList = autoUpdateFieldList;
        }
    }
}
