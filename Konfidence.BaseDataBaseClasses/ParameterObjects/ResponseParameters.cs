using System.Collections.Generic;
using Konfidence.Base;

namespace Konfidence.BaseData.ParameterObjects
{
    internal class ResponseParameters : BaseItem
    {
        internal int Id { get; private set; }

        internal Dictionary<string, DbParameterObject> AutoUpdateFieldList { get; private set; }

        internal void SetId(int id)
        {
            Id = id;
        }

        internal void SetAutoUpdateFieldList(Dictionary<string, DbParameterObject> autoUpdateFieldList)
        {
            AutoUpdateFieldList = autoUpdateFieldList;
        }
    }
}
