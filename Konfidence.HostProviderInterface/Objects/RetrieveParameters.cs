using Konfidence.Base;
using Konfidence.BaseDataInterfaces;

namespace Konfidence.HostProviderInterface.Objects
{
    public class RetrieveParameters : BaseItem
    {
        private readonly IBaseDataItem _dataItem;

        public string StoredProcedure { get; }

        public IDbParameterObjectList ParameterObjectList
        {
            get
            {
                var parameterObjectList = _dataItem.GetParameterObjectList();

                return parameterObjectList;
            }
        }

        public RetrieveParameters(IBaseDataItem dataItem, string storedProcedure)
        {
            StoredProcedure = storedProcedure;

            _dataItem = dataItem;
        }
    }
}
