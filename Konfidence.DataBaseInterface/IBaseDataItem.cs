﻿using System;
using System.Collections.Generic;

namespace Konfidence.DataBaseInterface
{
    public interface IBaseDataItem
    {
        string AutoIdField { get; set; }

        Dictionary<string, IDbParameterObject> AutoUpdateFieldDictionary { get; }

        Guid GuidIdValue { get; }

        bool IsSelected { get; set; }

        bool IsEditing { get; set; }

        IBaseClient Client { get; set; }

        string LoadStoredProcedure { get; set; }

        string DeleteStoredProcedure { get; set; }

        string SaveStoredProcedure { get; set; }

        void InitializeDataItem();

        List<IDbParameterObject> SetItemData();

        void Save();

        void Delete();

        void LoadDataItem();

        int GetId();

        void SetId(int id);

        void GetKey();

        void GetData();

        void SetProperties(Dictionary<string, object> propertyDictionary);

        void GetProperties(List<IDbParameterObject> properties);

        List<IDbParameterObject> GetParameterObjectList();
    }
}
