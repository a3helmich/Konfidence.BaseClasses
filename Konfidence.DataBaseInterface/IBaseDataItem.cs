using System.Collections.Generic;
using System.Data;

namespace Konfidence.DatabaseInterface
{
    public interface IBaseDataItem
    {
        string GuidIdField { get; set; }

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

        int GetId();

        void SetId(int id);

        void GetKey(IDataReader dataReader);

        void GetData(IDataReader dataReader);

        List<ISpParameterData> GetParameterObjects();

        bool IsNew { get; }
    }
}
