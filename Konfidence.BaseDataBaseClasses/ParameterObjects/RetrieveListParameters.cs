namespace Konfidence.BaseData.ParameterObjects
{
    class RetrieveListParameters
    {
        private readonly IBaseDataItemList _DataItemList;
        private readonly string _StoredProcedure;

        internal string StoredProcedure
        {
            get { return _StoredProcedure; }
        }

        internal DbParameterObjectList ParameterObjectList
        {
            get
            {
                var parameterObjectList = _DataItemList.GetParameterObjectList();

                return parameterObjectList;
            }
        }

        public RetrieveListParameters(IBaseDataItemList dataItemList, string storedProcedure)
        {
            _StoredProcedure = storedProcedure;

            _DataItemList = dataItemList;
        }
    }
}
