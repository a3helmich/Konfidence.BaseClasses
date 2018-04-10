using System.Collections.Generic;
using Konfidence.Base;
using Konfidence.BaseDataInterfaces;

namespace Konfidence.HostProviderInterface.Objects
{
    public class RequestParameters : BaseItem
    {
        private readonly IBaseDataItem _dataItem;

        public string StoredProcedure { get; }

        public string AutoIdField => _dataItem.AutoIdField;

        public int Id => _dataItem.GetId();

        public Dictionary<string, IDbParameterObject> AutoUpdateFieldList => _dataItem.AutoUpdateFieldDictionary;

        public IDbParameterObjectList ParameterObjectList
        {
            get
            {
                var parameterObjectList = _dataItem.SetItemData();

                return parameterObjectList;
            }
        }

        public RequestParameters(IBaseDataItem dataItem, string storedProcedure)
        {
            StoredProcedure = storedProcedure;

            _dataItem = dataItem;
        }
    }
}
