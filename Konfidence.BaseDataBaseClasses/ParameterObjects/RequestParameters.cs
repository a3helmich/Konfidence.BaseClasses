using System.Collections.Generic;
using Konfidence.Base;

namespace Konfidence.BaseData.ParameterObjects
{
    public class RequestParameters : BaseItem
    {
        private readonly BaseDataItem _dataItem;

        public string StoredProcedure { get; }

        public string AutoIdField => _dataItem.AutoIdField;

        public int Id => _dataItem.GetId();

        public Dictionary<string, DbParameterObject> AutoUpdateFieldList => _dataItem.AutoUpdateFieldDictionary;

        public DbParameterObjectList ParameterObjectList
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
