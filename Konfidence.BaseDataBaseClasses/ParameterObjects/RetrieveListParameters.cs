namespace Konfidence.BaseData.ParameterObjects
{
    class RetrieveListParameters
    {
        private readonly IBaseDataItemList _dataItemList;

        internal string StoredProcedure { get; }

        internal DbParameterObjectList ParameterObjectList
        {
            get
            {
                var parameterObjectList = _dataItemList.GetParameterObjectList();

                return parameterObjectList;
            }
        }

        public RetrieveListParameters(IBaseDataItemList dataItemList, string storedProcedure)
        {
            StoredProcedure = storedProcedure;

            _dataItemList = dataItemList;
        }
    }
}
