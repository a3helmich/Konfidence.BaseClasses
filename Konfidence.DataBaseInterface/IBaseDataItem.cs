using System;
using System.Collections.Generic;
using System.Data;

namespace Konfidence.DataBaseInterface
{
    public interface IBaseDataItem
    {
        string AutoIdField { get; set; }

        Dictionary<string, ISpParameterData> AutoUpdateFieldDictionary { get; }

        Guid GuidIdValue { get; }

        bool IsSelected { get; set; }

        bool IsEditing { get; set; }

        IBaseClient Client { get; set; }

        string GetStoredProcedure { get; set; }

        string DeleteStoredProcedure { get; set; }

        string SaveStoredProcedure { get; set; }

        void InitializeDataItem();

        List<ISpParameterData> SetItemData();

        void Save();

        void Delete();

        void LoadDataItem();

        int GetId();

        void SetId(int id);

        void GetKey(IDataReader dataReader);

        void GetData(IDataReader dataReader);

        //void SetProperties(Dictionary<string, object> propertyDictionary);

        //void GetProperties(List<IDbParameterObject> properties);

        List<ISpParameterData> GetParameterObjects();
    }
}
