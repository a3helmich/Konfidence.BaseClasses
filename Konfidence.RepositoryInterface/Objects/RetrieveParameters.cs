using Konfidence.DataBaseInterface;

namespace Konfidence.RepositoryInterface.Objects
{
    public class RetrieveParameters 
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
