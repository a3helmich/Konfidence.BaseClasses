using System.Collections.Generic;
using Konfidence.Base;

namespace Konfidence.BaseData.ParameterObjects
{
    internal class RetrieveParameters : BaseItem
    {
        private readonly BaseDataItem _DataItem;
        private readonly string _StoredProcedure;

        internal string StoredProcedure
        {
            get { return _StoredProcedure; }
        }

        internal DbParameterObjectList ParameterObjectList
        {
            get
            {
                var parameterObjectList = _DataItem.GetParameterObjectList();

                return parameterObjectList;
            }
        }

        public RetrieveParameters(BaseDataItem dataItem, string storedProcedure)
        {
            _StoredProcedure = storedProcedure;

            _DataItem = dataItem;
        }
    }
}
