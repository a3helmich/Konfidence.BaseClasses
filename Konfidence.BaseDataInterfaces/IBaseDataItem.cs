using System;
using System.Collections.Generic;

namespace Konfidence.BaseDataInterfaces
{
    public interface IBaseDataItem
    {
        string AutoIdField { get; set; }

        Dictionary<string, IDbParameterObject> AutoUpdateFieldDictionary { get; }

        Guid GuidIdValue { get; }

        bool IsSelected { get; set; }

        bool IsEditing { get; set; }

        IBaseHost DataHost { get; set; }

        string LoadStoredProcedure { get; set; }

        string DeleteStoredProcedure { get; set; }

        string SaveStoredProcedure { get; set; }

        void InitializeDataItem();

        IDbParameterObjectList SetItemData();

        void Save();

        void Delete();

        void LoadDataItem();

        int GetId();

        void SetId(int id);

        void GetKey();

        void GetData();

        void SetProperties(Dictionary<string, object> propertyDictionary);

        void GetProperties(IDbParameterObjectList properties);

        IDbParameterObjectList GetParameterObjectList();
    }
}
