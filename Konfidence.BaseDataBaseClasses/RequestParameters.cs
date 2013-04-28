using System.Collections.Generic;
using Konfidence.Base;

namespace Konfidence.BaseData
{
    internal class RequestParameters : BaseItem
    {
        private readonly BaseDataItem _DataItem;

        internal string SaveStoredProcedure
        {
            get { return _DataItem.SaveStoredProcedure; }
        }

        internal string AutoIdField
        {
            get { return _DataItem.AutoIdField; }
        }

        internal int Id
        {
            get { return _DataItem._Id; }
        }

        internal Dictionary<string, DbParameterObject> AutoUpdateFieldList
        {
            get { return _DataItem.AutoUpdateFieldList; }
        }

        internal List<DbParameterObject> ParameterObjectList
        {
            get
            {
                var parameterObjectList = _DataItem.SetItemData();

                return parameterObjectList;
            }
        }

        public RequestParameters(BaseDataItem dataItem)
        {
            _DataItem = dataItem;
        }
    }
}
