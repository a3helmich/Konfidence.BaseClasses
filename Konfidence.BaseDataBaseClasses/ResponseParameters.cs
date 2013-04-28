using System.Collections.Generic;
using Konfidence.Base;

namespace Konfidence.BaseData
{
    internal class ResponseParameters : BaseItem
    {
        private int _Id;
        private Dictionary<string, DbParameterObject> _AutoUpdateFieldList;

        internal int Id
        {
            get { return _Id; }
        }

        internal Dictionary<string, DbParameterObject> AutoUpdateFieldList
        {
            get { return _AutoUpdateFieldList; }
        }

        internal void SetId(int id)
        {
            _Id = id;
        }

        internal void SetAutoUpdateFieldList(Dictionary<string, DbParameterObject> autoUpdateFieldList)
        {
            _AutoUpdateFieldList = autoUpdateFieldList;
        }
    }
}
