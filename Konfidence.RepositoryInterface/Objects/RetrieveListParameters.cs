﻿using System.Collections.Generic;
using Konfidence.DataBaseInterface;

namespace Konfidence.RepositoryInterface.Objects
{
    public class RetrieveListParameters<T> where T : IBaseDataItem
    {
        private readonly IBaseDataItemList<T> _dataItemList;

        public string StoredProcedure { get; }

        public List<IDbParameterObject> ParameterObjectList
        {
            get
            {
                var parameterObjectList = _dataItemList.GetParameterObjectList();

                return parameterObjectList;
            }
        }

        public RetrieveListParameters(IBaseDataItemList<T> dataItemList, string storedProcedure)
        {
            StoredProcedure = storedProcedure;

            _dataItemList = dataItemList;
        }
    }
}