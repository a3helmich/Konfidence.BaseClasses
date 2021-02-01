using System.Collections.Generic;
using Konfidence.DataBaseInterface;

namespace Konfidence.RepositoryInterface.Objects
{
    public class RetrieveParameters 
    {
        private readonly IBaseDataItem _dataItem;

        public string StoredProcedure { get; }

        public List<IDbParameterObject> ParameterObjectList
        {
            get
            {
                var parameterObjectList = _dataItem.GetParameterObjects();

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
