using System.Collections.Generic;
using System.Data;

namespace Konfidence.DataBaseInterface
{
    public interface IBaseDataItem
    {
        string AutoIdField { get; set; }

        Dictionary<string, ISpParameterData> AutoUpdateFieldDictionary { get; }

        string GetStoredProcedure { get; set; }

        string DeleteStoredProcedure { get; set; }

        string SaveStoredProcedure { get; set; }

        string GetByGuidStoredProcedure { get; set; }

        void InitializeDataItem();

        List<ISpParameterData> SetItemData();

        void Save();

        void Delete();

        void LoadDataItem();

        int GetId();

        void SetId(int id);

        void GetKey(IDataReader dataReader);

        void GetData(IDataReader dataReader);

        List<ISpParameterData> GetParameterObjects();
    }
}
