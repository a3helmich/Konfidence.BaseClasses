using System.Collections.Generic;
using Konfidence.Base;

namespace Konfidence.BaseData.ParameterObjects
{
    internal class RequestParameters : BaseItem
    {
        private readonly BaseDataItem _dataItem;

        internal string StoredProcedure { get; }

        internal string AutoIdField => _dataItem.AutoIdField;

        internal int Id => _dataItem.GetId();

        internal Dictionary<string, DbParameterObject> AutoUpdateFieldList => _dataItem.AutoUpdateFieldDictionary;

        internal DbParameterObjectList ParameterObjectList
        {
            get
            {
                var parameterObjectList = _dataItem.SetItemData();

                return parameterObjectList;
            }
        }

        public RequestParameters(BaseDataItem dataItem, string storedProcedure)
        {
            StoredProcedure = storedProcedure;

            _dataItem = dataItem;
        }
    }
}
