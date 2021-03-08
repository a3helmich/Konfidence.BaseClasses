using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.BaseData.Sp;
using Konfidence.DatabaseInterface;

namespace Konfidence.BaseData
{
    public abstract class BaseDataItem : IBaseDataItem
    {
        private readonly bool _isInitialized;

        internal List<ISpParameterData> SpParameterData { get; }

        public string GuidIdField { get; set; } = string.Empty;

        public string AutoIdField { get; set; } = string.Empty;

        private int _id;

        public Dictionary<string, ISpParameterData> AutoUpdateFieldDictionary { get; }

        public void SetId(int id) => _id = id;

        public int GetId() => _id;

        protected IBaseClient Client { get; set; }

        public List<ISpParameterData> GetParameterObjects() => SpParameterData;

        protected BaseDataItem()
        {
            _isInitialized = false;

            SpParameterData = new List<ISpParameterData>();
            AutoUpdateFieldDictionary = new Dictionary<string, ISpParameterData>();

            InternalInitializeDataItem();

            _isInitialized = true;
        }

        public void GetKey(IDataReader dataReader)
        {
            if (AutoIdField.Length <= 0)
            {
                return;
            }

            dataReader.GetField(AutoIdField, out int id);

            _id = id;
        }

        [CanBeNull]
        internal object GetAutoUpdateField([NotNull] string fieldName)
        {
            if (AutoUpdateFieldDictionary.TryGetValue(fieldName, out var parameterData) && parameterData.IsAssigned())
            {
                return parameterData.Value;
            }

            return null;
        }

        [UsedImplicitly]
        protected internal void AddAutoUpdateField([NotNull] string fieldName, DbType fieldType)
        {
            if (!AutoUpdateFieldDictionary.ContainsKey(fieldName))
            {
                AutoUpdateFieldDictionary.Add(fieldName, new SpParameter(fieldName, fieldType, null));
            }
        }

        public string GetStoredProcedure { get; set; } = string.Empty;

        public string DeleteStoredProcedure { get; set; } = string.Empty;

        public string SaveStoredProcedure { get; set; } = string.Empty;

        public string GetByGuidStoredProcedure { get; set; } = string.Empty;

        [UsedImplicitly]
        public bool IsNew => _id == 0;

        public virtual void InitializeDataItem()
        {
        }

        private void InternalInitializeDataItem()
        {
            if (!_isInitialized)
            {
                InitializeDataItem();
            }
        }

        protected void GetItem()
        {
            Debug.WriteLine($"Client.GetItem(this) : this={GetType().FullName}");

            Client.GetItem(this);
        }

        protected void GetItemBy([NotNull] string storedProcedure)
        {
            Debug.WriteLine($"Client.GetItemBy(this, {storedProcedure}) : this={GetType().FullName}");

            Client.GetItemBy(this, storedProcedure);
        }

        protected void GetItem(int id)
        {
            this.SetField(AutoIdField, id);

            GetItem();
        }

        [UsedImplicitly]
        protected void GetItem(Guid guidId)
        {
            this.SetField(GuidIdField, guidId);

            GetItemBy(GetByGuidStoredProcedure);
        }

        public void Save()
        {
            if (!IsValidDataItem())
            {
                return;
            }

            Debug.WriteLine($"Client.Save(this) : this={GetType().FullName}");

            Client.Save(this);

            GetAutoUpdateData();
        }

        public void Delete()
        {
            Debug.WriteLine($"Client.Delete(this) : this={GetType().FullName}");

            Client.Delete(this);

            _id = 0;
        }

        [NotNull]
        public List<ISpParameterData> SetItemData()
        {
            SetData();

            return SetParameterData();
        }

        [NotNull]
        private List<ISpParameterData> SetParameterData()
        {
            var parameterObjectList = SpParameterData.ToList();

            SpParameterData.Clear();

            return parameterObjectList;
        }

        protected virtual void GetAutoUpdateData()
        {
        }

        public virtual void GetData(IDataReader dataReader)
        {
        }

        protected virtual void SetData()
        {
        }

        protected virtual bool IsValidDataItem()
        {
            return true;
        }
    }
}
