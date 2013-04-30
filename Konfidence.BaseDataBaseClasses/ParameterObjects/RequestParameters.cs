using System.Collections.Generic;
using Konfidence.Base;

namespace Konfidence.BaseData.ParameterObjects
{
    internal class RequestParameters : BaseItem
    {
        private readonly BaseDataItem _DataItem;
        private readonly string _StoredProcedure;

        internal string StoredProcedure
        {
            get { return _StoredProcedure; }
        }

        internal string AutoIdField
        {
            get { return _DataItem.AutoIdField; }
        }

        internal int Id
        {
            get { return _DataItem.GetId(); }
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

        public RequestParameters(BaseDataItem dataItem, string storedProcedure)
        {
            _StoredProcedure = storedProcedure;

            _DataItem = dataItem;
        }
    }
}
