using Konfidence.Base;

namespace Konfidence.BaseData.ParameterObjects
{
    public class RetrieveParameters : BaseItem
    {
        private readonly BaseDataItem _dataItem;

        public string StoredProcedure { get; }

        public DbParameterObjectList ParameterObjectList
        {
            get
            {
                var parameterObjectList = _dataItem.GetParameterObjectList();

                return parameterObjectList;
            }
        }

        public RetrieveParameters(BaseDataItem dataItem, string storedProcedure)
        {
            StoredProcedure = storedProcedure;

            _dataItem = dataItem;
        }
    }
}
